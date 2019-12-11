using DSS19;
using PyGAP2019;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;

/*
string dbpath = @"C:\Users\Marti\Desktop\Magistrale\3Anno\SistemidiSupportoalleDecisioni\ordiniMI2018.sqlite";
string sqLiteConnString = @"Data Source=" + dbpath + "; Version=3";
IDbConnection conn = new SQLiteConnection(sqLiteConnString);
*/

//1° query: SELECT DISTINCT CUSTOMER FROM ORDINI
//2° query: SELECT DISTINCT CUSTOMER FROM ORDINI ORDER BY random() LIMIT 20

namespace Esempio1
{
    // Model
    class Persistance
    {
        public string connectionString = "";
        private IDbConnection conn = null;
        private string factory = "";

        public void Load()
        {
            Trace.WriteLine("[PERSISTENCE] Inizio lettura dati...");
            List<int> lstQuant = new List<int>();

            try
            {
                string queryText;
                if (factory == "System.Data.SQLite")
                {
                    queryText = "select id, customer, time, quant from ordini LIMIT 100"; //SQLite
                }
                else
                {
                    queryText = "select TOP (100) id, customer, time, quant form ordini"; //Altri db
                }

                using (IDataReader reader = ExecuteQuery(queryText))
                {
                    while (reader.Read())
                    {
                        Trace.WriteLine(reader["id"] + " " + reader["customer"] + " " + reader["time"] + " " + reader["quant"]); //view.textConsole = ...
                        lstQuant.Add(Convert.ToInt32(reader["quant"]));
                    }
                    Trace.WriteLine('[' + String.Join(", ", lstQuant) + ']');
                }
            }
            catch (Exception ex)
            {
                ErrorLog("[Load] " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            Trace.WriteLine("[PERSISTANCE] fine lettura dati ");
        }

        public void LoadByCustomer(string customerID)
        {
            List<int> quantLst = new List<int>();
            try
            {
                string queryText = "SELECT id, customer, time, quant from ordini where customer = \'" + customerID + "\'";
                using (IDataReader reader = ExecuteQuery(queryText))
                {
                    while (reader.Read())
                    {
                        Trace.WriteLine(reader["id"] + " " + reader["customer"] + " " + reader["time"] + " " + reader["quant"]); //view.textConsole = ...
                        quantLst.Add(Convert.ToInt32(reader["quant"]));
                    }
                    Trace.WriteLine("Quantità: " + string.Join(",", quantLst));
                }
            }
            catch (Exception ex)
            {
                ErrorLog("[LoadByCustomer] " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            Trace.WriteLine("[PERSISTANCE] fine lettura dati ");
        }

        public void Create(string customerID)
        {
            try
            {
                string queryText = @"INSERT INTO ordini (customer) VALUES ('" + customerID + "') ";
                using (IDataReader reader = ExecuteQuery(queryText))
                {
                    LoadByCustomer(customerID); //to verify the insert
                }
            }
            catch (Exception ex)
            {
                ErrorLog("[Create] " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            Trace.WriteLine("[PERSISTANCE] create customer done");
        }

        public void Delete(string customerID)
        {
            try
            {
                string queryText = @"DELETE FROM ordini WHERE customer = '" + customerID + "'";
                using (IDataReader reader = ExecuteQuery(queryText))
                {
                    LoadByCustomer(customerID); //to verify the delete
                }
            }
            catch (Exception ex)
            {
                ErrorLog("[Delete] " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            Trace.WriteLine("[PERSISTANCE] delete done");
        }

        public void Update(string oldCustomerID, string newCustomerID)
        {
            try
            {
                string queryText = @"UPDATE ordini SET customer = '" + newCustomerID + "'  WHERE customer = '" + oldCustomerID + "'";
                using (IDataReader reader = ExecuteQuery(queryText))
                {
                    LoadByCustomer(newCustomerID); //to verify the insert
                }
            }
            catch (Exception ex)
            {
                ErrorLog("[Update] " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            Trace.WriteLine("[PERSISTANCE] update done");
        }

        /*
                public void ReadDB()
                {
                    Trace.WriteLine("[PERSISTENCE] Inizio lettura dati...");

                    var conn = ConnectionProvider.GetConnection(Db.Orders, DbEngine.SQLite);
                    List<int> lstQuant = new List<int>();

                    try
                    {
                        conn.Open();

                        IDbCommand com = conn.CreateCommand();
                        string queryText = "select id, customer, time, quant from ordini LIMIT 100";
                        com.CommandText = queryText;

                        IDataReader reader = com.ExecuteReader();
                        try
                        {
                            while (reader.Read())
                            {
                                Trace.WriteLine(reader["id"] + " " + reader["customer"] + " " + reader["time"] + " " + reader["quant"]);
                                lstQuant.Add(Convert.ToInt32(reader["quant"]));
                            }
                            Trace.WriteLine('[' + String.Join(", ", lstQuant) + ']');
                        }
                        catch (System.Exception ex)
                        {
                            Trace.WriteLine("[PERSISTENCE] errore: " + ex.Message);
                        } finally
                        {
                            reader.Close();
                        }

                    } catch (System.Exception ex)
                    {
                        Trace.WriteLine("[PERSISTENCE] errore: " + ex.Message);
                    } finally
                    {
                        conn.Close();
                    }
                    Trace.WriteLine("[PERSISTENCE] Quantità: " + string.Join(",", lstQuant));

                    Trace.WriteLine("[PERSISTENCE] Fine lettura dati");

                }
        */


        public void SetFactory(string factory)
        {
            this.factory = factory;
        }

        private void ErrorLog(string errTxt)
        {
            Trace.WriteLine("[PERSISTANCE] errore: " + errTxt);
        }

        // legge una stringa con i codici clienti da graficare
        public string ReadCustomerListORM(string dbpath, int n)
        {
            List<string> lstClienti;
            string ret = "Error reading DB";
            try
            {
                //var ctx = new SQLiteDatabaseContext(dbpath);
                using (var ctx = new DSS19.SQLiteDatabaseContext(dbpath))
                {
                    lstClienti = ctx.Database.SqlQuery<string>("SELECT distinct customer from ordini").ToList();
                }

                // legge solo alcuni clienti (si poteva fare tutto nella query)
                List<string> lstOutStrings = new List<string>();

                Random r = new Random(550);
                while (lstOutStrings.Count < n)
                {
                    int randomIndex = r.Next(0, lstClienti.Count); //Choose a random object in the list
                    lstOutStrings.Add("'" + lstClienti[randomIndex] + "'"); //add it to the new, random list
                    lstClienti.RemoveAt(randomIndex); //remove to avoid duplicates
                }
                ret = string.Join(",", lstOutStrings);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error: {ex.Message}");
            }
            Trace.WriteLine(ret);
            return ret;
        }

        public string ReadRandomCustomerListORM (string dbpath, int n)
        {
            IList<string> lstClienti = new List<string>();
            string ret = "Error reading DB";
            try
            {
                //var ctx = new SQLiteDatabaseContext(dbpath);
                using (var ctx = new SQLiteDatabaseContext(dbpath))
                {
                    lstClienti = ctx.Database.SqlQuery<string>($"SELECT DISTINCT CUSTOMER FROM ORDINI ORDER BY random() LIMIT {n}").ToList();
                }

                // legge solo alcuni clienti (si poteva fare tutto nella query)
                IList<string> lstOutStrings = new List<string>();

                
                foreach (var client in lstClienti)
                {
                    lstOutStrings.Add("'" + client + "'"); //add it to the new, random list
                }
                ret = string.Join(",", lstOutStrings);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error: {ex.Message}");
            }
            Trace.WriteLine(ret);
            return ret;
        }

        public string ReadOrderByCustomer(string dbpath, string customerID)
        {
            List<int> lstClienti;
            string ret = "Error reading DB";
            try
            {
                //var ctx = new SQLiteDatabaseContext(dbpath);
                using (var ctx = new SQLiteDatabaseContext(dbpath))
                {
                    lstClienti = ctx.Database.SqlQuery<int>(@"SELECT sum(quant) from ordini where customer='" + customerID + "'").ToList();
                }

                ret = string.Join(",", lstClienti);

            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error: {ex.Message}");
            }

            Trace.WriteLine(ret);
            return ret;
        }

        public List<string> ReadAllCustomers(string dbPath)
        {
            List<string> lstClienti = new List<string>();
            List<string> ret = new List<string>();
            try
            {
                //var ctx = new SQLiteDatabaseContext(dbpath);
                using (var ctx = new SQLiteDatabaseContext(dbPath))
                {
                    lstClienti = ctx.Database.SqlQuery<string>(@"SELECT DISTINCT customer from ordini ORDER BY customer").ToList();
                }

                foreach (string s in lstClienti)
                {
                     ret.Add("'" + s + "'");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error: {ex.Message}");
            }

            Trace.WriteLine("[Persistance] customer count = " + ret.Count());
            return ret;
        }

        public bool CustomerExist(string dbpath, string customerID)
        {
            int count = 0;
            try
            {
                //var ctx = new SQLiteDatabaseContext(dbpath);
                using (var ctx = new SQLiteDatabaseContext(dbpath))
                {
                    count = ctx.Database.SqlQuery<int>(@"SELECT COUNT(*) from ordini where customer='" + customerID + "'").First();
                }

            } catch (Exception ex)
            {
               Trace.WriteLine($"Error: {ex.Message}");
            }

            return count > 0 ? true : false;
        }


        // Reads an instance from the db
        public GAPclass ReadGAPinstance(string dbpath)
        {
            int i, j;
            List<int> lstCap;
            List<double> lstCosts;
            GAPclass G = new GAPclass();

            try
            {
                using (var ctx = new SQLiteDatabaseContext(dbpath))
                {
                    lstCap = ctx.Database.SqlQuery<int>("SELECT cap from capacita").ToList();
                    G.m = lstCap.Count();
                    G.cap = new int[G.m];
                    for (i = 0; i < G.m; i++)
                        G.cap[i] = lstCap[i];

                    lstCosts = ctx.Database.SqlQuery<double>("SELECT cost from costi").ToList();
                    G.n = lstCosts.Count / G.m;
                    G.c = new double[G.m, G.n];
                    G.req = new int[G.n];
                    G.sol = new int[G.n];
                    G.solbest = new int[G.n];
                    G.zub = Double.MaxValue;
                    G.zlb = Double.MinValue;

                    for (i = 0; i < G.m; i++)
                        for (j = 0; j < G.n; j++)
                            G.c[i, j] = lstCosts[i * G.n + j];

                    for (j = 0; j < G.n; j++)
                        G.req[j] = -1;          // placeholder
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("[readGAPinstance] Error:" + ex.Message);
            }

            Trace.WriteLine("Fine lettura dati istanza GAP");
            return G;
        }

        private IDbConnection OpenConnection()
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(factory);
            conn = dbFactory.CreateConnection();
            try
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                Trace.WriteLine("[PERSISTANCE] Connessione DB aperta");
                return conn;
            }
            catch (Exception ex)
            {
                ErrorLog("[OpenConnection] " + ex.Message);
            }
            return null;
        }

        private IDataReader ExecuteQuery(string queryText)
        {

            conn = OpenConnection();
            IDbCommand com = conn.CreateCommand();
            try
            {
                com.CommandText = queryText;
                IDataReader reader = com.ExecuteReader();
                Trace.WriteLine("[PERSISTANCE] query done ");
                return reader;
            }
            catch (Exception ex)
            {
                ErrorLog("[ExecuteQuery] " + ex.Message);
            }
            return null;
        }
    }
}
