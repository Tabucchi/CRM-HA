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
            }
        }
        catch (Exception ex) {

            log4net.Config.XmlConfigurator.Configure();
            log.Error("Cliente - " + DateTime.Now + "- " + ex.Message + " - Page_Load" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
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
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvClientes_ItemEditing" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
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

            cliente.Autorizacion = 0;
            cliente.ClaveSistema = cUsuario.Codify("null");

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
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvClientes_ItemUpdating" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
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

            cliente.Autorizacion = 0;
            cliente.ClaveSistema = "null";
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
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvClientes_ItemDeleting" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
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
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvClientes_ItemCanceling" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
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

            cliente.ClaveSistema = cCliente.Codify("null");
            cliente.Autorizacion = 0;

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
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvClientes_ItemInserting" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

}
