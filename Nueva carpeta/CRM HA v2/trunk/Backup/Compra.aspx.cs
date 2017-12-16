using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Web;
using CrystalDecisions.CrystalReports.Engine;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;

public partial class Compra : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lvNuevaCompra.Visible = true;
            Int16 usuario = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).IdCategoria;
            lbUsuario.Text = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Nombre;
            Session["aux"] = 0;

            List<cCompra> aux = cCompra.GetCompras(Request["Id"].ToString()); //busco si el pedido si tiene una compra asociada        

            if (Request["Id"].ToString() == "0" || aux.Count == 0 || aux == null) //Nueva Compra
            {
                if (aux.Count != 0)//Se ocultan los label
                {
                    pnlTicketNroCompra.Visible = true;
                    lbTicket.Text = Request["Id"].ToString();
                    lbTituloNroCompra.Visible = true;
                    lbNroCompra.Visible = false;
                }
                else
                {
                    pnlTicketNroCompra.Visible = false;
                }

                if (usuario == 1) //categoria 1 = administración
                {
                    pnlTotales.Visible = true;
                    pnlEnviarCotizacion.Visible = true;
                }

                pnlEnviarCotizacion.Visible = false;
                btnAprobado.Visible = false;
                btnStock.Visible = false;
                btnEntregado.Visible = false;
                btnRechazar.Visible = false; lbFecha.Text = String.Format("{0:d/M/yyyy}", DateTime.Today);
                lbEstado.Text = "Nuevo";
                CargarComboEmpresa(Request["Id"].ToString());
                Session["ListaItems"] = cItem.GetItems(null);
                lvNuevaCompra.DataSource = cItem.GetItems(null);
                lvNuevaCompra.DataBind();
            }
            else //Compra existente
            {
                cCompra compra = cCompra.GetCompraNro(Request["Id"].ToString(), null);
                btnSolicitarCotizacion.Visible = false;
                btnGuardar.Visible = true;

                if (usuario == 1) // categoria 1 = administración
                {
                    btnRechazar.Visible = true;
                    lbTotalProveedor.Text = compra.TotalProveedor;
                    lbTotalCliente.Text = compra.TotalCliente;
                    pnlTotales.Visible = true;
                    pnlEnviarCotizacion.Visible = true;
                }

                Session["ListaItems"] = cItem.GetItems(compra.Id); //se guardan los items de la compra, para actualizar sin trabajar sin la base de datos.
                CargarComboEmpresa(compra.IdEmpresa);
                CargarComboCliente(compra.IdEmpresa);
                CargarComboClienteEnvio(compra.IdEmpresa);
                //Se carga el ListView
                lvNuevaCompra.DataSource = cItem.GetItems(compra.Id);
                lvNuevaCompra.DataBind();

                if (compra.IdPedido != "0") //cuando el pedido no esta asociado a una compra
                    lbTicket.Text = compra.IdPedido;
                else
                {
                    lbTituloTicket.Visible = false;
                    lbTicket.Visible = false;
                    lkbVer.Visible = false;
                }

                if (compra.Iva == 0)
                    chbIva.Checked = false; //Iva incluido
                else
                    chbIva.Checked = true; //+ Iva

                pnlTicketNroCompra.Visible = true;
                pnlFiltro.Visible = false;
                lbNroCompra.Text = compra.Id;
                if (compra.GetEmpresa != "")
                    lbEmpresa.Text = compra.GetEmpresa;
                else
                    lbEmpresa.Text = "-";

                if (compra.GetCliente != "")
                    lbCliente.Text = "(" + compra.GetCliente + ")";

                lbUsuario.Text = cUsuario.Load(compra.IdUsuario).Nombre;
                lbFecha.Text = String.Format("{0:d/M/yyyy}", compra.Fecha);

                lbEstado.Text = compra.GetEstado;
                CargarComboEmpresa(null);

                //Se ocultan los textBox
                cbEmpresa.Visible = false;
                cbCliente.Visible = false;

                if (compra.IdEstado == Convert.ToInt16(EstadoCompraNombre.Nuevo).ToString())
                {
                    lbTitulo1.Visible = false;
                    lbTitulo2.Visible = true;
                    lbMensajeEstado.Visible = true;
                    if (usuario != 1) //categoria 1 = administración
                        btnStock.Enabled = false;
                }

                if (compra.IdEstado == Convert.ToInt16(EstadoCompraNombre.Aprobado).ToString())
                {
                    btnAprobado.Visible = false;
                    btnStock.Visible = true;
                    btnEntregado.Visible = true;
                    pnlEnviarCotizacion.Visible = false;
                }

                if (compra.IdEstado == Convert.ToInt16(EstadoCompraNombre.En_stock).ToString())
                {
                    btnAprobado.Visible = false;
                    btnStock.Visible = true;
                    btnStock.Enabled = false;
                    btnEntregado.Visible = true;
                    btnEntregado.Enabled = true;
                }

                if (compra.IdEstado == Convert.ToInt16(EstadoCompraNombre.Entregado).ToString())
                {
                    btnGuardar.Enabled = false;
                    btnCancelar.Enabled = false;
                    btnAprobado.Visible = false;
                    btnStock.Visible = false;
                    btnEntregado.Visible = true;
                    lbMensajeEntregado.Visible = true;
                    btnRechazar.Visible = false;
                }

                if (compra.IdEstado == Convert.ToInt16(EstadoCompraNombre.Rechazado).ToString())
                {
                    btnStock.Visible = false;
                    btnEntregado.Visible = true;
                    lbMensajeRechazada.Visible = true;
                }
            }

            // Comentarios
            //List<cComentarioCompra> comentarios = cComentarioCompra.GetList(Request["Id"].ToString());
            //if (comentarios.Count > 0)
            //{
            //    rptComentarios.DataSource = comentarios;
            //    rptComentarios.DataBind();
            //}
        }
    }

    #region Combos
    public void CargarComboEmpresa(string idEmpresa)
    {
        cbEmpresa.DataSource = cEmpresa.GetEmpresas();
        if (!string.IsNullOrEmpty(idEmpresa))
        {
            string id = cEmpresa.GetEmpresaByPedido(idEmpresa);
            cbEmpresa.SelectedValue = id;
        }
        cbEmpresa.DataValueField = "id";
        cbEmpresa.DataTextField = "nombre";
        cbEmpresa.DataBind();
        ListItem it = new ListItem("Seleccione una empresa", "0");
        cbEmpresa.Items.Insert(0, it);
    }

    public void CargarComboCliente(string idEmpresa)
    {
        cbCliente.DataSource = cCliente.GetClientesByIdEmpresa(idEmpresa);
        if (!string.IsNullOrEmpty(idEmpresa))
        {
            string id = cEmpresa.GetEmpresaByPedido(idEmpresa);
            cbCliente.SelectedValue = id;
        }
        cbCliente.DataValueField = "id";
        cbCliente.DataTextField = "nombre";
        cbCliente.DataBind();
        ListItem it = new ListItem("Seleccione un cliente", "0");
        cbCliente.Items.Insert(0, it);
    }

    public void CargarComboClienteEnvio(string idEmpresa)
    {
        cbClienteEnvio.DataSource = cCliente.GetClientesByIdEmpresa(idEmpresa);
        if (!string.IsNullOrEmpty(idEmpresa))
        {
            string id = cEmpresa.GetEmpresaByPedido(idEmpresa);
            cbClienteEnvio.SelectedValue = id;
        }
        cbClienteEnvio.DataValueField = "id";
        cbClienteEnvio.DataTextField = "nombre";
        cbClienteEnvio.DataBind();
        ListItem it = new ListItem("Seleccione un cliente", "0");
        cbClienteEnvio.Items.Insert(0, it);
    }

    protected void cbEmpresa_SelectedIndexChanged(object sender, EventArgs e) //Habilita el dropDownList de Cliente, y lo llena
    {
        CargarComboCliente(cbEmpresa.SelectedItem.Value);
    }
    #endregion

    #region ListView Nueva Compra

    #region Edición
    protected void lvNuevaCompra_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        //Session["ListaItems"] = null;
        List<cItem> aux;
        if (Session["ListaItems"] != null)
            aux = (List<cItem>)Session["ListaItems"];
        else
            aux = cItem.GetItems(Request["Id"].ToString());
        lvNuevaCompra.EditIndex = e.NewEditIndex;
        lvNuevaCompra.DataSource = (List<cItem>)aux;
        Session["ListaItems"] = aux;
        lvNuevaCompra.DataBind();
    }

    protected void lvNuevaCompra_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        string auxCant = null;
        string auxImporteProveedor = null;
        string auxImporteCliente = null;
        Int16 usuario = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).IdCategoria;

        TextBox txtCantidad = (lvNuevaCompra.Items[e.ItemIndex].FindControl("txtEditCantidad")) as TextBox;
        TextBox txtDescripcion = (lvNuevaCompra.Items[e.ItemIndex].FindControl("txtEditDescripcion")) as TextBox;
        DropDownList ddl = (lvNuevaCompra.Items[e.ItemIndex].FindControl("ddlEditProveedor")) as DropDownList;
        TextBox txtImporteProveedor = (lvNuevaCompra.Items[e.ItemIndex].FindControl("txtEditImporteProveedor")) as TextBox;
        TextBox txtImporteCliente = (lvNuevaCompra.Items[e.ItemIndex].FindControl("txtEditImporteCliente")) as TextBox;
        TextBox txtNroPedido = (lvNuevaCompra.Items[e.ItemIndex].FindControl("txtEditNroPedido")) as TextBox;

        lvNuevaCompra.EditIndex = -1;
        List<cItem> aux;
        aux = (List<cItem>)Session["ListaItems"];
        lvNuevaCompra.DataSource = (List<cItem>)aux;

        //reemplazo los valores modificados
        auxCant = aux[e.ItemIndex].Cantidad; //valor viejo
        aux[e.ItemIndex].Cantidad = txtCantidad.Text; //valor nuevo
        aux[e.ItemIndex].Descripcion = txtDescripcion.Text;
        aux[e.ItemIndex].IdProveedor = ddl.SelectedValue;
        auxImporteProveedor = aux[e.ItemIndex].ImporteProveedor; //valor viejo
        aux[e.ItemIndex].ImporteProveedor = txtImporteProveedor.Text;//valor nuevo
        auxImporteCliente = aux[e.ItemIndex].ImporteCliente; //valor viejo
        aux[e.ItemIndex].ImporteCliente = txtImporteCliente.Text;//valor nuevo
        aux[e.ItemIndex].NroPedidoProveedor = txtNroPedido.Text;

        #region Importe Proveedor
        if (lbTotalProveedor.Text == "")
            lbTotalProveedor.Text = "0";

        if (Convert.ToDecimal(txtImporteProveedor.Text) != Convert.ToDecimal(auxImporteProveedor)) //Caso en que se modifique un importe, y restar el importe viejo al total, para no tener errores en la suma.
        {
            lbTotalProveedor.Text = Convert.ToString(Convert.ToDecimal(lbTotalProveedor.Text) - (Convert.ToDecimal(auxCant) * Convert.ToDecimal(auxImporteProveedor)));
        }
        else
        {
            if (Convert.ToDecimal(txtCantidad.Text) != Convert.ToDecimal(auxCant))
                lbTotalProveedor.Text = Convert.ToString(Convert.ToDecimal(lbTotalProveedor.Text) - (Convert.ToDecimal(auxCant) * Convert.ToDecimal(auxImporteProveedor)));
        }

        if (Convert.ToDecimal(txtImporteProveedor.Text) != Convert.ToDecimal(auxImporteProveedor) || Convert.ToDecimal(txtCantidad.Text) != Convert.ToDecimal(auxCant)) //Si el importe o la cantidad no se modifica, los valores no se actualizan.
        {
            if (lbTotalProveedor.Text != "")
                lbTotalProveedor.Text = Convert.ToString(Convert.ToDecimal(lbTotalProveedor.Text) + (Convert.ToDecimal(txtCantidad.Text) * Convert.ToDecimal(txtImporteProveedor.Text)));
            else
                lbTotalProveedor.Text = Convert.ToString((Convert.ToDecimal(txtCantidad.Text) * Convert.ToDecimal(txtImporteProveedor.Text)));
        }
        #endregion

        #region Importe Cliente
        if (lbTotalCliente.Text == "")
            lbTotalCliente.Text = "0";

        if (Convert.ToDecimal(txtImporteCliente.Text) != Convert.ToDecimal(auxImporteCliente)) //Caso en que se modifique un importe y no se siga sumando en el total de la compra.
        {
            lbTotalCliente.Text = Convert.ToString(Convert.ToDecimal(lbTotalCliente.Text) - (Convert.ToDecimal(auxCant) * Convert.ToDecimal(auxImporteCliente)));
        }
        else
        {
            if (Convert.ToDecimal(txtCantidad.Text) != Convert.ToDecimal(auxCant))
                lbTotalCliente.Text = Convert.ToString(Convert.ToDecimal(lbTotalCliente.Text) - (Convert.ToDecimal(auxCant) * Convert.ToDecimal(auxImporteCliente)));
        }

        if (Convert.ToDecimal(txtImporteCliente.Text) != Convert.ToDecimal(auxImporteCliente) || Convert.ToDecimal(txtCantidad.Text) != Convert.ToDecimal(auxCant)) //Si el importe o la cantidad no se modifica, los valores no se actualizan.
        {
            if (lbTotalCliente.Text != "")
                lbTotalCliente.Text = Convert.ToString(Convert.ToDecimal(lbTotalCliente.Text) + (Convert.ToDecimal(txtCantidad.Text) * Convert.ToDecimal(txtImporteCliente.Text)));
            else
                lbTotalCliente.Text = Convert.ToString((Convert.ToDecimal(txtCantidad.Text) * Convert.ToDecimal(txtImporteCliente.Text)));
        }
        #endregion

        Session["ListaItems"] = aux;
        Session["aux"] = 1;
        lvNuevaCompra.DataBind();
    }

    protected void lvNuevaCompra_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        lvNuevaCompra.EditIndex = -1;
        cCompra compraImporte = null;
        List<cItem> aux;
        aux = (List<cItem>)Session["ListaItems"];
        lvNuevaCompra.DataSource = (List<cItem>)aux;
        string auxCant = null;
        string auxImporteProveedor = null;
        string auxImporteCliente = null;

        TextBox txtCantidad = (lvNuevaCompra.Items[e.ItemIndex].FindControl("_txtEditCantidad")) as TextBox;
        TextBox txtImporteProveedor = (lvNuevaCompra.Items[e.ItemIndex].FindControl("txtEditImporteProveedor")) as TextBox;
        TextBox txtImporteCliente = (lvNuevaCompra.Items[e.ItemIndex].FindControl("txtEditImporteCliente")) as TextBox;
        auxCant = aux[e.ItemIndex].Cantidad;
        auxImporteProveedor = txtImporteProveedor.Text; //valor viejo
        auxImporteCliente = txtImporteCliente.Text;
        aux[e.ItemIndex].ImporteCliente = txtImporteCliente.Text;

        if (Request["Id"].ToString() == "0")
        {
            aux.Remove(aux[e.ItemIndex]);
        }
        else
        {
            compraImporte = cCompra.GetCompraNro(Request["Id"].ToString(), null);
            if (compraImporte != null)
            {
                List<cItem> item = cItem.GetItems(Request["Id"].ToString());
                aux[e.ItemIndex].Delete(item[0].Id);//Elimino de la base de datos
            }

            aux.Remove(aux[e.ItemIndex]); //Elimino del listview si se esta trabajando con una variable de sesion 
        }

        lbTotalProveedor.Text = Convert.ToString(Convert.ToDecimal(lbTotalProveedor.Text) - (Convert.ToDecimal(auxCant) * Convert.ToDecimal(auxImporteProveedor)));
        lbTotalCliente.Text = Convert.ToString(Convert.ToDecimal(lbTotalCliente.Text) - (Convert.ToDecimal(auxCant) * Convert.ToDecimal(auxImporteCliente)));

        if (Request["Id"].ToString() != "0")
        {
            compraImporte.TotalCliente = lbTotalCliente.Text.Replace(",", ".");
            compraImporte.TotalProveedor = lbTotalProveedor.Text.Replace(",", ".");
            compraImporte.Save();
        }

        Session["ListaItems"] = aux;
        lvNuevaCompra.DataBind();
    }
    #endregion

    #region Metodo Insertar en ListView
    protected void lvNuevaCompra_ItemInserting(object sender, ListViewInsertEventArgs e)
    {
        cItem item = new cItem();
        Dictionary<int, string> d = new Dictionary<int, string>();
        Int16 usuario = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).IdCategoria;
        Session["TotalProveedor"] = null;
        Session["TotalCliente"] = null;

        //Se carga un item
        TextBox txt = (e.Item.FindControl("txtCantidad")) as TextBox;
        if (txt != null)
            item.Cantidad = txt.Text;

        txt = (e.Item.FindControl("txtDescripcion")) as TextBox;
        if (txt != null)
            item.Descripcion = txt.Text;

        DropDownList ddl = (e.Item.FindControl("ddlEditProveedor")) as DropDownList;
        if (ddl.SelectedValue != null)
            item.IdProveedor = ddl.SelectedValue;

        txt = (e.Item.FindControl("_txtImporteProveedor") as TextBox);
        if (txt != null)
            item.ImporteProveedor = txt.Text;

        txt = (e.Item.FindControl("_txtImporteCliente") as TextBox);
        if (txt != null)
            item.ImporteCliente = txt.Text;

        txt = (e.Item.FindControl("_txtNroPedidoProveedor") as TextBox);
        if (txt != null)
            item.NroPedidoProveedor = txt.Text;

        //Se refresca el ListView
        if (!string.IsNullOrEmpty(item.Cantidad) && !string.IsNullOrEmpty(item.Descripcion)) //Se verifica que la fila no este vacia
        {
            lvNuevaCompra.EditIndex = -1;
            List<cItem> aux;

            if (Session["ListaItems"] != null)
                aux = (List<cItem>)Session["ListaItems"];
            else
                aux = new List<cItem>();

            lvNuevaCompra.DataSource = (List<cItem>)aux;
            Session["ListaItems"] = aux;

            if (lbTotalProveedor.Text == "")
            {
                lbTotalProveedor.Text = CalcularTotalProveedor(item, 0).ToString();
                Session["TotalProveedor"] = CalcularTotalProveedor(item, 0).ToString();
                if (usuario == 1)
                    Session["TotalProveedor"] = CalcularTotalProveedor(item, Convert.ToDouble(lbTotalProveedor.Text)).ToString();
            }
            else
            {
                Session["TotalProveedor"] = CalcularTotalProveedor(item, Convert.ToDouble(lbTotalProveedor.Text)).ToString();
                lbTotalProveedor.Text = CalcularTotalProveedor(item, Convert.ToDouble(lbTotalProveedor.Text)).ToString();
            }

            if (lbTotalCliente.Text == "")
            {
                Session["TotalCliente"] = CalcularTotalCliente(item, 0).ToString();
                lbTotalCliente.Text = CalcularTotalCliente(item, 0).ToString();
                if (usuario == 1)
                    Session["TotalProveedor"] = CalcularTotalProveedor(item, Convert.ToDouble(lbTotalCliente.Text)).ToString();
            }
            else
            {
                Session["TotalCliente"] = CalcularTotalCliente(item, Convert.ToDouble(lbTotalCliente.Text)).ToString();
                lbTotalCliente.Text = CalcularTotalCliente(item, Convert.ToDouble(lbTotalCliente.Text)).ToString();
            }

            aux.Add(item);

            if (!string.IsNullOrEmpty(item.IdCompra))
            {
                item.IdCompra = lbNroCompra.Text;
                item.Save();
            }

            lvNuevaCompra.DataBind();
        }
    }

    public double CalcularTotalProveedor(cItem item, double acum)
    {
        double total = 0;
        if (string.IsNullOrEmpty(item.ImporteProveedor))
            item.ImporteProveedor = "0";
        return total = acum + Convert.ToDouble(item.Cantidad) * Convert.ToDouble(item.ImporteProveedor);
    }

    public double CalcularTotalCliente(cItem item, double acum)
    {
        double total = 0;
        if (string.IsNullOrEmpty(item.ImporteCliente))
            item.ImporteCliente = "0";
        return total = acum + Convert.ToDouble(item.Cantidad) * Convert.ToDouble(item.ImporteCliente);
    }
    #endregion

    #endregion

    #region Botones

    protected void btnSolicitarCotizacion_Click(object sender, EventArgs e)
    {
        Session["Estado"] = null;
        btnGuardar_Click(null, null);
        lbMensajeSolicitud.Visible = true;
        lbMensajeGuardar.Visible = false;
        btnSolicitarCotizacion.Enabled = false;
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        List<cItem> aux;
        int i = 1;
        cCompra compra = null;
        Int16 usuario = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).IdCategoria;

        //Si la compra existe, se actualizan algunos de los datos. Si no existe la compra se hace una nueva
        if (Request["Id"].ToString() == "0")
            compra = new cCompra();
        else
            compra = cCompra.Load(Request["Id"].ToString(), null);

        #region ListView Nueva Compra
        if (lvNuevaCompra.Items.Count != 0)
        {
            //ModalPopupExtender.Show(); //agregar comentario
            aux = (List<cItem>)Session["ListaItems"];

            if (compra.IdEstado != null)
                if (compra.IdEstado != "0")
                    if (Session["Estado"] != null)
                        compra.IdEstado = Session["Estado"].ToString(); //se actualiza el estado.
                    else
                        Session["Estado"] = compra.IdEstado;


            if (Session["Estado"] == null)
                compra.IdEstado = Convert.ToString(Convert.ToInt16(EstadoCompraNombre.Nuevo));
            else
                compra.IdEstado = Session["Estado"].ToString();

            if (string.IsNullOrEmpty(lbTicket.Text))
                compra.IdPedido = "0";
            else
                compra.IdPedido = lbTicket.Text;

            if (cbEmpresa.Visible != false)
            {
                if (cbEmpresa.SelectedValue == "0")
                    compra.IdEmpresa = "-1";
                else
                {
                    compra.IdEmpresa = cbEmpresa.SelectedValue;
                }
            }

            if (cbCliente.Visible != false)
            {
                if (cbCliente.SelectedValue == "0" || cbCliente.SelectedValue == "")
                    compra.IdCliente = "-1";
                else
                    compra.IdCliente = cbCliente.SelectedValue;
            }

            compra.Fecha = Convert.ToDateTime(lbFecha.Text);
            if (string.IsNullOrEmpty(compra.IdUsuario))
                compra.IdUsuario = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Id;

            double totalP = Convert.ToDouble(Session["TotalProveedor"]);
            double totalC = Convert.ToDouble(Session["TotalCliente"]);

            if (lbTotalProveedor.Text != "")
                totalP = Convert.ToDouble(lbTotalProveedor.Text);

            if (lbTotalCliente.Text != "")
                totalC = Convert.ToDouble(lbTotalCliente.Text);

            if (totalP != 0)
            {
                compra.TotalProveedor = totalP.ToString();
            }
            else
                compra.TotalProveedor = "0";

            if (totalC != 0)
            {
                compra.TotalCliente = totalC.ToString();
            }
            else
                compra.TotalCliente = "0";

            if (chbIva.Checked)
                compra.Iva = 1;// Iva incluido
            else
                compra.Iva = 0; //+ Iva

            if (compra.Codigo == "0" || compra.Codigo == null)
                compra.Codigo = "0";
            compra.Save();

            #region Hash
            string id = cCompraDAO.GetIdLastCompra();
            cCompra _compra = cCompra.GetCompraNro(id, null);

            if (compra.Codigo == "0")
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                Byte[] tmpSource = ASCIIEncoding.Default.GetBytes(id);
                Byte[] tmpHash = md5.ComputeHash(tmpSource);
                _compra.Codigo = BitConverter.ToString(tmpHash).Replace("-", "");

                double _totalP = Convert.ToDouble(_compra.TotalProveedor);
                double _totalC = Convert.ToDouble(_compra.TotalCliente);

                _compra.TotalProveedor = totalP.ToString();
                _compra.TotalCliente = totalC.ToString();

                _compra.Save();
            }
            #endregion

            int contItem = 0;
            //Datos de los items
            while (i <= lvNuevaCompra.Items.Count)
            {
                if (string.IsNullOrEmpty(lbNroCompra.Text)) //Si el lbNroCompra.Text no tiene una valor, se crea una nuevo item para la compra
                {
                    //Se crea un nuevo item
                    cItem item = new cItem();
                    item.IdCompra = compra.LastCompra();
                    item.Cantidad = aux[i - 1].Cantidad;
                    item.Descripcion = aux[i - 1].Descripcion;
                    item.IdProveedor = aux[i - 1].IdProveedor;
                    if (aux[i - 1].ImporteProveedor != "")
                        item.ImporteProveedor = aux[i - 1].ImporteProveedor.Replace(",", ".");
                    else
                        item.ImporteProveedor = "0";

                    if (aux[i - 1].ImporteCliente != "")
                        item.ImporteCliente = aux[i - 1].ImporteCliente.Replace(",", ".");
                    else
                        item.ImporteCliente = "0";

                    if (aux[i - 1].NroPedidoProveedor != "")
                        item.NroPedidoProveedor = aux[i - 1].NroPedidoProveedor;
                    else
                        item.NroPedidoProveedor = "-1";

                    item.Save();
                }
                else
                {
                    if (aux == null)
                        aux = cItem.GetItems(compra.Id); //se guardan los items de la compra, para actualizar sin trabajar sin la base de datos.

                    //Se actualiza datos del item
                    List<cItem> item = cItem.GetItems(Request["Id"].ToString());

                    if (i - 1 > 0 && Session["aux"].ToString() == "0" && contItem == item.Count)
                    {
                        cItem newItem = new cItem();
                        newItem.IdCompra = lbNroCompra.Text;
                        newItem.Cantidad = aux[i - 1].Cantidad;
                        newItem.Descripcion = aux[i - 1].Descripcion;
                        newItem.IdProveedor = aux[i - 1].IdProveedor;
                        if (Session["ImporteProveedor"] != null)
                        {
                            newItem.ImporteProveedor = Session["ImporteProveedor"].ToString().Replace(",", ".");
                            Session["ImporteProveedor"] = null;
                        }
                        else
                            newItem.ImporteProveedor = aux[i - 1].ImporteProveedor.Replace(",", ".");
                        if (aux[i - 1].ImporteCliente != "")
                        {
                            if (Session["ImporteCliente"] != null)
                            {
                                newItem.ImporteCliente = Session["ImporteCliente"].ToString().Replace(",", ".");
                                Session["ImporteCliente"] = null;
                            }
                            else
                                newItem.ImporteCliente = aux[i - 1].ImporteCliente.Replace(",", ".");
                        }
                        else
                            newItem.ImporteCliente = "0";
                        if (aux[i - 1].NroPedidoProveedor != "")
                            newItem.NroPedidoProveedor = aux[i - 1].NroPedidoProveedor;
                        else
                            newItem.NroPedidoProveedor = "-1";
                        newItem.NroPedidoProveedor = "0";
                        newItem.Save();
                    }
                    else
                    {
                        item[i - 1].IdCompra = lbNroCompra.Text;
                        item[i - 1].Cantidad = aux[i - 1].Cantidad;
                        item[i - 1].Descripcion = aux[i - 1].Descripcion;
                        item[i - 1].IdProveedor = aux[i - 1].IdProveedor;
                        if (aux[i - 1].ImporteProveedor != "")
                        {
                            if (Session["ImporteProveedor"] != null)
                            {
                                item[i - 1].ImporteProveedor = Session["ImporteProveedor"].ToString().Replace(",", ".");
                                Session["ImporteProveedor"] = null;
                            }
                            else
                                item[i - 1].ImporteProveedor = aux[i - 1].ImporteProveedor.Replace(",", ".");
                        }
                        else
                            item[i - 1].ImporteProveedor = "0";

                        if (aux[i - 1].ImporteCliente != "")
                        {
                            if (Session["ImporteCliente"] != null)
                            {
                                item[i - 1].ImporteCliente = Session["ImporteCliente"].ToString().Replace(",", ".");
                                Session["ImporteCliente"] = null;
                            }
                            else
                                item[i - 1].ImporteCliente = aux[i - 1].ImporteCliente.Replace(",", ".");
                        }
                        else
                            item[i - 1].ImporteCliente = "0";

                        if (aux[i - 1].NroPedidoProveedor != "")
                            item[i - 1].NroPedidoProveedor = aux[i - 1].NroPedidoProveedor;
                        else
                            item[i - 1].NroPedidoProveedor = "-1";

                        item[i - 1].Save();
                        contItem++;
                    }
                }

                i++;
            }
            Session["aux"] = 0;
        }
        else
        {
            if (Session["Estado"].ToString() == Convert.ToInt16(EstadoCompraNombre.Rechazado).ToString())
            {
                compra.IdEstado = Convert.ToString(Convert.ToInt16(EstadoCompraNombre.Rechazado));
                #region Totales
                double totalP = Convert.ToDouble(Session["TotalProveedor"]);
                double totalC = Convert.ToDouble(Session["TotalCliente"]);

                if (lbTotalProveedor.Text != "")
                    totalP = Convert.ToDouble(lbTotalProveedor.Text);

                if (lbTotalCliente.Text != "")
                    totalC = Convert.ToDouble(lbTotalCliente.Text);

                if (totalP != 0)
                {
                    compra.TotalProveedor = totalP.ToString();
                }
                else
                    compra.TotalProveedor = "0";

                if (totalC != 0)
                {
                    compra.TotalCliente = totalC.ToString();
                }
                else
                    compra.TotalCliente = "0";
                #endregion
                compra.Save();
            }
        }
        #endregion

        if (lbMensajeSolicitud.Visible == false)
            lbMensajeGuardar.Visible = true;
        else
            lbMensajeGuardar.Visible = false;
        Session["ListaItems"] = null;
        Session["Estado"] = null;
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Session["ListaItems"] = null;
        Response.Redirect("BuscarCompra.aspx");
    }

    protected void btnAprobado_Click(object sender, EventArgs e)
    {
        lbMensajeEstado.Visible = false;
        Session["Estado"] = Convert.ToInt16(EstadoCompraNombre.Aprobado).ToString();
        btnStock.Visible = true;
        btnEntregado.Visible = true;
        Session["TotalProveedor"] = lbTotalProveedor.Text;
        Session["TotalCliente"] = lbTotalCliente.Text;
        btnAprobado.Visible = false;
        pnlEnviarCotizacion.Visible = false;
        btnGuardar_Click(null, null);
    }

    protected void btnStock_Click(object sender, EventArgs e)
    {
        Session["Estado"] = Convert.ToInt16(EstadoCompraNombre.En_stock).ToString(); //la variable Session["Estado"] se usa para guardar el estado de la compra, cuando se elije una opcion entre Compra Inmediata, Proveedor o Rechazar
        btnAprobado.Enabled = false;
        btnEntregado.Enabled = true;
        Session["TotalProveedor"] = lbTotalProveedor.Text;
        Session["TotalCliente"] = lbTotalCliente.Text;
        btnStock.Enabled = false;
        btnGuardar_Click(null, null);
    }

    protected void btnEntregado_Click(object sender, EventArgs e)
    {
        cCompra compra = cCompra.GetCompraNro(Request["Id"].ToString(), null);
        Session["Estado"] = Convert.ToInt16(EstadoCompraNombre.Entregado).ToString();
        btnEntregado.Enabled = true;
        btnGuardar.Enabled = false;
        btnCancelar.Enabled = false;
        btnStock.Enabled = false;
        btnRechazar.Enabled = false;

        if (lbTotalProveedor.Text != "")
            Session["TotalProveedor"] = lbTotalProveedor.Text;
        else
            Session["TotalProveedor"] = compra.TotalProveedor;

        if (lbTotalProveedor.Text != "")
            Session["TotalCliente"] = lbTotalCliente.Text;
        else
            Session["TotalCliente"] = compra.TotalCliente;

        btnEntregado.Enabled = false;
        btnGuardar_Click(null, null);
    }

    protected void btnRechazar_Click(object sender, EventArgs e)
    {
        Session["Estado"] = Convert.ToInt16(EstadoCompraNombre.Rechazado).ToString();
        //ModalPopupExtender.Show();
        btnGuardar_Click(null, null);
    }

    protected void verDetalle_Click(object sender, EventArgs e)
    {
        Response.Redirect("DetallePedido.aspx?id=" + lbTicket.Text);
    }

    protected void lkbVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("BuscarCompra.aspx");
    }

    protected void lkbProveedor_Click(object sender, EventArgs e)
    {
        Response.Redirect("Proveedor.aspx");
    }

    protected void RedirectCompra(string nroCompra)
    {
        Response.Redirect("Compra.aspx?id=" + nroCompra, false);
    }
    #endregion
    
    protected void btnImporteProveedor_Click(object sender, EventArgs e)
    {
        double valorViejoProveedor = 1;
        double valorNuevoProveedor = 1;
        double valorViejoCliente = 1;
        double valorNuevoCliente = 1;
        double auxProveedor = 1;
        double auxCliente = 1;
        var l = ((TextBox)sender).Text;

        Panel p = ((Panel)((TextBox)sender).Parent);
        if (p.ID == "pnlImporteProveedor") //Para modificar importes proveedor
        {
            foreach (Object obj in p.Controls)
            {
                if (obj.GetType() == typeof(Label))
                {
                    auxProveedor = Convert.ToDouble(((Label)obj).Text);
                    valorViejoProveedor = valorViejoProveedor * auxProveedor;
                    Session["ImporteProveedor"] = valorNuevoProveedor; //La variable de sesion se usa en el caso que haya una actualización en el importe de proveedor, para guardar despues los datos nuevos en el metodo btnGuardar_Click
                }

                if (obj.GetType() == typeof(TextBox))
                {
                    valorNuevoProveedor = Convert.ToDouble(((TextBox)obj).Text) * auxProveedor;
                }
            }
        }

        if (p.ID == "pnlImporteCliente") //Para modificar importes cliente
        {
            foreach (Object obj in p.Controls)
            {
                if (obj.GetType() == typeof(Label))
                {
                    auxCliente = Convert.ToDouble(((Label)obj).Text);
                    valorViejoCliente = valorViejoCliente * auxCliente;
                    Session["ImporteCliente"] = valorNuevoCliente; //La variable de sesion se usa en el caso que haya una actualización en el importe de cliente, para guardar despues los datos nuevos en el metodo btnGuardar_Click
                }

                if (obj.GetType() == typeof(TextBox))
                {
                    valorNuevoCliente = Convert.ToDouble(((TextBox)obj).Text) * auxCliente;
                }
            }
        }

        Session["Guardar"] = 1;

        lbTotalProveedor.Text = Convert.ToString(Convert.ToDouble(lbTotalProveedor.Text) - valorViejoProveedor); //resta
        lbTotalProveedor.Text = Convert.ToString(Convert.ToDouble(lbTotalProveedor.Text) + (valorNuevoProveedor)); //suma

        lbTotalCliente.Text = Convert.ToString(Convert.ToDouble(lbTotalCliente.Text) - valorViejoCliente); //resta
        lbTotalCliente.Text = Convert.ToString(Convert.ToDouble(lbTotalCliente.Text) + (valorNuevoCliente)); //suma
        Session["aux"] = "1";
    }

    protected void btnEnviar_Click(object sender, EventArgs e)
    {
        Session["Estado"] = Convert.ToInt16(EstadoCompraNombre.Cotizado).ToString();
        btnGuardar_Click(null, null);

        if (cbClienteEnvio.SelectedValue != "0")
        {
            string mailCliente = cCliente.Load(cbClienteEnvio.SelectedValue).Mail;
            string cliente = cCliente.Load(cbClienteEnvio.SelectedValue).Nombre;
            Session["importarPDF"] = 0;

            CrearPDF();
            Attachment data = new Attachment("C:\\crm info\\cotizaciones\\" + Session["itemIdCompra"].ToString() + ".pdf");
            // Envio correo
            cSendMail mail = new cSendMail();
            mail.EnviarCotizacion(cliente, mailCliente, Session["compraCodigo"].ToString(), data);
            Session["compraCodigo"] = null;
            Session["itemIdCompra"] = null;
        }
        btnEnviar.Enabled = false;
        lbMensajeMail.Visible = true;
    }

    protected void btnPDF_Click(object sender, ImageClickEventArgs e)
    {
        Session["importarPDF"] = 1;
        CrearPDF();
    }

    public void CrearPDF()
    {
        cCompra compra = cCompra.GetCompraNro(Request["Id"].ToString(), null);
        List<cItem> item = cItem.GetItems(compra.Id);
        int i = 0;
        int j = 1;

        Session["compraCodigo"] = compra.Codigo;
        Session["itemIdCompra"] = item[0].IdCompra;

        //CrystalReportSource2.ReportDocument.SetParameterValue("fecha", compra.Fecha);

        //while (item.Count - 1 >= i) //completa las filas con los datos
        //{
        //    CrystalReportSource2.ReportDocument.SetParameterValue("cant" + j, item[i].Cantidad);
        //    CrystalReportSource2.ReportDocument.SetParameterValue("desc" + j, item[i].Descripcion);
        //    CrystalReportSource2.ReportDocument.SetParameterValue("imp" + j, item[i].ImporteCliente);
        //    i++;
        //    j++;
        //}

        //while (i < 18) //completa las filas vacías
        //{
        //    CrystalReportSource2.ReportDocument.SetParameterValue("cant" + j, "");
        //    CrystalReportSource2.ReportDocument.SetParameterValue("desc" + j, "");
        //    CrystalReportSource2.ReportDocument.SetParameterValue("imp" + j, "");
        //    j++;
        //    i++;
        //}

        //if (chbIva.Checked)
        //    CrystalReportSource2.ReportDocument.SetParameterValue("mensaje", "IVA incluido");
        //else
        //    CrystalReportSource2.ReportDocument.SetParameterValue("mensaje", "+ IVA");

        //CrystalReportSource2.ReportDocument.SetParameterValue("total", compra.TotalCliente);

        //CrystalReportSource2.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "C:\\crm info\\cotizaciones\\" + item[0].IdCompra + ".pdf");
    

        if (Session["importarPDF"].ToString() != "0")
        {
            string filename = item[0].IdCompra + ".pdf";
            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);

            FileInfo fileToDownload = new System.IO.FileInfo("C:\\crm info\\cotizaciones\\" + item[0].IdCompra + ".pdf"); 
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }
    }
}
