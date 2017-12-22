<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>
<%@ Register Src="~/sidebar.ascx" TagName="sidebar" TagPrefix="crm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<div id="maincol" >  
    <section>
        <h2> Mis Pendientes <asp:Label ID="lbCantRegistros" CssClass="notfCount" runat="server"></asp:Label> </h2>
        <asp:ListView ID="lvMisPedidos" runat="server">
            <LayoutTemplate>
                        <table border="0" cellpadding="1">
                            <thead>
                                <tr>
                                    <td style="width: 9%; height: 20px;" class="column_head">
                                        ID
                                    </td>
                                    <td style="width: 20%" class="column_head">
                                        CLIENTE
                                    </td>
                                    <td style="width: 10%" class="column_head">
                                        FECHA
                                    </td>
                                    <td style="width: 33%" class="column_head">
                                        TITULO
                                    </td>
                                    <td style="width: 10%" class="column_head">
                                        PRIORIDAD
                                    </td>
                                    <td style="width: 10%" class="column_head">
                                        ESTADO
                                    </td>
                                    <td style="width: 8%" class="column_head">
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </tbody>
                        </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr id="row<%# Eval("id")%>" style="background-color: #FFFFFF;">
                    <td>
                        <%# Eval("id") %>
                    </td>
                    <td>
                        <%# Eval("GetClienteNombre")%>
                        de <b>
                            <%# Eval("GetEmpresa")%></b>
                    </td>
                    <td>
                        <%# Eval("Fecha", "{0:d}") %>
                    </td>
                    <td>
                        <%# Eval("Titulo") %>
                    </td>
                    <td>
                        <%# Eval("GetPrioridad")%>
                    </td>
                    <td>
                        <%# Eval("GetEstado") %>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkDetalles" Text="Detalles" class="detailBtn" OnClick="lnkDetalles_Click" CommandArgument='<%# Eval("id") %>'
                            runat="server"></asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <table id="Table1" width="100%" runat="server">
                    <tr>
                        <td align="center" valign="middle">
                            No tienes tickets    asignados.
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
    </section>
    <section>
        <h2>Próximos Vencimientos</h2>
        <asp:ListView ID="lvVencimientos" runat="server">
            <LayoutTemplate>
                        <table>
                            <thead>
                                <tr>
                                    <td>
                                        ID
                                    </td>
                                    <td >
                                        CLIENTE
                                    </td>
                                    <td >
                                        FECHA
                                    </td>
                                    <td >
                                        TITULO
                                    </td>
                                    <td >
                                        PRIORIDAD
                                    </td>
                                    <td>
                                        VENCIMIENTO
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </tbody>
                        </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr id="row<%# Eval("id")%>" style="color:#f90;">
                    <td>
                        <%# Eval("id") %>
                    </td>
                    <td >
                        <%# Eval("GetClienteNombre")%>
                        de <b>
                            <%# Eval("GetEmpresa")%></b>
                    </td>
                    <td>
                        <%# Eval("Fecha", "{0:d}") %>
                    </td>
                    <td >
                        <%# Eval("Titulo") %>
                    </td>
                    <td>
                        <%# Eval("GetPrioridad")%>
                    </td>
                    <td>
                        <%# Eval("GetDiasVencidos")%>
                    </td>
                    <td >
                        <asp:LinkButton ID="lnkDetalles" Text="Detalles" OnClick="lnkDetalles_Click" class="detailBtn" CommandArgument='<%# Eval("id") %>'
                            runat="server"></asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <table id="Table1" width="100%" runat="server">
                    <tr>
                        <td align="center" valign="middle">
                            <font style="font-family: Calibri; font-weight: bold; font-size: 18px;">No hay Tickets
                                Vencidos.</font>
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
    </section>
    <section>
        <h2>Compras</h2>
        <asp:ListView ID="lvCompras" runat="server" DataKeyNames="id">
            <LayoutTemplate>
                <table border="0" cellpadding="1">                            
                    <thead>
                        <tr>
                            <td>NRO. COMPRA</td>
                            <td>FECHA</td>                             
                            <td>USUARIO</td>
                            <td>EMPRESA</td>
                            <td>CLIENTE</td>
                            <td>ESTADO</td>
                            <td>PRECIO PROVEEDOR</td>
                            <td>PRECIO CLIENTE</td>
                            <td></td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>                        
                </table>
        </LayoutTemplate>
            <ItemTemplate>                   
            <tr style="background-color:#FFFFFF; cursor:pointer;" onclick="Visible(<%# Eval("Id")%> )">
                <td><asp:Label ID="lbId" runat="Server" Text='<%#Eval("Id") %>' Enabled="False" /></td>
                <td><asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:d}") %>' /></td> 
                <td><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("GetUsuario") %>' /></td>
                <td><asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>' /></td>
                <td><asp:Label ID="lbCliente" runat="Server" Text='<%#Eval("GetCliente") %>' /></td>
                <td><asp:Label ID="lbImporte" runat="Server" Text='<%#Eval("GetEstado") %>' /></td>
                <%--<td><asp:Label ID="Label2" runat="Server" Text='<%#Eval("nroPedidoProveedor") %>' /></td> --%>
                <td><asp:Label ID="lbTotalProveedor" runat="Server" Text='<%#Eval("TotalProveedor") %>'/></td> 
                <td><asp:Label ID="lbTotalCliente" runat="Server" Text='<%#Eval("TotalCliente") %>'/></td> 
                <td><a href="Compra.aspx?id=<%# Eval("id") %>" class="detailBtn">Ver</a></td>
            </tr>
        </ItemTemplate>
        </asp:ListView>
    </section>
    </div>
<crm:sidebar runat="server" />
</asp:Content>
