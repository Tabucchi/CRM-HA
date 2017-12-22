<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Comprobantes.aspx.cs" Inherits="crm.Comprobantes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="css/orange.css" rel="stylesheet" />

<script type="text/javascript">
    var updateProgress = null;

    function postbackButtonClick() {
        updateProgress = $find("<%= UpdateProgress1.ClientID %>");
        window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
        return true;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>

    <section>        
        <div class="formHolder" id="searchBoxTop">
            <div class="headOptions headLine">
                <a href="#" alt="Filtro" class="toggleFiltr" style="margin-top: 9px; margin-right:5px">v</a>
                <h2>Comprobantes</h2>
                
                <div style="float: right; margin-top: 3px;">
                    <label class="col2" style="width:250px">
                        <asp:DropDownList ID="cbComprobante" runat="server" style="width:246px" AutoPostBack="True" OnSelectedIndexChanged="cbFiltro_SelectedIndexChanged">
                            <asp:ListItem Text="Todos" Value="Todos" />
                            <asp:ListItem Text="Recibos" Value="Recibo" />
                            <asp:ListItem Text="Notas de débito" Value="NotaDebito" />
                            <asp:ListItem Text="Notas de crédito" Value="NotaCredito" />  
                            <asp:ListItem Text="Condonaciones" Value="Condonacion" />                         
                        </asp:DropDownList>
                    </label>
                </div>
            </div>

            <div class="hideOpt" style="width: 100%; margin-bottom: 12px;">
                <div class="formHolder" style="padding: 22px 25px 12px; box-shadow: inherit;">
                    <div class="divFormInLine" style="width: 350px !important;">
                        <div>
                            <label class="col3" style="width: 90% !important;">
                                <span style="width: 30% !important;">CLIENTE</span>
                                <asp:DropDownList ID="cbFiltroCliente" runat="server" style="width: 200px;"><asp:ListItem Text="Todos" Value="0" /></asp:DropDownList>
                            </label>
                        </div>
                    </div>

                    <div class="divFormInLine" style="width: 350px !important;">
                        <div>
                            <label class="col3" style="width: 90% !important;">
                                <span style="width: 30% !important;">NRO.</span>
                                <label style="line-height: 14px;">
                                    <asp:TextBox ID="txtFiltroNro" runat="server" CssClass="textBox textBoxForm decimal" placeholder="Nro. comprobante" style="width: 186px;"></asp:TextBox>
                                </label>
                            </label>
                        </div>
                    </div>

                    <div class="divFormInLine" style="width: 350px !important;">
                        <div>
                            <label class="col3" style="width: 90% !important;">
                                <span style="width: 30% !important;">ESTADO</span>
                                <asp:DropDownList ID="cbEstado" runat="server" style="width: 200px;">
                                    <asp:ListItem Value="-1">Todos</asp:ListItem>
                                    <asp:ListItem Value="1">Emitido</asp:ListItem>
                                    <asp:ListItem Value="0">Anulado</asp:ListItem>                                    
                                </asp:DropDownList>
                            </label>
                        </div>
                    </div>
                </div>

                <div class="formHolderCalendar" style="padding-top: 10px;">
                    <div class="divFormInLine" style="width: 100% !important;">
                        <div style="float:left">
                            <label style="width: 800px !important;">
                                <span style="width: 10% !important; height: 29px;">FECHA</span>
                                <label style="line-height: 14px; width: 500px;">
                                    <asp:TextBox ID="txtFechaDesde" runat="server" class="textBox textBoxForm" placeholder="Desde" style="width: 186px; margin-right: 5px;"></asp:TextBox>
                                    <ajax:CalendarExtender ID="ce1" runat="server" CssClass="orange" TargetControlID="txtFechaDesde" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  

                                    <asp:TextBox ID="txtFechaHasta" runat="server" class="textBox textBoxForm" placeholder="Hasta" style="width: 186px;"></asp:TextBox>
                                    <ajax:CalendarExtender ID="ce2" runat="server" CssClass="orange" TargetControlID="txtFechaHasta" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  

                                    <asp:UpdatePanel runat="server" ID="pnlListView">
                                        <ContentTemplate>
                                            <asp:Button ID="btnBuscar" Text="Buscar" class="formBtnNar" runat="server" OnClick="btnBuscar_Click" OnClientClick="return postbackButtonClick();"/>     
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnBuscar" />
                                        </Triggers>
                                    </asp:UpdatePanel>                                   
                                </label>
                            </label>
                        </div>

                        <div style="float:right; margin-right: -212px; width: 33%;">
                            <label>
                                <asp:Button ID="btnImprimirCC" Text="Imprimir" class="formBtnNar" runat="server" OnClick="btnImprimirCC_Click"/>                                        
                                <asp:Button ID="btnTodos" Text="Ver todos" class="formBtnGrey1" runat="server" OnClick="btnTodos_Click" style="margin-right:10px"/>
                            </label>
                        </div>
                    </div>
                </div>   
            </div> 
        </div>                          
    </section> 

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfTotalR" runat="server" />
            <asp:HiddenField ID="hfTotalNC" runat="server" />
            <asp:HiddenField ID="hfTotalND" runat="server" />
            <asp:HiddenField ID="hfTotalC" runat="server" />

            <asp:Panel ID="pnlComprobantes" runat="server">                
                <section style="margin-bottom: 0px;">
                    <div class="headOptions">
                        <h2>Recibos</h2>
                    </div>
                </section>

                <asp:ListView ID="lvComprobanteRecibos" runat="server" OnItemCommand="lvRecibos_ItemCommand"> 
                    <LayoutTemplate>
                        <section>
                            <table style="margin-top:-25px">
                                <thead id="tableHead">
                                    <tr>
                                        <td style="width: 6%; text-align: center">NRO.</td>
                                        <td style="width: 10%; text-align: center">CLIENTE</td>
                                        <td style="width: 4%; text-align: center">FECHA</td>
                                        <td style="width: 4%; text-align: center">ESTADO</td>
                                        <td style="width: 6%; text-align: center">IMPORTE</td>
                                        <td style="width: 1%;"></td>
                                    </tr>
                                </thead>
                                <tbody style="height:80px; overflow:scroll">
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                </tbody>  
                                <tfoot class="footerTable">
                                    <tr>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotal" runat="server"></asp:Label></b></td>
                                        <td style="width: 3%;"></td>
                                    </tr>
                                </tfoot>   
                            </table>
                        </section>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer;">
                            <td style="width: 6%; text-align: center">
                                <asp:Label ID="lbNro" runat="Server" Text='<%#Eval("Nro") %>'/>
                            </td>                
                            <td style="width: 10%; text-align: center">
                                <asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>'/>
                            </td>
                            <td style="width: 4%; text-align: center">
                                <asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>'/>
                            </td>
                            <td style="width: 4%; text-align: center">
                                <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>'/>
                            </td>
                            <td style="width: 6%; text-align: right">
                                <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto") %>'/>
                            </td>
                            <td style="width: 1%;">
                                <asp:LinkButton ID="btnImprimir" runat="server" PostBackUrl="~/Comprobantes.aspx" class="savePrint" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdItemCCU")%>' Text="Imprimir recibo" ToolTip="Imprimir recibo" />
                            </td>                 
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <section>
                            <table style="width:100%; margin-top: -34px;" runat="server">
                                <tr>
                                    <td style="text-align:center"><b>No se encontraron Recibos.<b/></td>
                                </tr>
                            </table>
                        </section>
                    </EmptyDataTemplate>
                </asp:ListView> 

                <section>
                    <div class="headOptions">
                        <h2>Nota de crédito</h2>
                    </div>
                </section>

                <asp:ListView ID="lvComprobanteNotaCredito" runat="server" OnItemCommand="lvNotaCredito_ItemCommand">
                    <LayoutTemplate>
                        <section>
                            <table style="margin-top:-25px">
                                <thead id="tableHead">
                                    <tr>
                                        <td style="width: 6%; text-align: center">NRO.</td>
                                        <td style="width: 10%; text-align: center">CLIENTE</td>
                                        <td style="width: 4%; text-align: center">FECHA</td>
                                        <td style="width: 4%; text-align: center">ESTADO</td>
                                        <td style="width: 6%; text-align: center">IMPORTE</td>
                                        <td style="width: 1%;"></td>
                                    </tr>
                                </thead>
                                <tbody style="height:80px; overflow:scroll">
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                </tbody>  
                                <tfoot class="footerTable">
                                    <tr>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalNC" runat="server"></asp:Label></b></td>
                                        <td style="width: 3%;"></td>
                                    </tr>
                                </tfoot> 
                            </table>
                        </section>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer;">
                            <td style="text-align: center">
                                <asp:Label ID="lbNro" runat="Server" Text='<%#Eval("Nro") %>'/>
                            </td>                
                            <td style="text-align: center">
                                <asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>'/>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto") %>'/>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnImprimirNC" runat="server" PostBackUrl="~/Comprobantes.aspx" class="savePrint" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdItemCCU")%>' Text="Imprimir nota de crédito" ToolTip="Imprimir nota de crédito" />
                            </td>                   
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <section>
                            <table style="width:100%; margin-top: -34px;" runat="server">
                                <tr>
                                    <td style="text-align:center"><b>No se encontraron Notas de Crédito.<b/></td>
                                </tr>
                            </table>
                        </section>
                    </EmptyDataTemplate>
                </asp:ListView>

                <section>
                    <div class="headOptions">
                        <h2>Nota de débito</h2>
                    </div>
                </section>

                <asp:ListView ID="lvComprobanteNotaDebito" runat="server" OnItemCommand="lvNotaDebito_ItemCommand">
                    <LayoutTemplate>
                        <section>
                            <table style="margin-top:-25px">
                                <thead id="tableHead">
                                    <tr>
                                        <td style="width: 6%; text-align: center">NRO.</td>
                                        <td style="width: 10%; text-align: center">CLIENTE</td>
                                        <td style="width: 4%; text-align: center">FECHA</td>
                                        <td style="width: 4%; text-align: center">ESTADO</td>
                                        <td style="width: 6%; text-align: center">IMPORTE</td>
                                        <td style="width: 1%;"></td>
                                    </tr>
                                </thead>
                                <tbody style="height:80px; overflow:scroll">
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                </tbody> 
                                <tfoot class="footerTable">
                                    <tr>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalND" runat="server"></asp:Label></b></td>
                                        <td style="width: 3%;"></td>
                                    </tr>
                                </tfoot>
                            </table>
                        </section>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer;">
                            <td style="text-align: center">
                                <asp:Label ID="lbNro" runat="Server" Text='<%#Eval("Nro") %>'/>
                            </td>                
                            <td style="text-align: center">
                                <asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>'/>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto") %>'/>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnImprimirND" runat="server" PostBackUrl="~/Comprobantes.aspx" class="savePrint" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdItemCCU")%>' Text="Imprimir nota de débito" ToolTip="Imprimir nota de débito" />
                            </td>                  
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <section>
                            <table style="width:100%; margin-top: -34px;" runat="server">
                                <tr>
                                    <td style="text-align:center"><b>No se encontraron Notas de Débito.<b/></td>
                                </tr>
                            </table>
                        </section>
                    </EmptyDataTemplate>
                </asp:ListView> 

                <section>
                    <div class="headOptions">
                        <h2>Condonación</h2>
                    </div>
                </section>
                
                <asp:ListView ID="lvComprobanteCondonacion" runat="server" OnItemCommand="lvCondonacion_ItemCommand"> 
                    <LayoutTemplate>
                        <section>
                            <table style="margin-top:-25px">
                                <thead id="tableHead">
                                    <tr>
                                        <td style="width: 6%; text-align: center">NRO.</td>
                                        <td style="width: 10%; text-align: center">CLIENTE</td>
                                        <td style="width: 4%; text-align: center">FECHA</td>
                                        <td style="width: 4%; text-align: center">ESTADO</td>
                                        <td style="width: 6%; text-align: center">IMPORTE</td>
                                        <td style="width: 1%;"></td>
                                    </tr>
                                </thead>
                                <tbody style="height:80px; overflow:scroll">
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                </tbody>  
                                <tfoot class="footerTable">
                                    <tr>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotal" runat="server"></asp:Label></b></td>
                                        <td style="width: 3%;"></td>
                                    </tr>
                                </tfoot>   
                            </table>
                        </section>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer;">
                            <td style="text-align: center">
                                <asp:Label ID="lbNro" runat="Server" Text='<%#Eval("Nro") %>'/>
                            </td>                
                            <td style="text-align: center">
                                <asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>'/>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto") %>'/>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnImprimir" runat="server" PostBackUrl="~/Comprobantes.aspx" class="savePrint" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdItemCCU")%>' Text="Imprimir recibo" ToolTip="Imprimir recibo" />
                            </td>                   
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <section>
                            <table style="width:100%; margin-top: -34px;" runat="server">
                                <tr>
                                    <td style="text-align:center"><b>No se encontraron Recibos.<b/></td>
                                </tr>
                            </table>
                        </section>
                    </EmptyDataTemplate>
                </asp:ListView>

                <div style="width:100%">                    
                    <div>
                        <div class="EtiquetasComprobante" style="float:left; width: 100%;">
                            <div class="lfloat EtiquetasBody EtiquetasFinance" style="height: 20%; padding-top: 1px; background-size: 52px; background-position-x: -8px;">  
                                <span style="padding-top: 25px;text-align: right;font-size: 20px;line-height: 36px;letter-spacing: -1px;margin-bottom: 0;font-weight: 300;padding-right: 25px;" data-counter="counterup" data-value="12,5">
                                    Total $ &nbsp;<asp:Label ID="lbTotalComprobantes" runat="server"></asp:Label>
                                </span>                                                             
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlRecibos" runat="server">
               <asp:ListView ID="lvRecibos" runat="server" OnItemCommand="lvRecibos_ItemCommand"> 
                    <LayoutTemplate>
                        <section>
                            <table style="margin-top:-25px">
                                <thead id="tableHead">
                                    <tr>
                                        <td style="width: 6%; text-align: center">NRO.</td>
                                        <td style="width: 10%; text-align: center">CLIENTE</td>
                                        <td style="width: 4%; text-align: center">FECHA</td>
                                        <td style="width: 4%; text-align: center">ESTADO</td>
                                        <td style="width: 6%; text-align: center">IMPORTE</td>
                                        <td style="width: 1%;"></td>
                                    </tr>
                                </thead>
                                <tbody style="height:80px; overflow:scroll">
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                </tbody>
                                <tfoot class="footerTable">
                                    <tr>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotal" runat="server"></asp:Label></b></td>
                                        <td style="width: 3%;"></td>
                                    </tr>
                                </tfoot>  
                            </table>
                        </section>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer;">
                            <td style="text-align: center">
                                <asp:Label ID="lbNro" runat="Server" Text='<%#Eval("Nro") %>'/>
                            </td>                
                            <td style="text-align: center">
                                <asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>'/>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto") %>'/>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnImprimir" runat="server" PostBackUrl="~/Comprobantes.aspx" class="savePrint" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdItemCCU")%>' Text="Imprimir recibo" ToolTip="Imprimir recibo" />
                            </td>                   
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <section>
                            <table style="width:100%" runat="server">
                                <tr>
                                    <td style="text-align:center"><b>No se encontraron Recibos.<b/></td>
                                </tr>
                            </table>
                        </section>
                    </EmptyDataTemplate>
                </asp:ListView>
            </asp:Panel>

            <asp:Panel ID="pnlNotaCredito" runat="server" Visible="false">
                <asp:ListView ID="lvNotaCredito" runat="server" OnItemCommand="lvNotaCredito_ItemCommand">
                    <LayoutTemplate>
                        <section>
                            <table style="margin-top:-25px">
                                <thead id="tableHead">
                                    <tr>
                                        <td style="width: 6%; text-align: center">NRO.</td>
                                        <td style="width: 10%; text-align: center">CLIENTE</td>
                                        <td style="width: 4%; text-align: center">FECHA</td>
                                        <td style="width: 4%; text-align: center">ESTADO</td>
                                        <td style="width: 6%; text-align: center">IMPORTE</td>
                                        <td style="width: 1%;"></td>
                                    </tr>
                                </thead>
                                <tbody style="height:80px; overflow:scroll">
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                </tbody>
                                <tfoot class="footerTable">
                                    <tr>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalNC" runat="server"></asp:Label></b></td>
                                        <td style="width: 3%;"></td>
                                    </tr>
                                </tfoot>  
                            </table>
                        </section>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer;">
                            <td style="text-align: center">
                                <asp:Label ID="lbNro" runat="Server" Text='<%#Eval("Nro") %>'/>
                            </td>                
                            <td style="text-align: center">
                                <asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>'/>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto") %>'/>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnImprimirNC" runat="server" PostBackUrl="~/Comprobantes.aspx" class="savePrint" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdItemCCU")%>' Text="Imprimir nota de crédito" ToolTip="Imprimir nota de crédito" />
                            </td>                   
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <section>
                            <table style="width:100%" runat="server">
                                <tr>
                                    <td style="text-align:center"><b>No se encontraron Notas de Crédito.<b/></td>
                                </tr>
                            </table>
                        </section>
                    </EmptyDataTemplate>
                </asp:ListView>
            </asp:Panel>

            <asp:Panel ID="pnlNotaDebito" runat="server" Visible="false">
                <asp:ListView ID="lvNotaDebito" runat="server" OnItemCommand="lvNotaDebito_ItemCommand">
                    <LayoutTemplate>
                        <section>
                            <table style="margin-top:-25px">
                                <thead id="tableHead">
                                    <tr>
                                        <td style="width: 6%; text-align: center">NRO.</td>
                                        <td style="width: 10%; text-align: center">CLIENTE</td>
                                        <td style="width: 4%; text-align: center">FECHA</td>
                                        <td style="width: 4%; text-align: center">ESTADO</td>
                                        <td style="width: 6%; text-align: center">IMPORTE</td>
                                        <td style="width: 1%;"></td>
                                    </tr>
                                </thead>
                                <tbody style="height:80px; overflow:scroll">
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                </tbody>
                                <tfoot class="footerTable">
                                    <tr>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotalND" runat="server"></asp:Label></b></td>
                                        <td style="width: 3%;"></td>
                                    </tr>
                                </tfoot>  
                            </table>
                        </section>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer;">
                            <td style="text-align: center">
                                <asp:Label ID="lbNro" runat="Server" Text='<%#Eval("Nro") %>'/>
                            </td>                
                            <td style="text-align: center">
                                <asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>'/>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto") %>'/>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnImprimirND" runat="server" PostBackUrl="~/Comprobantes.aspx" class="savePrint" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdItemCCU")%>' Text="Imprimir nota de débito" ToolTip="Imprimir nota de débito" />
                            </td>                   
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <section>
                            <table style="width:100%" runat="server">
                                <tr>
                                    <td style="text-align:center"><b>No se encontraron Notas de Débito.<b/></td>
                                </tr>
                            </table>
                        </section>
                    </EmptyDataTemplate>
                </asp:ListView>
            </asp:Panel>    
            
            <asp:Panel ID="pnlCondonacion" runat="server" Visible="false">
                <asp:ListView ID="lvCondonacion" runat="server" OnItemCommand="lvCondonacion_ItemCommand"> 
                    <LayoutTemplate>
                        <section>
                            <table style="margin-top:-25px">
                                <thead id="tableHead">
                                    <tr>
                                        <td style="width: 6%; text-align: center">NRO.</td>
                                        <td style="width: 10%; text-align: center">CLIENTE</td>
                                        <td style="width: 4%; text-align: center">FECHA</td>
                                        <td style="width: 4%; text-align: center">ESTADO</td>
                                        <td style="width: 6%; text-align: center">IMPORTE</td>
                                        <td style="width: 1%;"></td>
                                    </tr>
                                </thead>
                                <tbody style="height:80px; overflow:scroll">
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                </tbody>  
                                <tfoot class="footerTable">
                                    <tr>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%;"></td>
                                        <td style="width: 3%; text-align:right"><b><asp:Label ID="lbTotal" runat="server"></asp:Label></b></td>
                                        <td style="width: 3%;"></td>
                                    </tr>
                                </tfoot>   
                            </table>
                        </section>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer;">
                            <td style="text-align: center">
                                <asp:Label ID="lbNro" runat="Server" Text='<%#Eval("Nro") %>'/>
                            </td>                
                            <td style="text-align: center">
                                <asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>'/>
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>'/>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto") %>'/>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnImprimir" runat="server" PostBackUrl="~/Comprobantes.aspx" class="savePrint" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdItemCCU")%>' Text="Imprimir recibo" ToolTip="Imprimir recibo" />
                            </td>                   
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <section>
                            <table style="width:100%; margin-top: -34px;" runat="server">
                                <tr>
                                    <td style="text-align:center"><b>No se encontraron Recibos.<b/></td>
                                </tr>
                            </table>
                        </section>
                    </EmptyDataTemplate>
                </asp:ListView>    
            </asp:Panel>    
        </ContentTemplate>
    </asp:UpdatePanel>

    <div style="float:left">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlListView">
            <ProgressTemplate>
                <div class="overlay">
                    <div class="overlayContent">
                        <div style="float:left;"><img src="images/ring_loading.gif"  width="300px" class="loading100" ImageAlign="left" /></div>
                        <div style="float:left; padding: 8px 0 0 10px">
                            <h2> Buscando... </h2>
                        </div>                                    
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
    <CR:CrystalReportSource ID="CrystalReportSourceRecibo" runat="server" Visible="false">
        <Report FileName="Reportes/Recibo.rpt"></Report>
    </CR:CrystalReportSource>

    <CR:CrystalReportSource ID="CrystalReportSourceNotaDebito" runat="server" Visible="false">
        <Report FileName="Reportes/NotaDebito.rpt"></Report>
    </CR:CrystalReportSource>

    <CR:CrystalReportSource ID="CrystalReportSourceNotaCredito" runat="server" Visible="false">
        <Report FileName="Reportes/NotaCredito.rpt"></Report>
    </CR:CrystalReportSource>
</asp:Content>
