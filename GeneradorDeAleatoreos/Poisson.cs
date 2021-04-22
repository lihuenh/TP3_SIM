using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorDeAleatoreos
{
    public class Poisson
    {
        public double lambda;

        public Poisson(double lambda)
        {
            this.lambda = lambda;
        }

        public void generarNumeros(double[] numerosAleatorios)
        {
            Random random = new Random();
            double p;
            int x;
            double A = Math.Exp(-lambda);

            for (int i = 0; i < numerosAleatorios.Length; i++)
            {
                p = 1;
                x = -1;
                do
                {
                    double rnd = random.NextDouble();
                    p *= rnd;
                    x += 1;
                } while (p >= A);
                numerosAleatorios[i] = x;
            }

        }
    }
}
