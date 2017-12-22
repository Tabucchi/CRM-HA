using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Xml;
using System.Globalization;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]

public class Services : System.Web.Services.WebService
{
    [WebMethod]
    public static string ObtenerValorDolar()
    {
        try
        {
            string csvData;
            string valor = null;

            using (WebClient web = new WebClient())
            {
                csvData = web.DownloadString("http://finance.yahoo.com/d/quotes.csv?e=.csv&f=sl1d1t1&s=USDARS=X");
                string[] split = csvData.ToString().Split(new Char[] { ',' });
                      
                valor = split[1].ToString().Length > 4 ? split[1].ToString().Substring(0, 4) : split[1].ToString();  
            }

            return valor;
        }
        catch { return "-1"; }
                
        #region Obtener valor dólar Viejo
        /*try
        {
            // Cear la solicitud de la URL.
            WebRequest request = WebRequest.Create("http://www.eldolarblue.net/calcularDolarLibre.php?as=xml&val=1000&dir=pdl");

            // Obtener la respuesta.
            WebResponse response = request.GetResponse();

            // Abrir el stream de la respuesta recibida.
            StreamReader reader = new StreamReader(response.GetResponseStream());

            //declarar documento XML
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);

            XmlNodeList valores = xml.GetElementsByTagName("exchangerate");

            XmlElement valor = (XmlElement)valores[0];

            // Cerrar los streams abiertos.
            reader.Close();
            response.Close();

         //   double valorCompra = Convert.ToDouble(valor.GetElementsByTagName("buy")[0].InnerText.Replace(".", ","));
         //   double valorVenta = Convert.ToDouble(valor.GetElementsByTagName("sell")[0].InnerText.Replace(".", ","));

            float valorCompra = Convert.ToSingle(valor.GetElementsByTagName("buy")[0].InnerText.Replace(".", ","), CultureInfo.CreateSpecificCulture("es-ES"));
            float valorVenta = Convert.ToSingle(valor.GetElementsByTagName("sell")[0].InnerText.Replace(".", ","), CultureInfo.CreateSpecificCulture("es-ES"));

            // Devuelvo el promedio entre valor compra y venta.
           // double valorFinal = Math.Round(((valorCompra + valorVenta) / 2), 2);

            return Math.Round(((valorCompra + valorVenta) / 2), 2).ToString(); //valorFinal.ToString();
        }
        catch { return "-1"; }*/
        #endregion
    }
}


