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
using System.Configuration;

namespace Esempio1
{
    public partial class View : Form
    {
        private TextBoxTraceListener _textBoxListener;
        private Controller _controller;
        private String pythonScriptsPath;
        private String pythonPath;
        private String dbOrdiniPath;
        private String customerID;

        public View()
        {
            InitializeComponent();
            _textBoxListener = new TextBoxTraceListener(txtConsole);
            Trace.Listeners.Add(_textBoxListener);

            this.dbOrdiniPath = ConfigurationManager.AppSettings["dbOrdiniFile"];
            this.pythonPath = ConfigurationManager.AppSettings["pythonPath"];
            this.pythonScriptsPath = ConfigurationManager.AppSettings["pyScripts"];

            _controller = new Controller(this.dbOrdiniPath, this.pythonPath, this.pythonScriptsPath);
        }

        private void readDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SetDBFile();
        }

        private void toolStripButton_SetDB_Click(object sender, EventArgs e)
        {
            SetDBFile();
            //ReadClientAsync();
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.customerID = txtCustomer.Text;
            if (this.customerID != null || this.customerID != "")
            {
                CheckIfCustomerExist();
            }
        }

        private void CheckIfCustomerExist()
        {
            _controller.CustomerExist(this.dbOrdiniPath, this.customerID);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            _controller.Delete(txtCustomer.Text);
        }

        /*
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            _controller.Update(txtCustomer.Text, txtNewCustomer.Text);
        }
        */

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            _controller.Create(txtCustomer.Text);
        }

        private void SetDBFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.dbOrdiniPath = ofd.FileName;
                txtConsole.AppendText("Sqlite file name: " + dbOrdiniPath + Environment.NewLine);
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButton_CustomersOrdersChart_Click(object sender, EventArgs e)
        {
            //_controller.LoadDB(txtCustomer.Text);
            ReadCustomersOrdersChartAsync();
        }

        private void toolStripButton_ARIMA_Click(object sender, EventArgs e)
        {
            ArimaAsync();
        }

        private void toolStripButton_Optimize_Click(object sender, EventArgs e)
        {
            OptimizeAsync();
        }

        // Legge i dati per costruire un grafico dell'andamento degli ordini dei 12 utenti selezionati (random)
        private async void ReadCustomersOrdersChartAsync()
        {
            _controller.ReadCustomers(this.dbOrdiniPath);
            Trace.WriteLine(this.dbOrdiniPath);
            Bitmap bmp = await _controller.BitmapOfCustomersOrdersChart(this.dbOrdiniPath, "chartOrders.py"); //facciamo partire un task asincrono
            pictureBox1.Image = bmp; //la bitmap ritornata viene messa nel campo Image della pictureBox della View
        }

        private async void ArimaAsync()
        {
            _controller.ReadCustomers(this.dbOrdiniPath, 1);
            Trace.WriteLine(this.dbOrdiniPath);
            string str = await _controller.ForecastSpecificCustomerOrderChart(this.dbOrdiniPath, "arima_forecast.py");
            Trace.WriteLine("Forecast Prevision = " + str);
        }

        private async void OptimizeAsync()
        {
            List<String> customers = _controller.ReadAllCustomers(this.dbOrdiniPath);
            _controller.OptimizeGAP(this.dbOrdiniPath, "arima_forecast.py", customers);
         }
    }
}
