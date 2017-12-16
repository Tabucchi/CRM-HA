<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DetalleManual" Codebehind="DetalleManual.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</ajax:ToolkitScriptManager> 
<link id="link1" rel="stylesheet" href="Estilos/Calendario.css" type="text/css" runat="server" /> 
<section> 
    <div class="headOptions">      
        <h2>Empresa:</h2>
        <asp:DropDownList ID="ddlEmpresas" runat="server" CssClass="floatInput"></asp:DropDownList>
        <h2>Titulo:</h2>
        <asp:TextBox ID="txtTitulo" runat="server" CssClass="floatInput"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" Display="None" ErrorMessage="Por favor, un titulo para el nuevo manual" ControlToValidate="txtTitulo" ValidationGroup="rfvTitulo"></asp:RequiredFieldValidator> 
        <ajax:ValidatorCalloutExtender runat="Server" ID="rfv2" TargetControlID="rfvTitulo" HighlightCssClass="highlight" PopupPosition="Right" />                                     
        <asp:Button ID="btnGuardarManual" CssClass="formBtnNar" runat="server" Text="Guardar" class="boton" OnClick="btnGuardarDatos_Click" CausesValidation="true" ValidationGroup="rfvTitulo"/></td>
        <div align="center" style="padding-top:9px"><asp:Label ID="lbMensajeGuardar" runat="server" Text="El manual se ha guardado correctamente" Font-Bold="True" ForeColor="#99CC00" Visible="false"></asp:Label></div>
    </div>
    <cc1:Editor ID="htmlEditor" runat="server"  Width="100%" Height="400px" IgnoreTab="True"/>
    <div class="headOptions">
        Creado por: 
        <asp:Label ID="lbCreado" runat="server"></asp:Label> &nbsp; &nbsp; &nbsp; 
        Fecha:
        <asp:Label ID="lbFecha" runat="server"></asp:Label>
        <div style=" float:right;">
            <a href="Manual.aspx" class="formBtnGrey">Volver a lista de Manuales</a>
            <asp:Button ID="btnEliminar" CssClass="formBtnGrey" runat="server" class="boton" Text="Eliminar" onclick="btnEliminar_Click" />
        </div>
    </div>
</section>
</asp:Content>

