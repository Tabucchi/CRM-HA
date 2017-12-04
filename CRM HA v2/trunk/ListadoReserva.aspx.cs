using DLL;
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
    public partial class ListadoReserva : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lvReservas.DataSource = cReserva.GetReservasToday();
                lvReservas.DataBind();

                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria != (Int32)eCategoria.Administración)
                        pnlReserva.Visible = false;
                }
            }
        }

        protected void btnReserva_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reserva.aspx");
        }

        #region Descargar
        private DataSet CrearDataTableReservas()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            DataSet ds = new DataSet();

            dt.Columns.Add(new DataColumn("cliente"));
            dt.Columns.Add(new DataColumn("obra"));
            dt.Columns.Add(new DataColumn("cod"));
            dt.Columns.Add(new DataColumn("unidadFuncional"));
            dt.Columns.Add(new DataColumn("nivel"));
            dt.Columns.Add(new DataColumn("nroUnidad"));

            foreach (ListViewItem item in lvReservas.Items)
            {
                Label lbCliente = item.FindControl("lbCliente") as Label;
                Label lbProyecto = item.FindControl("lbProyecto") as Label;
                Label lbCodUF = item.FindControl("lbCodUF") as Label;
                Label lbUnidadFuncional = item.FindControl("lbUnidadFuncional") as Label;
                Label lbNivel = item.FindControl("lbNivel") as Label;
                Label lbNroUnidad = item.FindControl("lbNroUnidad") as Label;

                dr = dt.NewRow();
                dr["cliente"] = lbCliente.Text;
                dr["obra"] = lbProyecto.Text;
                dr["cod"] = lbCodUF.Text;
                dr["unidadFuncional"] = lbUnidadFuncional.Text;
                dr["nivel"] = lbNivel.Text;
                dr["nroUnidad"] = lbNroUnidad.Text;
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "tReservas";

            return ds;
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            //string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\";
            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath;
            string filename = "Reservas.pdf";

            // Planilla
            DataSetUnidades ds = new DataSetUnidades();
            ds.Merge(CrearDataTableReservas(), false, System.Data.MissingSchemaAction.Ignore);
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

    }
}