using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using DLL.Negocio;
using System.Text;
using log4net;
public partial class Agenda : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region Combos
            cbEmpresa.DataSource = cEmpresa.GetDataTable();
            cbEmpresa.DataValueField = "id";
            cbEmpresa.DataTextField = "nombre";
            cbEmpresa.DataBind();
            ListItem ce = new ListItem("Seleccione un cliente...", "0");
            cbEmpresa.Items.Insert(0, ce);
            ListItem cet = new ListItem("Todos", "1");
            cbEmpresa.Items.Insert(1, cet);
            #endregion

            lvEmpresas.DataSource = cEmpresa.GetEmpresas();
            lvEmpresas.DataBind();
        }
    }

    #region ListView
    //OnPreRender(método): indica al control de servidor que realice los pasos previos a la representación que sean necesarios antes de guardar el estado de vista y el contenido de la representación.
    protected void ListPager_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (Request["idEmpresa"] == null)
            {
                lvEmpresas.DataSource = cEmpresa.GetEmpresas();
                lvEmpresas.DataBind();
            }
            else
            {
                lvEmpresas.DataSource = cEmpresa.Search(Convert.ToString(Request["idEmpresa"]));
                lvEmpresas.DataBind();
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - ListPager_PreRender" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void lvEmpresas_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int32)eCategoria.Administración)
                pnlFormulario.Visible = true;

            if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria != (Int32)eCategoria.Administración)
            {
                Panel pnl = e.Item.FindControl("pnlEditar") as Panel;
                pnl.Visible = false;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvEmpresas_ItemDataBound" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void lvProyectos_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            cEmpresa empresa = cEmpresa.Load(e.CommandArgument.ToString());

            switch (e.CommandName)
            {
                case "Detalle":
                    lvDetalle.DataSource = cUnidad.GetUnidadesByIdEmpresa(empresa.Id);
                    lvDetalle.DataBind();
                    lbCliente.Text = empresa.Nombre + " " + empresa.Apellido;
                    ModalDetalle.Show();
                    break;
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - lvProyectos_ItemCommand" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }
    #endregion

    #region Métodos de combo
    protected void cbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbEmpresa.SelectedValue == "1")
            Response.Redirect("Agenda.aspx");
        else
            Response.Redirect("Agenda.aspx?idEmpresa=" + cbEmpresa.SelectedValue);
    }
    #endregion

    #region Imprimir
    protected void btnImprimirCliente_Click(object sender, ImageClickEventArgs e)
    {
        string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Reportes\\";
        string filename = "Formulario_Cliente.pdf";

        CrystalReportSource.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

        Response.ContentType = "APPLICATION/pdf";
        Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);
        FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
        Response.Flush();
        Response.WriteFile(rutaURL + filename);
        Response.End();
    }
    #endregion

    #region Botones
    protected void btnCerrarObras_Click(object sender, EventArgs e)
    {
        ModalDetalle.Hide();
    }
    #endregion
}
