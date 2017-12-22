using DLL;
using DLL.Auxiliares;
using DLL.Negocio;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class UnidadesVendidas : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            lvUnidades.DataSource = CargarObras();
            lvUnidades.DataBind();

            CalcularTotales();
        }

        #region ListView
        public List<cUnidadesVendidas> CargarObras()
        {
            List<cProyecto> list4 = cProyecto.GetProyectos();
            List<cUnidadesVendidas> unidadesVendidas = new List<cUnidadesVendidas>();
            
            foreach (var item in list4)
            {
                DataTable dtUnidades = cUnidad.GetUnidadesVendidas(item.Id);

                decimal precioAcordado = 0;
                decimal supTotal = 0;

                cUnidadesVendidas unidades = new cUnidadesVendidas();
                unidades.idProyecto = item.Id;

                foreach (DataRow row in dtUnidades.Rows)
                {
                    if (row["moneda"].ToString() == Convert.ToString((Int16)tipoMoneda.Dolar))
                        precioAcordado += Convert.ToDecimal(row["precio"].ToString());
                    else
                        precioAcordado += Convert.ToDecimal(row["precio"].ToString()) / Convert.ToDecimal(row["valorDolar"].ToString());

                    supTotal += Convert.ToDecimal(row["sup"].ToString());
                }

                unidades.cantidad = dtUnidades.Rows.Count;
                unidades.cantidadSinBoleto = cUnidad.GetCantidadUnidadesVendidas(item.Id) - dtUnidades.Rows.Count;

                if (precioAcordado != 0 || supTotal != 0)
                    unidades.valorM2 = String.Format("{0:#,#0.00}", precioAcordado / supTotal);
                else
                    unidades.valorM2 = String.Format("{0:#,#0.00}", 0);
                unidades.precioAcordado = String.Format("{0:#,#0.00}", precioAcordado);
                unidades.supTotal = String.Format("{0:#,#0.00}", supTotal);
                
                unidadesVendidas.Add(unidades);
            }

            return unidadesVendidas;
        }

        private void CalcularTotales()
        {
            try
            {
                decimal _totalCantidad = 0;
                decimal _totalValorM2 = 0;
                decimal _totalSup = 0;
                decimal _totalPrecio = 0;

                foreach (ListViewItem item in lvUnidades.Items)
                {
                    Label lbCantidad = item.FindControl("lbCantidad") as Label;
                    Label lbValorM2 = item.FindControl("lbValorM2") as Label;
                    Label lbSup = item.FindControl("lbSup") as Label;
                    Label lbPrecio = item.FindControl("lbPrecio") as Label;

                    _totalCantidad += Convert.ToDecimal(lbCantidad.Text);
                    _totalValorM2 += Convert.ToDecimal(lbValorM2.Text);
                    _totalSup += Convert.ToDecimal(lbSup.Text);
                    _totalPrecio += Convert.ToDecimal(lbPrecio.Text);
                }

                Label lblTotalCantidad = (Label)lvUnidades.FindControl("lbTotalCantidad");
                Label lblTotalValorM2 = (Label)lvUnidades.FindControl("lbTotalValorM2");
                Label lblTotalSupTotal = (Label)lvUnidades.FindControl("lbTotalSupTotal");
                Label lblTotalMonto = (Label)lvUnidades.FindControl("lbTotalPrecio");

                lblTotalCantidad.Text = _totalCantidad.ToString();
                lblTotalValorM2.Text = String.Format("{0:#,#0.00}", _totalValorM2);
                lblTotalSupTotal.Text = String.Format("{0:#,#0.00}", _totalSup);
                lblTotalMonto.Text = String.Format("{0:#,#0.00}", _totalPrecio);
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("UnidadesVendidas - " + DateTime.Now + "- " + ex.Message + " - CalcularTotales" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Descargar
        private DataSet CrearDataTableUnidadesVendidas()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            DataSet ds = new DataSet();

            dt.Columns.Add(new DataColumn("proyecto"));
            dt.Columns.Add(new DataColumn("cantidad"));
            dt.Columns.Add(new DataColumn("valorM2"));
            dt.Columns.Add(new DataColumn("supTotal"));
            dt.Columns.Add(new DataColumn("precio"));
            
            foreach (ListViewItem item in lvUnidades.Items)
            {
                Label lbProyecto = item.FindControl("lbProyecto") as Label;
                Label lbCantidad = item.FindControl("lbCantidad") as Label;
                Label lbValorM2 = item.FindControl("lbValorM2") as Label;
                Label lbSupTotal = item.FindControl("lbSup") as Label;
                Label lbPrecio = item.FindControl("lbPrecio") as Label;

                dr = dt.NewRow();
                dr["proyecto"] = lbProyecto.Text;
                dr["cantidad"] = lbCantidad.Text;
                dr["valorM2"] = String.Format("{0:#,#0.00}",lbValorM2.Text);
                dr["supTotal"] = String.Format("{0:#,#0.00}",lbSupTotal.Text);
                dr["precio"] = String.Format("{0:#,#0.00}", lbPrecio.Text);
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "tUnidadesVendidas";

            Label lblTotalCantidad = (Label)lvUnidades.FindControl("lbTotalCantidad");
            Label lblTotalValorM2 = (Label)lvUnidades.FindControl("lbTotalValorM2");
            Label lblTotalSupTotal = (Label)lvUnidades.FindControl("lbTotalSupTotal");
            Label lblTotalMonto = (Label)lvUnidades.FindControl("lbTotalPrecio");

            hfTotalCantidad.Value = String.Format("{0:#,#0.00}", lblTotalCantidad.Text);
            hfTotalValorM2.Value = String.Format("{0:#,#0.00}", lblTotalValorM2.Text);
            hfTotalSupTotal.Value = String.Format("{0:#,#0.00}", lblTotalSupTotal.Text);
            hfTotalPrecio.Value = String.Format("{0:#,#0.00}", lblTotalMonto.Text);

            return ds;
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath;
            string filename = "UnidadesVendidas.pdf";

            // Planilla
            DataSetUnidades ds = new DataSetUnidades();
            ds.Merge(CrearDataTableUnidadesVendidas(), false, System.Data.MissingSchemaAction.Ignore);
            CrystalReportSource.ReportDocument.SetDataSource(ds);

            CrystalReportSource.ReportDocument.SetParameterValue("totalCantidad", hfTotalCantidad.Value);
            CrystalReportSource.ReportDocument.SetParameterValue("totalValorM2", hfTotalValorM2.Value);
            CrystalReportSource.ReportDocument.SetParameterValue("totalSupTotal", hfTotalSupTotal.Value);
            CrystalReportSource.ReportDocument.SetParameterValue("totalPrecio", hfTotalPrecio.Value);
            CrystalReportSource.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);

            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }
        #endregion
    }
}