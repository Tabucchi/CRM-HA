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
    public partial class PendientesOperacionesVenta : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Gerencia)
                {
                    lvOperacionVenta.DataSource = cOperacionVenta.GetOV_AConfirmar();
                    lvOperacionVenta.DataBind();
                }
                else
                    Response.Redirect("Default.aspx");
            }
        }

        #region Validar
        protected void btnConfirmarOV_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem item in lvOperacionVenta.Items)
                {
                    CheckBox check = item.FindControl("chBoxOV") as CheckBox;

                    if (check.Checked)
                    {
                        Label id = item.FindControl("lbId") as Label;
                        cOperacionVenta ov = cOperacionVenta.Load(id.Text);

                        ov.IdEstado = (Int16)estadoOperacionVenta.Activo;
                        ov.Save();
                    }
                }

                lvOperacionVenta.DataSource = cOperacionVenta.GetOV_AConfirmar();
                lvOperacionVenta.DataBind();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("PendientesOperacionesVenta - " + DateTime.Now + "- " + ex.Message + " - btnConfirmarOV_Click");
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
                foreach (ListViewItem item in lvOperacionVenta.Items)
                {
                    CheckBox check = item.FindControl("chBoxOV") as CheckBox;

                    if (check.Checked)
                    {
                        Label id = item.FindControl("lbId") as Label;
                        cOperacionVenta ov = cOperacionVenta.Load(id.Text);
                        ov.IdEstado = (Int16)estadoOperacionVenta.Cancelado;
                        ov.Save();

                        cUnidad unidad = cUnidad.LoadByIdEmpresaUnidad(ov.IdEmpresaUnidad);
                        unidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Disponible);
                        unidad.IdUsuario = "-1";
                        unidad.Save();

                        cEmpresaUnidad eu = cEmpresaUnidad.Load(ov.IdEmpresaUnidad);
                        eu.Papelera = (Int16)papelera.Eliminado;
                        eu.Save();
                    }
                }

                lvOperacionVenta.DataSource = cOperacionVenta.GetOV_AConfirmar();
                lvOperacionVenta.DataBind();

                pnlMensajeCancelar.Visible = false;
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("PendientesOperacionesVenta - " + DateTime.Now + "- " + ex.Message + " - btnCancelarSi_Click");
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