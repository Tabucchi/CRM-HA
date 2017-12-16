using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class Manual : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lvManuales.DataSource = cManual.GetManuales();
            lvManuales.DataBind();
        }
    }
     
    protected void ListPager_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (Request["id"] == null)
            {
                if (Request["idEmpresa"] != null)
                {
                    lvManuales.DataSource = cManual.SearchByEmpresa(Request["idEmpresa"]);
                }
                else
                    lvManuales.DataSource = cManual.GetManuales();
            }
            else
            {
                lvManuales.DataSource = cManual.Search(Convert.ToString(Request["id"]));
            }

            lvManuales.DataBind();
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - ListPager_PreRender" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    #region Botones
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtEmpresa.Text) && !string.IsNullOrEmpty(txtManual.Text))
        {
            Response.Redirect("Manual.aspx?idEmpresa=" + cEmpresa.GetIdByNombre(txtEmpresa.Text) + "&id=" + cManual.GetIdByNombre(txtManual.Text));
        }
        else
        {
            if (!string.IsNullOrEmpty(txtManual.Text))
            {
                Response.Redirect("Manual.aspx?id=" + cManual.GetIdByNombre(txtManual.Text));
            }
            else
            {
                if (!string.IsNullOrEmpty(txtEmpresa.Text))
                {
                    Response.Redirect("Manual.aspx?idEmpresa=" + cEmpresa.GetIdByNombre(txtEmpresa.Text));
                }
            }
        }
    }

    protected void btnNuevoManual_Click(object sender, EventArgs e)
    {
        Response.Redirect("DetalleManual.aspx?id=", false);
    }
    #endregion
}
