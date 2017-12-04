<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="OperacionVenta.aspx.cs" Inherits="OperacionVenta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Controles/CuotasManuales.ascx" TagPrefix="crm" TagName="CuotasManuales" %>

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
    
<script src="js/jquery.mask.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('.decimal').mask("#.##0,00", { reverse: true });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>

<asp:Label ID="lbMonedaUnidad" runat="server" Visible="false"></asp:Label>

<asp:UpdatePanel ID="pnlOperacionVenta" runat="server">
    <ContentTemplate>
        <div id="maincol" style="height:600px; width: 100%;"> 
               
            <section>
                <h2>Operación de venta</h2>
                <div class="formHolder" style="padding: 22px 25px 12px;">
                    <div><modaltitle style="text-align:left">Cliente</modaltitle></div><hr> 
                    <div style="float:left; width: 263px;">
                        <label style="width: 65%;">
                            <asp:DropDownList ID="cbEmpresa" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="1" style="width: 270px;" AutoPostBack="true" OnSelectedIndexChanged="cbEmpresa_SelectedIndexChanged"></asp:DropDownList>
                            <br />
                            <asp:RequiredFieldValidator id="rfv1" runat="server" style="width: 256px;"
                                ControlToValidate="cbEmpresa" InitialValue="0" ErrorMessage="Seleccione un cliente"
                                Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                            </asp:RequiredFieldValidator>
                        </label> 
                    </div>
                </div>
            </section>
            
            <asp:Panel ID="pnlReserva" runat="server" Visible="false">
                <section>
                    <div runat="server" class="formHolderAlert reserva" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                        <div>
                            <b><asp:Label ID="lbReserva" runat="server" style="margin-left: 7px;" Text="El cliente seleccionado tiene una reserva. Importe:"></asp:Label></b>
                            &nbsp;
                            <b><asp:Label ID="lbImporteReserva" runat="server" style="margin-left: -6px;"></asp:Label></b>
                        </div>
                    </div>
                </section>
            </asp:Panel>

            <asp:Panel ID="pnlMensajeUF" runat="server" Visible="false">
                <section>
                    <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                        <div>
                            <b><asp:Label ID="lbMensajeUF" runat="server" CssClass="messageError" style="margin-left: 7px;" Text="No se puede ingresar otra unidad funcional del tipo departamento/casa"></asp:Label></b>
                        </div>
                    </div>
                </section>
            </asp:Panel>

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
                                            Validationgroup="CustomerUnidadOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
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
                                            Validationgroup="CustomerUnidadOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
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
                                            Validationgroup="CustomerUnidadOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                        </asp:RequiredFieldValidator>
                                    </label> 
                                </div>                        
                            </div>
                            <div class="divFormInLine">
                                <div>
                                     <label>
                                        Nro. de Unidad:
                                        <asp:DropDownList ID="cbUnidad" runat="server" CssClass="dropDownList" TabIndex="5" style="width: 270px; margin-top: 2px;" AutoPostBack="true" OnSelectedIndexChanged="cbUnidad_SelectedIndexChanged" Enabled="false">
                                            <asp:ListItem>Seleccione una unidad...</asp:ListItem>
                                        </asp:DropDownList>
                                         <asp:RequiredFieldValidator id="rfv5" runat="server" style="width: 256px;"
                                            ControlToValidate="cbUnidad" InitialValue="0" ErrorMessage="Seleccione una unidad"
                                            Validationgroup="CustomerUnidadOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </div>                           
                            </div>
                        </div>

                        <div class="formHolder"  style="padding: 22px 25px 12px;">
                            <div class="divFormInLine">
                               <div>
                                    <label>                                
                                        <asp:HiddenField ID="hfCodUfUnidad" runat="server" />
                                        Precio base (Moneda):<br />
                                        <b style="font-size: medium;">
                                            <asp:Label ID="lbPrecio" runat="server" Text="-" CssClass="labelItem" style="width:auto"/>&nbsp; 
                                            <asp:Label ID="lbMoneda" runat="server" CssClass="labelItem" style="padding-left: 16px !important;"/>
                                        </b>
                                    </label>
                                </div>
                            </div>
                            <div class="divFormInLine">
                               <div>
                                    <label>
                                        Precio acordado:
                                        <asp:TextBox ID="txtPrecioAcordado" runat="server" CssClass="textBox textBoxForm decimal" TabIndex="6" style="width: 256px; margin-top: 2px;" Text="0" onkeypress="return onKeyDecimal(event,this);"/>
                                    </label>
                                </div>
                            </div>
                            <div class="divFormInLine">
                                <div>
                                    <label style="margin-top: 35px;"><asp:Button ID="btnAgregarUF" Text="Agregar unidad" CssClass="formBtnNar" runat="server" style="float:left; margin-top: -4px;" Validationgroup="CustomerUnidadOV" OnClick="btnAgregarUF_Click"/></label>
                                </div>
                            </div>
                        </div>
                                                 
                        <div class="formHolder" style="padding: 22px 25px 12px;">
                            <div>
                            <asp:ListView ID="lvUnidades" runat="server" OnItemCommand="lvUnidades_ItemCommand">
                                <LayoutTemplate>
                                    <section>            
                                        <table style="width:100%">                            
                                            <thead id="tableHead">
                                                <tr>                            
                                                    <td style="width:25px">OBRA</td>   
                                                    <td style="width:20px">TIPO UNIDAD</td>
                                                    <td style="width:20px">NIVEL</td>
                                                    <td style="width:15px">NRO. UNIDAD</td>
                                                    <td style="width:5px">PRECIO BASE</td>
                                                    <td style="width:5px">PRECIO ACORDADO</td>
                                                    <td style="width:2px"></td>
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
                                        <td align="left">
                                            <asp:Label ID="lbId" runat="Server" Text='<%#Eval("Id") %>' Visible="false" />
                                            <asp:Label ID="lbProyecto" runat="Server" Text='<%#Eval("GetProyecto") %>' />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("UnidadFuncional") %>' />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="Label1" runat="Server" Text='<%#Eval("Nivel") %>' />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="Label2" runat="Server" Text='<%#Eval("NroUnidad") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="Server" Text='<%#Eval("GetPrecioBase") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbPrecioAcordado" runat="Server" Text='<%#Eval("GetPrecioAcordado") %>' />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkDelete" CssClass="deleteBtn" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("id") %>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                            </div>
                            <hr class="line">
                            <div class="divFormInLine">
                                <div>
                                <label style="width:600px">
                                    <asp:Label ID="lbPrecioListaTitle" runat="server" Text="Precio de lista (Dolar): " style="background-color: transparent !important; border: inherit !important; width:auto; padding-left: 8px; font-weight: bold; font-size: 16px;" />
                                    <asp:Label ID="lbPrecioLista" runat="server" Text="0" CssClass="labelItem" style="font-size: 16px !important;"/>
                                </label>
                                </div>
                            </div>
                            <div class="divFormInLine">
                                <div>
                                <label style="width:600px">
                                    <asp:Label ID="lbPrecioListaTitlePesos" runat="server" Text="Precio de lista (Pesos): " style="background-color: transparent !important; border: inherit !important; width:auto; padding-left: 8px; font-weight: bold; font-size: 16px;" />
                                    <asp:Label ID="lbPrecioListaPesos" runat="server" Text="0" CssClass="labelItem" style="font-size: 16px !important;"/>
                                </label>
                                </div>
                            </div>
                            <div class="divFormInLine">
                                <div>
                                    <label style="width:600px">
                                        <asp:Label ID="lbPrecioAcordadoTitle" runat="server" Text="Precio acordado: " style="background-color: transparent !important; border: inherit !important; width:auto; padding-left: 8px; font-weight: bold; font-size: 16px;" />
                                        <asp:Label ID="lbPrecioAcordado" runat="server" Text="0" CssClass="labelItem" style="font-size: 16px !important;" />
                                    </label>
                                </div>
                            </div>
                        </div>
                    </section>
                   
                    <section>                        
                        <div class="formHolderCalendar" style="padding: 22px 25px 12px;">
                            <div><modalTitle style="text-align:left">Fechas</modalTitle></div><hr />
                            <div class="divFormInLine">
                                <div>
                                    <label>
                                        Fecha del boleto:
                                        <asp:TextBox ID="txtFechaOV" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 256px;"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalendarExtender8" runat="server" CssClass="orange" TargetControlID="txtFechaOV" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                        <asp:RequiredFieldValidator id="rfv100" runat="server"
                                            ControlToValidate="txtFechaOV"
                                            ErrorMessage="Campo obligatorio"
                                            Validationgroup="CustomerOV"
                                            Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                            style="width: 256px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                        </asp:RequiredFieldValidator>    
                                    </label> 
                                </div>
                            </div>
                            <div class="divFormInLine">
                                <div>
                                    <label>
                                        Fecha de posesión:
                                        <asp:TextBox ID="txtFechaPosesion" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 256px;"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalendarExtender9" runat="server" CssClass="orange" TargetControlID="txtFechaPosesion" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                    </label>
                                </div>
                            </div>                                            
                            <div class="divFormInLine">
                                <div>
                                    <label>
                                       Fecha de escritura:
                                       <asp:TextBox ID="txtFechaEscritura" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 256px;"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalendarExtender10" runat="server" CssClass="orange" TargetControlID="txtFechaEscritura" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                    </label> 
                                </div>                        
                            </div>
                        </div>
                    </section>     
                        
                    <section>
                        <div class="formHolder" style="padding: 12px 25px 12px;">
                            <div><modaltitle style="text-align:left">Vendedor</modaltitle></div><hr>
                            <div class="divFormInLine">
                               <div>
                                    <label>
                                        <asp:DropDownList ID="cbVendedor" runat="server" CssClass="dropDownList" TabIndex="8" style="width: 270px; margin-top: 2px;" />
                                        <asp:RequiredFieldValidator id="rfv22" runat="server" style="width: 256px;"
                                            ControlToValidate="cbVendedor" InitialValue="Seleccione un vendedor..." ErrorMessage="Seleccione un vendedor"
                                            Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </section>

                    <section>
                       <div class="formHolder" style="padding: 22px 25px 12px;">
                            <div><modaltitle style="text-align:left">Condiciones de venta</modaltitle></div><hr>
                            <div class="divFormInLine">
                               <div>
                                    <label>
                                        Moneda acordada:
                                        <asp:DropDownList ID="cbMonedaAcordada" runat="server" CssClass="dropDownList" TabIndex="9" style="width: 270px; margin-top: 2px;" AutoPostBack="true" OnTextChanged="cbMonedaAcordada_TextChanged" />
                                        <asp:RequiredFieldValidator id="rfv20" runat="server" style="width: 256px;"
                                            ControlToValidate="cbMonedaAcordada" InitialValue="Seleccione una moneda..." ErrorMessage="Seleccione una moneda"
                                            Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </div>
                            </div> 
                                                                                 
                            <div class="divFormInLine" style="width: 177px !important;">
                                <label style="width: 200px;">
                                    Dólar (Valor actual)
                                    <asp:Label ID="lbValorActualDolar" runat="server" Text="-" CssClass="labelItem" style="color: green; width: 146px;" />
                                </label>
                            </div>

                            <div class="divFormInLine" style="width: 152px !important;">
                                <label style="width:166px">
                                    Dólar (Valor acordado)
                                    <asp:TextBox ID="txtDolar" runat="server" CssClass="textBox textBoxForm" TabIndex="11" Text="0" style="width: 116px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                </label>
                            </div>

                            <div class="divFormInLine" style="width: 180px !important;">
                                <label style="width: 154px;">
                                    Gastos administrativos (%):
                                    <asp:TextBox ID="txtComision" runat="server" CssClass="textBox textBoxForm decimal" TabIndex="12" Text="0" style="width: 145px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                </label>
                            </div>

                            <div class="divFormInLine" style="width: 6px !important;">
                                <label>
                                    IVA
                                    <asp:CheckBox ID="chbIva" runat="server" Checked="True" TabIndex="13"/>
                                </label>
                            </div>                              
                        </div>

                        <div class="formHolderCalendar" style="padding: 34px 25px 12px; line-height: inherit !important;">
                            <div><modaltitle style="text-align:left; font-size: 16px !important;">Índices</modaltitle></div><hr class="line">
                                                        
                            <div class="divFormInLine" style="width: 500px;">
                                <div style="float:left; margin-top: 8px; width: 100%;">
                                    <div style="float:left;">Seleccione el tipo de índice que desea agregar:</div>
                                    <div style="float:right;">
                                        <asp:RadioButtonList ID="rblIndice" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"  TabIndex="7" TextAlign="Left" AutoPostBack="true" OnTextChanged="rblIndice_TextChanged" style="box-shadow: inherit; margin-bottom: -8px !important;  margin-top: -13px;">
                                            <asp:ListItem Value="CAC">CAC</asp:ListItem>
                                            <asp:ListItem Value="UVA">UVA</asp:ListItem>
                                            <asp:ListItem Value="Ninguno">Ninguno</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>                           
                            
                            <div class="divFormInLine" style="margin-top: 6px; width:100%">                                
                                <asp:Panel ID="pnlComboCAC" runat="server" Visible="false">
                                    <hr class="line" style="border-top: 0px solid #ccc !important;">
                                    <label>
                                        Índice CAC:
                                        <asp:DropDownList ID="cbComboCac" runat="server" TabIndex="10" CssClass="dropDownList dropDownListForm" style="width: 270px; margin-top: 2px;" />
                                        <asp:RequiredFieldValidator id="rfvComboCAC" runat="server" style="width: 256px;"
                                            ControlToValidate="cbComboCac" InitialValue="0" ErrorMessage="Seleccione el índice base"
                                            Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator id="RequiredFieldValidator10" runat="server" style="width: 256px;"
                                            ControlToValidate="cbComboCac" InitialValue="Seleccione el índice base..." ErrorMessage="Seleccione el índice base"
                                            Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                        </asp:RequiredFieldValidator>
                                    </label> 
                                </asp:Panel>
                                
                                <asp:Panel ID="pnlComboUVA" runat="server" Visible="false">
                                    <hr class="line" style="border-top: 0px solid #ccc !important;">
                                    <label>
                                        Índice UVA:
                                        <asp:TextBox ID="txtUVA" runat="server" CssClass="textBox textBoxForm decimal" Text="0" style="width: 256px; margin-top: 2px;"/>
                                        <asp:RequiredFieldValidator id="rfv26" runat="server"
                                            ControlToValidate="txtUVA" ErrorMessage="Agregue el monto"  InitialValue="0"
                                            Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                            style="width: 256px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                        </asp:RequiredFieldValidator>
                                    </label> 
                                </asp:Panel>
                            </div>
                        </div>
                   </section>

                    <section>
                        <div class="formHolderCalendar" style="padding: 12px 25px 12px;">
                            <div><modaltitle style="text-align:left">Forma de pago</modaltitle></div><hr>
                            <div class="divFormInLine">
                               <div>
                                    <label>
                                        Tipo de pago:
                                        <asp:DropDownList ID="cbTipoPago" runat="server" CssClass="dropDownList" TabIndex="14" style="width: 270px; margin-top: 2px;" AutoPostBack="true" OnTextChanged="cbTipoPago_TextChanged" >
                                            <asp:ListItem Value="0">Seleccione un tipo de pago</asp:ListItem>
                                            <asp:ListItem Value="1">Contado</asp:ListItem>
                                            <asp:ListItem Value="2">Financiado</asp:ListItem>
                                        </asp:DropDownList>
                                    </label>
                                </div>
                            </div> 
                        </div>         
                       
                        <asp:Panel ID="pnlCuotas" runat="server" Visible="false">
                        <div class="formHolder" style="padding: 34px 25px 12px;">
                            <div><modaltitle style="text-align:left; font-size: 16px !important;">Cuotas</modaltitle></div><hr class="line">
                            <div class="divFormInLine" style="float:left;">
                                <label>
                                    Tipo de moneda:
                                    <asp:DropDownList ID="cbOperacionMoneda" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="15" AutoPostBack="true" OnSelectedIndexChanged="cbOperacionMoneda_SelectedIndexChanged" style="width: 270px; margin-top: 2px;">
                                        <asp:ListItem Value="-1">Seleccione la moneda...</asp:ListItem>
                                            <asp:ListItem Value="0">Moneda acordada</asp:ListItem>
                                        <asp:ListItem Value="2">Dolar / Pesos</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator id="rfv21" runat="server" style="width: 256px;" Enabled="false"
                                        ControlToValidate="cbOperacionMoneda" InitialValue="-1" ErrorMessage="Seleccione una moneda para la forma de pago"
                                        Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                    </asp:RequiredFieldValidator>
                                </label> 
                            </div>
                        </div>
                        </asp:Panel>
                                                
                        <asp:UpdatePanel runat="server" ID="pnlMonedaAcordada" Visible="false">
                            <ContentTemplate>
                                <div>
                                    <div class="formHolderCalendar" style="padding-top: 2px;">
                                        <div style="margin-top: 2%;">
                                            <div style="height:auto;">
                                                <div class="divFormInLine" style="width: 288px !important;">
                                                    <div>
                                                        <label>
                                                            Monto:
                                                            <asp:TextBox ID="txtMontoMonedaAcordada" runat="server" CssClass="textBox textBoxForm decimal" Text="0" style="width: 256px; margin-top: 2px;" AutoPostBack="true" OnTextChanged="txtMontoMonedaAcordada_TextChanged"/>
                                                            <asp:HiddenField ID="hfMontoMonedaAcordada" runat="server" />
                                                            <asp:RequiredFieldValidator id="rfv6" runat="server"
                                                                ControlToValidate="txtMontoMonedaAcordada" ErrorMessage="Agregue el monto"  InitialValue="0"
                                                                Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                style="width: 256px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                            </asp:RequiredFieldValidator>
                                                        </label>
                                                    </div>
                                                </div>
                                                <asp:Panel ID="convertirMonedaAcordada" runat="server" Visible="false">
                                                    <div class="divFormInLine" style="width: 182px !important;">
                                                        <div style="width: 180px;">
                                                            <label>
                                                                Convertir <font style="font-style: italic; font-size: 13px;">(Moneda acordada)</font><br />
                                                                <asp:CheckBox ID="chbConvertirMonedaAcordada" runat="server" TabIndex="15" AutoPostBack="True" OnCheckedChanged="chbConvertirMonedaAcordada_CheckedChanged"/>
                                                                <b><asp:Label ID="lbConvertirMonedaAcordada" runat="server" Text=",00" CssClass="labelItem"/></b>
                                                            </label> 
                                                        </div>                        
                                                    </div>
                                                </asp:Panel>
                                                <div class="divFormInLine" style="width: 86px !important;">
                                                    <div style="width:40px">
                                                        <label>
                                                            Cuotas:
                                                            <asp:TextBox ID="txtCuotasMonedaAcordada" runat="server" AutoPostBack="true" OnTextChanged="txtCuotasMonedaAcordada_TextChanged" CssClass="textBox textBoxForm decimal" Text="0" TabIndex="21" style="width: 38px; margin-top: 2px;"/>
                                                            <asp:RequiredFieldValidator id="rfv14" runat="server"
                                                                ControlToValidate="txtCuotasMonedaAcordada" ErrorMessage="Agregue la cantidad de cuotas" InitialValue="0"
                                                                Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                style="width: 188px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                            </asp:RequiredFieldValidator>
                                                        </label> 
                                                    </div>
                                                </div>
                                                <div class="divFormInLine" style="width: 140px !important;">
                                                    <div style="width: 50%;">
                                                        <label>
                                                            Valor Cuota:
                                                            <div style="padding-top: 5px;"><b><asp:Label ID="lbValorMonedaAcordada" runat="server" Text=",00" CssClass="labelItem" style="padding-left: 15px !Important;"/></b></div>
                                                        </label> 
                                                    </div>
                                                </div> 
                                                <asp:Panel ID="pnlFechaVencAcordada" runat="server">
                                                    <div class="divFormInLine" style="width: 168px !important;">
                                                        <div style="width: 155px;">
                                                            <label>
                                                                Fecha de Vencimiento:
                                                                <asp:TextBox ID="txtFechaVencimientoMonedaAcordada" runat="server" CssClass="textBox textBoxForm" TabIndex="22" style="width: 116px;"></asp:TextBox>
                                                                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="orange" TargetControlID="txtFechaVencimientoMonedaAcordada" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                                                <asp:RequiredFieldValidator id="rfv7" runat="server"
                                                                    ControlToValidate="txtFechaVencimientoMonedaAcordada"
                                                                    ErrorMessage="Ingrese una fecha"
                                                                    Validationgroup="CustomerOV"
                                                                    Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                    style="width: 116px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                </asp:RequiredFieldValidator>
                                                            </label> 
                                                        </div>                        
                                                    </div>     
                                                </asp:Panel>                             
                                                <div class="divFormInLine" style="width: 80px !important;">
                                                    <div style="width: 60px;">
                                                        <label>
                                                            Refuerzos
                                                            <asp:CheckBox ID="chbCuotasManuales" runat="server" TabIndex="23" AutoPostBack="True" OnCheckedChanged="chbCuotasManuales_CheckedChanged"  />
                                                        </label> 
                                                    </div>                        
                                                </div>
                                                <div class="divFormInLine" style="width: 100px !important;">
                                                    <div style="width: 100px;">
                                                        <label>
                                                            Gastos adtvo.
                                                            <asp:CheckBox ID="chbGastosManuales" runat="server" TabIndex="24" Checked="true"/>
                                                        </label> 
                                                    </div>                        
                                                </div>
                                                <div class="divFormInLine" style="width: 7px !important;">
                                                    <div style="width: 100px;">
                                                        <label>
                                                            Interés anual (%)
                                                            <asp:TextBox ID="txtInteresAnual" runat="server" CssClass="textBox textBoxForm" TabIndex="25" Text="0" style="width: 38px; margin-top: 2px;"></asp:TextBox>
                                                        </label> 
                                                    </div>                        
                                                </div>
                                            </div>

                                            <asp:Panel ID="pnlRangoAcordada" runat="server">
                                                <div class="divFormInLine" style="width:100%; margin-top: 16px; margin-bottom: 1%;">
                                                    <div style="width:140px">
                                                        <label>
                                                            Rango cuotas ajustables:<br />
                                                            De&nbsp;<asp:TextBox ID="txtRangoDesdeAcordado" runat="server" CssClass="textBox textBoxForm" Text="1" TabIndex="26" style="width: 15px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                                            a&nbsp;<asp:TextBox ID="txtRangoAAcordado" runat="server" CssClass="textBox textBoxForm" Text="1" TabIndex="27" style="width: 15px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                                        </label> 
                                                    </div>
                                                </div>
                                            </asp:Panel>

                                            <asp:UpdatePanel runat="server" ID="upanel"> 
                                                <ContentTemplate>                                        
                                                    <div>
                                                        <asp:Panel ID="pnlCuotasManualesMonedaAcordada1" runat="server" Visible="false">
                                                            <div>
                                                                <asp:ListView ID="lvFormaPago" runat="server">
                                                                    <LayoutTemplate>
                                                                        <table style="margin-bottom: 0px; width: 84%">
                                                                            <thead>
                                                                                <tr>
                                                                                    <td style="width: 28%;">TOTAL</td>
                                                                                    <td style="width: 29%;">CUOTAS</td>
                                                                                    <td style="width: 20%;">VALOR</td>
                                                                                    <td style="width: 25%;">FECHA VENCIMIENTO</td>
                                                                                    <td style="width: 25%;">GASTOS ADTVO.</td>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                                            </tbody>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id='row<%# Eval("id")%>' style="color:#b40b0b;">
                                                                            <td><asp:TextBox ID="lbTotal" runat="Server" Text='<%#Eval("GetAnticipo") %>'></asp:TextBox></td>
                                                                            <td style="padding-left: 20px;"><asp:Label ID="lbCantCuotas" runat="Server" Text='1' /></td>
                                                                            <td style="padding-left: 35px;"><asp:Label ID="lbMontoCuota" runat="Server" Text='-' /></td>
                                                                            <td>
                                                                                <asp:TextBox ID="lbFechaVenc" runat="server" CssClass="textBox textBoxForm" style="width: 116px; margin-top: 2px;"></asp:TextBox>
                                                                                <ajax:CalendarExtender ID="CalendarExtender7" runat="server" CssClass="orange" TargetControlID="lbFechaVenc" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                                                                <%--<asp:RequiredFieldValidator id="RequiredFieldValidator12" runat="server"
                                                                                    ControlToValidate="lbFechaVenc"
                                                                                    ErrorMessage="Ingrese una fecha"
                                                                                    Validationgroup="CustomerOV"
                                                                                    Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                                    style="width: 280px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                                </asp:RequiredFieldValidator>--%>
                                                                            </td>
                                                                            <td>
	                                                                            <asp:CheckBox ID="chbGastosAdtvo" runat="server" Checked="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:ListView>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>   
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                            <asp:UpdatePanel runat="server" ID="upanelCAC"> 
                                                <ContentTemplate>                                        
                                                    <div>
                                                        <asp:Panel ID="pnlCuotasManualesMonedaAcordada1CAC" runat="server" Visible="false">
                                                            <div>
                                                                <asp:ListView ID="lvFormaPagoCAC" runat="server">
                                                                    <LayoutTemplate>
                                                                        <table style="margin-bottom: 0px; width: 84%">
                                                                            <thead>
                                                                                <tr>
                                                                                    <td style="width: 20%;">TOTAL</td>
                                                                                    <td style="width: 20%;">CUOTAS</td>
                                                                                    <td style="width: 20%;">VALOR</td>
                                                                                    <td style="width: 20%;">FECHA VENCIMIENTO</td>
                                                                                    <td style="width: 7%;">AJUSTE CAC</td>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                                            </tbody>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id='row<%# Eval("id")%>' style="color:#b40b0b;">
                                                                            <td><asp:TextBox ID="lbTotal" runat="Server" Text='<%#Eval("GetAnticipo") %>'></asp:TextBox></td>
                                                                            <td style="padding-left: 20px;"><asp:Label ID="lbCantCuotas" runat="Server" Text='1' /></td>
                                                                            <td style="padding-left: 35px;"><asp:Label ID="lbMontoCuota" runat="Server" Text='-' /></td>
                                                                            <td>
                                                                                <asp:TextBox ID="lbFechaVenc" runat="server" CssClass="textBox textBoxForm" style="width: 116px; margin-top: 2px;"></asp:TextBox>
                                                                                <ajax:CalendarExtender ID="CalendarExtender7" runat="server" CssClass="orange" TargetControlID="lbFechaVenc" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chbAjusteCAC" runat="server" Checked="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:ListView>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>   
                                                </ContentTemplate>
                                            </asp:UpdatePanel>    
                                                                                    
                                            <div class="divFormInLine" style="width:100%; margin-top: 2%; margin-bottom: 2%;">
                                                <div style="float:left">
                                                    <label style="width:100%; margin-right: 6px;">
                                                        <font style="font-style: italic; font-size: 14px;">¿Desea agregar más cuotas con otra condición de pago?</font>
                                                    </label>
                                                </div>
                                                <div align="left" style="margin-right: 40px;"><asp:CheckBox ID="chbCuotas" runat="server" TabIndex="28" AutoPostBack="True" OnCheckedChanged="chbCuotas_CheckedChanged" /></div>
                                            </div>

                                            <asp:Panel ID="pnlCuota2" runat="server" Visible="false" style="margin-top: 2px; padding-bottom: 11%;">
                                                <div>
                                                    <div>
                                                        <div class="divFormInLine" style=" width: 288px !important;">
                                                            <div>
                                                                <label>
                                                                    Monto: 
                                                                    <asp:TextBox ID="txtMontoMonedaAcordada2" runat="server" CssClass="textBox textBoxForm decimal" Text="0" TabIndex="29" style="width: 256px; margin-top: 2px;" AutoPostBack="true" OnTextChanged="txtMontoMonedaAcordada2_TextChanged"/>
                                                                    <asp:HiddenField ID="hfMontoMonedaAcordada2" runat="server" />
                                                                    <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server"
                                                                        ControlToValidate="txtMontoMonedaAcordada2" ErrorMessage="Agregue el monto"  InitialValue="0"
                                                                        Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                        style="width: 256px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                    </asp:RequiredFieldValidator>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <asp:Panel ID="convertirMonedaAcordada2" runat="server" Visible="false">
                                                        <div class="divFormInLine" style="width: 182px !important;">
                                                            <div style="width: 180px;">
                                                                <label>
                                                                    Convertir <font style="font-style: italic; font-size: 13px;">(Moneda acordada)</font><br />
                                                                    <asp:CheckBox ID="chbConvertirMonedaAcordada2" runat="server" TabIndex="30" AutoPostBack="True" OnCheckedChanged="chbConvertirMonedaAcordada2_CheckedChanged" />
                                                                    <b><asp:Label ID="lbConvertirMonedaAcordada2" runat="server" Text=",00" CssClass="labelItem"/></b>
                                                                </label> 
                                                            </div>                        
                                                        </div>
                                                        </asp:Panel>
                                                        <div class="divFormInLine" style="width: 86px !important;">
                                                            <div style="width:40px">
                                                                <label>
                                                                    Cuotas:
                                                                    <asp:TextBox ID="txtCuotasMonedaAcordada2" TabIndex="31" AutoPostBack="true" OnTextChanged="txtCuotasMonedaAcordada2_TextChanged" runat="server" CssClass="textBox textBoxForm decimal" Text="0" style="width: 38px; margin-top: 2px;"/>
                                                                    <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server"
                                                                        ControlToValidate="txtCuotasMonedaAcordada2" ErrorMessage="Ingrese una cantidad" InitialValue="0"
                                                                        Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                        style="width: 188px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                    </asp:RequiredFieldValidator>
                                                                </label> 
                                                            </div>
                                                        </div>                                        
                                                        <div class="divFormInLine" style="width: 140px !important;">
                                                            <div style="width: 50%;">
                                                                <label>
                                                                    Valor Cuota:
                                                                    <div style="padding-top: 5px;"><b><asp:Label ID="lbValorMonedaAcordada2" runat="server" Text=",00" CssClass="labelItem" style="padding-left: 15px !Important;"/></b></div>
                                                                </label> 
                                                            </div>
                                                        </div> 
                                                        <asp:Panel ID="pnlFechaVencAcordada2" runat="server">
                                                        <div class="divFormInLine" style="width: 168px !important;">
                                                            <div>
                                                                <label>
                                                                    Fecha de Vencimiento:
                                                                    <asp:TextBox ID="txtFechaVencimientoMonedaAcordada2" runat="server" TabIndex="32" CssClass="textBox textBoxForm" style="margin-top: 2px; width:116px"></asp:TextBox>
                                                                    <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="orange" TargetControlID="txtFechaVencimientoMonedaAcordada2" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                                                    <asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server"
                                                                        ControlToValidate="txtFechaVencimientoMonedaAcordada2"
                                                                        ErrorMessage="Ingrese una fecha"
                                                                        Validationgroup="CustomerOV"
                                                                        Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                        style="width: 116px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                    </asp:RequiredFieldValidator>
                                                                </label> 
                                                            </div>                        
                                                        </div>
                                                        </asp:Panel>
                                                        <div class="divFormInLine" style="width: 80px !important;">
                                                            <div style="width: 60px;">
                                                                <label>
                                                                    Refuerzos
                                                                    <asp:CheckBox ID="chbCuotasManuales2" runat="server" TabIndex="33" AutoPostBack="True" OnCheckedChanged="chbCuotasManuales2_CheckedChanged"  />
                                                                </label> 
                                                            </div>                        
                                                        </div>
                                                        <div class="divFormInLine" style="width: 100px !important;">
                                                            <div style="width: 100px;">
                                                                <label>
                                                                    Gastos adtvo.
                                                                    <asp:CheckBox ID="chbGastosManuales2" runat="server" TabIndex="34" Checked="true"/>
                                                                </label> 
                                                            </div>                        
                                                        </div>  
                                                        <div class="divFormInLine" style="width: 7px !important;">
                                                            <div style="width: 100px;">
                                                                <label>
                                                                    Interés anual (%)
                                                                    <asp:TextBox ID="txtInteresAnual2" runat="server" CssClass="textBox textBoxForm" TabIndex="35" Text="0" style="width: 38px; margin-top: 2px;"></asp:TextBox>
                                                                </label> 
                                                            </div>                        
                                                        </div>  
                                                    </div>
                                                    <asp:Panel ID="pnlRangoAcordada2" runat="server">
                                                    <div class="divFormInLine" style="width:100%; margin-top: 16px; margin-bottom: 1%;">
                                                        <div style="width:140px">
                                                            <label>
                                                                Rango cuotas ajustables:<br />
                                                                De&nbsp;<asp:TextBox ID="txtRangoDesdeAcordado2" runat="server" CssClass="textBox textBoxForm" Text="1" TabIndex="36" style="width: 15px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                                                a&nbsp;<asp:TextBox ID="txtRangoAAcordado2" runat="server" CssClass="textBox textBoxForm" Text="1" TabIndex="36" style="width: 15px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                                            </label> 
                                                        </div>
                                                    </div>
                                                    </asp:Panel>

                                                    <asp:UpdatePanel runat="server" ID="upanel2"> 
                                                        <ContentTemplate>                                        
                                                            <div>
                                                                <asp:Panel ID="pnlCuotasManualesMonedaAcordada2" runat="server" Visible="false">
                                                                    <div>
                                                                        <asp:ListView ID="lvFormaPago2" runat="server">
                                                                            <LayoutTemplate>
                                                                                <table style="margin-bottom: 0px; width: 84%">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <td style="width: 28%;">TOTAL</td>
                                                                                            <td style="width: 29%;">CUOTAS</td>
                                                                                            <td style="width: 25%;">FECHA VENCIMIENTO</td>
                                                                                            <td style="width: 25%;">GASTOS ADTVO.</td>
                                                                                        </tr>
                                                                                    </thead>
                                                                                    <tbody>
                                                                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                                                    </tbody>
                                                                                </table>
                                                                            </LayoutTemplate>
                                                                            <ItemTemplate>
                                                                                <tr id='row<%# Eval("id")%>' style="color:#b40b0b;">
                                                                                    <td><asp:TextBox ID="lbTotal" runat="Server" Text='<%#Eval("GetAnticipo") %>'></asp:TextBox></td>
                                                                                    <td style="padding-left: 20px;"><asp:Label ID="lbCantCuotas" runat="Server" Text='1' /></td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="lbFechaVenc" runat="server" CssClass="textBox textBoxForm" style="width: 116px; margin-top: 2px;"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender7" runat="server" CssClass="orange" TargetControlID="lbFechaVenc" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />
                                                                                    </td>
                                                                                    <td>
	                                                                                    <asp:CheckBox ID="chbGastosAdtvo" runat="server" Checked="true" />
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:ListView>
                                                                    </div>
                                                                </asp:Panel>
                                                            </div>   
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                        
                                                    <asp:UpdatePanel runat="server" ID="upanel2CAC"> 
                                                        <ContentTemplate>                                        
                                                            <div>
                                                                <asp:Panel ID="pnlCuotasManualesMonedaAcordada2CAC" runat="server" Visible="false">
                                                                    <div>
                                                                        <asp:ListView ID="lvFormaPago2CAC" runat="server">
                                                                            <LayoutTemplate>
                                                                                <table style="margin-bottom: 0px; width: 84%">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <td style="width: 20%;">TOTAL</td>
                                                                                            <td style="width: 20%;">CUOTAS</td>
                                                                                            <td style="width: 20%;">FECHA VENCIMIENTO</td>
                                                                                            <td style="width: 7%;">AJUSTE CAC</td>
                                                                                        </tr>
                                                                                    </thead>
                                                                                    <tbody>
                                                                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                                                    </tbody>
                                                                                </table>
                                                                            </LayoutTemplate>
                                                                            <ItemTemplate>
                                                                                <tr id='row<%# Eval("id")%>' style="color:#b40b0b;">
                                                                                    <td><asp:TextBox ID="lbTotal" runat="Server" Text='<%#Eval("GetAnticipo") %>'></asp:TextBox></td>
                                                                                    <td style="padding-left: 20px;"><asp:Label ID="lbCantCuotas" runat="Server" Text='1' /></td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="lbFechaVenc" runat="server" CssClass="textBox textBoxForm" style="width: 116px; margin-top: 2px;"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender7" runat="server" CssClass="orange" TargetControlID="lbFechaVenc" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chbAjusteCAC" runat="server" Checked="true" />
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:ListView>
                                                                    </div>
                                                                </asp:Panel>
                                                            </div>   
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>                                                           
                                                </div>
                                            </asp:Panel>                                            
                                        </div>                                        
                                    </div>   
                                </div> 
                                <div class="formHolderCalendar" style="padding: 26px 25px 10px !important;">   
                                    <div>
                                        <div><modaltitle style="text-align:left; font-size: 16px !important;">Total</modaltitle></div><hr class="line">
                                        <div id="divTotalMonedaAcordada" class="divFormInLine" runat="server" style="width: 100% !important;">
                                            <b><asp:Label ID="lbTotalMonedaAcordada" runat="server" Text=",00" CssClass="labelItem" /></b>
                                        </div>
                                    </div>  
                                </div>   
                            </ContentTemplate>                   
                        </asp:UpdatePanel>
                        
                        <asp:UpdatePanel runat="server" ID="pnlDolarPeso" Visible="false">
                            <ContentTemplate>
                                <div style="height:auto">
                                    <div class="formHolderCalendar">
                                        <div>
                                            <div><modaltitle style="text-align:left; font-size: 16px !important;">Dólar</modaltitle></div>
                                            <hr class="line">
                                            <div style="height:auto;">
                                                <div class="divFormInLine" style=" width: 268px !important;">
                                                    <div>
                                                        <label>
                                                            Monto <font style="font-style: italic; font-size: 13px;">(valor en la moneda correspondiente)</font>
                                                            <asp:TextBox ID="txtMontoMonedaDolar" runat="server" AutoPostBack="true" OnTextChanged="txtMontoMonedaDolar_TextChanged" CssClass="textBox textBoxForm decimal" Text="0" TabIndex="40" style="width: 226px; margin-top: 2px;"/>
                                                            <asp:HiddenField ID="hfMontoMonedaDolar" runat="server" />
                                                            <asp:RequiredFieldValidator id="rfv12" runat="server"
                                                                ControlToValidate="txtMontoMonedaDolar" ErrorMessage="Agregue el monto" InitialValue="0"
                                                                Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                            </asp:RequiredFieldValidator>
                                                        </label>
                                                    </div>
                                                </div>
                                                <asp:Panel ID="convertirDolarPeso" runat="server" Visible="false">
                                                <div class="divFormInLine" style="width: 182px !important;">
                                                    <div style="width: 180px;">
                                                        <label>
                                                            Convertir <font style="font-style: italic; font-size: 13px;">(Moneda acordada)</font><br />
                                                            <div style="margin-top: 6px;">
                                                                <asp:CheckBox ID="chbConvertirDolarPeso" runat="server" TabIndex="25" AutoPostBack="True" OnCheckedChanged="chbConvertirDolarPeso_CheckedChanged" />
                                                                <b><asp:Label ID="lbConvertirDolarPeso" runat="server" Text=",00" CssClass="labelItem"/></b>
                                                            </div>
                                                        </label> 
                                                    </div>                        
                                                </div>
                                                </asp:Panel>
                                                <div class="divFormInLine" style="width: 86px !important;">
                                                    <div style="width:40px">
                                                        <label>
                                                            Cuotas:
                                                            <asp:TextBox ID="txtCuotasMonedaDolar" TabIndex="41" AutoPostBack="true" OnTextChanged="txtCuotasMonedaDolar_TextChanged" runat="server" CssClass="textBox textBoxForm" Text="0" style="width: 38px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                                            <asp:RequiredFieldValidator id="rfv13" runat="server"
                                                                ControlToValidate="txtCuotasMonedaDolar" ErrorMessage="Agregue la cantidad de cuotas" InitialValue="0"
                                                                Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                style="width: 188px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                            </asp:RequiredFieldValidator>
                                                        </label> 
                                                    </div>
                                                </div>
                                                <div class="divFormInLine" style="width: 140px !important;">
                                                    <div style="width: 50%;">
                                                        <label>
                                                            Valor Cuota:
                                                            <div style="padding-top: 5px;"><b><asp:Label ID="lbValorMonedaDolar" runat="server" Text=",00" CssClass="labelItem" style="padding-left: 15px !Important;"/></b></div>
                                                        </label> 
                                                    </div>
                                                </div>                                                    
                                                <asp:Panel ID="pnlFechaVencDolarPeso" runat="server">                              
                                                    <div class="divFormInLine" style="width: 168px !important;">
                                                        <div style="width: 155px;">
                                                            <label>
                                                                Fecha de Vencimiento:
                                                                <asp:TextBox ID="txtFechaVencimientoMonedaDolar" runat="server" TabIndex="42" CssClass="textBox textBoxForm" style="width: 116px;"></asp:TextBox>
                                                                <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="orange" TargetControlID="txtFechaVencimientoMonedaDolar" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                                                <asp:RequiredFieldValidator id="rfv8" runat="server"
                                                                    ControlToValidate="txtFechaVencimientoMonedaDolar"
                                                                    ErrorMessage="Ingrese una fecha"
                                                                    Validationgroup="CustomerOV"
                                                                    Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                    style="width: 116px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                </asp:RequiredFieldValidator>
                                                            </label> 
                                                        </div>                        
                                                    </div>
                                                </asp:Panel>
                                                <div class="divFormInLine" style="width: 80px !important;">
                                                    <div style="width: 60px;">
                                                        <label>
                                                            Refuerzos
                                                            <asp:CheckBox ID="chbCuotasManualesDolarPeso" runat="server" TabIndex="43" AutoPostBack="True" OnCheckedChanged="chbCuotasManualesDolarPeso_CheckedChanged"  />
                                                        </label> 
                                                    </div>                        
                                                </div>
                                                <div class="divFormInLine" style="width: 100px !important;">
                                                    <div style="width: 100px;">
                                                        <label>
                                                            Gastos adtvo.
                                                            <asp:CheckBox ID="chbGastosDolarPeso" runat="server" TabIndex="44" Checked="true"/>
                                                        </label> 
                                                    </div>                        
                                                </div>
                                                <div class="divFormInLine" style="width: 7px !important;">
                                                    <div style="width: 100px;">
                                                        <label>
                                                            Interés anual (%)
                                                            <asp:TextBox ID="txtInteresAnualDolarPeso" runat="server" CssClass="textBox textBoxForm" TabIndex="45" Text="0" style="width: 38px; margin-top: 2px;"></asp:TextBox>
                                                        </label> 
                                                    </div>                        
                                                </div>
                                            </div>
                                
                                            <asp:UpdatePanel runat="server" ID="upanelDolarPeso"> 
                                                <ContentTemplate>                                        
                                                    <div>
                                                        <asp:Panel ID="pnlCuotasManualesMonedaDolarPeso1" runat="server" Visible="false">
                                                            <div>
                                                                <asp:ListView ID="lvFormaPagoDolarPeso" runat="server">
                                                                    <LayoutTemplate>
                                                                        <table style="margin-bottom: 0px;">
                                                                            <thead>
                                                                                <tr>
                                                                                    <td style="width: 28%;">TOTAL</td>
                                                                                    <td style="width: 29%;">CUOTAS</td>
                                                                                    <td style="width: 25%;">FECHA VENCIMIENTO</td>
                                                                                    <td style="width: 25%;">GASTOS ADTVO.</td>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                                            </tbody>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id='row<%# Eval("id")%>' style="color:#b40b0b;">
                                                                            <td><asp:TextBox ID="lbTotal" runat="Server" Text='<%#Eval("GetAnticipo") %>' AutoPostBack="true"></asp:TextBox></td>
                                                                            <td style="padding-left: 20px;"><asp:Label ID="lbCantCuotas" runat="Server" Text='1' /></td>
                                                                            <td>
                                                                                <asp:TextBox ID="lbFechaVenc" runat="server" CssClass="textBox textBoxForm" style="width: 116px; margin-top: 2px;"></asp:TextBox>
                                                                                <ajax:CalendarExtender ID="CalendarExtender7" runat="server" CssClass="orange" TargetControlID="lbFechaVenc" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chbGastosAdtvo" runat="server" Checked="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:ListView>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>   
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                            <div class="divFormInLine" style="width:100%; margin-top: 2%; margin-bottom: 2%;">
                                                <div style="float:left">
                                                    <label style="width:100%; margin-right: 6px;">
                                                        <font style="font-style: italic; font-size: 14px;">¿Desea agregar más cuotas con otra condición de pago?</font>
                                                    </label>
                                                </div>
                                                <div align="left" style="margin-right: 40px;"><asp:CheckBox ID="CheckBox1" runat="server" TabIndex="46" AutoPostBack="True" OnCheckedChanged="chbDolarPesoDolar_CheckedChanged" /></div>
                                            </div>

                                            <asp:Panel runat="server" ID="pnlDolarPeso2" Visible="false" style="padding-bottom: 16%;">
                                                <div>
                                                    <div>
                                                        <div class="divFormInLine" style=" width: 268px !important;">
                                                            <div>
                                                                <label>
                                                                    Monto <font style="font-style: italic; font-size: 13px;">(valor en la moneda correspondiente)</font>
                                                                    <asp:TextBox ID="txtMontoMonedaDolar2" runat="server" TabIndex="47" CssClass="textBox textBoxForm decimal" Text="0" style="width: 226px; margin-top: 2px;" AutoPostBack="true" OnTextChanged="txtMontoMonedaDolar2_TextChanged"/>
                                                                    <asp:HiddenField ID="hfMontoMonedaDolar2" runat="server" />
                                                                    <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                                                                        ControlToValidate="txtMontoMonedaDolar2" ErrorMessage="Agregue el monto" InitialValue="0"
                                                                        Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                        style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                    </asp:RequiredFieldValidator>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <asp:Panel ID="convertirDolarPeso2" runat="server" Visible="false">
                                                        <div class="divFormInLine" style="width: 182px !important;">                                                        
                                                            <div style="width: 180px;">
                                                            <label>
                                                                Convertir <font style="font-style: italic; font-size: 13px;">(Moneda acordada)</font><br />
                                                                <div style="margin-top: 6px;">
                                                                    <asp:CheckBox ID="chbConvertirDolarPeso2" runat="server" TabIndex="48" AutoPostBack="True" OnCheckedChanged="chbConvertirDolarPeso2_CheckedChanged" />
                                                                    <b><asp:Label ID="lbConvertirDolarPeso2" runat="server" Text=",00" CssClass="labelItem"/></b>
                                                                </div>
                                                            </label> 
                                                            </div>                                                                                 
                                                        </div>
                                                        </asp:Panel>  
                                                        <div class="divFormInLine" style="width: 86px !important;">
                                                        <div style="width:40px">
                                                                <label>
                                                                    Cuotas:
                                                                    <asp:TextBox ID="txtCuotasMonedaDolar2" TabIndex="49" AutoPostBack="true" OnTextChanged="txtCuotasMonedaDolar2_TextChanged" runat="server" CssClass="textBox textBoxForm" Text="0" style="width: 38px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                                                    <asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server"
                                                                        ControlToValidate="txtCuotasMonedaDolar2" ErrorMessage="Agregue la cantidad de cuotas" InitialValue="0"
                                                                        Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                        style="width: 188px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                    </asp:RequiredFieldValidator>
                                                                </label> 
                                                            </div>
                                                        </div>
                                                        <div class="divFormInLine" style="width: 140px !important;">
                                                            <div style="width: 50%;">
                                                                <label>
                                                                    Valor Cuota:
                                                                    <div style="padding-top: 5px;"><b><asp:Label ID="lbValorMonedaDolar2" runat="server" Text=",00" CssClass="labelItem" style="padding-left: 15px !Important;"/></b></div>
                                                                </label> 
                                                            </div>
                                                        </div>    
                                                        <asp:Panel ID="pnlFechaVencDolarPeso2" runat="server">
                                                            <div class="divFormInLine" style="width:168px !important">
                                                                <div>
                                                                    <label>
                                                                        Fecha de Vencimiento:
                                                                        <asp:TextBox ID="txtFechaVencimientoMonedaDolar2" runat="server" TabIndex="50" CssClass="textBox textBoxForm" style="margin-top: 2px; width:116px"></asp:TextBox>
                                                                        <ajax:CalendarExtender ID="CalendarExtender5" runat="server" CssClass="orange" TargetControlID="txtFechaVencimientoMonedaDolar2" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                                                        <asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server"
                                                                            ControlToValidate="txtFechaVencimientoMonedaDolar2"
                                                                            ErrorMessage="Ingrese una fecha"
                                                                            Validationgroup="CustomerOV"
                                                                            Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                            style="width: 116px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                        </asp:RequiredFieldValidator>
                                                                    </label> 
                                                                </div>                        
                                                            </div>
                                                        </asp:Panel>
                                                        <div class="divFormInLine" style="width: 80px !important;">
                                                            <div style="width: 60px;">
                                                                <label>
                                                                    Refuerzos
                                                                    <asp:CheckBox ID="chbCuotasManualesDolarPeso2" runat="server" TabIndex="51" AutoPostBack="True" OnCheckedChanged="chbCuotasManualesDolarPeso2_CheckedChanged" />
                                                                </label> 
                                                            </div>                        
                                                        </div>
                                                        <div class="divFormInLine" style="width: 100px !important;">
                                                            <div style="width: 100px;">
                                                                <label>
                                                                    Gastos adtvo.
                                                                    <asp:CheckBox ID="chbGastosDolarPeso2" runat="server" TabIndex="52" Checked="true"/>
                                                                </label> 
                                                            </div>                        
                                                        </div>
                                                        <div class="divFormInLine" style="width: 7px !important;">
                                                            <div style="width: 100px;">
                                                                <label>
                                                                    Interés anual (%)
                                                                    <asp:TextBox ID="txtInteresAnualDolarPeso2" runat="server" CssClass="textBox textBoxForm" TabIndex="53" Text="0" style="width: 38px; margin-top: 2px;"></asp:TextBox>
                                                                </label> 
                                                            </div>                        
                                                        </div>
                                                    </div>

                                                    <asp:UpdatePanel runat="server" ID="upanelDolarPeso2"> 
                                                        <ContentTemplate>                                        
                                                            <div>
                                                                <asp:Panel ID="pnlCuotasManualesMonedaDolarPeso2" runat="server" Visible="false">
                                                                    <div>
                                                                        <asp:ListView ID="lvFormaPagoDolarPeso2" runat="server">
                                                                            <LayoutTemplate>
                                                                                <table style="margin-bottom: 0px;">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <td style="width: 28%;">TOTAL</td>
                                                                                            <td style="width: 29%;">CUOTAS</td>
                                                                                            <td style="width: 25%;">FECHA VENCIMIENTO</td>
                                                                                            <td style="width: 25%;">GASTOS ADTVO.</td>
                                                                                        </tr>
                                                                                    </thead>
                                                                                    <tbody>
                                                                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                                                    </tbody>
                                                                                </table>
                                                                            </LayoutTemplate>
                                                                            <ItemTemplate>
                                                                                <tr id='row<%# Eval("id")%>' style="color:#b40b0b;">
                                                                                    <td><asp:TextBox ID="lbTotal" runat="Server" Text='<%#Eval("GetAnticipo") %>' AutoPostBack="true"></asp:TextBox></td>
                                                                                    <td style="padding-left: 20px;"><asp:Label ID="lbCantCuotas" runat="Server" Text='1' /></td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="lbFechaVenc" runat="server" CssClass="textBox textBoxForm" style="width: 116px; margin-top: 2px;"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender7" runat="server" CssClass="orange" TargetControlID="lbFechaVenc" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chbGastosAdtvo" runat="server" Checked="true" />
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:ListView>
                                                                    </div>
                                                                </asp:Panel>
                                                            </div>   
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </asp:Panel>                                        
                                        </div>
                                    </div>
                                    <div class="formHolderCalendar">
                                        <div>
                                            <div><modaltitle style="text-align:left; font-size: 16px !important;">Pesos</modaltitle></div>
                                            <hr class="line"> 
                                            <div style="height:auto;">
                                                <div class="divFormInLine" style=" width: 268px !important;">
                                                    <div>
                                                        <label>
                                                            Monto <font style="font-style: italic; font-size: 13px;">(valor en la moneda correspondiente)</font>:
                                                            <asp:TextBox ID="txtMontoMonedaPeso" runat="server" TabIndex="54" CssClass="textBox textBoxForm decimal" Text="0" style="width: 226px; margin-top: 2px;" AutoPostBack="true" OnTextChanged="txtMontoMonedaPeso_TextChanged"/>
                                                            <asp:HiddenField ID="hfMontoMonedaPeso" runat="server" />
                                                            <asp:RequiredFieldValidator id="rfv9" runat="server"
                                                                ControlToValidate="txtMontoMonedaPeso" ErrorMessage="Agregue el monto" InitialValue="0"
                                                                Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                            </asp:RequiredFieldValidator>
                                                        </label>
                                                    </div>
                                                </div>
                                                <asp:Panel ID="convertirPesoDolar" runat="server" Visible="false">
                                                <div class="divFormInLine" style="width: 182px !important;">
                                                    <div style="width: 180px;">
                                                        <label>
                                                            Convertir <font style="font-style: italic; font-size: 13px;">(Moneda acordada)</font><br />
                                                            <div style="margin-top: 6px;">
                                                                <asp:CheckBox ID="chbConvertirPesoDolar" runat="server" TabIndex="55" AutoPostBack="True" OnCheckedChanged="chbConvertirPesoDolar_CheckedChanged"/>
                                                                <b><asp:Label ID="lbConvertirPesoDolar" runat="server" Text=",00" CssClass="labelItem"/></b>
                                                            </div>
                                                        </label> 
                                                    </div>                        
                                                </div>
                                                </asp:Panel>
                                                <div class="divFormInLine" style="width: 86px !important;">
                                                    <div style="width:40px">
                                                        <label>
                                                            Cuotas:
                                                            <asp:TextBox ID="txtCuotasMonedaPeso" TabIndex="56" AutoPostBack="true" OnTextChanged="txtCuotasMonedaPeso_TextChanged" runat="server" CssClass="textBox textBoxForm" Text="0" style="width: 38px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                                            <asp:RequiredFieldValidator id="rfv10" runat="server"
                                                                ControlToValidate="txtCuotasMonedaPeso" ErrorMessage="Agregue la cantidad de cuotas" InitialValue="0"
                                                                Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                style="width: 188px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                            </asp:RequiredFieldValidator>
                                                        </label> 
                                                    </div>
                                                </div>
                                                <div class="divFormInLine" style="width:140px !important">
                                                    <div style="width: 50%;">
                                                        <label>
                                                            Valor Cuota:
                                                            <div style="padding-top: 5px;"><b><asp:Label ID="lbValorMonedaPeso" runat="server" Text=",00" CssClass="labelItem" style="padding-left: 15px !Important;"/></b></div>
                                                        </label> 
                                                    </div>
                                                </div> 
                                                <asp:Panel ID="pnlFechaVencPeso" runat="server">                              
                                                    <div class="divFormInLine" style="width: 168px !important">
                                                        <div style="width: 155px;">
                                                            <label>
                                                                Fecha de Vencimiento:
                                                                <asp:TextBox ID="txtFechaVencimientoMonedaPeso" runat="server" TabIndex="57" CssClass="textBox textBoxForm" style="width: 116px"></asp:TextBox>
                                                                <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="orange" TargetControlID="txtFechaVencimientoMonedaPeso" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                                                <asp:RequiredFieldValidator id="rfv11" runat="server"
                                                                    ControlToValidate="txtFechaVencimientoMonedaPeso"
                                                                    ErrorMessage="Ingrese una fecha"
                                                                    Validationgroup="CustomerOV"
                                                                    Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                    style="width: 116px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                </asp:RequiredFieldValidator>
                                                            </label> 
                                                        </div>                        
                                                    </div>
                                                </asp:Panel>
                                                <div class="divFormInLine" style="width: 80px !important;">
                                                    <div style="width: 60px;">
                                                        <label>
                                                            Refuerzos
                                                            <asp:CheckBox ID="chbCuotasManualesDolarPesoPeso" runat="server" TabIndex="58" AutoPostBack="True" OnCheckedChanged="chbCuotasManualesDolarPesoPeso_CheckedChanged"  />
                                                        </label> 
                                                    </div>                        
                                                </div>
                                                <div class="divFormInLine" style="width: 100px !important;">
                                                    <div style="width: 100px;">
                                                        <label>
                                                            Gastos adtvo.
                                                            <asp:CheckBox ID="chbGastosDolarPesoPeso" runat="server" TabIndex="59" Checked="true"/>
                                                        </label> 
                                                    </div>                        
                                                </div>
                                                <div class="divFormInLine" style="width: 7px !important;">
                                                    <div style="width: 100px;">
                                                        <label>
                                                            Interés anual (%)
                                                            <asp:TextBox ID="txtInteresAnualPesoPeso" runat="server" CssClass="textBox textBoxForm" TabIndex="60" Text="0" style="width: 38px; margin-top: 2px;"></asp:TextBox>
                                                        </label> 
                                                    </div>                        
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnlRangoPeso" runat="server">
                                                <div class="divFormInLine" style="width:100%; margin-top: 16px; margin-bottom: 1%;">
                                                    <div style="width:140px">
                                                        <label>
                                                            Rango cuotas ajustables:<br />
                                                            De&nbsp;<asp:TextBox ID="txtRangoDesdePeso" runat="server" CssClass="textBox textBoxForm" Text="1" TabIndex="61" style="width: 15px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                                            a&nbsp;<asp:TextBox ID="txtRangoAPeso" runat="server" CssClass="textBox textBoxForm" Text="1" style="width: 15px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                                        </label> 
                                                    </div>
                                                </div>
                                            </asp:Panel>

                                            <asp:UpdatePanel runat="server" ID="upanelDolarPesoPeso"> 
                                                <ContentTemplate>                                        
                                                    <div>
                                                        <asp:Panel ID="pnlCuotasManualesMonedaDolarPesoPeso" runat="server" Visible="false">
                                                            <div>
                                                                <asp:ListView ID="lvFormaPagoDolarPesoPeso" runat="server">
                                                                    <LayoutTemplate>
                                                                        <table style="margin-bottom: 0px;">
                                                                            <thead>
                                                                                <tr>
                                                                                    <td style="width: 20%;">TOTAL</td>
                                                                                    <td style="width: 20%;">CUOTAS</td>
                                                                                    <td style="width: 20%;">FECHA VENCIMIENTO</td>
                                                                                    <td style="width: 7%;">AJUSTE CAC</td>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                                            </tbody>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id='row<%# Eval("id")%>' style="color:#b40b0b;">
                                                                            <td><asp:TextBox ID="lbTotal" runat="Server" Text='<%#Eval("GetAnticipo") %>' AutoPostBack="true"></asp:TextBox></td>
                                                                            <td style="padding-left: 20px;"><asp:Label ID="lbCantCuotas" runat="Server" Text='1' /></td>
                                                                            <td>
                                                                                <asp:TextBox ID="lbFechaVenc" runat="server" CssClass="textBox textBoxForm" style="width: 116px; margin-top: 2px;"></asp:TextBox>
                                                                                <ajax:CalendarExtender ID="CalendarExtender7" runat="server" CssClass="orange" TargetControlID="lbFechaVenc" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chbAjusteCAC" runat="server" Checked="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:ListView>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>   
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                            <div class="divFormInLine" style="width:100%; margin-top: 1%; margin-bottom: 1%;">
                                                <div style="float:left">
                                                    <label style="width:100%; margin-right: 6px;">
                                                        <font style="font-style: italic; font-size: 14px;">¿Desea agregar más cuotas con otra condición de pago?</font>
                                                    </label>
                                                </div>
                                                <div align="left" style="margin-right: 40px;"><asp:CheckBox ID="CheckBox2" runat="server" TabIndex="62" AutoPostBack="True" OnCheckedChanged="chbDolarPesoPeso_CheckedChanged"/></div>
                                            </div>
                                                                                       
                                            <asp:Panel runat="server" ID="pnlDolarPeso3" Visible="false" style="padding-bottom: 3%;">
                                                <div>
                                                    <div>
                                                        <div class="divFormInLine" style=" width: 268px !important;">
                                                            <div>
                                                                <label>
                                                                    Monto <font style="font-style: italic; font-size: 13px;">(valor en la moneda correspondiente)</font>
                                                                    <asp:TextBox ID="txtMontoMonedaPeso3" runat="server" CssClass="textBox textBoxForm decimal" Text="0" TabIndex="63" style="width: 226px; margin-top: 2px;" AutoPostBack="true" OnTextChanged="txtMontoMonedaPeso3_TextChanged"/>
                                                                    <asp:HiddenField ID="hfMontoMonedaPeso3" runat="server" />
                                                                    <asp:RequiredFieldValidator id="RequiredFieldValidator7" runat="server"
                                                                        ControlToValidate="txtMontoMonedaPeso3" ErrorMessage="Agregue el monto" InitialValue="0"
                                                                        Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                        style="width: 226px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                    </asp:RequiredFieldValidator>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <asp:Panel ID="convertirPesoDolar2" runat="server" Visible="false">
                                                        <div class="divFormInLine" style="width: 182px !important;">
                                                            <div style="width: 180px;">
                                                                <label>
                                                                    Convertir <font style="font-style: italic; font-size: 13px;">(Moneda acordada)</font><br />
                                                                    <div style="margin-top: 6px;">
                                                                        <asp:CheckBox ID="chbConvertirPesoDolar2" runat="server" TabIndex="64" AutoPostBack="True" OnCheckedChanged="chbConvertirPesoDolar2_CheckedChanged" />
                                                                        <b><asp:Label ID="lbConvertirPesoDolar2" runat="server" Text=",00" CssClass="labelItem"/></b>
                                                                    </div>
                                                                </label> 
                                                            </div>                        
                                                        </div>
                                                        </asp:Panel>
                                                        <div class="divFormInLine" style="width: 86px !important;">
                                                            <div style="width:40px">
                                                                <label>
                                                                    Cuotas:
                                                                    <asp:TextBox ID="txtCuotasMonedaPeso3" TabIndex="65" AutoPostBack="true" OnTextChanged="txtCuotasMonedaPeso3_TextChanged"  runat="server" CssClass="textBox textBoxForm" Text="0" style="width: 38px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                                                    <asp:RequiredFieldValidator id="RequiredFieldValidator8" runat="server"
                                                                        ControlToValidate="txtCuotasMonedaPeso3" ErrorMessage="Agregue la cantidad de cuotas" InitialValue="0"
                                                                        Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                        style="width: 188px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                    </asp:RequiredFieldValidator>
                                                                </label> 
                                                            </div>
                                                        </div>
                                                        <div class="divFormInLine" style="width: 140px !important;">
                                                            <div style="width: 50%;">
                                                                <label>
                                                                    Valor Cuota:
                                                                    <div style="padding-top: 5px;"><b><asp:Label ID="lbValorMonedaPeso3" runat="server" Text=",00" CssClass="labelItem" style="padding-left: 15px !Important;"/></b></div>
                                                                </label> 
                                                            </div>
                                                        </div>    
                                                        <asp:Panel ID="pnlFechaVencPeso3" runat="server">
                                                            <div class="divFormInLine" style="width: 168px !important">
                                                                <div style="width:155px">
                                                                    <label>
                                                                        Fecha de Vencimiento:
                                                                        <asp:TextBox ID="txtFechaVencimientoMonedaPeso3" runat="server" TabIndex="66" CssClass="textBox textBoxForm" style="margin-top: 2px; width:116px"></asp:TextBox>
                                                                        <ajax:CalendarExtender ID="CalendarExtender6" runat="server" CssClass="orange" TargetControlID="txtFechaVencimientoMonedaPeso3" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                                                        <asp:RequiredFieldValidator id="RequiredFieldValidator9" runat="server"
                                                                            ControlToValidate="txtFechaVencimientoMonedaPeso3"
                                                                            ErrorMessage="Ingrese una fecha"
                                                                            Validationgroup="CustomerOV"
                                                                            Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                                                            style="width: 116px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                                                                        </asp:RequiredFieldValidator>
                                                                    </label> 
                                                                </div>                        
                                                            </div>
                                                        </asp:Panel>
                                                        <div class="divFormInLine" style="width: 80px !important;">
                                                            <div style="width: 60px;">
                                                                <label>
                                                                    Refuerzos
                                                                    <asp:CheckBox ID="chbCuotasManualesDolarPesoPeso3" runat="server" TabIndex="67" AutoPostBack="True" OnCheckedChanged="chbCuotasManualesDolarPesoPeso3_CheckedChanged"/>
                                                                </label> 
                                                            </div>                        
                                                        </div>
                                                        <div class="divFormInLine" style="width: 100px !important;">
                                                            <div style="width: 100px;">
                                                                <label>
                                                                    Gastos adtvo.
                                                                    <asp:CheckBox ID="chbGastosManualesDolarPesoPeso3" runat="server" TabIndex="68" Checked="true"/>
                                                                </label> 
                                                            </div>                        
                                                        </div>
                                                        <div class="divFormInLine" style="width: 7px !important;">
                                                            <div style="width: 100px;">
                                                                <label>
                                                                    Interés anual (%)
                                                                    <asp:TextBox ID="txtInteresAnualPesoPeso3" runat="server" CssClass="textBox textBoxForm" TabIndex="69" Text="0" style="width: 38px; margin-top: 2px;"></asp:TextBox>
                                                                </label> 
                                                            </div>                        
                                                        </div>
                                                    </div>
                                                    <asp:Panel ID="pnlRangoPeso3" runat="server">
                                                            <div class="divFormInLine" style="width:100%; margin-top: 16px; margin-bottom: 1%;">
                                                                <div style="width:140px">
                                                                    <label>
                                                                        Rango cuotas ajustables:<br />
                                                                        De&nbsp;<asp:TextBox ID="txtRangoDesdePeso3" runat="server" CssClass="textBox textBoxForm" Text="1" TabIndex="70" style="width: 15px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                                                        a&nbsp;<asp:TextBox ID="txtRangoAPeso3" runat="server" CssClass="textBox textBoxForm" Text="1" TabIndex="71" style="width: 15px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>
                                                                    </label> 
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                                                                        
                                                    <asp:UpdatePanel runat="server" ID="upanelDolarPesoPeso2"> 
                                                        <ContentTemplate>                                        
                                                            <div>
                                                                <asp:Panel ID="pnlCuotasManualesMonedaDolarPesoPeso2" runat="server" Visible="false">
                                                                    <div>
                                                                        <asp:ListView ID="lvFormaPagoDolarPesoPeso2" runat="server">
                                                                            <LayoutTemplate>
                                                                                <table style="margin-bottom: 0px;">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <td style="width: 20%;">TOTAL</td>
                                                                                            <td style="width: 20%;">CUOTAS</td>
                                                                                            <td style="width: 20%;">FECHA VENCIMIENTO</td>
                                                                                            <td style="width: 7%;">AJUSTE CAC</td>
                                                                                        </tr>
                                                                                    </thead>
                                                                                    <tbody>
                                                                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                                                    </tbody>
                                                                                </table>
                                                                            </LayoutTemplate>
                                                                            <ItemTemplate>
                                                                                <tr id='row<%# Eval("id")%>' style="color:#b40b0b;">
                                                                                    <td><asp:TextBox ID="lbTotal" runat="Server" Text='<%#Eval("GetAnticipo") %>' AutoPostBack="true"></asp:TextBox></td>
                                                                                    <td style="padding-left: 20px;"><asp:Label ID="lbCantCuotas" runat="Server" Text='1' /></td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="lbFechaVenc" runat="server" CssClass="textBox textBoxForm" style="width: 116px; margin-top: 2px;"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender7" runat="server" CssClass="orange" TargetControlID="lbFechaVenc" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chbAjusteCAC" runat="server" Checked="true" />
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:ListView>
                                                                    </div>
                                                                </asp:Panel>
                                                            </div>   
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </asp:Panel>                                            
                                        </div>
                                    </div>
                                </div>
                                <div class="formHolderCalendar" style="padding: 26px 25px 10px !important;">
                                    <div>
                                        <div><modaltitle style="text-align:left; font-size: 16px !important;">Total</modaltitle></div><hr class="line">
                                        <div id="divTotalDolarPeso" runat="server" class="divFormInLine" style="width: 100% !important;">
                                            <b><asp:Label ID="lbTotalDolarPeso" runat="server" Text=",00" CssClass="labelItem decimal" /></b>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:HiddenField ID="hfTotal" runat="server" />
                        <asp:HiddenField ID="hfTotalAux" runat="server" />
                    </section>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
                <section>
                    <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                        <div>
                            <b><asp:Label ID="lbMensaje" runat="server" CssClass="messageError" style="margin-left: 7px;" Text="Seleccione una o más unidades funcionales"></asp:Label></b>
                        </div>
                    </div>
                </section>
            </asp:Panel>

            <section>
                <asp:UpdatePanel ID="pnlFinalizarOV" runat="server">
                    <ContentTemplate>
                        <div runat="server" class="formHolder" style="padding: 22px 14px 12px;">
                            <div runat="server">
                                <div style="float:left">
                                    <label><asp:Button ID="btnFinalizar" Text="Finalizar" CssClass="formBtnNar" runat="server" style="float:left; margin-top: -4px; margin-left: 15px;" Validationgroup="CustomerOV" OnClick="btnFinalizar_Click"/></label>
                                </div>
                                <div style="float:right">
                                    <asp:Button ID="btnVolver" Text="Volver al listado de operaciones de venta" CssClass="formBtnGrey1" runat="server" OnClick="btnVolver_Click" style="float:left; margin-left:15px; margin-top: -5px;"/>
                                </div>                            
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </section>
        </div>            
    </ContentTemplate>
</asp:UpdatePanel>    
    
<div style="float:left">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlFinalizarOV">
        <ProgressTemplate>
            <div class="overlay">
                <div class="overlayContent">
                    <div style="float:left;"><img src="images/ring_loading.gif"  width="300px" class="loading100" ImageAlign="left"  /></div>
                    <div style="float:left; padding: 8px 0 0 10px">
                        <h2> Procesando... </h2>
                    </div>                                    
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
</asp:Content>