using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InnovoWebOnlineMonitor
{
    public partial class CargarParametros : Form
    {
        public CargarParametros()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sesion.Parametros = new ParametrosGlobales();
            Sesion.Parametros.web = textBox1.Text;
            Sesion.Parametros.RutaBD = textBox2.Text;

            Sesion.GuardarParametros();

            Close();
        }
    }
}
