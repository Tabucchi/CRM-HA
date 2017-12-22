using DLL;
using DLL.Negocio;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class DetalleUnidadesVendidas : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    cProyecto proyecto = cProyecto.Load(Request["idProyecto"].ToString());
                    lbProyecto.Text = proyecto.Descripcion;
                
                    lvUnidades.DataSource = cUnidad.GetListUnidadesVendidas(proyecto.Id);
                    lvUnidades.DataBind();
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Proyecto - " + DateTime.Now + "- " + ex.Message + " - Page_Load" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        #region Descargar
        private DataSet CrearDataTableUnidadesVendidas()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            DataSet ds = new DataSet();

            dt.Columns.Add(new DataColumn("cliente"));
            dt.Columns.Add(new DataColumn("codUf"));
            dt.Columns.Add(new DataColumn("nivel"));
            dt.Columns.Add(new DataColumn("nroUnidad"));
            dt.Columns.Add(new DataColumn("fechaBoleto"));
            dt.Columns.Add(new DataColumn("fechaPosesion"));
            dt.Columns.Add(new DataColumn("fechaEscritura"));

            foreach (ListViewItem item in lvUnidades.Items)
            {
                Label lbCliente = item.FindControl("lbCliente") as Label;
                Label lbCodUF = item.FindControl("lbCodUF") as Label;
                Label lbNivel = item.FindControl("lbNivel") as Label;
                Label lbNroUnidad = item.FindControl("lbNroUnidad") as Label;
                Label lbFechaBoleto = item.FindControl("lbFechaBoleto") as Label;
                Label lbFechaPosesion = item.FindControl("lbFechaPosesion") as Label;
                Label lbFechaEscritura = item.FindControl("lbFechaEscritura") as Label;

                dr = dt.NewRow();
                dr["cliente"] = lbCliente.Text;
                dr["codUf"] = lbCodUF.Text;
                dr["nivel"] = lbNivel.Text;
                dr["nroUnidad"] = lbNroUnidad.Text;
                dr["fechaBoleto"] = lbFechaBoleto.Text;
                dr["fechaPosesion"] = lbFechaPosesion.Text;
                dr["fechaEscritura"] = lbFechaEscritura.Text;
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "tUnidadesVendidasPorObra";

            return ds;
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath;
            string filename = "UnidadesVendidas.pdf";

            // Planilla
            DataSetUnidades ds = new DataSetUnidades();
            ds.Merge(CrearDataTableUnidadesVendidas(), false, System.Data.MissingSchemaAction.Ignore);
            CrystalReportSource.ReportDocument.SetDataSource(ds);

            CrystalReportSource.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);

            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }
        #endregion

        #region Editar
        protected void lvUnidades_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                cOperacionVenta op = cOperacionVenta.GetIdOperacionVentaByUnidad(e.CommandArgument.ToString());

                if (op == null)
                {
                    string _idOpv = cEmpresaUnidad.GetIdOperacionVentaByIdUnidad(e.CommandArgument.ToString());

                    if (_idOpv == "-1")
                        ModalMensaje.Show();
                }
                else
                {
                    switch (e.CommandName)
                    {
                        case "Editar":
                            {
                                txtEditFechaPosesion.Text = op.GetFechaPosesion;
                                txtEditFechaEscritura.Text = op.GetFechaEscritura;
                                hfId.Value = op.Id;
                                ModalEdit.Show();
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("DetalleUnidadesVendidas - " + DateTime.Now + "- " + ex.Message + " - lvUnidades_ItemCommand" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnCerrarEditar_Click(object sender, EventArgs e)
        {
            ModalEdit.Hide();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cOperacionVenta op = cOperacionVenta.Load(hfId.Value);

            if (!string.IsNullOrEmpty(txtEditFechaPosesion.Text) && txtEditFechaPosesion.Text != "-")
                op.FechaPosesion = Convert.ToDateTime(txtEditFechaPosesion.Text);
            else
                op.FechaEscritura = null;

            if (!string.IsNullOrEmpty(txtEditFechaEscritura.Text) && txtEditFechaEscritura.Text != "-")
                op.FechaEscritura = Convert.ToDateTime(txtEditFechaEscritura.Text);
            else
                op.FechaEscritura = null;
                
            op.Save();
            Response.Redirect("DetalleUnidadesVendidas.aspx?idProyecto=" + Request["idProyecto"].ToString());
        }
        #endregion

        #region Mensaje
        protected void btnCerrarMensaje_Click(object sender, EventArgs e)
        {
            ModalMensaje.Hide();
        }
        #endregion
    }
}