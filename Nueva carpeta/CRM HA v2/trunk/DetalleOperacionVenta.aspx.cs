using DLL.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class DetalleOperacionVenta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cOperacionVenta ov = cOperacionVenta.Load(Request["idOV"].ToString());
                lbCliente.Text = ov.GetEmpresa;
                lbFecha.Text = ov.GetFecha;
                lbFechaEscritura.Text = ov.GetFechaEscritura;
                lbFechaPosesion.Text = ov.GetFechaPosesion;

                lbPrecioAcordado.Text = ov.GetPrecioAcordado;
                lbMonedaAcordada.Text = ov.GetMoneda;
                lbEstado.Text = ov.GetEstado;
                lbValorDolar.Text = ov.GetValorDolar;
                lbAnticipo.Text = ov.GetAnticipo;
                lbGastosAdtvo.Text = ov.GetTotalComision;
                lbIndiceCAC.Text = ov.GetIdIndiceCAC;
                lbIndiceUVA.Text = String.Format("{0:#,#0.00}", ov.ValorBaseUVA);

                lvUnidadesFuncionales.DataSource = cEmpresaUnidad.GetEmpresaUnidadOV(Request["idOV"].ToString());
                lvUnidadesFuncionales.DataBind();

                lvFormaPago.DataSource = cFormaPagoOV.GetFormaPagoOVByIdOV(Request["idOV"].ToString());
                lvFormaPago.DataBind();

                if (ov.IdEstado == (Int16)estadoOperacionVenta.Activo)
                    pnlCC.Visible = true;

                cCuentaCorriente cc = cCuentaCorriente.GetCuentaCorrienteByIdOv(Request["idOV"].ToString());
                if (cc != null)
                {
                    pnlCC.Visible = false; // Para crear la cuenta corriente.
                    pnlLinkCC.Visible = true; // Si ya existe la cuenta corriente, el botón redirige al Detalle de la CC.
                    hfCC.Value = cc.Id;
                }

                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria != (Int16)eCategoria.Administración)
                {
                    pnlCC.Visible = false;
                    pnlAnularOV.Visible = false;
                }

                PanelPrincipal(ov.IdEstado);
            }
        }

        #region Botón
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            if (Request.Params[1].ToString() == "true")
                Response.Redirect("PendientesOperacionesVenta.aspx");
            else
                Response.Redirect("ListaOperacionVenta.aspx");
        }

        protected void btnCrearCC_Click(object sender, EventArgs e)
        {
            CrearCuentaCorriente();
        }

        protected void btnLinkCC_Click(object sender, EventArgs e)
        {
            Response.Redirect("DetalleCuota2.aspx?idCC=" + hfCC.Value);
        }
        #endregion

        #region Auxiliares
        public void PanelPrincipal(Int16 _idEstado)
        {
            if (_idEstado == (Int16)estadoOperacionVenta.Anulado)
            {
                divEncabezado.Style["background-image"] = "url(../images/anulado.gif)";
                divEncabezado.Style["background-repeat"] = "no-repeat";
                divEncabezado.Style["background-position-x"] = "38%";
                divEncabezado.Style["background-position-y"] = "10%";
                pnlFinalizarOV.Visible = false;
            }
        }

        public void CrearCuentaCorriente()
        {
            if (lvFormaPago.Items.Count != 0)
            {
                #region Cuenta Corriente
                //Creo cuenta corriente                
                cOperacionVenta ov = cOperacionVenta.Load(Request["idOV"].ToString());
                List<cFormaPagoOV> fpov = cFormaPagoOV.GetFormaPagoOVByIdOV(ov.Id);
                string empresa = cOperacionVenta.GetEmpresaByIdOv(Request["idOV"].ToString());

                int index = 0;
                cCuentaCorriente cc = new cCuentaCorriente();
                cc.IdEmpresa = empresa;
                cc.IdEstado = Convert.ToInt16(estadoCuenta_Cuota.Activa);
                cc.CantCuotas = 0;
                cc.Total = Convert.ToDecimal(lbPrecioAcordado.Text);
                cc.IdEmpresaUnidad = ov.IdEmpresaUnidad;

                cc.IdUnidad = "0";
                cc.UnidadFuncional = "";
                cc.IdIndiceCAC = ov.IdIndiceCAC;
                cc.FormaPago = Convert.ToInt16(formaDePago.Cuotas).ToString();
                cc.Iva = ov.Iva;
                cc.IdOperacionVenta = ov.Id;
                cc.Anticipo = Convert.ToDecimal(lbAnticipo.Text);
                cc.TextoAnulado = "";

                if (lbMonedaAcordada.Text == tipoMoneda.Dolar.ToString())
                {
                    cc.Saldo = Convert.ToDecimal(lbPrecioAcordado.Text);
                    cc.SaldoPeso = cValorDolar.ConvertToPeso(Convert.ToDecimal(lbPrecioAcordado.Text), Convert.ToDecimal(ov.ValorDolar));
                }
                else
                {
                    cc.Saldo = cValorDolar.ConvertToDolar(Convert.ToDecimal(lbPrecioAcordado.Text), Convert.ToDecimal(ov.ValorDolar));
                    cc.SaldoPeso = Convert.ToDecimal(lbPrecioAcordado.Text);
                }

                cc.MonedaAcordada = ov.MonedaAcordada;

                int id = cc.Save();

                cUnidad unidad = cUnidad.LoadByIdEmpresaUnidad(ov.IdEmpresaUnidad);
                unidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Vendido);
                unidad.Save();
                #endregion

                foreach (var item in lvFormaPago.Items)
                {
                    Label _fechaVenc = item.FindControl("lbFechaVenc") as Label;
                    string gastosAdtvo = lbGastosAdtvo.Text;
                    Label _total = item.FindControl("lbTotal") as Label;
                    Label _cantCuotas = item.FindControl("lbCantCuotas") as Label;
                    Label _montoCuota = item.FindControl("lbMontoCuota") as Label;
                    Label _monedaCuota = item.FindControl("lbMoneda") as Label;

                    Label _rangoCuota = item.FindControl("lbRangoCuota") as Label;
                    string[] rangoCuota = _rangoCuota.Text.Split();

                    Label _gastoAdtvoCuota = item.FindControl("lbGastosAdtvo") as Label;
                    Label _interesAnual = item.FindControl("lbInteresAnual") as Label;

                    decimal _saldo = Convert.ToDecimal(_total.Text);

                    #region Variables
                    Int16 count = 1;

                    int cantCuotas;
                    decimal valorCuota = 0;
                    decimal _saldoAjustado = 0;
                    decimal _saldoAux = 0;
                    decimal _vencimiento1 = 0;
                    decimal interes = 0;

                    DateTime fechaVenc = Convert.ToDateTime(_fechaVenc.Text);
                    decimal _comision = 0;
                    if (_gastoAdtvoCuota.Text == Convert.ToString((Int16)eGastosAdtvo.No))
                        _comision = 0;
                    else
                        _comision = Convert.ToDecimal(gastosAdtvo);

                    decimal _totalComision = 0;
                    string _indiceBase = null; //Índice base del CAC
                    string _indiceMes = null;  //índice CAC del mes
                    string _indiceMesUVA = null;  //índice UVA del mes
                    #endregion

                    if (string.IsNullOrEmpty(_cantCuotas.Text))
                        cantCuotas = 1;
                    else
                        cantCuotas = Convert.ToInt16(_cantCuotas.Text);

                    #region Cuota
                    bool sameIndice = false;
                    /// <summary>
                    /// Creo las cuotas
                    /// </summary>
                    while (count <= cantCuotas)
                    {
                        cCuota cuota = new cCuota();
                        cuota.IdCuentaCorriente = id.ToString();
                        cuota.IdFormaPagoOV = fpov[index].Id;
                        cuota.Nro = count;

                        if (Convert.ToInt16(fpov[index].Moneda) == (Int16)tipoMoneda.Pesos)
                        {
                            #region Pesos
                            _indiceBase = ov.IdIndiceCAC;
                            _indiceMes = cIndiceCAC.GetIndiceByFecha(fechaVenc).ToString();
                            
                            _indiceMesUVA = cUVA.GetLastIdIndice().ToString();

                            /// <summary>
                            /// Calcula la variación del CAC. Si el CAC no esta actualizado se agrega 0.
                            /// La primer cuota se calcula con el índice seleccionado en el combo.
                            /// Para el resto de las cuotas se divide por el mes anterior.
                            /// </summary>
                            decimal variacionCAC = 0;
                            decimal variacionUVA = 0;

                            if (rangoCuota[0].ToString() != "" || rangoCuota[1].ToString() != "-" || rangoCuota[2].ToString() != "")
                            {
                                if (count >= Convert.ToDecimal(rangoCuota[0].ToString()))
                                {
                                    if (Convert.ToDecimal(rangoCuota[0].ToString()) != 0)
                                    {
                                        if (count.ToString() == rangoCuota[0].ToString())
                                        {
                                            variacionCAC = cCuota.CalcularVariacionMensualCAC(_indiceBase, _indiceMes, ov.Cac);
                                            variacionUVA = cCuota.CalcularVariacionMensualUVA(cUVA.GetIndiceByFecha(fechaVenc).ToString(), _indiceMesUVA, ov.ValorBaseUVA);
                                        }
                                        else
                                        {
                                            variacionCAC = cCuota.CalcularVariacionMensualCAC(cIndiceCAC.GetIndiceByFecha(fechaVenc.AddMonths(-1)).ToString(), _indiceMes, ov.Cac);
                                            variacionUVA = cCuota.CalcularVariacionMensualUVA(cUVA.GetIndiceByFecha(fechaVenc).ToString(), _indiceMesUVA, 0);
                                        }

                                        if (_monedaCuota.Text == tipoMoneda.Dolar.ToString())
                                        {
                                            cuota.VariacionCAC = 0;
                                            cuota.VariacionUVA = 0;
                                        }
                                        else
                                        {
                                            if (variacionCAC > 0)
                                                cuota.VariacionCAC = variacionCAC;
                                            else
                                                cuota.VariacionCAC = 0;

                                            cuota.VariacionUVA = variacionUVA;
                                        }
                                        cuota.AjusteCAC = true;
                                    }
                                    else
                                    {
                                        cuota.VariacionCAC = 0;
                                        cuota.VariacionUVA = 0;
                                        cuota.AjusteCAC = false;
                                    }
                                }
                                else
                                {
                                    cuota.VariacionCAC = 0;
                                    cuota.VariacionUVA = 0;
                                    cuota.AjusteCAC = false;
                                }
                            }
                            else
                            {
                                cuota.VariacionCAC = 0;
                                cuota.VariacionUVA = 0;
                                cuota.AjusteCAC = false;
                            }

                            /// <summary>
                            /// Verifica si los índeces CAC son iguales. Para el caso que tiene que calcular una cuota cuyo índice sea 0, con la CAC actualizado.
                            /// Para que no haya problemas con las cuotas que no tienen actualizado el índice y quedan en 0.
                            /// </summary>
                            if (_indiceBase == _indiceMes)
                                sameIndice = true;
                            else
                                sameIndice = false;

                            #region Se le suma un mes más, para calcular la variación del CAC.
                            int year = fechaVenc.Month == 12 ? fechaVenc.Year + 1 : fechaVenc.Year;
                            int month = fechaVenc.Month == 12 ? 1 : fechaVenc.Month + 1;
                            int day = fechaVenc.Day;

                            if (month == 2)
                                day = 28;
                            else
                            {
                                if (day == 31 || month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
                                    day = 30;
                                else
                                    day = fechaVenc.Day;
                            }
                            #endregion

                            fechaVenc = new DateTime(year, month, day);

                            //Ajusta el saldo con el índice CAC
                            if (_monedaCuota.Text == tipoMoneda.Dolar.ToString())
                                _saldoAjustado = _saldo;
                            else
                            {
                                if (Convert.ToInt16(rangoCuota[0].ToString()) == 1)
                                {
                                    if (_interesAnual.Text == "0" || _interesAnual.Text == "0,00")
                                    {
                                        if (ov.Cac == true)
                                            _saldoAjustado = cCuota.CalcularSaldoByIndice(_saldo, variacionCAC);

                                        if (ov.Uva == true)
                                            _saldoAjustado = cCuota.CalcularSaldoByIndice(_saldo, variacionUVA);

                                        if (ov.Cac == false && ov.Uva == false)
                                            _saldoAjustado = cCuota.CalcularSaldoByIndice(_saldo, 0);
                                    }
                                    else
                                    {
                                        interes = Convert.ToDecimal(_interesAnual.Text) / 12;
                                        interes = interes + 100;

                                        if (ov.Cac == true)
                                            _saldoAjustado = cCuota.CalcularSaldoByIndice(_saldo, variacionCAC);

                                        if (ov.Uva == true)
                                            _saldoAjustado = cCuota.CalcularSaldoByIndice(_saldo, variacionUVA);

                                        if (ov.Cac == false && ov.Uva == false)
                                            _saldoAjustado = cCuota.CalcularSaldoByIndice(_saldo, 0);
                                        
                                        _saldoAjustado = (_saldoAjustado * interes) / 100;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt16(rangoCuota[0].ToString()) == count)
                                    {
                                        cCuota _cuotaAux = cCuota.GetCuotaByNro(id.ToString(), count - 1, null);

                                        if (_interesAnual.Text == "0")
                                        {
                                            if (ov.Cac == true)
                                                _saldoAjustado = cCuota.CalcularSaldoByIndice(_cuotaAux.Saldo, variacionCAC);

                                            if (ov.Uva == true)
                                                _saldoAjustado = cCuota.CalcularSaldoByIndice(_cuotaAux.Saldo, variacionUVA);

                                            if (ov.Cac == false && ov.Uva == false)
                                                _saldoAjustado = cCuota.CalcularSaldoByIndice(_cuotaAux.Saldo, 0);
                                        }
                                        else
                                        {
                                            interes = Convert.ToDecimal(_interesAnual.Text) / 12;
                                            interes = interes + 100;

                                            if (ov.Cac == true)
                                                _saldoAjustado = cCuota.CalcularSaldoByIndice(_cuotaAux.Saldo, variacionCAC);

                                            if (ov.Uva == true)
                                                _saldoAjustado = cCuota.CalcularSaldoByIndice(_cuotaAux.Saldo, variacionUVA);

                                            if (ov.Cac == false && ov.Uva == false)
                                                _saldoAjustado = cCuota.CalcularSaldoByIndice(_cuotaAux.Saldo, 0);

                                            _saldoAjustado = (_saldoAjustado * interes) / 100;
                                        }
                                    }
                                }
                            }

                            cuota.Comision = _comision;

                            #region Si el CAC no se actualiza, el resto de las cuotas quedan con el mismo valor
                            if (ov.Cac == true)
                            {
                                if (variacionCAC != 0)
                                {
                                    if (cuota.Nro == 1)
                                        valorCuota = _saldo;
                                    else
                                        valorCuota = cCuota.CalcularCuota((cantCuotas - cuota.Nro) + 1, _saldoAjustado);

                                    _totalComision = cCuota.CalcularComisionIva(valorCuota, _comision, ov.Iva);

                                    _vencimiento1 = valorCuota + _totalComision + interes;
                                    _saldo = _saldoAjustado - valorCuota;

                                    cuota.MontoAjustado = _saldoAjustado;
                                    cuota.TotalComision = _totalComision;
                                    cuota.Vencimiento1 = _vencimiento1;
                                    cuota.Saldo = _saldoAjustado - valorCuota;
                                }
                                else
                                {
                                    valorCuota = cCuota.CalcularCuota(cantCuotas - (cuota.Nro - 1), _saldo);
                                    _saldoAux = _saldo;
                                    _totalComision = cCuota.CalcularComisionIva(valorCuota, _comision, ov.Iva);

                                    if (_interesAnual.Text != "0")
                                        interes = 100 * (Convert.ToDecimal(_interesAnual.Text) / 12);

                                    _vencimiento1 = valorCuota + _totalComision + interes;
                                    _saldo = _saldoAjustado - valorCuota;

                                    cuota.MontoAjustado = _saldoAux;
                                    cuota.TotalComision = _totalComision;
                                    cuota.Vencimiento1 = _vencimiento1;
                                    cuota.Saldo = _saldoAux - valorCuota;
                                }
                            }
                            #endregion

                            #region Si el UVA no se actualiza, el resto de las cuotas quedan con el mismo valor
                            if (ov.Uva == true)
                            {
                                if (variacionUVA != 0)
                                {
                                    if (cuota.Nro == 1)
                                        valorCuota = cCuota.CalcularCuota(cantCuotas, _saldoAjustado);
                                    else
                                        valorCuota = cCuota.CalcularCuota((cantCuotas - cuota.Nro) + 1, _saldoAjustado);

                                    _totalComision = cCuota.CalcularComisionIva(valorCuota, _comision, ov.Iva);

                                    _vencimiento1 = valorCuota + _totalComision + interes;
                                    _saldo = _saldoAjustado - valorCuota;

                                    cuota.MontoAjustado = _saldoAjustado;
                                    cuota.TotalComision = _totalComision;
                                    cuota.Vencimiento1 = _vencimiento1;
                                    cuota.Saldo = _saldoAjustado - valorCuota;
                                }
                                else
                                {
                                    valorCuota = cCuota.CalcularCuota(cantCuotas - (cuota.Nro - 1), _saldo);
                                    _saldoAux = _saldo;
                                    _totalComision = cCuota.CalcularComisionIva(valorCuota, _comision, ov.Iva);

                                    if (_interesAnual.Text != "0")
                                        interes = 100 * (Convert.ToDecimal(_interesAnual.Text) / 12);

                                    _vencimiento1 = valorCuota + _totalComision + interes;
                                    _saldo = _saldoAjustado - valorCuota;

                                    cuota.MontoAjustado = _saldoAux;
                                    cuota.TotalComision = _totalComision;
                                    cuota.Vencimiento1 = _vencimiento1;
                                    cuota.Saldo = _saldoAux - valorCuota;
                                }
                            }
                            #endregion

                            //Redondea la última cuota
                            cuota.Monto = valorCuota;
                            cuota.MontoPago = 0;

                            //Al 2° vencimiento se le suma un 2%
                            cuota.Vencimiento2 = cCuota.Calcular2Venc(_vencimiento1);

                            DateTime max;

                            if (_cantCuotas.Text == "1")
                                max = Convert.ToDateTime(_fechaVenc.Text);
                            else
                                max = Convert.ToDateTime(_fechaVenc.Text).AddMonths(count - 1);

                            cuota.Fecha = Convert.ToDateTime(max);

                            cuota.FechaVencimiento1 = new DateTime(max.Year, max.Month, 10);
                            cuota.FechaVencimiento2 = new DateTime(max.Year, max.Month, 20);

                            cuota.Estado = Convert.ToInt16(estadoCuenta_Cuota.Activa);
                            cuota.IdRegistroPago = "-1"; //Se guarda con menos -1, hasta que se asocie un pago
                            cuota.Save();
                            count++;
                            #endregion
                        }
                        else
                        {
                            #region Dolar
                            if (rangoCuota[0].ToString() != "" || rangoCuota[1].ToString() != "-" || rangoCuota[2].ToString() != "")
                            {
                                if (count >= Convert.ToDecimal(rangoCuota[0].ToString()))
                                {
                                    if (Convert.ToDecimal(rangoCuota[0].ToString()) != 0)
                                    {
                                        cuota.VariacionCAC = 0;
                                        cuota.VariacionUVA = 0;
                                        cuota.AjusteCAC = true;
                                    }
                                    else
                                    {
                                        cuota.VariacionCAC = 0;
                                        cuota.VariacionUVA = 0;
                                        cuota.AjusteCAC = false;
                                    }
                                }
                                else
                                {
                                    cuota.VariacionCAC = 0;
                                    cuota.VariacionUVA = 0;
                                    cuota.AjusteCAC = false;
                                }
                            }
                            else
                            {
                                cuota.VariacionCAC = 0;
                                cuota.VariacionUVA = 0;
                                cuota.AjusteCAC = false;
                            }

                            /// <summary>
                            /// Verifica si los índeces CAC son iguales. Para el caso que tiene que calcular una cuota cuyo índice sea 0, con la CAC actualizado.
                            /// Para que no haya problemas con las cuotas que no tienen actualizado el índice y quedan en 0.
                            /// </summary>

                            #region Se le suma un mes más, para calcular la variación del CAC.
                            int year = fechaVenc.Month == 12 ? fechaVenc.Year + 1 : fechaVenc.Year;
                            int month = fechaVenc.Month == 12 ? 1 : fechaVenc.Month + 1;
                            int day = fechaVenc.Day;

                            if (month == 2)
                                day = 28;
                            else
                            {
                                if (day == 31 || month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
                                    day = 30;
                                else
                                    day = fechaVenc.Day;
                            }
                            #endregion

                            fechaVenc = new DateTime(year, month, day);

                            _saldoAjustado = _saldo;
                            cuota.Comision = _comision;

                            valorCuota = Convert.ToDecimal(_montoCuota.Text);
                            _saldoAux = _saldo;
                            _totalComision = cCuota.CalcularComisionIva(valorCuota, _comision, ov.Iva);

                            if (_interesAnual.Text != "0")
                                interes = 100 * (Convert.ToDecimal(_interesAnual.Text) / 12);

                            _vencimiento1 = valorCuota + _totalComision + interes;
                            _saldo = _saldoAjustado - valorCuota;

                            cuota.MontoAjustado = _saldoAux;
                            cuota.TotalComision = _totalComision;
                            cuota.Vencimiento1 = _vencimiento1;
                            cuota.Saldo = _saldoAux - valorCuota;

                            //Redondea la última cuota
                            cuota.Monto = valorCuota;
                            cuota.MontoPago = 0;

                            //Al 2° vencimiento se le suma un 2%
                            cuota.Vencimiento2 = _vencimiento1;

                            DateTime max;

                            if (_cantCuotas.Text == "1")
                                max = Convert.ToDateTime(_fechaVenc.Text);
                            else
                                max = Convert.ToDateTime(_fechaVenc.Text).AddMonths(count - 1);

                            cuota.Fecha = Convert.ToDateTime(max);

                            cuota.FechaVencimiento1 = new DateTime(max.Year, max.Month, 10);
                            cuota.FechaVencimiento2 = new DateTime(max.Year, max.Month, 20);

                            cuota.Estado = Convert.ToInt16(estadoCuenta_Cuota.Activa);
                            cuota.IdRegistroPago = "-1"; //Se guarda con menos -1, hasta que se asocie un pago
                            cuota.Save();
                            count++;
                            #endregion
                        }
                    }
                    index++;
                    #endregion
                }

                //En caso que haya cuotas de meses anteriores al que se cargo la operación, se agrega el valor de la cuota en la cuenta corriente
                ActualizarCuentasCorrientes(empresa);
            }

            Response.Redirect("CC.aspx");
        }
        #endregion

        #region Cuenta Corriente
        public void ActualizarCuentasCorrientes(string _idEmpresa)
        {
            string _idCCU = cCuentaCorrienteUsuario.GetCuentaCorrienteByIdEmpresa(_idEmpresa);
            cCuentaCorrienteUsuario c = cCuentaCorrienteUsuario.Load(_idCCU);
            cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());

            CargarCuotas(DateTime.Now, c.Id, c.IdEmpresa);
            actualizarEstadoCuotas(c.Id);

            if (lastIndice.Fecha.Month == DateTime.Now.AddMonths(-1).Month)
                CargarCuotas(DateTime.Now.AddMonths(1), c.Id, c.IdEmpresa);
        }

        public void CargarCuotas(DateTime _fecha, string _idCCU, string _idEmpresa)
        {
            string _idCC = null;
            string _idFormaPagoOv = null;

            List<cCuota> cuotas2 = cCuota.GetCuotasPendientes(_idEmpresa, _fecha);
            decimal _saldo = 0;
            decimal valorCuotaVencimiento1 = 0;
            decimal valorCuotaVencimiento2 = 0;

            #region Carga de cuotas del mes correspondiente
            if (cuotas2 != null && cuotas2.Count != 0)
            {
                foreach (cCuota cuota in cuotas2)
                {
                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos) && cuota.VariacionCAC != 0)
                    {
                        if (cuota.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                        {
                            if (cItemCCU.GetCantCuotasById(cuota.Id) == 0)
                            {
                                decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(_idCCU));
                                cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente);
                                cFormaPagoOV fpov = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                _idCC = cc.Id;
                                _idFormaPagoOv = fpov.Id;

                                if (string.IsNullOrEmpty(cItemCCU.GetCCByIdCuota(cuota.Id)))
                                {
                                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Dolar))
                                    {
                                        valorCuotaVencimiento1 = cuota.Vencimiento1 * cValorDolar.LoadActualValue();
                                        valorCuotaVencimiento2 = cuota.Vencimiento2 * cValorDolar.LoadActualValue();
                                    }
                                    else
                                    {
                                        valorCuotaVencimiento1 = cuota.Vencimiento1;
                                        valorCuotaVencimiento2 = cuota.Vencimiento2;
                                    }

                                    cItemCCU ccuNew = new cItemCCU();
                                    ccuNew.IdCuentaCorrienteUsuario = _idCCU;
                                    ccuNew.Concepto = "Cuota nro " + cuota.Nro + " - Obra " + cc.GetProyecto + " - Cod. U.F.: " + cUnidad.LoadByIdEmpresaUnidad(cc.IdEmpresaUnidad).CodigoUF;
                                    ccuNew.Debito = valorCuotaVencimiento1 * -1;

                                    _saldo += lastSaldo;

                                    //ccuNew.Saldo = _saldo + (valorCuotaVencimiento1 * -1);
                                    ccuNew.Saldo = _saldo - valorCuotaVencimiento1;

                                    ccuNew.Credito = 0;
                                    ccuNew.Fecha = DateTime.Now;
                                    ccuNew.IdCuota = cuota.Id;
                                    ccuNew.IdEstado = (Int16)eEstadoItem.Cuota;
                                    ccuNew.Save();

                                    #region En caso que haya pagos a cuenta
                                    if (lastSaldo != 0)
                                    {
                                        cItemCCU item = cItemCCU.GetLastItemById(_idCCU);
                                        if (item != null && item.IdEstado == (Int16)eEstadoItem.Pagado)
                                        {
                                            if (lastSaldo > valorCuotaVencimiento1)
                                            {
                                                decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                                cItemCCU ccuPago = new cItemCCU();
                                                ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                                ccuPago.Fecha = DateTime.Now;
                                                ccuPago.Concepto = "Pago cuota " + cuota.Nro;
                                                ccuPago.Debito = 0;
                                                ccuPago.Credito = cuota.Vencimiento1;
                                                ccuPago.Saldo = diferencia;
                                                ccuPago.IdCuota = cuota.Id;
                                                ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                                int _idItemCCU = ccuPago.Save();

                                                //Genera el recibo del pago
                                                cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), cuota.Vencimiento1);

                                                cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                                cuota.Save();
                                            }

                                            if (lastSaldo < valorCuotaVencimiento1)
                                            {
                                                decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                                cItemCCU ccuPago = new cItemCCU();
                                                ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                                ccuPago.Fecha = DateTime.Now;
                                                ccuPago.Concepto = "Pago parcial cuota " + cuota.Nro;
                                                ccuPago.Debito = 0;
                                                ccuPago.Credito = cuota.Vencimiento1;
                                                ccuPago.Saldo = diferencia;
                                                ccuPago.IdCuota = cuota.Id;
                                                ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                                int _idItemCCU = ccuPago.Save();

                                                //Genera el recibo del pago
                                                cReciboCuota recibo = cReciboCuota.CrearRecibo("-1", _idItemCCU.ToString(), cuota.Vencimiento1);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cuota.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                        {
                            if (cItemCCU.GetCantCuotasById(cuota.Id) == 0)
                            {
                                decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(_idCCU));
                                cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente);
                                cFormaPagoOV fpov = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                _idCC = cc.Id;
                                _idFormaPagoOv = fpov.Id;

                                if (string.IsNullOrEmpty(cItemCCU.GetCCByIdCuota(cuota.Id)))
                                {
                                    if (fpov.Moneda == Convert.ToString((Int16)tipoMoneda.Dolar))
                                    {
                                        valorCuotaVencimiento1 = cuota.Vencimiento1 * cValorDolar.LoadActualValue();
                                        valorCuotaVencimiento2 = cuota.Vencimiento2 * cValorDolar.LoadActualValue();
                                    }
                                    else
                                    {
                                        valorCuotaVencimiento1 = cuota.Vencimiento1;
                                        valorCuotaVencimiento2 = cuota.Vencimiento2;
                                    }

                                    cItemCCU ccuNew = new cItemCCU();
                                    ccuNew.IdCuentaCorrienteUsuario = _idCCU;
                                    ccuNew.Concepto = "Cuota nro " + cuota.Nro + " - Obra " + cc.GetProyecto + " - Cod. U.F.: " + cUnidad.LoadByIdEmpresaUnidad(cc.IdEmpresaUnidad).CodigoUF;
                                    ccuNew.Debito = valorCuotaVencimiento1 * -1;

                                    _saldo += lastSaldo;
                                    if (_saldo < 0)
                                        ccuNew.Saldo = (valorCuotaVencimiento1 * -1) + (_saldo);
                                    else
                                        ccuNew.Saldo = _saldo - valorCuotaVencimiento1;

                                    if (_saldo == 0)
                                        ccuNew.Saldo = _saldo - valorCuotaVencimiento1;

                                    ccuNew.Credito = 0;
                                    ccuNew.Fecha = DateTime.Now;
                                    ccuNew.IdCuota = cuota.Id;
                                    ccuNew.IdEstado = (Int16)eEstadoItem.Cuota;
                                    ccuNew.Save();

                                    #region En caso que haya pagos a cuenta
                                    if (lastSaldo != 0)
                                    {
                                        cItemCCU item = cItemCCU.GetLastItemById(_idCCU);
                                        if (item != null && item.IdEstado == (Int16)eEstadoItem.Pagado)
                                        {
                                            if (lastSaldo > valorCuotaVencimiento1)
                                            {
                                                decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                                cItemCCU ccuPago = new cItemCCU();
                                                ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                                ccuPago.Fecha = DateTime.Now;
                                                ccuPago.Concepto = "Pago cuota " + cuota.Nro;
                                                ccuPago.Debito = 0;
                                                ccuPago.Credito = cuota.Vencimiento1;
                                                ccuPago.Saldo = diferencia;
                                                ccuPago.IdCuota = cuota.Id;
                                                ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                                int _idItemCCU = ccuPago.Save();

                                                //Genera el recibo del pago
                                                cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), cuota.Vencimiento1);

                                                cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                                cuota.Save();
                                            }

                                            if (lastSaldo < valorCuotaVencimiento1)
                                            {
                                                decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                                cItemCCU ccuPago = new cItemCCU();
                                                ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                                ccuPago.Fecha = DateTime.Now;
                                                ccuPago.Concepto = "Pago parcial cuota " + cuota.Nro;
                                                ccuPago.Debito = 0;
                                                ccuPago.Credito = cuota.Vencimiento1;
                                                ccuPago.Saldo = diferencia;
                                                ccuPago.IdCuota = cuota.Id;
                                                ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                                int _idItemCCU = ccuPago.Save();

                                                //Genera el recibo del pago
                                                cReciboCuota recibo = cReciboCuota.CrearRecibo("-1", _idItemCCU.ToString(), cuota.Vencimiento1);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }

                    _saldo = 0;
                }
            }
            #endregion
        }

        public void actualizarEstadoCuotas(string _idCCU)
        {
            decimal newSaldo = 0;
            int contCuotasDolar = 0;
            List<cItemCCU> cuotasPendientes = cItemCCU.GetItemByCuotasPendientes(_idCCU);
            foreach (cItemCCU item in cuotasPendientes)
            {
                if (String.Format("{0:dd/MM/yyyy}", item.Fecha) != String.Format("{0:dd/MM/yyyy}", DateTime.Now))
                {
                    cCuota cuota = cCuota.Load(item.IdCuota);
                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Dolar))
                    {
                        #region Actualizo el valor de la fila que contine la cuota pendiente
                        //Actualizo el saldo
                        //Saldo -: (-debito) || Saldo +: (-debito)
                        if (item.Saldo < 0)
                            item.Saldo = item.Saldo - item.Debito;
                        else
                            item.Saldo = item.Saldo + item.Debito;

                        if (contCuotasDolar == 0)
                            newSaldo += item.Saldo + (cuota.Vencimiento1 * cValorDolar.LoadActualValue() * -1);
                        else
                            newSaldo += item.Debito;

                        if (item.Debito != 0)
                            item.Debito = cuota.Vencimiento1 * cValorDolar.LoadActualValue() * -1;

                        contCuotasDolar++;

                        #endregion
                    }
                    else
                    {
                        DateTime now = Convert.ToDateTime(DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day);
                        DateTime fechaVenc = cuota.FechaVencimiento1.AddDays(2);
                        fechaVenc = Convert.ToDateTime(fechaVenc.Year + "/" + fechaVenc.Month + "/" + fechaVenc.Day);

                        if (fechaVenc < now)
                        {
                            int ads = cItemCCU.GetCantCuotasById(cuota.Id);
                            string lastSaldo = cItemCCU.GetLastSaldoByIdCCU(_idCCU);
                            if (cItemCCU.GetCantCuotasById(cuota.Id) < 2) //Para que solo una vez agregue la fila con el segundo vencimiento
                            {
                                cuota.Estado = (Int16)estadoCuenta_Cuota.Pendiente;
                                cuota.Save();

                                decimal diferencia = (cuota.Vencimiento2 - cuota.Vencimiento1) * -1;
                                decimal newSaldo1 = Convert.ToDecimal(lastSaldo) + diferencia;
                                cItemCCU newItem = new cItemCCU(item.IdCuentaCorrienteUsuario, DateTime.Now, "Recargo por segundo vencimiento de la cuota " + cuota.Nro, diferencia, 0, newSaldo1, cuota.Id);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Anular Operación de venta
        protected void btnAnularOV_Click(object sender, EventArgs e)
        {
            modalAnular.Show();
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            modalAnular.Hide();
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            #region Variables
            string _idEmpresa = null;
            decimal _cancelarSaldo = 0;
            decimal _nuevoSaldo = 0;
            #endregion

            cOperacionVenta op = cOperacionVenta.Load(Request["idOV"].ToString());
            if (op != null)
            {
                op.IdEstado = (Int16)estadoOperacionVenta.Anulado;
                op.Save();
            }

            cCuentaCorriente cc = cCuentaCorriente.GetCuentaCorrienteByIdOv(op.Id);
            if (cc != null)
            {
                cc.IdEstado = (Int16)estadoCuenta_Cuota.Anulado;
                cc.Save();
            }

            cFormaPagoOV fp = cFormaPagoOV.LoadByIdOV(op.Id);
            if (fp != null)
            {
                fp.Papelera = (Int16)papelera.Eliminado;
                fp.Save();
            }

            #region Se deja en disponibles la/s propiedades
            foreach (var item in lvUnidadesFuncionales.Items)
            {
                Label _idEu = item.FindControl("lbIdEmpresaUnidad") as Label;
                cEmpresaUnidad eu = cEmpresaUnidad.Load(_idEu.Text);
                eu.Papelera = (Int16)papelera.Eliminado;
                eu.Save();

                _idEmpresa = eu.IdEmpresa;

                cUnidad unidad = cUnidad.Load(eu.IdUnidad);
                unidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Disponible);
                unidad.Save();
            }
            #endregion

            #region Cancelo el saldo
            if (cc != null)
            {
                string _idCCU = cCuentaCorrienteUsuario.GetCuentaCorrienteByIdEmpresa(_idEmpresa);
                decimal saldoCuota = cCuota.SaldoCC(cc.Id, fp.Id, fp.Moneda);
                string _nroCuota = cCuota.NroSaldoCC(cc.Id, fp.Id, fp.Moneda);

                if (fp.GetMoneda == tipoMoneda.Pesos.ToString())
                    _cancelarSaldo += saldoCuota;
                else
                    _cancelarSaldo += cValorDolar.ConvertToPeso(saldoCuota, cValorDolar.LoadActualValue());

                string lastSaldo = cItemCCU.GetLastSaldoByIdCCU(_idCCU);
                Decimal _saldo = Convert.ToDecimal(lastSaldo);
                decimal _montoSaldo = _cancelarSaldo;

                int _nro = Convert.ToInt16(_nroCuota) + 1;
                List<cCuota> cuotas = cCuota.GetCuotasSinceNro(cc.Id, _nro.ToString());

                //El resto de las cuotas las dejo en 0
                foreach (cCuota c in cuotas)
                {
                    c.MontoAjustado = 0;
                    c.Monto = 0;
                    c.TotalComision = 0;
                    c.Vencimiento1 = 0;
                    c.Vencimiento2 = 0;
                    c.Saldo = 0;
                    c.Estado = (Int16)estadoCuenta_Cuota.Anticipo;
                    c.Save();
                }

                //Actualizo la cuota del saldo
                cCuota _cuotaActual = cCuota.GetCuotaByNro(cc.Id, Convert.ToInt16(_nroCuota), null);
                _cuotaActual.Monto = 0;
                _cuotaActual.TotalComision = 0;
                _cuotaActual.Vencimiento1 = 0;
                _cuotaActual.Vencimiento2 = 0;
                _cuotaActual.Saldo = _montoSaldo;
                _cuotaActual.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                _cuotaActual.Save();

                _nuevoSaldo = _saldo + _montoSaldo;
            }
            #endregion

            Response.Redirect("ListaOperacionVenta.aspx");
        }
        #endregion

        #region Fecha Posesión
        protected void btnEditarFechaPosesion_Click(object sender, EventArgs e)
        {
            if (lbFechaPosesion.Text != "-")
            {
                txtFechaPosesion.Text = lbFechaPosesion.Text;
            }

            pnlEditFechaPosesion.Visible = true;
            pnlFechaPosesion.Visible = false;
        }

        protected void btnSaveFechaPosesion_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFechaPosesion.Text))
            {
                cOperacionVenta ov = cOperacionVenta.Load(Request["idOV"].ToString());
                ov.FechaPosesion = Convert.ToDateTime(txtFechaPosesion.Text);
                ov.Save();
                lbFechaPosesion.Text = Convert.ToDateTime(txtFechaPosesion.Text).ToString("dd/MM/yyyy");
            }
            else
            {
                cOperacionVenta ov = cOperacionVenta.Load(Request["idOV"].ToString());
                ov.FechaPosesion = null;
                ov.Save();
                lbFechaPosesion.Text = "-";
            }

            pnlEditFechaPosesion.Visible = false;
            pnlFechaPosesion.Visible = true;
        }

        protected void btnCancelFechaPosesion_Click(object sender, EventArgs e)
        {
            pnlEditFechaPosesion.Visible = false;
            pnlFechaPosesion.Visible = true;
        }
        #endregion

        #region Fecha Escritura
        protected void btnEditarFechaEscritura_Click(object sender, EventArgs e)
        {
            if (lbFechaEscritura.Text != "-")
            {
                txtFechaEscritura.Text = lbFechaEscritura.Text;
            }

            pnlEditFechaEscritura.Visible = true;
            pnlFechaEscritura.Visible = false;
        }

        protected void btnSaveFechaEscritura_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFechaEscritura.Text))
            {
                cOperacionVenta ov = cOperacionVenta.Load(Request["idOV"].ToString());
                ov.FechaEscritura = Convert.ToDateTime(txtFechaEscritura.Text);
                ov.Save();
                lbFechaEscritura.Text = Convert.ToDateTime(txtFechaEscritura.Text).ToString("dd/MM/yyyy");
            }
            else
            {
                cOperacionVenta ov = cOperacionVenta.Load(Request["idOV"].ToString());
                ov.FechaEscritura = null;
                ov.Save();
                lbFechaEscritura.Text = "-";
            }

            pnlEditFechaEscritura.Visible = false;
            pnlFechaEscritura.Visible = true;
        }

        protected void btnCancelFechaEscritura_Click(object sender, EventArgs e)
        {
            pnlEditFechaEscritura.Visible = false;
            pnlFechaEscritura.Visible = true;
        }
        #endregion
    }
}