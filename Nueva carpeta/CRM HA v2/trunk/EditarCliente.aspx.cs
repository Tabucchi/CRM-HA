using DLL.Negocio;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class EditarCliente : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int32)eCategoria.Administración)
                {
                    cbTipoCliente.DataSource = cCampoGenerico.CargarComboTipoPersona();
                    cbTipoCliente.DataBind();

                    cEmpresa empresa = cEmpresa.Load(Request["idCliente"].ToString());
                    cbTipoCliente.SelectedIndex = Convert.ToInt16(empresa.TipoCliente);

                    #region Habilitación de panel de Persona Fisica o Persona Juridica + Datos del cliente
                    if (cbTipoCliente.SelectedIndex.ToString() == Convert.ToString((Int16)tipoCliente.Persona_física))
                    {
                        CargarComboPersonaFisica();
                        pnlPersonaFisica.Visible = true;
                        pnlPersonaJuridica.Visible = false;

                        CargaClientePersonaFisica(empresa);
                    }

                    if (cbTipoCliente.SelectedIndex.ToString() == Convert.ToString((Int16)tipoCliente.Persona_jurídica))
                    {
                        CargarComboPersonaJuridica();
                        pnlPersonaFisica.Visible = false;
                        pnlPersonaJuridica.Visible = true;

                        CargaClientePersonaJuridica(empresa);
                    }
                    #endregion
                }
                else
                    Response.Redirect("Default.aspx");
            }
        }

        #region Auxiliares
        public void CargaClientePersonaFisica(cEmpresa empresa)
        {
            try
            {
                #region Datos cliente
                txtApellido.Text = empresa.Apellido;
                txtNombre.Text = empresa.Nombre;
                cbCaracter.SelectedValue = empresa.Caracter;
                txtCuit.Text = empresa.Cuit;
                ddlCondicionIva.SelectedValue = empresa.CondicionIva.ToString();
                cbTipoDoc.SelectedValue = empresa.TipoDoc;
                txtNroDoc.Text = empresa.Documento;
                txtTelefono.Text = empresa.Telefono;
                txtMail.Text = empresa.Mail;
                txtComentario.Text = empresa.Comentarios;
                #endregion

                #region Datos domicilio
                if (!string.IsNullOrEmpty(empresa.IdDomicilio) && empresa.IdDomicilio != "-1")
                {
                    cDomicilio domicilio = cDomicilio.Load(empresa.IdDomicilio);
                    txtCalle.Text = domicilio.Calle;
                    txtDireccion.Text = domicilio.Direccion;
                    txtCodPostal.Text = domicilio.CodPostal;
                    cbProvincia.SelectedValue = domicilio.IdProvincia;
                    txtCiudad.Text = domicilio.Ciudad;
                }
                #endregion

                #region Datos apoderado
                if (empresa.Apoderado == Convert.ToString((Int16)eApoderado.Si))
                {
                    pnlApoderadoFisica.Visible = true;
                    cbApoderado.SelectedValue = empresa.Apoderado;
                    cApoderado apoderado = cApoderado.Load(empresa.IdEmpresa);
                    txtNombreApoderadoFisica.Text = apoderado.RazonSocial;
                    txtCuitApoderadoFisica.Text = apoderado.Cuit;
                    cbTipoDocApoderadoFisica.SelectedValue = apoderado.TipoDoc;
                    txtNroDocApoderadoFisica.Text = apoderado.Documento;
                    txtTelefonoApoderadoFisica.Text = apoderado.Telefono;
                    txtMailApoderadoFisica.Text = apoderado.Mail;

                    cDomicilio domicilioApoderado = cDomicilio.Load(apoderado.IdDomicilio);
                    txtCalleApoderadoFisica.Text = domicilioApoderado.Calle;
                    txtDireccionApoderadoFisica.Text = domicilioApoderado.Direccion;
                    txtCodPostalApoderadoFisica.Text = domicilioApoderado.CodPostal;
                    cbProvinciaApoderadoFisica.SelectedValue = domicilioApoderado.IdProvincia;
                    txtCiudadApoderadoFisica.Text = domicilioApoderado.Ciudad;
                }
                #endregion
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("EditarCliente - " + DateTime.Now + "- " + ex.Message + " - CargaClientePersonaFisica" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        public void CargaClientePersonaJuridica(cEmpresa empresa)
        {
            try
            {
                #region Datos cliente
                txtRazonSocial.Text = empresa.Apellido;
                ddlCondicionIvaJuridica.SelectedValue = empresa.CondicionIva.ToString();
                txtCuitJuridica.Text = empresa.Cuit;
                txtTelefonoJuridica.Text = empresa.Telefono;
                txtMailApoderadoJuridica.Text = empresa.Mail;
                txtComentarioJuridica.Text = empresa.Comentarios;
                #endregion

                #region Datos domicilio
                cDomicilio domicilioJuridica = cDomicilio.Load(empresa.IdDomicilio);
                txtCalleJuridica.Text = domicilioJuridica.Calle;
                txtDireccionJuridica.Text = domicilioJuridica.Direccion;
                txtCodPostalJuridica.Text = domicilioJuridica.CodPostal;
                cbProvinciaJuridica.SelectedValue = domicilioJuridica.IdProvincia;
                txtCiudadJuridica.Text = domicilioJuridica.Ciudad;
                #endregion

                #region Datos apoderado
                if (empresa.Apoderado == Convert.ToString((Int16)eApoderado.Si))
                {
                    cApoderado apoderado = cApoderado.Load(empresa.IdEmpresa);
                    txtNombreApoderadoJuridica.Text = apoderado.RazonSocial;
                    txtCuitApoderadoJuridica.Text = apoderado.Cuit;
                    cbTipoDocApoderadoJuridica.SelectedValue = apoderado.TipoDoc;
                    txtNroDocApoderadoJuridica.Text = apoderado.Documento;
                    txtTelefonoApoderadoJuridica.Text = apoderado.Telefono;
                    txtMailApoderadoJuridica.Text = apoderado.Mail;

                    cDomicilio domicilioApoderado = cDomicilio.Load(apoderado.IdDomicilio);
                    txtCalleApoderadoJuridica.Text = domicilioApoderado.Calle;
                    txtDireccionApoderadoJuridica.Text = domicilioApoderado.Direccion;
                    txtCodPostalApoderadoJuridica.Text = domicilioApoderado.CodPostal;
                    cbProvinciaApoderadoJuridica.SelectedValue = domicilioApoderado.IdProvincia;
                    txtCiudadApoderadoJuridica.Text = domicilioApoderado.Ciudad;
                }
                #endregion
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("EditarCliente - " + DateTime.Now + "- " + ex.Message + " - CargaClientePersonaJuridica" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region DropDownList
        public void CargarComboPersonaFisica()
        {
            ddlCondicionIva.DataSource = cCampoGenerico.GetDataTable(Tablas.tCondicionIva);
            ddlCondicionIva.DataValueField = "id";
            ddlCondicionIva.DataTextField = "descripcion";
            ddlCondicionIva.DataBind();
            ListItem ci = new ListItem("Seleccione la condición de IVA...", "0");
            ddlCondicionIva.Items.Insert(0, ci);
        }

        public void CargarComboPersonaJuridica()
        {
            ddlCondicionIvaJuridica.DataSource = cCampoGenerico.GetDataTable(Tablas.tCondicionIva);
            ddlCondicionIvaJuridica.DataValueField = "id";
            ddlCondicionIvaJuridica.DataTextField = "descripcion";
            ddlCondicionIvaJuridica.DataBind();
            ListItem ci = new ListItem("Seleccione la condición de IVA...", "0");
            ddlCondicionIvaJuridica.Items.Insert(0, ci);
        }

        protected void cbTipoCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            cEmpresa empresa = cEmpresa.Load(Request["idCliente"].ToString());

            if (cbTipoCliente.SelectedIndex.ToString() == Convert.ToString((Int16)tipoCliente.Persona_física))
            {
                CargarComboPersonaFisica();
                pnlPersonaFisica.Visible = true;
                pnlPersonaJuridica.Visible = false;

                CargaClientePersonaFisica(empresa);
            }

            if (cbTipoCliente.SelectedIndex.ToString() == Convert.ToString((Int16)tipoCliente.Persona_jurídica))
            {
                CargarComboPersonaJuridica();
                pnlPersonaFisica.Visible = false;
                pnlPersonaJuridica.Visible = true;

                CargaClientePersonaJuridica(empresa);
            }
        }

        protected void ddlCondicion_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbApoderado.SelectedValue)
            {
                case "1":
                    pnlApoderadoFisica.Visible = false;
                    break;
                case "2":
                    pnlApoderadoFisica.Visible = true;
                    break;
            }
        }
        #endregion

        #region Botones
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cEmpresa empresa = cEmpresa.Load(Request["idCliente"].ToString());

            #region Persona fisica
            if (cbTipoCliente.SelectedValue == tipoCliente.Persona_física.ToString().Replace("_", " "))
            {
                int idApoderadoFisica = 0;
                //if (cAuxiliar.ValidarCuit(txtCuit.Text))
                //{
                empresa.Apellido = txtApellido.Text;
                empresa.Nombre = txtNombre.Text;
                empresa.Caracter = cbCaracter.SelectedValue;
                empresa.Cuit = txtCuit.Text;
                empresa.CondicionIva = Convert.ToInt16(ddlCondicionIva.SelectedValue);
                empresa.Apoderado = cbApoderado.SelectedValue;

                empresa.TipoDoc = cbTipoDoc.SelectedValue;
                empresa.Documento = txtNroDoc.Text;
                empresa.Comentarios = txtComentario.Text;

                cDomicilio domicilio = cDomicilio.Load(empresa.IdDomicilio);
                domicilio.Calle = txtCalle.Text;
                domicilio.Direccion = txtDireccion.Text;
                domicilio.CodPostal = txtCodPostal.Text;
                domicilio.IdProvincia = cbProvincia.SelectedValue;
                domicilio.Ciudad = txtCiudad.Text;
                domicilio.Save();

                empresa.Telefono = txtTelefono.Text;
                empresa.Mail = txtMail.Text;
                empresa.TipoCliente = Convert.ToString((Int16)tipoCliente.Persona_física);

                if (pnlApoderadoFisica.Visible == false)
                {
                    empresa.Apoderado = Convert.ToString((Int16)eApoderado.No);
                    empresa.IdEmpresa = "-1";
                }
                else
                {
                    if (empresa.IdEmpresa != "-1")
                    {
                        cApoderado apoderado = cApoderado.Load(empresa.IdEmpresa);
                        apoderado.RazonSocial = txtNombreApoderadoFisica.Text;
                        apoderado.Cuit = txtCuitApoderadoFisica.Text;

                        apoderado.TipoDoc = cbTipoDocApoderadoFisica.SelectedValue;
                        apoderado.Documento = txtNroDocApoderadoFisica.Text;

                        cDomicilio domicilioApoderado = cDomicilio.Load(apoderado.IdDomicilio);
                        domicilioApoderado.Calle = txtCalle.Text;
                        domicilioApoderado.Direccion = txtDireccion.Text;
                        domicilioApoderado.CodPostal = txtCodPostal.Text;
                        domicilioApoderado.IdProvincia = cbProvincia.SelectedValue;
                        domicilioApoderado.Ciudad = txtCiudad.Text;
                        domicilioApoderado.Save();

                        apoderado.Telefono = txtTelefonoApoderadoFisica.Text;
                        apoderado.Mail = txtMailApoderadoFisica.Text;
                        apoderado.Save();
                    }
                    else
                    {
                        cApoderado nuevoApoderado = new cApoderado();
                        nuevoApoderado.RazonSocial = txtNombreApoderadoFisica.Text;
                        nuevoApoderado.Cuit = txtCuitApoderadoFisica.Text;

                        nuevoApoderado.TipoDoc = cbTipoDocApoderadoFisica.SelectedValue;
                        nuevoApoderado.Documento = txtNroDocApoderadoFisica.Text;

                        cDomicilio domicilioApoderado = new cDomicilio(txtCalleApoderadoFisica.Text, txtDireccionApoderadoFisica.Text, txtCodPostalApoderadoFisica.Text, cbProvinciaApoderadoFisica.SelectedValue, txtCiudadApoderadoFisica.Text);
                        int idDomicilioApoderado = domicilioApoderado.Save();

                        nuevoApoderado.IdDomicilio = idDomicilioApoderado.ToString();
                        nuevoApoderado.Telefono = txtTelefonoApoderadoFisica.Text;
                        nuevoApoderado.Mail = txtMailApoderadoFisica.Text;
                        nuevoApoderado.Papelera = 1;

                        idApoderadoFisica = nuevoApoderado.Save();

                        empresa.Apoderado = Convert.ToString((Int16)eApoderado.Si);
                        empresa.IdEmpresa = idApoderadoFisica.ToString();
                    }
                }

                empresa.Save();
                Response.Redirect("Agenda.aspx");
                //}
                //else
                //{
                //    pnlCuit.Visible = true;
                //}
            }
            #endregion

            #region Persona jurídica
            if (cbTipoCliente.SelectedValue == tipoCliente.Persona_jurídica.ToString().Replace("_", " "))
            {
                int idApoderado = 0;
                /*if (cAuxiliar.ValidarCuit(txtCuitJuridica.Text))
                {*/
                empresa.Apellido = "";
                empresa.Nombre = txtRazonSocial.Text;
                empresa.Caracter = Convert.ToString((Int16)tipoCaracter.En_comisión);
                empresa.Cuit = txtCuitJuridica.Text;
                empresa.CondicionIva = Convert.ToInt16(ddlCondicionIvaJuridica.SelectedValue);
                empresa.TipoDoc = Convert.ToString((Int16)tipoDocumento.No);
                empresa.Documento = "-";
                empresa.Comentarios = txtComentarioJuridica.Text;

                cDomicilio domicilio = cDomicilio.Load(empresa.IdDomicilio);
                domicilio.Calle = txtCalleJuridica.Text;
                domicilio.Direccion = txtDireccionJuridica.Text;
                domicilio.CodPostal = txtCodPostalJuridica.Text;
                domicilio.IdProvincia = cbProvinciaJuridica.SelectedValue;
                domicilio.Ciudad = txtCiudadJuridica.Text;
                domicilio.Save();

                empresa.Telefono = txtTelefonoJuridica.Text;
                empresa.Mail = txtMailJuridica.Text;
                empresa.TipoCliente = Convert.ToString((Int16)tipoCliente.Persona_jurídica);

                empresa.Apoderado = Convert.ToString((Int16)eApoderado.Si);
                empresa.IdEmpresa = "-1";

                if (pnlApoderadoJuridica.Visible == false)
                {
                    empresa.Apoderado = Convert.ToString((Int16)eApoderado.No);
                    empresa.IdEmpresa = "-1";
                }
                else
                {
                    if (empresa.IdEmpresa != "-1")
                    {
                        cApoderado apoderado = cApoderado.Load(empresa.IdEmpresa);
                        apoderado.RazonSocial = txtNombreApoderadoJuridica.Text;
                        apoderado.Cuit = txtCuitApoderadoJuridica.Text;

                        apoderado.TipoDoc = cbTipoDocApoderadoJuridica.SelectedValue;
                        apoderado.Documento = txtNroDocApoderadoJuridica.Text;

                        cDomicilio domicilioApoderado = cDomicilio.Load(empresa.IdDomicilio);
                        domicilioApoderado.Calle = txtCalleApoderadoJuridica.Text;
                        domicilioApoderado.Direccion = txtDireccionApoderadoJuridica.Text;
                        domicilioApoderado.CodPostal = txtCodPostalApoderadoJuridica.Text;
                        domicilioApoderado.IdProvincia = cbProvinciaApoderadoJuridica.SelectedValue;
                        domicilioApoderado.Ciudad = txtCiudadApoderadoJuridica.Text;
                        domicilioApoderado.Save();

                        apoderado.Telefono = txtTelefonoApoderadoJuridica.Text;
                        apoderado.Mail = txtMailApoderadoJuridica.Text;
                        apoderado.Save();
                    }
                    else
                    {
                        cApoderado nuevoApoderado = new cApoderado();
                        nuevoApoderado.RazonSocial = txtNombreApoderadoJuridica.Text;
                        nuevoApoderado.Cuit = txtCuitApoderadoJuridica.Text;

                        nuevoApoderado.TipoDoc = cbTipoDocApoderadoJuridica.SelectedValue;
                        nuevoApoderado.Documento = txtNroDocApoderadoJuridica.Text;

                        cDomicilio domicilioApoderado = new cDomicilio(txtCalleApoderadoJuridica.Text, txtDireccionApoderadoJuridica.Text, txtCodPostalApoderadoJuridica.Text, cbProvinciaApoderadoJuridica.SelectedValue, txtCiudadApoderadoJuridica.Text);
                        int idDomicilioApoderado = domicilioApoderado.Save();

                        nuevoApoderado.IdDomicilio = idDomicilioApoderado.ToString();
                        nuevoApoderado.Telefono = txtTelefonoApoderadoJuridica.Text;
                        nuevoApoderado.Mail = txtMailApoderadoJuridica.Text;
                        nuevoApoderado.Papelera = 1;

                        idApoderado = nuevoApoderado.Save();

                        empresa.Apoderado = Convert.ToString((Int16)eApoderado.Si);
                        empresa.IdEmpresa = idApoderado.ToString();
                    }
                }

                empresa.Save();
                Response.Redirect("Agenda.aspx");
            }
            #endregion
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            cEmpresa empresa = cEmpresa.Load(Request["idCliente"].ToString());
            empresa.Papelera = (Int16)papelera.Eliminado;
            empresa.Save();
            Response.Redirect("Agenda.aspx");
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Agenda.aspx");
        }
        #endregion
    }
}