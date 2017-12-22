<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MensajeIndiceCac.ascx.cs" Inherits="crm.Controles.MensajeIndiceCac" %>
<section>         
    <asp:Panel ID="Panel1" runat="server" BorderColor="Red" BorderStyle="Solid" BorderWidth="1px" Height="120px" style="border-radius: 23px;">
        <div align="center" style="width:100%; margin-top: 19px; margin-bottom:10px">
            <h2> Índice CAC </h2>
            <div style="float:left; margin-left: 20%;"><asp:Image ID="Image1" runat="server" Height="50px" ImageUrl="~/images/Warning.png" Width="50px" /></div>
            <div style="float:right; margin-top: 18px; margin-right: 21%;">
                <asp:Label runat="server" Font-Bold="true" ForeColor="Red" Font-Size="Medium">Recuerde actualizar el índice CAC del presente mes. Para ello ingrese <a href="Configuracion.aspx">aquí</a>.</asp:Label>
            </div>
        </div>     
    </asp:Panel> 
</section>