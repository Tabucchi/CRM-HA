using DLL.Negocio;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class DetalleCuota2 : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    #region Actualizar cuotas activas a pendientes
                    List<cCuota> listCuotasVencidas = cCuota.GetCuotasVencidas(Request["idCC"].ToString());
                    int index = 0;
                    if (listCuotasVencidas.Count != 0)
                    {
                        foreach (cCuota c in listCuotasVencidas)
                        {
                            index = c.Nro - 1;
                            if (index != 0)
                            {
                                c.Estado = (Int16)estadoCuenta_Cuota.Pendiente;
                                c.Save();
                            }
                            else
                            {
                                c.Estado = (Int16)estadoCuenta_Cuota.Pendiente;
                                c.Save();
                            }
                        }
                    }
                    #endregion

                    ArrayList cuotas = cCuota.GetCuotasOrderByFormaPagoOV(Request["idCC"].ToString());
                    string _idOperacionVenta = null;
                    int countFormaPago = 1;
                    foreach (var c in cuotas)
                    {
                        var uc = (UserControl)Page.LoadControl("~/Controles/Cuotas.ascx");
                        pnlListViewCuotas.Controls.Add(uc);

                        ListView lv = (ListView)uc.FindControl("lvCuotas");
                        List<cCuota> listCuotas = cCuota.GetCuotasByIdFormaPagoOV(Request["idCC"].ToString(), c.ToString());
                        lv.DataSource = listCuotas;
                        lv.DataBind();

                        cFormaPagoOV formaPago = cFormaPagoOV.Load(c.ToString());
                        _idOperacionVenta = formaPago.IdOperacionVenta;
                        hfIdOpv.Value = _idOperacionVenta;
                        int cuotasPendientes = cCuota.GetCuotasPendientes(Request["idCC"].ToString(), c.ToString()).Count;

                        cOperacionVenta ov = cOperacionVenta.Load(_idOperacionVenta);

                        if (ov.Cac == true)
                            (lv.FindControl("lbIndice") as Label).Text = "CAC";

                        if (ov.Uva == true)
                            (lv.FindControl("lbIndice") as Label).Text = "UVA";

                        if (ov.Cac == false && ov.Uva == false)
                            (lv.FindControl("lbIndice") as Label).Text = "CAC";

                        if (listCuotas.Count > 0)
                        {
                            Panel pnl = (Panel)uc.FindControl("pnlHead");
                            pnl.Visible = true;
                            Label nroFormaPago = (Label)uc.FindControl("lbNroFormaPago");
                            nroFormaPago.Text = countFormaPago.ToString();

                            Label monedaFormaPago = (Label)uc.FindControl("lbMoneda");
                            monedaFormaPago.Text = formaPago.GetMoneda;

                            Label cuotasFormaPago = (Label)uc.FindControl("lbCuotasPendientes");
                            cuotasFormaPago.Text = cuotasPendientes.ToString();
                        }

                        HiddenField _hfCC = (HiddenField)uc.FindControl("hfCC");
                        _hfCC.Value = Request["idCC"].ToString();

                        countFormaPago++;
                    }

                    cCuentaCorriente cc = cCuentaCorriente.GetCuentaCorrienteById(Request["idCC"].ToString());
                    cEmpresa empresa = cEmpresa.Load(cc.IdEmpresa);
                    lblCliente.Text = empresa.GetNombreCompleto;

                    lblProyecto.Text = cc.GetProyecto;

                    cEmpresaUnidad empresaUnidad = cEmpresaUnidad.Load(cc.IdEmpresaUnidad);
                    List<cUnidad> unidades = cUnidad.GetUnidadByIdOV(empresaUnidad.IdOv);
                    string _unidad = "";
                    int count = 1;

                    foreach (cUnidad u in unidades)
                    {
                        if (unidades.Count == 1)
                            _unidad = u.Nivel + " - " + u.UnidadFuncional + " - " + u.NroUnidad;
                        else
                        {
                            if (unidades.Count != count)
                                _unidad += u.Nivel + " - " + u.UnidadFuncional + " - " + u.NroUnidad + " <br/> ";
                            else
                                _unidad += u.Nivel + " - " + u.UnidadFuncional + " - " + u.NroUnidad;

                            lblProyecto.Text += " <br/> ";
                            count++;
                        }
                    }

                    lblUnidad.Text = _unidad;

                    cOperacionVenta op = cOperacionVenta.Load(_idOperacionVenta);
                    string _moneda = op.GetMoneda;
                    lblMonedaAcordada.Text = _moneda;
                    lblDolar.Text = op.GetValorDolar;

                    List<cFormaPagoOV> saldos = cFormaPagoOV.GetFormaPagoOVByIdOV(op.Id);
                    decimal _saldoPesos = 0;

                    foreach (cFormaPagoOV fp in saldos)
                    {
                        decimal saldoCuota = cCuota.SaldoCC(Request["idCC"].ToString(), fp.Id, fp.Moneda);
                        if (fp.GetMoneda == tipoMoneda.Pesos.ToString())
                            _saldoPesos += saldoCuota;
                        else
                            _saldoPesos += cValorDolar.ConvertToPeso(saldoCuota, op.ValorDolar);

                        if (saldoCuota == 0)
                        {
                            cCuota c = cCuota.GetFirst(Request["idCC"].ToString(), fp.Id);
                            if (c.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                            {
                                if (fp.GetMoneda == tipoMoneda.Dolar.ToString())
                                {
                                    _saldoPesos += cValorDolar.ConvertToPeso(c.MontoAjustado, op.ValorDolar);
                                }
                                else
                                {
                                    _saldoPesos += c.MontoAjustado;
                                }
                            }
                        }
                    }

                    //lblSaldoDolar.Text = String.Format("{0:#,#0.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(cc.GetSaldoPesos), op.ValorDolar));
                    lblSaldoDolar.Text = String.Format("{0:#,#0.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(cc.GetSaldoPesos), cValorDolar.LoadActualValue()));
                    lblSaldoPesos.Text = String.Format("{0:#,#0.00}", cc.GetSaldoPesos);

                    lblEstado.Text = cc.GetEstado;
                    hfCC.Value = cc.Id;
                }
                //else
                //{
                //    ArrayList cuotas = cCuota.GetCuotasOrderByFormaPagoOV(Request["idCC"].ToString());
                //    foreach (var c in cuotas)
                //    {
                //        var uc = (UserControl)Page.LoadControl("~/Controles/Cuotas.ascx");
                //        pnlListViewCuotas.Controls.Add(uc);

                //        ListView lv = (ListView)uc.FindControl("lvCuotas");
                //        lv.DataSource = cCuota.GetCuotasByIdFormaPagoOV(Request["idCC"].ToString(), c.ToString());
                //        lv.DataBind();

                //        HiddenField _hfCC = (HiddenField)uc.FindControl("hfCC");
                //        _hfCC.Value = Request["idCC"].ToString();
                //        HiddenField _hfFormaPagoOV = (HiddenField)uc.FindControl("hfIdFormaPagoOV");
                //        _hfFormaPagoOV.Value = c.ToString();
                //    }
                //}
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("DetalleCuota - " + DateTime.Now + "- " + ex.Message + " - Page_Load");
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnOpv_Click(object sender, EventArgs e)
        {
            Response.Redirect("DetalleOperacionVenta.aspx?idOV=" + hfIdOpv.Value);
        }

        //protected void btnOpv_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("DetalleOperacionVenta.aspx?idOV=" + hfIdOpv.Value);
        //}
    }
}