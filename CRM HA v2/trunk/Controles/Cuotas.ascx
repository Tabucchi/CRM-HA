<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Cuotas.ascx.cs" Inherits="crm.Controles.Cuotas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<link href="../css/masterStyle.css" rel="stylesheet" />

<asp:HiddenField ID="hfCC" runat="server" />
<asp:HiddenField ID="hfIndice" runat="server" />
<asp:HiddenField ID="hfCuota" runat="server" />

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

<asp:Panel ID="pnlHead" runat="server" style="margin-top: 60px;" Visible="false">
    <div  class="formHolder" id="searchBoxTop">
        <div class="headOptions headLine">
            <div style="float: left; margin-top: 7px;">
                <b style="font-size: 16px; font-family: Open Sans Condensed; font-weight: bold; letter-spacing: 0px; color: #666;">Forma de pago</b>
                <asp:Label ID="lbNroFormaPago" runat="server" style="font-size: 18px;"></asp:Label>
            </div>
            <div style="float: left; margin-top: 7px; margin-left: 14px;">
                <b style="font-size: 16px; font-family: Open Sans Condensed; font-weight: bold; letter-spacing: 0px; color: #666;">Moneda:</b>
                <asp:Label ID="lbMoneda" runat="server" style="font-size: 18px;"></asp:Label>
            </div>
            <div style="float: left; margin-top: 7px; margin-left: 14px;">
                <b style="font-size: 16px; font-family: Open Sans Condensed; font-weight: bold; letter-spacing: 0px; color: #666;">Cuotas pendientes:</b>
                <asp:Label ID="lbCuotasPendientes" runat="server" style="font-size: 18px;"></asp:Label>
            </div> 
        </div>
        
        <div style="width: 100%;">
            <asp:ListView ID="lvCuotas" runat="server" OnItemCommand="lvCuotas_ItemCommand">
                <LayoutTemplate>
                    <section>
                        <table style="margin-top:-12px">
                            <thead id="tableHead">
                                <tr>
                                    <td></td>
                                    <td style="text-align: center"><asp:Label ID="lbIndice" runat="server"></asp:Label>&nbsp;(%)</td>
                                    <td style="width: 10%; text-align: center">SALDO AJUSTADO</td>
                                    <td style="text-align: center">MONTO</td>
                                    <td style="width: 8%; text-align: center">GASTOS ADTVO.</td>
                                    <td style="width: 8%; text-align: center">1re Venc.</td>
                                    <td style="text-align: center">IMPORTE</td>
                                    <td style="width: 8%;">2do Venc.</td>
                                    <td style="text-align: center">IMPORTE</td>
                                    <td style="text-align: center">ESTADO</td>
                                    <td style="text-align: center">RECIBO</td>
                                    <td style="text-align: center">SALDO</td>
                                    <td style="width: 0%;"></td>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </tbody>
                        </table>
                    </section>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr  style="cursor: pointer">
                        <td style="text-align: center">
                            <asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("nro") %>' />
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lbCAC" runat="Server" Text='<%#Eval("GetIndice") %>' />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lbSaldoPendiente" runat="Server" Text='<%#Eval("GetMontoAjustado") %>' />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto1") %>' />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lbComision" runat="Server" Text='<%#Eval("GetTotalComision") %>' />
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lbFecha1venc" runat="Server" Text='<%#Eval("FechaVencimiento1", "{0:dd/MM/yyyy}") %>' />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lbVencimiento1" runat="Server" Text='<%#Eval("GetVencimiento1") %>' />
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lbFecha2venc" runat="Server" Text='<%#Eval("FechaVencimiento2", "{0:dd/MM/yyyy}") %>' />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lbMontoAjustado" runat="Server" Text='<%#Eval("GetVencimiento2") %>' />
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>' />
                        </td>
                        <%--<td style="text-align: center; background-repeat: no-repeat; background-size: 40px;" class="reciboBtn" onclick='Visible(<%# Eval("id")%> )' title="Recibos emitidos">--%>
                        <td>
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>' class="reciboBtn" CommandName="Recibos" Text="Recibos" ToolTip="Ver recibos" />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label1" runat="Server" Text='<%#Eval("GetSaldo") %>' />
                        </td>
                        <td>                            
                            <asp:LinkButton ID="btnComentarioCuota" runat="server" CssClass="saveComment" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>' CommandName="Comentario" Text="Comentario" ToolTip="Agregar comentario" />
                        </td>
                    </tr>
                    <tr style="background-color:#F2F2F2">
                        <td id='fila<%# Eval("id")%>' class="invisible" colspan="13" style="height: 50px">
                            <table cellpadding="5px" style="padding:10px; width: 100%">
                                <tr>
                                    <td colspan="3">
                                        <h3>Recibos</h3>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ListView ID="lvRecibos1" runat="server" DataSource='<%#Eval("GetRecibos") %>' OnItemCommand="lvRecibos_ItemCommand"> 
                                            <LayoutTemplate>
                                                <section>
                                                    <table style="margin-top:-25px">
                                                        <thead id="tableHead">
                                                            <tr>
                                                                <td style="width: 6%; text-align: center">NRO.</td>
                                                                <td style="width: 4%; text-align: center">FECHA</td>
                                                                <td style="width: 6%; text-align: center">IMPORTE</td>
                                                                <td style="width: 1%;"></td>
                                                            </tr>
                                                        </thead>
                                                        <tbody style="height:80px; overflow:scroll">
                                                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                        </tbody> 
                                                    </table>
                                                </section>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer;">
                                                    <td style="text-align: center">
                                                        <asp:Label ID="lbNro" runat="Server" Text='<%#Eval("Nro") %>'/>
                                                    </td>  
                                                    <td style="text-align: center">
                                                        <asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>'/>
                                                    </td>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto") %>'/>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="btnImprimir" runat="server" class="savePrint" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdItemCCU")%>' Text="Imprimir recibo" ToolTip="Imprimir recibo" />
                                                    </td>                   
                                                </tr>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <section>
                                                    <table style="width:100%" runat="server">
                                                        <tr>
                                                            <td style="text-align:center"><b>No se encontraron Recibos.<b/></td>
                                                        </tr>
                                                    </table>
                                                </section>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</asp:Panel>

