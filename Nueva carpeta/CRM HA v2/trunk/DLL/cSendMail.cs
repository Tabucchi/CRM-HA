using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Text;
using System.IO;
using DLL.Negocio;

public class cSendMail
{
    public cSendMail() {}

    #region Credenciales
    /*string userName = "ntabucchi@naex.com.ar";
    string password = "lavalle557";*/

    string userName = "info@haemprendimientos.com.ar";
    string password = "Naex2017";
    #endregion

    private static string NuevoTicket_Cliente;
    private static string AsignarTicket;
    private static string ComentarioTicket;
    private static string FinalizarTicketCliente;
    private static string FinalizarTicketSoporte;
    private static string AvisoActualizacionPrecio;
    private static string AvisoPago;
    private static string VencimientoPago;

    //Envio al Crear / Finalizar un Ticket.
    public void CrearFinalizarTicket(cPedido pedido)
    {
        string _idPedido = pedido.IdEmpresa + "-" + pedido.Id;
        string messageBody = "";
        string messageBodySoporte = "";

        MailMessage _mail = new MailMessage();
        _mail.From = new MailAddress("HA Emprendimientos <info@haemprendimientos.com.ar>");
        //_mail.From = new MailAddress("NAEX - Sistema CRM <nicolas.tabucchi@bethcom.com.ar>");

        string[] direcciones = pedido.GetCliente().Mail.Split(new Char[] { ',', ';' });
        foreach (string dir in direcciones)
            _mail.To.Add(new MailAddress(dir));

        MailMessage _mailSoporte = new MailMessage("HA Emprendimientos <info@haemprendimientos.com.ar>", "HA Emprendimientos <info@haemprendimientos.com.ar>");
        //MailMessage _mailSoporte = new MailMessage("NAEX - Sistema CRM <ntabucchi@naex.com.ar>", "NAEX - Sistema de CRM <nicolas.tabucchi@bethcom.com.ar>");

        if (pedido.GetEstado == "Nuevo")
        {
            // Correo que se envía al Cliente al CREAR un Ticket.
            StreamReader archivo = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplate\\NuevoTicketCliente.html"));
            NuevoTicket_Cliente = archivo.ReadToEnd();
            archivo.Close();
            messageBody = NuevoTicket_Cliente.Replace("[#Cliente]", pedido.GetCliente().Nombre).Replace("[#Titulo]", pedido.Titulo).Replace("[#IdTicket]", _idPedido);
            
            _mail.Subject = "Ticket ID " + _idPedido + " - " + pedido.Titulo;
            _mailSoporte.Subject = pedido.GetCliente().GetEmpresa() + ": Ticket ID " + _idPedido + " - " + pedido.Titulo;
            _mailSoporte.Body = messageBody;
        }

        if (pedido.GetEstado == "Finalizado")
        {
            //Mail que envia al cliente cuando finaliza el ticket
            StreamReader archivo = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplate\\FinalizarTicketCliente.html"));
            FinalizarTicketCliente = archivo.ReadToEnd();
            archivo.Close();
            messageBody = FinalizarTicketCliente.Replace("[#idTicket]", _idPedido).Replace("[#Titulo]", pedido.Titulo);
            
            //Mail que envia a soporte cuando finaliza el ticket
            StreamReader archivo1 = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplate\\FinalizarTicketSoporte.html"));
            FinalizarTicketSoporte = archivo1.ReadToEnd();
            archivo1.Close();
            messageBodySoporte = FinalizarTicketSoporte.Replace("[#Titulo]", pedido.Titulo).Replace("[#Empresa]", pedido.GetEmpresa)
                .Replace("[#Cliente]", pedido.GetClienteNombre)
                .Replace("[#Mail]", pedido.GetCliente().Mail)
                .Replace("[#idTicket]", pedido.Id)
                .Replace("[#Usuario]", pedido.GetLastComentarioInfo);
         
            _mail.Subject = "NAEX:" + " Pedido ID " + _idPedido + " // FINALIZADO";
            _mailSoporte.Subject = pedido.GetCliente().GetEmpresa() + ": Pedido ID " + _idPedido + " // FINALIZADO";
            _mailSoporte.Body = messageBodySoporte;
        }

        try
        {
            // Mail Cliente
            _mail.IsBodyHtml = true;
            _mail.Priority = MailPriority.Normal;
            _mail.Body = messageBody;

            //Mail Soporte           
            _mailSoporte.IsBodyHtml = true;
            _mailSoporte.Priority = MailPriority.Normal;

            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential(userName, password);
            _smtpMail.EnableSsl = true;

            if (!string.IsNullOrEmpty(pedido.GetCliente().Mail))
            {
                System.Threading.Thread threadSendMails;
                threadSendMails = new System.Threading.Thread(delegate()
                {
                    try
                    {
                        _smtpMail.Send(_mail);
                        _smtpMail.Send(_mailSoporte);
                    }
                    catch (System.Net.Mail.SmtpFailedRecipientException ex)
                    {

                        throw ex;
                    }
                    catch (System.Net.Mail.SmtpException ex)
                    {

                        throw ex;
                    }
                });

                threadSendMails.IsBackground = true;
                threadSendMails.Start();
            }
        }
        catch (Exception ex)
        {
            string excepcion = ex.ToString();
        }
    }

