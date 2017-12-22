using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class AgendaClientesPotenciales : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region Combo
            ddlAccion.DataSource = cAccion.GetAcciones();
            ddlAccion.DataValueField = "id";
            ddlAccion.DataTextField = "descripcion";
            ddlAccion.DataBind();
            ListItem it = new ListItem("Seleccione una accion...", "0");
            ddlAccion.Items.Insert(0, it);

            ddlConseguidoPor.DataSource = cUsuario.GetUsuarios();
            ddlConseguidoPor.DataValueField = "id";
            ddlConseguidoPor.DataTextField = "nombre";
            ddlConseguidoPor.DataBind();
            ListItem it1 = new ListItem("Seleccione un usuario...", "0");
            ddlConseguidoPor.Items.Insert(0, it1);
            #endregion
        }
    }

    protected void ListPager_PreRender(object sender, EventArgs e)
    {
        if (Request["idEmpresa"] == null)
            lvEmpresas.DataSource = cClientePosible.SearchEmpresa(null);
        else
            lvEmpresas.DataSource = cClientePosible.SearchEmpresa(Convert.ToString(Request["idEmpresa"]));
        lvEmpresas.DataBind();
    }

    #region Botones
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        Response.Redirect("AgendaClientesPosibles.aspx?idEmpresa=" + cEmpresa.GetIdByNombre(txtEmpresa.Text));
    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        cClientePosible cliente = new cClientePosible();
        
        cEmpresa empresa = new cEmpresa(txtNombEmpresa.Text, txtDireccion.Text, txtTelefono.Text, "");
        empresa.Papelera = Convert.ToInt16(papelera.ClientePosible);
        int id  = empresa.Save();

        cliente.IdEmpresa = Convert.ToInt16(id);
        cliente.Rubro = txtRubro.Text;
        cliente.Contacto = txtContacto.Text;
        cliente.PuestoContacto = txtPuestoContacto.Text;
        cliente.ContactoVinculado = txtContactoVinculado.Text;
        cliente.Mail = txtMail.Text;
        cliente.IdUsuario = Convert.ToInt16(ddlConseguidoPor.SelectedValue);
        cliente.IdAccion1 = Convert.ToInt16(ddlAccion.SelectedValue);
        cliente.IdAccion2 = 0;
        cliente.IdAccion3 = 0;
        cliente.IdAccion4 = 0;
        cliente.IdEstado = Convert.ToInt16(ddlEstado.SelectedValue);
        cliente.Save();

        Response.Redirect("AgendaClientesPosibles.aspx");
    }
    #endregion
}
