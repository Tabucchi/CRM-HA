using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Clientes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lbUsuario.Text = cCliente.Load(Convert.ToString(Session["IdCliente"])).Nombre;
            string IdEmpresa = cCliente.Load(Convert.ToString(Session["IdCliente"])).IdEmpresa;

            #region Filtro
            cbPrioridad.DataSource = cCampoGenerico.GetDataTable(Tablas.tPrioridad);
            cbPrioridad.DataValueField = "id";
            cbPrioridad.DataTextField = "descripcion";
            cbPrioridad.DataBind();
            ListItem pr = new ListItem("Todas", "-1");
            cbPrioridad.Items.Insert(0, pr);
            cbPrioridad.SelectedIndex = -1;

            cbCliente.DataSource = cCliente.GetClientesByIdEmpresa(IdEmpresa);
            cbCliente.DataValueField = "id";
            cbCliente.DataTextField = "nombre";
            cbCliente.DataBind();
            ListItem cl = new ListItem("Todos", "-1");
            cbCliente.Items.Insert(0, cl);
            cbCliente.SelectedIndex = -1;

            cbEstado.DataSource = cCampoGenerico.GetDataTable(Tablas.tEstado);
            cbEstado.DataValueField = "id";
            cbEstado.DataTextField = "descripcion";
            cbEstado.DataBind();
            ListItem es = new ListItem("Todos", "-1");
            cbEstado.Items.Insert(0, es);
            cbEstado.SelectedIndex = 1;
            cbEstado.SelectedItem.Value = "0";
            #endregion

            lvClientes.DataSource = cPedido.GetPedidos(IdEmpresa);
            lvClientes.DataBind();
        }
    }

    protected void lvClientes_PreRender(object sender, EventArgs e)
    {
        string IdEmpresa = cCliente.Load(Convert.ToString(Session["IdCliente"])).IdEmpresa;
        List<cPedido> pedidos = cPedido.SearchCliente(IdEmpresa, cbPrioridad.SelectedValue, cbCliente.SelectedValue, cbEstado.SelectedValue,txtFechaDesde.Text,txtFechaHasta.Text);
        lvClientes.DataSource = pedidos;
        lvClientes.DataBind();
    }
  
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string IdEmpresa = cCliente.Load(Convert.ToString(Session["IdCliente"])).IdEmpresa;
       
        #region Exportar a Excel
        DataTable table = cPedido.ObtenerReporteXLS(IdEmpresa);
        
        string tab = "";

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=Pedidos.xls");
        Response.ContentType = "application/vnd.xls";
        Response.Charset = "";

        foreach (DataColumn dc in table.Columns)
        {
            Response.Write(tab + dc.ColumnName);
            tab = "\t";
        }
        Response.Write("\n");
        int i;
        foreach (DataRow dr in table.Rows)
        {
            tab = "";
            for (i = 0; i < table.Columns.Count; i++)
            {
                Response.Write(tab + dr[i].ToString());
                tab = "\t";
            }
            Response.Write("\n");
        }

        Response.End();
        #endregion       
    }

    protected void lnkDetalles_Click(object sender, EventArgs e)
    {
        LinkButton boton = (LinkButton)sender;
        Response.Redirect("DetallePedidoCliente.aspx?id=" + boton.CommandArgument.ToString(), false);
    }
}
