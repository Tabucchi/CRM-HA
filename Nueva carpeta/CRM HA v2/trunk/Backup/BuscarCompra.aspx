<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="BuscarCompra" Codebehind="BuscarCompra.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style5
        {
            width: 210px;
        }
        .style6
        {
            width: 212px;
        }
        .style7
        {
            width: 17px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>   
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <asp:ObjectDataSource ID="odsProveedor" runat="server" SelectMethod="GetListaProveedor" TypeName="cProveedor"></asp:ObjectDataSource>
    <div class="pal grayArea uiBoxGray noborder" style="height:536px">
        <table width="100%">
            <tr>
                <td height="35px" class="titulo1" colspan="4"><b>Compras</b></td> 
                <td align="right">
                    <asp:Button ID="btnNuevaCompra" runat="server" Text="Solicitud de Nueva Compra" class="boton" onclick="btnNuevaCompra_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="8"><b><hr/></b></td>
            </tr>
        </table>
        <div>
            <asp:Panel ID="pnlFiltro" runat="server" DefaultButton="btnBuscar">
                <table width="100%">            
                    <tr>
                        <td align="left"><b>Filtro:</b></td>
                    </tr>
                    <tr>
                        <td align="right" width="70px">EMPRESA</td>
                        <td align="left" class="style6">
                            <asp:DropDownList ID="cbEmpresa" runat="server" Width="200px" />
                        </td>
                        <td align="right" class="style7" style="padding-left:15px">
                            ESTADO</td>
                        <td align="left" width="250px;">
                            <asp:DropDownList ID="cbEstado" runat="server" Width="200px">
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Button ID="btnBuscar" runat="server" class="boton" 
                                onclick="btnBuscar_Click" Text="Buscar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>        
        <asp:ListView ID="lvCompra" runat="server" DataKeyNames="id" 
            onprerender="lvCompra_PreRender">
            <LayoutTemplate>
                <div class="PrettyGrid">                
                    <div style="overflow:auto; height:367px">             
                    <table border="0" cellpadding="1">                            
                        <thead>
                            <tr>
                                <th style="width: 8%" class="column_head">NRO. COMPRA</th>
                                <th style="width: 8%; height:20px;" class="column_head">FECHA</th>                             
                                <th style="width: 8%" class="column_head">PEDIDO</th>
                                <th style="width: 10%" class="column_head">USUARIO</th>
                                <th style="width: 10%" class="column_head">EMPRESA</th>
                                <th style="width: 10%" class="column_head">CLIENTE</th>
                                <th style="width: 8%" class="column_head">ESTADO</th>
                                <th style="width: 8%" class="column_head">PRECIO PROVEEDOR</th>
                                <th style="width: 8%" class="column_head">PRECIO CLIENTE</th>
                                <th style="width: 8%" class="column_head"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>                        
                    </table>
                    </div>                   
                </div>
            </LayoutTemplate>
            <ItemTemplate>                   
                <tr style="background-color:#FFFFFF; cursor:pointer;" onclick="Visible(<%# Eval("Id")%> )">
                    <td><asp:Label ID="lbId" runat="Server" Text='<%#Eval("Id") %>' Enabled="False" /></td>
                    <td><asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:d}") %>' /></td> 
                    <td><asp:Label ID="lbCantidad" runat="Server" Text='<%#Eval("idPedido") %>' /></td>
                    <td><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("GetUsuario") %>' /></td>
                    <td><asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>' /></td>
                    <td><asp:Label ID="lbCliente" runat="Server" Text='<%#Eval("GetCliente") %>' /></td>
                    <td><asp:Label ID="lbImporte" runat="Server" Text='<%#Eval("GetEstado") %>' /></td>
                    <%--<td><asp:Label ID="Label2" runat="Server" Text='<%#Eval("nroPedidoProveedor") %>' /></td> --%>
                    <td><asp:Label ID="lbTotalProveedor" runat="Server" Text='<%#Eval("TotalProveedor") %>'/></td> 
                    <td><asp:Label ID="lbTotalCliente" runat="Server" Text='<%#Eval("TotalCliente") %>'/></td> 
                    <td><a href="Compra.aspx?id=<%# Eval("id") %>">Ver</a></td>
                </tr>
            </ItemTemplate>
                                       
            <AlternatingItemTemplate>
                <tr style="background-color:#EFEFEF; cursor:pointer;" onclick="Visible(<%# Eval("Id")%> )">
                    <td><asp:Label ID="lbId" runat="Server" Text='<%#Eval("Id") %>' Enabled="False" /></td>
                    <td><asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:d}") %>' /></td> 
                    <td><asp:Label ID="lbCantidad" runat="Server" Text='<%#Eval("idPedido") %>' /></td>
                    <td><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("GetUsuario") %>' /></td>
                    <td><asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>' /></td> 
                    <td><asp:Label ID="lbCliente" runat="Server" Text='<%#Eval("GetCliente") %>' /></td>
                    <td><asp:Label ID="lbImporte" runat="Server" Text='<%#Eval("GetEstado") %>' /></td> 
                    <td><asp:Label ID="lbTotalProveedor" runat="Server" Text='<%#Eval("TotalProveedor")%>'/></td> 
                    <td><asp:Label ID="lbTotalCliente" runat="Server" Text='<%#Eval("TotalCliente") %>'/></td> 
                    <td><a href="Compra.aspx?id=<%# Eval("id") %>">Ver</a></td>
                    </td>
                </tr>             
            </AlternatingItemTemplate>
        </asp:ListView>
            
        <asp:Panel ID="pnlTotales" runat="server">
            <table width="100%">
                <tr>
                    <td align="left" style="padding-left: 5px; width:158px" >
                        <br />
                        <asp:Label ID="lbTituloTotalProveedor" runat="server" Text="Total Proveedor: $" Font-Bold="True" Font-Size="Small"></asp:Label>
                        <asp:Label ID="lbTotalProveedor" runat="server" Font-Bold="True" Font-Size="Small" Visible="true"></asp:Label>
                    </td>
                    <td align="left" style="padding-left: 5px; width:158px">
                        <br />
                        <asp:Label ID="lbTituloTotalCliente" runat="server" Text="Total Cliente: $" Font-Bold="True" Font-Size="Small"></asp:Label>
                        <asp:Label ID="lbTotalCliente" runat="server" Font-Bold="True" Font-Size="Small" Visible="true"></asp:Label>
                    </td>
                    <td align="left" style="padding-left: 5px">
                        <br />
                        <asp:Label ID="lbTituloDiferencia" runat="server" Text="Diferencia: $" Font-Bold="True" Font-Size="Small"></asp:Label>
                        <asp:Label ID="lbTotalDiferencia" runat="server" Font-Bold="True" Font-Size="Small" Visible="true"></asp:Label>
                    </td>
                    <td align="right" style="padding-top:12px">
                        Paginas:
                        <asp:DataPager ID="_moviesGridDataPager" PageSize="20" runat="server" PagedControlID="lvCompra">
                            <Fields><asp:NumericPagerField/></Fields>
                        </asp:DataPager>
                    </td>
                </tr>
            </table>
        </asp:Panel>
            
        <table width="100%">        
            <tr><td colspan="3"><hr /></td></tr>
            <tr>
                <td align="left" colspan="2"><asp:Button ID="btnProveedor" runat="server" 
                        Text="Proveedores" class="boton" onclick="btnProveedor_Click"/></td>
            </tr>
        </table>
    </div>
</div>
</asp:Content>