<asp:Panel ID="pnlCuota" runat="server" HorizontalAlign="Center" CssClass="ModalPopup" style="background-color:white; width:800px; top: -200px;">
    <table style="width: 100%">               
        <asp:Label ID="lblClose" Text="X" runat="server" CssClass="closebtn"></asp:Label>           
        <tr>
            <td colspan="2" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:center;"><h2>Comentarios</h2></td>
        </tr> 
        <tr>
            <td colspan="2">
                <label style="width:100%;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Repeater ID="rptComentariosCuota" runat="server">
                                <HeaderTemplate></HeaderTemplate>
                                <ItemTemplate>
                                    <li style="text-align: left; margin-bottom:8px">
                                        <b style="margin-left: -16px;"><%#Eval("Descripcion") %>. &nbsp;
                                        <br />
                                        <b><%#Eval("Fecha", "{0:dddd}")%>, <%#Eval("Fecha", "{0:dd}")%> de <%#Eval("Fecha", "{0:MMMM}")%> de <%#Eval("Fecha", "{0:yyyy}")%> a las <%#Eval("Fecha", "{0:hh:mm tt}")%></b>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                                <SeparatorTemplate></SeparatorTemplate>
                            </asp:Repeater>
                            </ContentTemplate>
                    </asp:UpdatePanel>   
                </label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="pnlComentarioCuota" CssClass="comments" runat="server" DefaultButton="btnAgregarComentarioCuota">
                    <label>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" style="width:100%">
                            <ContentTemplate>
                                <div>
                                    <asp:TextBox ID="txtComentarioCuota" runat="server" style="width:98%" TextMode="MultiLine"></asp:TextBox>
                                    <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtComentarioCuota" WatermarkText="Ingrese un comentario..." WatermarkCssClass="watermarked2" /> 
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>                                                       
                    </label>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    <asp:Button ID="Button1" runat="server" style="display:none" onclick="tempo_Click" />
                    <asp:Button ID="btnAgregarComentarioCuota" runat="server" CssClass="formBtnNar" Text="Agregar" UseSubmitBehavior="true" onclick="btnAgregarComentario_Click"/>
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>   
<asp:HiddenField ID="HiddenField1" runat="server" />
<ajax:ModalPopupExtender ID="ModalPopupExtender" runat="server" 
    TargetControlID="HiddenField1"
    PopupControlID="pnlCuota" 
    CancelControlID="lblClose"        
    BackgroundCssClass="ModalBackground"
    DropShadow="true" />


