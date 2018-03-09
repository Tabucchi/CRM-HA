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
    public partial class ResumenSaldo : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListarCuotasCAC();
                ListarCuotasUVA();

                #region CAC
                cIndiceCAC previousIndice = cIndiceCAC.Load(cIndiceCAC.GetPreviousIndice().ToString());
                cIndiceCAC nuevoIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());

                lbTotalVariacionCac.Text = String.Format("{0:#,#0.00}", cCuota.CalcularVariacionMensualCAC(previousIndice.Id, nuevoIndice.Id, true)) + " %";
                #endregion

                #region UVA
                cUVA previousIndiceUva = cUVA.Load(cUVA.GetPreviousIndice().ToString());
                cUVA nuevoIndiceUVA = cUVA.Load(cUVA.GetLastIdIndice().ToString());

                lbTotalVariacionUva.Text = String.Format("{0:#,#0.00}", cCuota.CalcularVariacionMensualUVA(previousIndiceUva.Id, nuevoIndiceUVA.Id, true)) + " %";
                #endregion

                CalcularTotales();
            }
        }

        public void ListarCuotasCAC()
        {
            cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
            List<cCuota> cuotas = cCuota.GetCuotasActivaByFechaConCAC(lastIndice.Fecha.AddMonths(2));

            lvSaldosCAC.DataSource = cuotas;
            lvSaldosCAC.DataBind();
        }

        public void ListarCuotasUVA()
        {
            cUVA lastIndice = cUVA.Load(cUVA.GetLastIdIndice().ToString());
            List<cCuota> cuotas = cCuota.GetCuotasActivaByFechaConUVA(lastIndice.Fecha.AddMonths(1));

            lvSaldosUVA.DataSource = cuotas;
            lvSaldosUVA.DataBind();
        }

        private void CalcularTotales()
        {
            try
            {
                #region CAC
                decimal _totalSaldoAnterior = 0;
                decimal _totalSaldoActual = 0;

                foreach (ListViewItem item in lvSaldosCAC.Items)
                {
                    Label lbSaldoAnterior = item.FindControl("lbSaldoAnterior") as Label;
                    Label lbSaldoActual = item.FindControl("lbSaldoActual") as Label;

                    _totalSaldoAnterior += Convert.ToDecimal(lbSaldoAnterior.Text);
                    _totalSaldoActual += Convert.ToDecimal(lbSaldoActual.Text);
                }

                Label lblTotalVariacion = (Label)lvSaldosCAC.FindControl("lbTotalVariacion");
                Label lblTotalSaldoAnterior = (Label)lvSaldosCAC.FindControl("lbTotalSaldoAnterior");
                Label lblTotalSaldoActual = (Label)lvSaldosCAC.FindControl("lbTotalSaldoActual");

                lblTotalSaldoAnterior.Text = String.Format("{0:#,#0.00}",Convert.ToDecimal( _totalSaldoAnterior.ToString()));
                lblTotalSaldoActual.Text = String.Format("{0:#,#0.00}", Convert.ToDecimal(_totalSaldoActual.ToString()));

                decimal totalIndice = (Convert.ToDecimal(_totalSaldoActual.ToString()) / Convert.ToDecimal(_totalSaldoAnterior.ToString()) * 100) - 100;

                lblTotalVariacion.Text = String.Format("{0:#,#0.00}", totalIndice);
                #endregion

                #region UVA
                decimal _totalSaldoAnteriorUVA = 0;
                decimal _totalSaldoActualUVA = 0;

                foreach (ListViewItem item in lvSaldosUVA.Items)
                {
                    Label lbSaldoAnteriorUVA = item.FindControl("lbSaldoAnteriorUVA") as Label;
                    Label lbSaldoActualUVA = item.FindControl("lbSaldoActualUVA") as Label;

                    _totalSaldoAnteriorUVA += Convert.ToDecimal(lbSaldoAnteriorUVA.Text);
                    _totalSaldoActualUVA += Convert.ToDecimal(lbSaldoActualUVA.Text);
                }

                Label lblTotalVariacionUVA = (Label)lvSaldosUVA.FindControl("lbTotalVariacionUVA");
                Label lblTotalSaldoAnteriorUVA = (Label)lvSaldosUVA.FindControl("lbTotalSaldoAnteriorUVA");
                Label lblTotalSaldoActualUVA = (Label)lvSaldosUVA.FindControl("lbTotalSaldoActualUVA");

                lblTotalSaldoAnteriorUVA.Text = String.Format("{0:#,#0.00}", Convert.ToDecimal(_totalSaldoAnteriorUVA.ToString()));
                lblTotalSaldoActualUVA.Text = String.Format("{0:#,#0.00}", Convert.ToDecimal(_totalSaldoActualUVA.ToString()));

                decimal totalIndiceUVA = (Convert.ToDecimal(_totalSaldoActualUVA.ToString()) / Convert.ToDecimal(_totalSaldoAnteriorUVA.ToString()) * 100) - 100;

                lblTotalVariacionUVA.Text = String.Format("{0:#,#0.00}", totalIndiceUVA);
                #endregion
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("ResumenSaldo - " + DateTime.Now + "- " + ex.Message + " - CalcularTotales" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
    }
}