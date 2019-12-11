// ************************************************************************************************
// Adapted from https://www.codeproject.com/Articles/5165602/Using-Python-Scripts-from-a-Csharp-Client-Includin
// Using Python Scripts from a C# Client (Including Plots and Images)
// ************************************************************************************************

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyGAP2019
{
	/// <summary>
	/// A specialized runner for python scripts. Supports textual output
	/// as well as image output, both synchronously and asynchronously.
	/// </summary>
	public class PythonRunner
	{
		// strings collecting output/error messages
		private StringBuilder _outputBuilder;
		private StringBuilder _errorBuilder;

		// The Python interpreter ('python.exe') that is used by this instance.
		public string Interpreter { get; }

		// The timeout for the underlying component in msec.
		public int Timeout { get; set; }

		// <param name="interpreter"> Full path to the Python interpreter ('python.exe').
		// <param name="timeout"> The script timeout in msec. Defaults to 10000 (10 sec).
		public PythonRunner(string interpreter, int timeout = 10000)
		{
			if (interpreter == null)
			{	throw new ArgumentNullException(nameof(interpreter));
			}

			if (!File.Exists(interpreter))
			{	throw new FileNotFoundException(interpreter);
			}

			Interpreter = interpreter;
			Timeout = timeout;
		}

		#region task spawners

		// Runs getStrings asynchronously, returns an awaitable task, with the text output of the script
		public Task<string> getStringsAsync(string pythonScriptsPath, string script, params object[] arguments)
		{
			var task = new Task<string>(() => getStrings(pythonScriptsPath, script, arguments));
			task.Start();
			return task;
		}

      /// <summary>
      /// Executes a Python script and returns the text that it prints to the console.
      /// </summary>
      /// <param name="script">Full path to the script to execute.</param>
      /// <param name="arguments">Arguments that were passed to the script.</param>
      /// <returns>The text output of the script.</returns>
      /// <exception cref="PythonRunnerException">
      /// Thrown if error text was outputted by the script (this normally happens
      /// if an exception was raised by the script). <br/>
      /// -- or -- <br/>
      /// An unexpected error happened during script execution. In this case, the
      /// <see cref="Exception.InnerException"/> property contains the original
      /// <see cref="Exception"/>.
      /// </exception>
      /// <exception cref="ArgumentNullException">
      /// Argument <paramref name="script"/> is null.
      /// </exception>
      /// <exception cref="FileNotFoundException">
      /// Argument <paramref name="script"/> is an invalid path.
      /// </exception>
      /// <remarks>
      /// Output to the error stream can also come from warnings, that are frequently
      /// outputted by various python package components. These warnings would result
      /// in an exception, therefore they must be switched off within the script by
      /// including the following statement: <c>warnings.simplefilter("ignore")</c>.
      /// </remarks>
      public string getStrings(string pythonScriptsPath, string script, params object[] arguments)
      {
         runScript(pythonScriptsPath, script, arguments);

         string errorMessage = _errorBuilder.ToString();

         return !string.IsNullOrWhiteSpace(errorMessage)
            ? throw new Exception(errorMessage)
            : _outputBuilder.ToString().Trim();

        
      }

		// Runs getImage asynchronously, returns an awaitable task, with the Bitmap that the script creates
		public Task<Bitmap> getImageAsync(string pythonScriptsPath, string script, params object[] arguments)
		{
			var task = new Task<Bitmap>(() => getImage(pythonScriptsPath, script, arguments));
			task.Start();
			return task;
		}

      /// <summary>
      /// Executes a Python script and returns the resulting image (mostly a chart that was produced
      /// by a Python package like e.g. <see href="https://matplotlib.org/">matplotlib</see> or
      /// <see href="https://seaborn.pydata.org/">seaborn</see>).
      /// </summary>
      /// <param name="script">Full path to the script to execute.</param>
      /// <param name="arguments">Arguments that were passed to the script.</param>
      /// <returns>The <see cref="Bitmap"/> that the script creates.</returns>
      /// <exception cref="PythonRunnerException">
      /// Thrown if error text was outputted by the script (this normally happens
      /// if an exception was raised by the script). <br/>
      /// -- or -- <br/>
      /// An unexpected error happened during script execution. In this case, the
      /// <see cref="Exception.InnerException"/> property contains the original
      /// <see cref="Exception"/>.
      /// </exception>
      /// <exception cref="ArgumentNullException">
      /// Argument <paramref name="script"/> is null.
      /// </exception>
      /// <exception cref="FileNotFoundException">
      /// Argument <paramref name="script"/> is an invalid path.
      /// </exception>
      /// <remarks>
      /// <para>
      /// In a 'normal' case, a Python script that creates a chart would show this chart
      /// with the help of Python's own backend, like this.
      /// <example>
      /// import matplotlib.pyplot as plt
      /// ...
      /// plt.show()
      /// </example>
      /// For the script to be used within the context of this <see cref="PythonRunner"/>,
      /// it should instead convert the image to a base64-encoded string and print this string
      /// to the console. The following code snippet shows a Python method (<c>print_figure</c>)
      /// that does this:
      /// <example>
      /// import io, sys, base64
      /// 
      /// def print_figure(fig):
      /// 	buf = io.BytesIO()
      /// 	fig.savefig(buf, format='png')
      /// 	print(base64.b64encode(buf.getbuffer()))
      ///
      /// import matplotlib.pyplot as plt
      /// ...
      /// print_figure(plt.gcf()) # the gcf() method retrieves the current figure
      /// </example>
      /// </para><para>
      /// Output to the error stream can also come from warnings, that are frequently
      /// outputted by various python package components. These warnings would result
      /// in an exception, therefore they must be switched off within the script by
      /// including the following statement: <c>warnings.simplefilter("ignore")</c>.
      /// </para>
      /// </remarks>
      public Bitmap getImage(string pythonScriptsPath, string script, params object[] arguments)
      {
         runScript(pythonScriptsPath, script, arguments);

         string errorMessage = _errorBuilder.ToString();

         if (!string.IsNullOrWhiteSpace(errorMessage))
            throw new Exception(errorMessage);

         string strBitmap = _outputBuilder.ToString().Trim();
         strBitmap = strBitmap.Substring(strBitmap.IndexOf("b'"));
         strBitmap = strBitmap.Remove(strBitmap.Length-4).Trim();

         try
         {  return FromPythonBase64String(strBitmap); //convertiamo la stringa in una stringa binaria compatibile con l'oggetto BitMap
         }
         catch (Exception exception)
         {
            throw new Exception(
               "An error occured while trying to create an image from Python script output. " +
               "See inner exception for details.",
               exception);
         }
      }
      #endregion // task spawners

      #region python script handlers

      // Formats the arguments for the python script
      internal static string BuildArgumentString(string scriptPath, object[] arguments)
		{
			var stringBuilder = new StringBuilder("\"" + scriptPath.Trim() + "\"");

			if (arguments != null)
				foreach (object argument in arguments.Where(arg => arg != null))
					stringBuilder.Append(" \"" + argument.ToString().Trim() + "\"");

			return stringBuilder.ToString();
		}

      // to run a batch file
      private void runBatFile(string strCommand)
      {
         // first generate the batch, then run it
         StreamWriter fbat = new StreamWriter("runPyScript.bat");
         //fbat.WriteLine(@"echo off");
         fbat.WriteLine($"conda activate ssd && call python {strCommand} && conda deactivate");
         fbat.WriteLine("exit");
         fbat.Close();

         ProcessStartInfo start = new ProcessStartInfo();
         start.FileName = "cmd.exe";
         start.Arguments = "/K runPyScript.bat";
         start.UseShellExecute = false;
         start.RedirectStandardOutput = true;
         start.RedirectStandardError = true;
         start.WorkingDirectory = ".";
         start.CreateNoWindow = true;

         string stdout, stderr;
         using (Process process = Process.Start(start))
         {
            using (StreamReader reader = process.StandardOutput)
            {
               stdout = reader.ReadToEnd();
            }

            using (StreamReader reader = process.StandardError)
            {
               stderr = reader.ReadToEnd();
            }

            process.WaitForExit();
            Console.WriteLine("ExitCode: {0}", process.ExitCode);
         }
      }

      // to run a sequence of dos commands, not read from a file
      private void runDosCommands(string strCommand)
      { 
         // Set working directory and create process
         //var workingDirectory = Path.GetFullPath(pythonScriptsPath);
         var process = new Process
         {
            StartInfo = new ProcessStartInfo
            {
               FileName = "cmd.exe",
               //WorkingDirectory = workingDirectory,
               UseShellExecute = false,
               RedirectStandardInput = true,
               RedirectStandardOutput = true,
               RedirectStandardError = true,
               CreateNoWindow = true
            }
         };
         process.Start();

         // Pass multiple commands to cmd.exe
         using (var sw = process.StandardInput)
         {
             if (sw.BaseStream.CanWrite)
             {
                 sw.WriteLine("echo off");
                 sw.WriteLine("conda activate ssd");
                 sw.WriteLine($"call python {strCommand}");
                 sw.WriteLine("exit");
            }
         }

         // read output lines
         while (!process.StandardOutput.EndOfStream)
         {
            var line = process.StandardOutput.ReadLine();
            _outputBuilder.AppendLine(line);
            Console.WriteLine(line);
         }
      }

      // runs the script without cmd support
      private void runMemoryScript(string strCommand)
      {
         // dati in input per il processo
			var startInfo = new ProcessStartInfo(Interpreter)
			{
				UseShellExecute   = false,
				CreateNoWindow    = true,
				RedirectStandardOutput = true,
				RedirectStandardError  = true,
				Arguments         = strCommand
			};

         // attivazione del processo
			using (var process = new Process{ StartInfo = startInfo, EnableRaisingEvents = true })
			{
				try
				{
					process.ErrorDataReceived  += OnProcessErrorDataReceived;
					process.OutputDataReceived += OnProcessOutputDataReceived;

					// Start the process.
					process.Start();
					OnStarted(process.StartTime); // just writes the starting time

					process.BeginOutputReadLine();
					process.BeginErrorReadLine();

					if (!process.WaitForExit(Timeout))
					{	throw new TimeoutException($"Timeout of {Timeout} msec expired.");
					}

					OnExited(process.ExitCode, process.ExitTime); // just writes the completion time
            }
				catch (Exception exception)
				{
					throw new Exception(
						"Error during script execution. See inner exception for details.",
						exception);
				}
			}
      }

      // Runs a python script
      private void runScript(string pythonScriptsPath, string script, object[] arguments)
		{  string strCommand;

			if (script == null)
				throw new ArgumentNullException(nameof(script));

			if (!File.Exists(pythonScriptsPath+"\\"+script))
				throw new FileNotFoundException(script);

			_outputBuilder = new StringBuilder();
			_errorBuilder  = new StringBuilder();

         strCommand = BuildArgumentString(pythonScriptsPath + "\\" + script, arguments);

         string mode = "dos";

         switch (mode)
         {
            case "bat":
               runBatFile(strCommand);
               break;
            case "dos":
               runDosCommands(strCommand);
               break;
            case "mem":
               runMemoryScript(strCommand);
               break;
            default:
					throw new Exception("Error in python script execution mode");
         }
		}

      // Converts a base64 string (as printed by python script) to a bitmap image.
      public Bitmap FromPythonBase64String(string pythonBase64String)
      {
         // Remove the first two chars and the last one.
         // First one is 'b' (python format sign), others are quote signs.
         string base64String = pythonBase64String.Substring(2, pythonBase64String.Length - 3);

         // Convert now raw base46 string to byte array.
         byte[] imageBytes = Convert.FromBase64String(base64String);

         // Read bytes as stream.
         var memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
         memoryStream.Write(imageBytes, 0, imageBytes.Length);

         // Create bitmap from stream.
         return (Bitmap)Image.FromStream(memoryStream, true);
      }

      // Collect output on the go. Otherwise, the internal buffer may run full and script execution
      // may be hanging. This can especially happen when printing images as base64 strings (which
      // produces a very large amount of ASCII text).
      private void OnProcessOutputDataReceived(object sender, DataReceivedEventArgs e) => 
			_outputBuilder.AppendLine(e.Data);

		// Collect error output on the go.
		private void OnProcessErrorDataReceived(object sender, DataReceivedEventArgs e) => 
			_errorBuilder.AppendLine(e.Data);

		// Raise the Exited event
		private void OnExited(int exitCode, DateTime exitTime) =>
         Trace.WriteLine($"Exiting process, exitcode = {exitCode} t = {exitTime}");

      // Raise the Started event
      private void OnStarted(DateTime startTime) =>
         Trace.WriteLine($"Starting process, t = {startTime}");

      #endregion // python script handlers
   }
}