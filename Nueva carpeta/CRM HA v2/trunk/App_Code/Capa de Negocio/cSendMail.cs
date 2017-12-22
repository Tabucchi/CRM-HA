using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Text;

public class cSendMail
{
    public cSendMail(string idPedido, string idEmpresa, string mail, string nombreCliente, string nombreEmpresa, string titulo, string estado, string categoria)
    {
        string _rutaURL = "http://www.naex.com.ar/crm/sistema/email/";
        string _idPedido = idEmpresa + "-" + idPedido;
        string _cuerpoClienteHTML, _cuerpoClienteTXT, _cuerpoSoporteHTML, _cuerpoDesarrolloHTML = "";
        
        MailMessage _mail = new MailMessage();
         _mail.From = new MailAddress("NAEX - Sistema de Soporte <soporte@naex.com.ar>");
        
        string[] direcciones = mail.Split(';');
        foreach(string dir in direcciones)
            _mail.To.Add(new MailAddress(dir));      

        MailMessage _mailComunicaciones = new MailMessage("NAEX - Sistema de Soporte <soporte@naex.com.ar>", "NAEX - Telecomunicaciones <telecomunicaciones@naex.com.ar>"); 
        MailMessage _mailSoporte = new MailMessage("NAEX - Sistema de Soporte <soporte@naex.com.ar>", "NAEX - Sistema de CRM <soporte@naex.com.ar>");
        MailMessage _mailDesarrollo = new MailMessage("NAEX - Sistema de Soporte <soporte@naex.com.ar>", "NAEX - Desarrollo <desarrollo@naex.com.ar>");

        if (estado == "nuevo") {
            _cuerpoClienteHTML = this.CreateBodyHTML_StartTicket_Cliente(_rutaURL, _idPedido, titulo, nombreCliente);
            _cuerpoClienteTXT = this.CreateBodyTXT_StartTicket(_idPedido, titulo, nombreCliente);
            _cuerpoSoporteHTML = this.CreateBodyHTML_StartTicket_Soporte(_rutaURL, _idPedido, nombreCliente, nombreEmpresa, mail, titulo);
            _cuerpoDesarrolloHTML = this.CreateBodyHTML_StartTicket_Desarrollo(_rutaURL, _idPedido, nombreCliente, nombreEmpresa, mail, titulo);
            _mail.Subject = "Pedido ID " + _idPedido;
            _mailSoporte.Subject = nombreEmpresa + ": Pedido ID " + _idPedido;
            _mailDesarrollo.Subject = nombreEmpresa + ": Pedido ID " + _idPedido;
        }
        else {
            _cuerpoClienteHTML = this.CreateBodyHTML_FinishTicket_Cliente(_rutaURL, _idPedido, titulo, nombreCliente);
            _cuerpoClienteTXT = this.CreateBodyTXT_FinishTicket(_idPedido, titulo, nombreCliente);
            _cuerpoSoporteHTML = this.CreateBodyHTML_FinishTicket_Soporte(_rutaURL, _idPedido, nombreCliente, nombreEmpresa, mail, titulo);
            _cuerpoDesarrolloHTML = this.CreateBodyHTML_FinishTicket_Desarrollo(_rutaURL, _idPedido, nombreCliente, nombreEmpresa, mail, titulo);
            _mail.Subject = "Pedido ID " + _idPedido + " // FINALIZADO";
            _mailSoporte.Subject = nombreEmpresa + ": Pedido ID " + _idPedido + " //FINALIZADO";
            _mailDesarrollo.Subject = nombreEmpresa + ": Pedido ID " + _idPedido;
        }

        try
        {
            // Mail Cliente
            _mail.IsBodyHtml = true;
            _mail.Priority = MailPriority.Normal;
            //Vista alternativa en HTML
            AlternateView alternativaHTML = AlternateView.CreateAlternateViewFromString(_cuerpoClienteHTML, null, "text/html");
            _mail.AlternateViews.Add(alternativaHTML);
            _mail.Body = _cuerpoClienteTXT;

            //Mail Soporte           
            _mailSoporte.IsBodyHtml = true;
            _mailSoporte.Priority = MailPriority.Normal;
            _mailSoporte.Body = _cuerpoSoporteHTML;

            //Mail Desarrollo
            _mailDesarrollo.IsBodyHtml = true;
            _mailDesarrollo.Priority = MailPriority.Normal;
            _mailDesarrollo.Body = _cuerpoDesarrolloHTML;

            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential("crm@naex.com.ar", "crmnaex");
            _smtpMail.EnableSsl = true;

            if (mail != "")
            {
                _smtpMail.Send(_mail);
            }

            if (categoria == "Desarrollo")
                _smtpMail.Send(_mailDesarrollo);
            if (categoria == "Telecomunicaciones")
                _smtpMail.Send(_mailComunicaciones);
            if (categoria == "Soporte")
                _smtpMail.Send(_mailSoporte);
            

            // Dispose
            _mail.Dispose();
            _mailSoporte.Dispose();
            _mailDesarrollo.Dispose();
        }
        catch (Exception ex)
        {
            string excepcion = ex.ToString();
        }
    }

