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
            List<cCuota> cuotas = cCuota.GetCuotasActivaByFechaConUVA(lastIndice.Fecha);

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
                lblTotalVariacion.Text = String.Format("{0:#,#0.00}", Convert.ToDecimal(_totalSaldoAnterior.ToString()) / Convert.ToDecimal(_totalSaldoActual.ToString()));
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
                lblTotalVariacionUVA.Text = String.Format("{0:#,#0.00}", Convert.ToDecimal(_totalSaldoAnteriorUVA.ToString()) / Convert.ToDecimal(_totalSaldoActualUVA.ToString()));
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