using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Esempio1
{
    class Controller
    {
        private Persistance _persistance;

        public Controller()
        {
            _persistance = new Persistance();
        }

        public void ReadDB()
        {
            _persistance.ReadDB();
        }
    }
}
