using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace InnovoWebOnlineMonitor
{
    [XmlRootAttribute("ParametrosGlobales", Namespace = "", IsNullable = false)]

    public class ParametrosGlobales
    {
        public string web { get; set; }
        public String UsuarioBD { get; set; }
        public String PassBD { get; set; }
        public String Puerto { get; set; }
        public String  RutaBD { get; set; }
        public String Autominimizar { get; set; }
        public String NombreUsuario { get; set; }
        public string estado { get; set; }
        public bool mailEnviado { get; set; }
    }
}