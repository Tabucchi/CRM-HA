using DLL.Negocio;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class CuentaCorriente : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //#region Combo
                    //cbEmpresa.DataSource = cEmpresa.GetDataTable();
                    //cbEmpresa.DataValueField = "id";
                    //cbEmpresa.DataTextField = "nombre";
                    //cbEmpresa.DataBind();
                    //ListItem ce = new ListItem("Seleccione un cliente...", "0");
                    //cbEmpresa.Items.Insert(0, ce);
                    //ListItem cet = new ListItem("Todos", "1");
                    //cbEmpresa.Items.Insert(1, cet);
                    //#endregion

                    List<cCuentaCorrienteUsuario> cc = cCuentaCorrienteUsuario.GetCuentaCorriente();
                    lvCC.DataSource = cc;
                    lvCC.DataBind();
                    CalcularTotales();
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("CuentaCorriente - " + DateTime.Now + "- " + ex.Message + " - Page_Load");
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void cbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cbEmpresa.SelectedValue != "1")
            //{
            //    lvCC.DataSource = cCuentaCorrienteUsuario.GetListCuentaCorrienteByIdEmpresa(cbEmpresa.SelectedValue);
            //    lvCC.DataBind();
            //}
            //else
            //{
            //    lvCC.DataSource = cCuentaCorrienteUsuario.GetCuentaCorrienteConSaldo();
            //    lvCC.DataBind();
            //}
            //CalcularTotales();
        }

        private void CalcularTotales()
        {
            try
            {
                decimal _totalCtaCte = 0;
                decimal _totalDeuda = 0;
                decimal _total = 0;

                foreach (ListViewItem item in lvCC.Items)
                {
                    Label lbTotalCtaCte = item.FindControl("lbSaldo") as Label;
                    _totalCtaCte += Convert.ToDecimal(lbTotalCtaCte.Text);

                    Label lbTotalDeuda = item.FindControl("lbTotalDeuda") as Label;
                    _totalDeuda += Convert.ToDecimal(lbTotalDeuda.Text);

                    Label lbTotal = item.FindControl("lbTotal") as Label;
                    _total += Convert.ToDecimal(lbTotal.Text);
                }

                Label lblTotalCtaCte = (Label)lvCC.FindControl("lbTotalCtaCte");
                lblTotalCtaCte.Text = String.Format("{0:#,#0.00}", _totalCtaCte);

                Label lblTotalDeuda = (Label)lvCC.FindControl("lbTotalTotalDeuda");
                lblTotalDeuda.Text = String.Format("{0:#,#0.00}", _totalDeuda);

                Label lblTotal = (Label)lvCC.FindControl("lbTotalTotal");
                lblTotal.Text = String.Format("{0:#,#0.00}", _total);
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("CuotasCliente - " + DateTime.Now + "- " + ex.Message + " - CalcularTotales" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string[] txt = txtSearch.Text.Split(' ');
                string nombreEmpresa = txtSearch.Text;

                if (txt != null)
                {
                    int index = txt.Count() - 1;

                    List<cCuentaCorrienteUsuario> ccu = cCuentaCorrienteUsuario.GetListCuentaCorrienteByEmpresa(txt[0].ToString(), txt[index].ToString());

                    lvCC.DataSource = ccu;
                    lvCC.DataBind();

                    if(ccu.Count != 0)
                        CalcularTotales();
                }
            }
            catch
            {
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            lvCC.DataSource = cCuentaCorrienteUsuario.GetCuentaCorriente();
            lvCC.DataBind();
            CalcularTotales();
        }
    }
}