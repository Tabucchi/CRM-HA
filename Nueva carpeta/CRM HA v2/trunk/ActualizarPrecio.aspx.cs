using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;
using log4net;
using DLL.Base_de_Datos;

namespace crm
{
    public partial class ActualizarPrecio : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria != (Int32)eCategoria.Administración)
                    Response.Redirect("Default.aspx");
            }
        }

        #region Botones
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Unidad.aspx?idProyecto=" + Request["idProyecto"].ToString());
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            // Para obtener nombre del proyecto
            cProyecto proyecto = cProyecto.Load(Request["idProyecto"].ToString());

            // DataTable con las unidades del proyecto
            DataTable tabla = new DataTable();
            tabla = cUnidad.SearchExport("0", Request["idProyecto"].ToString(), "1", "supTotal", "", "", "", "");

            if (tabla.Rows.Count > 0)
            {

                List<cUnidad> unidades = tabla.AsEnumerable().Select(m => new cUnidad()
                {
                    SupCubierta = m.Field<string>("supCubierta"),
                    SupSemiDescubierta = m.Field<string>("supSemiDescubierta"),
                    SupDescubierta = m.Field<string>("supDescubierta"),
                    SupTotal = m.Field<string>("supTotal"),
                    Porcentaje = m.Field<decimal>("porcentaje"),
                    PrecioBase = m.Field<Decimal>("precioBase")
                }).ToList();

                string lblSupCubierta = cUnidad.GetTotalSupCubierta(unidades);
                string lblSupBalcon = cUnidad.GetTotalSupSemiDescubierta(unidades);
                string lblSupTerraza = cUnidad.GetTotalSupDescubierta(unidades);
                string lblSupTotal = cUnidad.GetTotalSupTotal(unidades);
                string lblPorcentaje = cUnidad.GetTotalPorcentaje(unidades).ToString();
                string lblPrecioBase = cUnidad.GetTotalPrecioBase(unidades);

                List<DataTable> list = new List<DataTable>();
                list.Add(tabla);
                string filename = proyecto.Descripcion.Trim() + " - Unidades - " + DateTime.Now.ToShortDateString();

                cExcel.DataTableToExcelUnidades(list, filename, lblSupCubierta, lblSupBalcon, lblSupTerraza, lblSupTotal, lblPorcentaje, lblPrecioBase);
            }
        }

        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                bool isOK = true;
                int countUnidades = 0;

                try
                {
                    string path = HttpContext.Current.Request.PhysicalApplicationPath + "Archivos\\ActualizarValor\\" + Path.GetFileName(btnFileUpload.FileName);
                    btnFileUpload.PostedFile.SaveAs(path);

                    DataTable dt = cExcel.ExcelToDataTablePrecios(path);

                    decimal valorNuevo;

                    foreach (DataRow r in dt.Rows)
                    {
                        // Solo se cargan las unidades con valor asignado
                        if (!string.IsNullOrEmpty(r[13].ToString().Trim()))
                        {
                            decimal result;
                            valorNuevo = 0;

                            if (decimal.TryParse(r[13].ToString().Replace(" [$USD]", "").Trim(), out result) && decimal.Parse(r[13].ToString().Replace(" [$USD]", "").Trim()) > 0)
                            {
                                cUnidad u = cUnidad.GetUnidadByProyectoAndUF(Request["idProyecto"].ToString(), r[0].ToString().Trim());

                                if (u != null)
                                {
                                    // Si la unidad tienen una actualizacion pendiente, la actualizo, sino la inserto.
                                    cActualizarPrecio ap = cActualizarPrecio.GetActualizacionByProyectoAndUF(u.CodigoUF, Convert.ToInt32(u.IdProyecto));
                                    if (ap != null)
                                    {
                                        ap.ValorNuevo = decimal.Parse(r[13].ToString().Trim());
                                        ap.ValorActualizacion = decimal.Parse(r[13].ToString().Trim()) - u.PrecioBase;
                                        ap.Save();
                                    }
                                    else
                                    {
                                        string moneda = u.Moneda;

                                        if (moneda == Convert.ToString((Int16)tipoMoneda.Dolar))
                                            valorNuevo = cAuxiliar.RedondearCentenas(Convert.ToDecimal(r[13].ToString().Replace(" [$USD]", "").Trim()));
                                        else
                                            valorNuevo = cAuxiliar.RedondearMillar(Convert.ToDecimal(r[13].ToString().Replace(" [$USD]", "").Trim()));

                                        cActualizarPrecio precio = new cActualizarPrecio(u.CodigoUF, u.IdProyecto, u.PrecioBase, valorNuevo, "Valor", valorNuevo - u.PrecioBase);
                                    }

                                    countUnidades++;

                                }
                            }
                        }
                    }
                }
                catch
                {
                    isOK = false;
                }

                if (isOK)
                {
                    pnlOkExcel.Visible = true;
                    lbMensaje.CssClass = "messageOk";
                    lbMensaje.Text = "Se ha solicitado la actualización de precio/s de " + countUnidades + " unidades. Los mismos serán validados por el administrador.";

                    //Envío de mail avisando de la actualización
                    //cSendMail send = new cSendMail();
                    //send.EnviarActualizacionPrecio();
                }
                else
                {
                    pnlErrorExcel.Visible = true;
                    lbMensajeError.Text = "Se produjo un error con el archivo de Excel ingresado.";
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("ActualizarPrecio - " + DateTime.Now + "- " + ex.Message + " - btnProcesar_Click" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Auxiliares
        public void Exportar()
        {
            try
            {
                #region Variables
                List<cUnidad> unidades = new List<cUnidad>();
                ArrayList erroresExcel = new ArrayList();
                int fila = 2;
                string _nivel = null;
                decimal valorNuevo = 0;
                int countUnidades = 0;
                bool isOK = true;
                string path2 = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Unidades\\" + btnFileUpload.PostedFile.FileName;
                #endregion

                try
                {
                    if (!string.IsNullOrEmpty(btnFileUpload.PostedFile.FileName))
                    {
                        btnFileUpload.SaveAs(path2);
                        DataTable dt2 = cExcel.ExcelToDataTableRead2Sheet(path2);

                        foreach (DataRow r in dt2.Rows)
                        {
                            if (!string.IsNullOrEmpty(r[1].ToString()) || !string.IsNullOrEmpty(r[2].ToString()))
                            {
                                if (!r[2].ToString().Contains("BAULERA") && !r[2].ToString().Contains("COCHERA") && !r[2].ToString().Contains("PB") && !r[2].ToString().Contains("PLANTA") && !r[2].ToString().Contains("SB") && !r[2].ToString().Contains("SUBSUELO") && !r[2].ToString().Contains("PISO"))
                                {
                                    cUnidad unidad = cUnidad.GetUnidadByProyecto(Request["idProyecto"].ToString(), _nivel, r[2].ToString());
                                    string _precio = r[18].ToString().Replace(" [$USD]", "").Trim();

                                    if (unidad.IdEstado == Convert.ToString((Int16)estadoUnidad.Disponible))
                                    {
                                        if (cAuxiliar.IsNumeric(_precio))
                                        {
                                            //Si la unidad tienen una actualizacion pendiente, la actualizo, sino la inserto.
                                            cActualizarPrecio ap = cActualizarPrecio.GetActualizacionByProyectoAndUF(unidad.CodigoUF, Convert.ToInt32(unidad.IdProyecto));
                                            if (ap != null)
                                            {
                                                ap.ValorNuevo = decimal.Parse(r[13].ToString().Trim());
                                                ap.ValorActualizacion = decimal.Parse(r[13].ToString().Trim()) - unidad.PrecioBase;
                                                ap.Save();
                                            }
                                            else
                                            {
                                                if (unidad.GetMoneda == Convert.ToString((Int16)tipoMoneda.Dolar))
                                                    valorNuevo = cAuxiliar.RedondearCentenas(Convert.ToDecimal(_precio));
                                                else
                                                    valorNuevo = cAuxiliar.RedondearMillar(Convert.ToDecimal(_precio));

                                                cActualizarPrecio precio = new cActualizarPrecio(unidad.CodigoUF, unidad.IdProyecto, unidad.PrecioBase, valorNuevo, "Valor", valorNuevo - unidad.PrecioBase);
                                            }

                                            countUnidades++;
                                        }
                                    }
                                }
                                else
                                {
                                    _nivel = r[2].ToString();
                                    if (string.IsNullOrEmpty(_nivel))
                                        erroresExcel.Add("No se específico el nivel de las unidades. <b>Fila:</b> " + fila);
                                }
                            }
                            fila++;
                        }
                    }
                }
                catch
                {
                    isOK = false;
                }

                if (isOK)
                {
                    pnlOkExcel.Visible = true;
                    lbMensaje.CssClass = "messageOk";
                    lbMensaje.Text = "Se ha solicitado la actualización de precio/s de " + countUnidades + " unidades. Los mismos serán validados por el administrador.";

                    //Envío de mail avisando de la actualización
                    //cSendMail send = new cSendMail();
                    //send.EnviarActualizacionPrecio();
                }
                else
                {
                    pnlErrorExcel.Visible = true;
                    lbMensajeError.Text = "Se produjo un error con el archivo de Excel ingresado.";
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("CargaUnidad - " + DateTime.Now + "- " + ex.Message + " - CargarArchivo" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion
    }
}