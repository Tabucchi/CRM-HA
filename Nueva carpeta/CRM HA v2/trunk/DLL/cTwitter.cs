using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Xml.Linq;
using System.Text.RegularExpressions;

public class cTwitter
{
    public string Mensaje { get; set; }
    //public DateTime FechaPublicacion { get; set; }
    public string Link { get; set; }

    public cTwitter () {}

    public List<cTwitter> Mensajes(string usuario, int numeroMensajes)
    {
        List<cTwitter> mensajes = new List<cTwitter>();
        string url = ConfigurationManager.AppSettings["TwitterFeedUrl"] + usuario + ".rss";

        try
        {
            var doc = XElement.Load(url);
            var mensajesXML = (from item in doc.Element("channel").Elements("item") select item).Take(numeroMensajes);

            foreach (XElement elm in mensajesXML)
            {                
                var msg = new cTwitter();                
                msg.Mensaje = GetLinks(elm.Element("description").Value.Replace(usuario + ": ", ""));
                //msg.FechaPublicacion = DateTime.Parse(elm.Element("pubDate").Value);
                //msg.Link = elm.Element("link").Value;
                mensajes.Add(msg);
            }           
        }
        catch (Exception)
        {
            //throw new Exception("Hubo un error al obtener los tweets.");
        }
        return mensajes;
    }

    public string GetLinks(string text)
    {
        String pattern;
        pattern = @"(http:\/\/([\w.]+\/?)\S*)";
        Regex re = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        text = re.Replace(text, "<a href=\"$1\" target=\"_blank\">$1</a>");
        return text;
    }

}
