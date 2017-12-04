<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ResumenObra.aspx.cs" Inherits="crm.ResumenObra" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .divHead{width:100%}
        .divColumnLeft{float:left; width:50%}
        .divColumnRight{float:right; width:50%}
    </style>

    <script type="text/javascript">
        // simple redirect to your detail page, passing the selected ID 
        function redir(id) {
            window.location.href = "Unidad.aspx?idProyecto=" + id;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <asp:HiddenField ID="hfTotalDisponible" runat="server" />
    <asp:HiddenField ID="hfTotalReservada" runat="server" />
    <asp:HiddenField ID="hfTotalVendidaSinBoleto" runat="server" />
    <asp:HiddenField ID="hfTotalVendida" runat="server" />
    <asp:HiddenField ID="hfTotal" runat="server" />

    <section>
        <div class="headOptions">
            <div style="float:left"><h2>Resumen de Obras</h2></div>
            <div style="float:right">
                <label class="rigthLabel" style="width: 37%;">
                    <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="formBtnNar" OnClick="btnDescargar_Click" />
                </label>
            </div>
        </div>
    </section>
        
    <asp:ListView ID="lvProyectos" runat="server">
        <LayoutTemplate>
            <section>
                <table style="margin-top:-25px">
                    <thead id="tableHead">
                       <tr>
                            <td style="width: 18%">DESCRIPCIÓN</td> 
                            <td colspan="2" style="width: 16%; text-align: center">DISPONIBLE</td>
                            <td colspan="2" style="width: 16%; text-align: center">RESERVADAS</td>
                            <td colspan="2" style="width: 16%; text-align: center">VENDIDAS SIN BOLETO</td> 
                            <td colspan="2" style="width: 16%; text-align: center">VENDIDAS</td>
                            <td style="width: 12%; text-align: center">TOTAL</td>
                            <td style="width: 6%;" id="itemPlaceholder1"></td>
                        </tr>
                    </thead>
                    <tbody style="height:80px; overflow:scroll">
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </tbody>
                    <tfoot class="footerTable">
                        <tr>
                            <td style="width: 18%;"><b>TOTALES</b></td>
                            <td style="width: 8%; text-align: center"><b><asp:Label ID="lbTotalDisponible" runat="server"></asp:Label></b></td>
                            <td></td>
                            <td style="width: 8%; text-align: center"><b><asp:Label ID="lbTotalReservada" runat="server"></asp:Label></b></td>
                            <td></td>
                            <td style="width: 8%; text-align: center"><b><asp:Label ID="lbTotalVendidaSinBoleto" runat="server"></asp:Label></b></td>
                            <td></td>
                            <td style="width: 8%; text-align: center"><b><asp:Label ID="lbTotalVendida" runat="server"></asp:Label></b></td>
                            <td></td>
                            <td style="width: 12%; text-align: center"><b><asp:Label ID="lbTotalTotal" runat="server"></asp:Label></b></td>
                            <td style="width: 8%;"></td>
                        </tr>
                    </tfoot>
                </table>
            </section>
        </LayoutTemplate>           
        
        <ItemTemplate>   
            <tr onclick="redir('<%# Eval("idObra") %>');" style="cursor: pointer">
                <td style="text-align: left; border-right: 1px solid #bdbdbd;"><asp:Label ID="lbProyecto" runat="Server" Text='<%#Eval("obra") %>' /></td>

                <td style="text-align: center; width:8%"><asp:Label ID="lbDisponible" runat="Server" Text='<%#Eval("disponible") %>' /></td>
                <td style="text-align: center; width:8%; border-right: 1px solid #bdbdbd; background-color: #efebe9;">(<asp:Label ID="lbPorcentajeDisponible" runat="Server" Text='<%#Eval("porcentajeDisponible") %>' />%)</td>

                <td style="text-align: center; width:8%"><asp:Label ID="lbReservada" runat="Server" Text='<%#Eval("reservada") %>' /></td>
                <td style="text-align: center; width:8%; border-right: 1px solid #bdbdbd; background-color: #efebe9;">(<asp:Label ID="lbPorcentajeReservada" runat="Server" Text='<%#Eval("porcentajeReservada") %>' />%)</td>

                <td style="text-align: center; width:8%"><asp:Label ID="lbVendidaSinBoleto" runat="Server" Text='<%#Eval("vendida_sin_boleto") %>' /></td>
                <td style="text-align: center; width:8%; border-right: 1px solid #bdbdbd; background-color: #efebe9;">(<asp:Label ID="lbPorcentajeVendidaSinBoleto" runat="Server" Text='<%#Eval("porcentajeVendidaSinBoleto") %>' />%)</td>

                <td style="text-align: center; width:8%"><asp:Label ID="lbVendida" runat="Server" Text='<%#Eval("vendida") %>' /></td>
                <td style="text-align: center; width:8%; border-right: 1px solid #bdbdbd; background-color: #efebe9;">(<asp:Label ID="lbPorcentajeVendida" runat="Server" Text='<%#Eval("porcentajeVendida") %>' />%)</td>


                <td style="text-align: center;">
                    <asp:Label ID="lbTotal" runat="Server" Text='<%#Eval("total") %>' />
                </td>
                <td>
                    <a class="detailBtn" href="Unidad.aspx?idProyecto=<%# Eval("idObra") %>"></a>
                </td>
            </tr>                
        </ItemTemplate>              
        <EmptyDataTemplate>
            <table style="width:100%" runat="server">
                <tr>
                    <td align="center"><b>No se encontraron obras registradas.<b/></td>
                </tr>
            </table>
        </EmptyDataTemplate>
    </asp:ListView>

<CR:crystalreportviewer ID="CrystalReportViewer" runat="server" 
    AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource" 
    Width="894px" DisplayToolbar="False" Visible="false" />
<CR:crystalreportsource ID="CrystalReportSource" runat="server" 
    Visible="false">
    <Report FileName="Reportes/ResumenObra.rpt"></Report>
</CR:crystalreportsource>
</asp:Content>
