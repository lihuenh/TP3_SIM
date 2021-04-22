using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorDeAleatoreos
{
    public class Uniforme
    {
        private double a, b;

        public Uniforme(double a, double b)
        {
            this.b = b;
            this.a = a;
        }

        public void generarNumeros(double[] numerosAleatorios)
        {
            Random random = new Random();

            for (int i = 0; i < numerosAleatorios.Length; i++)
            {
                numerosAleatorios[i] = Math.Truncate((a + (random.NextDouble() * (b - a))) * 10000) / 10000;
            }

        }
    }
}
