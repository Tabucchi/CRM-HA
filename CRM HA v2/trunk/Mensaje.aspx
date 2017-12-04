<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Mensaje" Codebehind="Mensaje.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section>
<div style="padding-top:96px">
    <div align="center">
        <div align="center" style="padding-top:0px">
            <h2><asp:Label ID="lbText" runat="server" style="padding-left:5%"></asp:Label></h2>
            <asp:Button ID="btnVolver" Text="Volver" runat="server" style="border: 1px solid #CCCCCC; border-radius: 3px; padding: 6px;" onclick="btnVolver_Click" />
        </div>
    </div>
</div>
</section>
</asp:Content>