    public cSendMail(cPedido pedido, string mail, string mensaje)
    {
        string _rutaURL = "http://www.naex.com.ar/crm/sistema/email/";
        string _idPedido = pedido.IdEmpresa.ToString() + "-" + pedido.Id.ToString();

        MailMessage _mail = new MailMessage("NAEX - Sistema CRM <soporte@naex.com.ar>", mail);
        _mail.Subject = "Asignación de Pedido ID " + pedido.Id.ToString();

        cCliente _cliente = cCliente.Load(Convert.ToInt32(pedido.IdCliente));
        cEmpresa _empresa = cEmpresa.Load(Convert.ToInt32(pedido.IdEmpresa));
        string _cuerpo = this.CreateBodyHTML_AsignarResponsable(_rutaURL, _idPedido, _cliente.Nombre, _empresa.Nombre, _cliente.Mail, pedido.Titulo, mensaje);
        try
        {
            // Mail Cliente
            _mail.IsBodyHtml = true;
            _mail.Priority = MailPriority.Normal;
            //Vista alternativa en HTML
            AlternateView alternativaHTML = AlternateView.CreateAlternateViewFromString(_cuerpo, null, "text/html");
            _mail.AlternateViews.Add(alternativaHTML);

            SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
            _smtpMail.Credentials = new System.Net.NetworkCredential("crm@naex.com.ar", "crmnaex");
            _smtpMail.EnableSsl = true;

            if (mail != "")
                _smtpMail.Send(_mail);
            // Dispose
            _mail.Dispose();
        }
        catch (Exception ex)
        {
            string excepcion = ex.ToString();
        }
    }

    public cSendMail(cPedido pedido, ArrayList mailsUsuarios, string comentario, string nombreComento)
    {
        string _rutaURL = "http://www.naex.com.ar/crm/sistema/email/";
        string _idPedido = pedido.IdEmpresa.ToString() + "-" + pedido.Id.ToString();
        cEmpresa _empresa = cEmpresa.Load(Convert.ToInt32(pedido.IdEmpresa));
        for (int i = 0; mailsUsuarios.Count > i; i++) {
            try {
                cUsuario usuario = (cUsuario) mailsUsuarios[i];
                if (usuario.Mail != "") {
                    MailMessage _mail = new MailMessage("NAEX - Sistema CRM <soporte@naex.com.ar>", usuario.Mail);
                    _mail.Subject = "El Pedido " + _idPedido + " ha sido Comentado";
                    string _cuerpo = this.CreateBodyHTML_NotificarComentario(_rutaURL, _idPedido, _empresa.Nombre, comentario, pedido.Titulo, nombreComento);
                    // Mail Cliente
                    _mail.IsBodyHtml = true;
                    _mail.Priority = MailPriority.Normal;
                    //Vista alternativa en HTML
                    AlternateView alternativaHTML = AlternateView.CreateAlternateViewFromString(_cuerpo, null, "text/html");
                    _mail.AlternateViews.Add(alternativaHTML);

                    SmtpClient _smtpMail = new SmtpClient("smtp.gmail.com", 587);
                    _smtpMail.Credentials = new System.Net.NetworkCredential("crm@naex.com.ar", "crmnaex");
                    _smtpMail.EnableSsl = true;

                    _smtpMail.Send(_mail);
                    // Dispose
                    _mail.Dispose();
                }
            }
            catch { }
        }
    }


