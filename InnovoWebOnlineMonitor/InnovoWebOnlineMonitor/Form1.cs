using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Net.Mail;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace InnovoWebOnlineMonitor
{
    public partial class Form1 : Form
    {
        public delegate void DelegadoReportarAvance(string tipo);
        public Form1()
        {
            InitializeComponent();            
        }

        private bool Ping(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Timeout = 3000;
                request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                request.Method = "HEAD";

                using (var response = request.GetResponse())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        private void ChequearEstado()
        {
            string url = textBox1.Text;
            if (Ping(url))
            {
                SQLiteConnection conexion = new SQLiteConnection("Data Source=" + Sesion.Parametros.RutaBD + ";Version=3;New=False;");
                SQLiteCommand comando = new SQLiteCommand("insert into history (ref, topic, subject, body, date_and_time, user) values (@ref, @topic, @subject, @body, @date_and_time, @user)", conexion);
                //SQLiteCommand command = new SQLiteCommand("select * from log", conexion);
                comando.Parameters.AddWithValue("@ref", 0);
                comando.Parameters.AddWithValue("@topic", "Ping");
                comando.Parameters.AddWithValue("@subject", "Online");
                comando.Parameters.AddWithValue("@body", url);
                comando.Parameters.AddWithValue("@date_and_time", DateTime.Now);
                comando.Parameters.AddWithValue("@user", "WOM");
                conexion.Open();
                comando.ExecuteNonQuery();
                conexion.Close();
                if (Sesion.Parametros.estado == "Offline")
                {

                    MailMessage mail = new MailMessage("franco@innovotecnologias.com.ar", "franco@innovotecnologias.com.ar");
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("franco@innovotecnologias.com.ar", "franlopri10");
                    client.EnableSsl = true;
                    client.Host = "mail.innovotecnologias.com.ar";
                    mail.Subject = "Web Online Monitor - Estado: Online";
                    mail.Body = "La Página " + Sesion.Parametros.web + " se ha restablecido y se encuentra online";

                    ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };

                    client.Send(mail);
                    Sesion.Parametros.estado = "Online";
                    Sesion.Parametros.mailEnviado = false; 
                }
            }
            else
            {
                SQLiteConnection conexion = new SQLiteConnection("Data Source=" + Sesion.Parametros.RutaBD + ";Version=3;New=False;");
                SQLiteCommand comando = new SQLiteCommand("insert into history (ref, topic, subject, body, date_and_time, user) values (@ref, @topic, @subject, @body, @date_and_time, @user)", conexion);
                //SQLiteCommand command = new SQLiteCommand("select * from log", conexion);
                comando.Parameters.AddWithValue("@ref", 0);
                comando.Parameters.AddWithValue("@topic", "Ping");
                comando.Parameters.AddWithValue("@subject", "Offline");
                comando.Parameters.AddWithValue("@body", url);
                comando.Parameters.AddWithValue("@date_and_time", DateTime.Now);
                comando.Parameters.AddWithValue("@user", "WOM");
                conexion.Open();
                comando.ExecuteNonQuery();
                conexion.Close();
                Sesion.Parametros.estado = "Offline";

                if (Sesion.Parametros.mailEnviado == false)
                {
                    MailMessage mail = new MailMessage("franco@innovotecnologias.com.ar", "franco@innovotecnologias.com.ar");
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("franco@innovotecnologias.com.ar", "franlopri10");
                    client.EnableSsl = true;
                    client.Host = "mail.innovotecnologias.com.ar";
                    mail.Subject = "Web Online Monitor - Estado: Offline";
                    mail.Body = "La Página " + Sesion.Parametros.web + " se encuentra offline";

                    ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };

                    client.Send(mail);
                    Sesion.Parametros.mailEnviado = true; 
                }
            }

            ReportarAvance("FinChequearEstado"); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Sesion.Parametros.mailEnviado = false; 
            textBox1.Text = Sesion.Parametros.web;
            TimerEstado.Start(); 
        }

        private void TimerEstado_Tick(object sender, EventArgs e)
        {
            TimerEstado.Stop();
            Thread Hilo = new Thread(ChequearEstado);
            Hilo.IsBackground = true;
            Hilo.Start();
        }

        public void ReportarAvance(string tipo)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new DelegadoReportarAvance(ReportarAvance), new object[] { tipo });
                return;
            }

            if (tipo == "FinChequearEstado")
            {
              label3.Text = Sesion.Parametros.estado;
              TimerEstado.Start();          
            }           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close(); 
        }
    }
}
