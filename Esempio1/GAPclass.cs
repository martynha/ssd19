using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PyGAP2019
{
   class GAPclass
   {
      public int n;  // numero clienti
      public int m;  // numero magazzini
      public double[,] c;  // costi assegnamento
      public int[] req;    // richieste clienti
      public int[] cap;    // capacità magazzini

      public int[] sol,solbest;    // per ogni cliente, il suo magazzino
      public double zub;           // zub = costo della miglior soluzione trovata
      public double zlb;           // zlb = lower bound

      const double EPS = 0.0001;
      System.Random rnd = new Random(550);

      public GAPclass()
      {  zub = double.MaxValue;
         zlb = double.MinValue;
      }

        public double SimpleContruct()
        {
            int i, ii, j;
            int[] capleft = new int[cap.Length], ind = new int[m];
            double[] dist = new double[m];
            Array.Copy(cap, capleft, cap.Length);
            zub = 0;

            for (j = 0; j < n; j++)
            {
                for (i = 0; i < m; i++)
                {
                    dist[i] = c[i, j];
                    ind[i] = i;
                }
                Array.Sort(dist, ind);
                ii = 0;
                while (ii < m)
                {
                    i = ind[ii];
                    if (capleft[i] >= req[j])
                    {
                        sol[j] = i;
                        capleft[i] -= req[j];
                        zub += c[i, j];
                        Trace.WriteLine($"[SimpleConstruct] Client {j} server {i}.");
                        break;
                    }
                    ii++;
                }
                if (ii == m)
                {
                    Trace.WriteLine("[SimpleConstruct] Ahi Ahi.");
                }
            }

            return zub;
        }

        public double Opt10(double[,] c)
        {
            int[] capres = new int[cap.Length];
            double z = 0.0;
            
            // Calcolo la capacità residua rispetto alla soluzione.
            // Per tutti i clienti vedo a chi sono stati assegnati e tolgo dalla capacità residua del magazzino la capacità del cliente assegnatogli.
            // Dopodiché provo ad assegnare nuovamente e vedo se la nuova soluzione è migliore rispetto a quella corrente.
            Array.Copy(cap, capres, cap.Length);
            for (var j = 0; j < n; j++)
            {
                capres[sol[j]] -= req[j];
                z += c[sol[j], j];
            }

            l0: for (int j = 0; j < n; j++)
            {
                int isol = sol[j];
                for (int i = 0; i < m; i++)
                {
                    if (i == isol) continue;
                    if (c[i, j] < c[isol, j] && capres[i] >= req[j])
                    {
                        sol[j] = i;
                        capres[i] -= req[j];
                        capres[isol] += req[j];
                        z -= (c[isol, j] - c[i, j]);
                        if (z < zub) zub = z;
                        goto l0;
                    }
                }
            }
            return zub;
        }
   }
}