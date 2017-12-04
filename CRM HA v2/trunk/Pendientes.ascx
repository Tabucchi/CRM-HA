<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pendientes.ascx.cs" Inherits="crm.Pendientes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<link href="css/masterStyle.css" rel="stylesheet" />
<aside>
    <Ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
        <Services>
            <asp:ServiceReference Path="MyAutocompleteService.asmx" />
        </Services>
    </Ajax:ToolkitScriptManager>

    <div class="sideGroup">
        <div class="sideHeader" style="border-bottom: 1px solid rgba(205, 205, 205, 0.42);">
            <div><h3> Pendientes </h3></div>
        </div>
    </div>

    <div class="sideGroup">
		<div class="sideHeader itemPanel">
			<h2>Nuevos precios <asp:Label ID="lbCantPrecios" CssClass="itemCount" runat="server" Text="0"></asp:Label></h2>
            <ul class="options" style="padding-top: 3px !important; padding-right: 0px !important">
				<li><a href="PendientesPrecios.aspx" class="optMore">Ver+</a></li>
			</ul>
		</div>
            
		<div class="sideHeader itemPanel" style="border-bottom: 1px solid rgba(205, 205, 205, 0.42);">
			<h2>Operaciones de venta <asp:Label ID="lbCantOV" CssClass="itemCount" runat="server" Text="0"></asp:Label></h2>
            <ul class="options" style="padding-top: 3px !important; padding-right: 0px !important">
				<li><a href="PendientesOperacionesVenta.aspx" class="optMore">Ver+</a></li>
			</ul>
		</div>
    </div>
</aside>