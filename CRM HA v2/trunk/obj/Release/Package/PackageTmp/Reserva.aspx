<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Reserva.aspx.cs" Inherits="Reserva" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var updateProgress = null;

    function postbackButtonClick() {
        updateProgress = $find("<%= UpdateProgress1.ClientID %>");
        window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
        return true;
    }
</script>

<script type="text/javascript">
    var updateProgress = null;

    function postbackButtonClick() {
        updateProgress = $find("<%= UpdateProgress2.ClientID %>");
        window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
        return true;
    }
</script>

<script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress3.ClientID %>");
        window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
        return true;
    }
</script>

<script src="js/jquery.mask.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('.decimal').mask("#.##0,00", { reverse: true });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>

<section>
    <div class="formHolder" id="searchBoxTop1">
        <div class="formHolderLine">
            <h2>Reserva de unidades</h2>
        </div>
    </div>
</section>

<asp:UpdatePanel ID="pnlReserva" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfIdEmpresa" runat="server" />
        <asp:HiddenField ID="hfIdItemCCU" runat="server" />

        <div id="maincol" style="height:600px; width: 100%;">
            <section>
                <div class="formHolder" style="padding: 22px 25px 12px;">
                    <div><modaltitle style="text-align:left">Tipo de operación</modaltitle></div><hr> 
                    <div style="float:left; width: 263px;">
                        <label style="width: 65%;">
                            <asp:DropDownList ID="cbOperacion" runat="server" CssClass="dropDownList dropDownListForm" style="width: 270px;" AutoPostBack="true" OnSelectedIndexChanged="cbOperacion_SelectedIndexChanged">
                                <asp:ListItem Value="Operacion">Seleccione una operación...</asp:ListItem>
                                <asp:ListItem Value="Nueva">Nueva Reserva</asp:ListItem>
                                <asp:ListItem Value="Cancelar">Cancelar Reserva</asp:ListItem>
                            </asp:DropDownList>
                        </label> 
                    </div>
                </div>
            </section>

            <asp:Panel ID="pnlNuevaReserva" runat="server" Visible="false">
                <section>
                    <div class="formHolder" style="padding: 22px 25px 12px;">
                        <div><modaltitle style="text-align:left">Cliente</modaltitle></div><hr> 
                        <div style="float:left; width: 263px;">
                            <label style="width: 65%;">
                                <asp:DropDownList ID="cbEmpresa" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="1" style="width: 270px;"></asp:DropDownList>
                                <br />
                                <asp:RequiredFieldValidator id="rfv1" runat="server" style="width: 256px;"
                                    ControlToValidate="cbEmpresa" InitialValue="0" ErrorMessage="Seleccione un cliente"
                                    Validationgroup="vgNuevaReserva" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                </asp:RequiredFieldValidator>
                            </label> 
                        </div>
                    </div>
                </section>
                       
                <asp:UpdatePanel runat="server">
                   <ContentTemplate>   
                        <section>
                            <div class="formHolder" style="padding: 22px 25px 12px;">
                                <div><modalTitle style="text-align:left">Unidad Funcional</modalTitle></div><hr />
                                <div class="divFormInLine">
                                    <div>
                                        <label>
                                            Obra:
                                            <asp:DropDownList ID="cbProyectos" runat="server" TabIndex="2" style="width: 270px; margin-top: 2px;" AutoPostBack="true" OnSelectedIndexChanged="cbProyectos_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:RequiredFieldValidator id="rfv2" runat="server" style="width: 256px;"
                                                ControlToValidate="cbProyectos" InitialValue="0" ErrorMessage="Seleccione una obra"
                                                Validationgroup="vgNuevaReserva" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                            </asp:RequiredFieldValidator>
                                        </label> 
                                    </div>
                                </div>
                                <div class="divFormInLine">
                                    <div>
                                        <label>
                                            Tipo de Unidad:
                                            <asp:DropDownList ID="cbUnidadFuncional" runat="server" CssClass="dropDownList" TabIndex="3" style="width: 270px; margin-top: 2px;" AutoPostBack="true" OnSelectedIndexChanged="cbUnidadFuncional_SelectedIndexChanged" Enabled="false">
                                                <asp:ListItem>Seleccione un tipo de unidad funcional...</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator id="rfv3" runat="server" style="width: 256px;"
                                                ControlToValidate="cbUnidadFuncional" InitialValue="0" ErrorMessage="Seleccione un tipo de unidad funcional"
                                                Validationgroup="vgNuevaReserva" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </div>
                                </div>                                            
                                <div class="divFormInLine">
                                    <div>
                                        <label>
                                            Nivel:
                                            <asp:DropDownList ID="cbNivel" runat="server" CssClass="dropDownList" TabIndex="4" style="width: 270px; margin-top: 2px;" AutoPostBack="true" OnSelectedIndexChanged="cbNivel_SelectedIndexChanged" Enabled="false">
                                                <asp:ListItem>Seleccione un nivel...</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator id="rfv4" runat="server" style="width: 256px;"
                                                ControlToValidate="cbNivel" InitialValue="0" ErrorMessage="Seleccione un nivel"
                                                Validationgroup="vgNuevaReserva" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                            </asp:RequiredFieldValidator>
                                        </label> 
                                    </div>                        
                                </div>
                                <div class="divFormInLine">
                                    <div>
                                         <label>
                                            Nro. de Unidad:
                                            <asp:DropDownList ID="cbUnidad" runat="server" CssClass="dropDownList" TabIndex="5" style="width: 270px; margin-top: 2px;" Enabled="false">
                                                <asp:ListItem>Seleccione una unidad...</asp:ListItem>
                                            </asp:DropDownList>
                                             <asp:RequiredFieldValidator id="rfv5" runat="server" style="width: 256px;"
                                                ControlToValidate="cbUnidad" InitialValue="0" ErrorMessage="Seleccione una unidad"
                                                Validationgroup="vgNuevaReserva" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </div>                           
                                </div>
                            </div>
                        </section>
                   
                        <section>
                            <div class="formHolderCalendar" style="padding: 12px 25px 30px;">
                                <div><modaltitle style="text-align:left">Importe</modaltitle></div><hr>
                                <div class="divFormInLine" style="width: 430px !important;">
                                   <div style="float:left">
                                        <label>
                                            <asp:TextBox ID="txtImporte" runat="server" CssClass="textBox textBoxForm decimal" TabIndex="6" Text="0" style="width: 256px;" onkeypress="return onKeyDecimal(event,this);"/>
                                            <asp:RequiredFieldValidator id="rfv100" runat="server"
                                                ControlToValidate="txtImporte"
                                                ErrorMessage="Campo obligatorio"
                                                Validationgroup="vgNuevaReserva" InitialValue="0"
                                                Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                style="width: 256px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                            </asp:RequiredFieldValidator>                                    
                                        </label>
                                    </div>
                                    <div style="float:right">
                                        <asp:RadioButtonList ID="rblMoneda" runat="server" RepeatDirection="Horizontal" TabIndex="7" style="box-shadow: inherit; margin-bottom: -8px !important;  margin-top: -4px;">
                                            <asp:ListItem Value="Pesos" Selected="True">Pesos</asp:ListItem>
                                            <asp:ListItem Value="Dolar">Dólar</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:Panel ID="pnlMensajeReservaOk" runat="server" Visible="false">
                    <section>
                        <div runat="server" class="formHolder messageOk" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                            <div>
                                <asp:Label ID="lbMensaje" runat="server" CssClass="messageOk" style="margin-left: 7px;">La reserva se ha registrado correctamente.</asp:Label>
                            </div>
                        </div>
                    </section>
                </asp:Panel>

                <asp:Panel ID="pnlMensajeReserva" runat="server" Visible="false">
                    <section>
                        <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px;">
                            <div>La unidada seleccionada ya se encuentra reservada.</div>
                        </div>
                    </section>
                </asp:Panel>

                <section>
                    <asp:UpdatePanel ID="pnlFinalizar" runat="server">
                        <ContentTemplate>
                            <div runat="server" class="formHolder" style="padding: 22px 14px 12px;">
                                <div runat="server">
                                    <div style="float:left">
                                        <label><asp:Button ID="btnFinalizar" Text="Finalizar" class="formBtnNar" runat="server" style="float:left; margin-top: -4px; margin-left: 15px;" Validationgroup="vgNuevaReserva" OnClick="btnFinalizar_Click"/></label>
                                    </div>                          
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </section>
            </asp:Panel>

            <asp:Panel ID="pnlCancelarReserva" runat="server" Visible="false">
                <section>
                    <div class="formHolder" style="padding: 22px 25px 12px;">
                        <div><modaltitle style="text-align:left">Obra</modaltitle></div><hr>
                        <div class="divFormInLine"> 
                            <div>
                                <label style="width: 65%;">
                                    <asp:DropDownList ID="cbObraReserva" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="1" style="width: 270px;"></asp:DropDownList>
                                    <br />
                                    <asp:RequiredFieldValidator id="rfv31" runat="server" style="width: 256px;"
                                        ControlToValidate="cbObraReserva" InitialValue="0" ErrorMessage="Seleccione una obra"
                                        Validationgroup="vgCancelarReserva" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                    </asp:RequiredFieldValidator>
                                </label> 
                            </div>
                        </div>
                        <div class="divFormInLine"> 
                            <div>
                                <asp:UpdatePanel ID="pnlBuscarReserva" runat="server">
                                    <ContentTemplate>
                                        <label><asp:Button ID="btnBuscarCancelar" Text="Buscar" class="formBtnNar" runat="server" style="float:left; margin-top: 0px; margin-left: 15px;" Validationgroup="vgCancelarReserva" OnClick="btnBuscarCancelar_Click"/></label> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </section>

                <asp:UpdatePanel runat="server" ID="plnListaReserva" Visible="false">
                    <ContentTemplate>
                        <section>                    
                            <div class="formHolder" style="padding: 22px 25px 12px;">
                                <div><modaltitle style="text-align:left">Lista de unidades reservas</modaltitle></div>
                                <hr>
                                <div align="left" class="h7" style="width: 94%; margin-top: 9px; margin-bottom: 9px;">Seleccione la/s unidad/es que desea cancelar la reserva</div>
                                <div class="divFormInLine" style="width:100%">
                                    <asp:ListView ID="lvReservados" runat="server">
                                        <LayoutTemplate>         
                                            <table style="width:100%">                            
                                                <thead id="tableHead">
                                                    <tr>     
                                                        <td style="width: 10px;"></td> 
                                                        <td style="width: 25px; text-align:center">CLIENTE</td>
                                                        <td style="width: 25px; text-align:center">NIVEL</td>
                                                        <td style="width: 10px; text-align:center">UNIDAD</td>
                                                        <td style="width: 10px; text-align:center">MONTO</td>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                                </tbody>                       
                                            </table>
                                        </LayoutTemplate>
                
                                        <ItemTemplate>                   
                                            <tr style="cursor: pointer">
                                                <td>
                                                    <asp:CheckBox ID="chBox" runat="server" />
                                                    <asp:Label ID="lbId" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                                </td>
                                                <td style="text-align:center">
                                                    <asp:Label ID="lbNombre" runat="Server" Text='<%# Eval("GetEmpresa") %>'/>
                                                </td>
                                                <td style="text-align:center">
                                                    <asp:Label ID="Label1" runat="Server" Text='<%# Eval("Nivel") %>'/>
                                                </td>
                                                <td style="text-align:center">
                                                    <asp:Label ID="Label2" runat="Server" Text='<%# Eval("NroUnidad") %>'/>
                                                </td>
                                                <td style="text-align:right">
                                                    <asp:Label ID="Label3" runat="Server" Text='<%#Eval("GetImporteReserva") %>'/>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                       
                                        <EmptyDataTemplate>
                                            <section>
                                                <table id="Table1" width="100%" runat="server">
                                                    <tr>
                                                        <td><b>No se unidades reservadas.<b/></td>
                                                    </tr>
                                                </table>
                                            </section>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </div>
                                <div>
                                    <div runat="server">
                                        <div runat="server">
                                            <div style="float:left">
                                                <label><asp:Button ID="btnSiguienteLista" Text="Siguiente" class="formBtnNar" runat="server" style="float:left;" OnClick="btnSiguienteLista_Click"/></label>
                                            </div>                          
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>     
                    </ContentTemplate>
                </asp:UpdatePanel>     
                
                <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
                    <section>
                        <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px;">
                            <div>Seleccione una unidad de la lista</div>
                        </div>
                    </section>
                </asp:Panel>    
                
                <section>
                    <asp:Panel ID="pnlDevolucion" runat="server" Visible="false">
                        <div class="formHolder" style="padding: 22px 25px 12px;">
                            <div><modaltitle style="text-align:left">Devolución de reserva</modaltitle></div><hr>
                            <div class="divFormInLine"> 
                                <div>
                                    <label>
                                        <asp:RadioButtonList ID="rblDevolucion" runat="server" RepeatDirection="Horizontal" OnTextChanged="rblDevolucion_TextChanged" TabIndex="7" AutoPostBack="true" style="box-shadow: inherit; margin-bottom: -36px !important;">
                                            <asp:ListItem Value="Si">Si</asp:ListItem>
                                            <asp:ListItem Value="No">No</asp:ListItem>
                                        </asp:RadioButtonList>                                    
                                    </label> 
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </section>    

                <section>
                    <asp:UpdatePanel ID="pnlFinalizarCancelarReserva" runat="server" Visible="false">
                        <ContentTemplate>
                            <div runat="server" class="formHolder" style="padding: 22px 14px 12px;">
                                <div runat="server">
                                    <div style="float:left">
                                        <label><asp:Button ID="btnFinalizarCancelarReserva" Text="Finalizar" class="formBtnNar" runat="server" style="float:left; margin-top: -4px; margin-left: 15px;" Validationgroup="CustomerOV" OnClick="btnFinalizarCancelarReserva_Click"/></label>
                                    </div>                          
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </section>
            </asp:Panel>
        </div>            
    </ContentTemplate>
