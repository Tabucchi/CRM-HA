using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm.Controles
{
    public partial class CuotasManuales : System.Web.UI.UserControl
    {
        public string monto;

        public string Monto
        {
            get { return monto; }
            set { monto = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { }
        }

        

    }
}