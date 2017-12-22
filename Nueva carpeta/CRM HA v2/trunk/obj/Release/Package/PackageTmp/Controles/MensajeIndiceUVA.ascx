<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MensajeIndiceUVA.ascx.cs" Inherits="crm.Controles.MensajeIndiceUVA" %>
<section>         
    <div id="Panel1" class="formHolderMessage" style="margin-bottom: 0px; padding-bottom: 12px;">		
        <div align="center" style="width:100%; margin-bottom:0px;">
            <h2> Índice UVA </h2>
            <div style="float:left; margin-top: -1%; width: 27%; text-align: right;"><img id="Image1" src="images/Warning.png" style="height:70px; width:70px; border-width:0px;"></div>
            <div style="float:right; width: 72%; text-align: left;">
                <span style="color:Red; font-size:Medium; font-weight:bold;">
                    <asp:Label runat="server">Se informa que en el lapso de un mes se vence el servicio para obtener la cotización del UVA.</asp:Label>
                    <br />
                    <asp:Label runat="server">Para renovar el servicio se ingresa al siguiente sitio: <font style="font-style: italic; font-weight: normal;">http://estadisticasbcra.com/api/registracion</font></asp:Label>
                    <br />
                    Para registrar en el sistema, la actualización del servicio ingrese <a href="Configuracion.aspx">aquí</a>.
                </span>
            </div>
        </div>    
	</div> 
</section>