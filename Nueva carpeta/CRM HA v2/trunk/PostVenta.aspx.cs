using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class PostVenta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    #region Combos
                    cbProyecto.DataSource = cProyecto.GetDataTable();
                    cbProyecto.DataValueField = "id";
                    cbProyecto.DataTextField = "descripcion";
                    cbProyecto.DataBind();
                    cbProyecto.Items.Insert(0, new ListItem("Seleccione una obra...", "0"));
                    #endregion
                }
            }
            catch
            {
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            cSendMailPostVenta send = new cSendMailPostVenta();

            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                send.NuevaConsulta(cProyecto.Load(cbProyecto.SelectedValue).Descripcion, txtUF.Text, txtSearch.Text, txtMail.Text, txtTelefono.Text, txtDescripcion.Text);
            }

            Response.Redirect("MensajePostVenta.aspx");
        }
    }
}