</asp:UpdatePanel>   
    
<div style="float:left">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlFinalizar">
        <ProgressTemplate>
            <div class="overlay">
                <div class="overlayContent">
                    <div style="float:left;"><img src="images/ring_loading.gif" width="300px" class="loading100" ImageAlign="left"  /></div>
                    <div style="float:left; padding: 8px 0 0 10px">
                        <h2> Procesando... </h2>
                    </div>                                    
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>

<div style="float:left">
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlFinalizarCancelarReserva">
        <ProgressTemplate>
            <div class="overlay">
                <div class="overlayContent">
                    <div style="float:left;"><img src="images/ring_loading.gif" width="300px" class="loading100" ImageAlign="left"  /></div>
                    <div style="float:left; padding: 8px 0 0 10px">
                        <h2> Procesando... </h2>
                    </div>                                    
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>

<div style="float:left">
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlBuscarReserva">
        <ProgressTemplate>
            <div class="overlay">
                <div class="overlayContent">
                    <div style="float:left;"><img src="images/ring_loading.gif" width="300px" class="loading100" ImageAlign="left"  /></div>
                    <div style="float:left; padding: 8px 0 0 10px">
                        <h2> Buscando... </h2>
                    </div>                                    
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>

<CR:CrystalReportSource ID="CrystalReportSource" runat="server" Visible="false">
    <Report FileName="Reportes/Recibo.rpt"></Report>
</CR:CrystalReportSource>
</asp:Content>

