<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="DetalleCuota2.aspx.cs" Inherits="crm.DetalleCuota2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Controles/Cuotas.ascx" TagPrefix="crm" TagName="Cuotas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="JavaScript">
        var fila = '';
        function Visible(__id) {
            if (fila != '') {
                document.getElementById('fila' + fila).className = '';
                document.getElementById('fila' + fila).className = 'invisible';
            }
            if (fila != __id) {
                fila = __id;
                document.getElementById('fila' + fila).className = '';
            }
            else
                fila = '';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
<%--<asp:Panel ID="pnlPrincipal" runat="server">--%>
<section>
    <div class="headOptions">
        <h2>Detalle de pago</h2>
    </div>

    <div id="divEncabezado" runat="server" class="formHolder" style="padding-top: 6px; padding-bottom: 12px; margin-bottom: -48px !important;">
        <asp:HiddenField ID="hfCC" runat="server" />
        <asp:HiddenField ID="hfCuota" runat="server" /> 
    
        <div style="float:left;width: 48%;"> 
            <p class="" style="border-top-style: inherit;">
                <strong style="width:150px">CLIENTE:</strong>
                <span style="width: 400px;">
                    <asp:Label ID="lblCliente" runat="server"></asp:Label>
                </span>
            </p>            
            <p class="">
                <strong style="width:150px">OBRA:</strong>
                <span style="width: 70%;"><asp:Label ID="lblProyecto" runat="server"></asp:Label></span>
            </p>
            <p class="">
                <strong style="width:150px">MONEDA ACORDADA:</strong>
                <span><asp:Label ID="lblMonedaAcordada" runat="server"></asp:Label></span>
            </p>
            <p class="">
                <strong style="width:150px">SALDO AJUSTADO AL PRIMER MES ADEUDADO SIN GASTOS (Dólar):</strong>
                <span><asp:Label ID="lblSaldoDolar" runat="server"></asp:Label></span>
            </p>
            
        </div>
        <div style="float:right;width: 50%;">
            <p class="" style="border-top-style: inherit;">
                <strong style="width:150px">OPERACIÓN DE VENTA:</strong>
                <asp:HiddenField ID="hfIdOpv" runat="server" />
                <asp:LinkButton ID="btnOpv" runat="server" class="detailBtn" OnClick="btnOpv_Click" style="float:left !important"></asp:LinkButton>
            </p>
            <p class="">
                <strong style="width:150px">UNIDAD FUNCIONAL:</strong>
                <span style="width: 70%;"><asp:Label ID="lblUnidad" runat="server"></asp:Label></span>
            </p>
            <p class="">
                <strong style="width:150px">Dólar acordado:</strong>
                <span><asp:Label ID="lblDolar" runat="server"></asp:Label></span>
            </p>
            <p class="">
                <strong style="width:150px">SALDO AJUSTADO AL PRIMER MES ADEUDADO SIN GASTOS (Pesos):</strong>
                <span><asp:Label ID="lblSaldoPesos" runat="server"></asp:Label></span>
            </p>
        </div>

        <p class="" style="border-bottom-style: inherit;">
            <strong style="width:150px">ESTADO:</strong>
            <span style="width: 400px;">
                <asp:Label ID="lblEstado" runat="server"></asp:Label>
            </span>
        </p> 
    </div> 
    
    <asp:Panel ID="pnlListView" runat="server" Visible="true" style="margin-top: 2%;">           
    </asp:Panel>        
</section> 
              
<section>
    <div runat="server">
        <asp:Panel runat="server" ID="pnlListViewCuotas">
            <crm:Cuotas runat="server" id="usrCtrl" />
        </asp:Panel>
    </div>
</section>
<%--</asp:Panel>--%>
</asp:Content>
