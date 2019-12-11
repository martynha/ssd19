using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace Esempio1
{
    enum DbEngine
    {
        SQLite
    }

    enum Db
    {
        Orders
    }

    class ConnectionProvider
    {
        static public IDbConnection GetConnection(Db database, DbEngine engine)
        {
            var cs = _connectionStrings[database][engine];
            return new SQLiteConnection(cs);
        }

        private static Dictionary<Db, Dictionary<DbEngine, string>> _connectionStrings = new Dictionary<Db, Dictionary<DbEngine, string>>()
        {
            [Db.Orders] = new Dictionary<DbEngine, string>() { [DbEngine.SQLite] = "Data Source=" + ProjectDir + "\\res\\ordiniMI2018.sqlite; Version=3;" }
        };

        private static string ProjectDir
        {
            get
            {
                return Directory.GetParent(Environment.CurrentDirectory).FullName;
            }
        }
    }
}
