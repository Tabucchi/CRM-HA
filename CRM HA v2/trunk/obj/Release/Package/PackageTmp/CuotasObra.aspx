<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CuotasObra.aspx.cs" Inherits="crm.CuotasObra" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        // simple redirect to your detail page, passing the selected ID 
        function redir(id) {
            window.location.href = "CuotasClienteObra.aspx?idProyecto=" + id;
        }
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
    <asp:HiddenField ID="hfTotal" runat="server" />

   <%-- <section style="margin-bottom: 8px;">
        <div class="headOptions">
            <div style="float:left"><h2>Cuotas a cobrar por obra</h2></div>
            <div style="float:right">
                <label class="rigthLabel" style="width: 37%;">
                    <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="formBtnNar" OnClick="btnDescargar_Click" />
                </label>
            </div>
        </div>
        <div class="h7" style="margin-bottom: 5px;">Todos los valores están expresados en <b>pesos</b></div>
    </section>--%>

    <section>
        <div class="headOptions">
            <div style="float:left"><h2>Cuotas a cobrar por obra</h2></div>
            <div style="float:right">
                <div style="width:100%">
                    <div style="float:left; margin-right:10px">
                        <b><asp:LinkButton ID="btnNuevoOV" runat="server" CssClass="formBtnGrey" Text="Resumenes" OnClick="btnNuevoOV_Click" style="margin-top: -2px;"></asp:LinkButton></b>
                    </div>
                    <div style="float:right">
                        <label class="rigthLabel" style="width: 37%;">
                            <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="formBtnNar" OnClick="btnDescargar_Click" />
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <div class="formHolder" style="padding-bottom: 4px;">
                <div class="headOptions headLine" style="padding-bottom: 0px !important;">
                    <div style="float:left">
                        <div class="h7" style="padding-top: 9px;">Cuotas puras a cobrar sin gastos administrativos actualizada al primer vencimiento</div>
                    </div>
                </div>
                <div class="h7" style="margin-bottom: 5px;">Todos los valores están expresados en <b>pesos</b></div>
            </div>
        </div>
        <%--<div>
            <asp:Button ID="btnBuscar" runat="server" Text="Descargar" CssClass="formBtnNar" OnClick="btnBuscar_Click"  />
        </div>--%>


    </section>

    <asp:ListView ID="lvSaldos" runat="server" OnLayoutCreated="ListView11_LayoutCreated">
        <LayoutTemplate>
            <section>            
                <table>                            
                    <thead id="tableHead">
                        <tr>     
                            <td class="fontListview" style="width: 13%; text-align: center">OBRA</td>
                            <td class="fontListview" style="width: 10%; text-align: center"><asp:Label ID="lbMes1" runat="server"></asp:Label></td>
                            <td class="fontListview" style="width: 10%; text-align: center"><asp:Label ID="lbMes2" runat="server"></asp:Label></td>
                            <td class="fontListview" style="width: 10%; text-align: center"><asp:Label ID="lbMes3" runat="server"></asp:Label></td>    
                            <td class="fontListview" style="width: 10%; text-align: center"><asp:Label ID="lbMes4" runat="server"></asp:Label></td>                        
                            <td class="fontListview" style="width: 11%; text-align: center; font-size: 11px !important;">MESES RESTANTES</td>
                            <td class="fontListview" style="width: 12%; text-align: center;">TOTAL DE DEUDA</td>
                            <td class="fontListview" style="width: 2%; text-align: center;"></td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody> 
                    <tfoot class="footerTable">
                        <tr>
                            <td style="width: 3%; text-align:center"><h4 style="margin-bottom: 0px !important">Subtotal</h4></td>                            
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMes1" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMes2" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMes3" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMes4" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalMesesRestantes" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalDeuda" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"></td>
                        </tr>
                    </tfoot>                      
                </table>
            </section>   
        </LayoutTemplate>
                
        <ItemTemplate>                
            <tr onclick="redir('<%# Eval("idProyecto") %>');" style="cursor: pointer">  
                <td class="fontListview" style="text-align: center">
                    <asp:Label ID="Label6" runat="Server" Text='<%#Eval("proyecto") %>' />
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
                <td>
                    <a class="detailBtn" href="CuotasClienteObra.aspx?idProyecto=<%# Eval("idProyecto") %>"></a>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>

    <asp:ListView ID="lvSaldosCtaCte" runat="server">
        <LayoutTemplate>
            <section>      
                <h4 style="font-size: 19px; margin-bottom: 10px">Total</h4>      
                <table>                            
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
                            <td style="width: 3%; text-align:right; padding-right: 22px;"><b><asp:Label ID="lbTotalDeuda" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"></td>
                        </tr>
                    </tfoot>                      
                </table>
            </section>   
        </LayoutTemplate>
                
        <ItemTemplate>                   
            <tr style="cursor: pointer">
                <td class="fontListview" style="text-align: center; width: 13%;">
                    <asp:Label ID="Label6" runat="Server" Text='<%#Eval("proyecto") %>' />
                </td> 
                <td class="fontListview" style="text-align: right; width: 10%; padding-right: 17px;">
                    <asp:Label ID="lbMes1" runat="Server" Text='<%#Eval("saldo1") %>' />
                </td>
                <td class="fontListview" style="text-align: right; width: 10%; padding-right: 18px;">
                    <asp:Label ID="lbMes2" runat="Server" Text='<%#Eval("saldo2") %>' />
                </td>
                <td class="fontListview" style="text-align: right; width: 10%; padding-right: 19px;">
                    <asp:Label ID="lbMes3" runat="Server" Text='<%#Eval("saldo3") %>' />
                </td>  
                <td class="fontListview" style="text-align: right; width: 10%; padding-right: 20px;">
                    <asp:Label ID="lbMes4" runat="Server" Text='<%#Eval("saldo4") %>' />
                </td>           
                <td class="fontListview" style="text-align: right; width: 11%; padding-right: 21px;">
                    <asp:Label ID="lbMesesRestantes" runat="Server" Text='<%#Eval("mesesRestantes") %>' />
                </td>
                 <td class="fontListview" style="text-align: right; width: 12%; padding-right: 22px;">
                    <asp:Label ID="lbDeuda" runat="Server" Text='<%#Eval("total") %>' />
                </td>
                <td class="fontListview" style="text-align: right; width: 2%;"></td>
            </tr>
        </ItemTemplate>
    </asp:ListView> 

    <CR:crystalreportviewer ID="CrystalReportViewer" runat="server" 
            AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource" 
            Width="894px" DisplayToolbar="False" Visible="false" />
        <CR:crystalreportsource ID="CrystalReportSource" runat="server" 
            Visible="false">
            <Report FileName="Reportes/CuotasObra.rpt">
            </Report>
        </CR:crystalreportsource>
</asp:Content>
