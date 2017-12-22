using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLL.Negocio;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                #region Combo
                cbProyectos.DataSource = cProyecto.GetDataTable();
                cbProyectos.DataValueField = "id";
                cbProyectos.DataTextField = "descripcion";
                cbProyectos.DataBind();
                ListItem io = new ListItem("Seleccione una obra...", "0");
                cbProyectos.Items.Insert(0, io);
                cbProyectos.SelectedIndex = 0;
                #endregion
                
                #region índice CAC
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Administración)
                {
                    //Aviso Índice CAC
                    if (string.IsNullOrEmpty(cIndiceCAC.GetLastIndiceMonth()))
                        pnlIndiceCAC.Visible = true;
                    else
                        pnlIndiceCAC.Visible = false;

                    //Aviso Índice UVA
                    if (Convert.ToDateTime(cAutorizacionUVA.GetAutorizacionByFecha().AddMonths(-1).ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                        pnlIndiceUVA.Visible = true;
                    else
                        pnlIndiceUVA.Visible = false;
                }
                #endregion

                #region Categoría
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Administración)
                {
                    pnlNuevoCliente.Visible = true;
                    pnlNuevaOV.Visible = true;
                    //pnlReservas.Visible = true;
                }
                else
                    Configuracion();


                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Gerencia)
                {
                    //pnlReservas.Visible = true;
                    pnlPendientes.Visible = true;
                    lbCantPrecios.Text = cActualizarPrecio.GetPrecios().Count.ToString();
                    lbCantOV.Text = cOperacionVenta.GetOV_AConfirmar().Count.ToString();
                }

                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Vendedor)
                    pnlDatos.Visible = false;

                #endregion

                lbDolar.Text = String.Format("{0:#,#0.00}", cValorDolar.LoadActualValue());

                decimal asdasd = cUVA.GetLastValorIndice();
                string sadasda = cUVA.GetLastValorIndice().ToString();
                string addwqwe = String.Format("{0:#,#0.00}", cUVA.GetLastValorIndice());


                lbUVA.Text = String.Format("{0:#,#0.00}", cUVA.GetLastValorIndice());
                lbCAC.Text = String.Format("{0:#,#0.00}", cIndiceCAC.GetLastValueIndice());
            }
            catch
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    Response.Redirect("Login.aspx", false);
            }
        }
    }

    public void Configuracion()
    {
        divAgenda.Attributes["style"] = "float:left; width:100px; padding: 20px 0 0 116px !important;";
        divCC.Attributes["style"] = "float:right; width:150px; padding: 20px 88px 0 25px; !important;";

        divOV.Attributes["style"] = "float:left; width:100px; padding: 20px 0 0 116px  !important;";
        divHistorial.Attributes["style"] = "float:right; width:150px; padding: 20px 88px 0 25px;";
    }

    protected void btnProyectos_Click(object sender, EventArgs e)
    {
        Response.Redirect("Unidad.aspx?idProyecto=" + cbProyectos.SelectedValue, false);
    }
}
