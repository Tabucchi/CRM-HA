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
                CargaCombo();
                CargaListView(null);
            }
        }

        #region Carga
        public void CargaListView(string _idEmpresa)
        {
            cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
            if (lastIndice.Fecha.Month == DateTime.Now.AddMonths(-1).Month)
            {
                lvCuotas.DataSource = cCuota.GetEnvioCuotasMes((Int16)estadoCuenta_Cuota.Activa, lastIndice.Fecha.AddMonths(2), _idEmpresa);
                lvCuotas.DataBind();
            }
            else
            {
                lvCuotas.DataSource = cCuota.GetEnvioCuotasMes((Int16)estadoCuenta_Cuota.Activa, lastIndice.Fecha.AddMonths(2), _idEmpresa);
                lvCuotas.DataBind();
            }
        }

        public void CargaCombo()
        {
            cbEmpresa.DataSource = cEmpresa.GetDataTable();
            cbEmpresa.DataValueField = "id";
            cbEmpresa.DataTextField = "nombre";
            cbEmpresa.DataBind();
            ListItem ce = new ListItem("Seleccione un cliente...", "0");
            cbEmpresa.Items.Insert(0, ce);
        }
        #endregion

        #region Filtro
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                CargaListView(cbEmpresa.SelectedValue);
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("EnvioCuotas - " + DateTime.Now + "- " + ex.Message + " - btnBuscar_Click");
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            try
            {
                IniciarFiltro();
                CargaListView(null);
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("ListaOperacionVenta - " + DateTime.Now + "- " + ex.Message + " - btnVerTodos_Click");
                Response.Redirect("MensajeError.aspx");
            }
        }

        public void IniciarFiltro()
        {
            cbEmpresa.SelectedValue = "0";
        }
        #endregion

        #region Envio
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
                                cCuota cuota = cCuota.Load(idCuota.Text);
                                cuota.Enviado = true;
                                cuota.Save();
                                Thread.Sleep(500);
                            }
                            catch (Exception ex1)
                            {
                                HtmlControl ttError = (HtmlControl)item.FindControl("idTr");
                                ttError.Style.Add("background-color", "#ffcdd2");

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
                            tt.Style.Add("background-color", "#ffcdd2");

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
        #endregion
    }
}