    private string CreateBodyHTML_StartTicket_Cliente(string _rutaURL, string idPedido, string titulo, string nombreCliente)
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
        _cuerpoClienteHTML += "<tr height=\"75px\" background=\"" + _rutaURL + "imgs/fondoEncabezado.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoEncabezado.gif);\"><td width=\"438px\" align=\"left\"><img src=\"" + _rutaURL + "imgs/encabezado.jpg?45\" border=\"0\" width=\"438\" height=\"75\"></td>";
        _cuerpoClienteHTML += "<td width=\"100%\"></td>";
        _cuerpoClienteHTML += "<td width=\"68px\" align=\"right\"><img src=\"" + _rutaURL + "imgs/tituloEncabezado.jpg\" border=\"0\" width=\"68\" height=\"75\"></td></tr>";
        _cuerpoClienteHTML += "<tr><td colspan=\"3\" align=\"center\"><table width=\"96%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoClienteHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoClienteHTML += "<tr><td height=\"100%\" class=\"textoEmail\" valign=\"top\">" + nombreCliente + ", le enviamos este e-mail con el fin de informarle que su pedido con titulo <b>" + titulo + "</b> fue cargado en el sistema.";
        _cuerpoClienteHTML += "<br>El ID del mismo es <b>" + idPedido + "</b>. Ya estamos trabajando en el.<br><br>En breve nos pondremos en contacto con usted.<br><br>Ante cualquier duda, comuníquese con nosotros. <br>Muchas Gracias<br><br><p class=\"firmaEmail\"><b>Atte.<br>Soporte | Desarrollo<br><font color=\"#C6CF20\">NAEX</font> | Soluciones Informáticas<br>0810-888-<font color=\"#C6CF20\">NAEX</font>(6239) - soporte@naex.com.ar</b><br></td></tr>";
        _cuerpoClienteHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoClienteHTML += "</table></td></tr>";
        _cuerpoClienteHTML += "<tr height=\"11px\" background=\"" + _rutaURL + "imgs/fondoPie.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoPie.gif);\"><td colspan=\"3\" align=\"left\"><a href=\"www.naex.com.ar\"><img src=\"" + _rutaURL + "imgs/direccionPie.gif\" border=\"0\" width=\"135\" height=\"11\"></a></td></tr>";
        _cuerpoClienteHTML += "</table></body></center>";
        _cuerpoClienteHTML += "</html>";
        return _cuerpoClienteHTML;
    }

    private string CreateBodyTXT_StartTicket(string idPedido, string titulo, string nombreCliente)
    {
        string _cuerpoClienteTXT = nombreCliente + ", le enviamos este e-mail con el efecto de informarle que fue cargado en el sistema su pedido con titulo \"" + titulo + "\".\n";
        _cuerpoClienteTXT += "El ID del mismo es " + idPedido + "</b>. Ya estamos trabajando en el.\n\nEn breve nos pondremos en contacto con usted.\n\n";
        _cuerpoClienteTXT += "Ante cualquier duda, comuníquese con nosotros.\n";
        _cuerpoClienteTXT += "Muchas Gracias\n\n";
        _cuerpoClienteTXT += "Atte.\nSoporte | Desarrollo\nNAEX | Soluciones Informáticas\n0810-888-NAEX(6239) - soporte@naex.com.ar\n";
        return _cuerpoClienteTXT;
    }

    private string CreateBodyHTML_StartTicket_Soporte(string _rutaURL, string idPedido, string nombreCliente, string nombreEmpresa, string mail, string titulo)
    {
        string _cuerpoSoporteHTML = "<!DOCTYPE HTML PUBLIC\"-//W3C//DTD HTML 4.0 Transitional//EN\">";
        _cuerpoSoporteHTML += "<html>";
        _cuerpoSoporteHTML += "<head>";
        _cuerpoSoporteHTML += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">";
        _cuerpoSoporteHTML += "<title>Soporte // NAEX</title>";
        _cuerpoSoporteHTML += "<link rel=\"stylesheet\" href=\"" + _rutaURL + "estilo.css\" type=\"text/css\">";
        _cuerpoSoporteHTML += "</head>";
        _cuerpoSoporteHTML += "<center><body bgcolor=\"#FFFFFF\">";
        _cuerpoSoporteHTML += "<table width=\"100%\" height=\"100%\" bgcolor=\"#FFFFFF\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoSoporteHTML += "<tr height=\"75px\" background=\"" + _rutaURL + "imgs/fondoEncabezado.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoEncabezado.gif);\"><td width=\"438px\" align=\"left\"><img src=\"" + _rutaURL + "imgs/encabezado.jpg?45\" border=\"0\" width=\"438\" height=\"75\"></td>";
        _cuerpoSoporteHTML += "<td width=\"100%\"></td>";
        _cuerpoSoporteHTML += "<td width=\"68px\" align=\"right\"><img src=\"" + _rutaURL + "imgs/tituloEncabezado.jpg\" border=\"0\" width=\"68\" height=\"75\"></td></tr>";
        _cuerpoSoporteHTML += "<tr><td colspan=\"3\" align=\"center\"><table width=\"96%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoSoporteHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoSoporteHTML += "<tr><td height=\"100%\" class=\"textoEmail\" valign=\"top\">Se cargo en el sistema un pedido con titulo <b>" + titulo + "</b>, del cliente <b>" + nombreEmpresa + "</b>, usuario <i>" + nombreCliente + "</i>. El email del cliente es <a href=\"mailto:" + mail + "\">" + mail + "</a>.";
        _cuerpoSoporteHTML += "<br>El ID del mismo es <b>" + idPedido + "</b>.<br><br>Para verlo ingresa al sistema: <a href=\"http://arst.naex.com.ar/CRM\">http://arst.naex.com.ar/CRM</a><br></td></tr>";
        _cuerpoSoporteHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoSoporteHTML += "</table></td></tr>";
        _cuerpoSoporteHTML += "<tr height=\"11px\" background=\"" + _rutaURL + "imgs/fondoPie.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoPie.gif);\"><td colspan=\"3\" align=\"left\"><a href=\"www.naex.com.ar\"><img src=\"" + _rutaURL + "imgs/direccionPie.gif\" border=\"0\" width=\"135\" height=\"11\"></a></td></tr>";
        _cuerpoSoporteHTML += "</table></body></center>";
        _cuerpoSoporteHTML += "</html>";
        return _cuerpoSoporteHTML;
    }

    private string CreateBodyHTML_FinishTicket_Cliente(string _rutaURL, string idPedido, string titulo, string nombreCliente)
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
        _cuerpoClienteHTML += "<tr height=\"75px\" background=\"" + _rutaURL + "imgs/fondoEncabezado.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoEncabezado.gif);\"><td width=\"438px\" align=\"left\"><img src=\"" + _rutaURL + "imgs/encabezado.jpg?45\" border=\"0\" width=\"438\" height=\"75\"></td>";
        _cuerpoClienteHTML += "<td width=\"100%\"></td>";
        _cuerpoClienteHTML += "<td width=\"68px\" align=\"right\"><img src=\"" + _rutaURL + "imgs/tituloEncabezado.jpg\" border=\"0\" width=\"68\" height=\"75\"></td></tr>";
        _cuerpoClienteHTML += "<tr><td colspan=\"3\" align=\"center\"><table width=\"96%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoClienteHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoClienteHTML += "<tr><td height=\"100%\" class=\"textoEmail\" valign=\"top\">" + "Su pedido ID <b>" + idPedido + "</b> con titulo <b>" + titulo + "</b>, ha sido finalizado.";
        _cuerpoClienteHTML += "<br>Ante cualquier consulta, no dude en comunicarse con nosotros y con el numero de pedido, podra consultar los detalles del mismo.<br><br> <p class=\"firmaEmail\"><b>Atte.<br>Soporte | Desarrollo<br><font color=\"#C6CF20\">NAEX</font> | Soluciones Informáticas<br>0810-888-<font color=\"#C6CF20\">NAEX</font>(6239) - soporte@naex.com.ar</b><br></td></tr>";

        _cuerpoClienteHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoClienteHTML += "</table></td></tr>";
        _cuerpoClienteHTML += "<tr height=\"11px\" background=\"" + _rutaURL + "imgs/fondoPie.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoPie.gif);\"><td colspan=\"3\" align=\"left\"><a href=\"www.naex.com.ar\"><img src=\"" + _rutaURL + "imgs/direccionPie.gif\" border=\"0\" width=\"135\" height=\"11\"></a></td></tr>";
        _cuerpoClienteHTML += "</table></body></center>";
        _cuerpoClienteHTML += "</html>";
        return _cuerpoClienteHTML;
    }

    private string CreateBodyTXT_FinishTicket(string idPedido, string titulo, string nombreCliente)
    {
        string _cuerpoClienteTXT = "Su pedido ID " + idPedido + " con titulo " + titulo + ", ha sido finalizado.\n";
        _cuerpoClienteTXT += "Ante cualquier duda, comuníquese con nosotros y con el numero de pedido, podra consultar los detalles del mismo.\n";
        _cuerpoClienteTXT += "Muchas Gracias\n\n";
        _cuerpoClienteTXT += "Atte.\nSoporte | Desarrollo\nNAEX | Soluciones Informáticas\n0810-888-NAEX(6239) - soporte@naex.com.ar\n";
        return _cuerpoClienteTXT;
    }

    // Finalizar Ticket Soporte
    private string CreateBodyHTML_FinishTicket_Soporte(string _rutaURL, string idPedido, string nombreCliente, string nombreEmpresa, string mail, string titulo)
    {
        string _cuerpoSoporteHTML = "<!DOCTYPE HTML PUBLIC\"-//W3C//DTD HTML 4.0 Transitional//EN\">";
        _cuerpoSoporteHTML += "<html>";
        _cuerpoSoporteHTML += "<head>";
        _cuerpoSoporteHTML += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">";
        _cuerpoSoporteHTML += "<title>Soporte // NAEX</title>";
        _cuerpoSoporteHTML += "<link rel=\"stylesheet\" href=\"" + _rutaURL + "estilo.css\" type=\"text/css\">";
        _cuerpoSoporteHTML += "</head>";
        _cuerpoSoporteHTML += "<center><body bgcolor=\"#FFFFFF\">";
        _cuerpoSoporteHTML += "<table width=\"100%\" height=\"100%\" bgcolor=\"#FFFFFF\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoSoporteHTML += "<tr height=\"75px\" background=\"" + _rutaURL + "imgs/fondoEncabezado.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoEncabezado.gif);\"><td width=\"438px\" align=\"left\"><img src=\"" + _rutaURL + "imgs/encabezado.jpg?45\" border=\"0\" width=\"438\" height=\"75\"></td>";
        _cuerpoSoporteHTML += "<td width=\"100%\"></td>";
        _cuerpoSoporteHTML += "<td width=\"68px\" align=\"right\"><img src=\"" + _rutaURL + "imgs/tituloEncabezado.jpg\" border=\"0\" width=\"68\" height=\"75\"></td></tr>";
        _cuerpoSoporteHTML += "<tr><td colspan=\"3\" align=\"center\"><table width=\"96%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoSoporteHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoSoporteHTML += "<tr><td height=\"100%\" class=\"textoEmail\" valign=\"top\">Se finalizó en el sistema un pedido con titulo <b>" + titulo + "</b>, del cliente <b>" + nombreEmpresa + "</b>, usuario <i>" + nombreCliente + "</i>. El email del cliente es <a href=\"mailto:" + mail + "\">" + mail + "</a>.";
        _cuerpoSoporteHTML += "<br>El ID del mismo es <b>" + idPedido + "</b>.<br><br>Para verlo ingresa al sistema: <a href=\"http://arst.naex.com.ar/CRM\">http://arst.naex.com.ar/CRM</a><br></td></tr>";
        _cuerpoSoporteHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoSoporteHTML += "</table></td></tr>";
        _cuerpoSoporteHTML += "<tr height=\"11px\" background=\"" + _rutaURL + "imgs/fondoPie.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoPie.gif);\"><td colspan=\"3\" align=\"left\"><a href=\"www.naex.com.ar\"><img src=\"" + _rutaURL + "imgs/direccionPie.gif\" border=\"0\" width=\"135\" height=\"11\"></a></td></tr>";
        _cuerpoSoporteHTML += "</table></body></center>";
        _cuerpoSoporteHTML += "</html>";
        return _cuerpoSoporteHTML;
    }

    //Crear Ticket Desarrollo
    private string CreateBodyHTML_StartTicket_Desarrollo(string _rutaURL, string idPedido, string nombreCliente, string nombreEmpresa, string mail, string titulo)
    {
        string _cuerpoDesarrolloHTML = "<!DOCTYPE HTML PUBLIC\"-//W3C//DTD HTML 4.0 Transitional//EN\">";
        _cuerpoDesarrolloHTML += "<html>";
        _cuerpoDesarrolloHTML += "<head>";
        _cuerpoDesarrolloHTML += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">";
        _cuerpoDesarrolloHTML += "<title>Soporte // NAEX</title>";
        _cuerpoDesarrolloHTML += "<link rel=\"stylesheet\" href=\"" + _rutaURL + "estilo.css\" type=\"text/css\">";
        _cuerpoDesarrolloHTML += "</head>";
        _cuerpoDesarrolloHTML += "<center><body bgcolor=\"#FFFFFF\">";
        _cuerpoDesarrolloHTML += "<table width=\"100%\" height=\"100%\" bgcolor=\"#FFFFFF\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoDesarrolloHTML += "<tr height=\"75px\" background=\"" + _rutaURL + "imgs/fondoEncabezado.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoEncabezado.gif);\"><td width=\"438px\" align=\"left\"><img src=\"" + _rutaURL + "imgs/encabezado.jpg?45\" border=\"0\" width=\"438\" height=\"75\"></td>";
        _cuerpoDesarrolloHTML += "<td width=\"100%\"></td>";
        _cuerpoDesarrolloHTML += "<td width=\"68px\" align=\"right\"><img src=\"" + _rutaURL + "imgs/tituloEncabezado.jpg\" border=\"0\" width=\"68\" height=\"75\"></td></tr>";
        _cuerpoDesarrolloHTML += "<tr><td colspan=\"3\" align=\"center\"><table width=\"96%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoDesarrolloHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoDesarrolloHTML += "<tr><td height=\"100%\" class=\"textoEmail\" valign=\"top\">Se cargo en el sistema un pedido con titulo <b>" + titulo + "</b>, del cliente <b>" + nombreEmpresa + "</b>, usuario <i>" + nombreCliente + "</i>. El email del cliente es <a href=\"mailto:" + mail + "\">" + mail + "</a>.";
        _cuerpoDesarrolloHTML += "<br>El ID del mismo es <b>" + idPedido + "</b>.<br><br>Para verlo ingresa al sistema: <a href=\"http://arst.naex.com.ar/CRM\">http://arst.naex.com.ar/CRM</a><br></td></tr>";
        _cuerpoDesarrolloHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoDesarrolloHTML += "</table></td></tr>";
        _cuerpoDesarrolloHTML += "<tr height=\"11px\" background=\"" + _rutaURL + "imgs/fondoPie.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoPie.gif);\"><td colspan=\"3\" align=\"left\"><a href=\"www.naex.com.ar\"><img src=\"" + _rutaURL + "imgs/direccionPie.gif\" border=\"0\" width=\"135\" height=\"11\"></a></td></tr>";
        _cuerpoDesarrolloHTML += "</table></body></center>";
        _cuerpoDesarrolloHTML += "</html>";
        return _cuerpoDesarrolloHTML;
    }


    //Finalizar Ticket Desarrollo
    private string CreateBodyHTML_FinishTicket_Desarrollo(string _rutaURL, string idPedido, string nombreCliente, string nombreEmpresa, string mail, string titulo)
    {
        string _cuerpoDesarrolloHTML = "<!DOCTYPE HTML PUBLIC\"-//W3C//DTD HTML 4.0 Transitional//EN\">";
        _cuerpoDesarrolloHTML += "<html>";
        _cuerpoDesarrolloHTML += "<head>";
        _cuerpoDesarrolloHTML += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">";
        _cuerpoDesarrolloHTML += "<title>Soporte // NAEX</title>";
        _cuerpoDesarrolloHTML += "<link rel=\"stylesheet\" href=\"" + _rutaURL + "estilo.css\" type=\"text/css\">";
        _cuerpoDesarrolloHTML += "</head>";
        _cuerpoDesarrolloHTML += "<center><body bgcolor=\"#FFFFFF\">";
        _cuerpoDesarrolloHTML += "<table width=\"100%\" height=\"100%\" bgcolor=\"#FFFFFF\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoDesarrolloHTML += "<tr height=\"75px\" background=\"" + _rutaURL + "imgs/fondoEncabezado.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoEncabezado.gif);\"><td width=\"438px\" align=\"left\"><img src=\"" + _rutaURL + "imgs/encabezado.jpg?45\" border=\"0\" width=\"438\" height=\"75\"></td>";
        _cuerpoDesarrolloHTML += "<td width=\"100%\"></td>";
        _cuerpoDesarrolloHTML += "<td width=\"68px\" align=\"right\"><img src=\"" + _rutaURL + "imgs/tituloEncabezado.jpg\" border=\"0\" width=\"68\" height=\"75\"></td></tr>";
        _cuerpoDesarrolloHTML += "<tr><td colspan=\"3\" align=\"center\"><table width=\"96%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpoDesarrolloHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoDesarrolloHTML += "<tr><td height=\"100%\" class=\"textoEmail\" valign=\"top\">Se finalizó en el sistema un pedido con titulo <b>" + titulo + "</b>, del cliente <b>" + nombreEmpresa + "</b>, usuario <i>" + nombreCliente + "</i>. El email del cliente es <a href=\"mailto:" + mail + "\">" + mail + "</a>.";
        _cuerpoDesarrolloHTML += "<br>El ID del mismo es <b>" + idPedido + "</b>.<br><br>Para verlo ingresa al sistema: <a href=\"http://arst.naex.com.ar/CRM\">http://arst.naex.com.ar/CRM</a><br></td></tr>";
        _cuerpoDesarrolloHTML += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpoDesarrolloHTML += "</table></td></tr>";
        _cuerpoDesarrolloHTML += "<tr height=\"11px\" background=\"" + _rutaURL + "imgs/fondoPie.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoPie.gif);\"><td colspan=\"3\" align=\"left\"><a href=\"www.naex.com.ar\"><img src=\"" + _rutaURL + "imgs/direccionPie.gif\" border=\"0\" width=\"135\" height=\"11\"></a></td></tr>";
        _cuerpoDesarrolloHTML += "</table></body></center>";
        _cuerpoDesarrolloHTML += "</html>";
        return _cuerpoDesarrolloHTML;
    }

    private string CreateBodyHTML_AsignarResponsable(string _rutaURL, string idPedido, string nombreCliente, string nombreEmpresa, string mail, string titulo, string mensaje)
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
        _cuerpo += "<tr height=\"75px\" background=\"" + _rutaURL + "imgs/fondoEncabezado.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoEncabezado.gif);\"><td width=\"438px\" align=\"left\"><img src=\"" + _rutaURL + "imgs/encabezado.jpg?45\" border=\"0\" width=\"438\" height=\"75\"></td>";
        _cuerpo += "<td width=\"100%\"></td>";
        _cuerpo += "<td width=\"68px\" align=\"right\"><img src=\"" + _rutaURL + "imgs/tituloEncabezado.jpg\" border=\"0\" width=\"68\" height=\"75\"></td></tr>";
        _cuerpo += "<tr><td colspan=\"3\" align=\"center\"><table width=\"96%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpo += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpo += "<tr><td height=\"100%\" class=\"textoEmail\" valign=\"top\">El pedido con titulo <b>" + titulo + "</b>, del cliente <b>" + nombreEmpresa + "</b>, usuario <i>" + nombreCliente + "</i>, te ha sido asignado. </a>";
        _cuerpo += "<br>El E-Mail del cliente es <a href=\"mailto:" + mail + "\">" + mail + "</a>.";
        _cuerpo += "<br>El ID del mismo es <b>" + idPedido + "</b>.";
        _cuerpo += "<br><br>Comentario: <b>" + mensaje + "</b>.";
        _cuerpo += "<br><br>Para verlo ingresa al sistema: <a href=\"http://arst.naex.com.ar/CRM\">http://arst.naex.com.ar/CRM</a><br></td></tr>";
        _cuerpo += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpo += "</table></td></tr>";
        _cuerpo += "<tr height=\"11px\" background=\"" + _rutaURL + "imgs/fondoPie.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoPie.gif);\"><td colspan=\"3\" align=\"left\"><a href=\"www.naex.com.ar\"><img src=\"" + _rutaURL + "imgs/direccionPie.gif\" border=\"0\" width=\"135\" height=\"11\"></a></td></tr>";
        _cuerpo += "</table></body></center>";
        _cuerpo += "</html>";
        return _cuerpo;
    }

    private string CreateBodyHTML_NotificarComentario(string _rutaURL, string idPedido, string nombreEmpresa, string comentario, string titulo, string nombreComento)
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
        _cuerpo += "<tr height=\"75px\" background=\"" + _rutaURL + "imgs/fondoEncabezado.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoEncabezado.gif);\"><td width=\"438px\" align=\"left\"><img src=\"" + _rutaURL + "imgs/encabezado.jpg?45\" border=\"0\" width=\"438\" height=\"75\"></td>";
        _cuerpo += "<td width=\"100%\"></td>";
        _cuerpo += "<td width=\"68px\" align=\"right\"><img src=\"" + _rutaURL + "imgs/tituloEncabezado.jpg\" border=\"0\" width=\"68\" height=\"75\"></td></tr>";
        _cuerpo += "<tr><td colspan=\"3\" align=\"center\"><table width=\"96%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
        _cuerpo += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpo += "<tr><td height=\"100%\" class=\"textoEmail\" valign=\"top\"><b>" + nombreComento + "</b> ha ingresado un comentario en el pedido del cliente <b>" + nombreEmpresa + "</b> con titulo <b>" + titulo + "</b>.</a>";
        _cuerpo += "<br>El ID del mismo es <b>" + idPedido + "</b>.";
        _cuerpo += "<br><br>Comentario: <b>" + comentario + "</b>.";
        _cuerpo += "<br><br>Para verlo ingresa al sistema: <a href=\"http://arst.naex.com.ar/CRM\">http://arst.naex.com.ar/CRM</a><br></td></tr>";
        _cuerpo += "<tr><td height=\"20\">&nbsp;</td></tr>";
        _cuerpo += "</table></td></tr>";
        _cuerpo += "<tr height=\"11px\" background=\"" + _rutaURL + "imgs/fondoPie.gif\" style=\"BACKGROUND-IMAGE: url(" + _rutaURL + "imgs/fondoPie.gif);\"><td colspan=\"3\" align=\"left\"><a href=\"www.naex.com.ar\"><img src=\"" + _rutaURL + "imgs/direccionPie.gif\" border=\"0\" width=\"135\" height=\"11\"></a></td></tr>";
        _cuerpo += "</table></body></center>";
        _cuerpo += "</html>";
        return _cuerpo;
    }
}
