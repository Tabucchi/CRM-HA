<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageCliente.master" AutoEventWireup="true" Inherits="Clientes" Codebehind="Clientes.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section>   
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    
    <div align="left">
        <h2>Bienvenido   
        <asp:Label ID="lbUsuario" runat="server"></asp:Label>            
        </h2>
    </div>
       
    <asp:Panel ID="pnlFiltro" runat="server" DefaultButton="btnBuscar">
        <div class="formHolder" id="searchBoxTop" style="overflow:visible;">
            <div class="formHolderLine">
                <h2>Filtro: </h2>
            </div>
            <label class="col3"><span>ESTADO </span> <asp:DropDownList ID="cbEstado" runat="server" ></asp:DropDownList> </label>
            <label class="col3"><span>CLIENTE </span> <asp:DropDownList ID="cbCliente" runat="server" ></asp:DropDownList> </label>
            <label class="col3"><span>PRIORIDAD </span> <asp:DropDownList ID="cbPrioridad" runat="server" ></asp:DropDownList> </label>
                    
            <label class="col3"><span>FECHA DESDE</span>
                <asp:TextBox ID="txtFechaDesde" runat="server"></asp:TextBox>
                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaDesde" Format="dd/MM/yyyy" PopupButtonID="txtFechaDesde"/>
            </label>
            <label class="col3"><span>HASTA</span> 
                <asp:TextBox ID="txtFechaHasta"  runat="server"></asp:TextBox>
                <ajax:CalendarExtender ID="ceFechaHasta" runat="server" TargetControlID="txtFechaHasta" Format="dd/MM/yyyy" PopupButtonID="txtFechaHasta" />
            </label>
            <label class="col3"> <asp:Button  class="formBtnNar"  style="float:left;" ID="btnBuscar" Text="Buscar" runat="server"/> </label>
            <div style="clear:both;"></div>
        </div>
    </asp:Panel>       
       
    <asp:ListView ID="lvClientes" runat="server" onprerender="lvClientes_PreRender">
        <LayoutTemplate>     
            <table border="0" cellpadding="1">                            
                <thead id="listHead">
                    <tr>
                        <td style="width: 6%; height:20px;" class="column_head">ID</td>                              
                        <td style="width: 10%" class="column_head">CLIENTE</td>
                        <td style="width: 6%" class="column_head">FECHA</td>
                        <td style="width: 18%" class="column_head">TITULO</td>
                        <td style="width: 8%" class="column_head">PRIORIDAD</td>
                        <td style="width: 10%" class="column_head">FECHA DE CIERRE</td>
                        <td style="width: 8%" class="column_head">ESTADO</td>
                        <td style="width: 7%" class="column_head"></td>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </tbody>                        
            </table>                
        </LayoutTemplate>
        <ItemTemplate>                   
            <tr style="background-color:#FFFFFF; cursor:pointer;" onclick="Visible(<%# Eval("id")%> )">
                <td><asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' Enabled="False" /></td>
                <td align="left"><asp:Label ID="lbCliente" runat="Server" Text='<%#Eval("GetClienteNombre") %>' /></td>
                <td><asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:d}") %>' /></td>
                <td><asp:Label ID="lbTitulo" runat="Server" Text='<%#Eval("titulo") %>' /></td>   
                    <td><asp:Label ID="lbPrioridad" runat="Server" Text='<%#Eval("GetPrioridad") %>' /></td>
                <td><asp:Label ID="lbFechaCierre" runat="Server" Text='<%#Eval("Fecha") %>' /></td>
                <td><asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>' /></td>
                <td style="cursor:pointer;">
                        <asp:LinkButton ID="lnkDetalles" class="detailBtn" Text="Detalles" OnClick="lnkDetalles_Click" CommandArgument=<%# Eval("id") %> runat="server"></asp:LinkButton>
                </td>  
            </tr>
        </ItemTemplate>
                                    
        <AlternatingItemTemplate>
            <tr style="background-color:#EFEFEF; cursor:pointer;" onclick="Visible(<%# Eval("id")%> )">
                <td><asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' /></td>
                <td align="left"><asp:Label ID="lbCliente" runat="Server" Text='<%#Eval("GetClienteNombre") %>' /></td>
                <td><asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("fecha", "{0:d}") %>' /></td>
                <td><asp:Label ID="lbTitulo" runat="Server" Text='<%#Eval("titulo") %>' /></td>   
                <td><asp:Label ID="lbPrioridad" runat="Server" Text='<%#Eval("GetPrioridad") %>' /></td>
                <td><asp:Label ID="lbFechaCierre" runat="Server" Text='<%#Eval("Fecha") %>' /></td>
                <td><asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>' /></td>                   
                <td style="cursor:pointer;">
                    <asp:LinkButton ID="lnkDetalles" class="detailBtn" Text="Detalles" OnClick="lnkDetalles_Click" CommandArgument=<%# Eval("id") %> runat="server"></asp:LinkButton>
                </td>  
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

    <div class="formHolder">
        <label><asp:linkButton ID="btnExcel" style="float:left" class="formBtnNar" runat="server" Text="Exportar Todo a Excel" onclick="btnExcel_Click"/></label>
    </div>
</section>
</asp:Content>

