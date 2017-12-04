<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="DetalleCC1.aspx.cs" Inherits="crm.DetalleCC1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register Src="~/Controles/CuotasAnticipo.ascx" TagPrefix="crm" TagName="Cuotas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/orange.css" rel="stylesheet" />

    <script src="js/jquery.mask.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.decimal').mask("#.##0,00", { reverse: true });
        });
    </script>

    <script>
        function selectUnselectAllConfirm(val) {
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

    <script language="javascript" type="text/javascript">
        function SelectSingleRadioButtonPagoCuota(rdBtnID) {
            //process the radio button Being checked.
            var rduser = $(document.getElementById(rdBtnID));
            rduser.checked = true;
            // process all other radio buttons (excluding the the radio button Being checked).
            var list = rduser.closest('table').find("INPUT[type='radio']").not(rduser);
            list.attr('checked', false);
        }

        function SelectSingleRadioButtonCondonacion(rdBtnID) {
            //process the radio button Being checked.
            var rduser = $(document.getElementById(rdBtnID));
            rduser.checked = true;
            // process all other radio buttons (excluding the the radio button Being checked).
            var list = rduser.closest('table').find("INPUT[type='radio']").not(rduser);
            list.attr('checked', false);
        }

        function SelectSingleRadioButtonComprobante(rdBtnID) {
            //process the radio button Being checked.
            var rduser = $(document.getElementById(rdBtnID));
            rduser.checked = true;
            // process all other radio buttons (excluding the the radio button Being checked).
            var list = rduser.closest('table').find("INPUT[type='radio']").not(rduser);
            list.attr('checked', false);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>

<asp:HiddenField ID="hfIdEmpresa" runat="server" />
<asp:HiddenField ID="hfIdItemCC" runat="server" />
    
<asp:HiddenField ID="hfIdFormaPagoOV" runat="server" />

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfIdCC" runat="server" />
        <asp:HiddenField ID="hfNroCuota" runat="server" />

        <asp:HiddenField ID="hfIdCuota" runat="server" />

        <asp:TextBox ID="TextBoxFecha" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 256px;"></asp:TextBox>
        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="orange" TargetControlID="TextBoxFecha" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />

        <section>
            <div class="headOptions">
                <h2>Cuenta Corriente Nro.:&nbsp;<asp:Label ID="lbNroCC" runat="server"></asp:Label></h2>
            </div>
            <div class="formHolder" id="searchBoxTop">
                <div class="headOptions headLine">
                    <a href="#" alt="Filtro" class="toggleFiltr" style="margin-top: 9px; margin-right:5px">v</a>
                    <h2><asp:Label ID="lblCliente" runat="server"></asp:Label></h2>
                    <asp:Panel ID="pnlBotones" runat="server" Visible="false">
                    <div style="float:right; margin-top: 4px;">
                        <div style="float:left;">
                            <label>
                                <asp:Button ID="btnPagoCC" Text="Pago" CssClass="formBtnNar" runat="server" style="float:left; margin-right: 6px;" OnClick="btnPagoCC_Click"/>
                            </label>
                        </div>
                        <div style="float:left;">
                            <label>
                                <asp:Button ID="btnCreditoCC" Text="Nota de Crédito" class="formBtnNar" runat="server" style="float:left; margin-right: 6px;" OnClick="btnCreditoCC_Click"/>
                            </label>
                        </div>
                        <div style="float:right">
                            <label>
                                <asp:Button ID="btnNotaDebitoCC" Text="Nota de Débito" class="formBtnNar" runat="server" style="float:left;" OnClick="btnNotaDebitoCC_Click"/>
                            </label>
                        </div>
                    </div>
                    </asp:Panel>
                </div>

                <div class="hideOpt" style="width: 100%; margin-bottom: 12px;">
                    <div class="formHolderCalendar" style="padding-top: 10px; padding-left:0px; padding-right:0px; margin-bottom: -14px; box-shadow: inherit;">
                        <div class="divFormInLine" style="width: 100% !important;">
                            <div style="float:left">
                                <label style="width: 800px !important;">
                                    <span style="width: 10% !important; height: 29px;">FECHA</span>
                                    <label style="line-height: 14px; width: 580px;">
                                        <asp:TextBox ID="txtFechaDesde" runat="server" class="textBox textBoxForm" placeholder="Desde" style="width: 186px; margin-right: 5px;"></asp:TextBox>
                                        <ajax:CalendarExtender ID="ce1" runat="server" CssClass="orange" TargetControlID="txtFechaDesde" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                        <asp:RequiredFieldValidator id="rfv100" runat="server"
                                            ControlToValidate="txtFechaDesde"
                                            ErrorMessage="Campo obligatorio"
                                            Validationgroup="CustomerOV"
                                            Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                            style="width: 256px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                        </asp:RequiredFieldValidator>

                                        <asp:TextBox ID="txtFechaHasta" runat="server" class="textBox textBoxForm" placeholder="Hasta" style="width: 186px;"></asp:TextBox>
                                        <ajax:CalendarExtender ID="ce2" runat="server" CssClass="orange" TargetControlID="txtFechaHasta" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                        <asp:RequiredFieldValidator id="RequiredFieldValidator7" runat="server"
                                            ControlToValidate="txtFechaHasta"
                                            ErrorMessage="Campo obligatorio"
                                            Validationgroup="CustomerOV"
                                            Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                            style="width: 256px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                        </asp:RequiredFieldValidator>

                                        &nbsp;
                                        <asp:Button ID="btnVerTodos" Text="Ver todos" CssClass="formBtnGrey1" runat="server" style="margin-left: 6px;" OnClick="btnVerTodos_Click"/>
                                        <asp:Button ID="btnBuscar" Text="Buscar" class="formBtnNar" runat="server" OnClick="btnBuscar_Click"/>
                                        
                                    </label>
                                </label>
                            </div>

                            <div style="float:right; margin-right: -40px;">
                                <label>
                                    <asp:Button ID="btnImprimirCC" Text="Imprimir" class="formBtnNar" runat="server" OnClick="btnImprimirCC_Click"/>                                        
                                </label>
                            </div>
                        </div>
                    </div>   
                </div>  
            </div>
        </section>
                         
        <asp:Panel ID="pnlPago" runat="server" Visible="false">
            <section>
                <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 30px;">
                    <div><modaltitle style="text-align:left">Pago</modaltitle></div><hr style="margin-bottom: 24px;"/>
                    <div class="divFormInLine" style="width: 100% !important;">
                        <div style="float:left">
                            <label>
                                Tipo de operacion:
                                <asp:RadioButtonList ID="rblPago" runat="server" RepeatDirection="Horizontal" TabIndex="7" AutoPostBack="true" OnTextChanged="rblPago_TextChanged" style="box-shadow: inherit; margin-bottom: -8px !important;  margin-top: -4px;">
                                    <asp:ListItem Value="PagoCuota">Pago cuota</asp:ListItem>
                                    <asp:ListItem Value="OtrosPago">Otros pagos</asp:ListItem>
                                    <asp:ListItem Value="AdelantoCuota">Adelantos de cuota</asp:ListItem>
                                    <asp:ListItem Value="CancelarSaldo">Cancelar saldo</asp:ListItem>
                                    <asp:ListItem Value="Anular">Anular Comprobantes</asp:ListItem>
                                    <asp:ListItem Value="AnularReserva">Anular Reserva</asp:ListItem>
                                    <asp:ListItem Value="Condonacion">Condonación</asp:ListItem>
                                </asp:RadioButtonList>
                            </label>
                        </div>
                    </div>
                </div>
            </section>

            <section>
                <asp:Panel ID="pnlPagoCuota" runat="server" Visible="false">            
                    <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 16px;">
                        <div><modaltitle style="text-align:left">Pago de cuota</modaltitle></div><hr style="margin-bottom: 24px;"/>
                        <div runat="server">
                            <asp:ListView ID="lvCuotas" runat="server">
                                <LayoutTemplate>
                                    <section>
                                        <table style="margin-top:-12px; margin-bottom: -37px !important;">
                                            <thead id="tableHead">
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td style="text-align: center">CAC (%)</td>
                                                    <td style="width: 10%; text-align: center">SALDO AJUSTADO</td>
                                                    <td style="text-align: center">MONTO</td>
                                                    <td style="width: 8%; text-align: center">GASTOS ADTVO.</td>
                                                    <td style="width: 8%; text-align: center">1re Venc.</td>
                                                    <td style="text-align: center">IMPORTE</td>
                                                    <td style="width: 8%;">2do Venc.</td>
                                                    <td style="text-align: center">IMPORTE</td>
                                                    <td style="text-align: center">ESTADO</td>
                                                    <td style="text-align: center">SALDO</td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                            </tbody>
                                        </table>
                                    </section>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer">
                                        <td>
                                            <asp:RadioButton id="rdbUser" runat="server" OnClick="javascript:SelectSingleRadioButtonPagoCuota(this.id)" />
                                            <asp:Label ID="lbIdConfirm" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("nro") %>' />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="lbCAC" runat="Server" Text='<%#Eval("GetVariacionCAC") %>' />
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
                                        <td style="text-align: center">
                                            <asp:Label ID="lbFecha1venc" runat="Server" Text='<%#Eval("FechaVencimiento1", "{0:dd/MM/yyyy}") %>' />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lbVencimiento1" runat="Server" Text='<%#Eval("GetVencimiento1") %>' />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="lbFecha2venc" runat="Server" Text='<%#Eval("FechaVencimiento2", "{0:dd/MM/yyyy}") %>' />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lbMontoAjustado" runat="Server" Text='<%#Eval("GetVencimiento2") %>' />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>' />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label1" runat="Server" Text='<%#Eval("GetSaldo") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                        <div runat="server">
                            <div runat="server">
                                <div>
                                    <label>
                                        <asp:Button ID="btnContinuarPagoCuota" Text="Siguiente" class="formBtnNar" runat="server" style="
                                            text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                            width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px;"
                                            OnClick="btnContinuarPagoCuota_Click"/>
                                    </label>

                                    <label>
                                        <asp:Button ID="btnCancelarPagoCuota" Text="Cancelar" class="formBtnNarCancel" runat="server" style="margin-left: 10px;" OnClick="btnCancelarPagoCuota_Click"/>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <asp:Panel ID="PagoCuotaMonto" runat="server" Visible="false">
                        <div runat="server" class="formHolderCalendar" style="padding: 28px 25px 30px;">
                            <div class="divFormInLine" style="width: 200px !important;">
                                    <div style="float:left">
                                        <label>
                                            Moneda:
                                            <asp:RadioButtonList ID="rblMonedaPago" runat="server" RepeatDirection="Horizontal" TabIndex="7" AutoPostBack="true" OnTextChanged="rblMonedaPago_TextChanged" style="box-shadow: inherit; margin-bottom: -8px !important;  margin-top: -4px;">
                                                <asp:ListItem Value="Pesos">Pesos</asp:ListItem>
                                                <asp:ListItem Value="Dolar">Dólar</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </label>
                                    </div>
                                </div>

                            <div class="divFormInLine">
                                <div style="float:left">
                                    <label>
                                        Importe:
                                        <asp:TextBox ID="txtImportePago" runat="server" CssClass="textBox textBoxForm decimal" TabIndex="7" style="width: 256px;" Enabled="false"></asp:TextBox>    
                                        <asp:RequiredFieldValidator id="RequiredFieldValidator114" runat="server"
                                            ControlToValidate="txtImportePago" ErrorMessage="Campo obligatorio" InitialValue="0"
                                            Validationgroup="CustomerPago" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                            style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </div>                           
                            </div>                
                        </div>

                        <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 0px;">
                            <div class="divFormInLine">
                                <div style="float:left; width: 900px;">
                                    <label>
                                        Concepto:
                                        <asp:TextBox ID="txtConceptoPago" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 100%; height: 80px;" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                        <asp:RequiredFieldValidator id="RequiredFieldValidator2225" runat="server"
                                            ControlToValidate="txtConceptoPago" ErrorMessage="Campo obligatorio" InitialValue="0"
                                            Validationgroup="CustomerPago" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                            style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div runat="server" class="formHolderCalendar" style="padding: 12px 10px 10px;">
                            <div runat="server">
                                <div style="float:left; margin-top: 4px;">
                                    <label>
                                        <asp:Button ID="btnPago" Text="Aceptar" class="formBtnNar" runat="server" style="
                                            text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                            width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"
                                            OnClick="btnPago_Click" Validationgroup="CustomerPago"/>
                                    </label>
                                    <label>
                                        <asp:Button ID="btnPagoCancelar" Text="Cancelar" class="formBtnNarCancel" runat="server" style="margin-left: 10px;" OnClick="btnPagoCancelar_Click"/>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </section>

            <section>
                <asp:Panel ID="pnlOtrosPago" runat="server" Visible="false">
                    <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 30px;">
                        <div><modaltitle style="text-align:left">Otros Pagos</modaltitle></div><hr style="margin-bottom: 24px;"/>
                        <div class="divFormInLine">
                            <div style="float:left">
                                <label>
                                    Moneda:
                                    <asp:RadioButtonList ID="rblMonedaOtrosPago" runat="server" RepeatDirection="Horizontal" TabIndex="7" AutoPostBack="true" OnTextChanged="rblMonedaOtrosPago_TextChanged" style="box-shadow: inherit; margin-bottom: -8px !important;  margin-top: -4px;">
                                        <asp:ListItem Value="Pesos">Pesos</asp:ListItem>
                                        <asp:ListItem Value="Dolar">Dólar</asp:ListItem>
                                    </asp:RadioButtonList>
                                </label>
                            </div>
                        </div>

                        <div class="divFormInLine">
                            <div style="float:left">
                                <label>
                                    Importe:
                                    <asp:TextBox ID="txtImporteOtrosPago" runat="server" CssClass="textBox textBoxForm decimal" Text="0" TabIndex="7" style="width: 256px;" Enabled="false"></asp:TextBox>    
                                    <asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server"
                                        ControlToValidate="txtImporteOtrosPago" ErrorMessage="Campo obligatorio" InitialValue="0"
                                        Validationgroup="vgOtrosPago" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                        style="width: 100%; display: block; float: left; margin-top:-4px; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                    </asp:RequiredFieldValidator>
                                </label>
                            </div>                           
                        </div>
                    </div>
                    <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 0px;">
                        <div class="divFormInLine">
                            <div style="float:left; width: 900px;">
                                <label>
                                    Concepto:
                                    <asp:TextBox ID="txtConceptoOtrosPago" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 100%; height: 80px;" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                    <asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server"
                                        ControlToValidate="txtConceptoOtrosPago" ErrorMessage="Campo obligatorio"
                                        Validationgroup="vgOtrosPago" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                        style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                    </asp:RequiredFieldValidator>
                                </label>
                            </div>
                        </div>
                    </div>
                    <div runat="server" class="formHolderCalendar" style="padding: 12px 10px 10px;">
                        <div runat="server">
                            <div style="float:left; margin-top: 4px;">
                                <label>
                                    <asp:Button ID="btnOtrosPago" Text="Aceptar" class="formBtnNar" runat="server" style="
                                        text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                        width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"
                                        OnClick="btnOtrosPago_Click" Validationgroup="vgOtrosPago"/>
                                </label>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </section>

            <section>
                <asp:Panel ID="pnlAdelantoCuota" runat="server" Visible="false">
                    <div runat="server" class="formHolder" style="padding: 12px 25px 30px;">
                        <div><modaltitle style="text-align:left">Adelanto de cuotas</modaltitle></div><hr style="margin-bottom: 24px;"/>
                        <div class="divFormInLine">
                            <label>
                                Obras:
                                <asp:DropDownList ID="cbProyectos" runat="server" TabIndex="2" style="width: 270px; margin-top: 2px;" AutoPostBack="true" OnSelectedIndexChanged="cbProyectos_SelectedIndexChanged"></asp:DropDownList>
                            </label>
                        </div>

                        <div class="divFormInLine">
                            <label>
                            Formas de pago:
                            <asp:DropDownList ID="cbFormaPago" runat="server" TabIndex="2" style="width: 270px; margin-top: 2px;" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="cbFormaPago_SelectedIndexChanged"></asp:DropDownList>
                            </label>
                        </div>
                    </div>
                    <asp:Panel ID="pnlCantCuotas" runat="server" Visible="false">
                        <div runat="server" class="formHolder" style="padding: 12px 25px 30px;">
                            <div class="divFormInLine">
                                <div>
                                    <label style="width: 250px;">
                                        Cantidad de cuotas para adelantas:
                                        <asp:TextBox ID="txtCantCuotas" runat="server" CssClass="textBox textBoxForm" style="width: 270px; margin-top: 2px;" Validationgroup="validateAnticipoCuota" onkeypress="return onKeyDecimal(event,this);"/>
                                        <asp:RequiredFieldValidator id="rfvCantCuotas" runat="server"
                                            ControlToValidate="txtCantCuotas" ErrorMessage="Campo obligatorio"
                                            Validationgroup="vgAdelantoCuota" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator" style="width: 270px;">
                                        </asp:RequiredFieldValidator>    
                                    </label>
                                </div>
                            </div>
                            <div class="divFormInLine">
                                <div>
                                    <label style="margin-top: 16px;">
                                        <asp:Button ID="btnSiguiente" Text="Siguiente" class="formBtnNar" runat="server" Validationgroup="vgAdelantoCuota" OnClick="btnSiguiente_Click"
                                            style="text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                                width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"/>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div runat="server">
                        <asp:UpdatePanel ID="pnlListadoCuotas" runat="server" Visible="false">
                            <ContentTemplate>
                                <div runat="server" class="formHolder" style="padding: 18px 25px 0px;">
                                    <div align="left" class="h7" style="float:right;width: 100%;">A continuación se listan las últimas cuotas que se cancelarán</div>
                                    <div class="divFormInLine" style="width: 100% !Important;"">
                                        <asp:Panel runat="server" ID="pnlListViewCuotas">
                                            <crm:Cuotas runat="server" id="usrCtrl" />
                                        </asp:Panel>                    
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <asp:Panel ID="pnlConceptoAdelanto" runat="server" Visible="false">
                        <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 0px;">
                        <div class="divFormInLine">
                            <div style="float:left; width: 900px;">
                                <label>
                                    Concepto:
                                    <asp:TextBox ID="txtConceptoAdelantoCuota" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 100%; height: 80px;" TextMode="MultiLine" MaxLength="80"></asp:TextBox>
                                    <asp:RequiredFieldValidator id="RequiredFieldValidator5575" runat="server"
                                        ControlToValidate="txtConceptoAdelantoCuota" ErrorMessage="Campo obligatorio"
                                        Validationgroup="vgAdelantoCuota" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                        style="width: 100%; margin-top:-4px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                    </asp:RequiredFieldValidator>
                                </label>
                            </div>
                        </div>
                    </div>                    
                        <div runat="server" class="formHolderCalendar" style="padding: 12px 10px 10px;">
                            <div runat="server">
                                <div style="float:left; margin-top: 4px;">
                                    <label>
                                        <asp:Button ID="btnAdelantoCuota" Text="Aceptar" class="formBtnNar" runat="server" style="
                                            text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                            width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"
                                            OnClick="btnAdelantoCuota_Click" Validationgroup="vgAdelantoCuota"/>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </section>

            <section>
                <asp:Panel ID="pnlCancelarSaldo" runat="server" Visible="false">
                    <div runat="server" class="formHolder" style="padding: 12px 25px 30px;">
                        <div><modaltitle style="text-align:left">Cancelación de saldo</modaltitle></div><hr style="margin-bottom: 24px;"/>
                        <div class="divFormInLine">
                            <label>
                                Obras:
                                <asp:DropDownList ID="cbProyectosCancelarSaldo" runat="server" TabIndex="2" style="width: 270px; margin-top: 2px;" AutoPostBack="true" OnSelectedIndexChanged="cbProyectosCancelarSaldo_SelectedIndexChanged"></asp:DropDownList>
                            </label>
                        </div>

                        <div class="divFormInLine">
                            <label>
                                Saldo a cancelar:
                                <asp:Label ID="lbCancelarSaldo" runat="server" Text="-" style="width: 270px; border-radius: 3px; border-right: 1px dotted rgba(0, 0, 0, 0.1) !important;"></asp:Label>
                            </label>
                        </div>
                    </div>
                    <asp:Panel ID="pnlConceptoCancelarSaldo" runat="server" Visible="false">
                        <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 0px;">
                        <div class="divFormInLine">
                            <div style="float:left; width: 900px;">
                                <label>
                                    Concepto:
                                    <asp:TextBox ID="txtConceptoCancelarSaldo" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 100%; height: 80px;" TextMode="MultiLine" MaxLength="80"></asp:TextBox>
                                    <asp:RequiredFieldValidator id="RequiredFieldValidator1212" runat="server"
                                        ControlToValidate="txtConceptoCancelarSaldo" ErrorMessage="Campo obligatorio"
                                        Validationgroup="vgCancelarSaldo" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                        style="width: 100%; display: block; float: left; margin-top: -4px; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                    </asp:RequiredFieldValidator>
                                </label>
                            </div>
                        </div>
                    </div>                    
                        <div runat="server" class="formHolderCalendar" style="padding: 12px 10px 10px;">
                            <div runat="server">
                                <div style="float:left; margin-top: 4px;">
                                    <label>
                                        <asp:Button ID="btnCancelarSaldo" Text="Aceptar" class="formBtnNar" runat="server" style="
                                            text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                            width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"
                                            OnClick="btnCancelarSaldo_Click" Validationgroup="vgCancelarSaldo"/>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </section>

            <section>
                <asp:Panel ID="pnlAnular" runat="server" Visible="false">
                    <div runat="server" class="formHolder" style="padding: 12px 25px 30px;">
                        <div><modaltitle style="text-align:left">Anular comprobantes</modaltitle></div><hr style="margin-bottom: 10px;"/>
                        <div align="left" class="h7" style="float:right;width: 100%;margin-bottom: 10px;">Sólo se podrán anular los comprobantes emitidos en el día</div>
                        <div runat="server">
                            <asp:ListView ID="lvAnular" runat="server">
                                <LayoutTemplate>
                                    <section>
                                        <table style="margin-top:-12px; margin-bottom: -37px !important;">
                                            <thead id="tableHead">
                                                <tr>
                                                    <td style="width: 5%;"></td>
                                                    <td style="width: 35%; text-align: center">TIPO</td>
                                                    <td style="width: 35%; text-align: center">NRO.</td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                            </tbody>
                                        </table>
                                    </section>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer">
                                        <td>
                                            <asp:RadioButton id="rdbUser" runat="server" OnClick="javascript:SelectSingleRadioButtonComprobante(this.id)" />
                                            <asp:Label ID="lbId" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lbTipo" runat="server" Text='<%# Eval("tipo") %>' Visible="false"></asp:Label>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("GetTipo") %>' />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="lbCAC" runat="Server" Text='<%#Eval("nro") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                        <div class="divFormInLine" style="margin-left: -15px;">
                            <div>
                                <asp:HiddenField ID="hfIdComprobanteAnular" runat="server" />
                                <asp:HiddenField ID="hfTipoComprobanteAnular" runat="server" />
                                <label style="margin-right: -43px;">
                                    <asp:Button ID="btnSiguienteAnular" Text="Siguiente" class="formBtnNar" runat="server" OnClick="btnSiguienteAnular_Click"
                                        style="text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                            width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"/>
                                </label>
                                <label>
                                    <asp:Button ID="btnCancelarAnular" Text="Cancelar" CssClass="btnClose" class="formBtnNar" runat="server" OnClick="btnCancelarAnular_Click"
                                        style="text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                            width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"/>
                                </label>
                            </div>
                        </div>
                    </div>

                    <asp:Panel ID="pnlConceptoAnular" runat="server" Visible="false">
                        <div runat="server" class="formHolderCalendar" style="padding: 30px 25px 0px;">
                            <div class="divFormInLine">
                                <div style="float:left; width: 900px;">
                                    <label>
                                        Concepto:
                                        <asp:TextBox ID="txtConceptoAnular" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 100%; height: 80px;" TextMode="MultiLine" MaxLength="80"></asp:TextBox>
                                        <asp:RequiredFieldValidator id="RequiredFieldValidator15575" runat="server"
                                            ControlToValidate="txtConceptoAnular" ErrorMessage="Campo obligatorio"
                                            Validationgroup="vgAnularCuota" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                            style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </div>
                            </div>
                        </div>                    
                        <div runat="server" class="formHolderCalendar" style="padding: 12px 10px 10px;">
                            <div runat="server">
                                <div style="float:left; margin-top: 4px;">
                                    <label>
                                        <asp:Button ID="btnAnularCuota" Text="Aceptar" class="formBtnNar" runat="server" style="
                                            text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                            width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"
                                            OnClick="btnAnularCuota_Click" Validationgroup="vgAnularCuota"/>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </section>

            <section>
                <asp:Panel ID="pnlAnularReserva" runat="server" Visible="false">
                    <asp:HiddenField ID="hfIdItemCCU" runat="server" />
                    <asp:HiddenField ID="hfIdUnidad" runat="server" />
                    <div runat="server" class="formHolder" style="padding: 12px 25px 18px;">
                        <div><modaltitle style="text-align:left">Anular reserva/s</modaltitle></div><hr/>
                        <asp:Panel ID="pnlOneReserva" runat="server" Visible="false" style="margin-bottom: 4px">
                            <div class="divFormInLine">
                                <label style="width: 250px;">
                                    COD U.F.:
                                    <asp:Label ID="lbCodUF" runat="server" Text="-" style="width: 270px; border-radius: 3px; border-right: 1px dotted rgba(0, 0, 0, 0.1) !important;"></asp:Label>
                                </label>
                            </div>
                            <div class="divFormInLine">
                                <label style="width: 250px;">
                                    Nivel:
                                    <asp:Label ID="lbNivelReserva" runat="server" Text="-" style="width: 270px; border-radius: 3px; border-right: 1px dotted rgba(0, 0, 0, 0.1) !important;"></asp:Label>
                                </label>
                            </div>
                            <div class="divFormInLine">
                                <label style="width: 250px;">
                                    Unidad
                                    <asp:Label ID="lbUnidadReserva" runat="server" Text="-" style="width: 270px; border-radius: 3px; border-right: 1px dotted rgba(0, 0, 0, 0.1) !important;"></asp:Label>
                                </label>
                            </div>
                        </asp:Panel>                        
                    </div>
                    <asp:Panel ID="pnlReservas" runat="server" Visible="false">
                    <div>
                        <div runat="server">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div runat="server" class="formHolder" style="padding: 18px 25px 0px;">
                                        <div class="h7" style="float:right;width: 100%;">A continuación se listan las reservas</div>
                                        <div class="divFormInLine" style="width: 100% !Important; margin-bottom: -30px;">
                                            <asp:ListView ID="lvReservas" runat="server">
                                                <LayoutTemplate>
                                                    <section>
                                                        <table style="width:100%">                            
                                                        <thead id="tableHead">
                                                            <tr>
                                                                <td style="width: 1%; text-align:center"></td>
                                                                <td style="width: 20%; text-align:center">NIVEL</td>
                                                                <td style="width: 20%; text-align:center">UNIDAD</td>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                                        </tbody>
                                                    </table>
                                                    </section>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr style="color:#b40b0b;">
                                                        <td style="text-align:center">
                                                            <asp:CheckBox ID="chBoxConfirm" runat="server" />
                                                            <asp:Label ID="lbIdReserva" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lbIdItemCCU" runat="server" Text='<%# Eval("IdItemCCU") %>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td style="text-align:center">
                                                            <asp:Label ID="lbFecha" runat="server" Text='<%# Eval("GetNivel") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align:center">
                                                            <asp:Label ID="lbCliente" runat="server" Text='<%# Eval("GetNroUnidad")%>'></asp:Label>                                
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <EmptyDataTemplate>
                                                    <section>
                                                        <table id="Table1" width="100%" runat="server">
                                                            <tr>
                                                                <td style="text-align:center"><b>No se encontraron reservas.<b/></td>
                                                            </tr>
                                                        </table>
                                                    </section>
                                                </EmptyDataTemplate>
                                            </asp:ListView>                  
                                        </div>
                                    </div>
                                    <div runat="server" class="formHolder" style="padding: 4px 10px 2px;">
                                        <div class="divFormInLine" style="margin-bottom: 18px;">
                                            <div>
                                                <label style="margin-top: 16px; margin-right: -43px;">
                                                    <asp:Button ID="btnSiguienteReservas" Text="Siguiente" class="formBtnNar" runat="server" OnClick="btnSiguienteReservas_Click"
                                                        style="text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                                            width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"/>
                                                </label>
                                                <label style="margin-top: 16px;">
                                                    <asp:Button ID="btnCancelarReservas" Text="Cancelar" CssClass="btnClose" class="formBtnNar" runat="server" OnClick="btnCancelarReservas_Click"
                                                        style="text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                                            width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"/>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlConceptoAnularReserva" runat="server" Visible="false">
                        <div runat="server" class="formHolderCalendar" style="padding: 14px 25px 0px;">
                            <div class="divFormInLine">
                                <div style="float:left; width: 900px;">
                                    <label>
                                        Concepto:
                                        <asp:TextBox ID="txtConceptoAnularReserva" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 100%; height: 80px;" TextMode="MultiLine" MaxLength="80"></asp:TextBox>
                                        <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="txtConceptoAnularReserva" ErrorMessage="Campo obligatorio"
                                            Validationgroup="vgAnularReserva" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                            style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </div>
                            </div>
                        </div>                    
                        <div runat="server" class="formHolderCalendar" style="padding: 12px 10px 10px;">
                            <div runat="server">
                                <div style="float:left; margin-top: 4px;">
                                    <label>
                                        <asp:Button ID="btnAnularReserva" Text="Aceptar" class="formBtnNar" runat="server" OnClick="btnAnularReserva_Click"
                                            style="text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                            width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"
                                            Validationgroup="vgAnularReserva"/>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </section>

            <section>
                <asp:Panel ID="pnlCondonacion" runat="server" Visible="false">            
                    <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 16px;">
                        <div><modaltitle style="text-align:left">Condonación</modaltitle></div><hr style="margin-bottom: 24px;"/>
                        <div runat="server">
                            <asp:ListView ID="lvCondonacion" runat="server">
                                <LayoutTemplate>
                                    <section>
                                        <table style="margin-top:-12px; margin-bottom: -37px !important;">
                                            <thead id="tableHead">
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td style="text-align: center">CAC (%)</td>
                                                    <td style="width: 10%; text-align: center">SALDO AJUSTADO</td>
                                                    <td style="text-align: center">MONTO</td>
                                                    <td style="width: 8%; text-align: center">GASTOS ADTVO.</td>
                                                    <td style="width: 8%; text-align: center">1re Venc.</td>
                                                    <td style="text-align: center">IMPORTE</td>
                                                    <td style="width: 8%;">2do Venc.</td>
                                                    <td style="text-align: center">IMPORTE</td>
                                                    <td style="text-align: center">ESTADO</td>
                                                    <td style="text-align: center">RECIBO</td>
                                                    <td style="text-align: center">SALDO</td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                            </tbody>
                                        </table>
                                    </section>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer">
                                        <td>
                                            <asp:RadioButton id="rdbCondonacion" runat="server" OnClick="javascript:SelectSingleRadioButtonCondonacion(this.id)" />
                                            <asp:Label ID="lbIdConfirm" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("nro") %>' />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="lbCAC" runat="Server" Text='<%#Eval("GetVariacionCAC") %>' />
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
                                        <td style="text-align: center">
                                            <asp:Label ID="lbFecha1venc" runat="Server" Text='<%#Eval("FechaVencimiento1", "{0:dd/MM/yyyy}") %>' />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lbVencimiento1" runat="Server" Text='<%#Eval("GetVencimiento1") %>' />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="lbFecha2venc" runat="Server" Text='<%#Eval("FechaVencimiento2", "{0:dd/MM/yyyy}") %>' />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lbMontoAjustado" runat="Server" Text='<%#Eval("GetVencimiento2") %>' />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>' />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label1" runat="Server" Text='<%#Eval("GetSaldo") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                        <div runat="server">
                            <div runat="server">
                                <div>
                                    <label>
                                        <asp:Button ID="btnContinuarCondonacion" Text="Siguiente" class="formBtnNar" runat="server" style="
                                            text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                            width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px;"
                                            OnClick="btnContinuarCondonacion_Click"/>
                                    </label>
                                    <label>
                                        <asp:Button ID="btnCancelarCondonacion" Text="Cancelar" class="formBtnNarCancel" runat="server" style="margin-left: 10px;" OnClick="btnCancelarCondonacion_Click"/>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>                    

                    <asp:Panel ID="CondonacionCuotaMonto" runat="server" Visible="false">
                        <div runat="server" class="formHolderCalendar" style="padding: 28px 25px 30px;">
                            <div class="divFormInLine" style="width: 200px !important;">
                                    <div style="float:left">
                                        <label>
                                            Moneda:
                                            <asp:RadioButtonList ID="rblMonedaCondonacion" runat="server" RepeatDirection="Horizontal" TabIndex="7" AutoPostBack="true" OnTextChanged="rblMonedaCondonacion_TextChanged" style="box-shadow: inherit; margin-bottom: -8px !important;  margin-top: -4px;">
                                                <asp:ListItem Value="Pesos">Pesos</asp:ListItem>
                                                <asp:ListItem Value="Dolar">Dólar</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </label>
                                    </div>
                                </div>

                            <div class="divFormInLine">
                                <div style="float:left">
                                    <label>
                                        Importe:
                                        <asp:TextBox ID="txtImporteCondonacion" runat="server" CssClass="textBox textBoxForm decimal" TabIndex="7" style="width: 256px;" Enabled="false"></asp:TextBox>    
                                        <asp:RequiredFieldValidator id="RequiredFieldValidator11214" runat="server"
                                            ControlToValidate="txtImporteCondonacion" ErrorMessage="Campo obligatorio" InitialValue="0"
                                            Validationgroup="CustomerCondonacion" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                            style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </div>                           
                            </div>                
                        </div>
                        <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 0px;">
                            <div class="divFormInLine">
                                <div style="float:left; width: 900px;">
                                    <label>
                                        Concepto:
                                        <asp:TextBox ID="txtConceptoCondonacion" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 100%; height: 80px;" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                        <asp:RequiredFieldValidator id="RequiredFieldValidator12225" runat="server"
                                            ControlToValidate="txtConceptoCondonacion" ErrorMessage="Campo obligatorio" InitialValue="0"
                                            Validationgroup="CustomerCondonacion" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                            style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div runat="server" class="formHolderCalendar" style="padding: 12px 10px 10px;">
                            <div runat="server">
                                <div style="float:left; margin-top: 4px;">
                                    <label>
                                        <asp:Button ID="btnCondonacion" Text="Aceptar" class="formBtnNar" runat="server" style="
                                            text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                            width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"
                                            OnClick="btnCondonacion_Click" Validationgroup="CustomerCondonacion"/>
                                    </label>
                                    <label>
                                        <asp:Button ID="btnCondonacionCancelar" Text="Cancelar" class="formBtnNarCancel" runat="server" style="margin-left: 10px;" OnClick="btnCancelarCondonacion_Click"/>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </section>
        </asp:Panel>
               
        <asp:Panel ID="pnlOk" runat="server" Visible="false">
            <section>
                <div runat="server" class="formHolder messageOk" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                    <div>
                        <asp:Label ID="lbMensaje" runat="server" class="messageOk" style="margin-left: 7px;">El pago fue registrado correctamente.</asp:Label>
                        <br />
                        <asp:Label ID="Label4" runat="server" class="messageOk" style="margin-left: 7px;">Se emitió el recibo del pago. Puede descargarlo presionando</asp:Label>
                        <asp:LinkButton ID="lkbRecibo" runat="server" OnClick="lkbRecibo_Click" style="color:#3c763d;"><b>aquí</b></asp:LinkButton>.
                    </div>
                </div>
            </section>
        </asp:Panel>

        <asp:Panel ID="pnlCreditoOk" runat="server" Visible="false">
            <section>
                <div runat="server" class="formHolder messageOk" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                    <div>
                        <asp:Label ID="Label7" runat="server" class="messageOk" style="margin-left: 7px;">La nota de crédito fue registrado correctamente.</asp:Label>
                        <br />
                        <asp:Label ID="Label8" runat="server" class="messageOk" style="margin-left: 7px;">Se emitió el recibo correspondiente. Puede descargarlo presionando</asp:Label>
                        <asp:LinkButton ID="lkbCredito" runat="server" OnClick="lkbCredito_Click" style="color:#3c763d;"><b>aquí</b></asp:LinkButton>.
                    </div>
                </div>
            </section>
        </asp:Panel>

        <asp:Panel ID="pnlOkNotaDebito" runat="server" Visible="false">
            <section>
                <div runat="server" class="formHolder messageOk" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                    <div>
                        <asp:Label ID="Label3" runat="server" class="messageOk" style="margin-left: 7px;">La nota de débito fue registrado correctamente.</asp:Label>
                        <br />
                        <asp:Label ID="Label6" runat="server" class="messageOk" style="margin-left: 7px;">Se emitió el recibo correspondiente. Puede descargarlo presionando</asp:Label>
                        <asp:LinkButton ID="lkbNotaDebito" runat="server" OnClick="lkbNotaDebito_Click" style="color:#3c763d;"><b>aquí</b></asp:LinkButton>.
                    </div>
                </div>
            </section>
        </asp:Panel>

        <asp:Panel ID="pnlMensajePago" runat="server" Visible="false">
            <section>
                <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                    <div>
                        <asp:Label ID="lbMensajePago" runat="server" class="messageError" style="margin-left: 7px;"></asp:Label>
                    </div>
                </div>
            </section>
        </asp:Panel>

        <asp:Panel ID="pnlMensajeAnular" runat="server" Visible="false">
            <section>
                <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                    <div>
                        <asp:Label ID="Label11" runat="server" CssClass="messageError" style="margin-left: 7px;">No se encontraron pagos.</asp:Label>
                    </div>
                </div>
            </section>
        </asp:Panel>

        <asp:Panel ID="pnlMensajeReserva" runat="server" Visible="false">
            <section>
                <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                    <div>
                        <asp:Label ID="lbMensajeReserva" runat="server" CssClass="messageError" style="margin-left: 7px;">Seleccione de la lista una de las unidades reservadas</asp:Label>
                    </div>
                </div>
            </section>
        </asp:Panel>

        <asp:Panel ID="pnlAdelanto" runat="server" Visible="false">
            <section>
                <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                    <div>
                        <asp:Label ID="Label9" runat="server" class="messageOk" style="margin-left: 7px;">No se pueden adelantar cuotas porque hay cuotas pendientes</asp:Label>
                    </div>
                </div>
            </section>
        </asp:Panel>
                 
        <asp:Panel ID="pnlCredito" runat="server" Visible="false">
            <section>
                <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 30px;">
                    <div><modaltitle style="text-align:left">Nota de Crédito / Pago</modaltitle></div><hr style="margin-bottom: 24px;"/>

                    <div class="divFormInLine">
                        <div style="float:left">
                            <label>
                                Moneda:
                                <asp:RadioButtonList ID="rblMonedaCredito" runat="server" RepeatDirection="Horizontal" TabIndex="7" AutoPostBack="true" OnTextChanged="rblMonedaCredito_TextChanged" style="box-shadow: inherit; margin-bottom: -8px !important;  margin-top: -4px;">
                                    <asp:ListItem Value="Pesos">Pesos</asp:ListItem>
                                    <asp:ListItem Value="Dolar">Dólar</asp:ListItem>
                                </asp:RadioButtonList>
                            </label>
                        </div>
                    </div>

                    <div class="divFormInLine">
                        <div style="float:left">
                            <label>
                                Importe:
                                <asp:TextBox ID="txtImporteCredito" runat="server" CssClass="textBox textBoxForm decimal" TabIndex="7" style="width: 256px;" Enabled="false"></asp:TextBox>    
                                <asp:RequiredFieldValidator id="RequiredFieldValidator454" runat="server"
                                    ControlToValidate="txtImporteCredito" ErrorMessage="Campo obligatorio" InitialValue="0"
                                    Validationgroup="CustomerCredito" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                    style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                </asp:RequiredFieldValidator>
                            </label>
                        </div>                           
                    </div>
                </div>
                <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 0px;">
                    <div class="divFormInLine">
                        <div style="float:left; width: 900px;">
                            <label>
                                Concepto:
                                <asp:TextBox ID="txtConceptoCredito" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 100%; height: 80px;" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                <asp:RequiredFieldValidator id="RequiredFieldValidator325" runat="server"
                                    ControlToValidate="txtConceptoCredito" ErrorMessage="Campo obligatorio" InitialValue="0"
                                    Validationgroup="CustomerCredito" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                    style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                </asp:RequiredFieldValidator>
                            </label>
                        </div>
                    </div>
                </div>
                <div runat="server" class="formHolderCalendar" style="padding: 12px 10px 10px;">
                    <div runat="server">
                        <div style="float:left; margin-top: 4px;">
                            <label>
                                <asp:Button ID="btnCredito" Text="Aceptar" class="formBtnNar" runat="server" style="
                                    text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                    width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"
                                    OnClick="btnCredito_Click" Validationgroup="CustomerCredito"/>
                            </label>
                        </div>
                    </div>
                </div>
            </section>
        </asp:Panel>

        <asp:Panel ID="pnlNDebito" runat="server" Visible="false">
            <section>
                <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 30px;">
                    <div><modaltitle style="text-align:left">Nota de Débito</modaltitle></div><hr style="margin-bottom: 24px;"/>
                    <div class="divFormInLine">
                        <div style="float:left">
                            <label>
                                Moneda:
                                <asp:RadioButtonList ID="rblMonedaNotaDebito2" runat="server" RepeatDirection="Horizontal" TabIndex="7" AutoPostBack="true" OnTextChanged="rblMonedaNotaDebito2_TextChanged" style="box-shadow: inherit; margin-bottom: -8px !important;  margin-top: -4px;">
                                    <asp:ListItem Value="Pesos">Pesos</asp:ListItem>
                                    <asp:ListItem Value="Dolar">Dólar</asp:ListItem>
                                </asp:RadioButtonList>
                            </label>
                        </div>
                    </div>
                    <div class="divFormInLine">
                        <div style="float:left">
                            <label>
                                Importe: 
                                <asp:TextBox ID="txtImporteNotaDebito2" runat="server" CssClass="textBox textBoxForm decimal" Enabled="false" TabIndex="7" style="width: 256px;"></asp:TextBox>    
                                <asp:RequiredFieldValidator id="RequiredFieldValidator444" runat="server"
                                    ControlToValidate="txtImporteNotaDebito2" ErrorMessage="Campo obligatorio" InitialValue="0"
                                    Validationgroup="CustomerNotaDebito2" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                    style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                </asp:RequiredFieldValidator>
                            </label>
                        </div>                           
                    </div>
                </div>
                <div runat="server" class="formHolderCalendar" style="padding: 12px 25px 0px;">
                    <div class="divFormInLine">
                            <div style="float:left; width: 900px;">
                                <label>
                                    Concepto:
                                    <asp:TextBox ID="txtConceptoNotaDebito2" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 100%; height: 80px;" TextMode="MultiLine" MaxLength="80"></asp:TextBox>
                                    <asp:RequiredFieldValidator id="RequiredFieldValidator555" runat="server"
                                        ControlToValidate="txtConceptoNotaDebito2" ErrorMessage="Campo obligatorio" InitialValue="0"
                                        Validationgroup="CustomerNotaDebito2" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                        style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                    </asp:RequiredFieldValidator>
                                </label>
                            </div>
                    </div>
                </div>
                <div runat="server" class="formHolderCalendar" style="padding: 12px 10px 10px;">
                    <div runat="server">
                        <div style="float:left; margin-top: 4px;">
                            <label>
                                <asp:Button ID="btnNotaDebito" Text="Aceptar" class="formBtnNar" runat="server" style="
                                    text-decoration: none; border: 1px solid rgba(0, 0, 0, 0.2); color: #fff; float: right; line-height: 18px;
                                    width: auto !important; padding: 6px 15px !important; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; color: #fff; background: #b40b0b; background: -moz-linear-gradient(top, #b40b0b 0%, #890909 100%); background: -webkit-linear-gradient(top, #b40b0b 0%,#890909 100%); background: linear-gradient(to bottom, #b40b0b 0%,#890909 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b40b0b', endColorstr='#890909',GradientType=0 ); cursor: pointer; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; float:left; margin-top: 15px; margin-left: 15px;"
                                    OnClick="btnNotaDebito_Click" Validationgroup="CustomerNotaDebito2"/>
                            </label>
                        </div>
                    </div>
                </div>
            </section>
        </asp:Panel>      
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnBuscar" />
        <asp:PostBackTrigger ControlID="btnVerTodos" />
        <asp:PostBackTrigger ControlID="btnPagoCC" />
        <asp:PostBackTrigger ControlID="btnCreditoCC" />
        <asp:PostBackTrigger ControlID="btnNotaDebitoCC" />
        <asp:PostBackTrigger ControlID="btnPago" />
        <asp:PostBackTrigger ControlID="btnOtrosPago" /> 
        <asp:PostBackTrigger ControlID="btnAdelantoCuota" /> 
        <asp:PostBackTrigger ControlID="btnCancelarSaldo" />         
        <asp:PostBackTrigger ControlID="btnAnularCuota" />         
        <asp:PostBackTrigger ControlID="btnCredito" />
        <asp:PostBackTrigger ControlID="btnNotaDebito" />
        <asp:PostBackTrigger ControlID="btnImprimirCC" />        
        <asp:PostBackTrigger ControlID="lkbRecibo" />
        <asp:PostBackTrigger ControlID="btnAnularReserva" />
        <asp:PostBackTrigger ControlID="lkbCredito" />
        <asp:PostBackTrigger ControlID="lkbNotaDebito" />   
        <asp:PostBackTrigger ControlID="btnContinuarCondonacion" /> 
        <asp:PostBackTrigger ControlID="btnCancelarCondonacion" /> 
        <asp:PostBackTrigger ControlID="btnCondonacion" /> 
        <asp:PostBackTrigger ControlID="btnCondonacionCancelar" />                           
    </Triggers>
</asp:UpdatePanel>

<section>
    <div class="formHolder">
        <asp:ListView ID="lvCC" runat="server" OnItemCommand="lvCC_ItemCommand">
            <LayoutTemplate>
                <section>
                    <table style="margin-bottom: -20px;">
                        <thead id="tableHead">
                            <tr>
                                <td style="width: 5%; text-align: center">FECHA</td>
                                <td style="width: 53%;">CONCEPTO</td>
                                <td style="width: 9%; text-align: center">DÉBITO</td>
                                <td style="width: 9%; text-align: center">CRÉDITO</td>
                                <td style="width: 9%; text-align: center">SALDO</td>
                                <td style="width: 10%; text-align: center">COMPROBANTE</td>
                                <td style="width: 5%"></td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </tbody>
                    </table>
                </section>
            </LayoutTemplate>
            <ItemTemplate>
                <tr style="cursor: pointer">
                    <td style="text-align: center">
                        <asp:Label ID="lbIdCuentaCorrienteUsuario" runat="Server" Text='<%#Eval("id") %>' Visible="false"/>
                        <asp:Label ID="Label2" runat="Server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label14" runat="Server" Text='<%#Eval("Concepto") %>' />
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lbDebito" runat="Server" Text='<%#Eval("GetDebito") %>' />
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="Label5" runat="Server" Text='<%#Eval("GetCredito") %>' />
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lbSaldo" runat="Server" Text='<%#Eval("GetSaldo") %>' />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="Label1" runat="Server" Text='<%#Eval("GetComprobante") %>' />
                    </td>
                    <td onclick="Visible( -1 )">
                        <asp:LinkButton ID="btnImprimirCuota" runat="server" class="savePrint" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="Imprimir" Text="Imprimir recibo" ToolTip="Imprimir recibo" />
                    </td>
                </tr>                
            </ItemTemplate>
            <EmptyDataTemplate>
                <table id="Table1" width="100%" runat="server">
                    <tr>
                        <td align="center">No hay registros en su cuenta corriente</td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
</section>      
    
<CR:crystalreportviewer ID="CrystalReportViewer" runat="server" AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource" Width="894px" DisplayToolbar="False" Visible="false" />
<CR:crystalreportsource ID="CrystalReportSource" runat="server" Visible="false">
    <Report FileName="Reportes/CuentaCorriente.rpt"></Report>
</CR:crystalreportsource> 

<CR:CrystalReportSource ID="CrystalReportSourceRecibo" runat="server" Visible="false">
    <Report FileName="Reportes/Recibo.rpt"></Report>
</CR:CrystalReportSource>

<CR:CrystalReportSource ID="CrystalReportSourceCondonacion" runat="server" Visible="false">
    <Report FileName="Reportes/Condonacion.rpt"></Report>
</CR:CrystalReportSource>

<CR:CrystalReportSource ID="CrystalReportSourceNotaCredito" runat="server" Visible="false">
    <Report FileName="Reportes/NotaCredito.rpt"></Report>
</CR:CrystalReportSource>

<CR:CrystalReportSource ID="CrystalReportSourceNotaDebito" runat="server" Visible="false">
    <Report FileName="Reportes/NotaDebito.rpt"></Report>
</CR:CrystalReportSource>
</asp:Content>

