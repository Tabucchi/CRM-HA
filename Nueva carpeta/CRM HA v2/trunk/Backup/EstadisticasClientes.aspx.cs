using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class EstadisticasClientes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string IdEmpresa = cCliente.Load(Convert.ToString(Session["IdCliente"])).IdEmpresa;

        if (!IsPostBack)
        {
           /* #region Combo cliente
            cbCliente.DataSource = cCliente.GetClientesByIdEmpresa(IdEmpresa);
            cbCliente.DataValueField = "id";
            cbCliente.DataTextField = "nombre";
            cbCliente.DataBind();
            ListItem cl = new ListItem("Todos", "-1");
            cbCliente.Items.Insert(0, cl);
            cbCliente.SelectedIndex = -1;
            #endregion

            #region Top 5
            rptRanking.DataSource = cPedido.GetRanking(IdEmpresa, DatoAGraficar.idCliente, 5, 3);
            rptRanking.DataBind();
            #endregion*/
        }

       /* #region Torta de Estados
        GraficarByEstado();
        #endregion*/

      //  rbMeses_SelectedIndexChanged(null, null);
     //   GraficarTotalPedidosCliente(ChartTotalPedidosCliente, IdEmpresa);
    }

    /*
    #region Graficos
    //Grafico de torta Pedidos por Estado
    public void GraficarByEstado()
    {
        string idEmpresa = cCliente.Load(Convert.ToString(Session["IdCliente"])).IdEmpresa;
        Dictionary<string, int> d = cPedido.GetEstadoUltimoMes(idEmpresa);

        //if (d.Count != 0)
        //{
        //    foreach (KeyValuePair<string, int> entry in d)
        //    {
        //        var item = new System.Web.UI.DataVisualization.Charting.DataPoint(entry.Value, entry.Value);
        //        item.LegendText = entry.Key.ToString()+"s";
        //        item.ToolTip = entry.Value.ToString();
        //        PieChart.Series[0].Points.Add(item);
        //    }
        //}else
        //{
        //    PieChart.Visible = false;
        //    pnlUltimoMes.Visible = true;
        //    lbMensajeUltimoMes.Text = "No se encontraron pedidos en el último mes.";
        //}
    }

    //Grafico de barras para mostrar los pedidos de cada cliente
    public void GraficarPedidosPorCliente(Dictionary<string, int> d)
    {
        int total = 0;
        int i = 0;

        // Limpio el grafico
        //ChartCliente.Series[0].Points.Clear();

        //foreach (KeyValuePair<string, int> entry in d)
        //{
        //    total = total + entry.Value;
        //    ChartCliente.Series[0].Points.InsertXY(i, entry.Key, entry.Value);
        //    ChartCliente.Series[0].Points[i].ToolTip = Convert.ToString(entry.Value + " Tickets");
        //    i++;
        //}

        //if (total != 0)
        //{
        //    lbCantRegistros.Text = Convert.ToString(total);
        //    //Promedio Mensual
        //    decimal promedio = Convert.ToDecimal(total)/Convert.ToDecimal(d.Count);
        //    lbPromedioMensual.Text = Convert.ToString(decimal.Round(promedio, 2));
        //}
        //else
        //{
        //    ChartCliente.Visible = false;
        //    pnlRegistros.Visible = false;
        //    lbMensajeError.Visible = true;
        //    lbMensajeError.Text = "No se encontraron registros.";
        //}
    }

    //Grafico de barras para mostrar el total de tickets por cliente
    public void GraficarTotalPedidosCliente(Dictionary<string, int> d)
    {
        //int total = 0;
        //int i = 0;

        //ChartTotalPedidosCliente.Series[0].Points.Clear();
        //lbMensajeError_CantCliente.Visible = false;

        //foreach (KeyValuePair<string, int> entry in d)
        //{
        //    total = total + entry.Value;
        //    ChartTotalPedidosCliente.Series[0].Points.InsertXY(i, entry.Key, entry.Value);
        //    ChartTotalPedidosCliente.Series[0].Points[i].ToolTip = Convert.ToString(entry.Value + " Tickets") + " (" + entry.Key + ")";
        //    i++;
        //}

        //if (total != 0)
        //{
        //    lbCantRegistros_CantPedidosClientes.Text = Convert.ToString(total);
        //}
        //else
        //{
        //    ChartTotalPedidosCliente.Visible = false;
        //    pnlCantPedidosClientes.Visible = false;
        //    lbMensajeError_CantCliente.Visible = true;
        //    lbMensajeError_CantCliente.Text = "No se encontraron registros";
     //   }
    }

    //Grafico de barras para mostrar los estados de los tickets, separando cada estado por color
    public void GraficarPorEstado(int cantMeses, string _idEmpresa)
    {
        int total = 0, cant_nuevos=0, cant_finalizados=0;
        //Dictionary<string, int[]> d = cPedido.GetPedidosPorEstado(cantMeses, _idEmpresa);
        //int i = 0;

        //// Limpio el grafico
        //ChartEstado.Series["Series1"].Points.Clear();
        //ChartEstado.Series["Series2"].Points.Clear();

        //foreach (KeyValuePair<string, int[]> entry in d)
        //{
        //    ArrayList estados = cCampoGenerico.LoadTable(Tablas.tEstado);
            
        //    total = total + entry.Value[0] + entry.Value[1];
        //    cant_nuevos = cant_nuevos + entry.Value[0];
        //    cant_finalizados = cant_finalizados + entry.Value[1];

        //    //Tickets Finalizados
        //    var item_finalizado = new System.Web.UI.DataVisualization.Charting.DataPoint(i, entry.Value[0]);
        //    item_finalizado.SetValueXY(entry.Key, entry.Value[1]);
        //    item_finalizado.ToolTip = Convert.ToString(entry.Value[1] + " Tickets Finalizados");
        //    ChartEstado.Series["Series1"].Points.Add(item_finalizado);

        //    //Tickets Nuevos
        //    var item_nuevo = new System.Web.UI.DataVisualization.Charting.DataPoint(i, 0);
        //    item_nuevo.SetValueXY(entry.Key, entry.Value[0]);
        //    item_nuevo.ToolTip = Convert.ToString(entry.Value[0] + " Tickets Nuevos");
        //    ChartEstado.Series["Series2"].Points.Add(item_nuevo);

        //    i++;
        //}

        //if (total != 0)
        //{
        //    lbCantRegistrosEstado.Text = Convert.ToString(total);
        //    lbTicketsNuevos.Text = Convert.ToString(cant_nuevos);
        //    lbTicketsFinalizados.Text = Convert.ToString(cant_finalizados);
        //}else
        //{
        //    ChartEstado.Visible = false;
        //    pnlRefenciaEstados.Visible = false;
        //    pnlEstado.Visible = false;
        //    lbMensajeErrorEstado.Visible = true;
        //    lbMensajeErrorEstado.Text = "No se encontraron registros.";
        //}
    }

    //Grafico de barras para mostrar los estados de los tickets, separando cada estado por color
    public void GraficarPorPrioridad(int cantMeses, string _idEmpresa)
    {
        int total = 0, cant_sinUrgencia = 0, cant_inmediatos = 0, cant_24hs=0, cant_48hs=0, cant_proxVisita=0;
        Dictionary<string, int[]> d = cPedido.GetPedidosPorPrioridad(cantMeses, _idEmpresa);
        int i = 0;

        // Limpio el grafico
        //ChartPrioridad.Series["Series1"].Points.Clear();
        //ChartPrioridad.Series["Series2"].Points.Clear();
        //ChartPrioridad.Series["Series3"].Points.Clear();
        //ChartPrioridad.Series["Series4"].Points.Clear();
        //ChartPrioridad.Series["Series5"].Points.Clear();

        //foreach (KeyValuePair<string, int[]> entry in d)
        //{
        //    ArrayList prioridades = cCampoGenerico.LoadTable(Tablas.tPrioridad);

        //    total = total + entry.Value[0] + entry.Value[1] + entry.Value[2] + entry.Value[3] + entry.Value[4];
        //    cant_sinUrgencia = cant_sinUrgencia + entry.Value[0];
        //    cant_inmediatos = cant_inmediatos + entry.Value[1];
        //    cant_24hs = cant_24hs + entry.Value[2];
        //    cant_48hs = cant_48hs + entry.Value[3];
        //    cant_proxVisita = cant_proxVisita + entry.Value[4];

        //    //Sin Urgencia
        //    var item_sin_urgencia = new System.Web.UI.DataVisualization.Charting.DataPoint(i, 0);
        //    item_sin_urgencia.SetValueXY(entry.Key, entry.Value[0]);
        //    item_sin_urgencia.ToolTip = Convert.ToString(entry.Value[0] + " Tickets Sin Urgencia");
        //    ChartPrioridad.Series["Series1"].Points.Add(item_sin_urgencia);

        //    //Inmediato
        //    var item_inmediato = new System.Web.UI.DataVisualization.Charting.DataPoint(i, entry.Value[0]);
        //    item_inmediato.SetValueXY(entry.Key, entry.Value[1]);
        //    item_inmediato.ToolTip = Convert.ToString(entry.Value[1] + " Tickets Inmediatos");
        //    ChartPrioridad.Series["Series2"].Points.Add(item_inmediato);

        //    //24 hs.
        //    var item_24hs = new System.Web.UI.DataVisualization.Charting.DataPoint(i, entry.Value[1]);
        //    item_24hs.SetValueXY(entry.Key, entry.Value[2]);
        //    item_24hs.Color = System.Drawing.Color.Coral;
        //    item_24hs.ToolTip = Convert.ToString(entry.Value[2] + " Tickets 24 hs");
        //    ChartPrioridad.Series["Series3"].Points.Add(item_24hs);

        //    //48 hs.
        //    var item_48hs = new System.Web.UI.DataVisualization.Charting.DataPoint(i, entry.Value[2]);
        //    item_48hs.SetValueXY(entry.Key, entry.Value[3]);
        //    item_48hs.Color = System.Drawing.Color.DarkMagenta;
        //    item_48hs.ToolTip = Convert.ToString(entry.Value[3] + " Tickets 48 hs");
        //    ChartPrioridad.Series["Series4"].Points.Add(item_48hs);

        //    //Próxima Visita
        //    var item_proxVisita = new System.Web.UI.DataVisualization.Charting.DataPoint(i, entry.Value[3]);
        //    item_proxVisita.SetValueXY(entry.Key, entry.Value[4]);
        //    item_proxVisita.Color = System.Drawing.Color.LightGray;
        //    item_proxVisita.ToolTip = Convert.ToString(entry.Value[4] + " Tickets Próxima visita");
        //    ChartPrioridad.Series["Series5"].Points.Add(item_proxVisita);

        //    i++;
        //}
        
        //if (total != 0)
        //{
        //    lbCantRegistrosPrioridad.Text = Convert.ToString(total);
        //    lbSinUrgencia.Text = Convert.ToString(cant_sinUrgencia);
        //    lbInmediato.Text = Convert.ToString(cant_inmediatos);
        //    lb24hs.Text = Convert.ToString(cant_24hs);
        //    lb48hs.Text = Convert.ToString(cant_48hs);
        //    lbProximaVisita.Text = Convert.ToString(cant_proxVisita);
        //}
        //else
        //{
        //    ChartPrioridad.Visible = false;
        //    pnlRefenciaPrioridades.Visible = false;
        //    pnlPrioridad.Visible = false;
        //    lbMensajeErrorPrioridad.Visible = true;
        //    lbMensajeErrorPrioridad.Text = "No se encontraron registros.";
        //}
    }
    #endregion

    #region Filtro
    protected void rbMeses_SelectedIndexChanged(object sender, EventArgs e)
    {
        string IdEmpresa = cCliente.Load(Convert.ToString(Session["IdCliente"])).IdEmpresa;       

        if (rbMeses.SelectedItem.Text == "ÚLTIMO MES")
        {
            Dictionary<string, int> d = cPedido.GetCantidadPedidosPorMes(1, IdEmpresa, cbCliente.SelectedValue);
            GraficarPedidosPorCliente(d);
            GraficarPorEstado(1, IdEmpresa);
            GraficarPorPrioridad(1, IdEmpresa);

            Dictionary<string, int> _d = cPedido.GetTotalPedidosCliente(1, IdEmpresa);
            GraficarTotalPedidosCliente(_d);
        }

        if(rbMeses.SelectedItem.Text == "TRIMESTRE")
        {
            Dictionary<string, int> d = cPedido.GetCantidadPedidosPorMes(3, IdEmpresa, cbCliente.SelectedValue);
            GraficarPedidosPorCliente(d);
            GraficarPorEstado(3, IdEmpresa);
            GraficarPorPrioridad(3, IdEmpresa);

            Dictionary<string, int> _d = cPedido.GetTotalPedidosCliente(3, IdEmpresa);
            GraficarTotalPedidosCliente(_d);
        }

        if (rbMeses.SelectedItem.Text == "SEMESTRE")
        {
            Dictionary<string, int> d = cPedido.GetCantidadPedidosPorMes(6, IdEmpresa, cbCliente.SelectedValue);
            GraficarPedidosPorCliente(d);
            GraficarPorEstado(6, IdEmpresa);
            GraficarPorPrioridad(6, IdEmpresa);

            Dictionary<string, int> _d = cPedido.GetTotalPedidosCliente(6, IdEmpresa);
            GraficarTotalPedidosCliente(_d);
        }

        if (rbMeses.SelectedItem.Text == "ÚLTIMO AÑO")
        {
            Dictionary<string, int> d = cPedido.GetCantidadPedidosPorMes(12, IdEmpresa, cbCliente.SelectedValue);
            GraficarPedidosPorCliente(d);
            GraficarPorEstado(12, IdEmpresa);
            GraficarPorPrioridad(12, IdEmpresa);

            Dictionary<string, int> _d = cPedido.GetTotalPedidosCliente(12, IdEmpresa);
            GraficarTotalPedidosCliente(_d);
        }

        if (rbMeses.SelectedItem.Text == "HISTORICO")
        {
            TimeSpan res = DateTime.Today - cPedido.GetFechaPrimerPedido(IdEmpresa);
            int cantMeses = Convert.ToInt16(res.Days / 30) + 1;

            Dictionary<string, int> d = cPedido.GetCantidadPedidosPorMes(cantMeses, IdEmpresa, cbCliente.SelectedValue);
            GraficarPedidosPorCliente(d);
            GraficarPorEstado(cantMeses, IdEmpresa);
            GraficarPorPrioridad(cantMeses, IdEmpresa);

            Dictionary<string, int> _d = cPedido.GetTotalPedidosCliente(cantMeses, IdEmpresa);
            GraficarTotalPedidosCliente(_d);
        }
    }
    #endregion*/
}
