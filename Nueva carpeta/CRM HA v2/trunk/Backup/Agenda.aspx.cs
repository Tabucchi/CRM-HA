using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using log4net;

public partial class Agenda : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                cbEmpresa.DataSource = cEmpresa.GetEmpresas();
                cbEmpresa.DataValueField = "id";
                cbEmpresa.DataTextField = "nombre";
                cbEmpresa.DataBind();

                lvEmpresas.DataSource = cEmpresa.GetEmpresas();
                lvEmpresas.DataBind();
            }
        }catch(Exception ex){
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - ListPager_PreRender" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

   //OnPreRender(método): indica al control de servidor que realice los pasos previos a la representación que sean necesarios antes de guardar el estado de vista y el contenido de la representación.
    protected void ListPager_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (Request["idEmpresa"] == null)
            {
                lvEmpresas.DataSource = cEmpresa.GetEmpresas();
                lvEmpresas.DataBind();
            }
            else
            {
                lvEmpresas.DataSource = cEmpresa.Search(Convert.ToString(Request["idEmpresa"]));
                lvEmpresas.DataBind();
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - ListPager_PreRender" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Agenda.aspx?idEmpresa=" + cbEmpresa.SelectedValue);
    }

    #region Metodos de Edición en ListView
    protected void lvEmpresas_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        try
        {
            lvEmpresas.DataSource = cEmpresa.Search(Convert.ToString(Request["idEmpresa"]));
            lvEmpresas.DataBind();
            lvEmpresas.EditIndex = e.NewEditIndex;
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvEmpresas_ItemEditing" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void lvEmpresas_ItemUpdating(object sender, ListViewUpdateEventArgs e) //Actualiza datos de empresa
    {
        try
        {
            cEmpresa empresa = new cEmpresa();

            TextBox txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditId")) as TextBox;

            if (txt != null)
                empresa = cEmpresa.Load(txt.Text);

            if (txt != null)
                empresa.Id = txt.Text;

            txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditNombre")) as TextBox;
            if (txt != null)
                empresa.Nombre = txt.Text;

            txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditDireccion")) as TextBox;
            if (txt != null)
                empresa.Direccion = txt.Text;

            txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditTelefono")) as TextBox;
            if (txt != null)
                empresa.Telefono = txt.Text;

            txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditCuit")) as TextBox;
            if (txt != null)
                empresa.Cuit = txt.Text;

            txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditDominio")) as TextBox;
            if (txt != null)
                empresa.DominioMail = txt.Text;

            empresa.Datos = empresa.Datos;
            empresa.Papelera = empresa.Papelera;

            empresa.Save();

            lvEmpresas.EditIndex = -1;
            lvEmpresas.DataSource = cEmpresa.GetEmpresas();
            lvEmpresas.DataBind();
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvEmpresas_ItemUpdating" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }
      
    protected void lvEmpresas_ItemCanceling(object sender, ListViewCancelEventArgs e) //Boton Cancelar
    {
        try
        {
            lvEmpresas.EditIndex = -1;
            if (Request["idEmpresa"] == null)
            {
                lvEmpresas.DataSource = cEmpresa.GetEmpresas();
                lvEmpresas.DataBind();
            }
            else
            {
                lvEmpresas.DataSource = cEmpresa.Search(Convert.ToString(Request["idEmpresa"]));
                lvEmpresas.DataBind();
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvEmpresas_ItemCanceling" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void lvEmpresas_ItemDeleting(object sender, ListViewDeleteEventArgs e) //Cambia el estado del campo papelera, lo pasa a 0 (no activo)
    {
        cEmpresa empresa = new cEmpresa();

        TextBox txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditId")) as TextBox;
        if (txt != null)
            empresa = cEmpresa.Load(txt.Text);

        if (txt != null)
            empresa.Id = txt.Text;

        txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditNombre")) as TextBox;
        if (txt != null)
            empresa.Nombre = txt.Text;

        txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditDireccion")) as TextBox;
        if (txt != null)
            empresa.Direccion = txt.Text;

        txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditTelefono")) as TextBox;
        if (txt != null)
            empresa.Telefono = txt.Text;

        txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditCuit")) as TextBox;
        if (txt != null)
            empresa.Cuit = txt.Text;

        empresa.Datos = empresa.Datos;
        empresa.Papelera = 0;

        empresa.Save();

        lvEmpresas.EditIndex = -1;
        Response.Redirect("Agenda.aspx");
        lvEmpresas.DataSource = cEmpresa.GetEmpresas();
        lvEmpresas.DataBind();        
    }
    #endregion

    #region Metodo Insertar en ListView
    protected void lvEmpresas_ItemInserting(object sender, ListViewInsertEventArgs e) //Insertar una nueva empresa
    {
        try
        {
            cEmpresa empresa = new cEmpresa();

            TextBox txt = (e.Item.FindControl("txtNombre")) as TextBox;
            if (txt != null)
                empresa.Nombre = txt.Text;

            txt = (e.Item.FindControl("txtDireccion")) as TextBox;
            if (txt != null)
                empresa.Direccion = txt.Text;

            txt = (e.Item.FindControl("txtTelefono")) as TextBox;
            if (txt != null)
                empresa.Telefono = txt.Text;

            txt = (e.Item.FindControl("txtCuit")) as TextBox;
            if (txt != null)
                empresa.Cuit = txt.Text;

            txt = (e.Item.FindControl("txtDominio")) as TextBox;
            if (txt != null)
                empresa.DominioMail = txt.Text;

            empresa.Datos = "-"; //si se deja en null, aparece una excepcion de sql
            empresa.Papelera = 1;

            empresa.Save();

            lvEmpresas.EditIndex = -1;
            lvEmpresas.DataSource = cEmpresa.GetEmpresas();
            lvEmpresas.DataBind();
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvEmpresas_ItemInserting" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

    protected void lkbExcelReporte_Click(object sender, EventArgs e)
    {
        DataTable table = new DataTable();

        table = cPedido.ObtenerReporteSemanalXLS();

        string tab = "";

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=Pedidos.xls");
        Response.ContentType = "application/vnd.xls";
        Response.Charset = "";

        foreach (DataColumn dc in table.Columns)
        {
            Response.Write(tab + dc.ColumnName);
            tab = "\t";
        }

        Response.Write("\n");

        int i = 0;
        string auxTelefono = null;
        foreach (DataRow dr in table.Rows)
        {
            tab = "";

            string f = dr[2].ToString();
            int flag = String.Compare(auxTelefono, dr[2].ToString(), true);

            for (i = 0; i < table.Columns.Count; i++)
            {
                try
                {
                    if (i != 2)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    else
                    {
                        if (flag == -1)
                            Response.Write(tab + dr[2].ToString());
                        else
                            Response.Write(tab + "");
                        tab = "\t";
                    }
                }
                catch (Exception ex)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lkbExcelReporte_Click" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
                    return;
                }
            }

            auxTelefono = dr[2].ToString();                
            Response.Write("\n");
        }
        Response.End();
    }
}
