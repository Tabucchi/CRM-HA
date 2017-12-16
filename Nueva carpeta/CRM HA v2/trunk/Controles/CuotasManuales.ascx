<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CuotasManuales.ascx.cs" Inherits="crm.Controles.CuotasManuales" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<div style="margin-top: 7%;">
    <div class="divFormInLine">
        <div>
            <label>
                Monto:               
                <asp:TextBox ID="TextBox1" runat="server" CssClass="textBox textBoxForm" Text="0" style="width: 256px; margin-top: 2px;" />
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <%--<asp:RequiredFieldValidator id="RequiredFieldValidator10" runat="server"
                    ControlToValidate="txtMontoMonedaAcordada2" ErrorMessage="Agregue el monto"  InitialValue="0"
                    Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                    style="width: 256px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                </asp:RequiredFieldValidator>--%>
            </label>
        </div>
    </div>
    <div class="divFormInLine">
        <div>
            <label>
                Cuotas:
                <asp:TextBox ID="TextBox2" AutoPostBack="true" runat="server" CssClass="textBox textBoxForm" Text="0" style="width: 256px; margin-top: 2px;"/>
                <%--<asp:TextBox ID="TextBox2" AutoPostBack="true" OnTextChanged="TextBox2_TextChanged" runat="server" CssClass="textBox textBoxForm" Text="0" style="width: 256px; margin-top: 2px;" onkeypress="return onKeyDecimal(event,this);"/>--%>
                <%--<asp:RequiredFieldValidator id="RequiredFieldValidator11" runat="server"
                    ControlToValidate="txtCuotasMonedaAcordada2" ErrorMessage="Ingrese una cantidad" InitialValue="0"
                    Validationgroup="CustomerOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                    style="width: 256px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                </asp:RequiredFieldValidator>--%>
            </label> 
        </div>
    </div>                                        
    <div class="divFormInLine" style="width: 16%;">
        <div style="width: 50%;">
            <label>
                Valor Cuota:
                 <div style="padding-top: 5px;">
                    <b><asp:Label ID="Label3" runat="server" Text="0" class="labelItem" style="padding-left: 15px !Important;"/></b>
                </div>
            </label> 
        </div>
    </div> 
                                                                                   
    <div class="divFormInLine">
        <div style="width: 155px;">
            <label>
                Fecha de Vencimiento:
                <asp:TextBox ID="TextBox3" runat="server" CssClass="textBox textBoxForm" style="margin-top: 2px;"></asp:TextBox>
                <%--<ajax:CalendarExtender ID="CalendarExtender7" runat="server" CssClass="orange" TargetControlID="txtFechaVencimientoMonedaAcordada2" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                <asp:RequiredFieldValidator id="RequiredFieldValidator12" runat="server"
                    ControlToValidate="txtFechaVencimientoMonedaAcordada2"
                    ErrorMessage="Ingrese una fecha"
                    Validationgroup="CustomerOV"
                    Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                    style="width: 280px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                </asp:RequiredFieldValidator>--%>
            </label> 
        </div>                        
    </div>
</div>
