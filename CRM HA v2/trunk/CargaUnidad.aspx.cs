using DLL.Negocio;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class CargaUnidad : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /*if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria != (Int32)eCategoria.Administración)
                    Response.Redirect("Default.aspx");*/

                #region Combo
                cbMoneda.DataSource = cCampoGenerico.CargarComboMoneda();
                cbMoneda.DataBind();

                cbUnidadFuncional.DataSource = cCampoGenerico.GetListaTipoUnidad();
                cbUnidadFuncional.DataValueField = "id";
                cbUnidadFuncional.DataTextField = "descripcion";
                cbUnidadFuncional.DataBind();
                ListItem tu = new ListItem("Seleccione un tipo de unidad funcional", "0");
                cbUnidadFuncional.Items.Insert(0, tu);
                #endregion
            }
        }

        #region Carga unidades
        protected void btnExportar_Click(object sender, EventArgs e)
        {
            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Reportes\\";
            string filename = "Formulario_Excel.xlsx";

            Response.ContentType = "APPLICATION/pdf";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);
            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(rutaURL + filename);
            Response.End();
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            CargarArchivo();
        }

        /// <summary>
        /// <remarks>
        /// Se lee el archivo. Se crea un objeto cUnidad y se agrega a un List. 
        /// En caso de errores, los mismos se agregan a un List de errores
        /// </remarks>
        /// </summary>
        public void CargarArchivo()
        {
            try
            {
                #region Variables
                List<cUnidad> unidades = new List<cUnidad>();
                ArrayList erroresExcel = new ArrayList();
                int fila = 2;
                int countUF = 1;
                string _nivel = null;
                decimal auxSupTotal = 0;
                decimal supTotalObra = 0;
                decimal proporcionTerraza = Convert.ToDecimal(txtProporcion.Text);
                string path2 = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Unidades\\" + fileArchivo2.PostedFile.FileName;
                cProyecto proyecto = cProyecto.Load(Request["idProyecto"].ToString());
                #endregion

                if (!string.IsNullOrEmpty(fileArchivo2.PostedFile.FileName))
                {
                    fileArchivo2.SaveAs(path2);
                    DataTable dt2 = cExcel.ExcelToDataTableRead2Sheet(path2);

                    foreach (DataRow r in dt2.Rows)
                    {
                        if (!string.IsNullOrEmpty(r[1].ToString()) || !string.IsNullOrEmpty(r[2].ToString()))
                        {
                            if (!r[2].ToString().Contains("BAULERA") && !r[2].ToString().Contains("COCHERA") && !r[2].ToString().Contains("PB") && !r[2].ToString().Contains("PLANTA") && !r[2].ToString().Contains("SB") && !r[2].ToString().Contains("SUBSUELO") && !r[2].ToString().Contains("PISO"))
                            {
                                cUnidad unidad = new cUnidad();
                                unidad.IdProyecto = Request["idProyecto"].ToString();
                                unidad.Moneda = cAuxiliar.GetMonedaByDescripcion(cbMoneda.SelectedValue);

                                if (cbUnidadFuncional.SelectedValue != "0")
                                    unidad.UnidadFuncional = cCampoGenerico.Load(cbUnidadFuncional.SelectedValue, Tablas.tTipoUnidad).Descripcion;
                                else
                                    unidad.UnidadFuncional = "-";

                                if (cAuxiliar.IsNumeric(r[1].ToString()) && countUF == 1)
                                {
                                    unidad.CodigoUF = r[1].ToString();
                                    countUF = Convert.ToInt16(r[1].ToString());
                                }
                                else
                                    unidad.CodigoUF = countUF.ToString();
                                countUF++;

                                unidad.Nivel = _nivel != null ? _nivel : "-";
                                unidad.NroUnidad = !string.IsNullOrEmpty(r[2].ToString()) ? r[2].ToString() : "-";

                                #region Unidad Funcional / Ambiente
                                if (!string.IsNullOrEmpty(r[3].ToString()))
                                {
                                    switch (r[3].ToString())
                                    {
                                        case "Cochera":
                                            unidad.UnidadFuncional = "Cochera";
                                            unidad.Ambiente = "-";
                                            break;
                                        case "COCHERA":
                                            unidad.UnidadFuncional = "Cochera";
                                            unidad.Ambiente = "-";
                                            break;
                                        case "Baulera":
                                            unidad.UnidadFuncional = "Baulera";
                                            unidad.Ambiente = "-";
                                            break;
                                        case "Terreno":
                                            unidad.UnidadFuncional = "Terreno";
                                            unidad.Ambiente = "-";
                                            break;
                                        default:
                                            unidad.Ambiente = r[3].ToString().Replace(" AMB", "").Replace("IENTE", "");
                                            break;
                                    }
                                }
                                else
                                {
                                    erroresExcel.Add("No se específico la unidad funcional o ambiente. <b>Fila:</b> " + fila + " - <b>Columna:</b> 4");
                                }
                                #endregion

                                #region Superficies
                                unidad.SupSemiDescubierta = !string.IsNullOrEmpty(r[5].ToString()) ? r[5].ToString() : "0";
                                if (!string.IsNullOrEmpty(r[4].ToString()))
                                {
                                    unidad.SupCubierta = r[4].ToString();
                                }
                                else
                                    erroresExcel.Add("El valor de la superficie cubierta esta vacía. <b>Fila:</b> " + fila + " - <b>Columna:</b> 5");

                                if (!string.IsNullOrEmpty(r[6].ToString()) && r[6].ToString() != "-")
                                {
                                    if (proporcionTerraza != 0)
                                        unidad.SupDescubierta = Convert.ToString(Convert.ToDecimal(r[6].ToString()) * (proporcionTerraza / 100));
                                    else
                                        unidad.SupDescubierta = r[6].ToString();
                                }
                                else
                                    erroresExcel.Add("El valor de la superficie descubierta esta vacía. <b>Fila:</b> " + fila + " - <b>Columna:</b> 6");
                                //else
                                //    unidad.SupDescubierta = "0";

                                if (!string.IsNullOrEmpty(r[7].ToString()))
                                {
                                    if (cAuxiliar.IsNumeric(r[7].ToString()))
                                    {
                                        unidad.SupTotal = r[7].ToString();
                                        supTotalObra += Convert.ToDecimal(r[7].ToString());
                                    }
                                    else
                                    {
                                        decimal sup = Convert.ToDecimal(Convert.ToDecimal(unidad.SupCubierta) + Convert.ToDecimal(unidad.SupSemiDescubierta) + Convert.ToDecimal(unidad.SupDescubierta));
                                        unidad.SupTotal = sup.ToString();
                                        supTotalObra += Convert.ToDecimal(unidad.SupCubierta) + Convert.ToDecimal(unidad.SupSemiDescubierta) + Convert.ToDecimal(unidad.SupDescubierta);
                                    }
                                }
                                else
                                    erroresExcel.Add("El valor de la superficie total esta vacía. <b>Fila:</b> " + fila + " - <b>Columna:</b> 8");
                                #endregion

                                #region Precio/Estado
                                try
                                {
                                    if (r[18].ToString() != "RESERVADO" && r[18].ToString() != "RESERVADA" && r[18].ToString() != "VENDIDO" && r[18].ToString() != "VENDIDA" && r[18].ToString() != "PORTERIA")
                                    {
                                        if (cbMoneda.SelectedValue == Convert.ToString((Int16)tipoMoneda.Dolar))
                                        {
                                            unidad.PrecioBase = cAuxiliar.RedondearCentenas(Convert.ToDecimal(r[18].ToString().Replace("USD", "").Replace("$", "").Replace("[]", "")));
                                            unidad.PrecioBaseOriginal = cAuxiliar.RedondearCentenas(Convert.ToDecimal(r[18].ToString().Replace("USD", "").Replace("$", "").Replace("[]", "")));
                                        }
                                        else
                                        {
                                            unidad.PrecioBase = cAuxiliar.RedondearMillar(Convert.ToDecimal(r[18].ToString().Replace("USD", "").Replace("$", "").Replace("[]", "")));
                                            unidad.PrecioBaseOriginal = cAuxiliar.RedondearMillar(Convert.ToDecimal(r[18].ToString().Replace("USD", "").Replace("$", "").Replace("[]", "")));
                                        }
                                    }
                                }
                                catch
                                {
                                    erroresExcel.Add("Valor incorrecto del precio o estado de la unidad. <b>Fila:</b> " + fila + " - <b>Columna:</b> 19");
                                }

                                switch (r[18].ToString())
                                {
                                    case "RESERVADO":
                                        unidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Reservado);
                                        break;
                                    case "RESERVADA":
                                        unidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Reservado);
                                        break;
                                    case "VENDIDO":
                                        unidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Vendido);
                                        break;
                                    case "VENDIDA":
                                        unidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Vendido);
                                        break;
                                    case "PORTERIA":
                                        unidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Porteria);
                                        break;
                                    default:
                                        unidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Disponible);
                                        break;
                                }
                                #endregion

                                auxSupTotal = Convert.ToDecimal(unidad.SupCubierta) + Convert.ToDecimal(unidad.SupSemiDescubierta) + Convert.ToDecimal(unidad.SupDescubierta);

                                if (Convert.ToDecimal(unidad.SupTotal) != auxSupTotal)
                                    erroresExcel.Add("La suma de la sup. cubierta y descubierta no coincide con la sup. total. <b>Fila:</b> " + fila);

                                unidad.Porcentaje = 0;
                                unidad.Papelera = (Int16)papelera.Activo;
                                unidades.Add(unidad);
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


                    if (cUnidad.GetUnidadesByIdProyecto(Request["idProyecto"].ToString()).Count == 0)
                        CrearUnidades(unidades, erroresExcel, supTotalObra);
                    else
                        CrearUnidadesModificacion(unidades, erroresExcel, supTotalObra);
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

        #region Modificación de Unidades
        /// <summary>
        /// <remarks>
        /// Se recorre el List con los objetos cUnidades y se guardan en la base.
        /// En caso de errores, se muestra por pantalla los errores que estan guardados en el List de errores.
        /// </remarks>
        /// </summary>
        public void CrearUnidades(List<cUnidad> unidades, ArrayList erroresExcel, decimal supTotalObra)
        {
            try
            {
                #region Variables
                decimal porcentaje = 0;
                List<cUnidad> unidadesByProyecto = null;
                int idUnidad = 0;
                #endregion

                unidadesByProyecto = cUnidad.GetUnidadesByIdProyecto(Request["idProyecto"].ToString());

                if (erroresExcel.Count == 0)
                {
                    if (unidades.Count != 0)
                    {
                        foreach (cUnidad u in unidades)
                        {
                            #region Se calcula el porcentajes de cada unidad
                            porcentaje = (Convert.ToDecimal(u.SupTotal) * 100) / supTotalObra;

                            u.Porcentaje = Math.Round(porcentaje, 4);
                            #endregion
                            u.IdUsuario = "-1";
                            idUnidad = u.Save();

                            #region Se guarda la Sup. Total del proyecto
                            cProyecto proyecto = cProyecto.Load(u.IdProyecto);
                            proyecto.SupTotal = supTotalObra;
                            proyecto.Save();
                            #endregion

                            #region Se registra el ingreso de la unidad
                            cHistorial historial = new cHistorial(DateTime.Now, "Ingreso al sistema", 0, 0, u.CodigoUF, u.NroUnidad, u.IdEstado, u.IdEstado, HttpContext.Current.User.Identity.Name, u.IdProyecto);
                            historial.Save();
                            #endregion
                        }
                    }

                    pnlErrorExcel.Visible = false;
                    pnlOkExcel.Visible = true;
                    lbMensaje.Text = "Se registraron correctamente " + unidades.Count + " unidades.";
                }
                else
                {
                    #region Se listan los errores, surgidos al momento de la carga
                    StringBuilder sb = new StringBuilder();
                    int index = 0;
                    pnlErrorExcel.Visible = true;
                    pnlOkExcel.Visible = false;

                    foreach (string error in erroresExcel)
                    {
                        sb.Append("<div style=\"margin-top: 5px;\">- <span id=\"ctl00_ContentPlaceHolder1_Label" + index + "\" style=\"font-size: 16px;\">" + error + "</span></div>");
                        index++;
                    }

                    litMarkup.Text = sb.ToString();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("CargaUnidad - " + DateTime.Now + "- " + ex.Message + " - CargarArchivo" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        public void CrearUnidadesModificacion(List<cUnidad> unidades, ArrayList erroresExcel, decimal supTotalObra)
        {
            try
            {
                List<cUnidad> unidadesByProyecto = cUnidad.GetUnidadesByIdProyecto(Request["idProyecto"].ToString());
                if (unidades.Count == unidadesByProyecto.Count)
                    UnificacionUnidades(unidades, erroresExcel, supTotalObra);
                else
                    InsercionUnidades(unidades, erroresExcel, supTotalObra);
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("CargaUnidad - " + DateTime.Now + "- " + ex.Message + " - CrearUnidadesModificacion" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        public void UnificacionUnidades(List<cUnidad> unidades, ArrayList erroresExcel, decimal supTotalObra)
        {
            try
            {
                #region Variables
                decimal porcentaje = 0;
                List<cUnidad> unidadesByProyecto = null;
                int i = 0;
                #endregion

                unidadesByProyecto = cUnidad.GetUnidadesByIdProyecto(Request["idProyecto"].ToString());

                if (erroresExcel.Count == 0)
                {
                    if (unidades.Count != 0)
                    {
                        foreach (cUnidad u in unidades)
                        {
                            #region Modificaciones en las unidades ya existentes
                            bool flag = false;
                            if (u.Ambiente != unidadesByProyecto[i].Ambiente || u.SupCubierta != unidadesByProyecto[i].SupCubierta || u.SupSemiDescubierta != unidadesByProyecto[i].SupSemiDescubierta ||
                                    u.SupDescubierta != unidadesByProyecto[i].SupDescubierta || u.SupTotal != unidadesByProyecto[i].SupTotal)
                                flag = true;

                            if (flag == true && u.IdEstado == Convert.ToString((Int16)estadoUnidad.Disponible))
                            {
                                unidadesByProyecto[i].Ambiente = u.Ambiente;
                                unidadesByProyecto[i].SupCubierta = u.SupCubierta;
                                unidadesByProyecto[i].SupSemiDescubierta = u.SupSemiDescubierta;
                                unidadesByProyecto[i].SupDescubierta = u.SupDescubierta;
                                unidadesByProyecto[i].SupTotal = u.SupTotal;
                                unidadesByProyecto[i].PrecioBase = u.PrecioBase;
                                if (u.Ambiente == "0")
                                {
                                    unidadesByProyecto[i].IdEstado = Convert.ToString((Int16)estadoUnidad.Modificado);
                                    unidadesByProyecto[i].Porcentaje = 0;
                                    cHistorial historial = new cHistorial(DateTime.Now, "Unidad modificada", 0, 0, u.CodigoUF, u.NroUnidad, u.IdEstado, u.IdEstado, HttpContext.Current.User.Identity.Name, u.IdProyecto);
                                    historial.Save();
                                }

                                porcentaje = (Convert.ToDecimal(u.SupTotal) * 100) / supTotalObra;
                                unidadesByProyecto[i].Porcentaje = Math.Round(porcentaje, 4);
                                unidadesByProyecto[i].IdUsuario = "-1";
                                unidadesByProyecto[i].Save();
                            }
                            #endregion

                            i++;
                        }
                    }

                    foreach (cUnidad u in unidadesByProyecto)
                    {
                        u.Porcentaje = (Convert.ToDecimal(u.SupTotal) * 100) / supTotalObra;
                        u.Save();
                    }

                    pnlErrorExcel.Visible = false;
                    pnlOkExcel.Visible = true;
                    lbMensaje.Text = "Se registraron las modificaciones correctamente.";
                }
                else
                {
                    #region Se listan los errores, surgidos al momento de la carga
                    StringBuilder sb = new StringBuilder();
                    int index = 0;
                    pnlErrorExcel.Visible = true;
                    pnlOkExcel.Visible = false;

                    foreach (string error in erroresExcel)
                    {
                        sb.Append("<div style=\"margin-top: 5px;\">- <span id=\"ctl00_ContentPlaceHolder1_Label" + index + "\" style=\"font-size: 16px;\">" + error + "</span></div>");
                        index++;
                    }

                    litMarkup.Text = sb.ToString();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("CargaUnidad - " + DateTime.Now + "- " + ex.Message + " - UnificacionUnidades" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        public void InsercionUnidades(List<cUnidad> unidades, ArrayList erroresExcel, decimal supTotalObra)
        {
            try
            {
                #region Variables
                List<cUnidad> unidadesByProyecto = null;
                int indexCodUf = 0;
                int i = 0;
                #endregion

                unidadesByProyecto = cUnidad.GetUnidadesByIdProyecto(Request["idProyecto"].ToString());

                if (erroresExcel.Count == 0)
                {
                    if (unidades.Count != 0)
                    {
                        foreach (cUnidad u in unidades)
                        {
                            if (unidades.Count != unidadesByProyecto.Count)
                            {
                                #region Se agregan nuevas unidades o desdoblamiento de unidades existentes
                                indexCodUf = unidadesByProyecto.Count + 1;

                                bool flag = false;
                                if (u.Ambiente != unidadesByProyecto[i].Ambiente || u.SupCubierta != unidadesByProyecto[i].SupCubierta || u.SupSemiDescubierta != unidadesByProyecto[i].SupSemiDescubierta ||
                                        u.SupDescubierta != unidadesByProyecto[i].SupDescubierta || u.SupTotal != unidadesByProyecto[i].SupTotal)
                                    flag = true;

                                if (flag == true)
                                {
                                    if (cUnidad.GetUnidadByProyecto(u.IdProyecto, u.Nivel, u.NroUnidad) != null)
                                    {
                                        unidadesByProyecto[i].NroUnidad = u.NroUnidad;
                                        unidadesByProyecto[i].Nivel = u.Nivel;
                                        unidadesByProyecto[i].Ambiente = u.Ambiente;
                                        unidadesByProyecto[i].SupCubierta = u.SupCubierta;
                                        unidadesByProyecto[i].SupSemiDescubierta = u.SupSemiDescubierta;
                                        unidadesByProyecto[i].SupDescubierta = u.SupDescubierta;
                                        unidadesByProyecto[i].SupTotal = u.SupTotal;
                                        unidadesByProyecto[i].Porcentaje = (Convert.ToDecimal(u.SupTotal) * 100) / supTotalObra;
                                        unidadesByProyecto[i].Save();

                                        #region Se registra el ingreso de la unidad
                                        cHistorial historial = new cHistorial(DateTime.Now, "Unidad modificada", 0, 0, u.CodigoUF, u.NroUnidad, u.IdEstado, u.IdEstado, HttpContext.Current.User.Identity.Name, u.IdProyecto);
                                        historial.Save();
                                        #endregion
                                    }
                                    else
                                    {
                                        cUnidad newUnidad = new cUnidad();
                                        newUnidad.IdProyecto = Request["idProyecto"].ToString();
                                        newUnidad.UnidadFuncional = u.UnidadFuncional;
                                        newUnidad.NroUnidad = u.NroUnidad;
                                        newUnidad.Nivel = u.Nivel;
                                        newUnidad.CodigoUF = indexCodUf.ToString();
                                        newUnidad.Ambiente = u.Ambiente;
                                        newUnidad.SupCubierta = u.SupCubierta;
                                        newUnidad.SupSemiDescubierta = u.SupSemiDescubierta;
                                        newUnidad.SupDescubierta = u.SupDescubierta;
                                        newUnidad.SupTotal = u.SupTotal;
                                        newUnidad.PrecioBase = u.PrecioBase;
                                        newUnidad.PrecioBaseOriginal = u.PrecioBaseOriginal;
                                        newUnidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Disponible);
                                        newUnidad.Porcentaje = (Convert.ToDecimal(u.SupTotal) * 100) / supTotalObra;
                                        newUnidad.Moneda = u.Moneda;
                                        newUnidad.Papelera = (Int16)papelera.Activo;
                                        newUnidad.IdUsuario = "-1";
                                        newUnidad.Save();
                                        unidadesByProyecto.Add(newUnidad);

                                        #region Se registra el ingreso de la unidad
                                        cHistorial historial = new cHistorial(DateTime.Now, "Ingreso al sistema", 0, 0, newUnidad.CodigoUF, newUnidad.NroUnidad, newUnidad.IdEstado, newUnidad.IdEstado, HttpContext.Current.User.Identity.Name, newUnidad.IdProyecto);
                                        historial.Save();
                                        #endregion
                                    }
                                }
                                #endregion
                            }

                            i++;
                        }
                    }

                    pnlErrorExcel.Visible = false;
                    pnlOkExcel.Visible = true;
                    lbMensaje.Text = "Se registraron las modificaciones correctamente.";
                }
                else
                {
                    #region Se listan los errores, surgidos al momento de la carga
                    StringBuilder sb = new StringBuilder();
                    int index = 0;
                    pnlErrorExcel.Visible = true;
                    pnlOkExcel.Visible = false;

                    foreach (string error in erroresExcel)
                    {
                        sb.Append("<div style=\"margin-top: 5px;\">- <span id=\"ctl00_ContentPlaceHolder1_Label" + index + "\" style=\"font-size: 16px;\">" + error + "</span></div>");
                        index++;
                    }

                    litMarkup.Text = sb.ToString();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("CargaUnidad - " + DateTime.Now + "- " + ex.Message + " - InsercionUnidades" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Botones
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Unidad.aspx?idProyecto=" + Request["idProyecto"].ToString());
        }
        #endregion
    }
}