using DLL.Negocio;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm.Controles
{
    public partial class Cuotas : System.Web.UI.UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int cantCuotas;
        private decimal total;

        #region Propiedades
        public int CantCuotas
        {
            get { return cantCuotas; }
            set { cantCuotas = value; }
        }

        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }

        public ListView listView
        {
            get
            {
                return lvCuotas;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        #region ListView
        protected void lvCuotas_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            cCuota cuota = cCuota.Load(e.CommandArgument.ToString());

            switch (e.CommandName)
            {
                case "Comentario":
                    {
                        hfCuota.Value = id;
                        rptComentariosCuota.DataSource = cComentarioCC.GetComentariosByIdCuota(id);
                        rptComentariosCuota.DataBind();
                        ModalPopupExtender.Show();
                        break;
                    }
                case "Recibos":
                    {
                        lbNroCuota.Text = cuota.Nro.ToString();
                        lvRecibos.DataSource = cReciboCuota.GetRecibosByNroCuota(cuota.IdCuentaCorriente, cuota.Nro.ToString(), cuota.IdFormaPagoOV); ;
                        lvRecibos.DataBind();
                        modalRecibos.Show();
                        break;
                    }

            }
        }
        #endregion
                
        #region Comentario
        protected void tempo_Click(object sender, EventArgs e)
        {
            return;
        }

        protected void btnAgregarComentario_Click(object sender, EventArgs e)
        {
            string aux = "";
            string idCuota = "";

            if (string.IsNullOrEmpty(txtComentarioCuota.Text))
                return;
            else
            {
                aux = txtComentarioCuota.Text;
                idCuota = !string.IsNullOrEmpty(hfCuota.Value) ? hfCuota.Value : "0";
            }

            cComentarioCC comentario = new cComentarioCC(HttpContext.Current.User.Identity.Name, Request["idCC"].ToString(), idCuota, aux);
            comentario.Save();

            rptComentariosCuota.DataSource = cComentarioCC.GetComentariosByIdCuota(idCuota);
            rptComentariosCuota.DataBind();

            txtComentarioCuota.Text = "";
        }
        #endregion

        protected void lvRecibos_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            cItemCCU itemCCU = cItemCCU.Load(e.CommandArgument.ToString());
            string _idEmpresa = cCuentaCorrienteUsuario.Load(itemCCU.IdCuentaCorrienteUsuario).IdEmpresa;

            CrearPdfRecibo(itemCCU, _idEmpresa);
        }
        
        protected void CrearPdfRecibo(cItemCCU _itemCCU, string _idEmpresa)
        {
            cReciboCuota recibo;
            if (string.IsNullOrEmpty(cReciboCuota.GetNroReciboByIdItemCCU(_itemCCU.Id)))
                recibo = cReciboCuota.CrearRecibo("-1", _itemCCU.Id, _itemCCU.Credito + _itemCCU.Debito);
            else
                recibo = cReciboCuota.GetReciboByNro(_itemCCU.GetRecibo);

            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Comprobantes\\Recibos\\";
            string filename = "Recibo_" + recibo.Nro + ".pdf";

            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("fecha", String.Format("{0:dd/MM/yyyy}", recibo.Fecha));
            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("recibo", _itemCCU.GetRecibo);
            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("fechaImpresion", String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now));

            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("cliente", cEmpresa.Load(_idEmpresa).GetNombreCompleto);

            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("monto", String.Format("{0:#,#0.00}", recibo.Monto) + ".-");
            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("montoLetras", cAuxiliar.enLetras(recibo.Monto.ToString()) + ".-");

            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("concepto", _itemCCU.Concepto);
            CrystalReportSourceRecibo.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            string url = "http://crm.haemprendimientos.com.ar/Archivos/Comprobantes/Recibos/" + "Recibo_" + recibo.Nro + ".pdf";
            Response.Write("<script>window.open('" + url + "','_blank');</script>");
        }

        protected void btnCerrarRecibos_Click(object sender, EventArgs e)
        {
            modalRecibos.Hide();
        }
    }
}