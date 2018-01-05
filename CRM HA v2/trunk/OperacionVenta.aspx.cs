using DLL.Negocio;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class OperacionVenta : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Administración)
                {
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

                    cbComboCac.DataSource = cIndiceCAC.GetDataTable();
                    cbComboCac.DataValueField = "id";
                    cbComboCac.DataTextField = "descripcion";
                    cbComboCac.DataBind();
                    ListItem ic = new ListItem("Seleccione el índice base...", "0");
                    cbComboCac.Items.Insert(0, ic);
                    cbComboCac.SelectedIndex = -1;
                    //ListItem ic1 = new ListItem("Sin índice base...", "1");
                    //cbComboCac.Items.Insert(1, ic1);

                    //cbComboUVA.DataSource = cUVA.GetDataTable();
                    //cbComboUVA.DataValueField = "id";
                    //cbComboUVA.DataTextField = "valor";
                    //cbComboUVA.DataBind();
                    //ListItem iu = new ListItem("Seleccione el índice base...", "0");
                    //cbComboUVA.Items.Insert(0, iu);
                    //cbComboUVA.SelectedIndex = -1;
                    //ListItem iu1 = new ListItem("Sin índice base...", "1");
                    //cbComboUVA.Items.Insert(1, iu1);

                    cbVendedor.DataSource = cUsuario.GetDataTable();
                    cbVendedor.DataValueField = "id";
                    cbVendedor.DataTextField = "nombre";
                    cbVendedor.DataBind();
                    cbVendedor.Items.Insert(0, new ListItem("Seleccione un vendedor...", "0"));
                    #endregion

                    cbMonedaAcordada.DataSource = cCampoGenerico.CargarComboMoneda();
                    cbMonedaAcordada.DataBind();

                    cbMonedaAcordada.SelectedValue = "-1";
                    string dolar = cValorDolar.LoadActualValue().ToString();
                    lbValorActualDolar.Text = "US$ " + dolar;
                    txtDolar.Text = dolar;

                    hfTotalAux.Value = "0";
                }
                else
                    Response.Redirect("Default.aspx");
            }
        }
        catch
        {
            Response.Redirect("MensajeError.aspx");
        }
    }

    #region Combo
    protected void cbProyectos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbUnidadFuncional.Enabled = true;
            cbUnidadFuncional.DataSource = cUnidad.GroupByUnidadFuncional(cbProyectos.SelectedValue);
            cbUnidadFuncional.DataBind();
            ListItem tu = new ListItem("Seleccione un tipo de unidad funcional", "0");
            cbUnidadFuncional.Items.Insert(0, tu);
            cbUnidadFuncional.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - cbProyectos_SelectedIndexChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void cbUnidadFuncional_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbNivel.Enabled = true;
            cbNivel.DataSource = cUnidad.GroupByNivel(cbProyectos.SelectedValue);
            cbNivel.DataBind();
            ListItem inivel = new ListItem("Seleccione un nivel...", "0");
            cbNivel.Items.Insert(0, inivel);
            cbNivel.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - cbUnidadFuncional_SelectedIndexChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void cbNivel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbUnidad.Enabled = true;
            cbUnidad.DataSource = cUnidad.GetNroUnidadByIdProyecto(cbProyectos.SelectedValue, cbNivel.SelectedItem.Text);
            cbUnidad.DataBind();
            ListItem iunidad = new ListItem("Seleccione el nro. de unidad...", "0");
            cbUnidad.Items.Insert(0, iunidad);
            cbUnidad.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - cbNivel_SelectedIndexChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void cbUnidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cUnidad unidad = cUnidad.GetUnidadByProyecto(cbProyectos.SelectedValue, cbNivel.SelectedItem.Text, cbUnidad.SelectedItem.Text);
            lbPrecio.Text = unidad.GetPrecioBase;
            lbMoneda.Text = "(" + unidad.GetMoneda + ")";
            hfCodUfUnidad.Value = unidad.Id;
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - cbUnidad_SelectedIndexChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void cbOperacionMoneda_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            switch (cbOperacionMoneda.SelectedValue)
            {
                case "-1":
                    pnlMonedaAcordada.Visible = false;
                    pnlDolarPeso.Visible = false;
                    break;
                case "0":
                    pnlMonedaAcordada.Visible = true;
                    pnlDolarPeso.Visible = false;
                    pnlCuota2.Visible = false;

                    if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
                    {
                        if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                        {
                            convertirMonedaAcordada.Visible = true;
                            convertirMonedaAcordada2.Visible = true;
                        }
                        else
                        {
                            convertirMonedaAcordada.Visible = false;
                            convertirMonedaAcordada2.Visible = false;
                        }
                    }

                    if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                    {
                        if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                        {
                            convertirMonedaAcordada.Visible = true;
                        }
                        else
                        {
                            convertirMonedaAcordada.Visible = false;
                        }
                    }
                    break;
                case "1":
                    pnlMonedaAcordada.Visible = true;
                    pnlDolarPeso.Visible = false;
                    break;
                case "2":
                    pnlMonedaAcordada.Visible = false;
                    pnlDolarPeso.Visible = true;
                    break;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - cbOperacionMoneda_SelectedIndexChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void cbMonedaAcordada_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
            {
                if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                {
                    convertirDolarPeso.Visible = true;
                    convertirDolarPeso2.Visible = true;
                }
                else
                {
                    convertirDolarPeso.Visible = false;
                    convertirDolarPeso2.Visible = false;
                }

                if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                {
                    convertirPesoDolar.Visible = true;
                    convertirPesoDolar2.Visible = true;
                }
                else
                {
                    convertirPesoDolar.Visible = false;
                    convertirPesoDolar2.Visible = false;
                }
            }

            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
            {
                if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                {
                    convertirPesoDolar.Visible = true;
                    convertirPesoDolar2.Visible = true;
                }
                else
                {
                    convertirPesoDolar.Visible = false;
                    convertirPesoDolar2.Visible = false;
                }

                if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                {
                    convertirDolarPeso.Visible = true;
                    convertirDolarPeso2.Visible = true;
                }
                else
                {
                    convertirDolarPeso.Visible = false;
                    convertirDolarPeso2.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - cbMonedaAcordada_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void cbTipoPago_TextChanged(object sender, EventArgs e)
    {
        try
        {
            switch (cbTipoPago.SelectedValue)
            {
                case "0":
                    pnlCuotas.Visible = false;
                    rfv21.Enabled = false;
                    break;
                case "1":
                    pnlCuotas.Visible = false;
                    rfv21.Enabled = true;
                    break;
                case "2":
                    pnlCuotas.Visible = true;
                    rfv21.Enabled = false;
                    break;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - cbTipoPago_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void chbCuotasManuales_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chbCuotasManuales.Checked)
            {
                pnlFechaVencAcordada.Visible = false;
                pnlRangoAcordada.Visible = false;

                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                {
                    pnlCuotasManualesMonedaAcordada1.Visible = true;
                    pnlCuotasManualesMonedaAcordada1CAC.Visible = false;

                    List<cFormaPagoOV> fp = new List<cFormaPagoOV>();
                    lvFormaPago.DataBind();

                    int aux = 0;
                    while (aux < Convert.ToDecimal(txtCuotasMonedaAcordada.Text))
                    {
                        cFormaPagoOV u = new cFormaPagoOV();
                        u.Monto = Convert.ToDecimal(lbValorMonedaAcordada.Text);
                        u.CantCuotas = Convert.ToInt16("1");
                        u.Valor = Convert.ToDecimal("0");
                        u.FechaVencimiento = Convert.ToDateTime(DateTime.Now);
                        u.GastosAdtvo = chbGastosManuales.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                        fp.Add(u);

                        aux++;
                    }

                    lvFormaPago.DataSource = fp;
                    lvFormaPago.DataBind();
                }
                else
                {
                    pnlCuotasManualesMonedaAcordada1.Visible = false;
                    pnlCuotasManualesMonedaAcordada1CAC.Visible = true;

                    List<cFormaPagoOV> fp = new List<cFormaPagoOV>();
                    lvFormaPagoCAC.DataBind();

                    int aux = 0;
                    while (aux < Convert.ToDecimal(txtCuotasMonedaAcordada.Text))
                    {
                        cFormaPagoOV u = new cFormaPagoOV();
                        u.Monto = Convert.ToDecimal(lbValorMonedaAcordada.Text);
                        u.CantCuotas = Convert.ToInt16("1");
                        u.Valor = Convert.ToDecimal("0");
                        u.FechaVencimiento = Convert.ToDateTime(DateTime.Now);
                        u.GastosAdtvo = chbGastosManuales.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                        fp.Add(u);

                        aux++;
                    }

                    lvFormaPagoCAC.DataSource = fp;
                    lvFormaPagoCAC.DataBind();
                }
            }
            else
            {
                pnlFechaVencAcordada.Visible = true;
                if (pnlRangoAcordada.Visible == false)
                    pnlRangoAcordada.Visible = false;
                else
                    pnlRangoAcordada.Visible = true;
                pnlCuotasManualesMonedaAcordada1.Visible = false;
                pnlCuotasManualesMonedaAcordada1CAC.Visible = false;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - chbCuotasManuales_CheckedChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void chbCuotasManuales2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chbCuotasManuales2.Checked)
            {
                pnlFechaVencAcordada2.Visible = false;
                pnlRangoAcordada2.Visible = false;

                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                {
                    pnlCuotasManualesMonedaAcordada2.Visible = true;
                    pnlCuotasManualesMonedaAcordada2CAC.Visible = false;

                    List<cFormaPagoOV> fp = new List<cFormaPagoOV>();
                    lvFormaPago2.DataBind();

                    int aux = 0;
                    while (aux < Convert.ToDecimal(txtCuotasMonedaAcordada2.Text))
                    {
                        cFormaPagoOV u = new cFormaPagoOV();
                        u.Monto = Convert.ToDecimal(lbValorMonedaAcordada2.Text);
                        u.CantCuotas = Convert.ToInt16("1");
                        u.Valor = Convert.ToDecimal("0");
                        u.FechaVencimiento = Convert.ToDateTime(DateTime.Now);
                        fp.Add(u);
                        u.GastosAdtvo = chbGastosManuales2.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                        aux++;
                    }

                    lvFormaPago2.DataSource = fp;
                    lvFormaPago2.DataBind();
                }
                else
                {
                    pnlCuotasManualesMonedaAcordada2.Visible = false;
                    pnlCuotasManualesMonedaAcordada2CAC.Visible = true;

                    List<cFormaPagoOV> fp = new List<cFormaPagoOV>();
                    lvFormaPago2CAC.DataBind();

                    int aux = 0;
                    while (aux < Convert.ToDecimal(txtCuotasMonedaAcordada2.Text))
                    {
                        cFormaPagoOV u = new cFormaPagoOV();
                        u.Monto = Convert.ToDecimal(lbValorMonedaAcordada2.Text);
                        u.CantCuotas = Convert.ToInt16("1");
                        u.Valor = Convert.ToDecimal("0");
                        u.FechaVencimiento = Convert.ToDateTime(DateTime.Now);
                        u.GastosAdtvo = chbGastosManuales2.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                        fp.Add(u);

                        aux++;
                    }

                    lvFormaPago2CAC.DataSource = fp;
                    lvFormaPago2CAC.DataBind();
                }
            }
            else
            {
                pnlFechaVencAcordada2.Visible = true;
                if (pnlRangoAcordada2.Visible == false)
                    pnlRangoAcordada2.Visible = false;
                else
                    pnlRangoAcordada2.Visible = true;
                pnlCuotasManualesMonedaAcordada2.Visible = false;
                pnlCuotasManualesMonedaAcordada2CAC.Visible = false;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - chbCuotasManuales2_CheckedChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void chbCuotasManualesDolarPeso_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //Habilita el listView con la cantidad de fila que ingresa el textbox de Cantidad de Cuotas
            if (chbCuotasManualesDolarPeso.Checked)
            {
                pnlFechaVencDolarPeso.Visible = false;
                pnlCuotasManualesMonedaDolarPeso1.Visible = true;

                List<cFormaPagoOV> fp = new List<cFormaPagoOV>();

                int aux = 0;
                while (aux < Convert.ToDecimal(txtCuotasMonedaDolar.Text))
                {
                    cFormaPagoOV u = new cFormaPagoOV();
                    u.Monto = Convert.ToDecimal(lbValorMonedaDolar.Text);
                    u.CantCuotas = Convert.ToInt16("1");
                    u.Valor = Convert.ToDecimal("0");
                    u.FechaVencimiento = Convert.ToDateTime(DateTime.Now);

                    fp.Add(u);

                    aux++;
                }

                lvFormaPagoDolarPeso.DataSource = fp;
                lvFormaPagoDolarPeso.DataBind();
            }
            else
            {
                pnlFechaVencDolarPeso.Visible = true;
                pnlCuotasManualesMonedaDolarPeso1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - chbCuotasManualesDolarPeso_CheckedChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void chbCuotasManualesDolarPeso2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //Habilita el listView con la cantidad de fila que ingresa el textbox de Cantidad de Cuotas
            if (chbCuotasManualesDolarPeso2.Checked)
            {
                pnlFechaVencDolarPeso2.Visible = false;
                pnlCuotasManualesMonedaDolarPeso2.Visible = true;

                List<cFormaPagoOV> fp = new List<cFormaPagoOV>();

                int aux = 0;
                while (aux < Convert.ToDecimal(txtCuotasMonedaDolar2.Text))
                {
                    cFormaPagoOV u = new cFormaPagoOV();
                    u.Monto = Convert.ToDecimal(lbValorMonedaDolar2.Text);
                    u.CantCuotas = Convert.ToInt16("1");
                    u.Valor = Convert.ToDecimal("0");
                    u.FechaVencimiento = Convert.ToDateTime(DateTime.Now);

                    fp.Add(u);

                    aux++;
                }

                lvFormaPagoDolarPeso2.DataSource = fp;
                lvFormaPagoDolarPeso2.DataBind();
            }
            else
            {
                pnlFechaVencDolarPeso2.Visible = true;
                pnlCuotasManualesMonedaDolarPeso2.Visible = false;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - chbCuotasManualesDolarPeso2_CheckedChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void chbCuotasManualesDolarPesoPeso_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //Habilita el listView con la cantidad de fila que ingresa el textbox de Cantidad de Cuotas
            if (chbCuotasManualesDolarPesoPeso.Checked)
            {
                pnlFechaVencPeso.Visible = false;
                pnlRangoPeso.Visible = false;
                pnlCuotasManualesMonedaDolarPesoPeso.Visible = true;

                List<cFormaPagoOV> fp = new List<cFormaPagoOV>();

                int aux = 0;
                while (aux < Convert.ToDecimal(txtCuotasMonedaPeso.Text))
                {
                    cFormaPagoOV u = new cFormaPagoOV();
                    u.Monto = Convert.ToDecimal(lbValorMonedaPeso.Text);
                    u.CantCuotas = Convert.ToInt16("1");
                    u.Valor = Convert.ToDecimal("0");
                    u.FechaVencimiento = Convert.ToDateTime(DateTime.Now);

                    fp.Add(u);

                    aux++;
                }

                lvFormaPagoDolarPesoPeso.DataSource = fp;
                lvFormaPagoDolarPesoPeso.DataBind();
            }
            else
            {
                pnlFechaVencPeso.Visible = true;
                pnlRangoPeso.Visible = true;
                pnlCuotasManualesMonedaDolarPesoPeso.Visible = false;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - chbCuotasManualesDolarPesoPeso_CheckedChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void chbCuotasManualesDolarPesoPeso3_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //Habilita el listView con la cantidad de fila que ingresa el textbox de Cantidad de Cuotas
            if (chbCuotasManualesDolarPesoPeso3.Checked)
            {
                pnlFechaVencPeso3.Visible = false;
                pnlRangoPeso3.Visible = false;
                pnlCuotasManualesMonedaDolarPesoPeso2.Visible = true;

                List<cFormaPagoOV> fp = new List<cFormaPagoOV>();

                int aux = 0;
                while (aux < Convert.ToDecimal(txtCuotasMonedaPeso3.Text))
                {
                    cFormaPagoOV u = new cFormaPagoOV();
                    u.Monto = Convert.ToDecimal(lbValorMonedaPeso3.Text);
                    u.CantCuotas = Convert.ToInt16("1");
                    u.Valor = Convert.ToDecimal("0");
                    u.FechaVencimiento = Convert.ToDateTime(DateTime.Now);

                    fp.Add(u);

                    aux++;
                }

                lvFormaPagoDolarPesoPeso2.DataSource = fp;
                lvFormaPagoDolarPesoPeso2.DataBind();
            }
            else
            {
                pnlFechaVencPeso3.Visible = true;
                pnlRangoPeso3.Visible = true;
                pnlCuotasManualesMonedaDolarPesoPeso2.Visible = false;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - chbCuotasManualesDolarPesoPeso3_CheckedChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

    #region Bótones
    protected void btnAgregarUF_Click(object sender, EventArgs e)
    {
        try
        {
            List<cUnidad> listUnidades = new List<cUnidad>();
            decimal totalLista = 0;
            decimal totalAcordado = 0;
            string _auxTipoUnidad = null;

            cUnidad unidad = cUnidad.Load(hfCodUfUnidad.Value);

            foreach (var item in lvUnidades.Items)
            {
                Label idUnidad = item.FindControl("lbId") as Label;
                cUnidad u = cUnidad.Load(idUnidad.Text);
                switch (cbMonedaAcordada.SelectedValue)
                {
                    case "0":
                        if (u.GetMoneda == tipoMoneda.Pesos.ToString())
                        {
                            u.PrecioBase = cValorDolar.ConvertToDolar(u.PrecioBase);
                            u.Moneda = Convert.ToInt16(tipoMoneda.Dolar).ToString();
                        }
                        break;
                    case "1":
                        if (u.GetMoneda == tipoMoneda.Dolar.ToString())
                        {
                            u.PrecioBase = cValorDolar.ConvertToPeso(u.PrecioBase);
                            u.Moneda = Convert.ToInt16(tipoMoneda.Pesos).ToString();
                        }
                        break;
                }
                Label _precioAcordado = item.FindControl("lbPrecioAcordado") as Label;
                u.PrecioAcordado = Convert.ToDecimal(_precioAcordado.Text);

                _auxTipoUnidad = u.UnidadFuncional;

                totalAcordado += u.PrecioAcordado;
                totalLista += u.PrecioBase;

                listUnidades.Add(u);
            }

            if (unidad.UnidadFuncional != _auxTipoUnidad)
            {
                unidad.PrecioAcordado = Convert.ToDecimal(txtPrecioAcordado.Text);
                listUnidades.Add(unidad);

                totalAcordado += unidad.PrecioAcordado;
                totalLista += unidad.PrecioBase;

                if (unidad.GetMoneda == tipoMoneda.Dolar.ToString())
                {
                    lbPrecioLista.Text = String.Format("{0:#,#}", totalLista);
                    lbPrecioListaPesos.Text = String.Format("{0:#,#}", cValorDolar.ConvertToPeso(totalLista));
                }
                else
                {
                    lbPrecioLista.Text = String.Format("{0:#,#}", cValorDolar.ConvertToDolar(totalLista));
                    lbPrecioListaPesos.Text = String.Format("{0:#,#}", totalLista);
                }

                lbPrecioAcordado.Text = String.Format("{0:#,#}", totalAcordado);

                lbMonedaUnidad.Text = unidad.GetMoneda;

                lvUnidades.DataSource = listUnidades;
                lvUnidades.DataBind();
                pnlMensajeUF.Visible = false;
            }
            else
            {
                pnlMensajeUF.Visible = true;
            }

            LimpiarCampos();
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - btnAgregarUF_Click");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void btnFinalizar_Click(object sender, EventArgs e)
    {
        if (pnlMonedaAcordada.Visible == true)
        {
            if (Convert.ToDecimal(lbTotalMonedaAcordada.Text) != Convert.ToDecimal(lbPrecioAcordado.Text))
            {
                if (cValorDolar.ConvertToDolar(Convert.ToDecimal(hfTotalAux.Value), Convert.ToDecimal(txtDolar.Text)) != Math.Round(Convert.ToDecimal(lbPrecioAcordado.Text))
                        && cValorDolar.ConvertToPeso(Convert.ToDecimal(hfTotalAux.Value), Convert.ToDecimal(txtDolar.Text)) != Math.Round(Convert.ToDecimal(lbPrecioAcordado.Text)))
                    divTotalMonedaAcordada.Attributes.Add("class", "divMessageError");
                else
                    CargarOperacionVenta_FormaPago();
            }
            else
            {
                CargarOperacionVenta_FormaPago();
            }
        }

        if (pnlDolarPeso.Visible == true)
        {
            if (Convert.ToDecimal(lbTotalDolarPeso.Text) != Convert.ToDecimal(lbPrecioAcordado.Text))
            {
                if (cValorDolar.ConvertToDolar(Convert.ToDecimal(hfTotalAux.Value), Convert.ToDecimal(txtDolar.Text)) != Math.Round(Convert.ToDecimal(lbPrecioAcordado.Text))
                        && cValorDolar.ConvertToPeso(Convert.ToDecimal(hfTotalAux.Value), Convert.ToDecimal(txtDolar.Text)) != Math.Round(Convert.ToDecimal(lbPrecioAcordado.Text)))
                    divTotalDolarPeso.Attributes.Add("class", "divMessageError");
                else
                    CargarOperacionVenta_FormaPago();
            }
            else
            {
                CargarOperacionVenta_FormaPago();
            }
        }

        if (pnlMonedaAcordada.Visible == false && pnlDolarPeso.Visible == false)
        {
            CargarOperacionVenta_FormaPago();
        }
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("ListaOperacionVenta.aspx");
    }
    #endregion

    #region Método Carga Operación de venta
    public void CargarOperacionVenta_FormaPago()
    {
        int idEmpresaUnidad = 0;
        decimal valorviejo = 0;

        if (lvUnidades.Items.Count != 0)
        {
            #region Operacion Venta
            cOperacionVenta ov = new cOperacionVenta();
            ov.IdEmpresaUnidad = "-1";
            ov.PrecioAcordado = Convert.ToDecimal(lbPrecioAcordado.Text);
            ov.MonedaAcordada = cAuxiliar.GetMonedaByDescripcion(cbMonedaAcordada.SelectedValue);
            ov.Anticipo = 0;

            if (pnlComboCAC.Visible == true)
            {
                ov.Cac = true;
                ov.IdIndiceCAC = cbComboCac.SelectedValue;
            }
            else
            {
                ov.Cac = false;
                ov.IdIndiceCAC = "0";
            }

            if (pnlComboUVA.Visible == true)
            {
                ov.Uva = true;
                ov.ValorBaseUVA = Convert.ToDecimal(txtUVA.Text);
            }
            else
            {
                ov.Uva = false;
                ov.ValorBaseUVA = 0;
            }

            ov.TotalComision = Convert.ToDecimal(txtComision.Text);
            ov.Iva = chbIva.Checked;
            ov.ValorDolar = Convert.ToDecimal(txtDolar.Text.Replace("US$", ""));
            ov.IdEstado = (Int16)estadoOperacionVenta.A_confirmar;
            ov.Fecha = Convert.ToDateTime(txtFechaOV.Text);

            if (!string.IsNullOrEmpty(txtFechaPosesion.Text))
                ov.FechaPosesion = Convert.ToDateTime(txtFechaPosesion.Text);
            
            if (!string.IsNullOrEmpty(txtFechaEscritura.Text))
                ov.FechaEscritura = Convert.ToDateTime(txtFechaEscritura.Text);

            int idOV = ov.Save();
            #endregion

            #region Asociar Cliente con unidades
            cEmpresaUnidad eu = new cEmpresaUnidad();

            foreach (var item in lvUnidades.Items)
            {
                Label idUnidad = item.FindControl("lbId") as Label;
                cUnidad u = cUnidad.Load(idUnidad.Text);

                cEmpresaUnidad validarEU = cEmpresaUnidad.GetUnidad(u.CodigoUF, u.IdProyecto);
                if (validarEU != null)
                {
                    validarEU.Papelera = (Int16)papelera.Eliminado;
                    validarEU.Save();
                }

                eu.CodUF = u.CodigoUF;
                eu.IdProyecto = u.IdProyecto;
                eu.IdUnidad = u.Id;
                eu.IdEmpresa = cbEmpresa.SelectedValue;
                eu.IdOv = idOV.ToString();
                Label _precioAcordado = item.FindControl("lbPrecioAcordado") as Label;

                cOperacionVenta o = cOperacionVenta.Load(idOV.ToString());
                decimal asdad = ov.ValorDolar;

                if (o.MonedaAcordada == Convert.ToString((Int16)tipoMoneda.Pesos))
                    eu.PrecioAcordado = Convert.ToDecimal(_precioAcordado.Text) * o.ValorDolar;
                else
                    eu.PrecioAcordado = Convert.ToDecimal(_precioAcordado.Text);
                
                eu.Papelera = (Int16)papelera.Activo;
                idEmpresaUnidad = eu.Save();
                u.IdEstado = Convert.ToString((Int16)estadoUnidad.Vendido);
                u.IdUsuario = cbVendedor.SelectedValue;
                valorviejo = u.PrecioBase;
                u.PrecioBase = Convert.ToDecimal(_precioAcordado.Text);
                u.Save();

                cReserva unidadReservada = cReserva.GetReservaByIdUnidad(idUnidad.Text);
                unidadReservada.Papelera = (Int16)papelera.Eliminado;
                unidadReservada.Save();

                cHistorial _historial = new cHistorial(DateTime.Now, historial.Evolución_de_precios.ToString(), valorviejo, Convert.ToDecimal(_precioAcordado.Text), u.CodigoUF, u.NroUnidad, u.IdEstado, u.IdEstado, HttpContext.Current.User.Identity.Name, u.IdProyecto);
                _historial.Save();
            }
            #endregion

            cOperacionVenta _ov = cOperacionVenta.Load(idOV.ToString());
            _ov.IdEmpresaUnidad = idEmpresaUnidad.ToString();
            _ov.Save();

            #region Forma de Pago
            #region Moneda acordada
            if (pnlMonedaAcordada.Visible == true)
            {
                //Panel Cuota 1
                if (!chbCuotasManuales.Checked)
                {
                    cFormaPagoOV fg = new cFormaPagoOV();
                    fg.IdOperacionVenta = idOV.ToString();
                    fg.Papelera = (Int16)papelera.Activo;

                    fg.Moneda = cAuxiliar.GetMonedaByDescripcion(cbMonedaAcordada.SelectedValue);
                    fg.Monto = Convert.ToDecimal(txtMontoMonedaAcordada.Text);
                    fg.Saldo = Convert.ToDecimal(txtMontoMonedaAcordada.Text);
                    fg.CantCuotas = Convert.ToInt16(txtCuotasMonedaAcordada.Text);
                    fg.Valor = Convert.ToDecimal(lbValorMonedaAcordada.Text);
                    fg.FechaVencimiento = Convert.ToDateTime(txtFechaVencimientoMonedaAcordada.Text);
                    fg.RangoCuotaCAC = txtRangoDesdeAcordado.Text + " - " + txtRangoAAcordado.Text;
                    fg.GastosAdtvo = chbGastosManuales.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                    fg.InteresAnual = Convert.ToDecimal(txtInteresAnual.Text);
                    fg.Save();
                }
                else
                {
                    //ListView de los refuerzos en dólares
                    if (pnlCuotasManualesMonedaAcordada1.Visible == true)
                    {
                        foreach (var item in lvFormaPago.Items)
                        {
                            cFormaPagoOV fg = new cFormaPagoOV();
                            fg.IdOperacionVenta = idOV.ToString();
                            fg.Papelera = (Int16)papelera.Activo;

                            fg.Moneda = cAuxiliar.GetMonedaByDescripcion(cbMonedaAcordada.SelectedValue);
                            TextBox _monto = item.FindControl("lbTotal") as TextBox;
                            fg.Monto = Convert.ToDecimal(_monto.Text);
                            fg.Saldo = Convert.ToDecimal(_monto.Text);
                            Label _cantCuotas = item.FindControl("lbCantCuotas") as Label;
                            fg.CantCuotas = Convert.ToInt16(_cantCuotas.Text);
                            Label _valorCuota = item.FindControl("lbMontoCuota") as Label;
                            fg.Valor = Convert.ToDecimal(_monto.Text);
                            TextBox _fecha = item.FindControl("lbFechaVenc") as TextBox;
                            fg.FechaVencimiento = Convert.ToDateTime(_fecha.Text);
                            fg.RangoCuotaCAC = txtRangoDesdeAcordado.Text + " - " + txtRangoAAcordado.Text;
                            CheckBox _gastosAdtvo = item.FindControl("chbGastosAdtvo") as CheckBox;
                            if (_gastosAdtvo.Checked)
                                fg.GastosAdtvo = Convert.ToString((Int16)eGastosAdtvo.Si);
                            else
                                fg.GastosAdtvo = Convert.ToString((Int16)eGastosAdtvo.No);
                            fg.InteresAnual = Convert.ToDecimal(txtInteresAnual.Text);
                            fg.Save();
                        }
                    }

                    //ListView de los refuerzos en pesos, con la columna de CAC
                    if (pnlCuotasManualesMonedaAcordada1CAC.Visible == true)
                    {
                        foreach (var item in lvFormaPagoCAC.Items)
                        {
                            cFormaPagoOV fg = new cFormaPagoOV();
                            fg.IdOperacionVenta = idOV.ToString();
                            fg.Papelera = (Int16)papelera.Activo;

                            fg.Moneda = cAuxiliar.GetMonedaByDescripcion(cbMonedaAcordada.SelectedValue);
                            TextBox _monto = item.FindControl("lbTotal") as TextBox;
                            fg.Monto = Convert.ToDecimal(_monto.Text);
                            fg.Saldo = Convert.ToDecimal(_monto.Text);
                            Label _cantCuotas = item.FindControl("lbCantCuotas") as Label;
                            fg.CantCuotas = Convert.ToInt16(_cantCuotas.Text);
                            Label _valorCuota = item.FindControl("lbMontoCuota") as Label;
                            fg.Valor = Convert.ToDecimal(_monto.Text);
                            TextBox _fecha = item.FindControl("lbFechaVenc") as TextBox;
                            fg.FechaVencimiento = Convert.ToDateTime(_fecha.Text);
                            CheckBox _ajusteCAC = item.FindControl("chbAjusteCAC") as CheckBox;
                            if (_ajusteCAC.Checked)
                                fg.RangoCuotaCAC = "-1"; //true
                            else
                                fg.RangoCuotaCAC = "0";
                            fg.GastosAdtvo = chbGastosManuales.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                            fg.InteresAnual = Convert.ToDecimal(txtInteresAnual.Text);
                            fg.Save();
                        }
                    }
                }

                if (pnlCuota2.Visible == true)
                {
                    if (!chbCuotasManuales2.Checked)
                    {
                        cFormaPagoOV fg2 = new cFormaPagoOV();
                        fg2.IdOperacionVenta = idOV.ToString();
                        fg2.Papelera = (Int16)papelera.Activo;

                        fg2.Moneda = cAuxiliar.GetMonedaByDescripcion(cbMonedaAcordada.SelectedValue);
                        fg2.Monto = Convert.ToDecimal(txtMontoMonedaAcordada2.Text);
                        fg2.Saldo = Convert.ToDecimal(txtMontoMonedaAcordada2.Text);
                        fg2.CantCuotas = Convert.ToInt16(txtCuotasMonedaAcordada2.Text);
                        fg2.Valor = Convert.ToDecimal(lbValorMonedaAcordada2.Text);
                        fg2.FechaVencimiento = Convert.ToDateTime(txtFechaVencimientoMonedaAcordada2.Text);
                        fg2.RangoCuotaCAC = txtRangoDesdeAcordado2.Text + " - " + txtRangoAAcordado2.Text;
                        fg2.GastosAdtvo = chbGastosManuales2.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                        fg2.InteresAnual = Convert.ToDecimal(txtInteresAnual2.Text);
                        fg2.Save();
                    }
                    else
                    {
                        //ListView de los refuerzos en dólares
                        if (pnlCuotasManualesMonedaAcordada2.Visible == true)
                        {
                            foreach (var item in lvFormaPago2.Items)
                            {
                                cFormaPagoOV fg = new cFormaPagoOV();
                                fg.IdOperacionVenta = idOV.ToString();
                                fg.Papelera = (Int16)papelera.Activo;

                                fg.Moneda = cAuxiliar.GetMonedaByDescripcion(cbMonedaAcordada.SelectedValue);
                                TextBox _monto = item.FindControl("lbTotal") as TextBox;
                                fg.Monto = Convert.ToDecimal(_monto.Text);
                                fg.Saldo = Convert.ToDecimal(_monto.Text);
                                Label _cantCuotas = item.FindControl("lbCantCuotas") as Label;
                                fg.CantCuotas = Convert.ToInt16(_cantCuotas.Text);
                                Label _valorCuota = item.FindControl("lbMontoCuota") as Label;
                                fg.Valor = Convert.ToDecimal(_monto.Text);
                                TextBox _fecha = item.FindControl("lbFechaVenc") as TextBox;
                                fg.FechaVencimiento = Convert.ToDateTime(_fecha.Text);
                                fg.RangoCuotaCAC = txtRangoDesdeAcordado.Text + " - " + txtRangoAAcordado.Text;
                                CheckBox _gastosAdtvo = item.FindControl("chbGastosAdtvo") as CheckBox;
                                if (_gastosAdtvo.Checked)
                                    fg.GastosAdtvo = Convert.ToString((Int16)eGastosAdtvo.Si);
                                else
                                    fg.GastosAdtvo = Convert.ToString((Int16)eGastosAdtvo.No);
                                fg.InteresAnual = Convert.ToDecimal(txtInteresAnual2.Text);
                                fg.Save();
                            }
                        }

                        //ListView de los refuerzos en pesos, con la columna de CAC
                        if (pnlCuotasManualesMonedaAcordada2CAC.Visible == true)
                        {
                            foreach (var item in lvFormaPago2CAC.Items)
                            {
                                cFormaPagoOV fg = new cFormaPagoOV();
                                fg.IdOperacionVenta = idOV.ToString();
                                fg.Papelera = (Int16)papelera.Activo;

                                fg.Moneda = cAuxiliar.GetMonedaByDescripcion(cbMonedaAcordada.SelectedValue);
                                TextBox _monto = item.FindControl("lbTotal") as TextBox;
                                fg.Monto = Convert.ToDecimal(_monto.Text);
                                fg.Saldo = Convert.ToDecimal(_monto.Text);
                                Label _cantCuotas = item.FindControl("lbCantCuotas") as Label;
                                fg.CantCuotas = Convert.ToInt16(_cantCuotas.Text);
                                Label _valorCuota = item.FindControl("lbMontoCuota") as Label;
                                fg.Valor = Convert.ToDecimal(_monto.Text);
                                TextBox _fecha = item.FindControl("lbFechaVenc") as TextBox;
                                fg.FechaVencimiento = Convert.ToDateTime(_fecha.Text);
                                CheckBox _ajusteCAC = item.FindControl("chbAjusteCAC") as CheckBox;
                                if (_ajusteCAC.Checked)
                                    fg.RangoCuotaCAC = "-1"; //true
                                else
                                    fg.RangoCuotaCAC = "0";
                                fg.GastosAdtvo = chbGastosManuales2.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                                fg.InteresAnual = Convert.ToDecimal(txtInteresAnual2.Text);
                                fg.Save();
                            }
                        }
                    }
                }
            }
            #endregion

            #region Dolar-Peso
            if (pnlDolarPeso.Visible == true)
            {
                #region Dolar
                if (!chbCuotasManualesDolarPeso.Checked)
                {
                    cFormaPagoOV fgDolar = new cFormaPagoOV();
                    fgDolar.IdOperacionVenta = idOV.ToString();
                    fgDolar.Papelera = (Int16)papelera.Activo;

                    fgDolar.Moneda = Convert.ToString((Int16)tipoMoneda.Dolar);
                    fgDolar.Monto = Convert.ToDecimal(txtMontoMonedaDolar.Text);
                    fgDolar.Saldo = Convert.ToDecimal(txtMontoMonedaDolar.Text);
                    fgDolar.CantCuotas = Convert.ToInt16(txtCuotasMonedaDolar.Text);
                    fgDolar.Valor = Convert.ToDecimal(lbValorMonedaDolar.Text);
                    fgDolar.FechaVencimiento = Convert.ToDateTime(txtFechaVencimientoMonedaDolar.Text);
                    fgDolar.RangoCuotaCAC = "0";
                    fgDolar.GastosAdtvo = chbGastosDolarPeso.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                    fgDolar.InteresAnual = Convert.ToDecimal(txtInteresAnualDolarPeso.Text);
                    fgDolar.Save();
                }
                else
                {
                    foreach (var item in lvFormaPagoDolarPeso.Items)
                    {
                        cFormaPagoOV fg = new cFormaPagoOV();
                        fg.IdOperacionVenta = idOV.ToString();
                        fg.Papelera = (Int16)papelera.Activo;

                        fg.Moneda = Convert.ToString((Int16)tipoMoneda.Dolar);
                        TextBox _monto = item.FindControl("lbTotal") as TextBox;
                        fg.Monto = Convert.ToDecimal(_monto.Text);
                        fg.Saldo = Convert.ToDecimal(_monto.Text);
                        Label _cantCuotas = item.FindControl("lbCantCuotas") as Label;
                        fg.CantCuotas = Convert.ToInt16(_cantCuotas.Text);
                        Label _valorCuota = item.FindControl("lbMontoCuota") as Label;
                        fg.Valor = Convert.ToDecimal(_monto.Text);
                        TextBox _fecha = item.FindControl("lbFechaVenc") as TextBox;
                        fg.FechaVencimiento = Convert.ToDateTime(_fecha.Text);
                        fg.RangoCuotaCAC = "0";
                        CheckBox _gastosAdtvo = item.FindControl("chbGastosAdtvo") as CheckBox;
                        if (_gastosAdtvo.Checked)
                            fg.GastosAdtvo = Convert.ToString((Int16)eGastosAdtvo.Si);
                        else
                            fg.GastosAdtvo = Convert.ToString((Int16)eGastosAdtvo.No);
                        fg.InteresAnual = Convert.ToDecimal(txtInteresAnualDolarPeso.Text);
                        fg.Save();
                    }
                }

                if (pnlDolarPeso2.Visible == true)
                {
                    if (!chbCuotasManualesDolarPeso2.Checked)
                    {
                        cFormaPagoOV fgDolar2 = new cFormaPagoOV();
                        fgDolar2.IdOperacionVenta = idOV.ToString();
                        fgDolar2.Papelera = (Int16)papelera.Activo;

                        fgDolar2.Moneda = Convert.ToString((Int16)tipoMoneda.Dolar);
                        fgDolar2.Monto = Convert.ToDecimal(txtMontoMonedaDolar2.Text);
                        fgDolar2.Saldo = Convert.ToDecimal(txtMontoMonedaDolar2.Text);
                        fgDolar2.CantCuotas = Convert.ToInt16(txtCuotasMonedaDolar2.Text);
                        fgDolar2.Valor = Convert.ToDecimal(lbValorMonedaDolar2.Text);
                        fgDolar2.FechaVencimiento = Convert.ToDateTime(txtFechaVencimientoMonedaDolar2.Text);
                        fgDolar2.RangoCuotaCAC = "0";
                        fgDolar2.GastosAdtvo = chbGastosDolarPeso2.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                        fgDolar2.InteresAnual = Convert.ToDecimal(txtInteresAnualDolarPeso2.Text);
                        fgDolar2.Save();
                    }
                    else
                    {
                        foreach (var item in lvFormaPagoDolarPeso2.Items)
                        {
                            cFormaPagoOV fg = new cFormaPagoOV();
                            fg.IdOperacionVenta = idOV.ToString();
                            fg.Papelera = (Int16)papelera.Activo;

                            fg.Moneda = Convert.ToString((Int16)tipoMoneda.Dolar);
                            TextBox _monto = item.FindControl("lbTotal") as TextBox;
                            fg.Monto = Convert.ToDecimal(_monto.Text);
                            fg.Saldo = Convert.ToDecimal(_monto.Text);
                            Label _cantCuotas = item.FindControl("lbCantCuotas") as Label;
                            fg.CantCuotas = Convert.ToInt16(_cantCuotas.Text);
                            Label _valorCuota = item.FindControl("lbMontoCuota") as Label;
                            fg.Valor = Convert.ToDecimal(_monto.Text);
                            TextBox _fecha = item.FindControl("lbFechaVenc") as TextBox;
                            fg.FechaVencimiento = Convert.ToDateTime(_fecha.Text);
                            fg.RangoCuotaCAC = "0";
                            CheckBox _gastosAdtvo = item.FindControl("chbGastosAdtvo") as CheckBox;
                            if (_gastosAdtvo.Checked)
                                fg.GastosAdtvo = Convert.ToString((Int16)eGastosAdtvo.Si);
                            else
                                fg.GastosAdtvo = Convert.ToString((Int16)eGastosAdtvo.No);
                            fg.InteresAnual = Convert.ToDecimal(txtInteresAnualDolarPeso2.Text);
                            fg.Save();
                        }
                    }
                }
                #endregion

                #region Peso
                if (!chbCuotasManualesDolarPesoPeso.Checked)
                {
                    cFormaPagoOV fgPeso = new cFormaPagoOV();
                    fgPeso.IdOperacionVenta = idOV.ToString();
                    fgPeso.Papelera = (Int16)papelera.Activo;

                    fgPeso.Moneda = Convert.ToString((Int16)tipoMoneda.Pesos);
                    fgPeso.Monto = Convert.ToDecimal(txtMontoMonedaPeso.Text);
                    fgPeso.Saldo = Convert.ToDecimal(txtMontoMonedaPeso.Text);
                    fgPeso.CantCuotas = Convert.ToInt16(txtCuotasMonedaPeso.Text);
                    fgPeso.Valor = Convert.ToDecimal(lbValorMonedaPeso.Text);
                    fgPeso.FechaVencimiento = Convert.ToDateTime(txtFechaVencimientoMonedaPeso.Text);
                    fgPeso.RangoCuotaCAC = txtRangoDesdePeso.Text + " - " + txtRangoAPeso.Text;
                    fgPeso.GastosAdtvo = chbGastosDolarPesoPeso.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                    fgPeso.InteresAnual = Convert.ToDecimal(txtInteresAnualPesoPeso.Text);
                    fgPeso.Save();
                }
                else
                {
                    foreach (var item in lvFormaPagoDolarPesoPeso.Items)
                    {
                        cFormaPagoOV fg = new cFormaPagoOV();
                        fg.IdOperacionVenta = idOV.ToString();
                        fg.Papelera = (Int16)papelera.Activo;

                        fg.Moneda = Convert.ToString((Int16)tipoMoneda.Pesos);
                        TextBox _monto = item.FindControl("lbTotal") as TextBox;
                        fg.Monto = Convert.ToDecimal(_monto.Text);
                        fg.Saldo = Convert.ToDecimal(_monto.Text);
                        Label _cantCuotas = item.FindControl("lbCantCuotas") as Label;
                        fg.CantCuotas = Convert.ToInt16(_cantCuotas.Text);
                        Label _valorCuota = item.FindControl("lbMontoCuota") as Label;
                        fg.Valor = Convert.ToDecimal(_monto.Text);
                        TextBox _fecha = item.FindControl("lbFechaVenc") as TextBox;
                        fg.FechaVencimiento = Convert.ToDateTime(_fecha.Text);
                        CheckBox _ajusteCAC = item.FindControl("chbAjusteCAC") as CheckBox;
                        if (_ajusteCAC.Checked)
                            fg.RangoCuotaCAC = "-1"; //true
                        else
                            fg.RangoCuotaCAC = "0";
                        fg.GastosAdtvo = chbGastosDolarPesoPeso.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                        fg.InteresAnual = Convert.ToDecimal(txtInteresAnualPesoPeso.Text);
                        fg.Save();
                    }
                }

                if (pnlDolarPeso3.Visible == true)
                {
                    if (!chbCuotasManualesDolarPesoPeso3.Checked)
                    {
                        cFormaPagoOV fgPeso2 = new cFormaPagoOV();
                        fgPeso2.IdOperacionVenta = idOV.ToString();
                        fgPeso2.Papelera = (Int16)papelera.Activo;

                        fgPeso2.Moneda = Convert.ToString((Int16)tipoMoneda.Pesos);
                        fgPeso2.Monto = Convert.ToDecimal(txtMontoMonedaPeso3.Text);
                        fgPeso2.Saldo = Convert.ToDecimal(txtMontoMonedaPeso3.Text);
                        fgPeso2.CantCuotas = Convert.ToInt16(txtCuotasMonedaPeso3.Text);
                        fgPeso2.Valor = Convert.ToDecimal(lbValorMonedaPeso3.Text);
                        fgPeso2.FechaVencimiento = Convert.ToDateTime(txtFechaVencimientoMonedaPeso3.Text);
                        fgPeso2.RangoCuotaCAC = txtRangoDesdePeso3.Text + " - " + txtRangoAPeso3.Text;
                        fgPeso2.GastosAdtvo = chbGastosManualesDolarPesoPeso3.Checked ? Convert.ToString((Int16)eGastosAdtvo.Si) : Convert.ToString((Int16)eGastosAdtvo.No);
                        fgPeso2.InteresAnual = Convert.ToDecimal(txtInteresAnualPesoPeso3.Text);
                        fgPeso2.Save();
                    }
                    else
                    {
                        foreach (var item in lvFormaPagoDolarPesoPeso2.Items)
                        {
                            cFormaPagoOV fg = new cFormaPagoOV();
                            fg.IdOperacionVenta = idOV.ToString();
                            fg.Papelera = (Int16)papelera.Activo;

                            fg.Moneda = Convert.ToString((Int16)tipoMoneda.Pesos);
                            TextBox _monto = item.FindControl("lbTotal") as TextBox;
                            fg.Monto = Convert.ToDecimal(_monto.Text);
                            fg.Saldo = Convert.ToDecimal(_monto.Text);
                            Label _cantCuotas = item.FindControl("lbCantCuotas") as Label;
                            fg.CantCuotas = Convert.ToInt16(_cantCuotas.Text);
                            Label _valorCuota = item.FindControl("lbMontoCuota") as Label;
                            fg.Valor = Convert.ToDecimal(_monto.Text);
                            TextBox _fecha = item.FindControl("lbFechaVenc") as TextBox;
                            fg.FechaVencimiento = Convert.ToDateTime(_fecha.Text);
                            CheckBox _ajusteCAC = item.FindControl("chbAjusteCAC") as CheckBox;
                            if (_ajusteCAC.Checked)
                                fg.RangoCuotaCAC = "-1";//true
                            else
                                fg.RangoCuotaCAC = "0";
                            fg.InteresAnual = Convert.ToDecimal(txtInteresAnualPesoPeso3.Text);
                            fg.Save();
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region Al contado
            if (pnlMonedaAcordada.Visible == false && pnlDolarPeso.Visible == false)
            {
                cFormaPagoOV fgAnticipo = new cFormaPagoOV();
                fgAnticipo.IdOperacionVenta = idOV.ToString();
                fgAnticipo.Papelera = (Int16)papelera.Activo;

                fgAnticipo.Moneda = Convert.ToString((Int16)tipoMoneda.Dolar);
                fgAnticipo.Monto = Convert.ToDecimal(lbPrecioAcordado.Text);
                fgAnticipo.Saldo = Convert.ToDecimal(lbPrecioAcordado.Text);
                fgAnticipo.CantCuotas = 1;
                fgAnticipo.Valor = Convert.ToDecimal(lbPrecioAcordado.Text);
                fgAnticipo.FechaVencimiento = Convert.ToDateTime(DateTime.Now);
                fgAnticipo.RangoCuotaCAC = "-1";
                fgAnticipo.GastosAdtvo = Convert.ToString((Int16)eGastosAdtvo.Si);
                fgAnticipo.InteresAnual = 0;
                fgAnticipo.Save();
            }
            #endregion

            #endregion

            Response.Redirect("ListaOperacionVenta.aspx");
        }
        else
        {
            pnlMensaje.Visible = true;
        }
    }
    #endregion

    #region Form de pago - Moneda Acordada
    protected void txtMontoMonedaAcordada_TextChanged(object sender, EventArgs e)
    {
        try
        {
            decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);
            decimal monto = Convert.ToDecimal(txtMontoMonedaAcordada.Text);
            decimal valorCuota = CalcularValorCuota(txtMontoMonedaAcordada.Text, txtCuotasMonedaAcordada.Text);
            decimal montoViejo = 0;
            if (!string.IsNullOrEmpty(hfMontoMonedaAcordada.Value))
                montoViejo = Convert.ToDecimal(hfMontoMonedaAcordada.Value);

            if (lbValorMonedaAcordada.Text == "0")
            {
                lbValorMonedaAcordada.Text = String.Format("{0:#,#.00}", valorCuota);
                if (txtCuotasMonedaAcordada.Text == "0")
                    monto = 0;
                decimal nuevoTotal = Convert.ToDecimal(lbTotalMonedaAcordada.Text) + monto;
                lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", nuevoTotal);
                hfTotalAux.Value = nuevoTotal.ToString();
                hfMontoMonedaAcordada.Value = monto.ToString();
            }
            else
            {
                lbValorMonedaAcordada.Text = String.Format("{0:#,#.00}", valorCuota);
                if (txtCuotasMonedaAcordada.Text != "0")
                    CalcularValorTotal(monto, montoViejo, totalAux, false);
                else
                    CalcularValorTotal(0, montoViejo, totalAux, false);
                lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                hfMontoMonedaAcordada.Value = monto.ToString();
            }

            if (lbConvertirMonedaAcordada.Text != "0" && lbConvertirMonedaAcordada.Text != ",00")
            {
                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
                {
                    if (chbConvertirMonedaAcordada.Checked)
                    {
                        if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                        {
                            lbConvertirMonedaAcordada.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaAcordada.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                        }
                    }
                    else
                    {
                        lbConvertirMonedaAcordada.Text = cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaAcordada.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                    }
                }

                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                {
                    if (chbConvertirMonedaAcordada.Checked)
                    {
                        if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                        {
                            lbConvertirMonedaAcordada.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaAcordada.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                        }
                    }
                    else
                    {
                        lbConvertirMonedaAcordada.Text = cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaAcordada.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - txtMontoMonedaAcordada_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void txtCuotasMonedaAcordada_TextChanged(object sender, EventArgs e)
    {
        try
        {
            decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);
            decimal monto = Convert.ToDecimal(txtMontoMonedaAcordada.Text);
            decimal valorCuota = CalcularValorCuota(txtMontoMonedaAcordada.Text, txtCuotasMonedaAcordada.Text);
            decimal montoViejo = 0;
            if (!string.IsNullOrEmpty(hfMontoMonedaAcordada.Value))
                montoViejo = Convert.ToDecimal(hfMontoMonedaAcordada.Value);

            if (lbValorMonedaAcordada.Text == "0" || lbValorMonedaAcordada.Text == ",00")
            {
                lbValorMonedaAcordada.Text = String.Format("{0:#,#.00}", valorCuota);
                decimal nuevoTotal = Convert.ToDecimal(lbTotalMonedaAcordada.Text) + monto;
                lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", nuevoTotal);
                hfTotalAux.Value = nuevoTotal.ToString();
                hfMontoMonedaAcordada.Value = monto.ToString();
            }
            else
            {
                lbValorMonedaAcordada.Text = String.Format("{0:#,#.00}", valorCuota);
                if (txtCuotasMonedaAcordada.Text == "0")
                    CalcularValorTotal(0, montoViejo, totalAux, false);
                else
                    CalcularValorTotal(monto, montoViejo, totalAux, false);
                lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", totalAux);
                hfMontoMonedaAcordada.Value = monto.ToString();
            }

            txtRangoAAcordado.Text = txtCuotasMonedaAcordada.Text;
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - txtCuotasMonedaAcordada_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void chbConvertirMonedaAcordada_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
            {
                if (chbConvertirMonedaAcordada.Checked)
                {
                    if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                    {
                        lbConvertirMonedaAcordada.Text = txtMontoMonedaAcordada.Text;

                        decimal _precio = Math.Round(cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaAcordada.Text), Convert.ToDecimal(txtDolar.Text)), 2);
                        txtMontoMonedaAcordada.Text = String.Format("{0:#,#.00}", _precio);

                        decimal valorCuota = CalcularValorCuota(txtMontoMonedaAcordada.Text, txtCuotasMonedaAcordada.Text);
                        decimal monto = Convert.ToDecimal(txtMontoMonedaAcordada.Text);
                        decimal montoViejo = Convert.ToDecimal(hfMontoMonedaAcordada.Value);
                        decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                        lbValorMonedaAcordada.Text = String.Format("{0:#,#.00}", valorCuota);
                        CalcularValorTotal(monto, montoViejo, totalAux, false);

                        if (txtCuotasMonedaAcordada.Text != "0")
                            lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);

                        hfMontoMonedaAcordada.Value = monto.ToString();
                    }
                }
                else
                {
                    lbConvertirMonedaAcordada.Text = "0";
                    txtMontoMonedaAcordada.Text = cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaAcordada.Text), Convert.ToDecimal(txtDolar.Text)).ToString();

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaAcordada.Text, txtCuotasMonedaAcordada.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaAcordada.Text);
                    decimal montoViejo = Convert.ToDecimal(hfMontoMonedaAcordada.Value);
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaAcordada.Text = String.Format("{0:#,#.00}", valorCuota);
                    CalcularValorTotal(monto, montoViejo, totalAux, false);

                    if (txtCuotasMonedaAcordada.Text != "0")
                        lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);

                    hfMontoMonedaAcordada.Value = monto.ToString();
                }
            }

            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
            {
                if (chbConvertirMonedaAcordada.Checked)
                {
                    if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                    {
                        lbConvertirMonedaAcordada.Text = txtMontoMonedaAcordada.Text;
                        txtMontoMonedaAcordada.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaAcordada.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                    }
                }
                else
                {
                    lbConvertirMonedaAcordada.Text = "0";
                    txtMontoMonedaAcordada.Text = cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaAcordada.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - chbConvertirMonedaAcordada_CheckedChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    /*Cuota 2*/
    protected void txtMontoMonedaAcordada2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);
            decimal monto = Convert.ToDecimal(txtMontoMonedaAcordada2.Text);
            decimal valorCuota = CalcularValorCuota(txtMontoMonedaAcordada2.Text, txtCuotasMonedaAcordada2.Text);
            decimal montoViejo = 0;
            if (!string.IsNullOrEmpty(hfMontoMonedaAcordada2.Value))
                montoViejo = Convert.ToDecimal(hfMontoMonedaAcordada2.Value);

            if (lbValorMonedaAcordada2.Text == "0")
            {
                lbValorMonedaAcordada2.Text = String.Format("{0:#,#.00}", valorCuota);
                if (txtCuotasMonedaAcordada2.Text == "0")
                    monto = 0;
                decimal nuevoTotal = Convert.ToDecimal(lbTotalMonedaAcordada.Text) + monto;
                lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", nuevoTotal);
                hfTotalAux.Value = nuevoTotal.ToString();
                hfMontoMonedaAcordada2.Value = monto.ToString();
            }
            else
            {
                lbValorMonedaAcordada2.Text = String.Format("{0:#,#.00}", valorCuota);
                if (txtCuotasMonedaAcordada2.Text != "0")
                    CalcularValorTotal(monto, montoViejo, totalAux, false);
                else
                    CalcularValorTotal(0, montoViejo, totalAux, false);
                lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                hfMontoMonedaAcordada2.Value = monto.ToString();
            }

            if (lbConvertirMonedaAcordada2.Text != "0" && lbConvertirMonedaAcordada2.Text != ",00")
            {
                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
                {
                    if (chbConvertirMonedaAcordada2.Checked)
                    {
                        if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                        {
                            lbConvertirMonedaAcordada2.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaAcordada2.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                        }
                    }
                    else
                    {
                        lbConvertirMonedaAcordada2.Text = cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaAcordada2.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                    }
                }

                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                {
                    if (chbConvertirMonedaAcordada2.Checked)
                    {
                        if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                        {
                            lbConvertirMonedaAcordada2.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaAcordada2.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                        }
                    }
                    else
                    {
                        lbConvertirMonedaAcordada2.Text = cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaAcordada2.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - txtMontoMonedaAcordada2_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void txtCuotasMonedaAcordada2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);
            decimal monto = Convert.ToDecimal(txtMontoMonedaAcordada2.Text);
            decimal valorCuota = CalcularValorCuota(txtMontoMonedaAcordada2.Text, txtCuotasMonedaAcordada2.Text);
            decimal montoViejo = 0;
            if (!string.IsNullOrEmpty(hfMontoMonedaAcordada2.Value))
                montoViejo = Convert.ToDecimal(hfMontoMonedaAcordada2.Value);

            if (lbValorMonedaAcordada2.Text == ",00")
            {
                lbValorMonedaAcordada2.Text = String.Format("{0:#,#.00}", valorCuota);
                decimal nuevoTotal = Convert.ToDecimal(lbTotalMonedaAcordada.Text) + monto;
                if (txtCuotasMonedaAcordada2.Text != "0")
                    lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", nuevoTotal);
                hfTotalAux.Value = nuevoTotal.ToString();
                hfMontoMonedaAcordada2.Value = monto.ToString();
            }
            else
            {
                lbValorMonedaAcordada2.Text = String.Format("{0:#,#.00}", valorCuota);
                //CalcularValorTotal(monto, montoViejo, totalAux, false);
                if (txtCuotasMonedaAcordada2.Text == "0")
                    CalcularValorTotal(0, montoViejo, totalAux, false);
                else
                    CalcularValorTotal(monto, montoViejo, totalAux, false);
                lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", totalAux);
                hfMontoMonedaAcordada2.Value = monto.ToString();
            }

            txtRangoAAcordado2.Text = txtCuotasMonedaAcordada2.Text;
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - txtCuotasMonedaAcordada2_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void chbConvertirMonedaAcordada2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
            {
                if (chbConvertirMonedaAcordada2.Checked)
                {
                    if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                    {
                        lbConvertirMonedaAcordada2.Text = txtMontoMonedaAcordada2.Text;

                        decimal _precio = Math.Round(cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaAcordada2.Text), Convert.ToDecimal(txtDolar.Text)), 2);
                        txtMontoMonedaAcordada2.Text = String.Format("{0:#,#.00}", _precio);

                        decimal valorCuota = CalcularValorCuota(txtMontoMonedaAcordada2.Text, txtCuotasMonedaAcordada2.Text);
                        decimal monto = Convert.ToDecimal(txtMontoMonedaAcordada2.Text);
                        decimal montoViejo = Convert.ToDecimal(hfMontoMonedaAcordada2.Value);
                        decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                        lbValorMonedaAcordada2.Text = String.Format("{0:#,#.00}", valorCuota);
                        CalcularValorTotal(monto, montoViejo, totalAux, false);

                        if (txtCuotasMonedaAcordada2.Text != "0")
                            lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                        hfMontoMonedaAcordada2.Value = monto.ToString();
                    }
                }
                else
                {
                    lbConvertirMonedaAcordada2.Text = "0";
                    txtMontoMonedaAcordada2.Text = cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaAcordada2.Text), Convert.ToDecimal(txtDolar.Text)).ToString();

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaAcordada2.Text, txtCuotasMonedaAcordada2.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaAcordada2.Text);
                    decimal montoViejo = Convert.ToDecimal(hfMontoMonedaAcordada2.Value);
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaAcordada2.Text = String.Format("{0:#,#.00}", valorCuota);
                    CalcularValorTotal(monto, montoViejo, totalAux, false);

                    if (txtCuotasMonedaAcordada2.Text != "0")
                        lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                    hfMontoMonedaAcordada2.Value = monto.ToString();
                }

                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                {
                    if (chbConvertirMonedaAcordada2.Checked)
                    {
                        if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                        {
                            lbConvertirMonedaAcordada2.Text = txtMontoMonedaAcordada2.Text;
                            txtMontoMonedaAcordada2.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaAcordada2.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                        }
                    }
                    else
                    {
                        lbConvertirMonedaAcordada2.Text = "0";
                        txtMontoMonedaAcordada2.Text = cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaAcordada2.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - chbConvertirMonedaAcordada2_CheckedChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

    #region Form de pago - Dólar / Peso
    #region Condición de pago Dolar 1
    protected void txtMontoMonedaDolar_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtCuotasMonedaDolar.Text))
            {
                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaDolar.Text);
                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar.Text, txtCuotasMonedaDolar.Text);
                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaDolar.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaDolar.Value);

                    decimal aPeso = 0;
                    if (txtCuotasMonedaDolar.Text != "0")
                        aPeso = cValorDolar.ConvertToPeso(Convert.ToDecimal(monto), Convert.ToDecimal(txtDolar.Text));

                    decimal aPesoValorViejo = cValorDolar.ConvertToPeso(Convert.ToDecimal(montoViejo), Convert.ToDecimal(txtDolar.Text));

                    if (lbValorMonedaDolar.Text == ",00")
                    {
                        lbValorMonedaDolar.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + monto;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaDolar.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaDolar.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar.Text != "0")
                            CalcularValorTotal(aPeso, aPesoValorViejo, total, false);
                        else
                            CalcularValorTotal(0, aPesoValorViejo, total, false);
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                        hfMontoMonedaDolar.Value = monto.ToString();
                    }
                }
                else
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = 0;
                    if (txtCuotasMonedaDolar.Text != "0")
                        monto = Convert.ToDecimal(txtMontoMonedaDolar.Text);

                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaDolar.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaDolar.Value);

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar.Text, txtCuotasMonedaDolar.Text);

                    if (lbValorMonedaDolar.Text == ",00")
                    {
                        lbValorMonedaDolar.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + monto;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaDolar.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaDolar.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar.Text != "0")
                            CalcularValorTotal(monto, montoViejo, total, false);
                        else
                            CalcularValorTotal(0, montoViejo, total, false);
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                        hfMontoMonedaDolar.Value = monto.ToString();
                    }

                    if (lbConvertirDolarPeso.Text != "0" && lbConvertirDolarPeso.Text != ",00")
                    {
                        if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
                        {
                            if (chbConvertirDolarPeso.Checked)
                            {
                                if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                                {
                                    lbConvertirDolarPeso.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaDolar.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                                }
                            }
                            else
                            {
                                lbConvertirDolarPeso.Text = cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaDolar.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                            }
                        }

                        if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                        {
                            if (chbConvertirDolarPeso.Checked)
                            {
                                if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                                {
                                    lbConvertirDolarPeso.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaDolar.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                                }
                            }
                            else
                            {
                                lbConvertirDolarPeso.Text = cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaDolar.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - txtMontoMonedaDolar_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    protected void txtCuotasMonedaDolar_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtCuotasMonedaDolar.Text))
            {
                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaDolar.Text);
                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaDolar.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaDolar.Value);

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar.Text, txtCuotasMonedaDolar.Text);
                    decimal aPeso = 0;
                    if (txtCuotasMonedaDolar.Text != "0")
                        aPeso = cValorDolar.ConvertToPeso(Convert.ToDecimal(monto), Convert.ToDecimal(txtDolar.Text));

                    decimal aPesoValorViejo = cValorDolar.ConvertToPeso(Convert.ToDecimal(montoViejo), Convert.ToDecimal(txtDolar.Text));

                    if (lbValorMonedaDolar.Text == ",00")
                    {
                        lbValorMonedaDolar.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + aPeso;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaDolar.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaDolar.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar.Text != "0")
                            CalcularValorTotal(aPeso, aPesoValorViejo, total, false);
                        else
                            CalcularValorTotal(0, aPesoValorViejo, total, false);
                        hfMontoMonedaDolar.Value = monto.ToString();
                    }
                }
                else
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = 0;
                    if (txtCuotasMonedaDolar.Text != "0")
                        monto = Convert.ToDecimal(txtMontoMonedaDolar.Text);

                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaDolar.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaDolar.Value);

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar.Text, txtCuotasMonedaDolar.Text);

                    if (lbValorMonedaDolar.Text == ",00")
                    {
                        lbValorMonedaDolar.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + monto;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaDolar.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaDolar.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar.Text != "0")
                            CalcularValorTotal(monto, montoViejo, total, false);
                        else
                            CalcularValorTotal(0, montoViejo, total, false);
                        hfMontoMonedaDolar.Value = monto.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - txtCuotasMonedaDolar_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    protected void chbConvertirDolarPeso_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
            {
                if (chbConvertirDolarPeso.Checked)
                {
                    if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                    {
                        lbConvertirDolarPeso.Text = txtMontoMonedaDolar.Text;
                        txtMontoMonedaDolar.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaDolar.Text), Convert.ToDecimal(txtDolar.Text)));

                        decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar.Text, txtCuotasMonedaDolar.Text);
                        decimal monto = Convert.ToDecimal(txtMontoMonedaDolar.Text);
                        decimal montoViejo = Convert.ToDecimal(hfMontoMonedaDolar.Value);
                        decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                        lbValorMonedaDolar.Text = String.Format("{0:#,#.00}", valorCuota);
                        CalcularValorTotal(monto, montoViejo, totalAux, false);

                        if (txtCuotasMonedaDolar.Text != "0")
                            lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);

                        hfMontoMonedaDolar.Value = monto.ToString();
                    }
                }
                else
                {
                    lbConvertirDolarPeso.Text = "0";
                    txtMontoMonedaDolar.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaDolar.Text), Convert.ToDecimal(txtDolar.Text)).ToString());

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar.Text, txtCuotasMonedaDolar.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaDolar.Text);
                    decimal montoViejo = Convert.ToDecimal(hfMontoMonedaDolar.Value);
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaDolar.Text = String.Format("{0:#,#.00}", valorCuota);
                    CalcularValorTotal(monto, montoViejo, totalAux, false);

                    if (txtCuotasMonedaDolar.Text != "0")
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);

                    hfMontoMonedaDolar.Value = monto.ToString();
                }
            }

            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
            {
                if (chbConvertirDolarPeso.Checked)
                {
                    lbConvertirDolarPeso.Text = txtMontoMonedaDolar.Text;
                    txtMontoMonedaDolar.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaDolar.Text), Convert.ToDecimal(txtDolar.Text)).ToString());

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar.Text, txtCuotasMonedaDolar.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaDolar.Text);
                    decimal montoViejo = Convert.ToDecimal(hfMontoMonedaDolar.Value);
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaDolar.Text = String.Format("{0:#,#.00}", valorCuota);
                    if (txtCuotasMonedaPeso.Text != "0")
                    {
                        if (txtMontoMonedaPeso.Text == "0")
                            CalcularValorTotal(monto, montoViejo, totalAux, false);
                        else
                            CalcularValorTotal(cValorDolar.ConvertToPeso(Convert.ToDecimal(monto), Convert.ToDecimal(txtDolar.Text)), montoViejo, totalAux, false);
                        hfMontoMonedaDolar.Value = monto.ToString();
                    }

                    if (txtCuotasMonedaDolar.Text != "0")
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                }
                else
                {
                    lbConvertirDolarPeso.Text = "0";
                    txtMontoMonedaDolar.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaDolar.Text), Convert.ToDecimal(txtDolar.Text)).ToString());

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar.Text, txtCuotasMonedaDolar.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaDolar.Text);
                    decimal montoViejo = Convert.ToDecimal(hfMontoMonedaDolar.Value);
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaDolar.Text = String.Format("{0:#,#.00}", valorCuota);
                    if (txtCuotasMonedaPeso.Text != "0")
                    {
                        CalcularValorTotal(monto, montoViejo, totalAux, false);
                        hfMontoMonedaDolar.Value = monto.ToString();
                    }

                    if (txtCuotasMonedaDolar.Text != "0")
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - chbConvertirDolarPeso_CheckedChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

    #region Condición de pago Dolar 2
    protected void txtMontoMonedaDolar2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtCuotasMonedaDolar2.Text))
            {
                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaDolar2.Text);
                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar2.Text, txtCuotasMonedaDolar2.Text);
                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaDolar2.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaDolar2.Value);

                    decimal aPeso = 0;
                    if (txtCuotasMonedaDolar2.Text != "0")
                        aPeso = cValorDolar.ConvertToPeso(Convert.ToDecimal(monto), Convert.ToDecimal(txtDolar.Text));
                    decimal aPesoValorViejo = cValorDolar.ConvertToPeso(Convert.ToDecimal(montoViejo), Convert.ToDecimal(txtDolar.Text));

                    if (lbValorMonedaDolar2.Text == ",00")
                    {
                        lbValorMonedaDolar2.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar2.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + monto;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaDolar2.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaDolar2.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar2.Text != "0")
                            CalcularValorTotal(aPeso, montoViejo, total, false);
                        else
                            CalcularValorTotal(0, montoViejo, total, false);
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                        hfMontoMonedaDolar2.Value = monto.ToString();
                    }
                }
                else
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = 0;
                    if (txtCuotasMonedaDolar2.Text != "0")
                        monto = Convert.ToDecimal(txtMontoMonedaDolar2.Text);
                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaDolar2.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaDolar2.Value);

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar2.Text, txtCuotasMonedaDolar2.Text);

                    if (lbValorMonedaDolar2.Text == ",00")
                    {
                        lbValorMonedaDolar2.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar2.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + monto;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaDolar2.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaDolar2.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar2.Text != "0")
                            CalcularValorTotal(monto, montoViejo, total, false);
                        else
                            CalcularValorTotal(0, montoViejo, total, false);
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                        hfMontoMonedaDolar2.Value = monto.ToString();
                    }

                    if (lbConvertirDolarPeso2.Text != "0" && lbConvertirDolarPeso2.Text != ",00")
                    {
                        if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
                        {
                            if (chbConvertirDolarPeso2.Checked)
                            {
                                if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                                {
                                    lbConvertirDolarPeso2.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaDolar2.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                                }
                            }
                            else
                            {
                                lbConvertirDolarPeso2.Text = cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaDolar2.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                            }
                        }

                        if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                        {
                            if (chbConvertirDolarPeso2.Checked)
                            {
                                if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                                {
                                    lbConvertirDolarPeso2.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaDolar2.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                                }
                            }
                            else
                            {
                                lbConvertirDolarPeso2.Text = cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaDolar2.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - txtMontoMonedaDolar2_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    protected void txtCuotasMonedaDolar2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtCuotasMonedaDolar2.Text))
            {
                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaDolar2.Text);
                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar2.Text, txtCuotasMonedaDolar2.Text);
                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaDolar2.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaDolar2.Value);

                    decimal aPeso = 0;
                    if (txtCuotasMonedaDolar2.Text != "0")
                        aPeso = cValorDolar.ConvertToPeso(Convert.ToDecimal(monto), Convert.ToDecimal(txtDolar.Text));

                    decimal aPesoValorViejo = cValorDolar.ConvertToPeso(Convert.ToDecimal(montoViejo), Convert.ToDecimal(txtDolar.Text));

                    if (lbValorMonedaDolar2.Text == ",00")
                    {
                        lbValorMonedaDolar2.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar2.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + aPeso;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaDolar2.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaDolar2.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar2.Text != "0")
                            CalcularValorTotal(aPeso, aPesoValorViejo, total, false);
                        else
                            CalcularValorTotal(0, aPesoValorViejo, total, false);
                        hfMontoMonedaDolar2.Value = monto.ToString();
                    }
                }
                else
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = 0;
                    if (txtCuotasMonedaDolar2.Text != "0")
                        monto = Convert.ToDecimal(txtMontoMonedaDolar2.Text);

                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaDolar2.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaDolar2.Value);

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar2.Text, txtCuotasMonedaDolar2.Text);

                    if (lbValorMonedaDolar2.Text == ",00")
                    {
                        lbValorMonedaDolar2.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar2.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + monto;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaDolar2.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaDolar2.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaDolar2.Text != "0")
                            CalcularValorTotal(monto, montoViejo, total, false);
                        else
                            CalcularValorTotal(0, montoViejo, total, false);
                        hfMontoMonedaDolar2.Value = monto.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - txtCuotasMonedaDolar2_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    protected void chbConvertirDolarPeso2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
            {
                if (chbConvertirDolarPeso2.Checked)
                {
                    if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                    {
                        lbConvertirDolarPeso2.Text = txtMontoMonedaDolar2.Text;
                        txtMontoMonedaDolar2.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaDolar2.Text), Convert.ToDecimal(txtDolar.Text)));

                        decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar2.Text, txtCuotasMonedaDolar2.Text);
                        decimal monto = Convert.ToDecimal(txtMontoMonedaDolar2.Text);
                        decimal montoViejo = Convert.ToDecimal(hfMontoMonedaDolar2.Value);
                        decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                        lbValorMonedaDolar2.Text = String.Format("{0:#,#.00}", valorCuota);
                        CalcularValorTotal(monto, montoViejo, totalAux, false);

                        if (txtCuotasMonedaDolar2.Text != "0")
                            lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);

                        hfMontoMonedaDolar2.Value = monto.ToString();
                    }
                }
                else
                {
                    lbConvertirDolarPeso2.Text = "0";
                    txtMontoMonedaDolar2.Text = cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaDolar2.Text), Convert.ToDecimal(txtDolar.Text)).ToString();

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar2.Text, txtCuotasMonedaDolar2.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaDolar2.Text);
                    decimal montoViejo = Convert.ToDecimal(hfMontoMonedaDolar2.Value);
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaDolar2.Text = String.Format("{0:#,#.00}", valorCuota);
                    CalcularValorTotal(monto, montoViejo, totalAux, false);

                    if (txtCuotasMonedaDolar2.Text != "0")
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);

                    hfMontoMonedaDolar2.Value = monto.ToString();
                }
            }

            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
            {
                if (chbConvertirDolarPeso2.Checked)
                {
                    lbConvertirDolarPeso2.Text = txtMontoMonedaDolar2.Text;
                    txtMontoMonedaDolar2.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaDolar2.Text), Convert.ToDecimal(txtDolar.Text)).ToString());

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar2.Text, txtCuotasMonedaDolar2.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaDolar2.Text);
                    decimal montoViejo = Convert.ToDecimal(hfMontoMonedaDolar2.Value);
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaDolar2.Text = String.Format("{0:#,#.00}", valorCuota);

                    if (txtCuotasMonedaDolar2.Text != "0")
                    {
                        if (txtMontoMonedaPeso.Text == "0")
                            CalcularValorTotal(monto, montoViejo, totalAux, false);
                        else
                            CalcularValorTotal(cValorDolar.ConvertToPeso(Convert.ToDecimal(monto), Convert.ToDecimal(txtDolar.Text)), montoViejo, totalAux, false);
                        hfMontoMonedaDolar2.Value = monto.ToString();
                    }
                    lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                }
                else
                {
                    lbConvertirDolarPeso2.Text = "0";
                    txtMontoMonedaDolar2.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaDolar2.Text), Convert.ToDecimal(txtDolar.Text)).ToString());

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaDolar2.Text, txtCuotasMonedaDolar2.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaDolar2.Text);
                    decimal montoViejo = Convert.ToDecimal(cValorDolar.ConvertToPeso(Convert.ToDecimal(hfMontoMonedaDolar2.Value), Convert.ToDecimal(txtDolar.Text)));
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaDolar2.Text = String.Format("{0:#,#.00}", valorCuota);

                    if (txtMontoMonedaDolar2.Text != "0")
                    {
                        CalcularValorTotal(monto, montoViejo, totalAux, false);
                        hfMontoMonedaDolar2.Value = monto.ToString();
                    }
                    if (txtCuotasMonedaDolar2.Text != "0")
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - chbConvertirDolarPeso2_CheckedChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

    #region Condición de pago Pesos 1
    protected void txtMontoMonedaPeso_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtCuotasMonedaPeso.Text))
            {
                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaPeso.Text);
                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso.Text, txtCuotasMonedaPeso.Text);
                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaPeso.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaPeso.Value);

                    decimal aDolar = 0;
                    if (txtCuotasMonedaPeso.Text != "0")
                        aDolar = cValorDolar.ConvertToDolar(Convert.ToDecimal(monto), Convert.ToDecimal(txtDolar.Text));

                    decimal aDolarValorViejo = cValorDolar.ConvertToDolar(Convert.ToDecimal(montoViejo), Convert.ToDecimal(txtDolar.Text));

                    if (lbValorMonedaPeso.Text == ",00")
                    {
                        lbValorMonedaPeso.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaPeso.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + aDolar;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaPeso.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaPeso.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaPeso.Text != "0")
                            CalcularValorTotal(monto, montoViejo, total, false);
                        else
                            CalcularValorTotal(0, montoViejo, total, false);
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                        hfMontoMonedaPeso.Value = monto.ToString();
                    }
                }
                else
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = 0;
                    if (txtCuotasMonedaPeso.Text != "0")
                        monto = Convert.ToDecimal(txtMontoMonedaPeso.Text);

                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaPeso.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaPeso.Value);

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso.Text, txtCuotasMonedaPeso.Text);

                    if (lbValorMonedaPeso.Text == ",00")
                    {
                        lbValorMonedaPeso.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaPeso.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + monto;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaPeso.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaPeso.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaPeso.Text != "0")
                            CalcularValorTotal(monto, montoViejo, total, false);
                        else
                            CalcularValorTotal(0, montoViejo, total, false);
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                        hfMontoMonedaPeso.Value = monto.ToString();
                    }

                    if (lbConvertirPesoDolar.Text != ",00")
                    {
                        if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
                        {
                            if (chbConvertirPesoDolar.Checked)
                            {
                                if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                                {
                                    lbConvertirPesoDolar.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaPeso.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                                }
                            }
                            else
                            {
                                lbConvertirPesoDolar.Text = cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaPeso.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                            }
                        }

                        if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                        {
                            if (chbConvertirPesoDolar.Checked)
                            {
                                if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                                {
                                    lbConvertirPesoDolar.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaPeso.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                                }
                            }
                            else
                            {
                                lbConvertirPesoDolar.Text = cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaPeso.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                            }
                        }
                    }
                }
                txtRangoAPeso.Text = txtCuotasMonedaPeso.Text;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - txtMontoMonedaPeso_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    protected void txtCuotasMonedaPeso_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtCuotasMonedaPeso.Text))
            {
                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaPeso.Text);
                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso.Text, txtCuotasMonedaPeso.Text);
                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaPeso.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaPeso.Value);

                    decimal aDolar = 0;
                    if (txtCuotasMonedaPeso.Text != "0")
                        aDolar = cValorDolar.ConvertToDolar(Convert.ToDecimal(monto), Convert.ToDecimal(txtDolar.Text));
                    else
                        aDolar = Convert.ToDecimal(monto);

                    decimal aDolarValorViejo = cValorDolar.ConvertToDolar(Convert.ToDecimal(montoViejo), Convert.ToDecimal(txtDolar.Text));

                    if (lbValorMonedaPeso.Text == ",00")
                    {
                        lbValorMonedaPeso.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtMontoMonedaPeso.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + monto;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaPeso.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaPeso.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaPeso.Text != "0")
                            CalcularValorTotal(aDolar, aDolarValorViejo, total, false);
                        else
                            CalcularValorTotal(0, montoViejo, total, false);
                        hfMontoMonedaPeso.Value = monto.ToString();
                    }
                }
                else
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = 0;
                    if (txtCuotasMonedaPeso.Text != "0")
                        monto = Convert.ToDecimal(txtMontoMonedaPeso.Text);

                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaPeso.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaPeso.Value);

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso.Text, txtCuotasMonedaPeso.Text);

                    if (lbValorMonedaPeso.Text == "0" && lbValorMonedaPeso.Text == ",00")
                    {
                        lbValorMonedaPeso.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtMontoMonedaPeso.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + monto;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaPeso.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaPeso.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtMontoMonedaPeso.Text != "0")
                            CalcularValorTotal(monto, montoViejo, total, false);
                        else
                            CalcularValorTotal(0, montoViejo, total, false);
                        hfMontoMonedaPeso.Value = monto.ToString();
                    }
                }
                txtRangoAPeso.Text = txtCuotasMonedaPeso.Text;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - txtCuotasMonedaPeso_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    protected void chbConvertirPesoDolar_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
            {
                if (chbConvertirPesoDolar.Checked)
                {
                    if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                    {
                        lbConvertirPesoDolar.Text = txtMontoMonedaPeso.Text;
                        txtMontoMonedaPeso.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaPeso.Text), Convert.ToDecimal(txtDolar.Text)).ToString());

                        decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso.Text, txtCuotasMonedaPeso.Text);
                        decimal monto = Convert.ToDecimal(txtMontoMonedaPeso.Text);
                        decimal montoViejo = Convert.ToDecimal(hfMontoMonedaPeso.Value);
                        decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                        lbValorMonedaPeso.Text = String.Format("{0:#,#.00}", valorCuota);
                        CalcularValorTotal(monto, montoViejo, totalAux, false);

                        if (txtCuotasMonedaPeso.Text != "0")
                            lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);

                        hfMontoMonedaPeso.Value = monto.ToString();
                    }
                }
                else
                {
                    lbConvertirPesoDolar.Text = "0";
                    txtMontoMonedaPeso.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaPeso.Text), Convert.ToDecimal(txtDolar.Text)).ToString());

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso.Text, txtCuotasMonedaPeso.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaPeso.Text);
                    decimal montoViejo = Convert.ToDecimal(hfMontoMonedaPeso.Value);
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaPeso.Text = String.Format("{0:#,#.00}", valorCuota);
                    CalcularValorTotal(monto, montoViejo, totalAux, false);

                    if (txtCuotasMonedaPeso.Text != "0")
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);


                    hfMontoMonedaPeso.Value = monto.ToString();
                }
            }

            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
            {
                if (chbConvertirPesoDolar.Checked)
                {
                    lbConvertirPesoDolar.Text = txtMontoMonedaPeso.Text;
                    txtMontoMonedaPeso.Text = Math.Round(cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaPeso.Text), Convert.ToDecimal(txtDolar.Text)), 2).ToString();

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso.Text, txtCuotasMonedaPeso.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaPeso.Text);
                    decimal montoViejo = Convert.ToDecimal(hfMontoMonedaPeso.Value);
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaPeso.Text = String.Format("{0:#,#.00}", valorCuota);

                    if (txtCuotasMonedaPeso.Text != "0")
                    {
                        if (txtMontoMonedaPeso.Text == "0")
                            CalcularValorTotal(monto, montoViejo, totalAux, false);
                        else
                            CalcularValorTotal(cValorDolar.ConvertToDolar(Convert.ToDecimal(monto), Convert.ToDecimal(txtDolar.Text)), montoViejo, totalAux, false);
                        hfMontoMonedaPeso.Value = monto.ToString();
                    }
                    lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                }
                else
                {
                    lbConvertirPesoDolar.Text = "0";
                    //txtMontoMonedaPeso.Text = Math.Round(cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaPeso.Text), Convert.ToDecimal(txtDolar.Text)), 2).ToString();
                    txtMontoMonedaPeso.Text = Math.Round(cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaPeso.Text), Convert.ToDecimal(txtDolar.Text)), 2).ToString();

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso.Text, txtCuotasMonedaPeso.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaPeso.Text);
                    decimal montoViejo = Convert.ToDecimal(cValorDolar.ConvertToDolar(Convert.ToDecimal(hfMontoMonedaPeso.Value), Convert.ToDecimal(txtDolar.Text)));
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaPeso.Text = String.Format("{0:#,#.00}", valorCuota);

                    if (txtCuotasMonedaPeso.Text != "0")
                    {
                        CalcularValorTotal(monto, montoViejo, totalAux, false);
                        hfMontoMonedaPeso.Value = monto.ToString();
                    }
                    lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - chbConvertirPesoDolar_CheckedChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

    #region Condición de pago Pesos 2
    protected void txtMontoMonedaPeso3_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtCuotasMonedaPeso3.Text))
            {
                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaPeso3.Text);
                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso3.Text, txtCuotasMonedaPeso3.Text);
                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaPeso3.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaPeso3.Value);

                    decimal aDolar = 0;
                    if (txtCuotasMonedaPeso3.Text != "0")
                        aDolar = cValorDolar.ConvertToDolar(Convert.ToDecimal(monto), Convert.ToDecimal(txtDolar.Text));

                    decimal aDolarValorViejo = cValorDolar.ConvertToDolar(Convert.ToDecimal(montoViejo), Convert.ToDecimal(txtDolar.Text));

                    if (lbValorMonedaPeso3.Text == ",00")
                    {
                        lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaPeso3.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + aDolar;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", Convert.ToDecimal(nuevoTotal));
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaPeso3.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaPeso3.Text != "0")
                            CalcularValorTotal(aDolar, aDolarValorViejo, total, false);
                        else
                            CalcularValorTotal(0, aDolarValorViejo, total, false);
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", Convert.ToDecimal(hfTotalAux.Value));
                        hfMontoMonedaPeso3.Value = monto.ToString();
                    }
                }
                else
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = 0;
                    if (txtCuotasMonedaPeso3.Text != "0")
                        monto = Convert.ToDecimal(txtMontoMonedaPeso3.Text);

                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaPeso3.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaPeso3.Value);

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso3.Text, txtCuotasMonedaPeso3.Text);

                    if (lbValorMonedaPeso3.Text == ",00")
                    {
                        lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaPeso3.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + monto;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", Convert.ToDecimal(nuevoTotal));
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaPeso3.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaPeso3.Text != "0")
                            CalcularValorTotal(monto, montoViejo, total, false);
                        else
                            CalcularValorTotal(0, montoViejo, total, false);
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", Convert.ToDecimal(hfTotalAux.Value));
                        hfMontoMonedaPeso3.Value = monto.ToString();
                    }

                    if (lbConvertirPesoDolar2.Text != ",00")
                    {
                        if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
                        {
                            if (chbConvertirPesoDolar2.Checked)
                            {
                                if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                                {
                                    lbConvertirPesoDolar2.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaPeso3.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                                }
                            }
                            else
                            {
                                lbConvertirPesoDolar2.Text = cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaPeso3.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                            }
                        }

                        if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                        {
                            if (chbConvertirPesoDolar2.Checked)
                            {
                                if (lbMonedaUnidad.Text == tipoMoneda.Pesos.ToString())
                                {
                                    lbConvertirPesoDolar2.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaPeso3.Text), Convert.ToDecimal(txtDolar.Text)).ToString());
                                }
                            }
                            else
                            {
                                lbConvertirPesoDolar2.Text = cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaPeso3.Text), Convert.ToDecimal(txtDolar.Text)).ToString();
                            }
                        }
                    }
                }
                txtRangoAPeso3.Text = txtCuotasMonedaPeso3.Text;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - txtMontoMonedaPeso3_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    protected void txtCuotasMonedaPeso3_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtCuotasMonedaPeso3.Text))
            {
                if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaPeso3.Text);
                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso3.Text, txtCuotasMonedaPeso3.Text);
                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaPeso3.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaPeso3.Value);

                    decimal aDolar = 0;
                    if (txtCuotasMonedaPeso3.Text != "0")
                        aDolar = cValorDolar.ConvertToDolar(Convert.ToDecimal(monto), Convert.ToDecimal(txtDolar.Text));
                    else
                        aDolar = Convert.ToDecimal(monto);

                    decimal aDolarValorViejo = cValorDolar.ConvertToDolar(Convert.ToDecimal(montoViejo), Convert.ToDecimal(txtDolar.Text));

                    lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);

                    CalcularValorTotal(aDolar, aDolarValorViejo, total, false);
                    hfMontoMonedaPeso3.Value = txtMontoMonedaPeso3.Text;

                    if (lbValorMonedaPeso3.Text == ",00")
                    {
                        lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtMontoMonedaPeso3.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + monto;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaPeso3.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtCuotasMonedaPeso3.Text != "0")
                            CalcularValorTotal(aDolar, aDolarValorViejo, total, false);
                        else
                            CalcularValorTotal(0, aDolarValorViejo, total, false);
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", Convert.ToDecimal(hfTotalAux.Value));
                        hfMontoMonedaPeso3.Value = monto.ToString();
                    }
                }
                else
                {
                    decimal total = Convert.ToDecimal(hfTotalAux.Value);
                    decimal monto = 0;
                    if (txtCuotasMonedaPeso3.Text != "0")
                        monto = Convert.ToDecimal(txtMontoMonedaPeso3.Text);

                    decimal montoViejo = 0;
                    if (!string.IsNullOrEmpty(hfMontoMonedaPeso3.Value))
                        montoViejo = Convert.ToDecimal(hfMontoMonedaPeso3.Value);

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso3.Text, txtCuotasMonedaPeso3.Text);

                    if (lbValorMonedaPeso3.Text == "0" && lbValorMonedaPeso.Text == ",00")
                    {
                        lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtMontoMonedaPeso3.Text == "0")
                            monto = 0;
                        decimal nuevoTotal = Convert.ToDecimal(lbTotalDolarPeso.Text) + monto;
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
                        hfTotalAux.Value = nuevoTotal.ToString();
                        hfMontoMonedaPeso3.Value = monto.ToString();
                    }
                    else
                    {
                        lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);
                        if (txtMontoMonedaPeso3.Text != "0")
                            CalcularValorTotal(monto, montoViejo, total, false);
                        else
                            CalcularValorTotal(0, montoViejo, total, false);
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                        hfMontoMonedaPeso3.Value = monto.ToString();
                    }
                }

                txtRangoAPeso3.Text = txtCuotasMonedaPeso3.Text;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - txtCuotasMonedaPeso3_TextChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    protected void chbConvertirPesoDolar2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Pesos.ToString())
            {
                if (chbConvertirPesoDolar2.Checked)
                {
                    if (lbMonedaUnidad.Text == tipoMoneda.Dolar.ToString())
                    {
                        lbConvertirPesoDolar2.Text = txtMontoMonedaPeso3.Text;
                        txtMontoMonedaPeso3.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaPeso3.Text), Convert.ToDecimal(txtDolar.Text)).ToString());

                        decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso3.Text, txtCuotasMonedaPeso3.Text);
                        decimal monto = Convert.ToDecimal(txtMontoMonedaPeso3.Text);
                        decimal montoViejo = Convert.ToDecimal(hfMontoMonedaPeso3.Value);
                        decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                        lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);
                        CalcularValorTotal(monto, montoViejo, totalAux, false);

                        if (txtCuotasMonedaPeso3.Text != "0")
                            lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);

                        hfMontoMonedaPeso3.Value = monto.ToString();
                    }
                }
                else
                {
                    lbConvertirPesoDolar2.Text = "0";
                    txtMontoMonedaPeso3.Text = String.Format("{0:#,#.00}", cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaPeso3.Text), Convert.ToDecimal(txtDolar.Text)).ToString());

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso3.Text, txtCuotasMonedaPeso3.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaPeso3.Text);
                    decimal montoViejo = Convert.ToDecimal(hfMontoMonedaPeso3.Value);
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);
                    CalcularValorTotal(monto, montoViejo, totalAux, false);

                    if (txtCuotasMonedaPeso3.Text != "0")
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);

                    hfMontoMonedaPeso3.Value = monto.ToString();
                }
            }

            if (cbMonedaAcordada.SelectedValue == tipoMoneda.Dolar.ToString())
            {
                if (chbConvertirPesoDolar2.Checked)
                {
                    lbConvertirPesoDolar2.Text = txtMontoMonedaPeso3.Text;
                    txtMontoMonedaPeso3.Text = Math.Round(cValorDolar.ConvertToPeso(Convert.ToDecimal(txtMontoMonedaPeso3.Text), Convert.ToDecimal(txtDolar.Text)), 2).ToString();

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso3.Text, txtCuotasMonedaPeso3.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaPeso3.Text);
                    decimal montoViejo = Convert.ToDecimal(hfMontoMonedaPeso3.Value);
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);

                    if (txtCuotasMonedaPeso3.Text != "0")
                    {
                        if (txtMontoMonedaPeso.Text == "0")
                            CalcularValorTotal(monto, montoViejo, totalAux, false);
                        else
                            CalcularValorTotal(cValorDolar.ConvertToDolar(Convert.ToDecimal(monto), Convert.ToDecimal(txtDolar.Text)), montoViejo, totalAux, false);
                        hfMontoMonedaPeso3.Value = monto.ToString();
                    }

                    lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                }
                else
                {
                    lbConvertirPesoDolar2.Text = "0";
                    txtMontoMonedaPeso3.Text = Math.Round(cValorDolar.ConvertToDolar(Convert.ToDecimal(txtMontoMonedaPeso3.Text), Convert.ToDecimal(txtDolar.Text)), 2).ToString();

                    decimal valorCuota = CalcularValorCuota(txtMontoMonedaPeso3.Text, txtCuotasMonedaPeso3.Text);
                    decimal monto = Convert.ToDecimal(txtMontoMonedaPeso3.Text);
                    decimal montoViejo = Convert.ToDecimal(cValorDolar.ConvertToDolar(Convert.ToDecimal(hfMontoMonedaPeso3.Value), Convert.ToDecimal(txtDolar.Text)));
                    decimal totalAux = Convert.ToDecimal(hfTotalAux.Value);

                    lbValorMonedaPeso3.Text = String.Format("{0:#,#.00}", valorCuota);

                    if (txtCuotasMonedaPeso3.Text != "0")
                    {
                        CalcularValorTotal(monto, montoViejo, totalAux, false);
                        hfMontoMonedaPeso3.Value = monto.ToString();
                    }

                    if (txtCuotasMonedaPeso3.Text != "0")
                        lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", hfTotalAux.Value);
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - chbConvertirPesoDolar2_CheckedChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion
    #endregion

    #region Métodos Auxiliares
    public void LimpiarCampos()
    {
        cbProyectos.SelectedValue = "0";
        cbUnidadFuncional.SelectedValue = "0";
        cbUnidadFuncional.Enabled = false;
        cbNivel.SelectedValue = "0";
        cbNivel.Enabled = false;
        cbUnidad.SelectedValue = "0";
        cbUnidad.Enabled = false;
        lbPrecio.Text = "-";
        lbMoneda.Text = "";
        lbMoneda.Text.Replace("(", "").Replace(")", "");
        txtPrecioAcordado.Text = "";
    }

    public void CalcularTotal(decimal total)
    {
        if (cbMonedaAcordada.SelectedValue == Convert.ToString(tipoMoneda.Dolar) && cbOperacionMoneda.SelectedValue == Convert.ToString((Int16)tipoMoneda.Dolar))
            lbTotalMonedaAcordada.Text = String.Format("{0:#,#}", total);

        if (cbMonedaAcordada.SelectedValue == Convert.ToString(tipoMoneda.Dolar) && cbOperacionMoneda.SelectedValue == Convert.ToString((Int16)tipoMoneda.Pesos))
        {
            decimal _total = cValorDolar.ConvertToDolar(total);
            lbTotalMonedaAcordada.Text = String.Format("{0:#,#}", _total);
        }

        if (cbMonedaAcordada.SelectedValue == Convert.ToString(tipoMoneda.Pesos) && cbOperacionMoneda.SelectedValue == Convert.ToString((Int16)tipoMoneda.Pesos))
            lbTotalMonedaAcordada.Text = String.Format("{0:#,#}", total);

        if (cbMonedaAcordada.SelectedValue == Convert.ToString(tipoMoneda.Pesos) && cbOperacionMoneda.SelectedValue == Convert.ToString((Int16)tipoMoneda.Dolar))
        {
            decimal _total = cValorDolar.ConvertToPeso(total);
            lbTotalMonedaAcordada.Text = String.Format("{0:#,#}", _total);
        }
    }

    public void CalcularValorTotal(decimal monto, decimal montoViejo, decimal total, bool cuotaManual)
    {
        if (total == 0)
        {
            //lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", monto);
            //lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", monto);
            hfTotalAux.Value = monto.ToString();
        }
        else
        {
            decimal nuevoTotal = 0;
            if (cuotaManual == true)
                nuevoTotal = total + monto;
            else
                nuevoTotal = total - montoViejo + monto;

            //lbTotalMonedaAcordada.Text = String.Format("{0:#,#.00}", nuevoTotal);
            //lbTotalDolarPeso.Text = String.Format("{0:#,#.00}", nuevoTotal);
            hfTotalAux.Value = nuevoTotal.ToString();
        }
    }

    public decimal CalcularValorCuota(string anticipo, string cuotas)
    {
        if (anticipo != "0" && !string.IsNullOrEmpty(anticipo) && cuotas != "0" && !string.IsNullOrEmpty(cuotas))
            return Convert.ToDecimal(anticipo) / Convert.ToDecimal(cuotas);
        else
            return 0;
    }

    public string SumaAnticipo(string anticipoDolar, string anticipoPeso)
    {
        decimal valor = Convert.ToDecimal(anticipoDolar) + Convert.ToDecimal(anticipoPeso);
        return valor.ToString();
    }
    #endregion

    #region CheckBox
    protected void chbCuotas_CheckedChanged(object sender, EventArgs e)
    {
        if (chbCuotas.Checked)
            pnlCuota2.Visible = true;
        else
        {
            pnlCuota2.Visible = false;
            txtMontoMonedaAcordada2.Text = "";
            chbConvertirMonedaAcordada2.Checked = false;
            lbConvertirMonedaAcordada2.Text = ",00";
            txtCuotasMonedaAcordada2.Text = "";
            lbValorMonedaAcordada2.Text = ",00";
            txtFechaVencimientoMonedaAcordada2.Text = "";
            chbCuotasManuales2.Checked = false;
            chbGastosManuales2.Checked = true;
            txtInteresAnual2.Text = "0";
            txtRangoDesdeAcordado2.Text = "1";
            txtRangoAAcordado2.Text = "1";
            txtMontoMonedaAcordada_TextChanged(null, null);
        }
    }

    protected void chbDolarPesoDolar_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBox1.Checked)
            pnlDolarPeso2.Visible = true;
        else
            pnlDolarPeso2.Visible = false;
    }

    protected void chbDolarPesoPeso_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBox2.Checked)
            pnlDolarPeso3.Visible = true;
        else
            pnlDolarPeso3.Visible = false;
    }
    #endregion

    #region ListView
    protected void lvUnidades_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Eliminar":
                    {
                        List<cUnidad> listUnidades = new List<cUnidad>();
                        decimal totalLista = 0;
                        decimal totalAcordado = 0;

                        ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                        lvUnidades.Items.Remove(dataItem);

                        foreach (var item in lvUnidades.Items)
                        {
                            Label idUnidad = item.FindControl("lbId") as Label;
                            cUnidad u = cUnidad.Load(idUnidad.Text);
                            switch (cbMonedaAcordada.SelectedValue)
                            {
                                case "0":
                                    if (u.GetMoneda == tipoMoneda.Pesos.ToString())
                                    {
                                        u.PrecioBase = cValorDolar.ConvertToDolar(u.PrecioBase);
                                        u.Moneda = Convert.ToInt16(tipoMoneda.Dolar).ToString();
                                    }
                                    break;
                                case "1":
                                    if (u.GetMoneda == tipoMoneda.Dolar.ToString())
                                    {
                                        u.PrecioBase = cValorDolar.ConvertToPeso(u.PrecioBase);
                                        u.Moneda = Convert.ToInt16(tipoMoneda.Pesos).ToString();
                                    }
                                    break;
                            }
                            Label _precioAcordado = item.FindControl("lbPrecioAcordado") as Label;
                            u.PrecioAcordado = Convert.ToDecimal(_precioAcordado.Text);

                            totalAcordado += u.PrecioAcordado;
                            totalLista += u.PrecioBase;

                            listUnidades.Add(u);
                        }

                        cUnidad unidad = cUnidad.Load(e.CommandArgument.ToString());
                        switch (cbMonedaAcordada.SelectedValue)
                        {
                            case "0":
                                if (unidad.GetMoneda == tipoMoneda.Pesos.ToString())
                                {
                                    unidad.PrecioBase = cValorDolar.ConvertToDolar(unidad.PrecioBase);
                                    unidad.Moneda = Convert.ToInt16(tipoMoneda.Dolar).ToString();
                                }
                                break;
                            case "1":
                                if (unidad.GetMoneda == tipoMoneda.Dolar.ToString())
                                {
                                    unidad.PrecioBase = cValorDolar.ConvertToPeso(unidad.PrecioBase);
                                    unidad.Moneda = Convert.ToInt16(tipoMoneda.Pesos).ToString();
                                }
                                break;
                        }

                        if (!string.IsNullOrEmpty(txtPrecioAcordado.Text))
                            unidad.PrecioAcordado = Convert.ToDecimal(txtPrecioAcordado.Text);
                        else
                            unidad.PrecioAcordado = 0;

                        if (unidad.GetMoneda == tipoMoneda.Dolar.ToString())
                        {
                            lbPrecioLista.Text = String.Format("{0:#,#}", totalLista);
                            lbPrecioListaPesos.Text = String.Format("{0:#,#}", cValorDolar.ConvertToPeso(totalLista));
                        }
                        else
                        {
                            lbPrecioLista.Text = String.Format("{0:#,#}", cValorDolar.ConvertToDolar(totalLista));
                            lbPrecioListaPesos.Text = String.Format("{0:#,#}", totalLista);
                        }

                        lbPrecioAcordado.Text = String.Format("{0:#,#}", totalAcordado);
                        lbPrecioAcordadoTitle.Text = "Precio acordado (" + unidad.GetMoneda + "): ";

                        lvUnidades.DataSource = listUnidades;
                        lvUnidades.DataBind();

                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - lvUnidades_ItemCommand");
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

    #region Reserva
    protected void cbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            List<cReserva> reservas = cReserva.GetIdUnidadByIdEmpresa(cbEmpresa.SelectedValue);
            List<cUnidad> listUnidades = new List<cUnidad>();
            int cantidadReservas = reservas.Count;
            decimal reserva = 0;
            decimal precio = 0;
            string moneda = null;
            
            if(cantidadReservas != 0){
                pnlReserva.Visible = true;

                if (cantidadReservas == 1)
                {
                    cUnidad unidad = cUnidad.Load(reservas[0].IdUnidad);
                    cbProyectos.SelectedValue = unidad.IdProyecto;

                    lbImporteReserva.Text = unidad.GetImporteReserva;

                    cbUnidadFuncional.Enabled = true;
                    cbUnidadFuncional.DataSource = cUnidad.GroupByUnidadFuncional(cbProyectos.SelectedValue);
                    cbUnidadFuncional.DataBind();
                    ListItem uf = new ListItem("Seleccione un tipo de unidad funcional", "0");
                    cbUnidadFuncional.Items.Insert(0, uf);
                    cbUnidadFuncional.SelectedIndex = 0;
                    cbUnidadFuncional.SelectedValue = unidad.UnidadFuncional;

                    cbNivel.Enabled = true;
                    cbNivel.DataSource = cUnidad.GroupByNivel(cbProyectos.SelectedValue);
                    cbNivel.DataBind();
                    ListItem inivel = new ListItem("Seleccione un nivel...", "0");
                    cbNivel.Items.Insert(0, inivel);
                    cbNivel.SelectedIndex = 0;
                    cbNivel.SelectedValue = unidad.Nivel;

                    cbUnidad.Enabled = true;
                    cbUnidad.DataSource = cUnidad.GetNroUnidadReservadaByIdProyecto(cbProyectos.SelectedValue, cbNivel.SelectedItem.Text, cbEmpresa.SelectedValue);
                    cbUnidad.DataBind();
                    ListItem iunidad = new ListItem("Seleccione el nro. de unidad...", "0");
                    cbUnidad.Items.Insert(0, iunidad);
                    cbUnidad.SelectedIndex = 0;
                    cbUnidad.SelectedValue = unidad.NroUnidad;

                    lbPrecio.Text = unidad.GetPrecioBase;
                    lbMoneda.Text = "(" + unidad.GetMoneda + ")";
                    hfCodUfUnidad.Value = unidad.Id;
                }

                if (cantidadReservas > 1)
                {
                    foreach (cReserva item in reservas)
                    {
                        cUnidad unidad = cUnidad.Load(item.IdUnidad);
                        unidad.PrecioAcordado = unidad.PrecioBase;
                        unidad.Save();

                        reserva += item.Importe;
                        precio += unidad.PrecioBase;
                        moneda = unidad.GetMoneda;
                        listUnidades.Add(unidad);
                    }

                    lbMonedaUnidad.Text = moneda;

                    lbImporteReserva.Text = String.Format("{0:#,#}", reserva);
                    lbPrecio.Text = String.Format("{0:#,#}", precio);
                    lbMoneda.Text = "(" + moneda + ")";

                    lbPrecioLista.Text = String.Format("{0:#,#}", precio);
                    lbPrecioListaPesos.Text = String.Format("{0:#,#}", cValorDolar.ConvertToPeso(precio));
                    lbPrecioAcordado.Text = String.Format("{0:#,#}", precio);

                    lvUnidades.DataSource = listUnidades;
                    lvUnidades.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OperacionVenta - " + DateTime.Now + "- " + ex.Message + " - cbEmpresa_SelectedIndexChanged");
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

    #region Índices
    protected void rblIndice_TextChanged(object sender, EventArgs e)
    {
        if (rblIndice.SelectedValue == "CAC")
        {
            pnlComboCAC.Visible = true;
            pnlComboUVA.Visible = false;
        }

        if (rblIndice.SelectedValue == "UVA")
        {
            pnlComboCAC.Visible = false;
            pnlComboUVA.Visible = true;
        }

        if (rblIndice.SelectedValue == "Ninguno")
        {
            pnlComboCAC.Visible = false;
            pnlComboUVA.Visible = false;
        }
    }
    #endregion
}
