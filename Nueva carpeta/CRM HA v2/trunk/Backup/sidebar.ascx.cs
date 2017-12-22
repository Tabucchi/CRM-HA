using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class sidebar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Novedades
            List<cNovedad> novedades = cNovedad.GetList();
            if (novedades == null) return;
            if (novedades.Count > 0)
            {
                rptNovedades.DataSource = novedades;
                rptNovedades.DataBind();
            }
         
         lvManuales.DataSource = cManual.GetManualesTop5();
         lvManuales.DataBind();

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                cNovedad novedad = new cNovedad();
                novedad.Descripcion = txtDescripcion.Text;
                novedad.Fecha = Convert.ToDateTime(txtFecha.Text);
                novedad.Usuario = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Id;
                novedad.Save();
            }
            catch (Exception ex)
            {
                string excepcion = ex.ToString();
            }
        }
    }
}