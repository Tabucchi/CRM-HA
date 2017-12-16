using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLL.Negocio;
using log4net;
using System.IO;
using System.Web.Security;

namespace crm
{
    public partial class Proyecto : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lvProyectos.DataSource = cProyecto.GetProyectos();
                    lvProyectos.DataBind();
                    CalcularTotales();
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Proyecto - " + DateTime.Now + "- " + ex.Message + " - Page_Load" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        #region ListView
        protected void lvProyectos_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                cProyecto proyecto = cProyecto.Load(e.CommandArgument.ToString());

                switch (e.CommandName)
                {
                    case "Editar":
                        {
                            txtEditDescripcion.Text = proyecto.Descripcion;
                            hfId.Value = proyecto.Id;
                            ModalEdit.Show();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Proyecto - " + DateTime.Now + "- " + ex.Message + " - lvProyectos_ItemCommand" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        protected void lvProyectos_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria != (Int32)eCategoria.Administración)
                    {
                        LinkButton edit = e.Item.FindControl("btnEditar") as LinkButton;
                        edit.Attributes.CssStyle.Add("visibility", "hidden");

                        pnlAgregarObra.Visible = false;
                        pnlFinalizarProyecto.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Proyecto - " + DateTime.Now + "- " + ex.Message + " - lvProyectos_ItemDataBound" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        public void CalcularTotales()
        {
            try
            {
                decimal valorM2 = 0;

                foreach (ListViewItem item in lvProyectos.Items)
                {
                    Label lbValorM2 = item.FindControl("lbValorM2") as Label;
                    if (!string.IsNullOrEmpty(lbValorM2.Text))
                        valorM2 += Convert.ToDecimal(lbValorM2.Text);
                    else
                        valorM2 += 0;
                }

                Label lbTotalCantUnidades = (Label)lvProyectos.FindControl("lbTotalCantUnidades");
                lbTotalCantUnidades.Text = cUnidad.GetCantUnidadesDisponibles(null).ToString();

                Label lbTotalValorM2 = (Label)lvProyectos.FindControl("lbTotalValorM2");
                lbTotalValorM2.Text = String.Format("{0:#,#0.00}", valorM2);

                Label lbSupTotal = (Label)lvProyectos.FindControl("lbTotalSupTotal");
                lbSupTotal.Text = String.Format("{0:#,#0.00}", cUnidad.GetTotalSupTotal(-1));

                Label lbSupTotalDisponibles = (Label)lvProyectos.FindControl("lbTotalSupTotalDisponibles");
                lbSupTotalDisponibles.Text = String.Format("{0:#,#0.00}", cUnidad.GetTotalSupTotal((Int16)estadoUnidad.Disponible));

                Label lbSupTotalReservados = (Label)lvProyectos.FindControl("lbTotalSupTotalReservados");
                lbSupTotalReservados.Text = String.Format("{0:#,#0.00}", cUnidad.GetTotalSupTotal((Int16)estadoUnidad.Reservado));

                Label lbSupTotalVendidos = (Label)lvProyectos.FindControl("lbTotalSupTotalVendidos");
                lbSupTotalVendidos.Text = String.Format("{0:#,#0.00}", cUnidad.GetTotalSupTotal((Int16)estadoUnidad.Vendido));   
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Proyecto - " + DateTime.Now + "- " + ex.Message + " - CalcularTotales" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Botones
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                var argument = ((Button)sender).CommandArgument;

                switch (argument)
                {
                    case "Nueva":
                        ModalProyecto.Hide();
                        break;
                    case "Editar":
                        ModalEdit.Hide();
                        break;
                    case "Eliminar":
                        modalDelete.Hide();
                        break;
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Proyecto - " + DateTime.Now + "- " + ex.Message + " - btnCerrar_Click" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        /// <summary> Nueva obra </summary>
        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            cProyecto proyecto = new cProyecto();
            proyecto.Descripcion = txtDescripcion.Text;
            proyecto.Papelera = (Int16)papelera.Activo;
            proyecto.Save();
            Response.Redirect("Proyecto.aspx");
        }

        /// <summary> Editar </summary>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cProyecto proyecto = cProyecto.Load(hfId.Value);
            proyecto.Descripcion = txtEditDescripcion.Text;
            proyecto.Save();
            Response.Redirect("Proyecto.aspx");
        }

        //Se abre el modal para confirmar la eliminación de la obra
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                cProyecto proyecto = cProyecto.Load(hfId.Value);
                lbDeleteProyecto.Text = proyecto.Descripcion;
                hfId.Value = proyecto.Id;
                modalDelete.Show();
            }
            catch (Exception ex)
            {

                log4net.Config.XmlConfigurator.Configure();
                log.Error("Proyecto - " + DateTime.Now + "- " + ex.Message + " - btnDelete_Click" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        /// <summary> Eliminar obra </summary>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string eu = cEmpresaUnidad.GetUnidadByIdProyecto(hfId.Value);

            if (string.IsNullOrEmpty(eu))
            {
                cProyecto proyecto = cProyecto.Load(hfId.Value);
                proyecto.Papelera = (Int16)papelera.Eliminado;
                proyecto.Save();
                Response.Redirect("Proyecto.aspx");
            }
            else
            {
                pnlMensajeError.Visible = true;
            }
        }

        protected void btnResumen_Click(object sender, EventArgs e)
        {
            Response.Redirect("ResumenObra.aspx");
        }
        #endregion
    }
}