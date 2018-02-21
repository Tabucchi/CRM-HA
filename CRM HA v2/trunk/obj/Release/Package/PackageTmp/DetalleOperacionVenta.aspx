<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="DetalleOperacionVenta.aspx.cs" Inherits="crm.DetalleOperacionVenta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="css/orange.css" rel="stylesheet" />

<script type="text/javascript">
    var updateProgress = null;
    
    function postbackButtonClick() {
        updateProgress = $find("<%= UpdateProgress1.ClientID %>");
        window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
        return true;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>

    <div class="headOptions">
        <h2>Detalle de operación de venta</h2>
    </div>

    <div id="divEncabezado" runat="server" class="formHolder" style="padding-bottom: 10px;">   
        <%--<p class="">
            <strong style="width:150px">CLIENTE:</strong>
            <span style="width: 400px;">
                <asp:Label ID="lbCliente" runat="server"></asp:Label>
            </span>
        </p> --%>
        <div style="float:left;width: 48%;">
            <p class="">
                <strong style="width:150px">CLIENTE:</strong>
                <span style="width: 70%;"><asp:Label ID="lbCliente" runat="server" style="width: 100%;"></asp:Label></span>
            </p>            
            <p class="">
                <strong style="width:150px">MONEDA ACORDADA:</strong>
                <span><asp:Label ID="lbMonedaAcordada" runat="server"></asp:Label></span>
            </p>
            <p class="">
                <strong style="width:150px">PRECIO ACORDADA:</strong>
                <span><asp:Label ID="lbPrecioAcordado" runat="server"></asp:Label></span>
            </p>
            <p class="">
                <strong style="width:150px">ÍNDICE CAC:</strong>
                <span><asp:Label ID="lbIndiceCAC" runat="server"></asp:Label></span>
            </p>
            <p class="">
                <strong style="width:150px">VALOR DÓLAR:</strong>
                <span><asp:Label ID="lbValorDolar" runat="server"></asp:Label></span>
            </p>
        </div>
        <div style="float:right;width: 50%;">          
            <p>
                <strong style="width:150px">ESTADO:</strong>
                <span><asp:Label ID="lbEstado" runat="server"></asp:Label></span>
            </p>
            <p class="">
                <strong style="width:150px">ANTICIPO:</strong>
                <span><asp:Label ID="lbAnticipo" runat="server"></asp:Label></span>
            </p>
            <p class="">
                <strong style="width:150px">GASTOS ADTVO.:</strong>
                <span><asp:Label ID="lbGastosAdtvo" runat="server"></asp:Label></span>
            </p>
            <p class="">
                <strong style="width:150px">ÍNDICE UVA:</strong>
                <span><asp:Label ID="lbIndiceUVA" runat="server"></asp:Label></span>
            </p>
        </div>
    </div>
</section> 

<asp:UpdatePanel runat="server" ID="upanel2CAC"> 
    <ContentTemplate> 
        <section>
            <div id="div1" runat="server" class="formHolder" style="padding-bottom: 10px;"> 
                <div style="float:left;width: 48%;">
                    <p class="">
                        <strong style="width:150px">FECHA DEL BOLETO:</strong>
                        <span><asp:Label ID="lbFecha" runat="server"></asp:Label></span>
                    </p>                          
                    <p style="padding-bottom:0px !important">
                        <div style="float:left;margin-bottom: 8px;">
                            <strong style="width:150px; color: #b40b0b">FECHA ESCRITURA:</strong>
                        </div>

                        <div style="float:right">
                            <asp:Panel ID="pnlFechaEscritura" runat="server" style="width: 450px;">
                                <span>
                                    <asp:Label ID="lbFechaEscritura" runat="server" style="margin-left: 9%;"></asp:Label>
                                    <a href="#" alt="Editar fecha de escritura" class="tooltip tooltipColor">
                                        <asp:ImageButton ID="btnEditarFechaEscritura" runat="server" class="editBtn" style="margin-right: 67%;" onclick="btnEditarFechaEscritura_Click"/>
                                    </a>
                                </span>
                            </asp:Panel>
                
                            <asp:Panel ID="pnlEditFechaEscritura" runat="server" Visible="false" style="width: 410px; margin-top: -8px; ">
                                <asp:TextBox ID="txtFechaEscritura" runat="server" CssClass="textBox textBoxForm" style="width: 116px; margin-top: 2px;"></asp:TextBox>
                                <ajax:CalendarExtender ID="cefe" runat="server" CssClass="orange" TargetControlID="txtFechaEscritura" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                    

                                <a href="#" alt="Guardar" class="tooltip tooltipColor">
                                    <asp:LinkButton ID="btnSaveFechaEscritura" runat="server" class="saveBtn" style="margin-right: 62%; margin-top: 7px;" OnClick="btnSaveFechaEscritura_Click" />
                                </a>

                                <a href="#" alt="Cancelar" class="tooltip tooltipColor">
                                    <asp:LinkButton ID="btnCancelFechaEscritura" runat="server" class="cancelBtn" style="margin-right: 57%; margin-top: -23px;" OnClick="btnCancelFechaEscritura_Click"/>                    
                                </a>
                            </asp:Panel>
                        </div>
                    </p> 
                        
                </div>
                <div style="float:right;width: 50%;">  
                    <p style="padding-bottom:0px !important">
                        <div style="float:left;margin-bottom: 8px;">
                            <strong style="width:150px; color: #b40b0b">FECHA POSESIÓN:</strong>
                        </div>

                        <div style="float:right">
                            <asp:Panel ID="pnlFechaPosesion" runat="server" style="width: 450px;">
                                <span>
                                    <asp:Label ID="lbFechaPosesion" runat="server" style="margin-left: 5%;"></asp:Label>
                                    <a href="#" alt="Editar fecha de posesión" class="tooltip tooltipColor">
                                        <asp:ImageButton ID="btnEditarFechaPosesion" runat="server" class="editBtn" style="margin-right: 67%;" onclick="btnEditarFechaPosesion_Click"/>
                                    </a>
                                </span>
                            </asp:Panel>
                
                            <asp:Panel ID="pnlEditFechaPosesion" runat="server" Visible="false" style="width: 432px; margin-top: -8px; ">
                                <asp:TextBox ID="txtFechaPosesion" runat="server" CssClass="textBox textBoxForm" style="width: 116px; margin-top: 2px;"></asp:TextBox>
                                <ajax:CalendarExtender ID="cefp" runat="server" CssClass="orange" TargetControlID="txtFechaPosesion" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                    

                                <a href="#" alt="Guardar" class="tooltip tooltipColor">
                                    <asp:LinkButton ID="btnSaveFechaPosesion" runat="server" class="saveBtn" style="margin-right: 62%; margin-top: 7px;" OnClick="btnSaveFechaPosesion_Click" />
                                </a>
                                <a href="#" alt="Cancelar" class="tooltip tooltipColor">
                                    <asp:LinkButton ID="btnCancelFechaPosesion" runat="server" class="cancelBtn" style="margin-right: 57%; margin-top: -23px;" OnClick="btnCancelFechaPosesion_Click"/>    
                                </a>                                              
                            </asp:Panel>
                        </div>
                    </p>
                </div>
            </div>
        </section>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnEditarFechaPosesion" />
        <asp:PostBackTrigger ControlID="btnSaveFechaPosesion" />
        <asp:PostBackTrigger ControlID="btnCancelFechaPosesion" />
        <asp:PostBackTrigger ControlID="btnEditarFechaEscritura" />
        <asp:PostBackTrigger ControlID="btnSaveFechaEscritura" />
        <asp:PostBackTrigger ControlID="btnCancelFechaEscritura" />
    </Triggers>
</asp:UpdatePanel>
        
<section>
    <div runat="server" class="formHolder"> 
    <div>
        <div style="float:left">
            <h2>Unidades funcionales</h2>
        </div>
    </div>
    <asp:ListView ID="lvUnidadesFuncionales" runat="server">
        <LayoutTemplate>
            <table>
                <thead>
                    <tr>
                        <td style="text-align:center; width: 10%">COD U.F.</td>
                        <td style="text-align:center; width: 20%">OBRA</td>
                        <td style="text-align:center; width: 10%">TIPO UNIDAD</td>
                        <td style="text-align:center; width: 15%">NIVEL</td>
                        <td style="text-align:center; width: 15%">NRO. UNIDAD</td>
                        <td style="text-align:center; width: 15%; text-align: center">PRECIO BASE</td>
                        <td style="text-align:center; width: 15%; text-align: center">PRECIO ACORDADO</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </tbody>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr id='row<%# Eval("id")%>' style="color:#b40b0b;">
                <td style="text-align:center;">
                    <asp:Label ID="lbIdEmpresaUnidad" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                    <%# Eval("CodUF") %>
                </td>
                <td style="text-align:center;"><%# Eval("GetProyecto") %></td>
                <td style="text-align:center;"><%# Eval("GetTipoUnidad") %></td>
                <td style="text-align:center;"><%# Eval("GetNivel") %></td>
                <td style="text-align:center;"><%# Eval("GetNroUnidad") %></td>
                <td style="text-align: right"><%# Eval("GetPrecioBase") %></td>
                <td style="text-align: right"><%# Eval("GetPrecioAcordado") %></td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
    </div>
</section>    
        
<section>
    <div runat="server" class="formHolder"> 
    <div>
        <div style="float:left">
            <h2>Forma de pago</h2>
        </div>
    </div>
    <asp:ListView ID="lvFormaPago" runat="server">
        <LayoutTemplate>
            <table>
                <thead>
                    <tr>
                        <td style="width: 20%; text-align: center">MONEDA</td>
                        <td style="width: 20%; text-align: center">TOTAL</td>
                        <td style="width: 20%; text-align: center">CUOTAS</td>
                        <td style="width: 20%; text-align: center">VALOR</td>
                        <td style="width: 20%; text-align: center">FECHA VENCIMIENTO</td>
                        <td style="width: 20%; text-align: center">CUOTAS AJUSTADAS</td>
                        <td style="width: 20%; text-align: center">GASTOS ADTVO.</td>
                        <td style="width: 20%; text-align: center">INTERÉS ANUAL</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </tbody>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr id='row<%# Eval("id")%>' style="color:#b40b0b;">
                <td style="text-align: center"><asp:Label ID="lbMoneda" runat="Server" Text='<%#Eval("GetMoneda") %>' /></td>
                <td style="text-align: right"><asp:Label ID="lbTotal" runat="Server" Text='<%#Eval("GetAnticipo") %>' /></td>
                <td style="text-align: center"><asp:Label ID="lbCantCuotas" runat="Server" Text='<%#Eval("CantCuotas") %>' /></td>
                <td style="text-align: right"><asp:Label ID="lbMontoCuota" runat="Server" Text='<%#Eval("GetValor") %>' /></td>
                <td style="text-align: center"><asp:Label ID="lbFechaVenc" runat="Server" Text='<%#Eval("FechaVencimiento", "{0:d}") %>' /></td>
                <td style="text-align: center">
                    <asp:Label ID="lbRangoCuota" runat="Server" Text='<%#Eval("RangoCuotaCAC") %>' Visible="false" />
                    <asp:Label ID="lbRangoCuota1" runat="Server" Text='<%#Eval("GetRangoCuotaCAC") %>' />
                </td>
                <td style="text-align: center">
                    <asp:Label ID="lbGastosAdtvo" runat="Server" Text='<%#Eval("GastosAdtvo") %>' Visible="false"/>
                    <asp:Label ID="lbGastosAdtvo1" runat="Server" Text='<%#Eval("GetGastosAdtvo") %>' />
                </td>
                <td style="text-align: center">
                    <asp:Label ID="lbInteresAnual" runat="Server" Text='<%#Eval("GetInteresAnual") %>'/>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
        </div>
</section>         
   
<section>
    <asp:UpdatePanel ID="pnlFinalizarOV" runat="server">
        <ContentTemplate>
            <div runat="server" class="formHolder" style="padding: 22px 14px 12px;">
                <div runat="server">
                    <div style="float:left; width: 30%;">
                        <asp:Panel ID="pnlCC" runat="server" Visible="false"><label><asp:Button ID="btnCrearCC" Text="Crear Historial de Pagos" CssClass="formBtnNar" runat="server" style="float:left; margin-top: -4px; margin-left: 15px;" OnClick="btnCrearCC_Click"/></label></asp:Panel>
                        <asp:HiddenField ID="hfCC" runat="server" />
                        <asp:Panel ID="pnlLinkCC" runat="server" Visible="false">
                            <div style="float:left;"><label><asp:Button ID="btnLinkCC" Text="Ir a Historial de Pagos" CssClass="formBtnNar" runat="server" style="float:left; margin-top: -4px; margin-left: 15px;" OnClick="btnLinkCC_Click"/></label></div>
                        </asp:Panel>
                        <div style="float:left;"><label><asp:Button ID="btnVolver" Text="Volver" CssClass="formBtnGrey1" runat="server" OnClick="btnVolver_Click" style="float:left; margin-left:15px; margin-top: -4px;"/></label></div>
                    </div>
                    <div style="float:right">
                        <asp:Panel ID="pnlAnularOV" runat="server" Visible="true">
                            <asp:Button ID="btnAnularOV" Text="Anular Operación de Venta" CssClass="formBtnGrey1" runat="server" style="float:left; margin-right:15px; margin-top: -5px;" OnClick="btnAnularOV_Click"/>
                        </asp:Panel>
                    </div>                            
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAnularOV" />                    
        </Triggers>
    </asp:UpdatePanel>
</section>

<div style="float:left">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlFinalizarOV">
        <ProgressTemplate>
            <div class="overlay">
                <div class="overlayContent">
                    <div style="float:left;"><img src="images/ring_loading.gif"  width="300px" class="loading100" ImageAlign="left"  /></div>
                    <div style="float:left; padding: 8px 0 0 10px">
                        <h2> Procesando... </h2>
                    </div>                                    
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>

<section>
    <asp:UpdatePanel ID="pnlAnular" runat="server" HorizontalAlign="Center" class="modal" style="left: 26px; top: 24px !important; width: 500px;">
        <ContentTemplate>
            <table style="width: 100%">               
                <asp:Label ID="lblCloseDelete" Text="X" runat="server" CssClass="closebtn1" style="right: -11px !important;"></asp:Label>
                <tr>
                    <td colspan="2"><modalTitle><b>Eliminar Operación de Venta</b></modalTitle></td>
                </tr> 
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Label runat="server" style="font-size: 18px;" Text="¿Desea eliminar esta operación de venta?"></asp:Label>
                    </td>
                </tr>
                <tr class="spacer">
                    <td colspan="2" > <hr /> </td>
                </tr>
                <tr>            
                    <td>
                        <div align="center">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlAnular">
                                <ProgressTemplate>
                                    <script type="text/javascript">document.write("<div class='UpdateProgressBackground'></div>");</script>
                                    <center>
                                        <div class="UpdateProgressContent">
                                            <div style="float:left;"><img src="images/ring_loading.gif" style="height: 30px; width:30px" ImageAlign="left"  /></div>
                                            <div style="float:left; padding: 0px 0 0 10px">
                                                <font style="font-size: 25px; color: #B7B7B7; font-weight: bold;"> Procesando... </font>
                                            </div>                                    
                                        </div>
                                    </center>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                    </td>
                    <td>
                        <div style="float:right">
                            <asp:Button ID="btnSi" runat="server" Text="Si" style="padding: 12px; width: 50px; height: 40px; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; border: 1px solid #ccc;" OnClick="btnSi_Click"/>
                            <asp:Button ID="btnNo" runat="server" Text="No" style="margin-left:15px; padding: 12px; width: 50px; height: 40px; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; border: 1px solid #ccc; background-color:white" OnClick="btnNo_Click" />
                        </div>
                    </td>            
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>   
    <asp:HiddenField ID="hfAnular" runat="server" />
    <ajax:ModalPopupExtender ID="modalAnular" runat="server" 
        CancelControlID="lblCloseDelete"
        TargetControlID="hfAnular"
        PopupControlID="pnlAnular" 
        BackgroundCssClass="ModalBackground"
        DropShadow="true" /> 
</section>
</asp:Content>
