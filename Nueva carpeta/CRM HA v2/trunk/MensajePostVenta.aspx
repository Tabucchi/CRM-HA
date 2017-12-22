<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePostVenta.master" AutoEventWireup="true" CodeBehind="MensajePostVenta.aspx.cs" Inherits="crm.MensajePostVenta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section>
<div style="padding-top:96px">
    <div align="center">
        <div align="center" style="padding-top:0px">
            <h2><asp:Label ID="lbText" runat="server" style="padding-left:5%" Text="Su consulta ha sido enviada correctamente."></asp:Label></h2>
            <br />
            <h4><asp:Label ID="Label1" runat="server" style="padding-left:5%" Text="En breve nos pondremos en contacto con usted."></asp:Label></h4>
            <asp:Button ID="btnVolver" Text="Volver" runat="server" style="border: 1px solid #CCCCCC; border-radius: 3px; padding: 6px; margin-top: 5px;" onclick="btnVolver_Click" />
        </div>
    </div>
</div>
</section>
</asp:Content>
