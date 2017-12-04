using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Novedad : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lvNovedades.DataSource = cNovedad.GetNovedades();
            lvNovedades.DataBind();
        }
    }
    
    #region Metodos de Edición en ListView
    protected void lvNovedades_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        lvNovedades.EditIndex = e.NewEditIndex;
        lvNovedades.DataSource = cNovedad.GetNovedades();
        lvNovedades.DataBind();
    }

    protected void lvNovedades_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        cNovedad novedad = new cNovedad();
        TextBox txt = (lvNovedades.Items[e.ItemIndex].FindControl("txtEditId")) as TextBox;

        if (txt != null)
            novedad = cNovedad.Load(txt.Text);

        if (txt != null)
            novedad.Id = txt.Text;

        txt = (lvNovedades.Items[e.ItemIndex].FindControl("txtEditDescripcion")) as TextBox;
        if (txt != null)
            novedad.Descripcion = txt.Text;

        txt = (lvNovedades.Items[e.ItemIndex].FindControl("txtEditFecha")) as TextBox;
        if (txt != null)
        {
            DateTime fecha = Convert.ToDateTime(txt.Text);
            novedad.Fecha = Convert.ToDateTime(fecha.ToString("dd/MM/yyyy"));
        }
        
        novedad.IdUsuario = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Id;

        novedad.Save();

        lvNovedades.EditIndex = -1;
        lvNovedades.DataSource = cNovedad.GetNovedades();
        lvNovedades.DataBind();
    }

    protected void lvNovedades_ItemCanceling(object sender, ListViewCancelEventArgs e) 
    {
        lvNovedades.EditIndex = -1;
        lvNovedades.DataSource = cNovedad.GetNovedades();
        lvNovedades.DataBind();
    }

    protected void lvNovedades_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        string empid = "";
        TextBox txt = (lvNovedades.Items[e.ItemIndex].FindControl("txtEditId")) as TextBox;

        if (txt != null)
            empid = txt.Text;

        lvNovedades.EditIndex = -1;
        lvNovedades.DataSource = cNovedad.DeleteNovedad(empid);
        lvNovedades.DataBind();
    }
    #endregion

    #region Metodo Insertar en ListView
    protected void lvNovedades_ItemInserting(object sender, ListViewInsertEventArgs e)
    {
        cNovedad novedad = new cNovedad();
        TextBox txt = (e.Item.FindControl("txtDescripcion")) as TextBox;
        if (txt != null)
            novedad.Descripcion = txt.Text;

        txt = (e.Item.FindControl("txtFecha")) as TextBox;
        if (txt != null)
        {
            novedad.Fecha = Convert.ToDateTime(txt.Text);
        }

        novedad.IdUsuario = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Id;

        novedad.Save();

        lvNovedades.EditIndex = -1;
        lvNovedades.DataSource = cNovedad.GetNovedades();
        lvNovedades.DataBind();
    }
    #endregion

    protected void ListPager_PreRender(object sender, EventArgs e)
    {
        lvNovedades.DataSource = cNovedad.GetNovedades();
        lvNovedades.DataBind();
    }

}