    //Envio de Asignación de Responsable.
    public void AsignarPedido(cPedido pedido)
    {
        string messageBody = "";

        string _idPedido = pedido.IdEmpresa + "-" + pedido.Id;

        MailMessage _mail = new MailMessage("HA Emprendimientos <info@haemprendimientos.com.ar>", cUsuario.Load(pedido.GetResponsable().IdResponsable).Mail);
        //MailMessage _mail = new MailMessage("NAEX - Sistema CRM <nicolas.tabucchi@bethcom.com.ar>", cUsuario.Load(pedido.GetResponsable().IdResponsable).Mail);
        
        _mail.Subject = "Nuevo Ticket Asignado: " + pedido.Titulo + " (" + pedido.GetEmpresa + ") -" + pedido.Id.ToString();

        StreamReader archivo = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplate\\AsignarPedido.html"));
        AsignarTicket = archivo.ReadToEnd();
        archivo.Close();

        messageBody = AsignarTicket.Replace("[#Titulo]", pedido.Titulo).Replace("[#Empresa]", pedido.GetCliente().GetEmpresa()).Replace("[#Cliente]", pedido.GetCliente().Nombre)
            .Replace("[#MailCliente]", pedido.GetCliente().Mail)
            .Replace("[#IdTicket]", _idPedido)
            .Replace("[#Usuario]", cUsuario.Load(pedido.GetResponsable().IdResponsable).Nombre)
            .Replace("[#Responsable]", pedido.GetUsuario)
            .Replace("[#DescripcionTicket]", pedido.Descripcion)
            .Replace("[#Prioridad]", pedido.GetPrioridad);

        try
        {
            _mail.IsBodyHtml = true;
            _mail.Priority = MailPriority.Normal;

            AlternateView alternativaHTML = AlternateView.CreateAlternateViewFromString(messageBody, null, "text/html");
            _mail.AlternateViews.Add(alternativaHTML);

            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential(userName, password);
            _smtpMail.EnableSsl = true;

            if (cUsuario.Load(pedido.GetResponsable().IdResponsable).Mail != "")
            {
               _smtpMail.Send(_mail);  
            }

            // Dispose
            _mail.Dispose();
        }
        catch (Exception ex)
        {
            string excepcion = ex.ToString();
        }
    }

    //Envio de Comentario.
    public void EnviarComentario(cPedido pedido, List<iAutorComentario> involucrados, string comentario, string nombreUsuarioComento, bool visibilidadCliente)
    {
        string messageBody = "";
        string _idPedido = pedido.IdEmpresa + "-" + pedido.Id;        
        
        MailMessage _mail = new MailMessage();
        _mail.From = new MailAddress("HA Emprendimientos <info@haemprendimientos.com.ar>");
        //_mail.From = new MailAddress("NAEX - Sistema CRM <ntabucchi@naex.com.ar>");
                
        foreach (iAutorComentario user in involucrados)
        {
            if (user != null) {
                if (!string.IsNullOrEmpty(user.Mail)) {
                    //Si es Usuario, la agrego a la lista.
                    if (user.GetType().Name == "cUsuario")
                        _mail.To.Add(new MailAddress(user.Mail));

                    //Si es Cliente y la visibilidadCliente es TRUE, lo agrego a la lista.
                    if (user.GetType().Name == "cCliente" && visibilidadCliente == true)
                        _mail.To.Add(new MailAddress(user.Mail));
                }
            }
        }
        
        _mail.Subject = "Pedido ID " + _idPedido + " // COMENTADO";

        // Correo que se envía al Cliente al CREAR un Ticket.
        StreamReader archivo = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplate\\Comentario.html"));
        ComentarioTicket = archivo.ReadToEnd();
        archivo.Close();
        messageBody = ComentarioTicket.Replace("[#Usuario]", nombreUsuarioComento)
            .Replace("[#Cliente]", pedido.GetCliente().Nombre).Replace("[#IdTicket]", _idPedido)
            .Replace("[#Titulo]", pedido.Titulo)
            .Replace("[#Comentario]", comentario);

        //Mail Cliente
        _mail.IsBodyHtml = true;
        _mail.Priority = MailPriority.Normal;
        
        //Vista alternativa en HTML
        AlternateView alternativaHTML = AlternateView.CreateAlternateViewFromString(messageBody, null, "text/html");
        _mail.AlternateViews.Add(alternativaHTML);
        
        try {
            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential(userName, password);
            _smtpMail.EnableSsl = true;

            _smtpMail.Send(_mail);
            
            // Dispose
            _mail.Dispose();
        }       
        catch { }        
    }

