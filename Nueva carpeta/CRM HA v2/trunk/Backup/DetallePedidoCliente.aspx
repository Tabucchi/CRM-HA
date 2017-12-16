<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageCliente.master" AutoEventWireup="true" Inherits="DetallePedidoCliente" Codebehind="DetallePedidoCliente.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            margin-left:-645px;
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="headOptions">
            <h2><asp:Label ID="lblID" runat="server"></asp:Label> </h2>                               
            <div style="float:right; margin-right:10px;">
                <asp:LinkButton ID="btnNuevoCliente" runat="server" CommandName="Update" class="homeBtn" Text="Actualizar" onclick="btnNuevoCliente_Click"/>      
            </div>
    </div>

    <div class="formHolder">
        <p class=""><strong>FECHA DE CARGA:</strong><span><asp:Label ID="lblFechaCarga" runat="server"></asp:Label></span></p>
        <p class=""><strong>FECHA CIERRE:</strong><span><asp:Label ID="Label1" runat="server"></asp:Label></span></p>
        <p class=""><strong>ESTADO:</strong><span><asp:Label ID="lblEstado" runat="server"></asp:Label></span></p>
        <br /><br /><br /><br /><hr />
        <p class=""><strong>CLIENTE:</strong><span><asp:Label ID="lblCliente" runat="server"></asp:Label></span></p>
        <p class=""><strong>TITULO:</strong><span><asp:Label ID="lblTitulo" runat="server"></asp:Label></span></p>
        <p class=""><strong>CATEGORIA:</strong><span><asp:Label ID="lblCategoria" runat="server"></asp:Label></span></p>
        <p class=""><strong>PRIORIDAD:</strong><span><asp:Label ID="lblPrioridad" runat="server"></asp:Label></span></p>
        <p style="border-top: medium none; margin-top: 0; padding: 5px 0; width: 60%;"><strong>CERRADO POR:</strong><span> <asp:Label ID="lblFechaFinalizacion" runat="server"></asp:Label></span></p>             
        <p>COMENTARIOS</p> 
            <label style="width:100%">
            <asp:Repeater ID="rptComentarios" runat="server">
                <HeaderTemplate>                                
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <b><%#Eval("GetNombre_Autor") %>:</b> &nbsp; <%#Eval("Descripcion") %>. &nbsp;
                        <br />
                        <b><%#Eval("Fecha", "{0:dddd}")%>, <%#Eval("Fecha", "{0:dd}")%> de <%#Eval("Fecha", "{0:MMMM}")%> de <%#Eval("Fecha", "{0:yyyy}")%> a las <%#Eval("Fecha", "{0:hh:mm tt}")%></b>
                    </li>
                </ItemTemplate>
                <FooterTemplate></FooterTemplate>
                <SeparatorTemplate></SeparatorTemplate>
            </asp:Repeater>
            </label>
            <asp:Panel ID="pnlComentario" CssClass="comments" runat="server" DefaultButton="tempo" >
            <label style="width:88%">
                <asp:TextBox ID="txtComentario" runat="server" style="width:100%"></asp:TextBox>
                <ajax:TextBoxWatermarkExtender ID="txtWater" runat="server" TargetControlID="txtComentario" WatermarkText="Ingrese un comentario..." WatermarkCssClass="watermarked2" /> 
                <asp:Button ID="tempo" runat="server" style="display:none" onclick="tempo_Click" />                                   
            </label>
            <label style="width:10%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnAgregarComentario" runat="server" CssClass="formBtnNar" Text="Agregar" UseSubmitBehavior="true" onclick="btnAgregarComentario_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <script type="text/javascript">document.write("<div class='UpdateProgressBackground'></div>");</script>
                        <center>
                            <div class="UpdateProgressContent">
                                <div style="float:left; padding-left:82px"><img src="images/loading.gif"  width="300px" style="height: 100px; width:100px" ImageAlign="left"  /></div>
                                <div style="float:left; padding: 35px 0 0 10px">
                                    <font style="font-size: 25px; color: #B7B7B7; font-weight: bold;"> Procesando... </font>
                                </div>                                    
                                </div>
                        </center>
                    </ProgressTemplate>
                </asp:UpdateProgress>                                         
            </label>
            </asp:Panel>                    
    </div>        
                
    <div>
        <input type="hidden" id="txtId" runat="server" />
    </div>
</section>
</asp:Content>
