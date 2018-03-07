<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="EnvioCuotas.aspx.cs" Inherits="crm.EnvioCuotas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function selectUnselectAll(val) {
            if (val == true) {
                $('input:checkbox').prop('checked', true);
                $('input:checkbox').attr('checked', true);
            }
            else {
                $('input:checkbox').prop('checked', false);
                $('input:checkbox').attr('checked', false);
            }
        }
    </script>

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
                <div class="formHolder formHolderCalendar" id="searchBoxTop">
                    <div class="headOptions headLine">
                        <a href="#" class="toggleFiltr" style="margin-top: 9px; margin-right:5px">v</a>
                        <h2>Envío de cuotas</h2>
                        <div style="float:right;">
                            <div style="float:right">
                                <b><asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="formBtnNar" OnClick="btnEnviar_Click"/></b>
                            </div>
                        </div>
                    </div>
                    <div class="hideOpt" style="width: 100%;">
                        <label class="col3">
                            <span>CLIENTE</span>
                            <asp:DropDownList ID="cbEmpresa" runat="server"/>
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

    <asp:Panel ID="pnlMensajeError" runat="server" Visible="false">
                <section>
                    <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                        <div>
                            <asp:Label ID="lbMensajePago" runat="server" class="messageError" style="margin-left: 7px;">Error: no se enviaron todos los mails seleccionados.</asp:Label>
                        </div>
                    </div>
                </section>
            </asp:Panel>
        
    <asp:UpdatePanel ID="pnlClientes" runat="server">
        <ContentTemplate>
            <asp:ListView ID="lvCuotas" runat="server">
                <LayoutTemplate>
                    <section>            
                        <table style="width:100%">                            
                            <thead id="tableHead">
                                <tr>     
                                    <td><input id="checkall" type="checkbox" onchange="selectUnselectAll(checked);" /></td>
                                    <td style="text-align: center">CLIENTE</td>
                                    <td style="text-align: center">MAIL</td>
                                    <td style="text-align: center">NRO</td>
                                    <td style="text-align: center">MONEDA</td>
                                    <td style="text-align: center">Índice</td>
                                    <td style="text-align: center">Índice (%)</td>
                                    <td style="width: 10%; text-align: center">SALDO AJUSTADO</td>
                                    <td style="text-align: center">MONTO</td>
                                    <td style="width: 8%; text-align: center">GASTOS ADTVO.</td>
                                    <td style="text-align: center">IMPORTE</td>
                                    <td style="text-align: center">SALDO</td>
                                    <td style="width: 0%;"></td>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </tbody>                        
                        </table>
                    </section>   
                </LayoutTemplate>                
                <ItemTemplate>                   
                    <tr style="cursor: pointer" id="idTr" runat="server" class='<%#Eval("GetEnvio") %>' >
                        <td style="text-align:center">
                            <asp:CheckBox ID="chBoxOV" runat="server"/>
                            <asp:Label ID="lbId" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>' />
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lbMail" runat="Server" Text='<%#Eval("GetMail") %>' /> 
                            <asp:Label ID="lbIdCuota" runat="Server" Text='<%#Eval("Id") %>'  Visible="false"/> 
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lbMoneda" runat="Server" Text='<%#Eval("nro") %>' />
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lbNro" runat="Server" Text='<%#Eval("GetMoneda") %>' />
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="Label1" runat="Server" Text='<%#Eval("GetTipoIndice") %>' />
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lbCAC" runat="Server" Text='<%#Eval("GetIndice") %>' />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lbSaldoPendiente" runat="Server" Text='<%#Eval("GetMontoAjustado") %>' />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto1") %>' />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lbComision" runat="Server" Text='<%#Eval("GetTotalComision") %>' />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lbVencimiento1" runat="Server" Text='<%#Eval("GetVencimiento1") %>' />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lbSaldo" runat="Server" Text='<%#Eval("GetSaldo") %>' />
                        </td>
                        <td>                            
                            <a class="detailBtn" href="DetalleCuota2.aspx?idCC=<%# Eval("idCuentaCorriente") %>" style="margin-right: 40%;"></a>
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <section>
                        <table id="Table1" style="width:100%" runat="server">
                            <tr>
                                <td style="text-align:center"><b>No se encontraron cuotas.<b/></td>
                            </tr>
                        </table>
                    </section>
                </EmptyDataTemplate>
            </asp:ListView>     
        </ContentTemplate>
    </asp:UpdatePanel>

    <div style="float:left">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlClientes">
            <ProgressTemplate>
                <div class="overlay">
                    <div class="overlayContent">
                        <div style="float:left;"><img src="images/ring_loading.gif"  width="300px" class="loading100" ImageAlign="left" /></div>
                        <div style="float:left; padding: 8px 0 0 10px">
                            <h2> Enviando... </h2>
                        </div>                                    
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
</asp:Content>