    public void EnviarAvisoPago(cEmpresa em, cCuota cuota)
    {
        string messageBody = "";
        string _idPedido = em.Mail;

        MailMessage _mail = new MailMessage("HA Emprendimientos <info@haemprendimientos.com.ar>", em.Mail);
        //MailMessage _mail = new MailMessage("NAEX - Sistema CRM <nicolas.tabucchi@bethcom.com.ar>", em.Mail);

        _mail.Subject = "Aviso de pago";

        StreamReader archivo = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplate\\AvisoPagoTemplate.html"));
        AvisoPago = archivo.ReadToEnd();
        archivo.Close();

        decimal aux = (cuota.Monto * (Convert.ToDecimal(cuota.Comision) + 100)) / 100;
        decimal gastos = aux - cuota.Monto;
        gastos = Math.Round(gastos, 2);

        decimal iva =  Math.Round(((aux * 121) / 100) - aux,2);

        messageBody = AvisoPago.Replace("[#Cliente]", em.Nombre)
            .Replace("[#Fecha]", String.Format("{0:MM/dd/yyyy}", DateTime.Today))
            .Replace("[#Monto]", cuota.Monto.ToString())
            .Replace("[#Gastos]", gastos.ToString())
            .Replace("[#iva]", iva.ToString())
            .Replace("[#total]", Math.Round(cuota.Vencimiento1, 2).ToString());

        try
        {
            _mail.IsBodyHtml = true;
            _mail.Priority = MailPriority.Normal;

            AlternateView alternativaHTML = AlternateView.CreateAlternateViewFromString(messageBody, null, "text/html");
            _mail.AlternateViews.Add(alternativaHTML);

            //SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential(userName, password);
            _smtpMail.EnableSsl = true;

            if (em.Mail != "")
            {
                _smtpMail.Send(_mail);
            }

            // Dispose
            _mail.Dispose();
        }
        catch (Exception ex)
        {
            string excepcion = ex.ToString();
        }
    }

    public void EnviarVencimientoPago(string _empresa, string _mailEmpresa, string _cuota)
    {
        string messageBody = "";
        string _idPedido = _mailEmpresa;

        MailMessage _mail = new MailMessage("HA Emprendimientos <info@haemprendimientos.com.ar>", _mailEmpresa);
        //MailMessage _mail = new MailMessage("NAEX - Sistema CRM <ntabucchi@naex.com.ar>", _mailEmpresa);

        _mail.Subject = "Aviso vencimiento de pago";

        StreamReader archivo = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplate\\AvisoVencimientoTemplate.html"));
        VencimientoPago = archivo.ReadToEnd();
        archivo.Close();
        messageBody = VencimientoPago.Replace("[#Cliente]", _empresa).Replace("[#Cuota]", _cuota);

        try
        {
            _mail.IsBodyHtml = true;
            _mail.Priority = MailPriority.Normal;

            AlternateView alternativaHTML = AlternateView.CreateAlternateViewFromString(messageBody, null, "text/html");
            _mail.AlternateViews.Add(alternativaHTML);

            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential(userName, password);
            _smtpMail.EnableSsl = true;

            if (_mailEmpresa != "")
            {
                _smtpMail.Send(_mail);
            }

            // Dispose
            _mail.Dispose();
        }
        catch (Exception ex)
        {
            string excepcion = ex.ToString();
        }
    }

    public void EnviarActualizacionPrecio()
    {
        string messageBody = "";

        //MailMessage _mail = new MailMessage("HA Emprendimientos <info@haemprendimientos.com.ar>", em.Mail);
        MailMessage _mail = new MailMessage();
        _mail.From = new MailAddress("NAEX - Sistema CRM <ntabucchi@naex.com.ar>");
        _mail.To.Add(new MailAddress("ntabucchi@naex.com.ar"));
        _mail.To.Add(new MailAddress("ntabucchi@naex.com.ar"));

        _mail.Subject = "Actualización de precios";

        StreamReader archivo = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplate\\AvisoActualizacionPrecios.html"));
        AvisoActualizacionPrecio = archivo.ReadToEnd();
        archivo.Close();

        messageBody = AvisoActualizacionPrecio;

        try
        {
            _mail.IsBodyHtml = true;
            _mail.Priority = MailPriority.Normal;

            AlternateView alternativaHTML = AlternateView.CreateAlternateViewFromString(messageBody, null, "text/html");
            _mail.AlternateViews.Add(alternativaHTML);

            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential(userName, password);
            _smtpMail.EnableSsl = true;

            _smtpMail.Send(_mail);
            
            // Dispose
            _mail.Dispose();
        }
        catch (Exception ex)
        {
            string excepcion = ex.ToString();
        }
    }

