<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>
<%@ Register Src="~/sidebar.ascx" TagName="sidebar" TagPrefix="crm" %>
<%@ Register Src="~/Controles/MensajeIndiceCac.ascx" TagPrefix="crm" TagName="MensajeIndiceCac" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="maincol" >  
    <asp:Panel ID="pnlIndiceCAC" runat="server" Visible="false" >
        <crm:MensajeIndiceCac runat="server" id="MensajeIndiceCac" />
    </asp:Panel>
        
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
                <tr id="row<%# Eval("id")%>" style="color:#b40b0b;">
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
   
    </div>
<crm:sidebar runat="server" />
</asp:Content>
