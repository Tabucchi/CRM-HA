<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Compras" Codebehind="Compras.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="ajax" Namespace="System.Globalization" Assembly="mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>           
    <div class="pal grayArea uiBoxGray noborder">
        <div>
            <table>            
                <tr>
                    <td align="left"><b>Filtro:</b></td>
                </tr>
                <tr>                 
                    <td align="right">&nbsp;&nbsp;&nbsp;&nbsp;EMPRESA</td>
                    <td align="left" width="10%">
                        <asp:DropDownList ID="cbEmpresa" Width="150px" runat="server"></asp:DropDownList>
                    </td>
                    <td align="right">FECHA DESDE</td>
                    <td align="left">
                        <asp:TextBox ID="txtFechaDesde" runat="server" Width="100px"></asp:TextBox>&nbsp;
                        <img id="imgCalendarH" src="Imagenes/iconCalendar.gif" width="16" height="16" style="cursor:pointer" alt="" />    
                    </td>
                    <td>HASTA</td>
                    <td align="left">
                        <asp:TextBox ID="txtFechaHasta" runat="server" Width="100px"></asp:TextBox>&nbsp;
                        <img id="imgCalendarD" src="Imagenes/iconCalendar.gif" width="16" height="16" style="cursor:pointer" alt="" />  
                    </td>
                    <td><asp:Button ID="btnBuscar" Text="Buscar" class="boton" runat="server" 
                            onclick="btnBuscar_Click" />
                    </td>
                </tr>
            </table>
        </div>
   
        <asp:ListView ID="lvCompras" runat="server">
            <LayoutTemplate>
                <div class="PrettyGrid"> 
                    <div style="overflow:auto; height:375px">             
                    <table border="0" cellpadding="1">                            
                        <thead>
                            <tr>
                                <th style="width:8%; height:20px;" class="column_head">PEDIDO</th>                              
                                <th style="width: 15%" class="column_head">EMPRESA</th>
                                <th style="width: 15%" class="column_head">NOMBRE</th>
                                <th style="width: 20%" class="column_head">DESCRIPCION</th>
                                <th style="width: 6%" class="column_head">CANTIDAD</th>
                                <th style="width: 6%" class="column_head">COSTO</th>
                                <th style="width: 6%" class="column_head">PRECIO</th>
                                <th style="width: 8%" class="column_head">FECHA</th>
                                <th style="width: 10%" class="column_head">USUARIO</th>
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
                <tr style="background-color:#FFFFFF; cursor:pointer;" onclick="Visible(<%# Eval("idPedido")%> )">
                    <td><asp:Label ID="lbPedido" runat="Server" Text='<%#Eval("idPedido") %>' Enabled="False" /></td>
                    <td align="left"><asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresaNombre") %>' /></td>
                    <td><asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("Nombre") %>' /></td> 
                    <td><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("Descripcion") %>' /></td> 
                    <td><asp:Label ID="lbCantidad" runat="server" Text='<%#Eval("Cantidad") %>' /></td>
                    <td><asp:Label ID="lbCosto" runat="server" Text='<%#Eval("Costo") %>' /></td>
                    <td><asp:Label ID="lbPrecio" runat="server" Text='<%#Eval("precioCliente") %>' /></td>  
                    <td><asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:d}") %>' /></td>
                    <td><asp:Label ID="lbUsuario" runat="server" Text='<%#Eval("GetUsuario") %>' /></td>
                </tr>
            </ItemTemplate>
                                                 
            <AlternatingItemTemplate>
                <tr style="background-color:#EFEFEF; cursor:pointer;" onclick="Visible(<%# Eval("idPedido")%> )">
                    <td><asp:Label ID="lbPedido" runat="Server" Text='<%#Eval("idPedido") %>' /></td>
                    <td align="left"><asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresaNombre") %>' /></td>
                    <td><asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("Nombre") %>' /></td>   
                    <td><asp:Label ID="lbDescripcion" runat="server" Text='<%#Eval("Descripcion") %>' /></td> 
                    <td><asp:Label ID="lbCantidad" runat="Server" Text='<%#Eval("Cantidad") %>' /></td>   
                    <td><asp:Label ID="lbCosto" runat="server" Text='<%#Eval("Costo") %>' /></td>
                    <td><asp:Label ID="lbPrecio" runat="server" Text='<%#Eval("precioCliente") %>' /></td>
                    <td><asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:d}") %>' /></td>
                    <td><asp:Label ID="lbUsuario" runat="server" Text='<%#Eval("GetUsuario") %>' /></td>
                </tr>             
            </AlternatingItemTemplate>  
                          
            <EmptyDataTemplate>
                <table id="Table1" width="100%" runat="server">
                    <tr>
                        <td align="center"><b>No data found.<b/></td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
        <br />
    </div>
</div>
</asp:Content>

