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
    public partial class PendientesPrecios : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Gerencia)
                {
                    lvActualizacionPrecios.DataSource = cActualizarPrecio.GetPrecios();
                    lvActualizacionPrecios.DataBind();
                }
                else
                    Response.Redirect("Default.aspx");
            }
        }

        #region Validar
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem item in lvActualizacionPrecios.Items)
                {
                    CheckBox check = item.FindControl("chBox") as CheckBox;

                    if (check.Checked)
                    {
                        Label id = item.FindControl("lbId") as Label;
                        Label codUf = item.FindControl("lbCodUF") as Label;
                        Label idProyecto = item.FindControl("lbIdProyecto") as Label;
                        Label lbValorViejo = item.FindControl("lbValorViejo") as Label;
                        TextBox lbValorNuevo = item.FindControl("lbValorNuevo") as TextBox;
                        cUnidad unidad = cUnidad.LoadByCodUF(codUf.Text, idProyecto.Text);

                        unidad.PrecioBase = Convert.ToDecimal(lbValorNuevo.Text);
                        unidad.Save();

                        cActualizarPrecio precio = cActualizarPrecio.Load(id.Text);
                        precio.Estado = (Int16)estadoActualizarPrecio.Aceptar;
                        precio.Save();

                        #region Se registra el cambio
                        string valorViejo = null;
                        if (!string.IsNullOrEmpty(lbValorViejo.Text))
                            valorViejo = lbValorViejo.Text;
                        else
                            valorViejo = "0";
                        cHistorial _historial = new cHistorial(DateTime.Now, historial.Evolución_de_precios.ToString(), Convert.ToDecimal(valorViejo), Convert.ToDecimal(lbValorNuevo.Text), codUf.Text, unidad.NroUnidad, unidad.IdEstado, unidad.IdEstado, HttpContext.Current.User.Identity.Name, idProyecto.Text);
                        _historial.Save();
                        #endregion
                    }
                }

                lvActualizacionPrecios.DataSource = cActualizarPrecio.GetPrecios();
                lvActualizacionPrecios.DataBind();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("PendientesPrecios - " + DateTime.Now + "- " + ex.Message + " - btnConfirmar_Click");
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Cancelar operaciones de ventas
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlMensajeCancelar.Visible = true;
        }

        protected void btnCancelarSi_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem item in lvActualizacionPrecios.Items)
                {
                    CheckBox check = item.FindControl("chBox") as CheckBox;

                    if (check.Checked)
                    {
                        Label id = item.FindControl("lbId") as Label;
                        cActualizarPrecio precio = cActualizarPrecio.Load(id.Text);
                        precio.Estado = (Int16)estadoActualizarPrecio.Cancelar;
                        precio.Save();
                    }
                }

                lvActualizacionPrecios.DataSource = cActualizarPrecio.GetPrecios();
                lvActualizacionPrecios.DataBind();

                pnlMensajeCancelar.Visible = false;
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("PendientesPrecios - " + DateTime.Now + "- " + ex.Message + " - btnCancelarSi_Click");
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnCancelarNo_Click(object sender, EventArgs e)
        {
            pnlMensajeCancelar.Visible = false;
        }
        #endregion
    }
}