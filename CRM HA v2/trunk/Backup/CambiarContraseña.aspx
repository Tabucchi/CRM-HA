<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="CambiarContraseña" Title="Página sin título" Codebehind="CambiarContraseña.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <section>
        <h2>CAMBIO DE CONTRASEÑA</h2>
        <div class="formHolder">
            <p class="col3"><strong>Usuario:</strong> <asp:Literal ID="lbUsuario" runat="server" ></asp:Literal></p> 
            <p class="col3">&nbsp;</p>
            <p class="col3">&nbsp;</p>
            <p><strong>Nueva Clave:</strong> <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" style="width:200px"></asp:TextBox></p>
            <p><strong>Confirmar: </strong><asp:TextBox ID="txtConfirmarContraseña" runat="server" Width="200px" TextMode="Password"></asp:TextBox> </p>
            <p><strong>Ver Caracteres:</strong><asp:CheckBox ID="ckPass" runat="server" AutoPostBack="True" oncheckedchanged="ckPass_CheckedChanged" /> </p>
        </div>
        <div>
            <h1 style="color: #C9D034; font-weight: bold">
            La contraseña debe tener un mínimo de 6 caracteres alfanuméricos.
            </h1>
        </div>

        <div class="formHolder" >
            <label class="rigthLabel">
                <asp:Button ID="btnGuardar" Text="Guardar" class="formBtnNar" runat="server" onclick="btnGuardar_Click"/>  
                <asp:Button ID="btnCancelar" Text="Cancelar" style="float: right;" class="formBtnGrey" runat="server" onclick="btnCancelar_Click"/>  
            </label>
            <label>
                <h3 style="color: #FF0000; font-weight: bold">
                    <asp:Literal ID="lbMensaje" runat="server" Text="Las contraseñas no coinciden" Visible="false"></asp:Literal>
                    <asp:Literal ID="lbMensajeError" runat="server"  Text="La contraseña no es correcta"  Visible="false"></asp:Literal>
                </h3>
                <h3 style="color: #669900; font-weight: bold">
                    <asp:Literal ID="lbGuardar" runat="server" Text="Se ha guardado la nueva contraseña" Visible="False"></asp:Literal>
                </h3>
            </label>
        </div>
        <ajax:PasswordStrength ID="TextBox1_PasswordStrength" runat="server" Enabled="True" TargetControlID="txtPassword" MinimumNumericCharacters="1" PreferredPasswordLength="6" DisplayPosition="RightSide" StrengthIndicatorType="BarIndicator" BarBorderCssClass="barBorder" BarIndicatorCssClass="barInternal"> </ajax:PasswordStrength>                       
    </section>
</asp:Content>