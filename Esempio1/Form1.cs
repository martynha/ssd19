using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Esempio1
{
    public partial class Form1 : Form
    {
        private TextBoxTraceListener _textBoxListener;
        private Controller _controller;

        public Form1()
        {
            InitializeComponent();
            _textBoxListener = new TextBoxTraceListener(txtConsole);
            Trace.Listeners.Add(_textBoxListener);
            _controller = new Controller();
        }

        private void readDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadDB();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReadDB();
        }

        private void ReadDB()
        {
            _controller.ReadDB();
        }

        public class TextBoxTraceListener : TraceListener
        {
            private TextBox _target;
            private StringSendDelegate _invokeWrite;

            public TextBoxTraceListener(TextBox target)
            {
                _target = target;
                _invokeWrite = new StringSendDelegate(SendString);
            }

            public override void Write(string message)
            {
                _target.Invoke(_invokeWrite, new object[] { message });
            }

            public override void WriteLine(string message)
            {
                _target.Invoke(_invokeWrite, new object[] { message + Environment.NewLine });
            }

            private delegate void StringSendDelegate(string message);

            private void SendString(string message)
            {
                _target.AppendText(message);
            }
        }
    }
}
