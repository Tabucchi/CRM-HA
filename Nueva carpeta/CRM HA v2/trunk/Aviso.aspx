<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="Aviso.aspx.cs" Inherits="crm.Aviso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section>
    <div style="padding-top:96px">
        <div align="center">
            <div align="center" style="padding-top:0px">
                <h2>Estimado cliente: le informamos que su usuario se encuentra bloqueado.</h2>
                <h2>Comuníquese con un representante de HA.</h2>
                <h1>Tel: (011) 4897-5423 / 5424</h1>
                <asp:Button ID="btnVolver" Text="Volver" runat="server" style="border: 1px solid #CCCCCC; border-radius: 3px; padding: 6px; margin-top: 16px; width: 12%;" onclick="btnVolver_Click"/>
            </div>
        </div>
    </div>
</section>
</asp:Content>
