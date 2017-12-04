using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class MensajePostVenta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbText.Text = "Su consulta ha sido enviada correctamente.";
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("PostVenta.aspx");
        }
    }
}