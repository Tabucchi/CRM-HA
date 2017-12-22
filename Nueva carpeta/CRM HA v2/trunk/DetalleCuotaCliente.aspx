<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="DetalleCuotaCliente.aspx.cs" Inherits="crm.DetalleCuotaCliente" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/orange.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="headOptions">
        <h2>Detalle de pago</h2>
    </div>

    <div class="formHolder">
        <asp:HiddenField ID="hfCC" runat="server" />
        <asp:HiddenField ID="hfCuota" runat="server" />        
        <p class=""><strong style="width:150px">CLIENTE:</strong><span><asp:Label ID="lblCliente" runat="server"></asp:Label></span></p>
        <p class=""><strong style="width:150px">OBRA:</strong><span><asp:Label ID="lblProyecto" runat="server"></asp:Label></span></p>
        <p class=""><strong style="width:150px">UNIDAD FUNCIONAL:</strong><span><asp:Label ID="lblUnidad" runat="server"></asp:Label></span></p>       
        <p class=""><strong style="width:150px">TOTAL:</strong><span><asp:Label ID="lblTotal" runat="server"></asp:Label></span></p>        
        <p class=""><strong style="width:150px">SALDO:</strong><span><asp:Label ID="lblSaldo" runat="server"></asp:Label></span></p> 
        <asp:Panel ID="pnlAnticipo" runat="server" Visible="false">
            <p class=""><strong style="width:150px">ANTICIPO:</strong><span><asp:Label ID="lblAnticipo" runat="server"></asp:Label></span> </p>
        </asp:Panel>   
        <p class=""><strong style="width:150px">FORMA DE PAGO:</strong><span><asp:Label ID="lblFormaPago" runat="server"></asp:Label></span> </p>
        <p class="">
            <label style="margin-left:-15%">
                <asp:Button ID="btnAdelanto" runat="server" class="formBtnNar" Text="Adelanto de cuotas" style="margin-left:5px;" OnClick="btnAdelanto_Click"/>
                <asp:Button ID="btnPago" runat="server" class="formBtnNar" Text="Ingresar Pago" style="margin-left:5px;" OnClick="btnPago_Click"/>
                <asp:Button ID="btnDatosPersonales" runat="server" class="formBtnNar" Text="Datos personales" OnClick="btnDatosPersonales_Click"/>
                <%-- <asp:Button ID="btnAdelanto" runat="server" class="formBtnNar" Text="Adelanto de cuotas" style="margin-left:5px;" OnClick="btnPago_Click" />
                <asp:Button ID="btnPago" runat="server" class="formBtnNar" Text="Ingresar Pago" style="margin-left:5px;" OnClick="btnPago_Click" />
                <asp:Button ID="btnDatosPersonales" runat="server" class="formBtnNar" Text="Datos personales" OnClick="btnDatosPersonales_Click"/>--%></label></p>
        
        <asp:Panel ID="pnlUnPago" runat="server" Visible="false">
            <p class=""><strong style="width:150px">FECHA:</strong><span><asp:Label ID="lblFecha" runat="server"></asp:Label></span> </p>            
            <p class=""><strong style="width:150px">VARIACIÓN CAC:</strong><span><asp:Label ID="lblVariacionCAC" runat="server"></asp:Label> <asp:Label ID="lbMensajeCAC" runat="server" CssClass="tituloMensaje" Text="No se encuentra actualizado el índice CAC" Visible="false"></asp:Label></span></p>
            <p class=""><strong style="width:150px">GASTOS ADTVO.:</strong><span><asp:Label ID="lblComision" runat="server"></asp:Label></span> </p>            
            <p class=""><strong style="width:150px">1° VENC.:</strong><span><asp:Label ID="lblVencimiento1" runat="server"></asp:Label></span></p>        
            <p class=""><strong style="width:150px">2° VENC.:</strong><span><asp:Label ID="lblVencimiento2" runat="server"></asp:Label></span></p>    
            <p class=""><strong style="width:150px">PUNITORIO:</strong><span><asp:Label ID="lblPunitorio" runat="server"></asp:Label></span></p> 
            <p class=""><strong style="width:150px">RECIBO:</strong><span><asp:Label ID="lblRcibo" runat="server"></asp:Label></span> </p>
            <p class=""><strong style="width:150px">ESTADO:</strong><span><asp:Label ID="lblEstado" runat="server"></asp:Label></span> </p>            
        </asp:Panel>
        <p></p>                

        <div style="margin-left:20px; margin-top:10px">
            <asp:Label ID="lbMensaje" runat="server" Text="Su pago ya fue registrado, en breve serán procesados por nuestros representantes." Visible="false"></asp:Label>
        </div>

        <asp:Panel ID="pnlCuotas" runat="server" Visible="false">
            <p class=""><h2>CUOTAS:</h2></p>
            <asp:ListView ID="lvCuotas" runat="server">
            <%--<asp:ListView ID="lvCuotas" runat="server" OnItemCommand="lvClientes_ItemCommand">--%>
                <LayoutTemplate>
                    <section>
                        <table style="margin-top:-25px">
                            <thead id="tableHead">
                                <tr>
                                    <td></td>
                                    <td>CAC</td>
                                    <td style="width: 11%;">SALDO AJUSTADO</td>
                                    <td>MONTO</td>
                                    <td style="width: 11%">GASTOS ADTVO.</td>
                                    <td style="width: 8%;">1re Venc.</td>
                                    <td>IMPORTE</td>
                                    <td style="width: 8%;">2do Venc.</td>
                                    <td>IMPORTE</td>
                                    <td>ESTADO</td>
                                    <td>SALDO</td>
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
                            <asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("nro") %>' />
                        </td>
                        <td align="left">
                            <asp:Label ID="lbCAC" runat="Server" Text='<%#Eval("variacionCAC") %>' />&nbsp;%
                        </td>
                        <td align="left">
                            <asp:Label ID="lbSaldoPendiente" runat="Server" Text='<%#Eval("montoAjustado") %>' />
                        </td>
                        <td align="left">
                            <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("monto") %>' />
                        </td>
                        <td align="left">
                            <asp:Label ID="lbComision" runat="Server" Text='<%#Eval("GetTotalComision") %>' />
                        </td>
                        <td align="left">
                            <asp:Label ID="lbFecha1venc" runat="Server" Text='<%#Eval("FechaVencimiento1", "{0:dd/MM/yyyy}") %>' />
                        </td>
                        <td align="left">
                            $&nbsp;<asp:Label ID="lbVencimiento1" runat="Server" Text='<%#Eval("vencimiento1") %>' />
                        </td>
                        <td align="left">
                            <asp:Label ID="lbFecha2venc" runat="Server" Text='<%#Eval("FechaVencimiento2", "{0:dd/MM/yyyy}") %>' />
                        </td>
                        <td align="left">
                            $&nbsp;<asp:Label ID="lbMontoAjustado" runat="Server" Text='<%#Eval("vencimiento2") %>' />
                        </td>
                        <td align="left">
                            <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>' />
                        </td>
                        <td align="left">
                            <asp:Label ID="Label1" runat="Server" Text='<%#Eval("Saldo") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </asp:Panel>              
    </div>     
    
    <asp:Panel ID="pnlAdvertencia" runat="server" HorizontalAlign="Center" style="background-color:white; width:500px;">
        <table width="100%">                        
            <tr>
                <td colspan="2" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:center;"><b>Advertencia</b></td>
            </tr> 
            <tr>
                <td colspan="2" style="color:black">
                    La variación del CAC para el pago de la cuota es de <b><asp:Label ID="lbAdvertenciaCac" runat="server"/> %</b>, correspondiente al mes actual.<br /><br />
                    ¿Desea continua?
                </td>
            </tr>              
            <tr>   
                <td colspan="2">
                    <div align="right">                        
                        <%--<asp:Button ID="btnAdvertenciaAceptar" runat="server" class="formBtnNar1" Text="Aceptar" OnClick="btnAdvertenciaAceptar_Click" />
                        <asp:Button ID="btnAdvertenciaCancelar" runat="server" Text="Cancelar" style="margin-left:20px" OnClick="btnAdvertenciaCancelar_Click" />--%>
                    </div>
                </td>           
            </tr>
        </table>
    </asp:Panel>   
    <asp:HiddenField ID="HiddenField3" runat="server" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
        TargetControlID="HiddenField3"
        PopupControlID="pnlAdvertencia"       
        BackgroundCssClass="ModalBackground"
        DropShadow="true" /> 

    <asp:Panel ID="pnlDatosPersonales" runat="server" HorizontalAlign="Center" style="background-color:white; width:500px;">
        <table width="100%">                        
            <tr>
                <td colspan="2" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:center;"><b>Datos Personales</b></td>
            </tr> 
            <tr>
                <td style="color:black">
                    <div align="right">
                        Nombre:
                    </div>
                </td>
                <td>
                    <b><asp:Label ID="lbDatosNombre" runat="server"></asp:Label></b>
                </td>
            </tr>              
            <tr>   
               <td align="right" style="color:black">
                   <div align="right">
                        Dirección:
                    </div>
                </td>
                <td>
                    <b><asp:Label ID="lbDatosDireccion" runat="server"></asp:Label></b>
                </td>           
            </tr>
            <tr>   
               <td align="right" style="color:black">
                   <div align="right"> 
                    Teléfono:
                   </div>
                </td>
                <td>
                    <b><asp:Label ID="lbDatosTelefono" runat="server"></asp:Label></b>
                </td>           
            </tr>
            <tr>   
               <td align="right" style="color:black">
                    <div align="right"> 
                        CUIT:
                    </div>
                </td>
                <td>
                    <b><asp:Label ID="lbDatosCuit" runat="server"></asp:Label></b>
                </td>           
            </tr>
            <tr>   
               <td align="right" style="color:black">
                    <div align="right"> 
                        Mail:
                    </div>
                </td>
                <td>
                    <b><asp:Label ID="lbMail" runat="server"></asp:Label></b>
                </td>           
            </tr>
            <tr>   
                <td colspan="2">
                    <div align="right">
                        <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" OnClick="btnCerrar_Click"  />
                    </div>
                </td>           
            </tr>
        </table>
    </asp:Panel>   
    <asp:HiddenField ID="HiddenField4" runat="server" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender3" runat="server" 
        TargetControlID="HiddenField4"
        PopupControlID="pnlDatosPersonales"       
        BackgroundCssClass="ModalBackground"
        DropShadow="true" /> 
 
    <asp:Panel ID="pnlRegistrarPago" runat="server" HorizontalAlign="Center" style="background-color:white; width:100%;">
        <table width="100%">                        
            <tr>
                <td colspan="4" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:center;"><b>Registrar Pago</b></td>
            </tr> 
            <tr>
                <td style="color:black">
                    <div align="right">
                        Fecha de pago:
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="txtRegistroFecha" runat="server" style="width:305px" TabIndex="1"></asp:TextBox>
                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="orange" TargetControlID="txtRegistroFecha" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />
                </td> 
               <td align="right" style="color:black">
                   <div align="right">
                        Monto:
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="txtRegistroMonto" runat="server" style="width:305px" TabIndex="2"></asp:TextBox>
                </td>           
            </tr>
            <tr>   
               <td align="right" style="color:black">
                   <div align="right"> 
                    Sucursal:
                   </div>
                </td>
                <td>
                    <asp:TextBox ID="txtRegistroSucursal" runat="server" style="width:305px" TabIndex="3" MaxLength="100"></asp:TextBox>
                </td>  
               <td align="right" style="color:black; width: 100px;">
                    <div align="right"> 
                        Nro. Transacción:
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="txtRegistroTransaccion" runat="server" style="width:305px" TabIndex="4"></asp:TextBox>
                </td>           
            </tr>
            <tr colspan="3">
                <td align="right" style="color:black">
                    <div align="right"> 
                        Adjunto
                    </div>
                </td>
                <td>
                    <asp:FileUpload ID="fileArchivo" runat="server" style="width:305px" TabIndex="5"/>
                    <asp:Label ID="lbMensajeImagenOk" runat="server" Text="La imagen se cargo correctamente" Visible="False" Font-Bold="True" ForeColor="#669900"></asp:Label>
                    <asp:Label ID="lbMensajeImagenNo" runat="server" Text="La imagen no se cargo" Visible="False" Font-Bold="True" ForeColor="#FF3300"></asp:Label>
                </td>
            </tr>
            <tr>   
                <td colspan="4">
                    <div align="right">
                        <asp:Button ID="btnCargarRegistroPago" runat="server" class="formBtnNar1" Text="Aceptar" OnClick="btnCargarRegistroPago_Click" />
                        <asp:Button ID="btnCerrarRegistroPago" runat="server" Text="Cancelar" style="margin-left:20px" OnClick="btnCerrar_Click" />
                    </div>
                </td>           
            </tr>
        </table>
    </asp:Panel>   
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender4" runat="server" 
        TargetControlID="HiddenField1"
        PopupControlID="pnlRegistrarPago"       
        BackgroundCssClass="ModalBackground"
        DropShadow="true" />

     <asp:Panel ID="pnlRegistroPagoCuota" runat="server" HorizontalAlign="Center" style="background-color:white; width:540px;">
        <table width="100%">                        
            <tr>
                <td colspan="4" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:center;"><b>Registrar Pago</b></td>
            </tr> 
           <%-- <tr>
                <td style="color:black">
                    <div align="right">
                        Fecha de pago:
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="txtRegistroFechaCuota" runat="server" style="width:305px" TabIndex="1"></asp:TextBox>
                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="orange" TargetControlID="txtRegistroFechaCuota" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />
                </td> 
                <td align="right" style="color:black">
                    <div align="right">
                        Monto:
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="txtRegistroMontoCuota" runat="server" style="width:305px" TabIndex="2"></asp:TextBox>
                </td>           
            </tr>
            <tr>   
                <td align="right" style="color:black">
                   <div align="right"> 
                    Sucursal:
                   </div>
                </td>
                <td>
                    <asp:TextBox ID="txtRegistroSucursalCuota" runat="server" style="width:305px" TabIndex="3"></asp:TextBox>
                </td>  
                <td align="right" style="color:black; width: 100px;">
                    <div align="right"> 
                        Nro. Transacción:
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="txtRegistroTransaccionCuota" runat="server" style="width:305px" TabIndex="4"></asp:TextBox>
                </td>           
            </tr>
            <tr colspan="3">
                <td align="right" style="color:black">
                    <div align="right"> 
                        Adjunto
                    </div>
                </td>
                <td colspan="2">
                    <asp:FileUpload ID="fileArchivoCuotas" runat="server" style="width:305px" TabIndex="5"/>
                    <asp:Label ID="lbMensajeImagenCuotaOk" runat="server" Text="La imagen se cargo correctamente" Visible="False" Font-Bold="True" ForeColor="#669900"></asp:Label>
                    <asp:Label ID="lbMensajeImagenCuotaNo" runat="server" Text="La imagen no se cargo" Visible="False" Font-Bold="True" ForeColor="#FF3300"></asp:Label>
                </td>
                <td>
                    <div align="right"><asp:Button ID="btnAgregar" runat="server" class="formBtnNar1" Text="Agregar" OnClick="btnAgregar_Click" /></div>
                </td>
            </tr>
            <asp:UpdatePanel runat="server"><ContentTemplate>
            <tr>
                <td colspan="4">
                    <asp:ListView ID="lvRegistroCuotasPago" runat="server" OnItemCommand="lvRegistroCuotasPago_ItemCommand">
                    <LayoutTemplate>
                        <table>
                            <thead>
                                <tr>
                                    <td>FECHA DE PAGO</td>
                                    <td>MONTO</td>
                                    <td>SUCURSAL</td>
                                    <td>NR. TRANSACCIÓN</td>
                                    <td></td>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </tbody>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr id="row<%# Eval("id")%>" style="color:#b40b0b;">
                            <td>
                                <%# Eval("FechaPago", "{0:d}") %>
                            </td>
                            <td>
                                <%# Eval("Monto") %>
                            </td>
                            <td>
                                <%# Eval("Sucursal") %>
                            </td>
                            <td>
                                <%# Eval("Transaccion") %>
                            </td> 
                            <td style="width: 5px;">
                                <asp:LinkButton ID="lkbCancelarRegistro" runat="server" class="cancelBtn" CommandName="CancelarRegistro" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'/>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" width="100%" runat="server">
                            <tr>
                                <td align="center" valign="middle">
                                    Para adelantar cuotas, complete los campos y presione el botón Agregar. Luego de agregar las cuotas presione "Aceptar" para finalizar 
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:ListView>
                </td>
            </tr>
            </ContentTemplate></asp:UpdatePanel>--%>
            <tr style="border-top: 2px solid gray; border-bottom: 1px solid gray;">
                <td colspan="4" style="font-size:medium; padding:20px;color:#A3A3A3;">
                    Sr. Cliente, Ud. Tiene la posibilidad de adelantar Cuotas al valor de la Cuota pura vigente al momento de la precancelación. <br/>
                    La opción de precancelación es válida hasta el 10 de cada mes en curso y podrá hacerse mediante depósito en la cuenta bancaria del desarrollo, o transferencia
                    o bien coordinando otro medio de pago mediante comunicación con nuestras oficinas. Al informar dicho pago, los importes adelantados se deducirán de la última cuota
                    y las subsiguientes. Los pagos que se efectúen después del 10 de cada mes serán imputados de la misma manera pero teniendo en cuenta el valor de la cuota del mes siguiente.
                </td>
            </tr>
            <tr>   
                <td colspan="4">
                    <div align="right">
                        <asp:HiddenField ID="hfListIdRegistro" runat="server" />
                        <%--<asp:Button ID="Button1" runat="server" class="formBtnNar1" Text="Aceptar" OnClick="btnCargarRegistroCuotaPago_Click" />--%>
                        <asp:Button ID="Button2" runat="server" Text="Cerrar" style="margin-left:20px" OnClick="btnCerrarRegistroCuotaPago_Click" />
                    </div>
                </td>           
            </tr>
        </table>
    </asp:Panel>   
    <asp:HiddenField ID="HiddenField2" runat="server" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
        TargetControlID="HiddenField1"
        PopupControlID="pnlRegistroPagoCuota"       
        BackgroundCssClass="ModalBackground"
        DropShadow="true" />
</section>

<CR:CrystalReportSource ID="CrystalReportSource" runat="server" Visible="false">
    <Report FileName="Reportes/Recibo.rpt"></Report>
</CR:CrystalReportSource>
</asp:Content>
