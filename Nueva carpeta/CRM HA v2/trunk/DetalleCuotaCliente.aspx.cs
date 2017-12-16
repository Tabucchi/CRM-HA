using Common.Logging;
using DLL.Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class DetalleCuotaCliente : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    cCuentaCorriente cc = cCuentaCorriente.GetCuentaCorrienteById(Request["idCC"].ToString());
                    lblCliente.Text = cc.GetEmpresa;
                    lblProyecto.Text = cc.GetUnidad;
                     lblUnidad.Text = cc.UnidadFuncional;
                     lblTotal.Text = "$ " + cc.GetTotal;
                     lblSaldo.Text = "$ " + cc.Saldo;
                     lblFormaPago.Text = cc.GetFormaPago;
                     //lblEstado.Text = cc.GetEstado;
                     hfCC.Value = cc.Id;

                    if (cc.FormaPago == Convert.ToInt16(formaDePago.Cuotas).ToString())
                    {
                        pnlCuotas.Visible = true;
                        pnlAnticipo.Visible = true;
                        lblAnticipo.Text = "$ " + cc.Anticipo;

                        //Verifica el estado de la cuenta corriente, si se hizo algún pago o se anulo.
                        VerificarEstadoCC(cc);

                        lvCuotas.DataSource = cCuota.GetCuotas(Request["idCC"].ToString());
                        lvCuotas.DataBind();
                    }
                    else
                    {
                        foreach (cCuota cuota in cCuota.GetCuotas(Request["idCC"].ToString()))
                        {
                            hfCuota.Value = cuota.Id;
                            lblFecha.Text = String.Format("{0:dd/MM/yyyy}", cuota.Fecha);
                            lblVencimiento1.Text = "$ " + cuota.Vencimiento1;
                            lblVencimiento2.Text = "$ " + cuota.Vencimiento2;
                            lblVariacionCAC.Text = cuota.VariacionCAC.ToString() + " %";
                            if (cuota.VariacionCAC == 0) lbMensajeCAC.Visible = true;
                            lblComision.Text = cuota.Comision + " %";

                            lblRcibo.Text = cuota.GetRecibo;
                        }

                        pnlUnPago.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Cliente - " + DateTime.Now + "- " + ex.Message + " - Page_Load");
                Response.Redirect("MensajeError.aspx");
            }
        }
        
        public void VerificarEstadoCC(cCuentaCorriente cc)
        {
            int cantCuotaPagas = 0;
            foreach (cCuota cuota in cCuota.GetCuotas(Request["idCC"].ToString()))
            {
                if (cuota.Estado == Convert.ToInt16(estadoCuenta_Cuota.Pagado))
                    cantCuotaPagas++;
            }

            if (cantCuotaPagas == cc.CantCuotas)
            {
                cc.IdEstado = Convert.ToInt16(estadoCuenta_Cuota.Pagado);
                cc.Save();
                lblEstado.Text = cc.GetEstado;
            }
        }
        

        protected void btnDatosPersonales_Click(object sender, EventArgs e)
        {
            cEmpresa empresa = cEmpresa.Load(HttpContext.Current.User.Identity.Name);
            lbDatosNombre.Text = empresa.Nombre;
            lbDatosDireccion.Text = empresa.Direccion;
            lbDatosTelefono.Text = empresa.Telefono;
            lbDatosCuit.Text = empresa.Cuit;
            lbMail.Text = empresa.Mail;

            ModalPopupExtender3.Show();
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            ModalPopupExtender3.Hide();
        }

        #region Registro Pago
        protected void btnCargarRegistroPago_Click(object sender, EventArgs e)
        {
            try
            {
                int idImagen = 0;

                cRegistroPago registro = new cRegistroPago();
                registro.FechaPago = Convert.ToDateTime(txtRegistroFecha.Text);
                registro.Monto = Convert.ToDecimal(txtRegistroMonto.Text.Replace(".",","));
                registro.Sucursal = txtRegistroSucursal.Text;
                registro.Transaccion = txtRegistroTransaccion.Text;

                #region Imagen
                try
                {
                    string path = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\ImagenCuota\\" + HttpContext.Current.User.Identity.Name + ".jpg";
                    fileArchivo.SaveAs(path);

                    FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    BinaryReader reader = new BinaryReader(stream);
                    byte[] file = reader.ReadBytes((int)stream.Length);

                    cImagenCuota imagen = new cImagenCuota();
                    imagen.IdCC = Convert.ToInt32(Request["idCC"].ToString());
                    imagen.IdCuota = "-1";
                    imagen.Descripcion = "";
                    imagen.Imagen = file;
                    imagen.Papelera = 1;
                    stream.Close();
                    idImagen = imagen.Save();

                    File.Delete(HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\ImagenCuota\\" + HttpContext.Current.User.Identity.Name + ".jpg");
                }
                catch{
                    lbMensajeImagenNo.Visible = true;
                }
                #endregion

                registro.IdEstado = (Int16)estadoCuenta_Cuota.Validar;
                registro.IdImagen = idImagen;
                registro.IdEmpresa = cEmpresa.Load(HttpContext.Current.User.Identity.Name).Id;
                registro.IdCC = Convert.ToInt32(Request["idCC"].ToString());
                registro.Nro = 1;
                registro.FormaPago = (Int16)formaDePago.UnPago;
                registro.Save();

                lbMensaje.Visible = true;
                //MAIL
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Cliente - " + DateTime.Now + "- " + ex.Message + " - Pago");
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        protected void btnPago_Click(object sender, EventArgs e)
        {
            ModalPopupExtender4.Show();
        }

        #region Adelanto de cuotas
        protected void btnAdelanto_Click(object sender, EventArgs e)
        {
            //lvRegistroCuotasPago.DataSource = null;
            //lvRegistroCuotasPago.DataBind();
            ModalPopupExtender1.Show();
            //ModalPopupExtender5.Show();
        }

        /*
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            int idImagen = 0;
            int idRegistro;
            
            cRegistroPago registro = new cRegistroPago();
            registro.FechaPago = Convert.ToDateTime(txtRegistroFechaCuota.Text);
            registro.Monto = Convert.ToDecimal(txtRegistroMontoCuota.Text);
            registro.Sucursal = txtRegistroSucursalCuota.Text;
            registro.Transaccion = txtRegistroTransaccionCuota.Text;

            #region Imagen
            try
            {
                string path = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\ImagenCuota\\" + HttpContext.Current.User.Identity.Name + ".jpg";
                fileArchivoCuotas.SaveAs(path);

                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(stream);
                byte[] file = reader.ReadBytes((int)stream.Length);

                cImagenCuota imagen = new cImagenCuota();
                imagen.IdCC = Convert.ToInt32(Request["idCC"].ToString());
                imagen.IdCuota = "-1";
                imagen.Descripcion = "";
                imagen.Imagen = file;
                imagen.Papelera = 1;
                stream.Close();
                idImagen = imagen.Save();
            }
            catch
            {
                lbMensajeImagenNo.Visible = true;
            }
            #endregion

            registro.IdEstado = (Int16)estadoCuenta_Cuota.Validar;
            registro.IdImagen = idImagen;
            registro.IdEmpresa = cEmpresa.Load(HttpContext.Current.User.Identity.Name).Id;
            registro.IdCC = Convert.ToInt32(Request["idCC"].ToString());
            registro.Nro = 1;
            registro.FormaPago = (Int16)formaDePago.Cuotas;
            idRegistro = registro.Save();
            
            hfListIdRegistro.Value += idRegistro + ",";

            #region Limpiar Campos
            txtRegistroFechaCuota.Text = "";
            txtRegistroMontoCuota.Text = "";
            txtRegistroSucursalCuota.Text = "";
            txtRegistroTransaccionCuota.Text = "";
            #endregion

            lvRegistroCuotasPago.DataSource = cRegistroPago.GetRegistrosByIds(Request["idCC"].ToString(), hfListIdRegistro.Value);
            lvRegistroCuotasPago.DataBind();
            ModalPopupExtender1.Show();
        }
        */

        /*protected void btnCargarRegistroCuotaPago_Click(object sender, EventArgs e)
        {
            List<cRegistroPago> registros = cRegistroPago.GetRegistrosByIds(Request["idCC"].ToString(), hfListIdRegistro.Value);
            Int16 count = 1;
            foreach(cRegistroPago r in registros){
                r.Nro = count;
                r.Save();
                count++;
            }

            hfListIdRegistro.Value = "";
            lbMensaje.Visible = true;
        }*/

        protected void lvRegistroCuotasPago_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "CancelarRegistro":
                    cRegistroPago registro = cRegistroPago.Load(e.CommandArgument.ToString());
                    cRegistroPago.GetCancelarRegistro(registro.Id);
                    cImagenCuota.GetCancelarImagen(registro.IdImagen.ToString());
                    //lvRegistroCuotasPago.DataSource = cRegistroPago.GetRegistrosByIdCC(Request["idCC"].ToString());
                    //lvRegistroCuotasPago.DataBind();
                    ModalPopupExtender1.Show();
                    break;
            }
        }

        protected void btnCerrarRegistroCuotaPago_Click(object sender, EventArgs e)
        {
            /*List<cRegistroPago> registros = cRegistroPago.GetRegistrosByIdCC(Request["idCC"].ToString());
            Int16 count = 1;
            foreach (cRegistroPago r in registros)
            {
                cRegistroPago.GetCancelarRegistro(r.Id);
                cImagenCuota.GetCancelarImagen(r.IdImagen.ToString());
            }*/
            ModalPopupExtender1.Hide();
        }
        #endregion        
    }
}