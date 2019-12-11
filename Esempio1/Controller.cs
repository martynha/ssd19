using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;
using System.Drawing;
using PyGAP2019;
using System.IO;
using System.Globalization;

namespace Esempio1
{
    class Controller
    {
        private Persistance _persistance;
        //private string connectionString;
        private string strCustomers;

        private string dbPath;
        private string pythonPath;
        private string pythonScriptsPath;
        private PythonRunner pyRunnes;
        private GAPclass GAP;

        public Controller(string dbPath, string _pyPath, string _pyScriptsPath)
        {
            _persistance = new Persistance();
            this.dbPath = dbPath;

            this.pythonPath = _pyPath;
            this.pythonScriptsPath = _pyScriptsPath;

            this.pyRunnes = new PythonRunner(this.pythonPath);
            
            /*
            //string dbpath = @"C:\Users\Enrico\Desktop\ordiniMI2018.sqlite";
            string sdb = ConfigurationManager.AppSettings["dbServer"];

            switch (sdb)
            {
                case "SQLiteConn":
                    connectionString = ConfigurationManager.ConnectionStrings["SQLiteConn"].ConnectionString;
                    connectionString = connectionString.Replace("DBFILE", dbPath);
                    _persistance.SetFactory(ConfigurationManager.ConnectionStrings["SQLiteConn"].ProviderName);
                    break;
                case "LocalDbConn":
                    connectionString = ConfigurationManager.ConnectionStrings["LocalSqlServConn"].ConnectionString;
                    _persistance.SetFactory(ConfigurationManager.ConnectionStrings["LocalSqlServConn"].ProviderName);
                    break;
                case "RemoteSqlServConn":
                    connectionString = ConfigurationManager.ConnectionStrings["RemoteSQLConn"].ConnectionString;
                    _persistance.SetFactory(ConfigurationManager.ConnectionStrings["RemoteSQLConn"].ProviderName);
                    break;

            }
            _persistance.connectionString = connectionString;
            */
        }

        public string ReadCustomers(string dbPath, int numSerie=12)
        {
            //int numSerie = 12; //Numero di clienti di cui leggere la serie temporale
            this.strCustomers = _persistance.ReadRandomCustomerListORM(dbPath, numSerie);
            Trace.WriteLine($"Cliente: {this.strCustomers}");
            return this.strCustomers;
        }

        public List<string> ReadAllCustomers(string dbPath)
        {
            return _persistance.ReadAllCustomers(dbPath);
        }


        public Boolean CustomerExist(string dbPath, string customerID)
        {
            return _persistance.CustomerExist(dbPath, customerID);
        }

        public void LoadDB(string customerID)
        {
            /*
            if (customerID != "")
                _persistance.LoadByCustomer(customerID);
            else
                _persistance.Load();
            */

            //_persistance.readCustomerListORM(dbPath, 12);
            _persistance.ReadOrderByCustomer(dbPath, customerID);
        }

        public void Create(string customerID)
        {
            if (customerID != "")
                _persistance.Create(customerID);
        }

        public void Delete(string customerID)
        {
            if (customerID != "")
                _persistance.Delete(customerID);
        }

        public void Update(string customerID, string newCustomerID)
        {
            if (customerID != "" && newCustomerID != "")
                _persistance.Update(customerID, newCustomerID);
        }

        // Lettura e previsione di una serie di ordini
        public async Task<Bitmap> BitmapOfCustomersOrdersChart(string dbPath, string pythonFile)
        {
            Trace.WriteLine("Getting the orders chart ...");
            //pythonScriptsPath = System.IO.Path.GetFullPath(pythonScriptsPath); //trasforma i path relativi in path assoluti
            Bitmap bmp;
            try
            {
                // Funzione asincrona, prende un'immagine. Infatti quello che restituisce è una bitmap.
                // L'esecuzione di questo sarà infatti una Bitmap
                bmp = await pyRunnes.getImageAsync(
                    this.pythonScriptsPath,
                    pythonFile, 
                    this.pythonScriptsPath,
                    dbPath,
                    this.strCustomers);
                return bmp;
            } catch (Exception e)
            {
                Trace.WriteLine($"[BitmapOfCustomersOrdersChart]: {e.ToString()}");
                return null;
            }
        }

        public async Task<string> ForecastSpecificCustomerOrderChart(string dbPath, string pythonFile)
        {
            Trace.WriteLine("Getting the orders chart ...");
            //pythonScriptsPath = System.IO.Path.GetFullPath(pythonScriptsPath); //trasforma i path relativi in path assoluti
            string fcast = "";
            try
            {
                // Funzione asincrona, restituisce una stringa.
                string str = await pyRunnes.getStringsAsync(
                    this.pythonScriptsPath,
                    pythonFile,
                    this.pythonScriptsPath,
                    dbPath,
                    this.strCustomers);

                string[] lines = str.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in lines) {
                    if (s.StartsWith("Actual"))
                    {
                        fcast = s.Substring(s.LastIndexOf(" "));
                        //Trace.WriteLine(fcast);
                    }
                }
                return fcast;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"[ForecastSpecificCustomerOrderChart]: {e.ToString()}");
                return null;
            }
        }

        public async Task<int[]> ForecastAllCustomerOrderChart(string dbPath, string pythonFile, List<String> customers)
        {
            Trace.WriteLine("Getting the orders chart ...");
            double fcast = double.NaN;
            List<int> res = new List<int>(); 
            foreach (string customer in customers)
            {
                Trace.WriteLine(customer);

                try
                {
                    // Funzione asincrona, restituisce una stringa.
                    string str = await pyRunnes.getStringsAsync(
                        this.pythonScriptsPath,
                        pythonFile,
                        this.pythonScriptsPath,
                        dbPath,
                        customer);

                    NumberFormatInfo provider = new NumberFormatInfo();
                    provider.NumberDecimalSeparator = ".";

                    fcast = str.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                    .Where(s => s.StartsWith("Actual "))
                    .Select(s => s.Substring(s.LastIndexOf(' ')))
                    .Select(s => s.Trim())
                    .Select(s => Convert.ToDouble(s, provider))
                    .Last();

                    /*
                    str.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in lines)
                    {
                        if (s.StartsWith("Actual "))
                        {
                            Trace.WriteLine(s.Substring(s.LastIndexOf(" ") + 1));
                            fcast = Convert.ToDouble(s.Substring(s.LastIndexOf(" ") + 1));
                        }
                    }*/

                    Trace.WriteLine(Math.Round(fcast));
                    res.Add((int) Math.Round(fcast));
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"[ForecastAllCustomerOrderChart]: {e.ToString()}");
                }

            }
            return res.ToArray();
        }

        // Ricerca locale di istanze GAP
        public async void OptimizeGAP(string dbPath, string pythonFile, List<String> customers)
        {
            GAP = _persistance.ReadGAPinstance(dbPath);

            if (File.Exists("GAPreq.dat"))
            {
                string[] txtData = File.ReadAllLines("GAPreq.dat");
                GAP.req = Array.ConvertAll<string, int>(txtData, new Converter<string, int>(i => int.Parse(i)));
            }
            else
            {
                GAP.req = await ForecastAllCustomerOrderChart(dbPath, pythonFile, customers);
                File.WriteAllLines("GAPreq.dat", GAP.req.Select(x => x.ToString()));
            }

            /*
            double zub = GAP.SimpleContruct();
            Trace.WriteLine($"Constructive, zub = {zub}");
            zub = GAP.Opt10(GAP.c);
            Trace.WriteLine($"Local search, zub = {zub}");
            */
        }


    }
}
