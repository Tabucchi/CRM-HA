using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class MensajeError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  

            if (ex != null)
            {
                if (ex.InnerException != null)
                {
                    titulo.InnerText = ex.InnerException.Message;

                    string[] a = ex.InnerException.StackTrace.Split('\r');
                    detalle.InnerText = a[0];

                    log4net.Config.XmlConfigurator.Configure();
                    log.Error("Pedidos - " + DateTime.Now + "- " + ex.Message + " - Page_Load" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                }
            }
        }
        
    }
}