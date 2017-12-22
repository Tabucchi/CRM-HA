using Common.Logging;
using DLL.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class CCCliente : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                //Si el cliente tiene dos o más cuotas sin pagar se bloquea el usuario
                //Verifico cuantas cuotas impagas
                int count = 0;
                foreach (cCuota c in cCuota.GetAllCuotasPendienteByActiva(HttpContext.Current.User.Identity.Name))
                {
                    if (c.FechaVencimiento2.Date < DateTime.Now.Date)
                    {
                        count++;
                    }
                }

                if (count >= 2)
                {
                    Response.Redirect("Aviso.aspx");
                }
                else
                { 
                    CargarCombo();

                    lvClientes.DataSource = cCuentaCorriente.GetCuentaCorrienteByIdCliente(HttpContext.Current.User.Identity.Name, Convert.ToInt16(estadoCuenta_Cuota.Activa));
                    lvClientes.DataBind();

                    ddlEstado.SelectedIndex = 1;
                }
            }
        }

        public void CargarCombo()
        {
            //Estado de pago
            string[] estado = Enum.GetNames(typeof(estadoCuenta_Cuota));
            foreach (string item in estado)
            {
                int value = (int)Enum.Parse(typeof(estadoCuenta_Cuota), item);
                ListItem listItem = new ListItem(item, value.ToString());
                ddlEstado.Items.Add(listItem);
            }
            ListItem ie = new ListItem("Todas", "2");
            ddlEstado.Items.Insert(2, ie);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            lvClientes.DataSource = cCuentaCorriente.GetCuentaCorrienteByIdCliente(HttpContext.Current.User.Identity.Name, Convert.ToInt16(ddlEstado.SelectedValue));
            lvClientes.DataBind();
        }
    }
}