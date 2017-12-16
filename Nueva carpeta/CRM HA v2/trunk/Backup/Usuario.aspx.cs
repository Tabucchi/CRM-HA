using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

public partial class Usuario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            lvUsuarios.DataSource = cUsuario.GetUsuarios();
            lvUsuarios.DataBind();
        }
    }

    protected void ListPager_PreRender(object sender, EventArgs e)
    {
        lvUsuarios.DataSource = cUsuario.GetUsuarios();
        lvUsuarios.DataBind();
    }

    #region Edición
    protected void lvUsuarios_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        lvUsuarios.EditIndex = e.NewEditIndex;
        lvUsuarios.DataSource = cUsuario.GetUsuarios();
        lvUsuarios.DataBind();
    }

    protected void lvUsuarios_ItemUpdating(object sender, ListViewUpdateEventArgs e) //Actualiza datos de empresa
    {
        cUsuario usuario=new cUsuario();

        TextBox txt = (lvUsuarios.Items[e.ItemIndex].FindControl("txtEditId")) as TextBox;
        if (txt != null)
            usuario = cUsuario.Load(txt.Text);

        if (txt != null)
            usuario.Id = txt.Text;

        txt = (lvUsuarios.Items[e.ItemIndex].FindControl("txtEditNombre")) as TextBox;
        if (txt != null)
            usuario.Nombre = txt.Text;

        txt = (lvUsuarios.Items[e.ItemIndex].FindControl("txtEditUsuario")) as TextBox;
        if (txt != null)
            usuario.Usuario = txt.Text;

        txt = (lvUsuarios.Items[e.ItemIndex].FindControl("txtEditClave")) as TextBox;
        if (txt != null)
            usuario.Clave = cUsuario.Codify(txt.Text);

        txt = (lvUsuarios.Items[e.ItemIndex].FindControl("txtEditMail")) as TextBox;
        if (txt != null)
            usuario.Mail = txt.Text;
        
        DropDownList ddl = (lvUsuarios.Items[e.ItemIndex].FindControl("ddlEditCategoria")) as DropDownList;
        if (ddl.SelectedValue != null)
            usuario.IdCategoria = Convert.ToInt16(ddl.SelectedValue);

        usuario.Papelera = usuario.Papelera;

        usuario.Save();

        lvUsuarios.EditIndex = -1;
        lvUsuarios.DataSource = cUsuario.GetUsuarios();
        lvUsuarios.DataBind();
    }

    protected void lvUsuarios_ItemCanceling(object sender, ListViewCancelEventArgs e) 
    {
        lvUsuarios.EditIndex = -1;
        lvUsuarios.DataSource = cUsuario.GetUsuarios();
        lvUsuarios.DataBind();
    }

    protected void lvUsuarios_ItemDeleting(object sender, ListViewDeleteEventArgs e) 
    {
        cUsuario usuario = new cUsuario();

        TextBox txt = (lvUsuarios.Items[e.ItemIndex].FindControl("txtEditId")) as TextBox;
        if (txt != null)
            usuario = cUsuario.Load(txt.Text);

        if (txt != null)
            usuario.Id = txt.Text;

        txt = (lvUsuarios.Items[e.ItemIndex].FindControl("txtEditNombre")) as TextBox;
        if (txt != null)
            usuario.Nombre = txt.Text;

        txt = (lvUsuarios.Items[e.ItemIndex].FindControl("txtEditUsuario")) as TextBox;
        if (txt != null)
            usuario.Usuario = txt.Text;

        txt = (lvUsuarios.Items[e.ItemIndex].FindControl("txtEditClave")) as TextBox;
        if (txt != null)
            usuario.Clave = cUsuario.Codify(txt.Text);

        txt = (lvUsuarios.Items[e.ItemIndex].FindControl("txtEditMail")) as TextBox;
        if (txt != null)
            usuario.Mail = txt.Text;

        txt = (lvUsuarios.Items[e.ItemIndex].FindControl("txtEditCategoria")) as TextBox;
        if (txt != null)
            usuario.TipoCategoria = txt.Text;

        usuario.Papelera = 0;

        usuario.Save();

        lvUsuarios.EditIndex = -1;
        lvUsuarios.DataSource = cUsuario.GetUsuarios();
        lvUsuarios.DataBind();
    }
    #endregion

    #region Insertar 
    protected void lvUsuarios_ItemInserting(object sender, ListViewInsertEventArgs e) 
    {
        cUsuario usuario=new cUsuario();

        TextBox txt = (e.Item.FindControl("txtNombre")) as TextBox;
        if (txt != null)
            usuario.Nombre = txt.Text;

        txt = (e.Item.FindControl("txtUsuario")) as TextBox;
        if (txt != null)
            usuario.Usuario = txt.Text;

        txt = (e.Item.FindControl("txtClave")) as TextBox;
        if (txt != null)
        {
            int j = 0; //se usa como flag.
            int l = 0;
            for (int i = 0; i < txt.Text.Length - 1; i++) 
            {
                if (Char.IsNumber(txt.Text, i)) //Compruebo que haya números en el código
                    j = 1;

                if (Char.IsLetter(txt.Text, i)) //Compruebo que haya letras en el código
                    l = 1;
            }

            if (txt.Text.Length < 6 || j == 0 || l == 0)
            {
                pnlMensaje.Visible = true;
                return;
            }
            else
                pnlMensaje.Visible = false;

            usuario.Clave = cUsuario.Codify(txt.Text);
        }

        txt = (e.Item.FindControl("txtMail")) as TextBox;
        if (txt != null)
            usuario.Mail = txt.Text;

        DropDownList ddl = (e.Item.FindControl("ddlEditCategoria")) as DropDownList;
        if (ddl.SelectedValue != null)
            usuario.IdCategoria = Convert.ToInt16(ddl.SelectedValue);

        usuario.Papelera = 1;
        usuario.Save();

        lvUsuarios.EditIndex = -1;
        lvUsuarios.DataSource = cUsuario.GetUsuarios();
        lvUsuarios.DataBind();
    }
    #endregion
}
