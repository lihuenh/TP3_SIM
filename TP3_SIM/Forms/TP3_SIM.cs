using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneradorDeAleatoreos; //Importo la referencia.
using MathNet;
using Microsoft.Reporting.WinForms;
using System.Globalization;

namespace TP3_SIM
{
    public partial class TP3_SIM : Form
    {
        public TP3_SIM()
        {
            InitializeComponent();
        }

        private void TP3_SIM_Load(object sender, EventArgs e)
        {
            cmbDistribuciones.SelectedIndex = -1;
            cmbSignificancia.Enabled = false;
            this.reportViewer1.RefreshReport();
            txtA.Enabled = false; txtB.Enabled = false; txtMedia.Enabled = false; txtVarianza.Enabled = false;
        }

        //-------------------------------------------------------------VERIFICACIONES SOBRE LA DISTRIBUCION SELECCIONADA
        private void cmbDistribuciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMedia.Text = ""; txtVarianza.Text = ""; txtA.Text = ""; txtB.Text = "";
            if (cmbDistribuciones.Text.Equals("Exponencial Negativa"))
            {
                txtMedia.Enabled = true;
                txtMedia.Focus();
                txtVarianza.Enabled = false;
                txtA.Enabled = false;
                txtB.Enabled = false;
            }
            else
            {
                if (cmbDistribuciones.Text.Equals("Normal"))
                {
                    txtMedia.Enabled = true;
                    txtVarianza.Enabled = true;
                    txtMedia.Focus();
                    txtA.Enabled = false;
                    txtB.Enabled = false;
                }
                else
                {
                    if (cmbDistribuciones.Text.Equals("Poisson"))
                    {
                        txtMedia.Focus();
                        txtMedia.Enabled = true;
                        txtVarianza.Enabled = false;
                        txtA.Enabled = false;
                        txtB.Enabled = false;
                    }
                    else
                    {
                        if (cmbDistribuciones.Text.Equals("Uniforme"))
                        {
                            txtMedia.Enabled = false;
                            txtVarianza.Enabled = false;
                            txtA.Enabled = true;
                            txtB.Enabled = true;
                            txtA.Focus();
                        }
                        else
                        {
                            txtMedia.Enabled = false;
                            txtVarianza.Enabled = false;
                            txtA.Enabled = false;
                            txtB.Enabled = false;
                        }
                    }
                }
            }
        }

        //-------------------------------------------------------------BOTON GENERAR NUMEROS ALEATORIOS 
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (cmbDistribuciones.SelectedIndex == -1)
            {
                MessageBox.Show("ERROR!, Debe seleccionar el tipo de distribucion a realizar.","¡ERROR!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                if ((txtMedia.Enabled==true && txtMedia.Text=="")|| (txtVarianza.Enabled == true && txtVarianza.Text == "")|| 
                    (txtA.Enabled == true && txtA.Text == "")|| (txtB.Enabled == true && txtB.Text == "")|| 
                    (txtCantidadNros.Enabled == true && txtCantidadNros.Text == ""))
                {
                    MessageBox.Show("ERROR!, Debe completar todos los campos requeridos.", "¡ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    cmbSignificancia.SelectedIndex = -1;
                    String distribucion;
                    double[] numerosAleatorios = new double[0];
                    if (cmbDistribuciones.Text.Equals("Exponencial Negativa"))
                    {
                        txtMedia.Enabled = true;
                        txtVarianza.Enabled = false;
                        distribucion = cmbDistribuciones.Text;
                        numerosAleatorios = generarAleatorios(distribucion);
                    }
                    else
                    {
                        if (cmbDistribuciones.Text.Equals("Normal"))
                        {
                            txtMedia.Enabled = true;
                            txtVarianza.Enabled = true;
                            distribucion = cmbDistribuciones.Text;
                            numerosAleatorios = generarAleatorios(distribucion);
                        }
                        else
                        {
                            if (cmbDistribuciones.Text.Equals("Poisson"))
                            {
                                txtMedia.Enabled = true;
                                txtVarianza.Enabled = false;
                                distribucion = cmbDistribuciones.Text;
                                numerosAleatorios = generarAleatorios(distribucion);
                            }
                            else
                            {
                                txtMedia.Enabled = false;
                                txtVarianza.Enabled = false;
                                distribucion = cmbDistribuciones.Text;
                                numerosAleatorios = generarAleatorios(distribucion);
                            }
                        }
                    }
                    bool resultado = cargarGrilla(numerosAleatorios);

                    if (resultado)
                    {
                        txtIntervalos.Enabled = true;
                        txtIntervalos.Focus();
                    }
                    else
                    {
                        txtIntervalos.Enabled = false;
                    }
                } 
            }
        }
        //-------------------------------------------------------------METODO PARA GENERAR ALEATORIOS
        private double[] generarAleatorios(String distribucion)
        {
            double[] vectorAleatorios = new double[int.Parse(txtCantidadNros.Text.Trim())];
            double media, varianza, a, b;
           
            if (distribucion.Equals("Exponencial Negativa"))
            {
                media = double.Parse(txtMedia.Text.Trim());
                ExponencialNegativa expNeg = new ExponencialNegativa(media);
                expNeg.generarNumeros(vectorAleatorios);
            }
            else
            {
                if (distribucion.Equals("Normal"))
                {
                    media = double.Parse(txtMedia.Text.Trim());
                    varianza = double.Parse(txtVarianza.Text.Trim());
                    Normal normal = new Normal(media, Math.Sqrt(varianza));
                    normal.generarNumeros(vectorAleatorios);
                }
                else
                {
                    if (distribucion.Equals("Poisson"))
                    {
                        media = double.Parse(txtMedia.Text.Trim());
                        Poisson poisson = new Poisson(media);
                        poisson.generarNumeros(vectorAleatorios);
                    }
                    else
                    {
                        a = double.Parse(txtA.Text.Trim());
                        b = double.Parse(txtB.Text.Trim());

                        Uniforme uniforme = new Uniforme(a, b);
                        uniforme.generarNumeros(vectorAleatorios);
                    }
                }
            }
            return vectorAleatorios;
        }
        //-------------------------------------------------------------METODO PARA CARGAR LA GRILLA DE NUMEROS ALEATORIOS
        private bool cargarGrilla(double[] nros)
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("N");
            tabla.Columns.Add("Nro");

