using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorDeAleatoreos
{
    public class Normal
    {
        private double media, desviacion;

        public Normal(double media, double desviacion)
        {
            this.media = media;
            this.desviacion = desviacion;
        }

        public void generarNumeros(double[] numerosAleatorios)
        {
            Random random = new Random();
            double rnd1;
            double rnd2;

            for (int i = 0; i < numerosAleatorios.Length; i++)
            {
                rnd1 = random.NextDouble();
                rnd2 = random.NextDouble();
                numerosAleatorios[i] = Math.Truncate((((Math.Sqrt(-2 * Math.Log(rnd1)) * Math.Cos(2 * Math.PI * rnd2)) * desviacion) + media) * 10000) / 10000;
            }
        }
    }
}
