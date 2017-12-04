<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ListadoReserva.aspx.cs" Inherits="crm.ListadoReserva" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/jquery.mask.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.decimal').mask("#.##0,00", { reverse: true });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <section>
                <div class="formHolder" id="searchBoxTop">
                    <div class="headOptions headLine">
                        <h2>Reservas</h2>
                        <asp:Panel ID="pnlReserva" runat="server" Visible="true">
                        <div style="float:right;">
                            <div style="float:left">
                                <b><asp:LinkButton ID="btnReserva" runat="server"  CssClass="formBtnGrey" Text="Nueva / Cancelar reserva" OnClick="btnReserva_Click"></asp:LinkButton></b>
                            </div>
                            <div style="float:right">
                                <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="formBtnNar" OnClick="btnDescargar_Click" style="height: 35px; margin-left: 15px;"/>
                            </div>
                        </div>
                        </asp:Panel>
                    </div>
                </div>
            </section>
                
            <asp:ListView ID="lvReservas" runat="server">
                    <LayoutTemplate>
                        <section>            
                            <table style="width:100%">                            
                                <thead id="tableHead">
                                    <tr>     
                                        <td style="text-align:center; width: 22%;">CLIENTE</td>
                                        <td style="text-align:center; width: 22%;">OBRA</td>    
                                        <td style="text-align:center; width: 14px">Cod U.F.</td>            
                                        <td style="text-align:center; width: 14px">UNIDAD FUNCIONAL</td>
                                        <td style="text-align:center; width: 14px;">NIVEL</td>
                                        <td style="text-align:center; width: 14px">NRO. UNIDAD</td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                </tbody>                        
                            </table>
                        </section>   
                    </LayoutTemplate>
                
                    <ItemTemplate>                   
                        <tr onclick="redir('<%# Eval("id") %>');" style="cursor: pointer">
                            <td style="text-align:center">
                                <a href="#" alt="Cliente" class="tooltip tooltipColor">
                                    <asp:Label ID="lbCliente" runat="Server" Text='<%#Eval("GetEmpresa") %>' />
                                </a>
                            </td>
                            <td style="text-align:center">
                                <a href="#" alt="Obra" class="tooltip tooltipColor">
                                    <asp:Label ID="lbProyecto" runat="Server" Text='<%#Eval("GetProyecto") %>' />
                                </a>
                            </td>
                            <td style="text-align:center">
                                <a href="#" alt="Cod U.F." class="tooltip tooltipColor">
                                    <asp:Label ID="lbCodUF" runat="Server" Text='<%#Eval("GetCodigoUF") %>' />
                                </a>
                            </td>
                            <td style="text-align:center">
                                <a href="#" alt="Unidad Funcional" class="tooltip tooltipColor">
                                    <asp:Label ID="lbUnidadFuncional" runat="Server" Text='<%#Eval("GetUnidadFuncional") %>' />
                                </a>
                            </td>
                            <td style="text-align:center">
                                <a href="#" alt="Nivel" class="tooltip tooltipColor">
                                    <asp:Label ID="lbNivel" runat="Server" Text='<%#Eval("GetNivel") %>' />
                                </a>
                            </td>
                            <td style="text-align:center">
                                <a href="#" alt="Nro. Unidad" class="tooltip tooltipColor">
                                    <asp:Label ID="lbNroUnidad" runat="Server" Text='<%#Eval("GetNroUnidad") %>' />
                                </a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <section>
                            <table id="Table1" style="width: 100%" runat="server">
                                <tr>
                                    <td style="text-align:center"><b>No se encontraron unidades reservadas.<b/></td>
                                </tr>
                            </table>
                        </section>
                    </EmptyDataTemplate>
                </asp:ListView>    
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDescargar" />                           
        </Triggers>
    </asp:UpdatePanel>

<CR:crystalreportviewer ID="CrystalReportViewer" runat="server" 
    AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource" 
    Width="894px" DisplayToolbar="False" Visible="false" />
<CR:crystalreportsource ID="CrystalReportSource" runat="server" 
    Visible="false">
    <Report FileName="Reportes/Reservas.rpt">
    </Report>
</CR:crystalreportsource>
</asp:Content>