    public void EnviarAvisoCuota(string _empresa, string _mailEmpresa, string _idCuota)
    {
        string messageBody = "";
        string _idPedido = _mailEmpresa;

        MailMessage _mail = new MailMessage("HA Emprendimientos <info@haemprendimientos.com.ar>", _mailEmpresa);
        //MailMessage _mail = new MailMessage("NAEX - Sistema CRM <ntabucchi@naex.com.ar>", _mailEmpresa);

        _mail.Subject = "Aviso vencimiento de pago";

        StreamReader archivo = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplate\\AvisoCuota.html"));
        VencimientoPago = archivo.ReadToEnd();
        archivo.Close();

        cCuota cuota = cCuota.Load(_idCuota);
        string _indice = null;

        cOperacionVenta ov = cOperacionVenta.GetOperacionByFormaPago(cuota.IdFormaPagoOV);

        if (ov.Cac == true)
            _indice = eIndice.CAC.ToString();

        if (ov.Uva == true)
            _indice = eIndice.UVA.ToString();

        if (ov.Cac == false && ov.Uva == false)
            _indice = eIndice.CAC.ToString();

        messageBody = VencimientoPago.Replace("[#Cliente]", _empresa).Replace("[#Mes]", cuota.FechaVencimiento1.Month.ToString()).Replace("[#Indice]", _indice).Replace("[#Year]", cuota.FechaVencimiento1.Year.ToString()).Replace("[#Nro]", cuota.Nro.ToString()).Replace("[#CAC]", cuota.GetVariacionCAC).Replace("[#MontoAjustado]", cuota.GetMontoAjustado).Replace("[#Monto]", cuota.GetMonto1).Replace("[#Comision]", cuota.GetTotalComision).Replace("[#Fecha1]", String.Format("{0:dd/MM/yyyy}", cuota.FechaVencimiento1)).Replace("[#Monto1]", cuota.GetVencimiento1).Replace("[#Fecha2]", String.Format("{0:dd/MM/yyyy}", cuota.FechaVencimiento2)).Replace("[#Monto2]", cuota.GetVencimiento2);

        try
        {
            _mail.IsBodyHtml = true;
            _mail.Priority = MailPriority.Normal;

            AlternateView alternativaHTML = AlternateView.CreateAlternateViewFromString(messageBody, null, "text/html");
            _mail.AlternateViews.Add(alternativaHTML);

            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential(userName, password);
            _smtpMail.EnableSsl = true;

            if (_mailEmpresa != "")
            {
                _smtpMail.Send(_mail);
            }

            // Dispose
            _mail.Dispose();
        }
        catch (Exception ex)
        {
            string excepcion = ex.ToString();
        }
    }

    /**************************************************************************************************************************************************************/

    //Envio con los clientes autorizados para cargar ticket.
    public void CrearClienteAutorizadoTicket(string mailCliente, string nombreCliente, List<cCliente> clientes)
    {
        string _rutaURL = "http://www.naex.com.ar/crm/sistema/email/";
       // string _idPedido = pedido.IdEmpresa + "-" + pedido.Id;

        string _cuerpoClienteHTML = "";
        string _cuerpoSoporteHTML = "";

        MailMessage _mail = new MailMessage();
        //_mail.From = new MailAddress("NAEX - Sistema CRM <soporte@naex.com.ar>");
        _mail.From = new MailAddress("NAEX - Sistema CRM <ntabucchi@naex.com.ar>");
        
        _mail.To.Add(new MailAddress(mailCliente));

        //MailMessage _mailSoporte = new MailMessage("NAEX - Sistema CRM <soporte@naex.com.ar>", "NAEX - Sistema de CRM <soporte@naex.com.ar>");
        MailMessage _mailSoporte = new MailMessage("NAEX - Sistema CRM <ntabucchi@naex.com.ar>", "NAEX - Sistema de CRM <ntabucchi@naex.com.ar>");

        _cuerpoClienteHTML = this.CreateBodyHTML_StartTicketCliente_Cliente(_rutaURL, nombreCliente, clientes);
        _mail.Subject = "Cuenta no autorizada";
        
        try
        {
            // Mail Cliente
            _mail.IsBodyHtml = true;
            _mail.Priority = MailPriority.Normal;
            _mail.Body = _cuerpoClienteHTML;

            //Mail Soporte           
            _mailSoporte.IsBodyHtml = true;
            _mailSoporte.Priority = MailPriority.Normal;
            _mailSoporte.Body = _cuerpoSoporteHTML;

            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential(userName, password);
            _smtpMail.EnableSsl = true;

            System.Threading.Thread threadSendMails;
            threadSendMails = new System.Threading.Thread(delegate()
            {
                try
                {
                    _smtpMail.Send(_mail);
                }
                catch (System.Net.Mail.SmtpFailedRecipientException ex)
                {

                    throw ex;
                }
                catch (System.Net.Mail.SmtpException ex)
                {

                    throw ex;
                }
            });

            threadSendMails.IsBackground = true;
            threadSendMails.Start();          
        }
        catch (Exception ex)
        {
            string excepcion = ex.ToString();
        }
    }

