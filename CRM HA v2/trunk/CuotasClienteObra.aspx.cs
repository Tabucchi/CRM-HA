using DLL;
using DLL.Negocio;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class CuotasClienteObra : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Vendedor)
                    Response.Redirect("Default.aspx");

                lbObra.Text = cProyecto.Load(Request["idProyecto"].ToString()).Descripcion;
                CargarListViesw();
                CalcularTotales();
            }
        }

        #region Carga ListView
        public List<cCuotasCliente> listSaldos()
        {
            try
            {
                string _idProyecto = Request["idProyecto"].ToString();
                ArrayList list4 = cCuota.GetEmpresasByProyecto(_idProyecto);
                List<cCuotasCliente> saldos = new List<cCuotasCliente>();
                int cantColumnasMes = 5;

                foreach (var item in list4)
                {
                    cCuotasCliente saldo = new cCuotasCliente();
                    decimal _total = 0;
                    decimal _saldoCtaCte = 0;
                    decimal _saldo1 = 0;
                    decimal _saldo2 = 0;
                    decimal _saldo3 = 0;
                    decimal _saldo4 = 0;
                    decimal _mesesRestantes = 0;

                    cEmpresa empresa = cEmpresa.Load(item.ToString());

                    saldo.idCliente = empresa.Id;
                    saldo.cliente = empresa.GetNombreCompleto;
                    saldo.proyecto = "";

                    DateTime _date = new DateTime();

                    cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
                    if (lastIndice.Fecha.Month == DateTime.Now.AddMonths(-1).Month)
                        _date = DateTime.Now.AddMonths(1);
                    else
                        _date = DateTime.Now;

                    string date = DateTime.Now.Year.ToString() + " - " + DateTime.Now.Month.ToString() + " - " + "1";

                    DateTime dateDesde = Convert.ToDateTime(_date.Year.ToString() + " - " + _date.Month.ToString() + " - " + "1");
                    DateTime dateHasta = Convert.ToDateTime(_date.Year.ToString() + " - " + _date.Month.ToString() + " - " + "29");

                    string ccu = cCuentaCorrienteUsuario.GetCuentaCorrienteByIdEmpresa(empresa.Id);
                    _saldoCtaCte = Convert.ToDecimal(cCuentaCorrienteUsuario.Load(ccu).GetSaldo) * -1;
                    saldo.saldoCtaCte = String.Format("{0:#,#0.00}", _saldoCtaCte);

                    #region 4 meses
                    //Saldo 1
                    DataTable dtSaldo1 = cCuota.GetCuotasMesMontoByEmpresaAndProyecto(empresa.Id, _idProyecto, dateDesde.AddMonths(1), dateHasta.AddMonths(1));
                    foreach (DataRow dr in dtSaldo1.Rows)
                    {
                        if (dr[1].ToString() == "0")
                            _saldo1 += Convert.ToDecimal(dr[0].ToString()) * cValorDolar.LoadActualValue();
                        else
                            _saldo1 += Convert.ToDecimal(dr[0].ToString());
                    }
                    saldo.saldo1 = String.Format("{0:#,#0.00}", _saldo1);

                    //Saldo 2
                    DataTable dtSaldo2 = cCuota.GetCuotasMesMontoByEmpresaAndProyecto(empresa.Id, _idProyecto, dateDesde.AddMonths(2), dateHasta.AddMonths(2));
                    foreach (DataRow dr in dtSaldo2.Rows)
                    {
                        if (dr[1].ToString() == "0")
                            _saldo2 += Convert.ToDecimal(dr[0].ToString()) * cValorDolar.LoadActualValue();
                        else
                            _saldo2 += Convert.ToDecimal(dr[0].ToString());
                    }
                    saldo.saldo2 = String.Format("{0:#,#0.00}", _saldo2);

                    //Saldo 3
                    DataTable dtSaldo3 = cCuota.GetCuotasMesMontoByEmpresaAndProyecto(empresa.Id, _idProyecto, dateDesde.AddMonths(3), dateHasta.AddMonths(3));
                    foreach (DataRow dr in dtSaldo3.Rows)
                    {
                        if (dr[1].ToString() == "0")
                            _saldo3 += Convert.ToDecimal(dr[0].ToString()) * cValorDolar.LoadActualValue();
                        else
                            _saldo3 += Convert.ToDecimal(dr[0].ToString());
                    }
                    saldo.saldo3 = String.Format("{0:#,#0.00}", _saldo3);

                    //Saldo 4
                    DataTable dtSaldo4 = cCuota.GetCuotasMesMontoByEmpresaAndProyecto(empresa.Id, _idProyecto, dateDesde.AddMonths(4), dateHasta.AddMonths(4));
                    foreach (DataRow dr in dtSaldo4.Rows)
                    {
                        if (dr[1].ToString() == "0")
                            _saldo4 += Convert.ToDecimal(dr[0].ToString()) * cValorDolar.LoadActualValue();
                        else
                            _saldo4 += Convert.ToDecimal(dr[0].ToString());
                    }
                    saldo.saldo4 = String.Format("{0:#,#0.00}", _saldo4);
                    #endregion

                    #region Meses restantes
                    DateTime hoy = Convert.ToDateTime(_date.Year + " -  " + _date.Month + " -  " + 1);
                    DateTime dateNextYearDesde = Convert.ToDateTime(hoy.AddMonths(cantColumnasMes));

                    DataTable dtMesesRestantes = cCuota.GetCuotasMesRestantesMontoByEmpresaAndProyecto(empresa.Id, _idProyecto, dateNextYearDesde);
                    foreach (DataRow dr in dtMesesRestantes.Rows)
                    {
                        if (dr[1].ToString() == "0")
                            _mesesRestantes += Convert.ToDecimal(dr[0].ToString()) * cValorDolar.LoadActualValue();
                        else
                            _mesesRestantes += Convert.ToDecimal(dr[0].ToString());
                    }
                    saldo.mesesRestantes = String.Format("{0:#,#0.00}", _mesesRestantes);
                    #endregion

                    _total = _saldo1 + _saldo2 + _saldo3 + _saldo4 + _mesesRestantes;
                    saldo.total = String.Format("{0:#,#0.00}", _total);

                    saldos.Add(saldo);
                }

                return saldos;
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("CuotasClienteObra - " + DateTime.Now + "- " + ex.Message + " - listSaldos" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
                return null;
            }
        }

        public void CargarListViesw()
        {
            lvSaldos.DataSource = listSaldos();
            lvSaldos.DataBind();
        }

        private void CalcularTotales()
        {
            try
            {
                decimal _totalMes1 = 0;
                decimal _totalMes2 = 0;
                decimal _totalMes3 = 0;
                decimal _totalMes4 = 0;
                decimal _totalMesesRestantes = 0;
                decimal _totalDeuda = 0;

                foreach (ListViewItem item in lvSaldos.Items)
                {
                    Label lbTotalMes1 = item.FindControl("lbMes1") as Label;
                    Label lbTotalMes2 = item.FindControl("lbMes2") as Label;
                    Label lbTotalMes3 = item.FindControl("lbMes3") as Label;
                    Label lbTotalMes4 = item.FindControl("lbMes4") as Label;
                    Label lbTotalMesesRestantes = item.FindControl("lbMesesRestantes") as Label;
                    Label lbTotalDeuda = item.FindControl("lbDeuda") as Label;

                    _totalMes1 += Convert.ToDecimal(lbTotalMes1.Text);
                    _totalMes2 += Convert.ToDecimal(lbTotalMes2.Text);
                    _totalMes3 += Convert.ToDecimal(lbTotalMes3.Text);
                    _totalMes4 += Convert.ToDecimal(lbTotalMes4.Text);
                    _totalMesesRestantes += Convert.ToDecimal(lbTotalMesesRestantes.Text);
                    _totalDeuda += Convert.ToDecimal(lbTotalDeuda.Text);
                }

                Label lblTotalMes1 = (Label)lvSaldos.FindControl("lbTotalMes1");
                Label lblTotalMes2 = (Label)lvSaldos.FindControl("lbTotalMes2");
                Label lblTotalMes3 = (Label)lvSaldos.FindControl("lbTotalMes3");
                Label lblTotalMes4 = (Label)lvSaldos.FindControl("lbTotalMes4");
                Label lblTotalMesesRestantes = (Label)lvSaldos.FindControl("lbTotalMesesRestantes");
                Label lblTotalDeuda = (Label)lvSaldos.FindControl("lbTotalDeuda");

                lblTotalMes1.Text = String.Format("{0:#,#0.00}", _totalMes1);
                lblTotalMes2.Text = String.Format("{0:#,#0.00}", _totalMes2);
                lblTotalMes3.Text = String.Format("{0:#,#0.00}", _totalMes3);
                lblTotalMes4.Text = String.Format("{0:#,#0.00}", _totalMes4);
                lblTotalMesesRestantes.Text = String.Format("{0:#,#0.00}", _totalMesesRestantes);
                lblTotalDeuda.Text = String.Format("{0:#,#0.00}", _totalDeuda);

                hfTotalMes1.Value = String.Format("{0:#,#0.00}", _totalMes1);
                hfTotalMes2.Value = String.Format("{0:#,#0.00}", _totalMes2);
                hfTotalMes3.Value = String.Format("{0:#,#0.00}", _totalMes3);
                hfTotalMes4.Value = String.Format("{0:#,#0.00}", _totalMes4);
                hfTotalMesesRestantes.Value = String.Format("{0:#,#0.00}", _totalMesesRestantes);
                hfTotalDeuda.Value = String.Format("{0:#,#0.00}", _totalDeuda);

            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("CuotasCliente - " + DateTime.Now + "- " + ex.Message + " - CalcularTotales" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region ListView
        protected void ListView11_LayoutCreated(object sender, EventArgs e)
        {
            DateTime date = new DateTime();

            cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
            if (lastIndice.Fecha.Month == DateTime.Now.AddMonths(-1).Month)
                date = DateTime.Now.AddMonths(1);
            else
                date = DateTime.Now;

            (lvSaldos.FindControl("lbMes1") as Label).Text = String.Format("{0:MMM-yy}", date.AddMonths(1));
            (lvSaldos.FindControl("lbMes2") as Label).Text = String.Format("{0:MMM-yy}", date.AddMonths(2));
            (lvSaldos.FindControl("lbMes3") as Label).Text = String.Format("{0:MMM-yy}", date.AddMonths(3));
            (lvSaldos.FindControl("lbMes4") as Label).Text = String.Format("{0:MMM-yy}", date.AddMonths(4));

            hfMes1.Value = String.Format("{0:MMM-yy}", date.AddMonths(1));
            hfMes2.Value = String.Format("{0:MMM-yy}", date.AddMonths(2));
            hfMes3.Value = String.Format("{0:MMM-yy}", date.AddMonths(3));
            hfMes4.Value = String.Format("{0:MMM-yy}", date.AddMonths(4));
        }
        #endregion

        #region Descargar
        private DataSet CrearDataTableCuotasCliente()
        {
            List<cCuotasCliente> saldos = listSaldos();

            DataTable dt = new DataTable();
            DataRow dr;
            DataSet ds = new DataSet();

            dt.Columns.Add(new DataColumn("cliente"));
            dt.Columns.Add(new DataColumn("obra"));
            dt.Columns.Add(new DataColumn("mes1"));
            dt.Columns.Add(new DataColumn("mes2"));
            dt.Columns.Add(new DataColumn("mes3"));
            dt.Columns.Add(new DataColumn("mes4"));
            dt.Columns.Add(new DataColumn("mesesRestantes"));
            dt.Columns.Add(new DataColumn("total"));

            foreach (cCuotasCliente p in saldos)
            {
                dr = dt.NewRow();
                dr["cliente"] = p.cliente;
                dr["obra"] = p.proyecto;
                dr["mes1"] = p.saldo1;
                dr["mes2"] = p.saldo2;
                dr["mes3"] = p.saldo3;
                dr["mes4"] = p.saldo4;
                dr["mesesRestantes"] = p.mesesRestantes;
                dr["total"] = p.total;
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "tCuotasClientes";

            return ds;
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath;
            string filename = "Cuotas a cobrar por cliente de la obra " + cProyecto.Load(Request["idProyecto"].ToString()).Descripcion + ".pdf";

            // Planilla
            DataSetUnidades ds = new DataSetUnidades();
            ds.Merge(CrearDataTableCuotasCliente(), false, System.Data.MissingSchemaAction.Ignore);
            CrystalReportSource.ReportDocument.SetDataSource(ds);

            CrystalReportSource.ReportDocument.SetParameterValue("titleMes1", hfMes1.Value.ToUpper());
            CrystalReportSource.ReportDocument.SetParameterValue("titleMes2", hfMes2.Value.ToUpper());
            CrystalReportSource.ReportDocument.SetParameterValue("titleMes3", hfMes3.Value.ToUpper());
            CrystalReportSource.ReportDocument.SetParameterValue("titleMes4", hfMes4.Value.ToUpper());

            CrystalReportSource.ReportDocument.SetParameterValue("totalMes1", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalMes1.Value)));
            CrystalReportSource.ReportDocument.SetParameterValue("totalMes2", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalMes2.Value)));
            CrystalReportSource.ReportDocument.SetParameterValue("totalMes3", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalMes3.Value)));
            CrystalReportSource.ReportDocument.SetParameterValue("totalMes4", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalMes4.Value)));
            CrystalReportSource.ReportDocument.SetParameterValue("totalMesesRestantes", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalMesesRestantes.Value)));
            CrystalReportSource.ReportDocument.SetParameterValue("totalDeuda", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalDeuda.Value)));

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