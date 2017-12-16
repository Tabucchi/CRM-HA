<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="UnidadesVendidas.aspx.cs" Inherits="crm.UnidadesVendidas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" />
    <asp:HiddenField ID="hfTotalCantidad" runat="server" />
    <asp:HiddenField ID="hfTotalValorM2" runat="server" />
    <asp:HiddenField ID="hfTotalSupTotal" runat="server" />
    <asp:HiddenField ID="hfTotalPrecio" runat="server" />

    <section>
        <div class="headOptions">
            <div style="float:left"><h2>Unidades Vendidas</h2></div>
            <div style="float:right">
                <label class="rigthLabel" style="width: 37%;">
                    <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="formBtnNar" OnClick="btnDescargar_Click" />
                </label>
            </div>
        </div>
        <div class="formHolder">
            <div class="headOptions headLine" style="padding-bottom: 0px;">
                <div align="left" class="h7" style="float:left;width: 94%;margin-top: 9px;">Listado de unidades vendidas con boletos registrados en el sistema.</div>
            </div>
        </div>
    </section>

    <asp:ListView ID="lvUnidades" runat="server">
        <LayoutTemplate>
            <section>            
                <table>                            
                    <thead id="tableHead">
                        <tr>     
                            <td class="fontListview" style="width: 20%; text-align: center">OBRA</td>
                            <td class="fontListview" style="width: 10%; text-align: center">VENDIDAS</td>                            
                            <td class="fontListview" style="width: 10%; text-align: center">VALOR M2 (DÓLAR)</td>
                            <td class="fontListview" style="width: 10%; text-align: center">SUP. TOTAL</td>    
                            <td class="fontListview" style="width: 10%; text-align: center">PRECIO ACORDADO (DÓLAR)</td>
                            <td class="fontListview" style="width: 1%; text-align: center">DETALLE</td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody> 
                    <tfoot class="footerTable">
                        <tr>
                            <td style="width: 3%; text-align:center"></td>                          
                            <td style="width: 3%; text-align:center"><b><asp:Label ID="lbTotalCantidad" runat="server"></asp:Label></b></td>                            
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalValorM2" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalSupTotal" runat="server"></asp:Label></b></td>
                            <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalPrecio" runat="server"></asp:Label></b></td>
                            <td></td>
                        </tr>
                    </tfoot>                      
                </table>
            </section>   
        </LayoutTemplate>
                
        <ItemTemplate>                
            <tr onclick="redir('<%# Eval("idProyecto") %>');" style="cursor: pointer">  
                <td class="fontListview" style="text-align: center">
                    <asp:Label ID="lbProyecto" runat="Server" Text='<%#Eval("GetProyecto") %>' />
                </td> 
                <td class="fontListview" style="text-align: center">
                    <asp:Label ID="Label1" runat="Server" Text='<%#Eval("cantidadSinBoleto") %>' />
                </td>
                <td class="fontListview" style="text-align: center">
                    <asp:Label ID="lbCantidad" runat="Server" Text='<%#Eval("cantidad") %>' />
                </td>
                <td class="fontListview" style="text-align: right">
                    <asp:Label ID="lbValorM2" runat="Server" Text='<%#Eval("valorM2") %>' />
                </td>
                <td class="fontListview" style="text-align: right">
                    <asp:Label ID="lbSup" runat="Server" Text='<%#Eval("supTotal") %>' />
                </td>  
                <td class="fontListview" style="text-align: right">
                    <asp:Label ID="lbPrecio" runat="Server" Text='<%#Eval("precioAcordado") %>' />                    
                </td>
                <td style="text-align: center">
                    <a class="detailBtn" href="DetalleUnidadesVendidas.aspx?idProyecto=<%# Eval("idProyecto") %>"></a>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>

<CR:crystalreportviewer ID="CrystalReportViewer" runat="server" 
    AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource" 
    Width="894px" DisplayToolbar="False" Visible="false" />
<CR:crystalreportsource ID="CrystalReportSource" runat="server" 
    Visible="false">
    <Report FileName="Reportes/UnidadesVendidas.rpt"></Report>
</CR:crystalreportsource>
</asp:Content>
