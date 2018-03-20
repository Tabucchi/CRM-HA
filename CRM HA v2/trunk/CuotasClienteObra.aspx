<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CuotasClienteObra.aspx.cs" Inherits="crm.CuotasClienteObra" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            <div style="float:left"><h2>Cuotas a vencer por cliente de la obra <asp:Label ID="lbObra" runat="Server" /></h2></div>
            <div style="float:right">
                <label class="rigthLabel" style="width: 37%;">
                    <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="formBtnNar" OnClick="btnDescargar_Click" />
                </label>
            </div>
        </div>
        <div class="formHolder">
            <div class="headOptions headLine" style="padding-bottom: 0px;">
                <div align="left" class="h7" style="float:left;width: 94%;margin-top: 9px;">Cuotas puras a vencer sin gastos administrativos actualizada al primer vencimiento. Saldos impagos en cuenta corriente.</div>
            </div>
        </div>
    </section>
        
    <asp:ListView ID="lvSaldos" runat="server" OnLayoutCreated="ListView11_LayoutCreated">
        <LayoutTemplate>
            <section>            
                <table>                            
                    <thead id="tableHead">
                        <tr>     
                            <td class="fontListview" style="width: 16%; text-align: center">CLIENTE</td>
                            <td class="fontListview" style="width: 10%; text-align: center"><asp:Label ID="lbMes1" runat="server"></asp:Label></td>
                            <td class="fontListview" style="width: 10%; text-align: center"><asp:Label ID="lbMes2" runat="server"></asp:Label></td>
                            <td class="fontListview" style="width: 10%; text-align: center"><asp:Label ID="lbMes3" runat="server"></asp:Label></td>
                            <td class="fontListview" style="width: 10%; text-align: center"><asp:Label ID="lbMes4" runat="server"></asp:Label></td>
                            <td class="fontListview" style="width: 11%; text-align: center; font-size: 11px !important;">MESES RESTANTES</td>
                            <td class="fontListview" style="width: 12%; text-align: center;">TOTAL DE DEUDA</td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>
                    <tfoot class="footerTable">
                        <tr>
                            <td style="width: 3%;"></td>                            
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMes1" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMes2" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMes3" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMes4" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMesesRestantes" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalDeuda" runat="server"></asp:Label></b></td>
                        </tr>
                    </tfoot>                   
                </table>
            </section>   
        </LayoutTemplate>
                
        <ItemTemplate>                   
            <tr style="cursor: pointer">
                <td class="fontListview" style="text-align: center">
                    <asp:Label ID="lbIdCliente" runat="Server" Text='<%#Eval("idCliente") %>' Visible="false"/>
                    <asp:Label ID="lbCliente" runat="Server" Text='<%#Eval("cliente") %>' />
                </td>
                <td class="fontListview" style="text-align: right">
                    <asp:Label ID="lbMes1" runat="Server" Text='<%#Eval("saldo1") %>' />
                </td>                 
                <td class="fontListview" style="text-align: right">
                    <asp:Label ID="lbMes2" runat="Server" Text='<%#Eval("saldo2") %>' />
                </td>
                <td class="fontListview" style="text-align: right">
                    <asp:Label ID="lbMes3" runat="Server" Text='<%#Eval("saldo3") %>' />
                </td>
                <td class="fontListview" style="text-align: right">
                    <asp:Label ID="lbMes4" runat="Server" Text='<%#Eval("saldo4") %>' />
                </td>             
                <td class="fontListview" style="text-align: right">
                    <asp:Label ID="lbMesesRestantes" runat="Server" Text='<%#Eval("mesesRestantes") %>' />
                </td>
                 <td class="fontListview" style="text-align: right">
                    <asp:Label ID="lbDeuda" runat="Server" Text='<%#Eval("total") %>' />
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>

    <CR:crystalreportviewer ID="CrystalReportViewer" runat="server" AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource" Width="894px" DisplayToolbar="False" Visible="false" />
    <CR:crystalreportsource ID="CrystalReportSource" runat="server" Visible="false">
        <Report FileName="Reportes/CuotasClientes1.rpt"></Report>
    </CR:crystalreportsource>
</asp:Content>
