using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

public class cSendMailPostVenta
{
    public cSendMailPostVenta() { }

    #region Credenciales
    //string userName = "ntabucchi@naex.com.ar";
    //string password = "lavalle557";

    string userName = "info@haemprendimientos.com.ar";
    string password = "Naex2017";

    //string userName = "soporte@naex.com.ar";
    //string password = "Soporte2016";
    #endregion


    private static string _NuevaConsulta;

    public void NuevaConsulta(string obra, string uf, string cliente, string mail, string telefono, string descripcion)
    {
        string messageBody = "";

        MailMessage _mail = new MailMessage();
        //_mail.From = new MailAddress("NAEX - Sistema CRM <nicolas.tabucchi@bethcom.com.ar>");
        _mail.From = new MailAddress("HA Emprendimientos - Sistema CRM <info@haemprendimientos.com.ar>");

        _mail.To.Add(new MailAddress("mjamoedo@haemprendimientos.com.ar"));
        _mail.To.Add(new MailAddress("esampaolesi@haemprendimientos.com.ar"));
        _mail.To.Add(new MailAddress("tmonsegur@haemprendimientos.com.ar"));
        _mail.To.Add(new MailAddress("info@haemprendimientos.com.ar"));
        //_mail.To.Add(new MailAddress("ntabucchi@naex.com.ar"));

        _mail.Subject = "Nuevo Consulta";

        StreamReader archivo = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplate\\NuevaConsulta.html"));
        _NuevaConsulta = archivo.ReadToEnd();
        archivo.Close();

        messageBody = _NuevaConsulta.Replace("[#Cliente]", cliente).Replace("[#Descripcion]", descripcion).Replace("[#Obra]", obra)
            .Replace("[#UnidadFuncional]", uf)
            .Replace("[#Mail]", mail)
            .Replace("[#Telefono]", telefono);

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
}

