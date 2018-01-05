using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLL.Negocio;
using System.Web.Security;
using DLL;
using System.Data;
using System.Collections;
using System.IO;
using System.Threading;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                #region Combo
                cbProyectos.DataSource = cProyecto.GetDataTable();
                cbProyectos.DataValueField = "id";
                cbProyectos.DataTextField = "descripcion";
                cbProyectos.DataBind();
                ListItem io = new ListItem("Seleccione una obra...", "0");
                cbProyectos.Items.Insert(0, io);
                cbProyectos.SelectedIndex = 0;
                #endregion
                
                #region índice CAC
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Administración)
                {
                    //Aviso Índice CAC
                    if (string.IsNullOrEmpty(cIndiceCAC.GetLastIndiceMonth()))
                        pnlIndiceCAC.Visible = true;
                    else
                        pnlIndiceCAC.Visible = false;

                    //Aviso Índice UVA
                    if (Convert.ToDateTime(cAutorizacionUVA.GetAutorizacionByFecha().AddMonths(-1).ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                        pnlIndiceUVA.Visible = true;
                    else
                        pnlIndiceUVA.Visible = false;
                }
                #endregion

                #region Categoría
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Administración)
                {
                    pnlNuevoCliente.Visible = true;
                    pnlNuevaOV.Visible = true;
                    //pnlReservas.Visible = true;
                }
                else
                    Configuracion();


                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Gerencia)
                {
                    //pnlReservas.Visible = true;
                    pnlPendientes.Visible = true;
                    lbCantPrecios.Text = cActualizarPrecio.GetPrecios().Count.ToString();
                    lbCantOV.Text = cOperacionVenta.GetOV_AConfirmar().Count.ToString();
                }

                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Vendedor)
                    pnlDatos.Visible = false;

                #endregion

                lbDolar.Text = String.Format("{0:#,#0.00}", cValorDolar.LoadActualValue());

                decimal asdasd = cUVA.GetLastValorIndice();
                string sadasda = cUVA.GetLastValorIndice().ToString();
                string addwqwe = String.Format("{0:#,#0.00}", cUVA.GetLastValorIndice());


                lbUVA.Text = String.Format("{0:#,#0.00}", cUVA.GetLastValorIndice());
                lbCAC.Text = String.Format("{0:#,#0.00}", cIndiceCAC.GetLastValueIndice());



                #region Genera el archivo de cuotas por obra
                DateTime date = DateTime.Now;
                DateTime desde = new DateTime(date.Year, date.Month, date.Day);
                DateTime hasta = new DateTime(date.Year, date.Month, date.Day + 1);

                List<cArchivoCuotasObra> archivo = cArchivoCuotasObra.Search(String.Format("{0:dd/MM/yyyy}", desde), String.Format("{0:dd/MM/yyyy}", hasta));
                if (archivo.Count == 0)
                {
                    btnDescargar_Click();
                }
                #endregion
            }
            catch
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    Response.Redirect("Login.aspx", false);
            }
        }
    }

    public void Configuracion()
    {
        divAgenda.Attributes["style"] = "float:left; width:100px; padding: 20px 0 0 116px !important;";
        divCC.Attributes["style"] = "float:right; width:150px; padding: 20px 88px 0 25px; !important;";

        divOV.Attributes["style"] = "float:left; width:100px; padding: 20px 0 0 116px  !important;";
        divHistorial.Attributes["style"] = "float:right; width:150px; padding: 20px 88px 0 25px;";
    }

    protected void btnProyectos_Click(object sender, EventArgs e)
    {
        Response.Redirect("Unidad.aspx?idProyecto=" + cbProyectos.SelectedValue, false);
    }

    #region Archivo cuotas por obra
    public void btnDescargar_Click()
    {
        btnDescargar_Click(null, null);
    }

    public decimal CalcularSaldo(string _idProyecto, DateTime dateDesde, DateTime dateHasta)
    {
        decimal _saldoTotal = 0;
        DataTable dtSaldo1 = cCuota.GetCuotasObraByFecha(_idProyecto, dateDesde, dateHasta);

        if (dtSaldo1 != null)
        {
            foreach (DataRow dr in dtSaldo1.Rows)
            {
                //Si tiene más de una obra diferente
                ArrayList cantProyectos = cUnidad.GetCantProyectosByOV(dr[5].ToString());

                if (cantProyectos.Count > 1)
                {
                    List<cUnidad> unidades = cUnidad.GetUnidadByOV(dr[5].ToString());

                    if (unidades.Count > 1)
                    {
                        cOperacionVenta op = cOperacionVenta.Load(dr[5].ToString());

                        DataTable dt = new DataTable();
                        DataRow dr1;
                        DataSet ds = new DataSet();
                        decimal valorApeso = 0;
                        decimal valorBoletoApeso = 0;
                        decimal cuota = 0;

                        dt.Columns.Add(new DataColumn("idUnidad"));
                        dt.Columns.Add(new DataColumn("PorcentajeUnidad"));
                        dt.Columns.Add(new DataColumn("PorcentajeMonto"));
                        dt.Columns.Add(new DataColumn("idProyecto"));

                        foreach (cUnidad u in unidades)
                        {
                            dr1 = dt.NewRow();

                            cEmpresaUnidad eu = cEmpresaUnidad.GetUnidad(u.CodigoUF, u.IdProyecto);

                            #region Pesificar
                            if (op.GetMoneda == tipoMoneda.Dolar.ToString())
                            {
                                //Pesificar precio acordado de la unidad
                                if (u.Moneda == Convert.ToString((Int16)tipoMoneda.Dolar))
                                    valorApeso = eu.PrecioAcordado * cValorDolar.LoadActualValue();
                                else
                                    valorApeso = eu.PrecioAcordado;
                            }
                            else
                            {
                                valorApeso = eu.PrecioAcordado;
                            }

                            //Pesificar precio acordado del boleto
                            if (op.MonedaAcordada == Convert.ToString((Int16)tipoMoneda.Dolar))
                                valorBoletoApeso = op.PrecioAcordado * cValorDolar.LoadActualValue();
                            else
                                valorBoletoApeso = op.PrecioAcordado;

                            //Pesificar precio de la cuota
                            if (dr[3].ToString() == "0")
                                cuota = Convert.ToDecimal(dr[2].ToString()) * cValorDolar.LoadActualValue();
                            else
                                cuota = Convert.ToDecimal(dr[2].ToString());
                            #endregion

                            decimal porcentajeUnidad = (valorApeso * 100) / valorBoletoApeso;
                            decimal porcentajeCuota = Math.Round((porcentajeUnidad * cuota) / 100, 2);

                            dr1["idUnidad"] = u.Id;
                            dr1["PorcentajeUnidad"] = porcentajeUnidad;
                            dr1["PorcentajeMonto"] = porcentajeCuota;
                            dr1["idProyecto"] = u.IdProyecto;
                            dt.Rows.Add(dr1);
                        }

                        foreach (DataRow row in dt.Rows)
                        {
                            if (row[3].ToString() == _idProyecto)
                            {
                                _saldoTotal += Convert.ToDecimal(row[2].ToString());
                            }
                        }
                    }
                }
                else
                {
                    if (dr[1].ToString() == _idProyecto)
                    {
                        if (dr[3].ToString() == "0")
                            _saldoTotal += Convert.ToDecimal(dr[2].ToString()) * cValorDolar.LoadActualValue();
                        else
                            _saldoTotal += Convert.ToDecimal(dr[2].ToString());
                    }
                }
            }
        }
        return _saldoTotal;
    }

    public decimal CalcularSaldoMesesRestantes(string _idProyecto, DateTime date, int _cantColumnasMes)
    {
        DateTime hoy = Convert.ToDateTime(date.Year + " -  " + date.Month + " -  " + 1);
        DateTime dateRestante = Convert.ToDateTime(hoy.AddMonths(_cantColumnasMes));

        decimal _totalRestante = 0;
        DataTable dtSaldoRestante = cCuota.GetCuotasObraByFechaRestante(_idProyecto, dateRestante);

        foreach (DataRow dr in dtSaldoRestante.Rows)
        {
            //Si tiene más de una obra diferente
            ArrayList cantProyectos = cUnidad.GetCantProyectosByOV(dr[5].ToString());

            if (cantProyectos.Count > 1)
            {
                List<cUnidad> unidades = cUnidad.GetUnidadByOV(dr[5].ToString());

                if (unidades.Count > 1)
                {
                    cOperacionVenta op = cOperacionVenta.Load(dr[5].ToString());

                    DataTable dt = new DataTable();
                    DataRow dr1;
                    DataSet ds = new DataSet();
                    decimal valorApeso = 0;
                    decimal valorBoletoApeso = 0;
                    decimal cuota = 0;

                    dt.Columns.Add(new DataColumn("idUnidad"));
                    dt.Columns.Add(new DataColumn("PorcentajeUnidad"));
                    dt.Columns.Add(new DataColumn("PorcentajeMonto"));
                    dt.Columns.Add(new DataColumn("idProyecto"));

                    foreach (cUnidad u in unidades)
                    {
                        dr1 = dt.NewRow();

                        cEmpresaUnidad eu = cEmpresaUnidad.GetUnidad(u.CodigoUF, u.IdProyecto);

                        #region Pesificar
                        if (op.GetMoneda == tipoMoneda.Dolar.ToString())
                        {
                            //Pesificar precio acordado de la unidad
                            if (u.Moneda == Convert.ToString((Int16)tipoMoneda.Dolar))
                                valorApeso = eu.PrecioAcordado * cValorDolar.LoadActualValue();
                            else
                                valorApeso = eu.PrecioAcordado;
                        }
                        else
                        {
                            valorApeso = eu.PrecioAcordado;
                        }

                        //Pesificar precio acordado del boleto
                        if (op.MonedaAcordada == Convert.ToString((Int16)tipoMoneda.Dolar))
                            valorBoletoApeso = op.PrecioAcordado * cValorDolar.LoadActualValue();
                        else
                            valorBoletoApeso = op.PrecioAcordado;

                        //Pesificar precio de la cuota
                        if (dr[3].ToString() == "0")
                            cuota = Convert.ToDecimal(dr[2].ToString()) * cValorDolar.LoadActualValue();
                        else
                            cuota = Convert.ToDecimal(dr[2].ToString());
                        #endregion

                        decimal porcentajeUnidad = (valorApeso * 100) / valorBoletoApeso;
                        decimal porcentajeCuota = Math.Round((porcentajeUnidad * cuota) / 100, 2);

                        dr1["idUnidad"] = u.Id;
                        dr1["PorcentajeUnidad"] = porcentajeUnidad;
                        dr1["PorcentajeMonto"] = porcentajeCuota;
                        dr1["idProyecto"] = u.IdProyecto;
                        dt.Rows.Add(dr1);
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[3].ToString() == _idProyecto)
                        {
                            _totalRestante += Convert.ToDecimal(row[2].ToString());
                        }
                    }
                }
            }
            else
            {
                if (dr[1].ToString() == _idProyecto)
                {
                    if (dr[3].ToString() == "0")
                        _totalRestante += Convert.ToDecimal(dr[2].ToString()) * cValorDolar.LoadActualValue();
                    else
                        _totalRestante += Convert.ToDecimal(dr[2].ToString());
                }
            }
        }

        return _totalRestante;
    }

    public DateTime GetFecha()
    {
        DateTime date = new DateTime();

        cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
        if (lastIndice.Fecha.Month == DateTime.Now.AddMonths(-1).Month)
            date = DateTime.Now.AddMonths(1);
        else
            date = DateTime.Now;

        return date;
    }

    #region Variables
    decimal totalCtaCte = 0;
    decimal totalMes1 = 0;
    decimal totalMes2 = 0;
    decimal totalMes3 = 0;
    decimal totalMes4 = 0;
    decimal totalMesesRestantes = 0;
    decimal totalDeuda = 0;
    #endregion

    public List<cCuotasObra> listSaldos()
    {
        List<cProyecto> list4 = cProyecto.GetProyectos();
        List<cCuotasObra> saldos = new List<cCuotasObra>();
        List<cCuotasObra> saldosCtaCte = new List<cCuotasObra>();
        int cantColumnasMes = 5;

        foreach (var item in list4)
        {
            cCuotasObra saldo = new cCuotasObra();
            decimal _total = 0;

            DateTime date = GetFecha();
            DateTime dateDesde = Convert.ToDateTime(date.Year.ToString() + " - " + date.Month.ToString() + " - " + "1");
            DateTime dateHasta = Convert.ToDateTime(date.Year.ToString() + " - " + date.Month.ToString() + " - " + "29");

            saldo.proyecto = item.Descripcion;
            saldo.idProyecto = item.Id;

            #region 4 meses
            //Saldo 1
            decimal _saldo1Total = CalcularSaldo(item.Id, dateDesde.AddMonths(1), dateHasta.AddMonths(1));
            saldo.saldo1 = String.Format("{0:#,#0.00}", _saldo1Total);
            totalMes1 += _saldo1Total;

            //Saldo 2
            decimal _saldo2Total = CalcularSaldo(item.Id, dateDesde.AddMonths(2), dateHasta.AddMonths(2));
            saldo.saldo2 = String.Format("{0:#,#0.00}", _saldo2Total);
            totalMes2 += _saldo2Total;

            //Saldo 3
            decimal _saldo3Total = CalcularSaldo(item.Id, dateDesde.AddMonths(3), dateHasta.AddMonths(3));
            saldo.saldo3 = String.Format("{0:#,#0.00}", _saldo3Total);
            totalMes3 += _saldo3Total;

            //Saldo 4
            decimal _saldo4Total = CalcularSaldo(item.Id, dateDesde.AddMonths(4), dateHasta.AddMonths(4));
            saldo.saldo4 = String.Format("{0:#,#0.00}", _saldo4Total);
            totalMes4 += _saldo4Total;
            #endregion

            #region Meses restantes
            decimal _totalRestante = CalcularSaldoMesesRestantes(item.Id, date, cantColumnasMes);
            saldo.mesesRestantes = String.Format("{0:#,#0.00}", _totalRestante);
            totalMesesRestantes += _totalRestante;
            #endregion

            _total = _saldo1Total + _saldo2Total + _saldo3Total + _saldo4Total + _totalRestante;
            saldo.total = String.Format("{0:#,#0.00}", _total);
            totalDeuda += _total;

            saldos.Add(saldo);
        }

        return saldos;
    }

    private DataSet CrearDataSet()
    {
        List<cCuotasObra> saldos = listSaldos();

        DataTable dt = new DataTable();
        DataRow dr;
        DataSet ds = new DataSet();

        dt.Columns.Add(new DataColumn("obra"));
        dt.Columns.Add(new DataColumn("mes1"));
        dt.Columns.Add(new DataColumn("mes2"));
        dt.Columns.Add(new DataColumn("mes3"));
        dt.Columns.Add(new DataColumn("mes4"));
        dt.Columns.Add(new DataColumn("mesesRestantes"));
        dt.Columns.Add(new DataColumn("total"));

        foreach (cCuotasObra p in saldos)
        {
            dr = dt.NewRow();
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
        ds.Tables[0].TableName = "tCuotasObra";

        ds.Tables[0].DefaultView.Sort = "obra";

        return ds;
    }

    protected void btnDescargar_Click(object sender, EventArgs e)
    {
        try
        {
            string rutaURL = "C:\\PUBLICACIONES\\HA CRM\\Archivos\\Cuotas.pdf";
            //string rutaURL = "C:\\Users\\ntabucchi\\Documents\\GitHub\\CRM-HA\\CRM HA v2\\trunk\\Archivos\\Cuotas.pdf";
            //string filename = "Cuotas a cobrar por obra.pdf";

            CrystalDecisions.Web.CrystalReportSource s = new CrystalDecisions.Web.CrystalReportSource();
            s.Report.FileName = "C:\\PUBLICACIONES\\HA CRM\\Reportes";
            //s.Report.FileName = "C:\\Users\\ntabucchi\\Documents\\GitHub\\CRM-HA\\CRM HA v2\\trunk\\Reportes\\CuotasObra.rpt";

            // Planilla
            DataSetUnidades ds = new DataSetUnidades();
            ds.Merge(CrearDataSet(), false, System.Data.MissingSchemaAction.Ignore);
            s.ReportDocument.SetDataSource(ds);

            #region Encabezado

            DateTime date = GetFecha();

            s.ReportDocument.SetParameterValue("titleMes1", String.Format("{0:MMM-yy}", date.AddMonths(1)));
            s.ReportDocument.SetParameterValue("titleMes2", String.Format("{0:MMM-yy}", date.AddMonths(2)));
            s.ReportDocument.SetParameterValue("titleMes3", String.Format("{0:MMM-yy}", date.AddMonths(3)));
            s.ReportDocument.SetParameterValue("titleMes4", String.Format("{0:MMM-yy}", date.AddMonths(4)));

            #endregion

            s.ReportDocument.SetParameterValue("ctaCte", String.Format("{0:#,#0.00}", (cCuentaCorrienteUsuario.GetTotalCtaCte() * -1)));
            s.ReportDocument.SetParameterValue("totalMes1", String.Format("{0:#,#0.00}", totalMes1));
            s.ReportDocument.SetParameterValue("totalMes2", String.Format("{0:#,#0.00}", totalMes2));
            s.ReportDocument.SetParameterValue("totalMes3", String.Format("{0:#,#0.00}", totalMes3));
            s.ReportDocument.SetParameterValue("totalMes4", String.Format("{0:#,#0.00}", totalMes4));
            s.ReportDocument.SetParameterValue("totalMesesRestantes", String.Format("{0:#,#0.00}", totalMesesRestantes));
            s.ReportDocument.SetParameterValue("totalDeuda", String.Format("{0:#,#0.00}", totalDeuda));
            s.ReportDocument.SetParameterValue("total", String.Format("{0:#,#0.00}", totalDeuda + (cCuentaCorrienteUsuario.GetTotalCtaCte() * -1)));

            s.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL);

            FileStream stream = new FileStream(rutaURL, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            cArchivoCuotasObra arch = new cArchivoCuotasObra(reader.ReadBytes((int)stream.Length));

            stream.Close();
        }
        catch (Exception ex)
        {
        }
    }
    
    #endregion
}