    //Envio con los clientes autorizados para cargar ticket.
    public void CrearMailNoAutorizado(string mailCliente)
    {
        string _rutaURL = "http://www.naex.com.ar/crm/sistema/email/";
        // string _idPedido = pedido.IdEmpresa + "-" + pedido.Id;

        string _cuerpoClienteHTML = "";
        string _cuerpoSoporteHTML = "";

        MailMessage _mail = new MailMessage();
        //_mail.From = new MailAddress("NAEX - Sistema CRM <soporte@naex.com.ar>");
        _mail.From = new MailAddress("NAEX - Sistema CRM <ntabucchi@naex.com.ar>");
                
        _mail.To.Add(new MailAddress(mailCliente));

        //MailMessage _mailSoporte = new MailMessage("NAEX - Sistema CRM <soporte@naex.com.ar>", "NAEX - Sistema de CRM <soporte@naex.com.ar>");
        MailMessage _mailSoporte = new MailMessage("NAEX - Sistema CRM <ntabucchi@naex.com.ar>", "NAEX - Sistema de CRM <ntabucchi@naex.com.ar>");

        _cuerpoClienteHTML = this.CreateBodyHTML_MailInvalido(_rutaURL);
        _mail.Subject = "Cuenta no autorizada";

        try
        {
            // Mail Cliente
            _mail.IsBodyHtml = true;
            _mail.Priority = MailPriority.Normal;
            _mail.Body = _cuerpoClienteHTML;

            //Mail Soporte           
            _mailSoporte.IsBodyHtml = true;
            _mailSoporte.Priority = MailPriority.Normal;
            _mailSoporte.Body = _cuerpoSoporteHTML;

            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential(userName, password); 
            _smtpMail.EnableSsl = true;

            System.Threading.Thread threadSendMails;
            threadSendMails = new System.Threading.Thread(delegate()
            {
                try
                {
                    _smtpMail.Send(_mail);
                }
                catch (System.Net.Mail.SmtpFailedRecipientException ex)
                {

                    throw ex;
                }
                catch (System.Net.Mail.SmtpException ex)
                {

                    throw ex;
                }
            });

            threadSendMails.IsBackground = true;
            threadSendMails.Start();
        }
        catch (Exception ex)
        {
            string excepcion = ex.ToString();
        }
    }

    public void EnviarError(string clase, string metodo, string descripcion)
    {
        string _rutaURL = "http://www.naex.com.ar/crm/sistema/email/";
        MailMessage _mail = new MailMessage();
        _mail.From = new MailAddress("NAEX - Sistema CRM <info@haemprendimientos.com.ar>");
        //_mail.From = new MailAddress("NAEX - Sistema CRM <ntabucchi@naex.com.ar>");
        
        _mail.To.Add(new MailAddress("ntabucchi@naex.com.ar"));

        _mail.Subject = "Error CRM";
        string _cuerpo = this.CreateBodyHTML_NotificarError(_rutaURL, clase, metodo, descripcion);

        //Mail Cliente
        _mail.IsBodyHtml = true;
        _mail.Priority = MailPriority.Normal;

        //Vista alternativa en HTML
        AlternateView alternativaHTML = AlternateView.CreateAlternateViewFromString(_cuerpo, null, "text/html");
        _mail.AlternateViews.Add(alternativaHTML);

        try
        {
            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential(userName, password);
            _smtpMail.EnableSsl = true;

            _smtpMail.Send(_mail);

            // Dispose
            _mail.Dispose();
        }
        catch(Exception ex) { }
    }

    public void EnviarPedidosSemanal(List<cPedido> listaPedidosPendientes, List<cPedido> listaPedidosVencidos)
    {
        string _rutaURL = "http://www.naex.com.ar/crm/sistema/email/";
        MailMessage _mail = new MailMessage();
        //_mail.From = new MailAddress("NAEX - Sistema CRM <soporte@naex.com.ar>");
        _mail.From = new MailAddress("NAEX - Sistema CRM <ntabucchi@naex.com.ar>");

        _mail.To.Add(new MailAddress("soporte@naex.com.ar"));
        _mail.To.Add(new MailAddress("co@naex.com.ar"));
        _mail.To.Add(new MailAddress("nicolas.tabucchi@bethcom.com.ar"));

        _mail.Subject = "Tickets Pendientes y Vencidos";
        string _cuerpo = this.CreateBodyHTML_PedidosSemanal(_rutaURL, listaPedidosPendientes, listaPedidosVencidos);

        //Mail Cliente
        _mail.IsBodyHtml = true;
        _mail.Priority = MailPriority.Normal;

        //Vista alternativa en HTML
        AlternateView alternativaHTML = AlternateView.CreateAlternateViewFromString(_cuerpo, null, "text/html");
        _mail.AlternateViews.Add(alternativaHTML);

        try
        {
            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential(userName, password);
            _smtpMail.EnableSsl = true;

            _smtpMail.Send(_mail);

            // Dispose
            _mail.Dispose();
        }
        catch { }
    }
    
