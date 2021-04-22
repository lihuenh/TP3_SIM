using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorDeAleatoreos
{
    public class ExponencialNegativa
    {
        private double media;

        public ExponencialNegativa(double media)
        {
            this.media = media;
        }

        public void generarNumeros(double[] numerosAleatorios)
        {
            Random random = new Random();
            double lambda = 1 / media;
            for (int i = 0; i < numerosAleatorios.Length; i++)
            {
                numerosAleatorios[i] = Math.Truncate(((-1/lambda) * Math.Log(1 - random.NextDouble())) * 10000) / 10000;
            }
        }
    }
}
