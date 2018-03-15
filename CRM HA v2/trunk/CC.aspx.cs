using Common.Logging;
using DLL.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class CC : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargarCombo();

                    List<cCuentaCorriente> cc = cCuentaCorriente.GetCuentaCorrienteLastTransactions();
                    lvCC.DataSource = cc.Reverse<cCuentaCorriente>();
                    lvCC.DataBind();
                    CalcularTotales(cc);
                    
                    ddlEstado.SelectedValue = Convert.ToString((Int16)estadoCuenta_Cuota.Activa);
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Historial de Pagos - " + DateTime.Now + "- " + ex.Message + " - Page_Load");
                Response.Redirect("MensajeError.aspx");
            }
        }
        
        #region Combo
        public void CargarCombo()
        {
            ddlEstado.DataSource = cCuentaCorriente.CargarEstadoCuenta_Cuota();
            ddlEstado.DataValueField = "value";
            ddlEstado.DataTextField = "text";
            ddlEstado.DataBind();

            ListItem ie = new ListItem("Todas", "0");
            ddlEstado.Items.Insert(0, ie);

            //Obra
            ddlObra.DataSource = cProyecto.GetDataTable();
            ddlObra.DataValueField = "id";
            ddlObra.DataTextField = "descripcion";
            ddlObra.DataBind();
            ListItem ip = new ListItem("Todas", "0");
            ddlObra.Items.Insert(0, ip);
            ddlObra.SelectedIndex = 0;

            //Cliente
            ddlEmpresa.DataSource = cEmpresa.GetDataTable();
            ddlEmpresa.DataValueField = "id";
            ddlEmpresa.DataTextField = "nombre";
            ddlEmpresa.DataBind();
            //Agrego item TODAS
            ListItem it = new ListItem("Todas", "0");
            ddlEmpresa.Items.Insert(0, it);

            //Moneda
            cbMonedaIndice.DataSource = cOperacionVenta.CargarComboMonedaIndiceOV();
            cbMonedaIndice.DataBind();
        }
        #endregion
        
        #region Botones
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string cliente = ddlEmpresa.SelectedValue == "0" ? "" : ddlEmpresa.SelectedValue;
            string obra = ddlObra.SelectedValue == "0" ? "" : ddlObra.SelectedValue;
            //string moneda = ddlMoneda.SelectedValue == "Seleccione una moneda..." ? "-1" : ddlMoneda.SelectedValue;

            List<cCuentaCorriente> cc = cCuentaCorriente.GetCuentaCorriente(cliente, Convert.ToInt16(ddlEstado.SelectedValue), obra, (Int16)cbMonedaIndice.SelectedIndex);
            lvCC.DataSource = cc;
            lvCC.DataBind();
            if(cc.Count != 0 && cc != null)
                CalcularTotales(cc);
        }

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            List<cCuentaCorriente> cc = cCuentaCorriente.GetCuentaCorriente("", Convert.ToInt16(ddlEstado.SelectedValue), "", (Int16)cbMonedaIndice.SelectedIndex);
            lvCC.DataSource = cc;
            lvCC.DataBind();
        }
        #endregion

        #region Auxiliares
        private void CalcularTotales(List<cCuentaCorriente> cc)
        {
            try
            {
                decimal _valorVenta = 0;
                int _dolar = 0;
                int _pesosCAC = 0;
                decimal _pesosUVA = 0;
                foreach (ListViewItem item in lvCC.Items)
                {
                    Label lbTotalPesos = item.FindControl("lbTotalPesos") as Label;
                    Label lbCantCuotasDolar = item.FindControl("lbCantCuotasDolar") as Label;
                    Label lbCantCuotasPesosCAC = item.FindControl("lbCantCuotasPesosCAC") as Label;
                    Label lbCantCuotasPesosUVA = item.FindControl("lbCantCuotasPesosUVA") as Label;

                    _valorVenta += Convert.ToDecimal(lbTotalPesos.Text);
                    _dolar += Convert.ToInt16(lbCantCuotasDolar.Text);
                    _pesosCAC += Convert.ToInt16(lbCantCuotasPesosCAC.Text);
                    _pesosUVA += Convert.ToInt16(lbCantCuotasPesosUVA.Text);
                }

                Label lblValorVenta = (Label)lvCC.FindControl("lbValorVenta");
                Label lblValorDolar = (Label)lvCC.FindControl("lbValorDolar");
                Label lblValorCAC = (Label)lvCC.FindControl("lbValorCAC");
                Label lblValorUVA = (Label)lvCC.FindControl("lbValorUVA");

                lblValorVenta.Text = String.Format("{0:#,#0.00}", _valorVenta);
                lblValorDolar.Text = String.Format("{0:#,#0}", _dolar);
                lblValorCAC.Text = String.Format("{0:#,#0}", _pesosCAC);
                lblValorUVA.Text = String.Format("{0:#,#0}", _pesosUVA);
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Cuentas Corrientes - " + DateTime.Now + "- " + ex.Message + " - CalcularTotales" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        
    }
}