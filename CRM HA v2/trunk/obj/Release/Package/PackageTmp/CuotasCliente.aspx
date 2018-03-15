<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CuotasCliente.aspx.cs" Inherits="crm.CuotasCliente" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/autocomplete/jquery-1.4.1.min.js"></script>
    <link href="js/autocomplete/jquery.autocomplete.css" rel="stylesheet" />
    <script src="js/autocomplete/jquery.autocomplete.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtSearch.ClientID%>").autocomplete('/Web-Service/Search_CS.ashx');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" />
    <asp:HiddenField ID="hfMes1" runat="server" />
    <asp:HiddenField ID="hfMes2" runat="server" />
    <asp:HiddenField ID="hfMes3" runat="server" />
    <asp:HiddenField ID="hfMes4" runat="server" />

    <asp:HiddenField ID="hfTotalCtaCte" runat="server" />
    <asp:HiddenField ID="hfTotalMes1" runat="server" />
    <asp:HiddenField ID="hfTotalMes2" runat="server" />
    <asp:HiddenField ID="hfTotalMes3" runat="server" />
    <asp:HiddenField ID="hfTotalMes4" runat="server" />
    <asp:HiddenField ID="hfTotalMesesRestantes" runat="server" />
    <asp:HiddenField ID="hfTotalDeuda" runat="server" />

    <section>
        <div class="headOptions">
            <div style="float:left"><h2>Cuotas a cobrar por cliente</h2></div>
            <div style="float:right">
                <label class="rigthLabel" style="width: 37%;">
                    <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="formBtnNar" OnClick="btnDescargar_Click" style="margin-right: 25px;"/>
                </label>
            </div>
        </div>
        <div>
            <div class="formHolder" style="padding-bottom: 4px;">
                <div class="headOptions headLine" style="padding-bottom: 0px !important;">
                    <div style="float:left">
                        <div class="h7" style="padding-top: 9px;">Cuotas puras a cobrar sin gastos administrativos actualizada al primer vencimiento</div>
                    </div>
                    <div style="float:right">
                        <label class="col2" style="width:510px">
                            <asp:Label ID="lbCantUnidades" runat="server" style="width: 15% !important;" Text="Clientes"></asp:Label>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>                       
                                    <asp:TextBox ID="txtSearch" name="txtSearch" runat="server" style="width: 44%" CssClass="textBox textBoxForm"></asp:TextBox>  
                                    <asp:Button ID="btnBuscar" runat="server" CssClass="formBtnNar" Text="Buscar" OnClick="btnBuscar_Click" style="float: left; margin-left: 8px;"/>   
                                    <asp:Button ID="btnVerTodos" Text="Ver todos" CssClass="formBtnGrey1" runat="server" style="margin-left: 8px;" OnClick="btnVerTodos_Click"/>                                                  
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnBuscar"/>
                                    <asp:PostBackTrigger ControlID="btnVerTodos"/>
                                </Triggers>
                            </asp:UpdatePanel> 
                        </label>
                    </div>
                </div>
                <div class="h7" style="margin-bottom: 5px;">Todos los valores están expresados en <b>pesos</b></div>
            </div>
        </div>
    </section>

    <asp:ListView ID="lvSaldos" runat="server" OnLayoutCreated="ListView11_LayoutCreated">
        <LayoutTemplate>
            <section>            
                <table style="width:100%">                            
                    <thead id="tableHead">
                        <tr>     
                            <td class="fontListview" style="width: 16%; text-align: center">CLIENTE</td>
                            <td class="fontListview" style="width: 16%; text-align: center">SALDO CUENTA CORRIENTE</td>
                            <td class="fontListview" style="width: 10%; text-align: center"><asp:Label ID="lbMes1" runat="server"></asp:Label></td>
                            <td class="fontListview" style="width: 10%; text-align: center"><asp:Label ID="lbMes2" runat="server"></asp:Label></td>
                            <td class="fontListview" style="width: 10%; text-align: center"><asp:Label ID="lbMes3" runat="server"></asp:Label></td>
                            <td class="fontListview" style="width: 10%; text-align: center"><asp:Label ID="lbMes4" runat="server"></asp:Label></td>
                            <td class="fontListview" style="width: 11%; text-align: center; font-size: 11px !important;">MESES RESTANTES</td>
                            <td class="fontListview" style="width: 12%; text-align: center;">TOTAL DE DEUDA</td>
                            <td style="width: 1%;"></td>
                        </tr>
                    </thead>
                </table>
                <div class="tableBody">
                    <table style="width:100%">
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>    
                    </table>
                </div>
                <div class="tableFoot">
                    <table style="width:100%">
                        <tfoot class="footerTable">
                            <tr>
                                <td style="width: 8%;"></td>      
                                <td style="width: 4%; text-align:right"><b><asp:Label ID="lbTotalCC" runat="server"></asp:Label></b></td>                     
                                <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMes1" runat="server"></asp:Label></b></td>
                                <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMes2" runat="server"></asp:Label></b></td>
                                <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMes3" runat="server"></asp:Label></b></td>
                                <td style="width: 3%; text-align:right"><b style="margin-right: 10px;"><asp:Label ID="lbTotalMes4" runat="server"></asp:Label></b></td>
                                <td style="width: 2%; text-align:right"><b style="margin-right: -1px;"><asp:Label ID="lbTotalMesesRestantes" runat="server"></asp:Label></b></td>
                                <td style="width: 4%; text-align:right"><b style="margin-right: 17px;"><asp:Label ID="lbTotalDeuda" runat="server"></asp:Label></b></td>
                            </tr>
                        </tfoot>                  
                    </table>
                </div>
            </section> 
        </LayoutTemplate>
                
        <ItemTemplate>                   
            <tr style="cursor: pointer">
                <td class="fontListview" style="width: 16%; text-align: center">
                    <asp:Label ID="lbIdCliente" runat="Server" Text='<%#Eval("idCliente") %>' Visible="false"/>
                    <asp:Label ID="lbCliente" runat="Server" Text='<%#Eval("cliente") %>' />
                </td>
                <td class="fontListview" style="width: 10%; text-align: right">
                    <asp:Label ID="lbCC" runat="Server" Text='<%#Eval("saldoCtaCte") %>' />
                </td>
                <td class="fontListview" style="width: 10%; text-align: right">
                    <asp:Label ID="lbMes1" runat="Server" Text='<%#Eval("saldo1") %>' />
                </td>
                <td class="fontListview" style="width: 10%; text-align: right">
                    <asp:Label ID="lbMes2" runat="Server" Text='<%#Eval("saldo2") %>' />
                </td>
                <td class="fontListview" style="width: 10%; text-align: right">
                    <asp:Label ID="lbMes3" runat="Server" Text='<%#Eval("saldo3") %>' />
                </td>
                <td class="fontListview" style="width: 10%; text-align: right">
                    <asp:Label ID="lbMes4" runat="Server" Text='<%#Eval("saldo4") %>' />
                </td>
                <td class="fontListview" style="width: 11%; text-align: right;">
                    <asp:Label ID="lbMesesRestantes" runat="Server" Text='<%#Eval("mesesRestantes") %>' />
                </td>
                <td class="fontListview" style="width: 12%; text-align: right;">
                    <asp:Label ID="lbDeuda" runat="Server" Text='<%#Eval("total") %>' />
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>

    <CR:crystalreportviewer ID="CrystalReportViewer" runat="server" AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource" Width="894px" DisplayToolbar="False" Visible="false" />
    <CR:crystalreportsource ID="CrystalReportSource" runat="server" Visible="false">
        <Report FileName="Reportes/CuotasClientes.rpt"></Report>
    </CR:crystalreportsource>
</asp:Content>


