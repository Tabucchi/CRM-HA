using DLL.Base_de_Datos;
using DLL.Negocio;
using log4net;
using Quartz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class JobSchedulerTotales : IJob
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public virtual void Execute(JobExecutionContext context)
    {
        try
        {
            cTotalesSaldos totales = new cTotalesSaldos();
            totales.Fecha = DateTime.Now;
            totales.TotalSaldoCuotasporObra = listSaldos();
            totales.TotalCuentaCorriente = totalCuentaCorriente();
            totales.TotalRecibos = totalRecibos();


        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("JobSchedulerCuotas - " + DateTime.Now + "- " + ex.Message + " - Execute");
            return;
        }
    }

    #region Total cuotas a cobrar por obra
    public decimal listSaldos()
    {
        List<cProyecto> list4 = cProyecto.GetProyectos();
        //List<cCuotasObra> saldos = new List<cCuotasObra>();
        List<cCuotasObra> saldosCtaCte = new List<cCuotasObra>();
        int cantColumnasMes = 5;
        decimal totalDeuda = 0;

        foreach (var item in list4)
        {
            //cCuotasObra saldo = new cCuotasObra();
            decimal _total = 0;

            DateTime date = GetFecha();
            DateTime dateDesde = Convert.ToDateTime(date.Year.ToString() + " - " + date.Month.ToString() + " - " + "1");
            DateTime dateHasta = Convert.ToDateTime(date.Year.ToString() + " - " + date.Month.ToString() + " - " + "29");

            //saldo.proyecto = item.Descripcion;
            //saldo.idProyecto = item.Id;

            #region 4 meses
            //Saldo 1
            decimal _saldo1Total = CalcularSaldo(item.Id, dateDesde.AddMonths(1), dateHasta.AddMonths(1));
            //saldo.saldo1 = String.Format("{0:#,#0.00}", _saldo1Total);

            //Saldo 2
            decimal _saldo2Total = CalcularSaldo(item.Id, dateDesde.AddMonths(2), dateHasta.AddMonths(2));
            //saldo.saldo2 = String.Format("{0:#,#0.00}", _saldo2Total);

            //Saldo 3
            decimal _saldo3Total = CalcularSaldo(item.Id, dateDesde.AddMonths(3), dateHasta.AddMonths(3));
            //saldo.saldo3 = String.Format("{0:#,#0.00}", _saldo3Total);

            //Saldo 4
            decimal _saldo4Total = CalcularSaldo(item.Id, dateDesde.AddMonths(4), dateHasta.AddMonths(4));
            //saldo.saldo4 = String.Format("{0:#,#0.00}", _saldo4Total);
            #endregion

            #region Meses restantes
            decimal _totalRestante = CalcularSaldoMesesRestantes(item.Id, date, cantColumnasMes);
            //saldo.mesesRestantes = String.Format("{0:#,#0.00}", _totalRestante);
            #endregion

            _total = _saldo1Total + _saldo2Total + _saldo3Total + _saldo4Total + _totalRestante;
            //saldo.total = String.Format("{0:#,#0.00}", _total);
            
            totalDeuda += _total;

            //saldos.Add(saldo);
        }

        return totalDeuda;
        //return saldos;
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

    public decimal CalcularSaldo(string _idProyecto, DateTime dateDesde, DateTime dateHasta)
    {
        decimal _saldoTotal = 0;
        DataTable dtSaldo1 = cCuota.GetCuotasObraByFecha(_idProyecto, dateDesde, dateHasta);

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

                        decimal porcentajeUnidad = Math.Round((valorApeso * 100) / valorBoletoApeso, 5);
                        decimal porcentajeCuota = Math.Round((porcentajeUnidad * cuota) / 100, 5);

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

                        decimal porcentajeUnidad = Math.Round((valorApeso * 100) / valorBoletoApeso, 5);
                        decimal porcentajeCuota = Math.Round((porcentajeUnidad * cuota) / 100, 5);

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
    #endregion
    
    public decimal totalCuentaCorriente()
    {
        List<cCuentaCorrienteUsuario> cuentas = cCuentaCorrienteUsuario.GetCuentaCorriente();
        decimal total = 0;

        foreach(cCuentaCorrienteUsuario cc in cuentas){
            total += Convert.ToDecimal(cc.GetTotal);
        }

        return total;
    }

    public decimal totalRecibos()
    {
        DateTime date = DateTime.Now;
        DateTime desde = new DateTime(date.Year, date.Month, 1);
        DateTime hasta = new DateTime(date.Month == 12 ? date.Year + 1 : date.Year, date.Month == 12 ? 1 : date.Month + 1, 1);
        decimal total = 0;

        List<cReciboCuota> recibos = cReciboCuota.GetRecibos(String.Format("{0:dd/MM/yyyy HH:mm:ss}", desde), String.Format("{0:dd/MM/yyyy HH:mm:ss}", hasta), "0", null, "1");

        foreach (cReciboCuota c in recibos)
        {
            total += Convert.ToDecimal(c.GetMonto);
        }

        return total;
    }
}
