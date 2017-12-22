<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="DetalleCuota.aspx.cs" Inherits="crm.DetalleCuota" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="headOptions">
        <h2>Detalle de pago</h2>
    </div>

    <div class="formHolder">
        <asp:HiddenField ID="hfCuota" runat="server" />
        <p class=""><strong style="width:150px">OBRA:</strong><span><asp:Label ID="lblProyecto" runat="server"></asp:Label></span></p>
        <p class=""><strong style="width:150px">CLIENTE:</strong><span><asp:Label ID="lblCliente" runat="server"></asp:Label></span></p>
        <p class=""><strong style="width:150px">TOTAL:</strong><span><asp:Label ID="lblTotal" runat="server"></asp:Label></span></p>
        <p class=""><strong style="width:150px">SALDO:</strong><span><asp:Label ID="lblSaldo" runat="server"></asp:Label></span></p>
        <p class=""><strong style="width:150px">FORMA DE PAGO:</strong><span><asp:Label ID="lblFormaPago" runat="server"></asp:Label></span> </p>
        <asp:Panel ID="pnlUnPago" runat="server" Visible="false">
            <p class=""><strong style="width:150px">FECHA:</strong><span><asp:Label ID="lblFecha" runat="server"></asp:Label></span> </p>
            <p class=""><strong style="width:150px">MONTO AJUSTADO:</strong><span><asp:Label ID="lblMontoAjustado" runat="server"></asp:Label></span> </p>
            <p class=""><strong style="width:150px">VARIACIÓN CAC:</strong><span><asp:Label ID="lblVariacionCAC" runat="server"></asp:Label> <asp:Label ID="lbMensajeCAC" runat="server" CssClass="tituloMensaje" Text="No se encuentra actualizado el índice CAC" Visible="false"></asp:Label></span></p>
            <p class=""><strong style="width:150px">GASTOS ADTVO.:</strong><span><asp:Label ID="lblComision" runat="server"></asp:Label></span> </p>
            <p class=""><strong style="width:150px">ESTADO:</strong><span><asp:Label ID="lblEstado" runat="server"></asp:Label></span> </p>
            <p class=""><strong style="width:150px">RECIBO:</strong><span><asp:Label ID="lblRcibo" runat="server"></asp:Label></span> </p>
            <p class="">
                <label style="margin-left:-18%">
                    <asp:Button ID="btnAnularPago" runat="server" class="formBtnNar" Text="Anular Pago" OnClick="btnAnularPago_Click" />
                    <asp:Button ID="btnImprimir" runat="server" class="formBtnNar" Text="Imprimir recibo" OnClick="btnImprimir_Click" style="margin-left:5px; margin-right:5px" />
                    <asp:Button ID="btnAdjuntar" runat="server" class="formBtnNar" Text="Adjuntar" style="margin-left:5px;" OnClick="btnAdjuntar_Click" />
                    <asp:Button ID="btnPago" runat="server" class="formBtnNar" Text="Pago" OnClick="btnPago_Click" />
                </label>
            </p>
        </asp:Panel>
        <p></p>                

        <asp:Panel ID="pnlCuotas" runat="server" Visible="false">
        
        <p class=""><strong>CUOTAS:</strong></p>
        <asp:ListView ID="lvCuotas" runat="server" OnItemCommand="lvClientes_ItemCommand">
            <LayoutTemplate>
                <section>
                    <table style="margin-top:-25px">
                        <thead id="tableHead">
                            <tr>
                                <td></td>
                                <td>FECHA</td>
                                <td>MONTO</td>
                                <td>MONTO AJUSTADO</td>
                                <td>VARIACIÓN CAC</td>
                                <td style="width:6%">COMISIÓN</td>
                                <td>ESTADO</td>
                                <td>RECIBO</td>
                                <td style="width:16%"></td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </tbody>
                    </table>
                </section>
            </LayoutTemplate>
            <ItemTemplate>
                <tr onclick='Visible(<%# Eval("id")%> )'>
                    <td align="left">
                        Cuota&nbsp; <asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("nro") %>' />
                    </td>
                    <td align="left">
                        <asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>' />
                    </td>
                    <td align="left">
                        $&nbsp;<asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("monto") %>' />
                    </td>
                    <td align="left">
                        $&nbsp;<asp:Label ID="lbMontoAjustado" runat="Server" Text='<%#Eval("montoAjustado") %>' />
                    </td>
                    <td align="left">
                        <asp:Label ID="lbCAC" runat="Server" Text='<%#Eval("variacionCAC") %>' />&nbsp;%
                    </td>
                    <td align="left">
                        <asp:Label ID="lbComision" runat="Server" Text='<%#Eval("Comision") %>' />&nbsp;%
                    </td>
                    <td align="left">
                        <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>' />
                    </td>
                    <td align="left">
                        <asp:Label ID="lbRecibo" runat="Server" Text='<%#Eval("GetRecibo") %>' />
                    </td>
                    <td>
                        <asp:LinkButton ID="btnAnularCuota" runat="server" class="cancelBtn" CommandName="Anular" Text="Anular pago" ToolTip="Anular pago" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'/>
                        <asp:LinkButton ID="btnComentarioCuota" runat="server" class="saveComment" CommandName="Comentario" Text="Comentario" ToolTip="Agregar comentario" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'/>
                        <asp:LinkButton ID="btnImprimirCuota" runat="server" class="savePrint" CommandName="Imprimir" Text="Imprimir recibo" ToolTip="Imprimir recibo" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'/>
                        <asp:LinkButton ID="btnAdjuntarCuota" runat="server" class="saveAttach" CommandName="Adjunto" Text="Imagen" ToolTip="Adjunto" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'/>
                        <asp:LinkButton ID="btnPagoCuota" runat="server" class="saveBtn" CommandName="Pago" Text="Pago" ToolTip="Pago" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'/>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        </asp:Panel>
        
        <label style="width:100%">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <asp:Repeater ID="rptComentarios" runat="server">
                        <HeaderTemplate>                                
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <b><%#Eval("Descripcion") %>. &nbsp;
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

        <asp:Panel ID="pnlComentario" CssClass="comments" runat="server" DefaultButton="btnAgregarComentario">
            <label>
                <asp:TextBox ID="txtComentario" runat="server" style="width:100%"></asp:TextBox>
                <ajax:TextBoxWatermarkExtender ID="txtWater" runat="server" TargetControlID="txtComentario" WatermarkText="Ingrese un comentario..." WatermarkCssClass="watermarked2" /> 
                <asp:Button ID="tempo" runat="server" style="display:none" onclick="tempo_Click" />
            </label>
            <label>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnAgregarComentario" runat="server" CssClass="formBtnNar" Text="Agregar" UseSubmitBehavior="true" onclick="btnAgregarComentario_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>                                                       
            </label>
        </asp:Panel>                 
    </div>     
      
    <asp:Panel ID="pnlCuota" runat="server" HorizontalAlign="Center" CssClass="ModalPopup" style="background-color:white; width:500px">
        <table width="100%">               
            <asp:Label ID="lblClose" Text="X" runat="server" CssClass="closebtn"></asp:Label>           
            <tr>
                <td colspan="2" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:center;"><b>Comentarios</b></td>
            </tr> 
            <tr>
                <td colspan="2">
                        <label style="width:100%; height:50%">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Repeater ID="rptComentariosCuota" runat="server">
                                        <HeaderTemplate></HeaderTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <b><%#Eval("Descripcion") %>. &nbsp;
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

                        <asp:Panel ID="pnlComentarioCuota" CssClass="comments" runat="server" DefaultButton="btnAgregarComentarioCuota">
                        <label>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" style="width:162%">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtComentarioCuota" runat="server" style="width:50%"></asp:TextBox>
                                    <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtComentarioCuota" WatermarkText="Ingrese un comentario..." WatermarkCssClass="watermarked2" /> 
                                    <asp:Button ID="Button1" runat="server" style="display:none" onclick="tempo_Click" />
                                    <asp:Button ID="btnAgregarComentarioCuota" runat="server" CssClass="formBtnNar" Text="Agregar" UseSubmitBehavior="true" onclick="btnAgregarComentario_Click"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>                                                       
                        </label>
                    </asp:Panel>
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

    <asp:Panel ID="pnlImagen" runat="server" HorizontalAlign="Center" CssClass="ModalPopup" style="background-color:white; width:500px; margin-left:-253px">
        <table width="100%">               
            <asp:Label ID="lblClose1" Text="X" runat="server" CssClass="closebtn" style="right:-285px"></asp:Label>           
            <tr>
                <td colspan="2" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:center;"><b>Adjuntar archivo</b></td>
            </tr> 
            <tr>
                <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                    Descripción
                </td>
                <td>
                    <asp:TextBox ID="txtDescripcion" runat="server" Width="430px"></asp:TextBox>
                </td>
            </tr> 
            <tr>
                <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                    Imagen
                </td>
                <td>
                    <asp:FileUpload ID="fileArchivo" runat="server" style="width:430px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                     <img ID="imgUploaded" runat="server"  alt="imagen" src="~/images/notFound.jpg" style="height: 550px; width: 750px"/>
                </td>
            </tr>               
            <tr>   
                <td colspan="2">
                    <div style="float:left">
                        <asp:Button ID="btnVerImagen" runat="server" Text="Ver imagen"  OnClick="btnVerImagen_Click"/>
                        <asp:Label ID="lbMensajeAdjunto" runat="server" CssClass="tituloMensaje"></asp:Label>
                    </div>
                    <div align="right">
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click"  />
                        <asp:Button ID="btnCargarImagen" runat="server" class="formBtnNar1" Text="Guardar" OnClick="btnCargarImagen_Click" style="margin-left:20px"  />
                    </div>
                </td>           
            </tr>
        </table>
    </asp:Panel>   
    <asp:HiddenField ID="HiddenField2" runat="server" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
        TargetControlID="HiddenField2"
        PopupControlID="pnlImagen" 
        CancelControlID="lblClose1"        
        BackgroundCssClass="ModalBackground"
        DropShadow="true" /> 
</section>

<CR:CrystalReportSource ID="CrystalReportSource" runat="server" Visible="false">
    <Report FileName="Reportes/Recibo.rpt"></Report>
</CR:CrystalReportSource>
</asp:Content>
