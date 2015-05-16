using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Net;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data;
using System.Threading;

namespace InnovoWebOnlineMonitor
{
    public class Sesion
    {
        public static string ConnectionString { get; set; }
        public static ParametrosGlobales Parametros;
        public static string UbicacionXMLs;

        public static void GuardarParametros()
        {
            bool SeBorro = false;

            while (SeBorro == false)
            {
                try
                {
                    File.Delete(Application.StartupPath + @"\ParametrosGlobalesMonitor.xml");
                    SeBorro = true;
                }
                catch
                {
                    MessageBox.Show("Error al borrar los parámetros globales", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Thread.Sleep(1000);
                }
            }

            TextWriter writer = new StreamWriter(Sesion.UbicacionXMLs + @"\ParametrosGlobalesMonitor.xml");
            XmlSerializer serializerEscritura = new XmlSerializer(typeof(ParametrosGlobales));

            serializerEscritura.Serialize(writer, Parametros);
            writer.Close();
        }
    }
}