<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ResumenSaldo.aspx.cs" Inherits="crm.ResumenSaldo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" />
    <section>
        <div class="headOptions">
            <div style="float:left"><h2>Control de índices</h2></div>
            <%--<div style="float:right">
                <label class="rigthLabel" style="width: 37%;">
                    <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="formBtnNar" OnClick="btnDescargar_Click" style="margin-right: 25px;"/>
                </label>
            </div>--%>
        </div>
    </section>

    <div style="width:100%; margin-top: -22px;">
        <div style="float:left; width:48%">
            <section>
                <div class="formHolder" id="searchBoxTop1">
                    <div class="formHolderLine">
                        <h2>Índice CAC</h2>
                    </div>
                </div>
            </section>

            <section>
                <div class="formHolder" style="padding-bottom: 0px !important; background-color: #ffe0b2;">
                    <div class="formHolderLine">
                        <b style="font-size: 16px; color: #666;">Variación mensual: &nbsp;<asp:Label ID="lbTotalVariacionCac" runat="server" /></b>
                    </div>
                </div>  
            </section>

            <asp:ListView ID="lvSaldosCAC" runat="server">
                <LayoutTemplate>
                    <section>            
                        <table style="width:100%">                            
                            <thead>
                                <tr>     
                                    <td class="fontListview" style="width: 25%; text-align: center">CLIENTE</td>
                                    <td class="fontListview" style="width: 5%; text-align: center">NRO. CUOTA</td>
                                    <td class="fontListview" style="width: 5%; text-align: center">VARIACIÓN (%)</td>
                                    <td class="fontListview" style="width: 15%; text-align: center">SALDO ANTERIOR</td>
                                    <td class="fontListview" style="width: 15%; text-align: center">SALDO ACTUAL</td>  
                                    <td class="fontListview" style="width: 15%; text-align: center">DIFERENCIA SALDOS</td> 
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </tbody>
                            <tfoot class="footerTable">
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="width: 8%; text-align: center"><b><asp:Label ID="lbTotalVariacion" runat="server"></asp:Label></b></td>
                                    <td style="width: 8%; text-align: center"><b><asp:Label ID="lbTotalSaldoAnterior" runat="server"></asp:Label></b></td>
                                    <td style="width: 8%; text-align: center"><b><asp:Label ID="lbTotalSaldoActual" runat="server"></asp:Label></b></td>
                                    <td></td>
                                </tr>
                            </tfoot>
                        </table>
                    </section> 
                </LayoutTemplate>
                
                <ItemTemplate>                   
                    <tr style="cursor: pointer">
                        <td class="fontListview" style="text-align: center">
                            <asp:Label ID="lbCliente" runat="Server" Text='<%#Eval("GetEmpresa") %>' />
                        </td>
                         <td class="fontListview" style="text-align: center">
                            <asp:Label ID="Label3" runat="Server" Text='<%#Eval("Nro") %>' />
                        </td>
                         <td class="fontListview" style="text-align: center">
                            <asp:Label ID="Label1" runat="Server" Text='<%#Eval("ValidarIndice") %>' />
                        </td>
                        <td class="fontListview" style="text-align: right">
                            <asp:Label ID="lbSaldoAnterior" runat="Server" Text='<%#Eval("SaldoAnteriorByIndiceCAC") %>' />
                        </td>
                        <td class="fontListview" style="text-align: right">
                            <asp:Label ID="lbSaldoActual" runat="Server" Text='<%#Eval("GetSaldoResumen") %>' />
                        </td>    
                        <td class="fontListview" style="text-align: right">
                            <asp:Label ID="Label4" runat="Server" Text='<%#Eval("DiferenciaSaldosCAC") %>' />
                        </td>                   
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>

        <div style="float:right; width:48%">
            <section>
                <div class="formHolder">
                    <div class="formHolderLine">
                        <h2>Índice UVA</h2>
                    </div>
                </div>
            </section>

            <section>
                <div class="formHolder" style="padding-bottom: 0px !important; background-color: #ffe0b2;">
                    <div class="formHolderLine">
                        <b style="font-size: 16px; color: #666;">Variación mensual: &nbsp;<asp:Label ID="lbTotalVariacionUva" runat="server" /></b>
                    </div>
                </div>  
            </section>

            <asp:ListView ID="lvSaldosUVA" runat="server">
                <LayoutTemplate>
                    <section>            
                        <table style="width:100%">                            
                            <thead id="tableHead">
                                <tr>     
                                    <td class="fontListview" style="width: 25%; text-align: center">CLIENTE</td>
                                    <td class="fontListview" style="width: 5%; text-align: center">NRO. CUOTA</td>
                                    <td class="fontListview" style="width: 5%; text-align: center">VARIACIÓN (%)</td>
                                    <td class="fontListview" style="width: 15%; text-align: center">SALDO ANTERIOR</td>
                                    <td class="fontListview" style="width: 15%; text-align: center">SALDO ACTUAL</td>  
                                    <td class="fontListview" style="width: 15%; text-align: center">DIFERENCIA SALDOS</td>                                  
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </tbody> 
                            <tfoot class="footerTable">
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="width: 8%; text-align: center"><b><asp:Label ID="lbTotalVariacionUVA" runat="server"></asp:Label></b></td>
                                    <td style="width: 8%; text-align: center"><b><asp:Label ID="lbTotalSaldoAnteriorUVA" runat="server"></asp:Label></b></td>
                                    <td style="width: 8%; text-align: center"><b><asp:Label ID="lbTotalSaldoActualUVA" runat="server"></asp:Label></b></td>
                                    <td></td>
                                </tr>
                            </tfoot>   
                        </table>
                    </section> 
                </LayoutTemplate>
                
                <ItemTemplate>                   
                    <tr style="cursor: pointer">
                        <td class="fontListview" style="text-align: center">
                            <asp:Label ID="lbCliente" runat="Server" Text='<%#Eval("GetEmpresa") %>' />
                        </td>
                        <td class="fontListview" style="text-align: center">
                            <asp:Label ID="Label2" runat="Server" Text='<%#Eval("Nro") %>' />
                        </td>
                        <td class="fontListview" style="text-align: center">
                            <asp:Label ID="Label1" runat="Server" Text='<%#Eval("ValidarIndice") %>' />
                        </td>
                        <td class="fontListview" style="text-align: right">
                            <asp:Label ID="lbSaldoAnteriorUVA" runat="Server" Text='<%#Eval("SaldoAnteriorByIndiceUVA") %>' />
                        </td>
                        <td class="fontListview" style="text-align: right">
                            <asp:Label ID="lbSaldoActualUVA" runat="Server" Text='<%#Eval("GetSaldoResumen") %>' />
                        </td>  
                        <td class="fontListview" style="text-align: right">
                            <asp:Label ID="Label4" runat="Server" Text='<%#Eval("DiferenciaSaldosUVA") %>' />
                        </td>                       
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</asp:Content>
