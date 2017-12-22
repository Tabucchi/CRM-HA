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
    public partial class ResumenObra : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lvProyectos.DataSource = CargarObras();
                lvProyectos.DataBind();

                CalcularTotales();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("ResumenObra - " + DateTime.Now + "- " + ex.Message + " - Page_Load" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        #region ListView
        public List<cResumenObra> CargarObras()
        {
            List<cProyecto> list4 = cProyecto.GetProyectos();
            List<cResumenObra> unidades = new List<cResumenObra>();
            decimal totalPorcentaje = 0;

            foreach (var item in list4)
            {
                decimal cantidadTotal = Convert.ToDecimal(cUnidad.GetUnidadesByIdProyectoSinUnidadesModificadas(item.Id).Count);

                cResumenObra resumen = new cResumenObra();
                resumen.idObra = item.Id;
                resumen.obra = item.Descripcion;

                decimal disponible = cUnidad.GetUnidadesByEstado((Int16)estadoUnidad.Disponible, item.Id);
                resumen.disponible = disponible;
                string porcentajeDisponible = CalcularPorcentaje(disponible, cantidadTotal);
                resumen.porcentajeDisponible = porcentajeDisponible;
                totalPorcentaje += Convert.ToDecimal(porcentajeDisponible);

                decimal reservada = cUnidad.GetUnidadesByEstado((Int16)estadoUnidad.Reservado, item.Id);
                resumen.reservada = reservada;
                string porcentajeReservada = CalcularPorcentaje(reservada, cantidadTotal);
                resumen.porcentajeReservada = porcentajeReservada;
                totalPorcentaje += Convert.ToDecimal(porcentajeReservada);

                decimal vendida_sin_boleto = cUnidad.GetUnidadesByEstado((Int16)estadoUnidad.Vendido_sin_boleto, item.Id);
                resumen.vendida_sin_boleto = vendida_sin_boleto;
                string porcentajeVendidaSinBoleto = CalcularPorcentaje(vendida_sin_boleto, cantidadTotal);
                resumen.porcentajeVendidaSinBoleto = porcentajeVendidaSinBoleto;
                totalPorcentaje += Convert.ToDecimal(porcentajeVendidaSinBoleto);

                decimal vendida = cUnidad.GetUnidadesByEstado((Int16)estadoUnidad.Vendido, item.Id);
                resumen.vendida = vendida;
                string porcentajeVendida = CalcularPorcentaje(vendida, cantidadTotal);
                resumen.porcentajeVendida = porcentajeVendida;
                totalPorcentaje += Convert.ToDecimal(porcentajeVendida);
               
                resumen.total = cantidadTotal;

                #region Redondeo en el caso que la suma de los porcentajes no sea igual 100%. Le sumo o resto la diferencia al estado con menos unidades
                if (cantidadTotal != 0)
                {
                    if (100 != totalPorcentaje)
                    {
                        DataTable dt = cUnidad.GetMinCantidadEstado(item.Id);
                        DataRow dr = dt.Rows[0];
                        switch (dr[1].ToString())
                        {
                            case "1":
                                resumen.porcentajeDisponible = String.Format("{0:#,#0.00}", Convert.ToDecimal(resumen.porcentajeDisponible) + (100 - totalPorcentaje));
                                break;
                            case "2":
                                resumen.porcentajeReservada = String.Format("{0:#,#0.00}", Convert.ToDecimal(resumen.porcentajeReservada) + (100 - totalPorcentaje));
                                break;
                            case "3":
                                resumen.porcentajeVendidaSinBoleto = String.Format("{0:#,#0.00}", Convert.ToDecimal(resumen.porcentajeVendidaSinBoleto) + (100 - totalPorcentaje));
                                break;
                            case "8":
                                resumen.porcentajeVendida = String.Format("{0:#,#0.00}", Convert.ToDecimal(resumen.porcentajeVendida) + (100 - totalPorcentaje));
                                break;
                        }
                    }
                }
                #endregion

                unidades.Add(resumen);
                totalPorcentaje = 0;
            }

            return unidades;
        }

        private void CalcularTotales()
        {
            try
            {
                decimal _totalDisponible = 0;
                decimal _totalReservada = 0;
                decimal _totalVendidaSinBoleto = 0;
                decimal _totalVendida = 0;
                decimal _totalTotal = 0;

                foreach (ListViewItem item in lvProyectos.Items)
                {
                    Label lbDisponible = item.FindControl("lbDisponible") as Label;
                    Label lbReservada = item.FindControl("lbReservada") as Label;
                    Label lbVendidaSinBoleto = item.FindControl("lbVendidaSinBoleto") as Label;
                    Label lbVendida = item.FindControl("lbVendida") as Label;
                    Label lbTotal = item.FindControl("lbTotal") as Label;

                    _totalDisponible += Convert.ToDecimal(lbDisponible.Text);
                    _totalReservada += Convert.ToDecimal(lbReservada.Text);
                    _totalVendidaSinBoleto += Convert.ToDecimal(lbVendidaSinBoleto.Text);
                    _totalVendida += Convert.ToDecimal(lbVendida.Text);
                    _totalTotal += Convert.ToDecimal(lbTotal.Text);
                }

                Label lblTotalDisponible = (Label)lvProyectos.FindControl("lbTotalDisponible");
                Label lblTotalReservada = (Label)lvProyectos.FindControl("lbTotalReservada");
                Label lblTotalVendidaSinBoleto = (Label)lvProyectos.FindControl("lbTotalVendidaSinBoleto");
                Label lblTotalVendida = (Label)lvProyectos.FindControl("lbTotalVendida");
                Label lblTotalTotal = (Label)lvProyectos.FindControl("lbTotalTotal");

                lblTotalDisponible.Text = _totalDisponible.ToString();
                lblTotalReservada.Text = _totalReservada.ToString();
                lblTotalVendidaSinBoleto.Text = _totalVendidaSinBoleto.ToString();
                lblTotalVendida.Text = _totalVendida.ToString();
                lblTotalTotal.Text = _totalTotal.ToString();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("ResumenObra - " + DateTime.Now + "- " + ex.Message + " - CalcularTotales" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Auxiliar
        public string CalcularPorcentaje(decimal cantidad, decimal total)
        {
            decimal porcentaje = 0;

            if (total != 0)
                porcentaje = (Convert.ToDecimal(cantidad) * 100) / total;
            else
                porcentaje = 0;
            
            return String.Format("{0:#,#0.00}", porcentaje);
        }
        #endregion

        #region Descargar
        private DataSet CrearDataTableResumenObra()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            DataSet ds = new DataSet();

            dt.Columns.Add(new DataColumn("proyecto"));
            dt.Columns.Add(new DataColumn("disponible"));
            dt.Columns.Add(new DataColumn("disponiblePorcentaje"));
            dt.Columns.Add(new DataColumn("reservado"));
            dt.Columns.Add(new DataColumn("reservadoPorcentaje"));
            dt.Columns.Add(new DataColumn("vendidaSinBoleto"));
            dt.Columns.Add(new DataColumn("vendidaSinBoletoPorcentaje"));
            dt.Columns.Add(new DataColumn("vendida"));
            dt.Columns.Add(new DataColumn("vendidaPorcentaje"));
            dt.Columns.Add(new DataColumn("total"));

            foreach (ListViewItem item in lvProyectos.Items)
            {
                Label lbProyecto = item.FindControl("lbProyecto") as Label;

                Label lbDisponible = item.FindControl("lbDisponible") as Label;
                Label lbPorcentajeDisponible = item.FindControl("lbPorcentajeDisponible") as Label;
                Label lbReservada = item.FindControl("lbReservada") as Label;
                Label lbPorcentajeReservada = item.FindControl("lbPorcentajeReservada") as Label;
                Label lbVendidaSinBoleto = item.FindControl("lbVendidaSinBoleto") as Label;
                Label lbPorcentajeVendidaSinBoleto = item.FindControl("lbPorcentajeVendidaSinBoleto") as Label;
                Label lbVendida = item.FindControl("lbVendida") as Label;
                Label lbPorcentajeVendida = item.FindControl("lbPorcentajeVendida") as Label;
                Label lbTotal = item.FindControl("lbTotal") as Label;

                dr = dt.NewRow();
                dr["proyecto"] = lbProyecto.Text;
                dr["disponible"] = lbDisponible.Text;
                dr["disponiblePorcentaje"] = "(" + String.Format("{0:#,#0.00}", lbPorcentajeDisponible.Text) + "%)";
                dr["reservado"] = String.Format("{0:#,#0.00}", lbReservada.Text);
                dr["reservadoPorcentaje"] = "(" + String.Format("{0:#,#0.00}", lbPorcentajeReservada.Text) + "%)";
                dr["vendidaSinBoleto"] = lbVendidaSinBoleto.Text;
                dr["vendidaSinBoletoPorcentaje"] = "(" + String.Format("{0:#,#0.00}", lbPorcentajeVendidaSinBoleto.Text) + "%)";
                dr["vendida"] = String.Format("{0:#,#0.00}", lbVendida.Text);
                dr["vendidaPorcentaje"] = "(" + String.Format("{0:#,#0.00}", lbPorcentajeVendida.Text) + "%)";
                dr["total"] = String.Format("{0:#,#0.00}", lbTotal.Text);
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "tResumenObra";

            Label lblTotalDisponible = (Label)lvProyectos.FindControl("lbTotalDisponible");
            Label lblTotalReservada = (Label)lvProyectos.FindControl("lbTotalReservada");
            Label lblTotalVendidaSinBoleto = (Label)lvProyectos.FindControl("lbTotalVendidaSinBoleto");
            Label lblTotalVendida = (Label)lvProyectos.FindControl("lbTotalVendida");
            Label lblTotal = (Label)lvProyectos.FindControl("lbTotalTotal");

            hfTotalDisponible.Value = String.Format("{0:#,#0.00}", lblTotalDisponible.Text);
            hfTotalReservada.Value = String.Format("{0:#,#0.00}", lblTotalReservada.Text);
            hfTotalVendidaSinBoleto.Value = String.Format("{0:#,#0.00}", lblTotalVendidaSinBoleto.Text);
            hfTotalVendida.Value = String.Format("{0:#,#0.00}", lblTotalVendida.Text);
            hfTotal.Value = String.Format("{0:#,#0.00}", lblTotal.Text);

            return ds;
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath;
            string filename = "Resumen de Obras.pdf";

            // Planilla
            DataSetUnidades ds = new DataSetUnidades();
            ds.Merge(CrearDataTableResumenObra(), false, System.Data.MissingSchemaAction.Ignore);
            CrystalReportSource.ReportDocument.SetDataSource(ds);

            CrystalReportSource.ReportDocument.SetParameterValue("totalDisponible", hfTotalDisponible.Value);
            CrystalReportSource.ReportDocument.SetParameterValue("totalReserva", hfTotalReservada.Value);
            CrystalReportSource.ReportDocument.SetParameterValue("totalVendidaSinBoleto", hfTotalVendidaSinBoleto.Value);
            CrystalReportSource.ReportDocument.SetParameterValue("totalVendida", hfTotalVendida.Value);
            CrystalReportSource.ReportDocument.SetParameterValue("total", hfTotal.Value);
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