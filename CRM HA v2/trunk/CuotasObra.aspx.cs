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
    public partial class CuotasObra : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Vendedor)
                    Response.Redirect("Default.aspx");

                CargarListViesw();
                CalcularTotales();
            }
        }

        #region Auxiliares
        private decimal GetCuotasMesPesos(string _idProyecto, DateTime _dateDesde, DateTime _dateHasta)
        {
            decimal _total = 0;
            List<cCuota> _cuotas = cCuota.GetCuotasMesProyecto(_idProyecto, _dateDesde, _dateHasta, (Int16)tipoMoneda.Pesos);

            foreach (cCuota c in _cuotas)
            {
                _total += c.Monto;
            }

            return _total;
        }
        private decimal GetCuotasMesDolar(string _idProyecto, DateTime _dateDesde, DateTime _dateHasta)
        {
            decimal _total = 0;
            decimal _valorDolar = 0;
            int i = 0;
            List<cCuota> _cuotas = cCuota.GetCuotasMesProyecto(_idProyecto, _dateDesde, _dateHasta, (Int16)tipoMoneda.Dolar);

            foreach (cCuota c in _cuotas)
            {
                cFormaPagoOV fp = cFormaPagoOV.Load(c.IdFormaPagoOV);
                cOperacionVenta ov = cOperacionVenta.Load(fp.IdOperacionVenta);
                _valorDolar = ov.ValorDolar;

                _total += c.Monto * _valorDolar;
                i++;
            }

            return _total;
        }

        private decimal GetCuotasRepetidasPesos(DateTime _dateDesde, DateTime _dateHasta)
        {
            decimal _total = 0;
            List<cCuota> _cuotas = cCuota.GetCuotasRepetidas(_dateDesde, _dateHasta, (Int16)tipoMoneda.Pesos);

            foreach (cCuota c in _cuotas)
            {
                _total += c.Monto;
            }

            return _total;
        }

        private decimal GetCuotasRepetidasDolar(DateTime _dateDesde, DateTime _dateHasta)
        {
            decimal _total = 0;
            decimal _valorDolar = 0;
            int i = 0;
            List<cCuota> _cuotas = cCuota.GetCuotasRepetidas(_dateDesde, _dateHasta, (Int16)tipoMoneda.Dolar);

            foreach (cCuota c in _cuotas)
            {
                cFormaPagoOV fp = cFormaPagoOV.Load(c.IdFormaPagoOV);
                cOperacionVenta ov = cOperacionVenta.Load(fp.IdOperacionVenta);
                _valorDolar = ov.ValorDolar;

                _total += c.Monto * _valorDolar;
                i++;
            }

            return _total;
        }

        private decimal GetCuotasPendientesPesos(string _idProyecto)
        {
            decimal _total = 0;
            List<cCuentaCorriente> _cc = cCuentaCorriente.GetCuotasMesProyectoPendientes(_idProyecto, (Int16)tipoMoneda.Pesos);

            if (_cc.Count > 0)
            {
                foreach (cCuentaCorriente c in _cc)
                {
                    _total += Convert.ToDecimal(c.GetDeudaPesos);
                }
            }
            else
                _total = 0;

            return _total;
        }

        private decimal GetCuotasPendientesDolar(string _idProyecto)
        {
            decimal _total = 0;
            decimal _valorDolar = 0;
            int i = 0;
            List<cCuota> _cuotas = cCuota.GetCuotasMesProyectoPendientes(_idProyecto, (Int16)tipoMoneda.Dolar);

            foreach (cCuota c in _cuotas)
            {
                _total += c.Monto * _valorDolar;
            }

            return _total;
        }

        private decimal GetCuotasRestantesPesos(string _idProyecto, DateTime _date)
        {
            decimal _total = 0;
            List<cCuota> _cuotas = cCuota.GetCuotasProyectoRestantes(_idProyecto, _date, (Int16)tipoMoneda.Pesos);

            foreach (cCuota c in _cuotas)
                _total += c.Monto;

            return _total;
        }
        private decimal GetCuotasRestantesDolar(string _idProyecto, DateTime _date)
        {
            decimal _total = 0;
            decimal _valorDolar = 0;
            int i = 0;
            List<cCuota> _cuotas = cCuota.GetCuotasProyectoRestantes(_idProyecto, _date, (Int16)tipoMoneda.Dolar);

            foreach (cCuota c in _cuotas)
            {
                cFormaPagoOV fp = cFormaPagoOV.Load(c.IdFormaPagoOV);
                cOperacionVenta ov = cOperacionVenta.Load(fp.IdOperacionVenta);
                _valorDolar = ov.ValorDolar;

                _total += c.Monto * _valorDolar;
                i++;
            }

            return _total;
        }

        private decimal GetCuotasRestantesPesosRepetidos(DateTime _date)
        {
            decimal _total = 0;
            List<cCuota> _cuotas = cCuota.GetCuotasProyectoRestantesRepetidos(_date, (Int16)tipoMoneda.Pesos);

            foreach (cCuota c in _cuotas)
                _total += c.Monto;

            return _total;
        }

        private decimal GetCuotasRestantesDolarRepetidos(DateTime _date)
        {
            decimal _total = 0;
            decimal _valorDolar = 0;
            int i = 0;
            List<cCuota> _cuotas = cCuota.GetCuotasProyectoRestantesRepetidos(_date, (Int16)tipoMoneda.Dolar);

            foreach (cCuota c in _cuotas)
            {
                cFormaPagoOV fp = cFormaPagoOV.Load(c.IdFormaPagoOV);
                cOperacionVenta ov = cOperacionVenta.Load(fp.IdOperacionVenta);
                _valorDolar = ov.ValorDolar;

                _total += c.Monto * _valorDolar;
                i++;
            }

            return _total;
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
        #endregion

        #region Carga ListView
        public void CargarListViesw()
        {
            List<cCuotasObra> saldosCtaCte = new List<cCuotasObra>();

            #region Cuenta Corriente
            cCuotasObra saldoCtaCte = new cCuotasObra();

            decimal _totalCtaCte = cCuentaCorrienteUsuario.GetTotalCtaCte();            

            hfTotalCtaCte.Value = Convert.ToString(_totalCtaCte * -1);

            saldoCtaCte.proyecto = "Cuentas Corrientes";
            saldoCtaCte.saldo1 = String.Format("{0:#,#0.00}", 0);
            saldoCtaCte.saldo2 = String.Format("{0:#,#0.00}", 0);
            saldoCtaCte.saldo3 = String.Format("{0:#,#0.00}", 0);
            saldoCtaCte.saldo4 = String.Format("{0:#,#0.00}", 0);
            saldoCtaCte.mesesRestantes = String.Format("{0:#,#0.00}", 0);
            saldoCtaCte.total = String.Format("{0:#,#0.00}", _totalCtaCte * -1);
            saldosCtaCte.Add(saldoCtaCte);
            #endregion

            lvSaldos.DataSource = listSaldos();
            lvSaldos.DataBind();

            lvSaldosCtaCte.DataSource = saldosCtaCte;
            lvSaldosCtaCte.DataBind();
        }
                
        public decimal CalcularSaldo(string _idProyecto, DateTime dateDesde, DateTime dateHasta)
        {
            decimal _saldoTotal = 0;
            DataTable dtSaldo1 = cCuota.GetCuotasObraByFecha(_idProyecto, dateDesde, dateHasta);

            int asd = 0;
            if (_idProyecto == "42")
                asd++;

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
            return _saldoTotal;
        }

        public decimal CalcularSaldoMesesRestantes(string _idProyecto, DateTime date, int _cantColumnasMes)
        {
            string auxFormaPago = null;
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
                        if (dr[9].ToString() == auxFormaPago)//nuevo
                        {
                            if (dr[3].ToString() == "0")
                                _totalRestante += Convert.ToDecimal(dr[2].ToString()) * cValorDolar.LoadActualValue();
                            else
                                _totalRestante += Convert.ToDecimal(dr[2].ToString());

                            auxFormaPago = dr[9].ToString();//nuevo
                        }
                    }
                }
            }

            return _totalRestante;
        }

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
                DateTime dateHasta = Convert.ToDateTime(date.Year.ToString() + " - " + date.Month.ToString() + " - " + "28");

                saldo.proyecto = item.Descripcion;
                saldo.idProyecto = item.Id;

                #region 4 meses
                //Saldo 1
                decimal _saldo1Total = CalcularSaldo(item.Id, dateDesde.AddMonths(1), dateHasta.AddMonths(1));
                saldo.saldo1 = String.Format("{0:#,#0.00}", _saldo1Total);

                //Saldo 2
                decimal _saldo2Total = CalcularSaldo(item.Id, dateDesde.AddMonths(2), dateHasta.AddMonths(2));
                saldo.saldo2 = String.Format("{0:#,#0.00}", _saldo2Total);

                //Saldo 3
                decimal _saldo3Total = CalcularSaldo(item.Id, dateDesde.AddMonths(3), dateHasta.AddMonths(3));
                saldo.saldo3 = String.Format("{0:#,#0.00}", _saldo3Total);

                //Saldo 4
                decimal _saldo4Total = CalcularSaldo(item.Id, dateDesde.AddMonths(4), dateHasta.AddMonths(4));
                saldo.saldo4 = String.Format("{0:#,#0.00}", _saldo4Total);
                #endregion

                #region Meses restantes
                decimal _totalRestante = CalcularSaldoMesesRestantes(item.Id, date, cantColumnasMes);
                saldo.mesesRestantes = String.Format("{0:#,#0.00}", _totalRestante);
                #endregion

                _total = _saldo1Total + _saldo2Total + _saldo3Total + _saldo4Total + _totalRestante;
                saldo.total = String.Format("{0:#,#0.00}", _total);

                saldos.Add(saldo);
            }

            return saldos;
        }

        public decimal totalRepetidos(DateTime dateDesde, DateTime dateHasta)
        {
            decimal totalRepetidos = 0;
            string _idOpAux = null;

            DataTable dtSaldoRestante = cCuota.GetCuotasConMasProyectoByFecha(dateDesde, dateHasta);
            foreach (DataRow dr in dtSaldoRestante.Rows)
            {
                if (dr[0].ToString() == _idOpAux)
                {
                    if (dr[2].ToString() == "0")
                        totalRepetidos += Convert.ToDecimal(dr[1].ToString()) * Convert.ToDecimal(dr[3].ToString());
                    else
                        totalRepetidos += Convert.ToDecimal(dr[1].ToString());
                }
                _idOpAux = dr[0].ToString();
            }

            return totalRepetidos;
        }

        

        private void CalcularTotales()
        {
            try
            {
                #region ListView Saldos
                decimal _totalMes1 = 0;
                decimal _totalMes2 = 0;
                decimal _totalMes3 = 0;
                decimal _totalMes4 = 0;
                decimal _totalMesesRestantes = 0;
                decimal _totalDeuda = 0;

                DateTime date = GetFecha();
                DateTime dateDesde = Convert.ToDateTime(date.Year.ToString() + " - " + date.Month.ToString() + " - " + "1");
                DateTime dateHasta = Convert.ToDateTime(date.Year.ToString() + " - " + date.Month.ToString() + " - " + "28");

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
                #endregion

                #region ListView Cuenta Corriente
                decimal _totalDeudaCtaCte = 0;

                foreach (ListViewItem item in lvSaldosCtaCte.Items)
                {
                    Label lbTotalDeuda = item.FindControl("lbDeuda") as Label;
                    _totalDeudaCtaCte += Convert.ToDecimal(lbTotalDeuda.Text);
                }

                Label lblTotalDeudaCtaCte = (Label)lvSaldosCtaCte.FindControl("lbTotalDeuda");
                lblTotalDeudaCtaCte.Text = String.Format("{0:#,#0.00}", _totalDeudaCtaCte + _totalDeuda);

                hfTotal.Value = String.Format("{0:#,#0.00}", _totalDeudaCtaCte + _totalDeuda);
                #endregion
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
            DateTime date = GetFecha();

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
            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath;
            string filename = "Cuotas a cobrar por obra.pdf";

            // Planilla
            DataSetUnidades ds = new DataSetUnidades();
            ds.Merge(CrearDataSet(), false, System.Data.MissingSchemaAction.Ignore);
            CrystalReportSource.ReportDocument.SetDataSource(ds);

            CrystalReportSource.ReportDocument.SetParameterValue("titleMes1", hfMes1.Value.ToUpper());
            CrystalReportSource.ReportDocument.SetParameterValue("titleMes2", hfMes2.Value.ToUpper());
            CrystalReportSource.ReportDocument.SetParameterValue("titleMes3", hfMes3.Value.ToUpper());
            CrystalReportSource.ReportDocument.SetParameterValue("titleMes4", hfMes4.Value.ToUpper());

            CrystalReportSource.ReportDocument.SetParameterValue("ctaCte", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalCtaCte.Value)));
            CrystalReportSource.ReportDocument.SetParameterValue("totalMes1", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalMes1.Value)));
            CrystalReportSource.ReportDocument.SetParameterValue("totalMes2", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalMes2.Value)));
            CrystalReportSource.ReportDocument.SetParameterValue("totalMes3", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalMes3.Value)));
            CrystalReportSource.ReportDocument.SetParameterValue("totalMes4", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalMes4.Value)));
            CrystalReportSource.ReportDocument.SetParameterValue("totalMesesRestantes", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalMesesRestantes.Value)));
            CrystalReportSource.ReportDocument.SetParameterValue("totalDeuda", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalDeuda.Value)));
            CrystalReportSource.ReportDocument.SetParameterValue("total", String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotal.Value)));

            CrystalReportSource.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);

            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }
        #endregion
        
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarListViesw();
            CalcularTotales();
        }

        protected void btnNuevoOV_Click(object sender, EventArgs e)
        {
            Response.Redirect("ResumenCuotasObra.aspx");
        }

        public static string TotalDeuda()
        {
            List<cProyecto> list4 = cProyecto.GetProyectos();
            List<cCuotasObra> saldos = new List<cCuotasObra>();
            List<cCuotasObra> saldosCtaCte = new List<cCuotasObra>();
            int cantColumnasMes = 5;

            decimal total = 0;

            foreach (var item in list4)
            {
                cCuotasObra saldo = new cCuotasObra();
                decimal _total = 0;

                DateTime date = GetFecha1();
                DateTime dateDesde = Convert.ToDateTime(date.Year.ToString() + " - " + date.Month.ToString() + " - " + "1");
                DateTime dateHasta = Convert.ToDateTime(date.Year.ToString() + " - " + date.Month.ToString() + " - " + "28");

                saldo.proyecto = item.Descripcion;
                saldo.idProyecto = item.Id;

                #region 4 meses
                //Saldo 1
                decimal _saldo1Total = CalcularSaldo1(item.Id, dateDesde.AddMonths(1), dateHasta.AddMonths(1));
                //saldo.saldo1 = String.Format("{0:#,#0.00}", _saldo1Total);
                total += _saldo1Total;

                //Saldo 2
                decimal _saldo2Total = CalcularSaldo1(item.Id, dateDesde.AddMonths(2), dateHasta.AddMonths(2));
                //saldo.saldo2 = String.Format("{0:#,#0.00}", _saldo2Total);
                total += _saldo2Total;

                //Saldo 3
                decimal _saldo3Total = CalcularSaldo1(item.Id, dateDesde.AddMonths(3), dateHasta.AddMonths(3));
                //saldo.saldo3 = String.Format("{0:#,#0.00}", _saldo3Total);
                total += _saldo3Total;

                //Saldo 4
                decimal _saldo4Total = CalcularSaldo1(item.Id, dateDesde.AddMonths(4), dateHasta.AddMonths(4));
                //saldo.saldo4 = String.Format("{0:#,#0.00}", _saldo4Total);
                total += _saldo4Total;
                #endregion

                #region Meses restantes
                decimal _totalRestante = CalcularSaldoMesesRestantes1(item.Id, date, cantColumnasMes);
                //saldo.mesesRestantes = String.Format("{0:#,#0.00}", _totalRestante);
                total += _totalRestante;
                #endregion

                //_total = _saldo1Total + _saldo2Total + _saldo3Total + _saldo4Total + _totalRestante;
                //saldo.total = String.Format("{0:#,#0.00}", _total);

                //saldos.Add(saldo);
            }

            return String.Format("{0:#,#0.00}", total);
        }

        public static DateTime GetFecha1()
        {
            DateTime date = new DateTime();

            cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
            if (lastIndice.Fecha.Month == DateTime.Now.AddMonths(-1).Month)
                date = DateTime.Now.AddMonths(1);
            else
                date = DateTime.Now;

            return date;
        }

        public static decimal CalcularSaldo1(string _idProyecto, DateTime dateDesde, DateTime dateHasta)
        {
            decimal _saldoTotal = 0;
            DataTable dtSaldo1 = cCuota.GetCuotasObraByFecha(_idProyecto, dateDesde, dateHasta);

            int asd = 0;
            if (_idProyecto == "42")
                asd++;

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
            return _saldoTotal;
        }

        public static decimal CalcularSaldoMesesRestantes1(string _idProyecto, DateTime date, int _cantColumnasMes)
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

    }
}