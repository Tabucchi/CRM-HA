using DLL;
using DLL.Auxiliares;
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
    public partial class Morosos : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            lvMorosos.DataSource = Cargar();
            lvMorosos.DataBind();

            CalcularTotales();
        }

        #region Auxiliares
        public List<cMoroso> Cargar()
        {
            List<cMoroso> listMorosos = new List<cMoroso>();
            DataTable dtMorosos = cCuota.GetMorosos();
            
            foreach (DataRow row in dtMorosos.Rows)
            {
                cMoroso moroso = new cMoroso();
                moroso.empresa = cEmpresa.Load(row["id"].ToString()).GetNombreCompleto;

                if (row["moneda"].ToString() == Convert.ToString((Int16)tipoMoneda.Dolar))
                    moroso.monto = Convert.ToDecimal(row["monto"].ToString()) * Convert.ToDecimal(row["valorDolar"].ToString());
                else
                    moroso.monto = Convert.ToDecimal(row["monto"].ToString());

                moroso.idCuentaCorriente = row["idCC"].ToString();                
                listMorosos.Add(moroso);
            }

            return listMorosos;            
        }

        private void CalcularTotales()
        {
            try
            {
                decimal _total = 0;

                foreach (ListViewItem item in lvMorosos.Items)
                {
                    Label lbTotal = item.FindControl("lbMonto") as Label;

                    _total += Convert.ToDecimal(lbTotal.Text);
                }

                Label lblTotalMes = (Label)lvMorosos.FindControl("lbTotal");

                hfTotal.Value = String.Format("{0:#,#0.00}", _total);
                lblTotalMes.Text = String.Format("{0:#,#0.00}", _total);
            }
            catch (Exception ex)
            {

                log4net.Config.XmlConfigurator.Configure();
                log.Error("CuotasCliente - " + DateTime.Now + "- " + ex.Message + " - CalcularTotales" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region ListView
        protected void lvMorosos_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "Comentario":
                    {
                        rptComentarios.DataSource = cComentarioMorosos.GetComentariosByIdCCC(id);
                        rptComentarios.DataBind();
                        hfIdCC.Value = id;
                        ModalPopupExtender.Show();
                        break;
                    }
            }
        }
        #endregion

        #region Descargar
        private DataSet CrearDataTableMorosos()
        {
            DataTable dtMorosos = cCuota.GetMorosos();

            DataTable dt = new DataTable();
            DataRow dr;
            DataSet ds = new DataSet();

            dt.Columns.Add(new DataColumn("cliente"));
            dt.Columns.Add(new DataColumn("monto"));

            foreach (DataRow row in dtMorosos.Rows)
            {
                dr = dt.NewRow();
                dr["cliente"] = cEmpresa.Load(row["id"].ToString()).GetNombreCompleto;
                if (row["moneda"].ToString() == tipoMoneda.Dolar.ToString())
                    dr["monto"] = String.Format("{0:#,#0.00}", Convert.ToDecimal(row["monto"].ToString()) * Convert.ToDecimal(row["valorDolar"].ToString()));
                else
                    dr["monto"] = String.Format("{0:#,#0.00}", Convert.ToDecimal(row["monto"].ToString()));
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "tMorosos";

            return ds;
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath;
            string filename = "Deuda pendiente.pdf";

            // Planilla
            DataSetUnidades ds = new DataSetUnidades();
            ds.Merge(CrearDataTableMorosos(), false, System.Data.MissingSchemaAction.Ignore);
            CrystalReportSource.ReportDocument.SetDataSource(ds);

            CrystalReportSource.ReportDocument.SetParameterValue("total", hfTotal.Value);
            CrystalReportSource.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);

            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }
        #endregion

        protected void btnAgregarComentario_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtComentario.Text))
                {
                    new cComentarioMorosos(hfIdCC.Value, txtComentario.Text, DateTime.Now, HttpContext.Current.User.Identity.Name);
                    txtComentario.Text = "";
                    txtComentario.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Morosos - " + DateTime.Now + "- " + ex.Message + " - btnAgregarComentario_Click");
                Response.Redirect("MensajeError.aspx");
            }
        }
    }
}