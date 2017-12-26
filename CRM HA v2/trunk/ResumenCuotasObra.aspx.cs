using DLL.Negocio;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class ResumenCuotasObra : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime date = DateTime.Now;
                DateTime desde = new DateTime(date.Year, date.Month, 1);
                DateTime hasta = new DateTime(date.Month == 12 ? date.Year + 1 : date.Year, date.Month == 12 ? 1 : date.Month + 1, 1);

                txtFechaDesde.Text = String.Format("{0:dd/MM/yyyy}", desde);
                txtFechaHasta.Text = String.Format("{0:dd/MM/yyyy}", hasta);

                lvResumen.DataSource = cArchivoCuotasObra.Search(String.Format("{0:dd/MM/yyyy}", desde), String.Format("{0:dd/MM/yyyy}", hasta));
                lvResumen.DataBind();
            }
        }

        protected void lkbDescargar_Click(object sender, EventArgs e)
        {
            LinkButton boton = (LinkButton)sender;
            try
            {
                cArchivoCuotasObra archivo = cArchivoCuotasObra.Load(boton.CommandArgument.ToString());

                Response.Clear();
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", archivo.Id));
                Response.ContentType = "application/pdf";
            
                string fileName = "Cuotas a cobrar por obra" + ".pdf";
                string path = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\" + fileName;
                fileArchivo.SaveAs(path);

                // Create a new stream to write to the file
                BinaryWriter Writer = new BinaryWriter(File.OpenWrite(path));

                // Writer raw data                
                Writer.Write(archivo.Archivo);
                Writer.Flush();
                Writer.Close();

                Response.Redirect("~\\Archivos\\" + fileName);
            }
            catch { }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                lvResumen.DataSource = cArchivoCuotasObra.Search(String.Format("{0:dd/MM/yyyy}", txtFechaDesde.Text), String.Format("{0:dd/MM/yyyy}", txtFechaHasta.Text));
                lvResumen.DataBind();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("ResumenCuotasObra - " + DateTime.Now + "- " + ex.Message + " - btnBuscar_Click");
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            lvResumen.DataSource = cArchivoCuotasObra.Search(null, null);
            lvResumen.DataBind();
        }
    }
}