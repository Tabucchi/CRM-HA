<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ListaOperacionVenta.aspx.cs" Inherits="crm.ListaOperacionVenta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function redir(id) {
            window.location.href = "DetalleOperacionVenta.aspx?idOV=" + id;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    
    <section>
        <div class="formHolder" id="searchBoxTop">
            <div class="headOptions headLine">
                <a href="#" class="toggleFiltr" style="margin-top: 9px; margin-right:5px">v</a>
                <h2>Operaciones de venta</h2>
                <asp:Panel ID="pnlNuevoOV" runat="server" Visible="false">
                    <div style="float:right;">
                        <div style="float:right">
                            <b><asp:LinkButton ID="btnNuevoOV" runat="server"  CssClass="formBtnGrey" Text="Agregar nueva operación de venta" OnClick="btnNuevoOV_Click"></asp:LinkButton></b>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="hideOpt" style="width: 100%;">
                <label class="col3">
                    <span>CLIENTE</span>
                    <asp:DropDownList ID="cbEmpresa" runat="server"/>
                </label>

                <label class="col3">
                    <span>OBRA</span>
                    <asp:DropDownList ID="cbProyectos" runat="server" style="width:245px"/>
                </label>  

                <label class="col3">
                    <span>ESTADO</span>
                    <asp:DropDownList ID="cbEstado" runat="server" style="width:245px"/>
                </label> 
            </div>  
            <div class="hideOpt footerLine" style="width: 100%;">
                <label class="leftLabel" style="width: 17%;">
                    <asp:Button ID="btnBuscar" Text="Buscar" CssClass="formBtnNar" runat="server" OnClick="btnBuscar_Click" style="margin-right: 32px !important;"/>
                    <asp:Button ID="btnVerTodos" Text="Ver Todos" CssClass="formBtnGrey1" runat="server" Onclick="btnVerTodos_Click"/>
                </label>
            </div>
        </div>
    </section>

    <asp:ListView ID="lvOperacionVenta" runat="server" OnLayoutCreated="lvOperacionVenta_LayoutCreated">
        <LayoutTemplate>
            <section>            
                <table style="width:100%">                            
                    <thead id="tableHead">
                        <tr>     
                            <td style="width: 20%; text-align: left">CLIENTE</td>
                            <td style="width: 20%; text-align: left">OBRA</td>                            
                            <td style="width: 12%; text-align: center">PRECIO BASE UNIDAD</td>
                            <td style="width: 12%; text-align: center">PRECIO ACORDADO</td>
                            <td style="width: 12%; text-align: center">MONEDA ACORDADA</td>
                            <td style="width: 12%; text-align: center">PRECIO ACORDADO A PESOS</td>
                            <td style="width: 10%; text-align: center">ESTADO</td>
                            <td style="width: 5%;"></td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody> 
                    <tfoot class="footerTable">
                        <tr>
                            <td style="width: 1%;"></td>
                            <td style="width: 6%;"></td>
                            <td style="width: 9%; text-align: right"><a href="#" alt="Precio Base" class="tooltipTop tooltipColor" style="margin-left: -8px;"><b><asp:Label ID="lbPrecioBase" runat="server"></asp:Label></b></a></td>
                            <td style="width: 4%; text-align: right"><%--<a href="#" alt="Precio Acordado" class="tooltipTop tooltipColor" style="margin-left: -8px;"><b><asp:Label ID="lbTotalPrecioAcordado" runat="server"></asp:Label></b></a>--%></td>
                            <td style="width: 1%;"></td>
                            <td style="width: 4%; text-align: right"><a href="#" alt="Precio Acordado a pesos" class="tooltipTop tooltipColor" style="margin-left: -8px;"><b><asp:Label ID="lbTotalPrecioAcordadoPesos" runat="server"></asp:Label></b></a></td>
                            <td style="width: 1%;"></td>
                            <td style="width: 1%;"></td>
                        </tr>
                    </tfoot>                       
                </table>
            </section>   
        </LayoutTemplate>
                
        <ItemTemplate>                   
            <tr onclick="redir('<%# Eval("id") %>');" style="cursor: pointer">
                <td style="text-align: left">
                    <asp:Label ID="Label1" runat="Server" Text='<%#Eval("GetEmpresa") %>' />
                </td>
                <td style="text-align: left">
                    <asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("GetProyecto") %>' />
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label4" runat="Server" Text='<%#Eval("GetPrecioBase") %>' />
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lbPrecioAcordado" runat="Server" Text='<%#Eval("GetPrecioAcordado") %>' />
                </td>
                <td style="text-align: center">
                    <asp:Label ID="Label2" runat="Server" Text='<%#Eval("GetMoneda") %>' />
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lbPrecioAcordadoPesos" runat="Server" Text='<%#Eval("GetPrecioAcordadoAPesos") %>' />
                </td>
                <td style="text-align: center">
                    <asp:Label ID="Label3" runat="Server" Text='<%#Eval("GetEstado") %>' />
                </td>
                <td>
                    <a class="detailBtn" href="DetalleOperacionVenta.aspx?idOV=<%# Eval("id") %>"></a>
                </td>
            </tr>
        </ItemTemplate>
                                       
        <EmptyDataTemplate>
            <section>
                <table id="Table1" style="width:100%" runat="server">
                    <tr>
                        <td style="text-align: center"><b>No se encontraron operaciones de venta registradas.<b/></td>
                    </tr>
                </table>
            </section>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Content>
