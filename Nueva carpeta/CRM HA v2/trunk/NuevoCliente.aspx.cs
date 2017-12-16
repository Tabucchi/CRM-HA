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
    public partial class NuevoCliente : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria != (Int32)eCategoria.Administración)
                    Response.Redirect("Default.aspx");

                cbTipoCliente.DataSource = cCampoGenerico.CargarComboTipoPersona();
                cbTipoCliente.DataBind();
            }
        }

        #region DropDownList
        protected void cbTipoCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTipoCliente.SelectedIndex.ToString() == Convert.ToString((Int16)tipoCliente.Persona_física))
            {
                //CargarComboPersonaFisica();
                pnlPersonaFisica.Visible = true;
                pnlPersonaJuridica.Visible = false;
            }

            if (cbTipoCliente.SelectedIndex.ToString() == Convert.ToString((Int16)tipoCliente.Persona_jurídica))
            {
                CargarComboPersonaJuridica();
                pnlPersonaFisica.Visible = false;
                pnlPersonaJuridica.Visible = true;
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
        #endregion

        #region Botones
        protected void btnFinalizar_Click(object sender, EventArgs e)
        {

            #region Persona fisica
            if (cbTipoCliente.SelectedValue == tipoCliente.Persona_física.ToString().Replace("_", " "))
            {
                int idApoderadoFisica = 0;
                /*if (cAuxiliar.ValidarCuit(txtCuit.Text))
                {*/
                cEmpresa empresa = new cEmpresa();
                empresa.Apellido = txtApellido.Text;
                empresa.Nombre = txtNombre.Text;
                empresa.Caracter = cbCaracter.SelectedValue;
                empresa.Cuit = txtCuit.Text;
                empresa.CondicionIva = Convert.ToInt16(ddlCondicionIva.SelectedValue);
                empresa.Apoderado = cbApoderado.SelectedValue;

                empresa.TipoDoc = cbTipoDoc.SelectedValue;
                empresa.Documento = txtNroDoc.Text;

                string calle = !string.IsNullOrEmpty(txtCalle.Text) ? txtCalle.Text : "-";
                string direccion = !string.IsNullOrEmpty(txtDireccion.Text) ? txtDireccion.Text : "-";
                string codPostal = !string.IsNullOrEmpty(txtCodPostal.Text) ? txtCodPostal.Text : "-";
                string ciudad = !string.IsNullOrEmpty(txtCiudad.Text) ? txtCiudad.Text : "-";

                cDomicilio domicilio = new cDomicilio(calle, direccion, codPostal, cbProvincia.SelectedValue, ciudad);
                int idDomicilio = domicilio.Save();

                empresa.IdDomicilio = idDomicilio.ToString();
                empresa.Telefono = txtTelefono.Text;
                empresa.Mail = txtMail.Text;
                empresa.TipoCliente = Convert.ToString((Int16)tipoCliente.Persona_física);
                empresa.Datos = "-"; //si se deja en null, aparece una excepcion de sql
                empresa.Papelera = 1;
                empresa.Comentarios = txtComentarioFisica.Text;

                if (pnlApoderadoFisica.Visible == false)
                {
                    empresa.Apoderado = Convert.ToString((Int16)eApoderado.No);
                    empresa.IdEmpresa = "-1";
                }
                else
                {
                    cApoderado apoderado = new cApoderado();
                    apoderado.RazonSocial = txtNombreApoderadoFisica.Text;
                    apoderado.Cuit = txtCuitApoderadoFisica.Text;

                    apoderado.TipoDoc = cbTipoDocApoderadoFisica.SelectedValue;
                    apoderado.Documento = txtNroDocApoderadoFisica.Text;

                    cDomicilio domicilioApoderado = new cDomicilio(txtCalleApoderadoFisica.Text, txtDireccionApoderadoFisica.Text, txtCodPostalApoderadoFisica.Text, cbProvinciaApoderadoFisica.SelectedValue, txtCiudadApoderadoFisica.Text);
                    int idDomicilioApoderado = domicilioApoderado.Save();

                    apoderado.IdDomicilio = idDomicilioApoderado.ToString();
                    apoderado.Telefono = txtTelefonoApoderadoFisica.Text;
                    apoderado.Mail = txtMailApoderadoFisica.Text;
                    apoderado.Papelera = 1;

                    idApoderadoFisica = apoderado.Save();

                    empresa.Apoderado = Convert.ToString((Int16)eApoderado.Si);
                    empresa.IdEmpresa = idApoderadoFisica.ToString();
                }

                //BORRAR
                empresa.IdEstadoCivil = 0;
                empresa.Clave = "";
                empresa.Direccion = "";
                empresa.IdEstado = 0;

                int _idEmpresa = empresa.Save();

                cCuentaCorrienteUsuario ccu = new cCuentaCorrienteUsuario(_idEmpresa.ToString());
                Response.Redirect("Agenda.aspx");
                /*}
                else
                {
                    pnlCuit.Visible = true;
                }*/
            }
            #endregion

            #region Persona jurídica
            if (cbTipoCliente.SelectedValue == tipoCliente.Persona_jurídica.ToString().Replace("_", " "))
            {
                int idApoderado = 0;
                cEmpresa empresa = new cEmpresa();
                empresa.Apellido = txtRazonSocial.Text;
                empresa.Nombre = "";
                empresa.Caracter = Convert.ToString((Int16)tipoCaracter.En_comisión);
                empresa.Cuit = txtCuitJuridica.Text;
                empresa.CondicionIva = Convert.ToInt16(ddlCondicionIvaJuridica.SelectedValue);
                empresa.TipoDoc = Convert.ToString((Int16)tipoDocumento.No);
                empresa.Documento = "-";

                cDomicilio domicilio = new cDomicilio(txtCalleJuridica.Text, txtDireccionJuridica.Text, txtCodPostalJuridica.Text, cbProvinciaJuridica.SelectedValue, txtCiudadJuridica.Text);
                int idDomicilio = domicilio.Save();

                empresa.IdDomicilio = idDomicilio.ToString();
                empresa.Telefono = txtTelefonoJuridica.Text;
                empresa.Mail = txtMailJuridica.Text;
                empresa.TipoCliente = Convert.ToString((Int16)tipoCliente.Persona_jurídica);
                empresa.Datos = "-"; //si se deja en null, aparece una excepcion de sql
                empresa.Papelera = 1;
                empresa.Comentarios = txtComentarioJuridica.Text;

                empresa.Apoderado = Convert.ToString((Int16)eApoderado.Si);
                empresa.IdEmpresa = "-1";

                if (pnlApoderadoJuridica.Visible == false)
                {
                    empresa.Apoderado = Convert.ToString((Int16)eApoderado.No);
                    empresa.IdEmpresa = "-1";
                }
                else
                {
                    cApoderado apoderado = new cApoderado();
                    apoderado.RazonSocial = txtNombreApoderadoJuridica.Text;
                    apoderado.Cuit = txtCuitApoderadoJuridica.Text;

                    apoderado.TipoDoc = cbTipoDocApoderadoJuridica.SelectedValue;
                    apoderado.Documento = txtNroDocApoderadoJuridica.Text;

                    cDomicilio domicilioApoderado = new cDomicilio(txtCalleApoderadoJuridica.Text, txtDireccionApoderadoJuridica.Text, txtCodPostalApoderadoJuridica.Text, cbProvinciaApoderadoJuridica.SelectedValue, txtCiudadApoderadoJuridica.Text);
                    int idDomicilioApoderado = domicilioApoderado.Save();

                    apoderado.IdDomicilio = idDomicilioApoderado.ToString();
                    apoderado.Telefono = txtTelefonoApoderadoJuridica.Text;
                    apoderado.Mail = txtMailApoderadoJuridica.Text;
                    apoderado.Papelera = 1;

                    idApoderado = apoderado.Save();

                    empresa.Apoderado = Convert.ToString((Int16)eApoderado.Si);
                    empresa.IdEmpresa = idApoderado.ToString();
                }

                //BORRAR
                empresa.IdEstadoCivil = 0;
                empresa.Clave = "-";
                empresa.Direccion = "-";
                empresa.IdEstado = 0;

                empresa.Save();
                Response.Redirect("Agenda.aspx");
            }
            #endregion
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Agenda.aspx");
        }
        #endregion

        #region Auxiliares
        public static bool ValidarCuit(string _cuit)
        {
            int sumatoria = 0;
            string[] cuit = (_cuit.Replace("-", "")).Select(c => c.ToString()).ToArray();

            int[] serie = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

            int sdfdf = cuit.Count();

            for (int i = 0; i < serie.Count(); i++)
            {
                if (sdfdf > i && cAuxiliar.IsNumeric(cuit[i]))
                    sumatoria += Convert.ToInt32(cuit[i]) * serie[i];
            }

            int mod = 11 - (sumatoria % 11);

            if (cuit.Last() == Convert.ToString(mod))
                return true;
            else
                return false;
        }
        #endregion
    }
}