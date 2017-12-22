<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="MensajeError.aspx.cs" Inherits="crm.MensajeError" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section>
<div style="padding-top:96px">
    <div align="center">
        <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/images/Error.png" Height="282px" Width="600px">
            <div align="center" style="padding-top:146px">
                <h2>Se ha producido un error</h2>
                <h3 id="titulo" runat="server"></h3>
                <h3 id="detalle" runat="server"></h3>
            </div>
        </asp:Panel>
    </div>
</div>
</section>
</asp:Content>
