using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using log4net;

public partial class Cliente : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try {
            lbEmpresa.Text = cEmpresa.GetNombreEmpresa(Request["idEmpresa"].ToString());
        }
        catch {
            Response.Redirect("Agenda.aspx", false);
        }

        try
        {
            if (!IsPostBack)
            {
                lvClientes.DataSource = cEmpresa.GetClientes(Request["idEmpresa"].ToString());
                lvClientes.DataBind();

                if (System.Convert.ToBoolean(Session["Agenda"]) != false)
                {
                    if (System.IO.File.Exists("C:\\crm info\\Clientes\\" + Request["idEmpresa"].ToString() + ".txt"))
                    {
                        FileStream fs = File.Open("C:\\crm info\\Clientes\\" + Request["idEmpresa"].ToString() + ".txt", FileMode.Open);
                        StreamReader sr = new StreamReader(fs);
                        string cadena = "";
                        cadena = sr.ReadToEnd();
                        htmlEditor.Content = cadena.Trim();
                        sr.Close();
                        htmlEditor.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex) {

            log4net.Config.XmlConfigurator.Configure();
            log.Error("Cliente - " + DateTime.Now + "- " + ex.Message + " - Page_Load" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    #region Edición
    protected void lvClientes_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        try
        {
            lvClientes.EditIndex = e.NewEditIndex;
            lvClientes.DataSource = cEmpresa.GetClientes(Request["idEmpresa"].ToString());
            lvClientes.DataBind();
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvClientes_ItemEditing" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void lvClientes_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        try
        {
            cCliente cliente = new cCliente();
            string id = cEmpresa.GetClientes(Request["idEmpresa"].ToString()).ToString();

            TextBox txt = (lvClientes.Items[e.ItemIndex].FindControl("txtEditId")) as TextBox;

            if (txt != null)
                cliente = cCliente.Load(txt.Text);

            if (txt != null)
                cliente.Id = txt.Text;

            txt = (lvClientes.Items[e.ItemIndex].FindControl("txtEditNombre")) as TextBox;
            if (txt != null)
                cliente.Nombre = txt.Text;

            txt = (lvClientes.Items[e.ItemIndex].FindControl("txtEditInterno")) as TextBox;
            if (txt != null)
                cliente.Interno = txt.Text;

            txt = (lvClientes.Items[e.ItemIndex].FindControl("txtEditMail")) as TextBox;
            if (txt != null)
                cliente.Mail = txt.Text;

            CheckBox ch = (lvClientes.Items[e.ItemIndex].FindControl("chEditAutorizacion")) as CheckBox;
            if (ch != null)
                cliente.Autorizacion = Convert.ToInt16(ch.Checked);

            txt = (lvClientes.Items[e.ItemIndex].FindControl("txtEditClave")) as TextBox;
            if (txt != null)
                cliente.ClaveSistema = cUsuario.Codify(txt.Text);

            cliente.Ip = cliente.Ip;
            cliente.Password = cliente.Password;
            cliente.UsuarioRed = cliente.UsuarioRed;
            cliente.PasswordRed = cliente.PasswordRed;
            cliente.Papelera = cliente.Papelera;

            cliente.Save();

            lvClientes.EditIndex = -1; //se cierra
            lvClientes.DataSource = cEmpresa.GetClientes(Request["idEmpresa"].ToString());
            lvClientes.DataBind();
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvClientes_ItemUpdating" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void lvClientes_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        try
        {
            cCliente cliente = new cCliente();

            TextBox txt = (lvClientes.Items[e.ItemIndex].FindControl("txtEditId")) as TextBox;

            if (txt != null)
                cliente = cCliente.Load(txt.Text);

            if (txt != null)
                cliente.Id = txt.Text;

            txt = (lvClientes.Items[e.ItemIndex].FindControl("txtEditNombre")) as TextBox;
            if (txt != null)
                cliente.Nombre = txt.Text;

            txt = (lvClientes.Items[e.ItemIndex].FindControl("txtEditInterno")) as TextBox;
            if (txt != null)
                cliente.Interno = txt.Text;

            txt = (lvClientes.Items[e.ItemIndex].FindControl("txtEditMail")) as TextBox;
            if (txt != null)
                cliente.Mail = txt.Text;

            txt = (lvClientes.Items[e.ItemIndex].FindControl("txtEditClave")) as TextBox;
            if (txt != null)
                cliente.ClaveSistema = txt.Text;

            cliente.Ip = cliente.Ip;
            cliente.Password = cliente.Password;
            cliente.UsuarioRed = cliente.UsuarioRed;
            cliente.PasswordRed = cliente.PasswordRed;
            cliente.Papelera = 0;

            cliente.Save();

            lvClientes.EditIndex = -1;
            lvClientes.DataSource = cEmpresa.GetClientes(Request["idEmpresa"].ToString());
            lvClientes.DataBind();
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvClientes_ItemDeleting" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void lvClientes_ItemCanceling(object sender, ListViewCancelEventArgs e)
    {
        try
        {
            lvClientes.EditIndex = -1;
            lvClientes.DataSource = cEmpresa.GetClientes(Request["idEmpresa"].ToString()); //cambiar
            lvClientes.DataBind();
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvClientes_ItemCanceling" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

    #region Insertar
    protected void lvClientes_ItemInserting(object sender, ListViewInsertEventArgs e) //Insertar un nuevo empresa
    {
        try
        {
            cCliente cliente = new cCliente();

            TextBox txt = (e.Item.FindControl("txtNombre")) as TextBox;
            if (txt != null)
                cliente.Nombre = txt.Text;

            txt = (e.Item.FindControl("txtInterno")) as TextBox;
            if (txt != null)
                cliente.Interno = txt.Text;

            txt = (e.Item.FindControl("txtMail")) as TextBox;
            if (txt != null)
                cliente.Mail = txt.Text;

            txt = (e.Item.FindControl("txtClave")) as TextBox;
            if (txt != null)
                cliente.ClaveSistema = cCliente.Codify(txt.Text);

            CheckBox ch = (e.Item.FindControl("chbAutorizacion")) as CheckBox;
            if (ch != null)
                cliente.Autorizacion = Convert.ToInt16(ch.Checked);

            cliente.IdEmpresa = Request["idEmpresa"].ToString();

            cliente.Ip = "-";
            cliente.Password = "-";
            cliente.UsuarioRed = "-";
            cliente.PasswordRed = "-";
            cliente.Papelera = 1;

            cliente.Save();

            lvClientes.EditIndex = -1;
            lvClientes.DataSource = cEmpresa.GetClientes(Request["idEmpresa"].ToString());
            lvClientes.DataBind();
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvClientes_ItemInserting" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

    #region Datos Técnicos
    protected void btnGuardarDatos_Click(object sender, EventArgs e) //Guarda datos técnicos en un txt
    {
        try
        {
            cEmpresa empresa = cEmpresa.Load(Request["idEmpresa"].ToString());

            empresa.Save();

            //Guarda en txt
            string fileName = "C:\\crm info\\Clientes\\" + empresa.Id + ".txt";
            FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

            StreamWriter datos = new StreamWriter(stream);
            datos.WriteLine(htmlEditor.Content);
            datos.Close();
        }
        catch (Exception ex)
        {

            log4net.Config.XmlConfigurator.Configure();
            log.Error("Cliente - " + DateTime.Now + "- " + ex.Message + " - btnGuardarDatos_Click");
            Response.Redirect("MensajeError.aspx");
        }

    }

    protected void btnIngresarDatos_Click(object sender, EventArgs e) //Carga datos técnicos de un txt
    {
        try {
            if (login.Value == "Crm2014")
            {
                lbErrorLog.Visible = false;

                string ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                cUsuario.InformationAccess(cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Id, cEmpresa.Load(Request["idEmpresa"].ToString()).Nombre, ip);

                try {
                    FileStream fs = File.Open("C:\\crm info\\Clientes\\" + Request["idEmpresa"].ToString() + ".txt", FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    string cadena = "";
                    cadena = sr.ReadToEnd();
                    htmlEditor.Content = cadena.Trim();
                    sr.Close();
                    htmlEditor.Visible = true;
                    Session.Add("Agenda", true);
                }
                catch
                {
                    htmlEditor.Visible = true;
                }
            }
            else
            {
                lbErrorLog.Visible = true;
                lbErrorLog.Text = "Contraseña inválida";
            }
        }
        catch (Exception ex)
        {

            log4net.Config.XmlConfigurator.Configure();
            log.Error("Cliente - " + DateTime.Now + "- " + ex.Message + " - btnIngresarDatos_Click");
            Response.Redirect("MensajeError.aspx");
            return;
        }
    }
    #endregion
    
}