    private string CreateBodyHTML_NotificarError(string _rutaURL, string clase, string metodo, string descripcion)
    {
        string _cuerpo = "<!DOCTYPE HTML PUBLIC\"-//W3C//DTD HTML 4.0 Transitional//EN\">";
        _cuerpo += "<html>";
        _cuerpo += "<head>";
        _cuerpo += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">";
        _cuerpo += "<title>Soporte // NAEX</title>";
        _cuerpo += "<link rel=\"stylesheet\" href=\"" + _rutaURL + "estilo.css\" type=\"text/css\">";
        _cuerpo += "</head>";
        _cuerpo += "<center><body bgcolor=\"#FFFFFF\">";
        _cuerpo += "<table width=\"100%\" height=\"100%\" bgcolor=\"#FFFFFF\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpo += "<tr height=\"75px\"><td width=\"438px\" align=\"left\"><img src=\"" + _rutaURL + "imgs/encabezado.png?45\" border=\"0\" height=\"75\"></td>";
        _cuerpo += "<td></td>";
        _cuerpo += "</tr>";
        _cuerpo += "<tr><td colspan=\"2\" align=\"center\"><table width=\"96%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpo += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpo += "<tr><td height=\"100%\" class=\"textoEmail\" valign=\"top\"><b> ERROR: </b>";
        _cuerpo += "<br>Clase <b>" + clase + "</b>.";
        _cuerpo += "<br><br>Método: <b>" + metodo + "</b>.";
        _cuerpo += "<br><br>Descripción: <br>" + descripcion + "</td></tr>";
        _cuerpo += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpo += "</table></td></tr>";
        _cuerpo += "<tr height=\"11px\"><td align=\"left\"><img src=\"" + _rutaURL + "imgs/direccionPie.png\" border=\"0\" width=\"770px\" height=\"30\"></td></tr>";
        _cuerpo += "</table></body></center>";
        _cuerpo += "</html>";
        return _cuerpo;
    }
    
    //Correo que se envía con los pedidos pendientes y vencidos
    private string CreateBodyHTML_PedidosSemanal(string _rutaURL, List<cPedido> listaPedidosPendientes, List<cPedido> listaPedidosVencidos)
    {
        string _cuerpo = "<!DOCTYPE HTML PUBLIC\"-//W3C//DTD HTML 4.0 Transitional//EN\">";
        _cuerpo += "<html>";
        _cuerpo += "<head>";
        _cuerpo += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">";
        _cuerpo += "<title>Soporte // NAEX</title>";
        _cuerpo += "<link rel=\"stylesheet\" href=\"" + _rutaURL + "estilo.css\" type=\"text/css\">";
        _cuerpo += "</head>";
        _cuerpo += "<center><body bgcolor=\"#FFFFFF\">";

        _cuerpo += "<div align=\"left\"><img src=\"" + _rutaURL + "imgs/encabezado.png?45\" border=\"0\" style=\"width: 100%;\" height=\"75\"></div>";
        _cuerpo += "<br/>";

        _cuerpo += "<div align=\"left\" style=\"color: black;\"><b> Tickets Vencidos:</b>";

        if (listaPedidosVencidos.Count != 0)
        {
            _cuerpo += "<div>";
            _cuerpo += "<div><asp:ListView ID=\"lvPedidosVencidos\" runat=\"server\">";
            _cuerpo += "<LayoutTemplate>";
            _cuerpo += "<table border=\"0\" cellpadding=\"1\" style=\"width: 100%; background-color: #fff; margin-top: 6px; margin-bottom: -6px; border-radius: 3px; color: white\"><thead style=\"background-color: #98B429; border-bottom: 1px solid #dadada;\"><tr>";
            _cuerpo += "<td style=\"width: 63px; height: 20px; padding-left: 16px;\" class=\"column_head\">TICKET</td>";
            _cuerpo += "<td style=\"width: 209px; padding-left: 10px;\" class=\"column_head\">CLIENTE</td>";
            _cuerpo += "<td style=\"width: 113px; padding-left: 10px;\" class=\"column_head\">TELEFONO</td>";
            _cuerpo += "<td style=\"width: 303px; padding-left: 10px;\" class=\"column_head\">TITULO</td>";
            _cuerpo += "<td style=\"width: 116px; padding-left: 12px;\" class=\"column_head\">FECHA VENC.</td>";
            _cuerpo += "<td style=\"width: 140px; padding-left: 12px;\" class=\"column_head\">DEPTO.</td>";
            _cuerpo += "</tr></thead><tbody><asp:PlaceHolder runat=\"server\" ID=\"itemPlaceholder\"/></tbody></table>";
            _cuerpo += "</LayoutTemplate>";

            _cuerpo += "<ItemTemplate>";

            foreach (cPedido p in listaPedidosVencidos)
            {
                _cuerpo += "<tr style=\"background-color: #e9e9e9; color: #777;\">";
                _cuerpo += "<td style=\"width: 56px; height: 26px; padding-left: 22px; text-transform: uppercase; font-weight: bold; text-shadow: 0px 1px 0px #fff; \">" + p.Id + "</td>";
                _cuerpo += "<td style=\"width: 192px; padding-left: 10px;\">" + cCliente.Load(p.IdCliente).Nombre + " <b>(" + cEmpresa.Load(p.IdEmpresa).Nombre + ")</b></td>";
                _cuerpo += "<td style=\"width: 118px; padding-left: 10px;\">" + cEmpresa.Load(p.IdEmpresa).Telefono + "</td>";
                _cuerpo += "<td style=\"width: 272px; padding-left: 10px;\">" + p.Titulo + "</td>";
                _cuerpo += "<td style=\"width: 107px; padding-left: 10px;\">" + String.Format("{0:dd/MM/yyyy}", p.FechaRealizacion) + "</td>";
                _cuerpo += "<td style=\"width: 128px; padding-left: 10px;\">" + p.GetCategoria + "</td>";
            }

            _cuerpo += "</tr>";
            _cuerpo += "</ItemTemplate>";
            _cuerpo += "</asp:ListView></div>";
        }
        else
        {
            _cuerpo += " no hay registros<div>";
        }
        
        _cuerpo += "<br/>";        

        _cuerpo += "<div align=\"left\"><b> Tickets Pendientes:";

        if (listaPedidosPendientes.Count != 0)
        {
            _cuerpo += "</b><div>";

            _cuerpo += "<div><asp:ListView ID=\"lvPedidosPendientes\" runat=\"server\">";
            _cuerpo += "<LayoutTemplate>";
            _cuerpo += "<table border=\"0\" cellpadding=\"1\" style=\"width: 100%; background-color: #fff; margin-top: 6px; margin-bottom: -6px; border-radius: 3px; color: white\"><thead style=\"background-color: #98B429; border-bottom: 1px solid #dadada;\"><tr>";
            _cuerpo += "<td style=\"width: 63px; height: 20px; padding-left: 16px;\" class=\"column_head\">TICKET</td>";
            _cuerpo += "<td style=\"width: 208px; padding-left: 10px;\" class=\"column_head\">CLIENTE</td>";
            _cuerpo += "<td style=\"width: 131px; padding-left: 10px;\" class=\"column_head\">TELEFONO</td>";
            _cuerpo += "<td style=\"width: 314px; padding-left: 10px;\" class=\"column_head\">TITULO</td>";
            _cuerpo += "<td style=\"width: 110px; padding-left: 12px;\" class=\"column_head\">FECHA VENC.</td>";
            _cuerpo += "<td style=\"width: 140px; padding-left: 12px;\" class=\"column_head\">DEPTO.</td>";
            _cuerpo += "</tr></thead><tbody><asp:PlaceHolder runat=\"server\" ID=\"itemPlaceholder\"/></tbody></table>";
            _cuerpo += "</LayoutTemplate>";

            _cuerpo += "<ItemTemplate>";

            foreach (cPedido p in listaPedidosPendientes)
            {
                _cuerpo += "<tr style=\"background-color: #e9e9e9; color: #777;\">";
                _cuerpo += "<td style=\"width: 67px; height: 26px; padding-left: 22px; text-transform: uppercase; font-weight: bold; text-shadow: 0px 1px 0px #fff; \">" + p.Id + "</td>";
                _cuerpo += "<td style=\"width: 250px; padding-left: 10px;\">" + cCliente.Load(p.IdCliente).Nombre + " <b>(" + cEmpresa.Load(p.IdEmpresa).Nombre + ")</b></td>";
                _cuerpo += "<td style=\"width: 160px; padding-left: 10px;\">" + cEmpresa.Load(p.IdEmpresa).Telefono + "</td>";
                _cuerpo += "<td style=\"width: 390px; padding-left: 10px;\">" + p.Titulo + "</td>";
                _cuerpo += "<td style=\"width: 120px; padding-left: 10px;\">" + String.Format("{0:dd/MM/yyyy}", p.FechaRealizacion) + "</td>";
                _cuerpo += "<td style=\"width: 120px; padding-left: 10px;\">" + p.GetCategoria + "</td>";
            }

            _cuerpo += "</tr>";
            _cuerpo += "</ItemTemplate>";
            _cuerpo += "</asp:ListView></div>";
        }
        else
        {
            _cuerpo += " no hay registros<div>";
        }

        _cuerpo += "<br/>";
        _cuerpo += "<div align=\"left\"><img src=\"" + _rutaURL + "imgs/direccionPie.png\" border=\"0\" width=\"100%\" height=\"30\"></div>";
        _cuerpo += "</body></center>";
        _cuerpo += "</html>";
        return _cuerpo;
    }