<asp:Panel ID="pnlRecibos" runat="server" HorizontalAlign="Center" CssClass="modal" style="background-color:white; width:800px; top: -200px;">

                <table width="100%">               
                    <tr>
                        <td style="float:left"><modalTitle style="text-align:left"><b>Recibos</b></modalTitle></td>
                        <td style="float:right"><modalTitle style="text-align:left">Nro. Cuota:&nbsp;<asp:Label Id="lbNroCuota" runat="server"/></modalTitle></td>
                    </tr> 
                    <tr>
                        <td colspan="2">
                            <asp:ListView ID="lvRecibos" runat="server" OnItemCommand="lvRecibos_ItemCommand"> 
                                            <LayoutTemplate>
                                                <section style="margin-bottom: -17px;">
                                                    <table>
                                                        <thead id="tableHead">
                                                            <tr>
                                                                <td style="width: 6%; text-align: center">NRO.</td>
                                                                <td style="width: 4%; text-align: center">FECHA</td>
                                                                <td style="width: 6%; text-align: center">IMPORTE</td>
                                                                <td style="width: 1%;"></td>
                                                            </tr>
                                                        </thead>
                                                        <tbody style="height:80px; overflow:scroll">
                                                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                        </tbody> 
                                                    </table>
                                                </section>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer;">
                                                    <td style="text-align: center">
                                                        <asp:Label ID="lbNro" runat="Server" Text='<%#Eval("Nro") %>'/>
                                                    </td>  
                                                    <td style="text-align: center">
                                                        <asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>'/>
                                                    </td>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto") %>'/>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="btnImprimir" runat="server" class="savePrint" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdItemCCU")%>' Text="Imprimir recibo" ToolTip="Imprimir recibo" />
                                                    </td>                   
                                                </tr>
                                               

                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <section>
                                                    <table style="width:100%" runat="server">
                                                        <tr>
                                                            <td style="text-align:center"><b>No se encontraron Recibos.<b/></td>
                                                        </tr>
                                                    </table>
                                                </section>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                        </td>
                    </tr>
                     <tr>    
                                                        <td style="float:right">
                                                            <asp:Button ID="btnCerrarRecibos" runat="server" CssClass="btnClose" Text="Cerrar" OnClick="btnCerrarRecibos_Click" />
                                                        </td>
                                                </tr> 
                </table>

        </asp:Panel>   
        <asp:HiddenField ID="hfRecibos" runat="server" />
        <ajax:ModalPopupExtender ID="modalRecibos" runat="server" 
            TargetControlID="hfRecibos"
            PopupControlID="pnlRecibos" 
            BackgroundCssClass="ModalBackground"
            DropShadow="true" /> 


<CR:CrystalReportSource ID="CrystalReportSourceRecibo" runat="server" Visible="false">
    <Report FileName="Reportes/Recibo.rpt"></Report>
</CR:CrystalReportSource>