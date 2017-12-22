using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace crm
{
    public partial class stats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DrawChartDistribucionDeTrabajo();
            DrawChartTop5();
            DrawChartTicketsAbiertos();
            rbMeses_SelectedIndexChanged(null, null);            
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

        #region Tickets no resueltos
        public void DrawChartTicketsAbiertos()
        {
            string dataInChart = "['Label', 'Value'],";
            dataInChart = dataInChart + "[' '," + cPedido.SearchByEstado(Estado.Nuevo).Count + "],";

            string script = @"<script type='text/javascript'>
                              google.load('visualization', '1', {packages:['gauge']});
                              google.setOnLoadCallback(drawChart);
                              function drawChart() {
                                var data = google.visualization.arrayToDataTable([ " + dataInChart + @"]);

                                var options = {
                                  width: 400, height: 200,
                                  max: 50,
                                  redFrom:40, redTo:50,
                                  yellowFrom:30, yellowTo:40,
                                  greenFrom:10, greenTo:30,
                                  minorTicks: 5
                                };

                                var chart = new google.visualization.Gauge(document.getElementById('chartClock'));
                                chart.draw(data, options);
                              }
                            </script>
                        ";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "chartClock", script, false);
        }
        #endregion
    }
}