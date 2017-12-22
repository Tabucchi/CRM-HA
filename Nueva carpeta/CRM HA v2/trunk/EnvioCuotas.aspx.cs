using DLL.Negocio;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class EnvioCuotas : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
                if (lastIndice.Fecha.Month == DateTime.Now.AddMonths(-1).Month)
                {
                    lvCuotas.DataSource = cCuota.GetCuotasMes((Int16)estadoCuenta_Cuota.Activa, lastIndice.Fecha.AddMonths(2));
                    lvCuotas.DataBind();
                }
                else
                {
                    lvCuotas.DataSource = cCuota.GetCuotasMes((Int16)estadoCuenta_Cuota.Activa, lastIndice.Fecha.AddMonths(2));
                    lvCuotas.DataBind();
                }
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem item in lvCuotas.Items)
                {
                    CheckBox check = item.FindControl("chBoxOV") as CheckBox;

                    Label empres1a = item.FindControl("lbEmpresa") as Label;

                    if (check.Checked)
                    {
                        Label empresa = item.FindControl("lbEmpresa") as Label;
                        Label mail = item.FindControl("lbMail") as Label;
                        Label idCuota = item.FindControl("lbIdCuota") as Label;

                        cSendMail send = new cSendMail();
                        if (!string.IsNullOrEmpty(mail.Text) && cAuxiliar.ValidarMail(mail.Text))
                        {
                            try
                            {
                                send.EnviarAvisoCuota(empresa.Text, mail.Text, idCuota.Text);
                                Thread.Sleep(500);
                            }
                            catch
                            {
                                HtmlControl ttError = (HtmlControl)item.FindControl("idTr");
                                ttError.Style.Add("background-color", "#c8e6c9");

                                Label estadoError = item.FindControl("lbEstado") as Label;
                                estadoError.Text = "Error";
                                estadoError.Style.Add("font-weight", "bold");

                                pnlMensajeError.Visible = true;
                            }

                            HtmlControl tt = (HtmlControl)item.FindControl("idTr");
                            tt.Style.Add("background-color", "#b9f6ca");
                        }
                        else
                        {
                            HtmlControl tt = (HtmlControl)item.FindControl("idTr");
                            tt.Style.Add("background-color", "#f2dede");

                            pnlMensajeError.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("EnvioCuotas - " + DateTime.Now + "- " + ex.Message + " - btnEnviar_Click");
                Response.Redirect("MensajeError.aspx");
            }
        }
    }
}