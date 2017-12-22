using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Proveedor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cbProveedor.DataSource = cProveedor.GetProveedores();
            cbProveedor.DataValueField = "id";
            cbProveedor.DataTextField = "nombre";
            cbProveedor.DataBind();
            
            lvProveedores.DataSource = cProveedor.GetProveedores();
            lvProveedores.DataBind();
        }
    }

    protected void ListPager_PreRender(object sender, EventArgs e)
    {
        if (Request["idProveedor"] == null)
        {
            lvProveedores.DataSource = cProveedor.GetProveedores();
            lvProveedores.DataBind();
        }
        else
        {
            lvProveedores.DataSource = cProveedor.Search(Convert.ToString(Request["idProveedor"]));
            lvProveedores.DataBind();
        }
    }

    #region Metodos de Edición en ListView
    protected void lvProveedores_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        lvProveedores.DataSource = cProveedor.Search(Convert.ToString(Request["idProveedor"]));
        lvProveedores.DataBind();
        lvProveedores.EditIndex = e.NewEditIndex;
    }

    protected void lvProveedores_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        cProveedor empresa = new cProveedor();

        TextBox txt = (lvProveedores.Items[e.ItemIndex].FindControl("txtEditId")) as TextBox;

        if (txt != null)
            empresa = cProveedor.Load(txt.Text);

        if (txt != null)
            empresa.Id = txt.Text;

        txt = (lvProveedores.Items[e.ItemIndex].FindControl("txtEditNombre")) as TextBox;
        if (txt != null)
            empresa.Nombre = txt.Text;

        txt = (lvProveedores.Items[e.ItemIndex].FindControl("txtEditDireccion")) as TextBox;
        if (txt != null)
            empresa.Direccion = txt.Text;

        txt = (lvProveedores.Items[e.ItemIndex].FindControl("txtEditTelefono")) as TextBox;
        if (txt != null)
            empresa.Telefono = txt.Text;

        txt = (lvProveedores.Items[e.ItemIndex].FindControl("txtEditMail")) as TextBox;
        if (txt != null)
            empresa.Mail = txt.Text;

        empresa.Papelera = empresa.Papelera;
        empresa.Save();

        lvProveedores.EditIndex = -1;
        lvProveedores.DataSource = cProveedor.GetProveedores();
        lvProveedores.DataBind();
    }

    protected void lvProveedores_ItemCanceling(object sender, ListViewCancelEventArgs e)
    {
        lvProveedores.EditIndex = -1;
        if (Request["idProveedor"] == null)
        {
            lvProveedores.DataSource = cProveedor.GetProveedores();
            lvProveedores.DataBind();
        }
        else
        {
            lvProveedores.DataSource = cProveedor.Search(Convert.ToString(Request["idProveedor"]));
            lvProveedores.DataBind();
        }
    }

    protected void lvProveedores_ItemDeleting(object sender, ListViewDeleteEventArgs e) 
    {
        cProveedor proveedor = new cProveedor();

        TextBox txt = (lvProveedores.Items[e.ItemIndex].FindControl("txtEditId")) as TextBox;
        if (txt != null)
            proveedor = cProveedor.Load(txt.Text);

        if (txt != null)
            proveedor.Id = txt.Text;

        txt = (lvProveedores.Items[e.ItemIndex].FindControl("txtEditNombre")) as TextBox;
        if (txt != null)
            proveedor.Nombre = txt.Text;

        txt = (lvProveedores.Items[e.ItemIndex].FindControl("txtEditDireccion")) as TextBox;
        if (txt != null)
            proveedor.Direccion = txt.Text;

        txt = (lvProveedores.Items[e.ItemIndex].FindControl("txtEditTelefono")) as TextBox;
        if (txt != null)
            proveedor.Telefono = txt.Text;

        txt = (lvProveedores.Items[e.ItemIndex].FindControl("txtEditMail")) as TextBox;
        if (txt != null)
            proveedor.Mail = txt.Text;

        proveedor.Papelera = 1;
        proveedor.Save();

        lvProveedores.EditIndex = -1;
        //Carga de la pantalla
        Response.Redirect("Proveedor.aspx");
        //Carga del combo
        cbProveedor.DataSource = cProveedor.GetProveedores();
        cbProveedor.DataValueField = "id";
        cbProveedor.DataTextField = "nombre";
        cbProveedor.DataBind();
        lvProveedores.DataSource = cProveedor.GetProveedores();
        lvProveedores.DataBind();
    }
    #endregion

    #region Metodo Insertar en ListView
    protected void lvProveedores_ItemInserting(object sender, ListViewInsertEventArgs e) 
    {
        cProveedor proveedor = new cProveedor();

        TextBox txt = (e.Item.FindControl("txtNombre")) as TextBox;
        if (txt != null)
            proveedor.Nombre = txt.Text;

        txt = (e.Item.FindControl("txtDireccion")) as TextBox;
        if (txt != null)
            proveedor.Direccion = txt.Text;

        txt = (e.Item.FindControl("txtTelefono")) as TextBox;
        if (txt != null)
            proveedor.Telefono = txt.Text;

        txt = (e.Item.FindControl("txtMail")) as TextBox;
        if (txt != null)
            proveedor.Mail = txt.Text;

        proveedor.Papelera = 0;

        proveedor.Save();

        lvProveedores.EditIndex = -1;
        lvProveedores.DataSource = cProveedor.GetProveedores();
        lvProveedores.DataBind();
    }
    #endregion

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Proveedor.aspx?idProveedor=" + cbProveedor.SelectedValue);
    }

    protected void lbVerTodos_Click(object sender, EventArgs e)
    {
        Response.Redirect("Proveedor.aspx");
    }

    protected void lkbVolverCompra_Click(object sender, EventArgs e)
    {
        Response.Redirect("BuscarCompra.aspx");
    }
}