    // Correo que se envía al Cliente para informarle los mails autorizados para crear tickets.
    private string CreateBodyHTML_StartTicketCliente_Cliente(string _rutaURL, string nombreCliente, List<cCliente> clientes)
    {
        string _cuerpoClienteHTML = "<!DOCTYPE HTML PUBLIC\"-//W3C//DTD HTML 4.0 Transitional//EN\">";
        _cuerpoClienteHTML += "<html>";
        _cuerpoClienteHTML += "<head>";
        _cuerpoClienteHTML += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">";
        _cuerpoClienteHTML += "<title>Soporte // NAEX</title>";
        _cuerpoClienteHTML += "<link rel=\"stylesheet\" href=\"" + _rutaURL + "estilo.css\" type=\"text/css\">";
        _cuerpoClienteHTML += "</head>";
        _cuerpoClienteHTML += "<center><body bgcolor=\"#FFFFFF\">";
        _cuerpoClienteHTML += "<table width=\"100%\" height=\"100%\" bgcolor=\"#FFFFFF\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoClienteHTML += "<tr><td align=\"left\"><img src=\"" + _rutaURL + "imgs/encabezado.png?45\" border=\"0\" width=\"825px\" height=\"75\"></td>";
        _cuerpoClienteHTML += "<td></td>";
        _cuerpoClienteHTML += "</tr>";
        _cuerpoClienteHTML += "<tr><td colspan=\"2\" align=\"center\"><table width=\"96%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoClienteHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";

        if (!string.IsNullOrEmpty(nombreCliente))
            _cuerpoClienteHTML += "<tr><td height=\"100%\" class=\"textoEmail\" valign=\"top\">" + nombreCliente + ", le enviamos este e-mail con el fin de informarle que el/los mail/s autorizados para crear tickets son los siguientes: ";
        else
            _cuerpoClienteHTML += "<tr><td height=\"100%\" class=\"textoEmail\" valign=\"top\"> Le enviamos este e-mail con el fin de informarle que el/los mail/s autorizados para crear tickets son los siguientes: ";

        _cuerpoClienteHTML += "<br>";
        foreach (cCliente c in clientes)
        {
            _cuerpoClienteHTML += "  <br><b>" + c.Nombre + ": </b>" + c.Mail;
        }
        _cuerpoClienteHTML += "<br><br>Ante cualquier duda, comuníquese con nosotros. <br>Muchas Gracias<br><p class=\"firmaEmail\">";
        _cuerpoClienteHTML += "<font style=\"font-style: oblique; font-size: 12px;\"><b>Importante:</b> este correo es informativo, por favor no responder a esta dirección de correo, ya que no se encuentra habilitada para recibir mensajes. </font><br><br>";    
        _cuerpoClienteHTML += "<b>Atte.<br>Soporte | Desarrollo<br><font color=\"#C6CF20\">NAEX</font> | Soluciones Informáticas<br>0810-888-<font color=\"#C6CF20\">NAEX</font>(6239) - soporte@naex.com.ar</b></td></tr>";
        _cuerpoClienteHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoClienteHTML += "</table></td></tr>";
        _cuerpoClienteHTML += "<tr height=\"11px\"><td align=\"left\"><img src=\"" + _rutaURL + "imgs/direccionPie.png\" border=\"0\" width=\"825px\" height=\"30\"></td></tr>";
        _cuerpoClienteHTML += "</table></body></center>";
        _cuerpoClienteHTML += "</html>";
        return _cuerpoClienteHTML;
    }