            for (int i = 0; i < nros.Length; i++)
            {
                tabla.Rows.Add(i + 1, nros[i]);
            }
            dgbAleatorios.DataSource = tabla;

            txtMaximo.Text = nros.Max().ToString();
            txtMinimo.Text = nros.Min().ToString();

            if (dgbAleatorios.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------BOTON PARA CALCULAR CHI
        private void btnChiCuadrado_Click(object sender, EventArgs e)
        {
            int distribucion = obtenerDistribucion();

            if (dgbAleatorios.Rows.Count == 0)
            {
                MessageBox.Show("ERROR!, Primero debe generar los valores aleatorios.", "¡ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGenerar.Focus();
            }
            else
            {
                if (txtIntervalos.Text.Equals(""))
                {
                    MessageBox.Show("ERROR!, Debe ingresar la cantidad de intervalos.", "¡ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtIntervalos.Focus();
                }
                else
                {
                    double media=0, varianza=0;
                    if (distribucion == 1)  //Exponencial
                    {
                        if (txtMedia.Text.Equals(""))
                        {
                            MessageBox.Show("Ingrese valor para el parámetro Media");
                            txtMedia.Focus();
                        }
                        else
                        {
                            media = double.Parse(txtMedia.Text);
                            txtGdLibertad.Text = (int.Parse(txtIntervalos.Text) - 1 - 1).ToString();
                        }
                    }
                    else
                    {
                        if (distribucion==2)//Normal
                        {
                            if (txtMedia.Text.Equals("") || txtVarianza.Text.Equals(""))
                            {
                                MessageBox.Show("Ingrese valor para el parámetro Media o Varianza o ambos");
                                txtMedia.Focus();
                            }
                            else
                            {
                                media = double.Parse(txtMedia.Text);
                                varianza = Math.Sqrt(double.Parse(txtVarianza.Text));
                                txtGdLibertad.Text = (int.Parse(txtIntervalos.Text) - 1 - 2).ToString();
                            }
                        }
                        else
                        {
                            if (distribucion==3)//Poisson
                            {
                                if (txtMedia.Text.Equals(""))
                                {
                                    MessageBox.Show("Ingrese valor para el parámetro Media");
                                    txtMedia.Focus();
                                }
                                else
                                {
                                    media = double.Parse(txtMedia.Text);
                                    txtGdLibertad.Text = (int.Parse(txtIntervalos.Text) - 1 - 1).ToString();
                                }
                            }
                            else
                            {
                                if (distribucion==4)//Uniforme
                                {
                                    if (txtA.Text.Equals(""))
                                    {
                                        MessageBox.Show("Ingrese parametro 'a'");
                                        txtA.Focus();
                                    }
                                    else
                                    {
                                        if (txtB.Text.Equals(""))
                                        {
                                            MessageBox.Show("Ingrese parametro 'b'");
                                            txtB.Focus();
                                        }
                                        else
                                        {
                                            if (double.Parse(txtA.Text) >= double.Parse(txtB.Text))
                                            {
                                                MessageBox.Show("El valor del parámetro 'a' no puede ser mayor o igual al valor del parámetro 'b'");
                                                txtA.Focus();
                                            }
                                            else
                                            {
                                                txtGdLibertad.Text = (int.Parse(txtIntervalos.Text) - 1).ToString();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No seleccionó distribución");
                                }
                            }
                        }
                    }
                    
                    bool resultado = generarChiCuadrado(media, varianza);
                    cmbSignificancia.Enabled = true;

                    if (resultado)
                    {
                        txtEstadistico.Text = dgbPruebasBondad.Rows[dgbPruebasBondad.Rows.Count - 1].Cells[6].Value.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                }
                
            }

        }
        //-------------------------------------------------------------METODO PARA GENERAR CHI-CUADRADO
        private bool generarChiCuadrado(double media, double desviacion)
        {

            DataTable tabla = new DataTable();

            double maximo = double.Parse(txtMaximo.Text);
            double minimo = double.Parse(txtMinimo.Text);
            double intervalos = double.Parse(txtIntervalos.Text);
            double tam = (maximo - minimo) / intervalos;

            double desde = minimo, hasta, marca, nro, fe = 0, chiCuadrado = 0, ac = 0;

            int distribucion = obtenerDistribucion();

            tabla.Columns.Add("Desde");
            tabla.Columns.Add("Hasta");
            tabla.Columns.Add("Marca");
            tabla.Columns.Add("FO");
            tabla.Columns.Add("FE");
            tabla.Columns.Add("Chi-Cuadrado");
            tabla.Columns.Add("Acumulador");

            for (int i = 0; i < intervalos; i++)
            {
                int fo = 0;
                hasta = Math.Truncate((desde + tam) * 10000) / 10000;
                for (int j = 0; j < dgbAleatorios.Rows.Count; j++)
                {
                    nro = double.Parse(dgbAleatorios.Rows[j].Cells[1].Value.ToString());
                    if (nro >= desde && nro < hasta)
                    {
                        fo++;
                    }
                }
                switch (distribucion)
                {
                    case 1:
                        fe = (MathNet.Numerics.Distributions.Exponential.CDF(1 / media, hasta) - MathNet.Numerics.Distributions.Exponential.CDF(1 / media, desde)) * dgbAleatorios.Rows.Count;
                        break;
                    case 2:
                        fe = (MathNet.Numerics.Distributions.Normal.CDF(media, desviacion, hasta) - MathNet.Numerics.Distributions.Normal.CDF(media, desviacion, desde)) * dgbAleatorios.Rows.Count;
                        break;
                    case 3:
                        fe = (MathNet.Numerics.Distributions.Poisson.CDF(media, hasta) - MathNet.Numerics.Distributions.Poisson.CDF(media, desde)) * dgbAleatorios.Rows.Count;
                        break;
                    case 4:
                        fe = dgbAleatorios.Rows.Count / intervalos;
                        break;
                }

                marca = (hasta + desde) / 2;
                fe = Math.Truncate(fe * 10000) / 10000;
                chiCuadrado = Math.Truncate(Math.Pow(fo - fe, 2) / fe * 10000) / 10000;
                ac = ac + chiCuadrado;

                tabla.Rows.Add(desde, hasta, marca, fo, fe, chiCuadrado, ac);
                desde = hasta;

                ReportDataSource Datos = new ReportDataSource("DataSet1", tabla);
                reportViewer1.LocalReport.ReportEmbeddedResource = "TP3_SIM.Report.Report1.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(Datos);
                reportViewer1.RefreshReport();
            }

            dgbPruebasBondad.DataSource = tabla;
        
            if (dgbPruebasBondad.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------METODO PARA OBTENER EL TIPO DE DISTRIBUCION ELEGIDO
        private int obtenerDistribucion()
        {
            if (cmbDistribuciones.Text.Equals("Exponencial Negativa"))
            {
                return 1;
            }
            else
            {
                if (cmbDistribuciones.Text.Equals("Normal"))
                {
                    return 2;
                }
                else
                {
                    if (cmbDistribuciones.Text.Equals("Poisson"))
                    {
                        return 3;
                    }
                    else
                    {
                        return 4;
                    }
                }
            }
        }
        //-------------------------------------------------------------BOTON PARA CALCULAR LA APROBACION O RECHAZO DE LA HIPOTESIS
        private void btnChiTabulado_Click(object sender, EventArgs e)
        {
            if (cmbSignificancia.Enabled == false)
            {
                MessageBox.Show("¡ERROR!, primero debe calcular los intervalos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (cmbSignificancia.SelectedIndex == -1)
                {
                    MessageBox.Show("¡ERROR!, seleccione el grado de significancia.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (double.Parse(txtChiTabulado.Text) >= double.Parse(txtEstadistico.Text))
                    {
                        MessageBox.Show("Como el valor de Ho= " + txtEstadistico.Text + " es menor que el valor de Ha= " + txtChiTabulado.Text + ". No hay evidencia suficiente para rechazar la Hipotesis nula", "¡NO SE RECHAZA!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Como el valor de Ho= " + txtEstadistico.Text + " es mayor que el valor de Ha= " + txtChiTabulado.Text + ". Se rechaza la Hipotesis nula", "¡SE RECHAZA!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        //-------------------------------------------------------------ACTUACION DEL COMBO BOX SIGNIFICANCIA 
        private void cmbSignificancia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSignificancia.SelectedIndex != -1)
            {
                double n = double.Parse(txtGdLibertad.Text);
                double significancia = double.Parse(cmbSignificancia.Text);
                txtChiTabulado.Text = (Math.Truncate(MathNet.Numerics.Distributions.ChiSquared.InvCDF(n, significancia) * 10000) / 10000).ToString();
            }
        }
    }
}
