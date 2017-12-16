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

public partial class ClientePosible : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region Combos
            ddlEditConseguidoPor.DataSource = cUsuario.GetUsuarios();
            ddlEditConseguidoPor.DataValueField = "id";
            ddlEditConseguidoPor.DataTextField = "nombre";
            ddlEditConseguidoPor.DataBind();
            ListItem it0 = new ListItem("Seleccione un usuario...", "0");
            ddlEditConseguidoPor.Items.Insert(0, it0);

            ddlAccion1.DataSource = cAccion.GetAcciones();
            ddlAccion1.DataValueField = "id";
            ddlAccion1.DataTextField = "descripcion";
            ddlAccion1.DataBind();
            ListItem it1 = new ListItem("Seleccione una accion...", "0");
            ddlAccion1.Items.Insert(0, it1);

            ddlAccion2.DataSource = cAccion.GetAcciones();
            ddlAccion2.DataValueField = "id";
            ddlAccion2.DataTextField = "descripcion";
            ddlAccion2.DataBind();
            ListItem it2 = new ListItem("Seleccione una accion...", "0");
            ddlAccion2.Items.Insert(0, it2);

            ddlAccion3.DataSource = cAccion.GetAcciones();
            ddlAccion3.DataValueField = "id";
            ddlAccion3.DataTextField = "descripcion";
            ddlAccion3.DataBind();
            ListItem it3 = new ListItem("Seleccione una accion...", "0");
            ddlAccion3.Items.Insert(0, it3);

            ddlAccion4.DataSource = cAccion.GetAcciones();
            ddlAccion4.DataValueField = "id";
            ddlAccion4.DataTextField = "descripcion";
            ddlAccion4.DataBind();
            ListItem it4 = new ListItem("Seleccione una accion...", "0");
            ddlAccion4.Items.Insert(0, it4);
            #endregion

            try
            {
                if (Request["idCliente"] != null)
                {
                    cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
                    lblEmpresa.Text = cEmpresa.Load(cliente.IdEmpresa.ToString()).Nombre;
                    lblNombre.Text = cEmpresa.Load(cliente.IdEmpresa.ToString()).Nombre;
                    lblRubro.Text = cliente.Rubro;
                    lblDireccion.Text = cEmpresa.Load(cliente.IdEmpresa.ToString()).Direccion;
                    lblTelefono.Text = cEmpresa.Load(cliente.IdEmpresa.ToString()).Telefono;
                    lblMail.Text = cliente.Mail;
                    lblContacto.Text = cliente.Contacto;
                    lblPuestoContacto.Text = cliente.PuestoContacto;
                    lblContactoVinculo.Text = cliente.ContactoVinculado;
                    if (cliente.IdUsuario != 0)
                        lblConseguidoPor.Text = cUsuario.Load(cliente.IdUsuario.ToString()).Nombre;

                    if (cliente.GetAccion1 != "0")
                        lblAccion1.Text = cliente.GetAccion1;
                    else
                    {
                        lblAccion1.Text = "";
                        ImageAccion1.Visible = false;
                    }

                    if (cliente.IdEstadoAccion1 == Convert.ToInt16(papelera.Activo))
                        ImageAccion1.ImageUrl = "~/Imagenes/imageTrue.png";
                    else
                        ImageAccion1.ImageUrl = "~/Imagenes/imageFalse.png";

                    if (cliente.GetAccion2 != "0")
                        lblAccion2.Text = cliente.GetAccion2;
                    else{
                        lblAccion2.Text = "";
                        ImageAccion2.Visible = false;
                    }

                    if (cliente.IdEstadoAccion2 == Convert.ToInt16(papelera.Activo))
                        ImageAccion2.ImageUrl = "~/Imagenes/imageTrue.png";
                    else
                        ImageAccion2.ImageUrl = "~/Imagenes/imageFalse.png";

                    if (cliente.GetAccion3 != "0")
                        lblAccion3.Text = cliente.GetAccion3;
                    else{
                        lblAccion3.Text = "";
                        ImageAccion3.Visible = false;
                    }

                    if (cliente.IdEstadoAccion3 == Convert.ToInt16(papelera.Activo))
                        ImageAccion3.ImageUrl = "~/Imagenes/imageTrue.png";
                    else
                        ImageAccion3.ImageUrl = "~/Imagenes/imageFalse.png";

                    if (cliente.GetAccion4 != "0")
                        lblAccion4.Text = cliente.GetAccion4;
                    else{
                        lblAccion4.Text = "";
                        ImageAccion4.Visible = false;
                    }

                    if (cliente.IdEstadoAccion4 == Convert.ToInt16(papelera.Activo))
                        ImageAccion4.ImageUrl = "~/Imagenes/imageTrue.png";
                    else
                        ImageAccion4.ImageUrl = "~/Imagenes/imageFalse.png";

                    ddlEstado.SelectedValue = Convert.ToString(cliente.IdEstado);
                }
            }
            catch
            {
                lblEmpresa.Text = "ERRONEO";
                lblEmpresa.Text = "-";
                lblRubro.Text = "-";
                lblDireccion.Text = "-";
                lblTelefono.Text = "-";
                lblMail.Text = "-";
                lblContacto.Text = "-";
                lblPuestoContacto.Text = "-";
                lblContactoVinculo.Text = "-";
                lblConseguidoPor.Text = "-";
                ddlAccion1.SelectedValue = Convert.ToString(0);
                ddlAccion2.SelectedValue = Convert.ToString(0);
                ddlAccion3.SelectedValue = Convert.ToString(0);
                ddlAccion4.SelectedValue = Convert.ToString(0);
            }

        }
    }

    #region Botones
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        cClientePosible clienteFuturo = cClientePosible.Load(Request["idCliente"]);
        cEmpresa empresa = cEmpresa.Load(clienteFuturo.IdEmpresa.ToString());
        empresa.Papelera = 1;
        empresa.Save();

        cCliente cliente = new cCliente();
        cliente.IdEmpresa = clienteFuturo.IdEmpresa.ToString();
        cliente.Nombre = clienteFuturo.Contacto;
        cliente.Interno = "";
        cliente.Mail = clienteFuturo.Mail;
        cliente.Ip = "";
        cliente.Password = "";
        cliente.UsuarioRed = "";
        cliente.PasswordRed = "";
        cliente.ClaveSistema = "";
        cliente.Autorizacion = (Int16)Autorizaciones.No_Autorizado;
        cliente.Papelera = Convert.ToInt16(papelera.Activo);
        cliente.Save();

        Response.Redirect("AgendaClientesPosibles.aspx");
    }

    protected void btnEditarEmpresa_Click(object sender, EventArgs e)
    {
        pnlEmpresa.Visible = false;
        pnlEmpresaEdit.Visible = true;
        btnEditarEmpresa.Visible = false;
        btnAceptarEmpresa.Visible = true;

        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        txtNombre.Text = cEmpresa.Load(cliente.IdEmpresa.ToString()).Nombre;
        txtRubro.Text = cliente.Rubro;
        txtDireccion.Text = cEmpresa.Load(cliente.IdEmpresa.ToString()).Direccion;
        txtTelefono.Text = cEmpresa.Load(cliente.IdEmpresa.ToString()).Telefono;
        txtMail.Text = cliente.Mail;
    }

    protected void btnAceptarEmpresa_Click(object sender, EventArgs e)
    {
        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        cEmpresa empresa = cEmpresa.Load(cliente.IdEmpresa.ToString());
        empresa.Nombre = txtNombre.Text;
        empresa.Direccion = txtDireccion.Text;
        empresa.Telefono = txtTelefono.Text;
        empresa.Save();
        cliente.Rubro = txtRubro.Text;
        cliente.Mail = txtMail.Text;
        cliente.Save();

        Response.Redirect("ClientePosible.aspx?idCliente=" + cliente.Id);
    }

    protected void btnEditContacto_Click(object sender, EventArgs e)
    {
        pnlContacto.Visible = false;
        pnlContactoEdit.Visible = true;
        btnEditContacto.Visible = false;
        btnAceptarContacto.Visible = true;

        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        txtContacto.Text = cliente.Contacto;
        txtPuestoContacto.Text = cliente.PuestoContacto;
        txtContactoVinculo.Text = cliente.ContactoVinculado;
        ddlEditConseguidoPor.SelectedValue = Convert.ToString(cliente.IdUsuario);    
    }

    protected void btnAceptarContacto_Click(object sender, EventArgs e)
    {
        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        cliente.Contacto = txtContacto.Text;
        cliente.PuestoContacto = txtPuestoContacto.Text;
        cliente.ContactoVinculado = txtContactoVinculo.Text;
        cliente.IdUsuario = Convert.ToInt16(ddlEditConseguidoPor.SelectedValue); 
        cliente.Save();

        Response.Redirect("ClientePosible.aspx?idCliente=" + cliente.Id);
    }

    protected void btnEditAcciones_Click(object sender, EventArgs e)
    {
        pnlAcciones.Visible = false;
        pnlEditAcciones.Visible = true;
        btnEditAcciones.Visible = false;
        btnAceptarAcciones.Visible = true;

        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        ddlAccion1.SelectedValue = Convert.ToString(cliente.IdAccion1);
        ddlAccion2.SelectedValue = Convert.ToString(cliente.IdAccion2);
        ddlAccion3.SelectedValue = Convert.ToString(cliente.IdAccion3);
        ddlAccion4.SelectedValue = Convert.ToString(cliente.IdAccion4);
    }

    protected void btnAceptarAcciones_Click(object sender, EventArgs e)
    {
        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        cliente.IdAccion1 = Convert.ToInt16(ddlAccion1.SelectedValue);
        cliente.IdAccion2 = Convert.ToInt16(ddlAccion2.SelectedValue);
        cliente.IdAccion3 = Convert.ToInt16(ddlAccion3.SelectedValue);
        cliente.IdAccion4 = Convert.ToInt16(ddlAccion4.SelectedValue);
        cliente.Save();

        Response.Redirect("ClientePosible.aspx?idCliente=" + cliente.Id);
    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        cAccion accion = new cAccion(txtNuevaAccion.Text);
        accion.Save();
    }

    protected void btnImageTrue1_Click(object sender, EventArgs e)
    {
        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        cliente.IdEstadoAccion1 = Convert.ToInt16(papelera.Activo);
        cliente.Save();
        Response.Redirect("ClientePosible.aspx?idCliente=" + cliente.Id);
    }

    protected void btnImageTrue2_Click(object sender, EventArgs e)
    {
        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        cliente.IdEstadoAccion2 = Convert.ToInt16(papelera.Activo);
        cliente.Save();
        Response.Redirect("ClientePosible.aspx?idCliente=" + cliente.Id);
    }

    protected void btnImageTrue3_Click(object sender, EventArgs e)
    {
        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        cliente.IdEstadoAccion3 = Convert.ToInt16(papelera.Activo);
        cliente.Save();
        Response.Redirect("ClientePosible.aspx?idCliente=" + cliente.Id);
    }

    protected void btnImageTrue4_Click(object sender, EventArgs e)
    {
        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        cliente.IdEstadoAccion4 = Convert.ToInt16(papelera.Activo);
        cliente.Save();
        Response.Redirect("ClientePosible.aspx?idCliente=" + cliente.Id);
    }

    protected void btnImageFalse1_Click(object sender, EventArgs e)
    {
        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        cliente.IdEstadoAccion1 = Convert.ToInt16(papelera.Eliminado);
        cliente.Save();
        Response.Redirect("ClientePosible.aspx?idCliente=" + cliente.Id);
    }

    protected void btnImageFalse2_Click(object sender, EventArgs e)
    {
        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        cliente.IdEstadoAccion2 = Convert.ToInt16(papelera.Eliminado);
        cliente.Save();
        Response.Redirect("ClientePosible.aspx?idCliente=" + cliente.Id);
    }

    protected void btnImageFalse3_Click(object sender, EventArgs e)
    {
        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        cliente.IdEstadoAccion3 = Convert.ToInt16(papelera.Eliminado);
        cliente.Save();
        Response.Redirect("ClientePosible.aspx?idCliente=" + cliente.Id);
    }

    protected void btnImageFalse4_Click(object sender, EventArgs e)
    {
        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        cliente.IdEstadoAccion4 = Convert.ToInt16(papelera.Eliminado);
        cliente.Save();
        Response.Redirect("ClientePosible.aspx?idCliente=" + cliente.Id);
    }
    #endregion   
    
    #region SelectedIndexChanged
    protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        cClientePosible cliente = cClientePosible.Load(Request["idCliente"]);
        cliente.IdEstado = Convert.ToInt16(ddlEstado.SelectedValue);
        cliente.Save();
        Response.Redirect("ClientePosible.aspx?idCliente=" + cliente.Id);
    }
    #endregion
}
