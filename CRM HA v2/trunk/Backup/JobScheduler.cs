using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Globalization;
using System.Xml;
using System.IO;
using OpenPop.Pop3;
using System.Data;
using OpenPop.Mime;
using OpenPop.Pop3.Exceptions;
using System.Collections;
using log4net;

public class JobScheduler : IJob
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
    private string pathXML = "C:\\PUBLICACIONES\\CRM NAEX v5\\xml\\fechaMail.xml";
    //private string pathXML = "C:\\Users\\ntabucchi\\Documents\\Proyectos\\NAEX\\trunk\\xml\\fechaMail.xml";

    public virtual void Execute(JobExecutionContext context)
    {
        try
        {                 
            Pop3Client pop3Client;
            pop3Client = new Pop3Client();
            pop3Client.Connect("pop.gmail.com", 995, true);
           // pop3Client.Authenticate("ntabucchi@naex.com.ar", "nt121186");
            pop3Client.Authenticate("crm@naex.com.ar", "Naex2014@");

            cSendMail send = new cSendMail();
            DateTime fechaMail = new DateTime();
            string ultimaFechaMail = null;
            ArrayList uid = new ArrayList();
            
            int count = pop3Client.GetMessageCount();
                       
            for (int j = 1; j <= count; j++) //Guardo en el array todo los MessageId de los mail no leidos hasta la fecha
            {
                Message message = pop3Client.GetMessage(j);
                uid.Add(message.Headers.MessageId);
            }
            
            for (int i = count; i >= 1; i--)
            {
                Message message = pop3Client.GetMessage(i);

                if (IsDate(message.Headers.Date)) //Verifica el formato de la fecha               
                {
                    string[] split = message.Headers.Date.Split(new Char[] { ' ' });
                    string da = split[1].ToString() + "/" + split[2].ToString().Replace("ene", "01").Replace("feb", "02").Replace("Mar", "03").Replace("abr", "04").Replace("may", "05").Replace("jun", "06").Replace("jul", "07").Replace("ago", "08").Replace("sep", "09").Replace("oct", "10").Replace("nov", "11").Replace("dic", "12") + "/" + split[3].ToString() + " " + split[4].ToString();
                    fechaMail = Convert.ToDateTime(da);
                    ultimaFechaMail = Convert.ToDateTime(ultimaFechaMail) > fechaMail ? ultimaFechaMail : fechaMail.ToString();
                }
                else
                {
                    fechaMail = Convert.ToDateTime(ObtenerFecha(message.Headers.Date));
                    ultimaFechaMail = Convert.ToDateTime(ultimaFechaMail) > fechaMail ? ultimaFechaMail : fechaMail.ToString();
                }

                for (int j = 0; j < uid.Count; j++)
                {
                    if (uid[j].ToString() == message.Headers.MessageId) //Verifica que el nro del mensaje no exista en la bd
                    {
                        //Compara fecha
                        if (FechaXml() < fechaMail)  //FechaXml(): fecha que se guarda en el Xml
                        {
                            #region Compara Fechas
                            if (message.Headers.InReplyTo.Count == 0) //Verifica que el mail es una respuesta a otro.
                            {
                                cCliente c = cCliente.SearchByMail(message.Headers.From.Address.ToString(), true);
                                if (c != null) //Valida que el mail pertenece a un cliente que esta en la base de datos
                                {
                                    #region Autorizado
                                    if (cCliente.SearchByMail(message.Headers.From.Address.ToString(), true).Autorizacion == (Int16)Autorizaciones.Autorizado) //Valida que el cliente sea el autorizado para mandar los mails
                                    {
                                       if (!ValidarNroMail(message.Headers.MessageId))
                                       {
                                            cPedido pedido = new cPedido();
                                            pedido.IdEmpresa = cCliente.SearchByMail(message.Headers.From.Address.ToString(), false).IdEmpresa;
                                            pedido.IdCliente = cCliente.SearchByMail(message.Headers.From.Address.ToString(), false).Id;
                                            pedido.IdUsuario = "21";  //CRM 
                                            pedido.Titulo = message.Headers.Subject;
                                        
                                            string body = null;
                                            try {
                                                body = message.MessagePart.MessageParts[0].GetBodyAsText();
                                            }
                                            catch {
                                                body = message.MessagePart.MessageParts[0].MessageParts[0].GetBodyAsText();
                                            }
                                        
                                            string[] split = body.Split(new Char[] { '*' });
                                        
                                            pedido.Descripcion = split[0].ToString();
                                            //pedido.Fecha = Convert.ToDateTime(message.Headers.Date);
                                            pedido.Fecha = Convert.ToDateTime(message.Headers.DateSent.Date);
                                            pedido.IdEstado = (Int16)Estado.Nuevo;
                                            pedido.IdCategoria = (Int16)Categoria.Soporte;
                                            pedido.IdPrioridad = (Int16)Prioridad._24hs;
                                            pedido.IdResponsable = "-1";
                                            pedido.IdModoResolucion = 0;
                                            int id = pedido.Save();
                                            pedido.Id = id.ToString();
                                            pedido.Save();

                                            cPedidoMail pm = new cPedidoMail();
                                            pm.IdPedido = id.ToString();
                                            pm.NroMail = message.Headers.MessageId;
                                            pm.Save();
                                           
                                        //Envio de mail
                                        send.CrearFinalizarTicket(pedido);

                                        }else{
                                            GuardarXML(Convert.ToDateTime(ultimaFechaMail), count);
                                       }
                                    }
                                    else
                                    {
                                        cCliente cliente = cCliente.SearchByMail(message.Headers.From.Address.ToString(), true);
                                        List<cCliente> clientes = cCliente.GetClientesAutorizados(cliente.IdEmpresa);
                                        //Envia mail con los clientes autorizados de la empresa para crear un ticket
                                        //send.CrearClienteAutorizadoTicket(cliente.Mail, cliente.Nombre, clientes);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Dominio
                                    //Si el dominio le pertenece a alguna empresa envio el mail con los responsables
                                    if (ValidarDominio(message.Headers.From.Address.ToString()))
                                    {
                                        cCliente cliente = cCliente.SearchByMail(message.Headers.From.Address.ToString(), false);
                                        List<cCliente> clientes = cCliente.GetClientesAutorizados(cliente.IdEmpresa);
                                        //Envia mail con los clientes autorizados de la empresa para crear un ticket
                                        send.CrearClienteAutorizadoTicket(cliente.Mail, cliente.Nombre, clientes);
                                        //GuardarXML(fechaMail);
                                    }
                                    else
                                    {
                                        //Envia mail contestanto que no esta autorizado para cargar tickets
                                        send.CrearMailNoAutorizado(message.Headers.From.Address.ToString());
                                        //GuardarXML(fechaMail);
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                // GuardarXML(fechaMail); 
                            }
                            #endregion
                        }
                    }
                }
            }

            if (ultimaFechaMail != null)
            {
                DateTime f = Convert.ToDateTime(ultimaFechaMail);
                GuardarXML(Convert.ToDateTime(ultimaFechaMail), count);
            }
            else
            {
                string date = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                GuardarXML(Convert.ToDateTime(date), count);
            }
        }
        catch(Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("JobScheduler - " + DateTime.Now + "- " + ex.Message + " - Execute");
            return;
        }
    }

    #region XML
    public int indiceMail()
    {
        //XmlTextReader reader = new XmlTextReader(pathXML);
        XmlTextReader reader = new XmlTextReader(pathXML);
        string name = "";
        int indiceMail = 0;
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element: // The node is an element.
                    name = reader.Name;
                    break;
                case XmlNodeType.Text: //Display the text in each element. 
                    if (name == "indiceMail")
                    {
                        indiceMail = Convert.ToInt16(reader.Value);
                    }
                    break;
            }
        }
        reader.Close();
        return indiceMail;
    }

    //Verifica que sea una fecha correcta, la fecha del mail
    public static bool IsDate(string inputDate)
    {
        bool isDate = true;
        try
        {
            DateTime dateValue;
            string[] split = inputDate.Split(new Char[] { ' ' });
            string da = split[1].ToString() + "/" + split[2].ToString().Replace("ene", "01").Replace("feb", "02").Replace("Mar", "03").Replace("abr", "04").Replace("may", "05").Replace("jun", "06").Replace("jul", "07").Replace("ago", "08").Replace("sep", "09").Replace("oct", "10").Replace("nov", "11").Replace("dic", "12") + "/" + split[3].ToString() + " " + split[4].ToString();
            dateValue = DateTime.ParseExact(da, "d/MM/yyyy hh:mm:ss", null);
        }
        catch
        {
            isDate = false;
        }
        return isDate;
    }
    
    //Se obtiene la fecha del mail
    public string ObtenerFecha(string fecha)
    {
        //string[] split = fecha.Split(new Char[] { '-' });
        string[] split = fecha.Split(new Char[] { ' ' });
        string da = split[1].ToString() + "/" + split[2].ToString().Replace("ene", "1").Replace("feb", "2").Replace("Mar", "3").Replace("abr", "4").Replace("may", "5").Replace("jun", "6").Replace("jul", "7").Replace("ago", "8").Replace("sep", "9").Replace("oct", "10").Replace("nov", "11").Replace("dic", "12") + "/" + split[3].ToString() + " " + split[4].ToString();
        return Convert.ToDateTime(da).ToString("dd/MM/yyyy hh:mm:ss tt");
        //string dt = Convert.ToDateTime(split[0].ToString()).ToString("dd/MM/yyyy hh:mm:ss tt");
        //return Convert.ToDateTime(split[0].ToString()).ToString("dd/MM/yyyy hh:mm:ss tt");
    }

    //se obtiene la fecha guardada en el xml
    public DateTime FechaXml()
    {
        XmlTextReader reader = new XmlTextReader(pathXML);
        string name = "";
        DateTime xmlFecha = new DateTime();
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element: // The node is an element.
                    name = reader.Name;
                    break;
                case XmlNodeType.Text: //Display the text in each element.                    
                    if (name == "fecha")
                    {
                        xmlFecha = Convert.ToDateTime(reader.Value);
                    }
                    break;
            }
        }
        reader.Close();

        return xmlFecha;
    }

    public void GuardarXML(DateTime fecha, int indiceMail)
    {
        XmlWriter w = XmlWriter.Create(pathXML);
        w.WriteStartElement("Datos");
        w.WriteElementString("fecha", fecha.ToString());
        w.WriteElementString("indiceMail", indiceMail.ToString());
        w.WriteEndElement();
        w.Close();
    }

    
    #endregion
    
    public bool ValidarDominio(string dominio)
    {
        string[] split = dominio.Split(new Char[] { '@', '.' });
        bool flag = false;
        int i=0;
        string idEmpresa = null;
        foreach (string s in split)
        {
            if (!string.IsNullOrEmpty(cEmpresa.GetDominioMail(s.ToString())))
            {
                idEmpresa = cEmpresa.GetDominioMail(s.ToString());
            }
        }

        if(!string.IsNullOrEmpty(idEmpresa))
            flag = true;
        else
            flag = false;

        return flag;
    }
    
    public static bool ValidarNroMail(string nro)
    {
        bool flag = false;
        string nroMail = cPedidoMail.GetNroMail(nro);
       
        if (!string.IsNullOrEmpty(nroMail))
            flag = true;
        else
            flag = false;
        
        return flag;
    }

} 