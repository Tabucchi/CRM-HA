<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CC.aspx.cs" Inherits="crm.CC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
    <link href="css/orange.css" rel="stylesheet" />

    <script type="text/javascript">
        // simple redirect to your detail page, passing the selected ID 
        function redir(id) {
            window.location.href = "DetalleCuota2.aspx?idCC=" + id;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    
    <section>
        <div class="headOptions">
            <h2>Historial de Pagos</h2>
        </div>

        <asp:Panel runat="server" DefaultButton="btnBuscar">
        <div class="formHolder" id="searchBoxTop">
            <div>
                <label class="col3">
                    <span>Obra</span>
                    <asp:DropDownList ID="ddlObra" runat="server" Height="32px"></asp:DropDownList>
                </label>

                <label class="col3">
                    <span style="margin-right: -2px; margin-left: 12px; padding-top: 2px;">Cliente</span>
                    <asp:DropDownList ID="ddlEmpresa" Width="58%" Height="34px" runat="server">
                        <asp:ListItem Text="Todas" Value="0" />
                    </asp:DropDownList>
                </label>
                <label class="col3">
                    <span>Estado</span>
                    <asp:DropDownList ID="ddlEstado" runat="server" Height="32px"></asp:DropDownList>
                </label>
            </div>
            <div>
                <label style="width:33%">
                    <span>Moneda/índice</span>
                    <asp:DropDownList ID="cbMonedaIndice" runat="server" Height="32px"></asp:DropDownList>
                </label>
                <label class="col3 rigthLabel" style="margin-right: 38px !important">
                    <asp:Button ID="btnBuscar" Text="Buscar" CssClass="formBtnNar" runat="server" onclick="btnBuscar_Click" />
                </label>
            </div>
        </div>
        </asp:Panel>
    </section>
                    
    <asp:ListView ID="lvCC" runat="server">
        <LayoutTemplate>
            <section>            
                <table style="width:100%">                            
                    <thead id="tableHead">
                        <tr>     
                            <td style="width: 20px; text-align:center">CLIENTE</td>                         
                            <td style="width: 30px; text-align:center">OBRA</td>   
                            <td style="width: 10px; text-align:center">MONEDA ACORDADA</td>
                            <td style="width: 10px; text-align:center">VALOR DE VENTA (PESOS)</td>
                            <td style="width: 10px; text-align:center">SALDO (PESOS)</td>
                            <td style="width: 5px;  text-align:center">ESTADO</td>
                            <td style="width: 5px;  text-align:center"></td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>  
                    <tfoot class="footerTable">
                        <tr>
                            <td style="width: 3%;"></td>
                            <td style="width: 3%;"></td>
                            <td style="width: 3%;"></td>
                            <td style="width: 3%; text-align:right"><a href="#" alt="Monto Acordado" class="tooltipTop tooltipColor" style="margin-left: -8px;"><b><asp:Label ID="lbValorVenta" runat="server"></asp:Label></b></a></td>
                            <td style="width: 3%; text-align:right"><a href="#" alt="Saldo" class="tooltipTop tooltipColor" style="margin-left: -8px;"><b><asp:Label ID="lbSaldo" runat="server"></asp:Label></b></a></td>
                            <td style="width: 1%;"></td>
                            <td style="width: 1%;"></td>
                        </tr>
                    </tfoot>                        
                </table>
            </section>   
        </LayoutTemplate>
                
        <ItemTemplate>                   
            <tr onclick="redir('<%# Eval("id") %>');" style="cursor: pointer">
                <td style="text-align:center">
                    <a href="#" alt="Cliente" class="tooltip tooltipColor">
                        <asp:Label ID="Label3" runat="Server" Text='<%#Eval("GetEmpresa") %>' />
                    </a>
                </td>
                <td style="text-align:center">
                    <a href="#" alt="Obra" class="tooltip tooltipColor">
                        <asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("GetProyecto") %>' />
                    </a>
                </td>                
                <td style="text-align:center">
                    <a href="#" alt="Moneda Acordada" class="tooltip tooltipColor">
                        <asp:Label ID="Label6" runat="Server" Text='<%#Eval("GetMoneda") %>' />
                    </a>
                </td>
                <td style="text-align:right">
                    
                        <asp:Label ID="lbTotalPesos" runat="Server" Text='<%#Eval("GetTotalPesos") %>' />
                    
                </td>
                <td style="text-align:right">
                    <a href="#" alt="Saldo" class="tooltip tooltipColor">
                        <asp:Label ID="lbSaldoPesos" runat="Server" Text='<%#Eval("GetSaldoPesos") %>' />
                    </a>
                </td>
                <td style="text-align:center">
                    <a href="#" alt="Estado" class="tooltip tooltipColor">
                        <asp:Label ID="Label4" runat="Server" Text='<%#Eval("GetEstado") %>' />
                    </a>
                </td>
                <td>
                    <a class="detailBtn" href="DetalleCuota2.aspx?idCC=<%# Eval("id") %>"></a>
                </td>
            </tr>
        </ItemTemplate>
                                       
        <EmptyDataTemplate>
            <section>
                <table id="Table1" style="width: 100%" runat="server">
                    <tr>
                        <td align="center"><b>No se encontraron historiales de pagos registradas.<b/></td>
                    </tr>
                </table>
            </section>
        </EmptyDataTemplate>
    </asp:ListView>

</asp:Content>
