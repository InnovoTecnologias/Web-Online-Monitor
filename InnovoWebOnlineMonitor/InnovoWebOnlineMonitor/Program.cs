using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Net;
using Microsoft.Win32;
using System.Threading;

namespace InnovoWebOnlineMonitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (File.Exists(Application.StartupPath + @"\ParametrosGlobalesMonitor.xml")) Sesion.UbicacionXMLs = Application.StartupPath;
            else if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Innovo-Monitor\ParametrosGlobalesMonitor.xml")) Sesion.UbicacionXMLs = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Innovo-Monitor";
            else Sesion.UbicacionXMLs = "**";

            //Lectura de ParámetrosGlobales.
            if (Sesion.UbicacionXMLs == "**")
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Innovo-Monitor");
                Sesion.UbicacionXMLs = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Innovo-Monitor";
                CargarParametros nuevo = new CargarParametros();
                nuevo.ShowDialog();
            }

            Application.UseWaitCursor = true;
            StreamReader reader = null;

            try
            {
                Sesion.Parametros = new ParametrosGlobales();

                reader = new StreamReader(Sesion.UbicacionXMLs + @"\ParametrosGlobalesMonitor.xml");

                XmlSerializer serializerLectura = new XmlSerializer(typeof(ParametrosGlobales));

                Sesion.Parametros = (ParametrosGlobales)serializerLectura.Deserialize(reader);

                reader.Close();
            }
            catch
            {
                MessageBox.Show("Error en la lectura de los parámetros globales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            Application.UseWaitCursor = false;
            Application.Run(new Form1());
        }
    }
}
