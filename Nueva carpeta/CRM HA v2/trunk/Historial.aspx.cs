using DLL;
using DLL.Negocio;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class Historial : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Combo
                cbProyecto.DataSource = cProyecto.GetDataTable();
                cbProyecto.DataValueField = "id";
                cbProyecto.DataTextField = "descripcion";
                cbProyecto.DataBind();
                ListItem ip = new ListItem("Seleccione una obra...", "0");
                cbProyecto.Items.Insert(0, ip);
                cbProyecto.SelectedIndex = 0;

                cbHistorial.DataSource = cHistorial.CargaCombo();
                cbHistorial.DataBind();
                #endregion
            }
        }

        #region Buscar Unidades
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            pnlMensaje.Visible = false;
            Listar(false);
        }

        public void Listar(bool todos)
        {
            try
            {
                pnlUnidades.Visible = true;
                cbNivel.DataSource = cUnidad.GroupByNivel(cbProyecto.SelectedValue);
                cbNivel.DataBind();
                ListItem ce = new ListItem("Seleccione un nivel...", "0");
                cbNivel.Items.Insert(0, ce);
                cbUnidad.Enabled = false;
                cbUnidad.SelectedIndex = 0;

                lvHistorial.DataSource = cHistorial.GetHistorialByIdProyecto(cbProyecto.SelectedValue, cbHistorial.SelectedIndex.ToString(), todos);
                lvHistorial.DataBind();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Historial - " + DateTime.Now + "- " + ex.Message + " - Listar" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Buscar unidad
        protected void cbNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cbUnidad.Enabled = true;
                cbUnidad.DataSource = cUnidad.GetNroUnidadMotivoByIdProyecto(cbProyecto.SelectedValue, cbNivel.SelectedItem.Text);
                cbUnidad.DataBind();
                ListItem iunidad = new ListItem("Seleccione el nro. de unidad...", "0");
                cbUnidad.Items.Insert(0, iunidad);
                cbUnidad.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Historial - " + DateTime.Now + "- " + ex.Message + " - cbUnidad_SelectedIndexChanged" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void cbUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cUnidad unidad = cUnidad.GetUnidadByProyecto(cbProyecto.SelectedValue, cbNivel.SelectedItem.Text, cbUnidad.SelectedItem.Text);
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Historial - " + DateTime.Now + "- " + ex.Message + " - cbUnidad_SelectedIndexChanged" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnBuscarUnidad_Click(object sender, EventArgs e)
        {
            try
            {
                lvHistorial.DataSource = cHistorial.GetHistorialByUnidad(cbProyecto.SelectedValue, cbUnidad.SelectedValue, cbNivel.SelectedValue, cbHistorial.SelectedValue);
                lvHistorial.DataBind();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Historial - " + DateTime.Now + "- " + ex.Message + " - btnBuscarUnidad_Click" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Imprimir
        string moneda = null;
        private DataSet CrearDataSet()
        {
            List<cHistorial> unidades = new List<cHistorial>();
            if (cbUnidad.SelectedValue != "Seleccione una unidad...")
                unidades = cHistorial.GetHistorialByUnidad(cbProyecto.SelectedValue, cbUnidad.SelectedValue, cbNivel.SelectedValue, cbHistorial.SelectedValue);
            else
                unidades = cHistorial.GetHistorialByIdProyecto(cbProyecto.SelectedValue, cbHistorial.SelectedValue, false);

            ArrayList seleccionados = new ArrayList();

            foreach (cHistorial h in unidades)
            {
                cHistorial pe = cHistorial.Load(h.Id);
                seleccionados.Add(pe);
            }

            DataTable dt = new DataTable();
            DataRow dr;
            DataSet ds = new DataSet();

            dt.Columns.Add(new DataColumn("fecha"));
            dt.Columns.Add(new DataColumn("motivo"));
            dt.Columns.Add(new DataColumn("GetProyecto"));
            dt.Columns.Add(new DataColumn("codUF"));
            dt.Columns.Add(new DataColumn("nroUnidad"));
            dt.Columns.Add(new DataColumn("PrecioOriginal"));
            dt.Columns.Add(new DataColumn("GetValorViejo"));
            dt.Columns.Add(new DataColumn("GetValorNuevo"));
            dt.Columns.Add(new DataColumn("Porcentaje"));
            dt.Columns.Add(new DataColumn("usuario"));

            foreach (cHistorial h in unidades)
            {
                dr = dt.NewRow();
                dr["fecha"] = String.Format("{0:dd/MM/yyyy}", h.Fecha);
                dr["motivo"] = h.Motivo;
                dr["GetProyecto"] = h.GetProyecto;
                dr["codUF"] = h.CodUF;
                dr["nroUnidad"] = h.NroUnidad;
                dr["PrecioOriginal"] = h.GetPrecioOriginal;
                dr["GetValorViejo"] = h.GetValorViejo;
                dr["GetValorNuevo"] = h.GetValorNuevo;
                dr["Porcentaje"] = h.PorcentajePrecio;
                dr["usuario"] = h.GetUsuario;
                dt.Rows.Add(dr);
                moneda = cUnidad.LoadByCodUF(h.CodUF, h.IdProyecto).GetMoneda;
            }

            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "tHistorial";

            return ds;
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "Archivos\\Historial\\";
            string filename = "Historial de " + cProyecto.Load(cbProyecto.SelectedValue).Descripcion + ".pdf";

            DataSetUnidades ds = new DataSetUnidades();
            ds.Merge(CrearDataSet(), false, System.Data.MissingSchemaAction.Ignore);
            CrystalReportSource.ReportDocument.SetDataSource(ds);

            CrystalReportSource.ReportDocument.SetParameterValue("fecha", String.Format("{0:MMMM yyyy}", DateTime.Today));
            CrystalReportSource.ReportDocument.SetParameterValue("fechaImpresion", String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now));
            CrystalReportSource.ReportDocument.SetParameterValue("obra", cbProyecto.SelectedValue);
            CrystalReportSource.ReportDocument.SetParameterValue("moneda", moneda);

            CrystalReportSource.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);

            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }
        #endregion

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            Listar(true);
        }
    }
}