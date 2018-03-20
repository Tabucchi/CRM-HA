<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>
<%@ Register Src="~/Controles/MensajeIndiceCac.ascx" TagPrefix="crm" TagName="MensajeIndiceCac" %>
<%@ Register Src="~/Controles/MensajeIndiceUVA.ascx" TagPrefix="crm" TagName="MensajeIndiceUVA" %>
<%@ Register Src="Pendientes.ascx" TagPrefix="crm" TagName="Pendientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<div id="maincol" style="max-width: 1600px !important;">  
    <asp:Panel ID="pnlIndiceCAC" runat="server" Visible="false" >
        <crm:MensajeIndiceCac runat="server" id="MensajeIndiceCac" />
    </asp:Panel>

    <asp:Panel ID="pnlIndiceUVA" runat="server" Visible="false" >
        <crm:MensajeIndiceUVA runat="server" id="MensajeIndiceCac1" />
    </asp:Panel>
        
    <div style="width:100%;">
        <asp:Panel ID="pnlPendientes" runat="server" Visible="false">
            <div>
                <div>
                    <div style="margin-top: -29px;float:left;margin-right: 5px; margin-bottom: 22px;">
                    <section>
                        <div class="formHolderMessage panelMenu headPendiente">
                            <h3 class="titlePendiente"> Pendientes </h3>
                        </div>
                    </section>
                </div>
                    <div style="margin-top: -29px; float:left;">
                    <section>
                        <div class="formHolderMessage panelMenu bodyPendiente">
                            <div class="sideHeader itemPanel itemPendiente" style="float:left;">
			                    <div style="float:left;">
                                    <h2>Nuevos precios <asp:Label ID="lbCantPrecios" CssClass="itemCount" runat="server" Text="0"></asp:Label></h2>
			                    </div>
                                <div style="float:right;margin-top: 0px;"><a href="PendientesPrecios.aspx" class="optMore linkPendiente">Ver+</a></div>
		                    </div>

                            <div class="sideHeader itemPanel itemPendiente" style="float:right; width: 310px !important;">
                                <div style="float:left;">
                                    <h2>Operaciones de venta <asp:Label ID="lbCantOV" CssClass="itemCount" runat="server" Text="0"></asp:Label></h2>
                                </div>
                                <div style="float:right;margin-top: 0px;"><a href="PendientesOperacionesVenta.aspx" class="optMore linkPendiente">Ver+</a></div>
		                    </div>                           
                        </div>
                    </section>
                </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlAdmin" runat="server" Visible="false">
            <div style="width:100%; text-align:center;">
                <table>
                    <tr>
                        <td>
                            <section>
                                <div style="margin-right: 6px;">
                                    <div class="formHolderMessage panelMenu" style="height: 393px; padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">		
                                        <div style="width:330px;">
                                            <div class="encabezadoMenu"><h2> Obras </h2></div>
                                            <div>
                                                <ul class="listSeccionMenu">
                                                    <li class="liMenu">
                                                        <a href="./Proyecto.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/obras.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Listado de Obras</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu">
                                                        <a href="./ResumenObra.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/resumenObras.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Resumen de Obras</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu" style="margin-right: 20px !important;">
                                                        <a href="#" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/unidades.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu" style="margin-top: 5px;">
                                                                    <div>Listado de unidades / precios de </div>
                                                                    <div style="margin-top: 4px;">
                                                                        <asp:DropDownList ID="cbProyectos" runat="server" CssClass="selectMenu"></asp:DropDownList>
                                                                        <label><asp:Button ID="btnProyectos" Text="Ir" CssClass="buttonMenu" runat="server" OnClick="btnProyectos_Click"/></label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <asp:Panel ID="pnlReservas" runat="server" Visible="true">
                                                    <li class="liMenu">
                                                        <a href="./ListadoReserva.aspx" class="linkMenu">
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/reserva.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Reservas</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    </asp:Panel>
                                                    <li class="liMenu" style="margin-bottom:20px">
                                                        <a href="./Historial.aspx" class="linkMenu">
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/historial.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Evolución de unidades</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </td>

                        <td>
                            <section>
                                <div style="margin-right: 6px;">
                                    <div class="formHolderMessage panelMenu" style="height: 320px; padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">		
                                        <div style="width:286px;">
                                            <div class="encabezadoMenu"><h2> Clientes </h2></div>
                                            <div>
                                                <ul class="listSeccionMenu">
                                                    <li class="liMenu">
                                                        <a href="./Agenda.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/directorio.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Directorio</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu">
                                                        <a href="./NuevoCliente.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/newCliente.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Nuevo cliente</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu" style="margin-right: 20px !important;">
                                                        <a href="./CuentaCorriente.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/cuentaCorriente.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Cuentas Corrientes</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu" style="margin-bottom:20px">
                                                        <a href="./Morosos.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/deuda.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Deudas pendientes</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </td>

                        <td>
                            <section>
                                <div style="margin-right: 6px;">
                                    <div class="formHolderMessage panelMenu" style="height: 320px; padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">		
                                        <div style="width:286px;">
                                            <div class="encabezadoMenu"><h2> Operaciones de venta </h2></div>
                                            <div>
                                                <ul class="listSeccionMenu">
                                                    <li class="liMenu">
                                                        <a href="./ListaOperacionVenta.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/operacionesVenta.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Operaciones de venta</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu">
                                                        <a href="./OperacionVenta.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/newOV.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Nueva operación de venta</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu" style="margin-right: 20px !important;">
                                                        <a href="./CC.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/historialPago.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Historial de pagos</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu" style="margin-bottom:20px">
                                                        <a href="./CC.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/comprobantes.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Comprobantes</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </td>

                        <%--<td>
                            <div style="margin-right: 5px;">
                                <div style="margin-left: 8px; margin-bottom: -25px;">
                                    <section>
                                        <div>
                                            <div class="formHolderMessage panelMenu" style="height: 136px; padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">
                                                <div style="width:480px; margin-bottom: 10px;">
                                                    <div class="encabezadoMenu"><h2> Clientes </h2></div>
                                                    <div>
                                                        <div style="width:100%; text-align:center;">
                                                            <div id="divAgenda" runat="server" style="float:left; width:100px; padding: 20px 0 0 40px;">
                                                                <a href="./Agenda.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                    <div>
                                                                        <div><img src="images/menu/directorio.png" style="border-width:0px;"/></div>
                                                                        <div >Directorio</div>
                                                                    </div>
                                                                </a>
                                                            </div>

                                                            <asp:Panel ID="pnlNuevoCliente" runat="server" Visible="false" style="display: inline-block; margin-left: 12px; width:100px; padding: 20px 0 0 25px;">
                                                            <div>
                                                                <a href="./NuevoCliente.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                    <div>
                                                                        <div><img src="images/menu/newCliente.png" style="border-width:0px;"/></div>
                                                                        <div>Nuevo cliente</div>
                                                                    </div>
                                                                </a>
                                                            </div>
                                                            </asp:Panel>

                                                            <div id="divCC" runat="server" style="float:right; width:150px; padding: 20px 25px 0 25px;">
                                                                <a href="./CuentaCorriente.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                    <div>
                                                                        <div><img src="images/menu/cuentaCorriente.png" style="border-width:0px;"/></div>
                                                                        <div>Cuentas Corrientes</div>
                                                                    </div>
                                                                </a>
                                                            </div>

                                                            <div id="divDeuda" runat="server" style="float:right; width:150px; padding: 20px 25px 0 25px;">
                                                                <a href="./Morosos.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                    <div>
                                                                        <div><img src="images/menu/deuda.png" style="border-width:0px;"/></div>
                                                                        <div>Deudas pendientes</div>
                                                                    </div>
                                                                </a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </section>
                                </div>

                                <div style="margin-left: 8px; margin-top: 4px;">
                                    <section>
                                        <div>
                                            <div class="formHolderMessage panelMenu" style="height: 153px; padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">
                                                <div style="width:480px; margin-bottom: 10px;">
                                                    <div class="encabezadoMenu"><h2> Operaciones de venta </h2></div>
                                                    <div>
                                                        <div style="width:100%; text-align:center;">
                                                            <div id="divOV" runat="server" style="float:left; width:100px; padding: 20px 0 0 40px;">
                                                                <a href="./ListaOperacionVenta.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                    <div>
                                                                        <div><img src="images/menu/operacionesVenta.png" style="border-width:0px;"/></div>
                                                                        <div >Operaciones de venta</div>
                                                                    </div>
                                                                </a>
                                                            </div>
                                                            <asp:Panel ID="pnlNuevaOV" runat="server" Visible="false" style="display: inline-block; margin:0 auto; margin-left: 12px; width:100px; padding: 20px 0 0 25px;">
                                                                <div>
                                                                    <a href="./OperacionVenta.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                        <div>
                                                                            <div><img src="images/menu/newOV.png" style="border-width:0px;"/></div>
                                                                            <div style="text-align: center; font-size: 16px;">Nueva operación de venta</div>
                                                                        </div>
                                                                    </a>
                                                                </div>
                                                            </asp:Panel>
                                        
                                                            <div id="divHistorial" runat="server" style="float:right; width:150px; padding: 20px 25px 0 25px;">                                            
                                                                <a href="./CC.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                    <div>
                                                                        <div><img src="images/menu/historialPago.png" style="border-width:0px;"/></div>
                                                                        <div>Historial de pagos</div>
                                                                    </div>
                                                                </a>
                                                            </div>

                                                            <div id="divComprobante" runat="server" style="float:right; width:150px; padding: 20px 25px 0 25px;">                                            
                                                                <a href="./CC.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                    <div>
                                                                        <div><img src="images/menu/comprobantes.png" style="border-width:0px;"/></div>
                                                                        <div>Comprobantes</div>
                                                                    </div>
                                                                </a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </section>
                                </div>
                            </div>

                            <div style="margin-top: -29px;">
                                <section>
                                    <div class="formHolderMessage panelMenu" style="padding: 0px 15px 12px 6px !important;">
                                        <div style="float:left;">
                                            <div class="titleMenu">
                                                <font class="fontTitleIndice">Dólar:</font>
                                                <font class="fontIndice"><asp:Label ID="lbDolar" runat="server"/></font>
                                            </div>
                                        </div>
                                        <div style="display: inline-block; margin:0 auto;">
                                            <div class="titleMenu">
                                                <font class="fontTitleIndice">UVA:</font>
                                                <font class="fontIndice"><asp:Label ID="lbUVA" runat="server">-</asp:Label></font>
                                            </div>
                                        </div>
                                        <div style="float:right;">
                                            <div class="titleMenu">
                                                <font class="fontTitleIndice">CAC:</font>
                                                <font class="fontIndice"><asp:Label ID="lbCAC" runat="server"/></font>
                                            </div>
                                        </div>
                                    </div>
                                </section>
                            </div>
                        </td>--%>

                        <td>
                            <div>
                                <asp:Panel ID="pnlDatos" runat="server">
                                    <div>
                                        <section>
                                            <div>
                                                <div class="formHolderMessage panelMenu" style="padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">		
                                                    <div style="width:330px;">
                                                        <div class="encabezadoMenu"><h2> Información de gestión </h2></div>
                                                        <div>
                                                            <ul class="listSeccionMenu">
                                                                <li class="liMenu">
                                                                    <a href="./CuotasCliente.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                        <div>
                                                                            <div style="float:left"><img src="images/menu/cuotasClientes.png" style="border-width:0px;"/></div>
                                                                            <div class="titleMenu">Cuotas a vencer por cliente</div>
                                                                        </div>
                                                                    </a>
                                                                </li>
                                                                <li class="liMenu">
                                                                    <a href="./CuotasObra.aspx" class="linkMenu">
                                                                        <div>
                                                                            <div style="float:left"><img src="images/menu/cuotasObras.png" style="border-width:0px;"/></div>
                                                                            <div class="titleMenu">Saldos a cobrar por obra</div>
                                                                        </div>
                                                                    </a>
                                                                </li>
                                                                <li class="liMenu" style="margin-bottom:20px">
                                                                    <a href="./TotalDeudaCliente.aspx" class="linkMenu">
                                                                        <div>
                                                                            <div style="float:left"><img src="images/menu/totaldeudacliente.png" style="border-width:0px;"/></div>
                                                                            <div class="titleMenu">Total de deuda por cliente</div>
                                                                        </div>
                                                                    </a>
                                                                </li>
                                                                <li class="liMenu" style="margin-bottom:20px">
                                                                    <a href="./UnidadesVendidas.aspx" class="linkMenu">
                                                                        <div>
                                                                            <div style="float:left"><img src="images/menu/vendidas.png" style="border-width:0px;"/></div>
                                                                            <div class="titleMenu">Unidades vendidas</div>
                                                                        </div>
                                                                    </a>
                                                                </li>
                                                                <li class="liMenu" style="margin-bottom:20px">
                                                                    <a href="./ResumenSaldo.aspx" class="linkMenu">
                                                                        <div>
                                                                            <div style="float:left"><img src="images/menu/controlIndice.png" style="border-width:0px;"/></div>
                                                                            <div class="titleMenu">Control de índices</div>
                                                                        </div>
                                                                    </a>
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </section>
                                    </div>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        
            <div style="margin-top: -101px; width: 590px; margin-left: 342px;">
                <section>
                    <div class="formHolderMessage panelMenu" style="padding: 0px 15px 12px 6px !important;">
                        <div style="float:left;">
                            <div class="titleMenu" style="margin-left: 42px;">
                                <font class="fontTitleIndice">Dólar:</font>
                                <font class="fontIndice"><asp:Label ID="lbDolar" runat="server"/></font>
                            </div>
                        </div>
                        <div style="display: inline-block; margin:0 auto;">
                            <div class="titleMenu" style="margin-left: 100px !important;">
                                <font class="fontTitleIndice">UVA:</font>
                                <font class="fontIndice"><asp:Label ID="lbUVA" runat="server">-</asp:Label></font>
                            </div>
                        </div>
                        <div style="float:right;">
                            <div class="titleMenu" style="margin-right: 42px;">
                                <font class="fontTitleIndice">CAC:</font>
                                <font class="fontIndice"><asp:Label ID="lbCAC" runat="server"/></font>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlUsuarioGerencia" runat="server" Visible="false">
            <div style="width:100%; text-align:center;">
                <table>
                    <tr>
                        <td>
                            <section>
                                <div style="margin-right: -3px;">
                                    <div class="formHolderMessage panelMenu" style="height: 393px; padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">		
                                        <div style="width:366px;">
                                            <div class="encabezadoMenu"><h2> Obras </h2></div>
                                            <div>
                                                <ul class="listSeccionMenu">
                                                    <li class="liMenu">
                                                        <a href="./Proyecto.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/obras.png" style="border-width:0px;"></div>
                                                                <div class="titleMenu">Listado de Obras</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu">
                                                        <a href="./ResumenObra.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/resumenObras.png" style="border-width:0px;"></div>
                                                                <div class="titleMenu">Resumen de Obras</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu" style="margin-right: 20px !important;">
                                                        <a href="#" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/unidades.png" style="border-width:0px;"></div>
                                                                <div class="titleMenu" style="margin-top: 5px;">
                                                                    <div>Listado de unidades / precios de </div>
                                                                    <div style="margin-top: 4px;">
                                                                        <div style="margin-top: 4px;">
                                                                        <asp:DropDownList ID="cbProyectosUsrGerencia" runat="server" CssClass="selectMenu"></asp:DropDownList>
                                                                        <label><asp:Button ID="btnProyectosUsrGerencia" Text="Ir" CssClass="buttonMenu" runat="server" OnClick="btnProyectosUsrGerencia_Click"/></label>
                                                                    </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu">
                                                        <a href="./ListadoReserva.aspx" class="linkMenu">
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/reserva.png" style="border-width:0px;"></div>
                                                                <div class="titleMenu">Reservas</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu" style="margin-bottom:20px">
                                                        <a href="./Historial.aspx" class="linkMenu">
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/historial.png" style="border-width:0px;"></div>
                                                                <div class="titleMenu">Evolución de unidades</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </td>

                        <td>
                            <div style="margin-right: 5px;">
                                <div style="margin-left: 8px; margin-bottom: -25px;">
                                    <section>
                                        <div>
                                            <div class="formHolderMessage panelMenu" style="height: 136px; padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">
                                                <div style="width:480px; margin-bottom: 10px;">
                                                    <div class="encabezadoMenu"><h2> Clientes </h2></div>
                                                    <div>
                                                        <div style="width:100%; text-align:center;">
                                                            <div id="ctl00_ContentPlaceHolder1_divAgenda" style="float:left; width:100px; padding: 20px 0 0 116px !important;">
                                                                <a href="./Agenda.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                    <div>
                                                                        <div><img src="images/menu/directorio.png" style="border-width:0px;"></div>
                                                                        <div>Directorio</div>
                                                                    </div>
                                                                </a>
                                                            </div>

                                                            <div style="float:right; width:150px; padding: 20px 80px 0 25px;">
                                                                <a href="./CuentaCorriente.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                    <div>
                                                                        <div><img src="images/menu/cuentaCorriente.png" style="border-width:0px;"></div>
                                                                        <div>Cuentas Corrientes</div>
                                                                    </div>
                                                                </a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </section>
                                </div>

                                <div style="margin-left: 8px; margin-top: 4px;">
                                    <section>
                                        <div>
                                            <div class="formHolderMessage panelMenu" style="height: 153px; padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">
                                                <div style="width:480px; margin-bottom: 10px;">
                                                    <div class="encabezadoMenu"><h2> Operaciones de venta </h2></div>
                                                    <div>
                                                        <div style="width:100%; text-align:center;">
                                                            <div style="float:left; width:100px; padding: 20px 0 0 40px;">
                                                                <a href="./ListaOperacionVenta.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                    <div>
                                                                        <div><img src="images/menu/operacionesVenta.png" style="border-width:0px;"></div>
                                                                        <div>Operaciones de venta</div>
                                                                    </div>
                                                                </a>
                                                            </div>
                                                            <div style="display: inline-block; margin:0 auto; margin-left: 12px; width:100px; padding: 20px 0 0 28px;">
                                                                <div>
                                                                    <a href="./CC.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                        <div>
                                                                            <div><img src="images/menu/historialPago.png" style="border-width:0px;"></div>
                                                                            <div>Historial de pagos</div>
                                                                        </div>
                                                                    </a>
                                                                </div>
                                                            </div>                                        
                                                            <div style="float:right; width:150px; padding: 20px 7px 0 9px;">                                            
                                                                <a href="./Comprobantes.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                    <div>
                                                                        <div><img src="images/menu/comprobantes.png" style="border-width:0px;"/></div>
                                                                        <div>Comprobantes</div>
                                                                    </div>
                                                                </a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </section>
                                </div>
                            </div>
                        </td>

                        <td>
                            <section>
                                <div>
                                    <div class="formHolderMessage panelMenu" style="padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">		
                                        <div style="width:366px;">
                                            <div class="encabezadoMenu"><h2> Información de gestión </h2></div>
                                            <div>
                                                <ul class="listSeccionMenu">
                                                    <li class="liMenu">
                                                        <a href="./CuotasCliente.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/cuotasClientes.png" style="border-width:0px;"></div>
                                                                <div class="titleMenu">Cuotas a vencer por cliente</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu">
                                                        <a href="./CuotasObra.aspx" class="linkMenu">
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/cuotasObras.png" style="border-width:0px;"></div>
                                                                <div class="titleMenu">Saldos a cobrar por obra</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu" style="margin-bottom:20px">
                                                        <a href="./TotalDeudaCliente.aspx" class="linkMenu">
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/totaldeudacliente.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Total de deuda por cliente</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu" style="margin-bottom:20px">
                                                        <a href="./UnidadesVendidas.aspx" class="linkMenu">
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/vendidas.png" style="border-width:0px;"></div>
                                                                <div class="titleMenu">Unidades vendidas</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="liMenu" style="margin-bottom:20px">
                                                        <a href="./ResumenSaldo.aspx" class="linkMenu">
                                                            <div>
                                                                <div style="float:left"><img src="images/menu/controlIndice.png" style="border-width:0px;"/></div>
                                                                <div class="titleMenu">Control de índices</div>
                                                            </div>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>       
                        </td>
                    </tr>
                </table>
            </div>

            <div style="margin-top: -101px; width: 487px; margin-left: 377px;">
                <section>
                    <div class="formHolderMessage panelMenu" style="padding: 0px 15px 12px 6px !important;">
                        <div style="float:left;">
                            <div class="titleMenu" style="margin-left: 47px;">
                                <font class="fontTitleIndice">Dólar:</font>
                                <font class="fontIndice"><asp:Label ID="lbDolarUsrGerencia" runat="server"/></font>
                            </div>
                        </div>
                        <div style="display: inline-block; margin:0 auto;">
                            <div class="titleMenu" style="margin-left: 57px !important;">
                                <font class="fontTitleIndice">UVA:</font>
                                <font class="fontIndice"><asp:Label ID="lbUVAUsrGerencia" runat="server">-</asp:Label></font>
                            </div>
                        </div>
                        <div style="float:right;">
                            <div class="titleMenu" style="margin-right: 42px;">
                                <font class="fontTitleIndice">CAC:</font>
                                <font class="fontIndice"><asp:Label ID="lbCACUsrGerencia" runat="server"/></font>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlVendedor" runat="server" Visible="false">
            <div style="width:100%; text-align:center;">
            <table>
                <tr>
                    <td>
                        <section>
                            <div style="margin-right: -3px;">
                                <div class="formHolderMessage panelMenu" style="height: 393px; padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">		
                                    <div style="width:366px;">
                                        <div class="encabezadoMenu"><h2> Obras </h2></div>
                                        <div>
                                            <ul class="listSeccionMenu">
                                                <li class="liMenu">
                                                    <a href="./Proyecto.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                        <div>
                                                            <div style="float:left"><img src="images/menu/obras.png" style="border-width:0px;"></div>
                                                            <div class="titleMenu">Listado de Obras</div>
                                                        </div>
                                                    </a>
                                                </li>
                                                <li class="liMenu">
                                                    <a href="./ResumenObra.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                        <div>
                                                            <div style="float:left"><img src="images/menu/resumenObras.png" style="border-width:0px;"></div>
                                                            <div class="titleMenu">Resumen de Obras</div>
                                                        </div>
                                                    </a>
                                                </li>
                                                <li class="liMenu" style="margin-right: 20px !important;">
                                                    <a href="#" class="linkMenu"><span class="clientes-dir"></span>
                                                        <div>
                                                            <div style="float:left"><img src="images/menu/unidades.png" style="border-width:0px;"></div>
                                                            <div class="titleMenu" style="margin-top: 5px;">
                                                                <div>Listado de unidades / precios de </div>
                                                                <div style="margin-top: 4px;">
                                                                    <asp:DropDownList ID="cbProyectosVendedor" runat="server" CssClass="selectMenu"></asp:DropDownList>
                                                                    <label><asp:Button ID="btnProyectosVendedor" Text="Ir" CssClass="buttonMenu" runat="server" OnClick="btnProyectosVendedor_Click"/></label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </a>
                                                </li>
                                                <li class="liMenu">
                                                    <a href="./ListadoReserva.aspx" class="linkMenu">
                                                        <div>
                                                            <div style="float:left"><img src="images/menu/reserva.png" style="border-width:0px;"></div>
                                                            <div class="titleMenu">Reservas</div>
                                                        </div>
                                                    </a>
                                                </li>
                                                <li class="liMenu" style="margin-bottom:20px">
                                                    <a href="./Historial.aspx" class="linkMenu">
                                                        <div>
                                                            <div style="float:left"><img src="images/menu/historial.png" style="border-width:0px;"></div>
                                                            <div class="titleMenu">Evolución de unidades</div>
                                                        </div>
                                                    </a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </td>

                    <td>
                        <div style="margin-right: 5px;">
                            <div style="margin-left: 8px; margin-bottom: -25px;">
                                <section>
                                    <div>
                                        <div class="formHolderMessage panelMenu" style="height: 136px; padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">
                                            <div style="width:480px; margin-bottom: 10px;">
                                                <div class="encabezadoMenu"><h2> Clientes </h2></div>
                                                <div>
                                                    <div style="width:100%; text-align:center;">
                                                        <div style="float:left; width:100px; padding: 20px 0 0 116px !important;">
                                                            <a href="./Agenda.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                <div>
                                                                    <div><img src="images/menu/directorio.png" style="border-width:0px;"></div>
                                                                    <div>Directorio</div>
                                                                </div>
                                                            </a>
                                                        </div>

                                                        <div style="float:right; width:150px; padding: 20px 80px 0 25px; !important;">
                                                            <a href="./CuentaCorriente.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                <div>
                                                                    <div><img src="images/menu/cuentaCorriente.png" style="border-width:0px;"></div>
                                                                    <div>Cuentas Corrientes</div>
                                                                </div>
                                                            </a>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </section>
                            </div>

                            <div style="margin-left: 8px; margin-top: 4px;">
                                <section>
                                    <div>
                                        <div class="formHolderMessage panelMenu" style="height: 153px; padding-top: 22px !important; padding-left: 0px !important; padding-right: 0px !important;">
                                            <div style="width:480px; margin-bottom: 10px;">
                                                <div class="encabezadoMenu"><h2> Operaciones de venta </h2></div>
                                                <div>
                                                    <div style="width:100%; text-align:center;">
                                                        <div style="float:left; width:100px; padding: 20px 0 0 40px;">
                                                            <a href="./ListaOperacionVenta.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                <div>
                                                                    <div><img src="images/menu/operacionesVenta.png" style="border-width:0px;"></div>
                                                                    <div>Operaciones de venta</div>
                                                                </div>
                                                            </a>
                                                        </div>
                                                        <div style="display: inline-block; margin:0 auto; margin-left: 12px; width:100px; padding: 20px 0 0 28px;">
                                                            <div>
                                                                <a href="./CC.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                    <div>
                                                                        <div><img src="images/menu/historialPago.png" style="border-width:0px;"></div>
                                                                        <div>Historial de pagos</div>
                                                                    </div>
                                                                </a>
                                                            </div>
                                                        </div>                                        
                                                        <div style="float:right; width:150px; padding: 20px 7px 0 9px;">                                            
                                                            <a href="./Comprobantes.aspx" class="linkMenu"><span class="clientes-dir"></span>
                                                                <div>
                                                                    <div><img src="images/menu/comprobantes.png" style="border-width:0px;"/></div>
                                                                    <div>Comprobantes</div>
                                                                </div>
                                                            </a>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </section>
                            </div>
                        </div>
                    </td>

                    <td>
                        <div>
                            <div style="margin-top: -29px;">
                                <section>
                                    <div class="formHolderMessage panelMenu" style="padding: 2px 15px 12px 6px !important;">
                                        <div style="float:left;">
                                            <div class="titleMenu">
                                                <font class="fontTitleIndice">Dólar:</font>
                                                <font class="fontIndice"><asp:Label ID="lbDolarVendedor" runat="server"/></font>
                                            </div>
                                        </div>
                                        <div style="display: inline-block; margin:0 auto;">
                                            <div class="titleMenu">
                                                <font class="fontTitleIndice">UVA:</font>
                                                <font class="fontIndice"><asp:Label ID="lbUVAVendedor" runat="server">-</asp:Label></font>
                                            </div>
                                        </div>
                                        <div style="float:right;">
                                            <div class="titleMenu">
                                                <font class="fontTitleIndice">CAC:</font>
                                                <font class="fontIndice"><asp:Label ID="lbCACVendedor" runat="server"/></font>
                                            </div>
                                        </div>
                                    </div>
                                </section>
                            </div>
                        </div>
                    </td>
            </tr>
            </table>
        </div>
        </asp:Panel>
    </div>

</div>

<%--<asp:Panel ID="pnlPendientes" runat="server" Visible="false">
    <crm:Pendientes runat="server" ID="Pendientes" />
</asp:Panel>--%>
</asp:Content>