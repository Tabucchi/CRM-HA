using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DLL.Negocio;

namespace crm
{
    public partial class stats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbTotalCobrar.Text = cCuota.GetTotalACobrar();
            DrawChartTotalCobarPorCliente();
            DrawChartTotalCobarPorProyecto();

            DrawChartDistribucionDeTrabajo();
            DrawChartTop5();
            lbTicketAbiertos.Text = cPedido.SearchByEstado(Estado.Nuevo).Count.ToString();
            rbMeses_SelectedIndexChanged(null, null);

            Graficar1(1, "ada");
        }

        #region Distribucion de trabajo
        public void DrawChartDistribucionDeTrabajo()
        {
            Dictionary<string, int> data = cPedido.GetTicketsSolucionadoPorUsuarioUltimoMes();

            string dataInChart = "['Usuario', 'Resueltos'],";
            foreach (KeyValuePair<string, int> entry in data)
            {
                dataInChart = dataInChart + "['" + entry.Key + "'," + entry.Value + "],";
            }

            string script = @"<script type='text/javascript'>

                              google.load('visualization', '1', {packages:['corechart']});
                              google.setOnLoadCallback(drawChart);
      
                              function drawChart() {
                                  var data = google.visualization.arrayToDataTable([ " + dataInChart + @"]);

                                  var options = { 
                                      title: 'My Daily Activities',
                                      chartArea:{left:10,top:10,right:10,bottom:10,width:'95%',height:'95%'},
                                        backgroundColor: 'transparent', 
                                  };
 
                                  var chart = new google.visualization.PieChart(document.getElementById('pieChart'));
                                  chart.draw(data, options);
                              }
                              </script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "chart_distribucion_de_trabajo", script, false);
        }
        #endregion

        #region Top 5 Empresas con mas pedido ultimo trimestre
        public void DrawChartTop5()
        {
            DataTable table = cPedido.GetRanking(null, DatoAGraficar.idEmpresa, 5, 3);
            table.Columns.Add(new DataColumn("nombreEmpresa"));

            foreach (DataRow r in table.Rows)
            {
                r["nombreEmpresa"] = cEmpresa.GetNombreEmpresa(r["idEmpresa"].ToString());
            }

            rptRanking.DataSource = table;
            rptRanking.DataBind();
        }
        #endregion

        #region Total a cobrar
        public void DrawChartTotalCobarPorProyecto()
        {
            /*DataTable table = cCuentaCorriente.GetTotalACobrarPorProyecto();
            table.Columns.Add(new DataColumn("nombreProyecto"));

            foreach (DataRow r in table.Rows)
            {
                cUnidad unidad = cUnidad.Load(r[0].ToString());
                r["nombreProyecto"] = unidad.Nivel + " " + unidad.NroUnidad;
            }

            rptTotalACobrarProyecto.DataSource = table;
            rptTotalACobrarProyecto.DataBind();*/
        }
        #endregion

        #region Tickets no resueltos
        public void DrawChartTotalCobarPorCliente()
        {
            DataTable table = cCuentaCorriente.GetTotalACobrarPorCliente();
            table.Columns.Add(new DataColumn("nombreEmpresa"));

            foreach (DataRow r in table.Rows)
            {
                r["nombreEmpresa"] = cEmpresa.GetNombreEmpresa(r["id"].ToString());
            }

            rptTotalACobrarCliente.DataSource = table;
            rptTotalACobrarCliente.DataBind();
        }
        #endregion

        #region Tickets por mes
        protected void rbMeses_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (rbMeses.SelectedItem.Text)
            {
                case "SEMESTRE":
                    GraficarPedidosPorMes(6, null);
                    break;

                case "HISTORICO":
                    TimeSpan res = DateTime.Today - cPedido.GetFechaPrimerPedido("0");
                    int cantMeses = Convert.ToInt16(res.Days / 30) + 1;
                    GraficarPedidosPorMes(cantMeses, null);
                    break;

                default: // Ultimo año
                    GraficarPedidosPorMes(12, null);
                    break;                   
            }
        }

        public void GraficarPedidosPorMes(int cantMeses, string idEmpresa)
        {
            int total = 0;
            Dictionary<string, int> data = cPedido.GetCantidadPedidosPorMes(cantMeses, idEmpresa, null);

            string dataInChart = "['Meses', 'Cantidad'],";
            
            foreach (KeyValuePair<string, int> entry in data)
            {
                total = total + entry.Value;
                dataInChart = dataInChart + "['" + entry.Key + "'," + entry.Value + "],";
            }

            string script = @"<script type='text/javascript'>

                              google.load('visualization', '1', {packages:['corechart']});
                              google.setOnLoadCallback(drawChart);
      
                              function drawChart() {
                                  var data = google.visualization.arrayToDataTable([ " + dataInChart + @"]);

                                  var options = { title: ''};                            

                                  var chart = new google.visualization.ColumnChart(document.getElementById('barChart'));
                                  chart.draw(data, options);
                              }     
                              </script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "barChart", script, false);
            
            lbCantRegistros.Text = Convert.ToString(total);

            //Promedio Mensual
            lbPromedioMensaul.Text = Convert.ToString(decimal.Round(Convert.ToDecimal(total) / Convert.ToDecimal(cantMeses), 2));
        }
        #endregion

        public void Graficar1(int cantMeses, string idEmpresa)
        {
            Dictionary<string, string> data = cUnidad.GetEstadoUnidadesPorProyecto();

            string dataInChart = "['Obra', 'Disponible', 'Reservado', 'Vendido'],";

            foreach (KeyValuePair<string, string> entry in data)
            {
                dataInChart = dataInChart + "['" + entry.Key + "'," + entry.Value + "],";
            }
            
            string script = @"<script type='text/javascript'>
                                function drawVisualization() {
                                    // Create and populate the data table.
                                    var data = google.visualization.arrayToDataTable([" + dataInChart + @"]);

                                    // Create and draw the visualization.
                                    new google.visualization.ColumnChart(document.getElementById('StackedUnidades')).
                                        draw(data,
                                            {width:600, height:400, isStacked: true,}
                                    );
                                }
                                
                                google.load('visualization', '1', {packages:['corechart']});
                                google.setOnLoadCallback(drawVisualization);  
                              </script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "StackedUnidades", script, false);
        }
    }
}