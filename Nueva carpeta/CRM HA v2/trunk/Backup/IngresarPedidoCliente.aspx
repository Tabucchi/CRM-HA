<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageCliente.master" AutoEventWireup="true" CodeBehind="IngresarPedidoCliente.aspx.cs" Inherits="crm.IngresarPedidoCliente" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<Ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    <Services>
        <asp:ServiceReference Path="MyAutocompleteService.asmx" />
    </Services>
</Ajax:ToolkitScriptManager>
    <section>
        <h2>INFORMACIÓN DEL TICKET (Campos Obligatorios)</h2>
        <div class="formHolder">
            <label> <span> TITULO </span> <asp:TextBox ID="txtTitulo" runat="server" CssClass="textbox"></asp:TextBox> 
                <asp:RequiredFieldValidator ID="RFV2" runat="server" Display="None" ErrorMessage="Por favor, ingrese un titulo." ControlToValidate="txtTitulo"></asp:RequiredFieldValidator> 
                <ajax:ValidatorCalloutExtender 
                    runat="Server"
                    ID="RFVE2"
                    TargetControlID="RFV2" 
                    Width="250px"
                    HighlightCssClass="highlight"
                    PopupPosition="Right" />
            </label>
            <label class="rigthLabel"> <span> DESCRIPCION </span>
                <asp:TextBox ID="txtDescripcion" Rows="3" CssClass="textbox" TextMode="MultiLine" Height="60px" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFV3" runat="server" Display="None" ErrorMessage="Por favor, ingrese una descripción." ControlToValidate="txtDescripcion"></asp:RequiredFieldValidator> 
                <ajax:ValidatorCalloutExtender 
                    runat="Server"
                    ID="VFVE3"
                    TargetControlID="RFV3" 
                    Width="300px"
                    HighlightCssClass="highlight" 
                    PopupPosition="Right" />
            </label>
            <label><span>CATEGORIA</span> 
                <asp:DropDownList ID="cbCategoria" runat="server">
                    <asp:ListItem Text="Seleccione una Categoría..." Value="0" />
                </asp:DropDownList>
            </label>          
            <label> <span>PRIORIDAD</span> 
                <asp:DropDownList ID="cbPrioridad" runat="server"></asp:DropDownList>
            </label>
        </div>   
        <div class="formHolder">
            <label> <asp:Button ID="btnCargar" Text="Cargar" class=" formBtnNar" style="float:left;" runat="server"  onclick="btnCargar_Click" /></label>
        </div>
    </section> 
</asp:Content>
