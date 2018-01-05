using DLL.Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Reserva : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria != (Int32)eCategoria.Administración)
                Response.Redirect("Default.aspx");
        }
    }

    #region Combo
    protected void cbOperacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbOperacion.SelectedValue == "Operacion")
        {
            pnlNuevaReserva.Visible = false;
            pnlCancelarReserva.Visible = false;
        }

        if (cbOperacion.SelectedValue == "Nueva")
        {
            pnlNuevaReserva.Visible = true;
            pnlCancelarReserva.Visible = false;

            #region Combos
            cbEmpresa.DataSource = cEmpresa.GetDataTable();
            cbEmpresa.DataValueField = "id";
            cbEmpresa.DataTextField = "nombre";
            cbEmpresa.DataBind();
            ListItem ce = new ListItem("Seleccione un cliente...", "0");
            cbEmpresa.Items.Insert(0, ce);

            cbProyectos.DataSource = cProyecto.GetDataTable();
            cbProyectos.DataValueField = "id";
            cbProyectos.DataTextField = "descripcion";
            cbProyectos.DataBind();
            ListItem io = new ListItem("Seleccione una obra...", "0");
            cbProyectos.Items.Insert(0, io);
            cbProyectos.SelectedIndex = 0;
            #endregion
        }

        if (cbOperacion.SelectedValue == "Cancelar")
        {
            pnlNuevaReserva.Visible = false;
            pnlCancelarReserva.Visible = true;

            #region Combos
            cbObraReserva.DataSource = cProyecto.GetDataTable();
            cbObraReserva.DataValueField = "id";
            cbObraReserva.DataTextField = "descripcion";
            cbObraReserva.DataBind();
            ListItem io = new ListItem("Seleccione una obra...", "0");
            cbObraReserva.Items.Insert(0, io);
            cbObraReserva.SelectedIndex = 0;
            #endregion
        }
    }

    protected void cbProyectos_SelectedIndexChanged(object sender, EventArgs e)
    {
        cbUnidadFuncional.Enabled = true;
        cbUnidadFuncional.DataSource = cUnidad.GroupByUnidadFuncional(cbProyectos.SelectedValue);
        cbUnidadFuncional.DataBind();
        ListItem tu = new ListItem("Seleccione un tipo de unidad funcional", "0");
        cbUnidadFuncional.Items.Insert(0, tu);
        cbUnidadFuncional.SelectedIndex = 0;
    }

    protected void cbUnidadFuncional_SelectedIndexChanged(object sender, EventArgs e)
    {
        cbNivel.Enabled = true;
        cbNivel.DataSource = cUnidad.GroupByNivel(cbProyectos.SelectedValue);
        cbNivel.DataBind();
        ListItem inivel = new ListItem("Seleccione un nivel...", "0");
        cbNivel.Items.Insert(0, inivel);
        cbNivel.SelectedIndex = 0;
    }

    protected void cbNivel_SelectedIndexChanged(object sender, EventArgs e)
    {
        cbUnidad.Enabled = true;
        cbUnidad.DataSource = cUnidad.GetNroUnidadByIdProyecto1(cbProyectos.SelectedValue, cbNivel.SelectedItem.Text);
        cbUnidad.DataValueField = "id";
        cbUnidad.DataTextField = "nroUnidad";
        cbUnidad.DataBind();
        ListItem iunidad = new ListItem("Seleccione el nro. de unidad...", "0");
        cbUnidad.Items.Insert(0, iunidad);
        cbUnidad.SelectedIndex = 0;
    }
    #endregion

    #region Nueva Reserva
    protected void btnFinalizar_Click(object sender, EventArgs e)
    {
        try
        {
            decimal importe = 0;
            int idItemCCU = 0;

            if (string.IsNullOrEmpty(cReserva.GetIdReservaByIdUnidad(cbUnidad.SelectedValue))) //Verifica que la unidad no este reservada
            {
                cUnidad unidad = cUnidad.Load(cbUnidad.SelectedValue);
                unidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Reservado);
                unidad.Save();

                cEmpresaUnidad eu = new cEmpresaUnidad();
                eu.CodUF = unidad.CodigoUF;
                eu.IdProyecto = unidad.IdProyecto;
                eu.IdUnidad = unidad.Id;
                eu.IdEmpresa = cbEmpresa.SelectedValue;
                eu.IdOv = "-1";
                eu.PrecioAcordado = 0;
                eu.Papelera = (Int16)papelera.Activo;
                int idEmpresaUnidad = eu.Save();

                string concepto = "Pago de reserva de unidad " + unidad.CodigoUF + " de la obra " + cProyecto.Load(unidad.IdProyecto).Descripcion;

                if (rblMoneda.SelectedValue == tipoMoneda.Dolar.ToString())
                    importe = Convert.ToDecimal(txtImporte.Text) * cValorDolar.LoadActualValue();
                else
                    importe = Convert.ToDecimal(txtImporte.Text);

                string idCuentaCorriente = cCuentaCorrienteUsuario.GetCuentaCorrienteByIdEmpresa(cbEmpresa.SelectedValue);
                if (!string.IsNullOrEmpty(idCuentaCorriente))
                {
                    string lastSaldo = cItemCCU.GetLastSaldoByIdCCU(idCuentaCorriente);
                    decimal _nuevoSaldo = Convert.ToDecimal(lastSaldo) + importe;

                    cItemCCU item = new cItemCCU();
                    item.IdCuentaCorrienteUsuario = idCuentaCorriente;
                    item.Fecha = DateTime.Now;
                    item.Concepto = concepto;
                    item.Debito = 0;
                    item.Credito = importe;
                    item.Saldo = _nuevoSaldo;
                    item.IdCuota = "-1";
                    item.IdEstado = (Int16)eEstadoItem.Reserva;
                    item.TipoOperacion = (Int16)eTipoOperacion.Reserva;
                    idItemCCU = item.Save();

                    hfIdEmpresa.Value = cbEmpresa.SelectedValue;
                    hfIdItemCCU.Value = idItemCCU.ToString();
                }
                else
                {
                    cCuentaCorrienteUsuario ccu = new cCuentaCorrienteUsuario();
                    ccu.IdEmpresa = cbEmpresa.SelectedValue;
                    ccu.Papelera = (Int16)papelera.Activo;
                    int idCCU = ccu.Save();

                    string lastSaldo = cItemCCU.GetLastSaldoByIdCCU(idCuentaCorriente);
                    decimal _nuevoSaldo = Convert.ToDecimal(lastSaldo) + importe;
                    
                    cItemCCU item = new cItemCCU();
                    item.IdCuentaCorrienteUsuario = idCCU.ToString();
                    item.Fecha = DateTime.Now;
                    item.Concepto = concepto;
                    item.Debito = 0;
                    item.Credito = importe;
                    item.Saldo = _nuevoSaldo;
                    item.IdCuota = "-1";
                    item.IdEstado = Convert.ToInt16(estadoCuenta_Cuota.Pagado);
                    idItemCCU = item.Save();

                    hfIdEmpresa.Value = cbEmpresa.SelectedValue;
                    hfIdItemCCU.Value = idItemCCU.ToString();
                }

                cReserva reserva = new cReserva(cbEmpresa.SelectedValue, unidad.Id, DateTime.Now.AddDays(10), idEmpresaUnidad.ToString(), importe, idItemCCU);
                cReciboCuota recibo = cReciboCuota.CrearRecibo("-1", idItemCCU.ToString(), importe);

                pnlMensajeReserva.Visible = false;
                pnlMensajeReservaOk.Visible = true;
            }
            else
            {
                pnlMensajeReserva.Visible = true;
                pnlMensajeReservaOk.Visible = false;
            }
        }
        catch
        {
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

    #region Cancelar Reserva
    protected void btnBuscarCancelar_Click(object sender, EventArgs e)
    {
        plnListaReserva.Visible = true;
        lvReservados.DataSource = cUnidad.Search("0", cbObraReserva.SelectedValue, Convert.ToString((Int16)estadoUnidad.Reservado), "0", "Todos", null, null, null, null, null);
        lvReservados.DataBind();
    }

    protected void btnSiguienteLista_Click(object sender, EventArgs e)
    {
        bool flag = false;

        foreach (ListViewItem item in lvReservados.Items)
        {
            
            CheckBox check = item.FindControl("chBox") as CheckBox;
            if (check.Checked)
            {
                flag = true;
            }
        }

        if (flag == true)
        {
            pnlDevolucion.Visible = true;
            pnlEstado.Visible = true;
            pnlMensaje.Visible = false;
        }
        else
        {
            pnlMensaje.Visible = true;
        }
    }

    protected void rblDevolucion_TextChanged(object sender, EventArgs e)
    {
        pnlFinalizarCancelarReserva.Visible = true;
    }

    protected void btnFinalizarCancelarReserva_Click(object sender, EventArgs e)
    {
        if (cbObraReserva.SelectedIndex != 0)
        {
            foreach (ListViewItem item in lvReservados.Items)
            {
                CheckBox check = item.FindControl("chBox") as CheckBox;

                if (check.Checked)
                {
                    Label id = item.FindControl("lbId") as Label;
                    string _idReserva = cReserva.GetIdReservaByIdUnidad(id.Text);

                    cReserva reserva = cReserva.Load(_idReserva);
                    reserva.Papelera = (Int16)papelera.Eliminado;
                    reserva.Save();

                    cUnidad unidad = cUnidad.Load(id.Text);
                    cEmpresaUnidad eu = cEmpresaUnidad.Load(reserva.IdEmpresaUnidad);

                    if (rblEstado.SelectedValue == "Si")
                    {
                        unidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Disponible);
                        unidad.Save();

                        eu.Papelera = (Int16)papelera.Eliminado;
                        eu.Save();
                    }

                    #region Actualiza la cuenta corriente
                    if (rblDevolucion.SelectedValue == "Si")
                    {
                        string idCCU = cCuentaCorrienteUsuario.GetCuentaCorrienteByIdEmpresa(eu.IdEmpresa);
                        cItemCCU itemCCU = cItemCCU.Load(reserva.IdItemCCU);
                        itemCCU.IdEstado = (Int16)eEstadoItem.Pagado;
                        itemCCU.Save();

                        decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(idCCU));
                        decimal _monto = itemCCU.Credito * -1;
                        decimal _nuevoSaldo = lastSaldo + _monto;

                        cItemCCU ccuNew = new cItemCCU();
                        ccuNew.IdCuentaCorrienteUsuario = idCCU;
                        ccuNew.Fecha = DateTime.Now;
                        ccuNew.Concepto = "Cancelación de reserva. Cod. U.F.:" + unidad.CodigoUF;
                        ccuNew.Debito = _monto;
                        ccuNew.Credito = 0;
                        ccuNew.Saldo = _nuevoSaldo;
                        ccuNew.IdCuota = "-1";
                        ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                        ccuNew.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;
                        int _idItemCCU = ccuNew.Save();

                        //Genera el recibo del pago
                        cReciboCuota recibo = cReciboCuota.CrearRecibo("-1", _idItemCCU.ToString(), _monto);
                    }
                    #endregion
                }
            }
        }

        Response.Redirect("Unidad.aspx?idProyecto=" + cbObraReserva.SelectedValue);
    }
    #endregion

    
}