    //Correo que se envia como respuesta en el caso que el mail del cliente no esta autorizado para crear un ticket
    private string CreateBodyHTML_MailInvalido(string _rutaURL)
    {
        string _cuerpoClienteHTML = "<!DOCTYPE HTML PUBLIC\"-//W3C//DTD HTML 4.0 Transitional//EN\">";
        _cuerpoClienteHTML += "<html>";
        _cuerpoClienteHTML += "<head>";
        _cuerpoClienteHTML += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">";
        _cuerpoClienteHTML += "<title>Soporte // NAEX</title>";
        _cuerpoClienteHTML += "<link rel=\"stylesheet\" href=\"" + _rutaURL + "estilo.css\" type=\"text/css\">";
        _cuerpoClienteHTML += "</head>";
        _cuerpoClienteHTML += "<center><body bgcolor=\"#FFFFFF\">";
        _cuerpoClienteHTML += "<table width=\"100%\" height=\"100%\" bgcolor=\"#FFFFFF\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoClienteHTML += "<tr height=\"75px\"><td align=\"left\"><img src=\"" + _rutaURL + "imgs/encabezado.png?45\" border=\"0\" width=\"825px\" height=\"75\"></td>";
        _cuerpoClienteHTML += "<td></td>";
        _cuerpoClienteHTML += "</tr>";
        _cuerpoClienteHTML += "<tr><td colspan=\"2\" align=\"center\"><table width=\"96%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoClienteHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoClienteHTML += "<tr><td height=\"100%\" class=\"textoEmail\" valign=\"top\"> Le enviamos este e-mail con el fin de informarle que su cuenta de correo no esta autorizada para generar un nuevo ticket. ";
        _cuerpoClienteHTML += "<br><br> <font style=\"font-style: oblique; font-size: 12px;\"> <b>Importante:</b> este correo es informativo, por favor no responder a esta dirección de correo, ya que no se encuentra habilitada para recibir mensajes. </font>";
        _cuerpoClienteHTML += "<br><br>Ante cualquier duda, comuníquese con nosotros. <br>Muchas Gracias<br><br><p class=\"firmaEmail\"><b>Atte.<br>Soporte | Desarrollo<br><font color=\"#C6CF20\">NAEX</font> | Soluciones Informáticas<br>0810-888-<font color=\"#C6CF20\">NAEX</font>(6239) - soporte@naex.com.ar</b><br></td></tr>";
        _cuerpoClienteHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoClienteHTML += "</table></td></tr>";
        _cuerpoClienteHTML += "<tr height=\"11px\"><td align=\"left\"><img src=\"" + _rutaURL + "imgs/direccionPie.png\" border=\"0\" width=\"825px\" height=\"30\"></td></tr>";
        _cuerpoClienteHTML += "</table></body></center>";
        _cuerpoClienteHTML += "</html>";
        return _cuerpoClienteHTML;
    }

}
