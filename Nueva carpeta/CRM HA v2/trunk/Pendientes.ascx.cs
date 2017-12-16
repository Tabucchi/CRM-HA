using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLL.Negocio;

namespace crm
{
    public partial class Pendientes : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbCantPrecios.Text = cActualizarPrecio.GetPrecios().Count.ToString();

            lbCantOV.Text = cOperacionVenta.GetOV_AConfirmar().Count.ToString();
        }
    }
}