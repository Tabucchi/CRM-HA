<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="IngresoPedido" Culture="Auto" UICulture="Auto" Codebehind="IngresoPedido.aspx.cs" %>
<%@ Register Src="~/sidebar.ascx" TagName="sidebar" TagPrefix="crm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .UpdateProgressContent{
            padding: 40px;
            border: 1px dashed #C0C0C0;
            background-color: #FFFFFF;
            width: 400px;
            text-align: center;
            vertical-align: bottom;
            z-index: 1001;
            top: 34%;
            margin:0px;
            margin-left:-245px;
            position: absolute;
        }
    .UpdateProgressBackground
    {
        margin:0px;
        padding:0px;
        top:0px; bottom:0px; left:0px; right:0px;
        position:absolute;
        z-index:1000;
        background-color:#cccccc;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="maincol" > 
    <section> 
        <h2>INFORMACIÓN DEL TICKET (Campos Obligatorios)</h2>
        <div class="formHolder">
            <label>
                <span>CLIENTE</span>         
                <ajax:combobox ID="cbClientes" runat="server"
                    AutoPostBack="false"
                    Width="250"
                    DropDownStyle="DropDownList"
                    AutoCompleteMode="SuggestAppend"
                    CaseSensitive="False"
                    CssClass=""
                    ItemInsertLocation="Append" 
                    AppendDataBoundItems="true"
                    Visible="false">
                    <asp:ListItem Text="Seleccione un Cliente..." Value="0" />
                </ajax:combobox>

                <asp:TextBox ID="txtClientes" runat="server" onkeypress="copiar(this.value)"></asp:TextBox>
                <asp:LinkButton ID="btnNuevoCliente" runat="server" CommandName="Update" class="saveBtn" Text="Actualizar" CausesValidation="true" ValidationGroup="rfvEditMail"/>
                <Ajax:AutoCompleteExtender ID="buscar_AutoCompleteExtender" runat="server" 
                    enabled="True"
                    servicepath="MyAutocompleteService.asmx" 
                    servicemethod="GetSuggestions" 
                    minimumprefixlength="2"                            
                    enablecaching="true" 
                    targetcontrolid="txtClientes" 
                    usecontextkey="True" 
                    completionsetcount="10"                                
                    completioninterval="200">                     
                </Ajax:AutoCompleteExtender>
            </label>
          
            <label>
                <span>TITULO</span>
                <asp:TextBox ID="txtTitulo" runat="server" CssClass="textbox" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFV2" runat="server" Display="None" 
                ErrorMessage="Por favor, ingrese un titulo." ControlToValidate="txtTitulo" 
                ValidationGroup="ValidarPedido"></asp:RequiredFieldValidator>
            </label> 
                   
            <label ><span>CATEGORIA</span>
                <asp:DropDownList ID="cbCategoria" runat="server">
                    <asp:ListItem Text="Seleccione una Categoría..." Value="0" />
                </asp:DropDownList>
            </label>
                                    
            <label class="rigthLabel">
                <span>DESCRIPCION</span>
                <asp:TextBox ID="txtDescripcion" Rows="3" CssClass="textbox" TextMode="MultiLine" runat="server" ></asp:TextBox>      
                <asp:RequiredFieldValidator ID="RFV3" runat="server" Display="None" 
                    ErrorMessage="Por favor, ingrese una descripción." 
                ControlToValidate="txtDescripcion" ValidationGroup="ValidarPedido"></asp:RequiredFieldValidator>
            </label>    
            <label><span>PRIORIDAD</span>
            <asp:DropDownList ID="cbPrioridad" runat="server"></asp:DropDownList> </label>
        </div>

        <div>
            <asp:Panel ID="pnlNuevoCliente" runat="server" Width="410px" HorizontalAlign="Center" CssClass="ModalPopup">
                <table width="100%">
                    <asp:Label ID="lblClose1" Text="X" runat="server" CssClass="closebtn"></asp:Label>
                    <tr>
                        <td colspan="2" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:center;"><b>NUEVO CLIENTE</b></td>
                    </tr>
                    <tr>
                        <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">NOMBRE:</td>
                        <td width="75%" align="left"><asp:TextBox ID="txtNombreCliente" runat="server" CssClass="textbox" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">EMPRESA:</td>
                        <td width="75%" align="left"><asp:DropDownList ID="cbEmpresa" Width="214px" runat="server" ><asp:ListItem Text="Todas" Value="0" /></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">INTERNO:</td>
                        <td width="75%" align="left"><asp:TextBox ID="txtInternoCliente" runat="server" CssClass="textbox" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">MAIL:</td>
                        <td width="75%" align="left"><asp:TextBox ID="txtMailCliente" runat="server" CssClass="textbox" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="padding-left:71px">
                            <asp:Button ID="btnAceptarCliente" runat="server" Text="Aceptar" onclick="btnAceptarCliente_Click"/>                                                             
                        </td>
                    </tr>                 
                </table>
            </asp:Panel>
            <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
                TargetControlID="btnNuevoCliente"
                PopupControlID="pnlNuevoCliente" 
                CancelControlID="lblClose1"
                BackgroundCssClass="ModalBackground"
                DropShadow="true" />
        </div>
    </section>
    
    <section>                
        <h2>INFORMACIÓN ADICIONAL</h2>
        <div class="formHolder">
            <label><span>FECHA LIMITE</span>
            <asp:TextBox ID="txtFecha" runat="server"></asp:TextBox></label>

            <label class="rigthLabel"><asp:Label ID="lbInfoAdicionalMensaje" runat="server">MENSAJE</asp:Label>
            <asp:TextBox ID="txtMensajeResponsable" CssClass="textbox" TextMode="MultiLine" runat="server"></asp:TextBox></label>
        
            <label>
                <asp:Label ID="lbInfoAdicionalResponsable" runat="server">RESPONSABLE</asp:Label>           
                <asp:DropDownList ID="cbResponsable" runat="server">
                <asp:ListItem Text="Seleccione un Responsable..." Value="0" />
               </asp:DropDownList>
            </label>
        </div>
        <div class="formHolder">
            <label class="rigthLabel">
                <asp:Button ID="btnCargar" Text="Cargar" class="formBtnNar" runat="server" 
                onclick="btnCargar_Click" ValidationGroup="ValidarPedido" />
            </label>
        </div>
    </section>
</div>
    <crm:sidebar ID="Sidebar1" runat="server" />
</asp:Content>

