<%@ Page Language="C#" MasterPageFile="~/MasterPageCotizacion.master" AutoEventWireup="true" Inherits="CotizacionCliente" Title="Página sin título" Codebehind="CotizacionCliente.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>   
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <asp:ObjectDataSource ID="odsProveedor" runat="server" SelectMethod="GetListaProveedor" TypeName="cProveedor"></asp:ObjectDataSource>
    <div class="pal grayArea uiBoxGray noborder">
        <table width="100%">
            <tr>   
                <td colspan="4" align="left" style="padding-left:16px;">
                    <asp:Label ID="lbTituloNroCompra" runat="server" Text="NRO. COMPRA:" Font-Bold="True"></asp:Label>&nbsp;
                    <asp:Label ID="lbNroCompra" runat="server"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <b>CLIENTE:</b> &nbsp;
                    <asp:Label ID="lbEmpresa" runat="server"></asp:Label>&nbsp;
                    (<asp:Label ID="lbCliente" runat="server"></asp:Label>)
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbTituloTicket" runat="server" Text="TICKET:" Font-Bold="True" Visible="false"></asp:Label>
                    <asp:Label ID="lbTicket" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
                       
        <asp:ListView ID="lvNuevaCompra" runat="server" DataKeyNames="id">
            <LayoutTemplate>
                <div class="PrettyGrid">                
                    <div style="overflow:auto; height:250px">             
                    <table border="0" cellpadding="1">                            
                        <thead>
                            <tr>
                                <th style="width: 2%; height:20px;" class="column_head">CANTIDAD</th> 
                                <th style="width: 15%; height:20px;" class="column_head">DESCRIPCION</th>
                                <th style="width: 2%; height:20px;" class="column_head">IMPORTE</th>
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
                <tr style="background-color:#FFFFFF; cursor:pointer;" onclick="Visible(<%#Eval("Id")%> )">
                    <td><asp:Label ID="lbCantidad" runat="Server" Text='<%#Eval("cantidad") %>' /></td>
                    <td><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("descripcion") %>' /></td>
                    <td><asp:Label ID="txtImporteCliente" runat="server" Text='<%#Eval("ImporteCliente") %>' Width="100px"></asp:Label></td>
                </tr>
            </ItemTemplate>
                         
            <AlternatingItemTemplate>
                <tr style="background-color:#EFEFEF; cursor:pointer;" onclick="Visible(<%#Eval("Id")%> )">
                    <td><asp:Label ID="lbCantidad" runat="Server" Text='<%#Eval("cantidad")%>' /></td>
                    <td><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("descripcion")%>' /></td>
                    <td><asp:Label ID="txtImporteCliente" runat="server" Text='<%#Eval("ImporteCliente") %>' Width="100px"></asp:Label></td>
                </tr>             
            </AlternatingItemTemplate>
        </asp:ListView>
                        
        <table width="100%">
            <tr>
                <td align="left" style="padding-left:20px">
                    <b><asp:Label ID="lbIvaIncluido" runat="server" Text="IVA incluido" Visible="false" /></b>
                    <b><asp:Label ID="lbmasIva" runat="server" Text="+ IVA" Visible="false"/></b>
                </td>
            </tr>
            <tr><td colspan="5"><hr /></td></tr>
            <tr>
                <td align="left" style="padding-left:20px" colspan="5"> 
                    <asp:Label ID="lbTituloTotal" runat="server" Text="Total: $" Font-Bold="true" Font-Size="Small"></asp:Label>
                    <asp:Label ID="lbTotal" runat="server" Font-Bold="True" Font-Size="Small" Visible="true"></asp:Label>
                </td>
            </tr>
            <tr><td colspan="5"><hr /></td></tr>   
            <tr>
                <td align="left" style="padding-left:20px" colspan="5"> 
                    <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="FECHA:"></asp:Label> &nbsp;
                    <asp:Label ID="lbFecha" runat="server"></asp:Label>
                </td>
            </tr>
                        
            <tr><td colspan="5"><hr /></td></tr>
            <tr width="700px">
                <td align="left" width="286px" colspan="2"></td>
                <td align="right" style="padding-bottom: 5px" colspan="3"> 
                      
                    <asp:Label ID="lbMensajeAprobado" runat="server" Text="Esta compra fue aprobada" Font-Bold="True" 
                        ForeColor="#B9BE42" Visible="False"></asp:Label>
                                                    
                    <asp:Label ID="lbMensajeRechazada" runat="server" Text="Esta compra fue rechazada" Font-Bold="True" 
                        ForeColor="#B9BE42" Visible="False"></asp:Label>
                        
                    <asp:Button ID="btnAprobado" runat="server" Text="Aprobado" 
                        
                        style="cursor: pointer; font-size: 11px; font-weight: bold; color: #333333; padding: 2px 1px; margin-left: 15px;" 
                        onclick="btnAprobado_Click" />    
                    
                    <asp:Button ID="btnRechazar" runat="server" Text="Rechazar" Enable="false"
                        
                        style="cursor: pointer; font-size: 11px; font-weight: bold; color: #333333; padding: 2px 1px; margin-left: 12px;" 
                        onclick="btnRechazar_Click"/>
                </td>
            </tr>
        </table>
    </div>    
    <div>
        <input type="hidden" id="txtId" runat="server" />
    </div>
</div>
</asp:Content>

