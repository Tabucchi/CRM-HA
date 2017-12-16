<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="DetallePedido.aspx.cs" Inherits="crm.DetallePedido" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>


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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="headOptions">
            <h2><asp:Label ID="lblID" runat="server"></asp:Label> </h2>                      
             
            <asp:Button ID="btnNext" Text=">>" runat="server" CssClass="arrowBtn formBtnNar" onclick="btnNext_Click" ToolTip="Ticket Siguiente" />      
            <asp:Button ID="btnPrevious" Text="<<" runat="server" CssClass="arrowBtn formBtnNar" onclick="btnLast_Click" ToolTip="Ticket Anterior" />
         
        <div style="float:right; margin-right:10px;">
            <asp:LinkButton ID="btnEstado" runat="server"  CssClass="formBtnGrey" Text="Cambiar Estado"></asp:LinkButton>
            <asp:LinkButton ID="btnImprimir1" runat="server"  CssClass="formBtnGrey" Text="Imprimir Ticket" onclick="btnImprimir_Click"></asp:LinkButton>

            <asp:Button ID="btnFinalizar" Text="Finalizar Ticket" CssClass="formBtnGrey" runat="server" />
            <asp:Button ID="btnAcobrar" runat="server" Text="A cobrar" CssClass="formBtnGrey" Visible="false" onclick="btnAcobrar_Click"/>                      
            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" 
                CssClass="formBtnGrey" Visible="false" onclick="btnEliminar_Click"/>
        </div>
    </div>

    <div class="formHolder">
            <p class=""><strong>CLIENTE:</strong><span> <asp:Label ID="lblCliente" runat="server"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnAbrirAgenda" runat="server" ForeColor="blue" Text="Ver Info" ></asp:LinkButton> </span></p>
            <p class=""><strong>TITULO:</strong><span><asp:Label ID="lblTitulo" runat="server"></asp:Label></span></p>
            <p class=""><strong>DESCRIPCION:</strong><span> <asp:Label ID="lblDescripcion" runat="server"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnEditar" runat="server" ForeColor="blue" Text="Editar" ></asp:LinkButton></span></p>
            <p class=""><strong>FECHA DE CARGA:</strong><span>  <asp:Label ID="lblFechaCarga" runat="server"></asp:Label></span> </p>
            <p></p>
            <p class="col3"><strong>ESTADO:</strong><span> <asp:Label ID="lblEstado" runat="server"></asp:Label></span> </p>
            <p class="col3"><strong>CARGADO POR: </strong><span><asp:Label ID="lblCargadoPor" runat="server"></asp:Label></span> </p>
            <p class="col3"><strong>CATEGORIA:</strong><span> <asp:Label ID="lblCategoria" runat="server"></asp:Label></span></p>
            <p class="col3"><strong>PRIORIDAD:</strong><span> <asp:Label ID="lblPrioridad" runat="server"></asp:Label></span></p>
            <p class="col3"><strong>REALIZACION:</strong><span> <asp:Label ID="lblFechaRealizacion" runat="server"></asp:Label></span></p>
            <p class="col3"><strong>RESPONSABLE: </strong><span><asp:Label ID="lblResponsable" runat="server"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnReasignar" runat="server" ForeColor="blue" Text="Reasignar"></asp:LinkButton></span></p>
            <p class="col3"><strong>MODO DE RESOLUCION:</strong><span> <asp:Label ID="lblModoResolucion" runat="server"></asp:Label></span></p>
            <p style="border-top: medium none; margin-top: 0; padding: 5px 0; width: 60%;"><strong>CERRADO POR:</strong><span> <asp:Label ID="lblFechaFinalizacion" runat="server"></asp:Label></span></p>             
            <%--<p class="col3"><strong>COMPRAS:</strong><span>
            <asp:Label ID="lbMensaje1" runat="server" Text="No hay compras registradas" Visible="false">
            </asp:Label> <asp:LinkButton ID="btnAgregar" runat="server" ForeColor="blue" Text="Agregar" Visible="false" onclick="btnAgregar_Click"></asp:LinkButton> <asp:Label ID="lbMensaje2" runat="server"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp; <asp:LinkButton ID="btnVer" runat="server" ForeColor="blue" Text="Ver" Visible="false" onclick="btnVer_Click"></asp:LinkButton></span></p>
            --%>
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
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <script type="text/javascript">                                document.write("<div class='UpdateProgressBackground'></div>");</script>
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

                <asp:CheckBox ID="cbVisibilidad" style="float: left;width: 20px;padding: 9px 0px 8px 10px;margin-left: 20px;" runat="server" />
                <span class="check">Notificar al cliente</span>
                                         
            </label>
            </asp:Panel>                    
    </div>        

    <div>
        <asp:Panel ID="pnlFinalizarPedido" runat="server" Width="410px" HorizontalAlign="Center" CssClass="ModalPopup">
            <table width="100%">               
                  <asp:Label ID="lblClose" Text="X" runat="server" CssClass="closebtn"></asp:Label>
                <tr>
                    <td colspan="2" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:center;"><b>Ingrese un comentario sobre la finalización del Ticket.</b></td>
                </tr> 
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtComentarioFin" Rows="3" CssClass="textbox" TextMode="MultiLine" Height="60px" Width="400px" runat="server"></asp:TextBox>
 
                        <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                            TargetControlID="txtComentarioFin"
                            WatermarkText="Ingrese un comentario..."
                            WatermarkCssClass="watermarked2" />
                    </td>
                </tr>
                <tr id="filaQuienCerroTicket" runat="server">
                    <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                        Ticket solucionado por: 
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlUsuario" runat="server" PostBack="False" Width="250">
                        </asp:DropDownList>  
                    </td>
                </tr>
                <tr>
                    <td width="35%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                        Modo de resolución:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlModoResolucion" runat="server" PostBack="False" Width="250">
                        </asp:DropDownList> 
                    </td>
                </tr>
                <tr class="spacer">
                    <td colspan="2" > <hr /> </td>
                </tr>
                <tr>
            
                    <td align="left">
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="btnFinalizarTicket" runat="server" class="boton" 
                            onclick="btnFinalizarTicket_Click" Text="Finalizar" />
                    </td>
            
                </tr>
            </table>
        </asp:Panel>   
        <ajax:ModalPopupExtender ID="ModalPopupExtender" runat="server" 
            TargetControlID="btnFinalizar"
            PopupControlID="pnlFinalizarPedido" 
            CancelControlID="lblClose"
            BackgroundCssClass="ModalBackground"
            DropShadow="true" /> 
            
        <asp:Panel ID="pnlReasignar" runat="server" Width="410px" HorizontalAlign="Center" CssClass="ModalPopup">
            <table width="100%">
               <asp:Label ID="lblClose2" Text="X" runat="server" CssClass="closebtn"></asp:Label>
                <tr>
                    <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:left;">REASIGNAR A </td>
                    <td width="75%" align="left">
                        <asp:DropDownList ID="cbResponsable" runat="server" PostBack="False" Width="250">
                            <asp:ListItem Text="Seleccione un Responsable..." Value="0" />
                        </asp:DropDownList>  
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:left;">MENSAJE</td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <asp:TextBox ID="txtMensajeResponsable" Rows="3" CssClass="textbox" TextMode="MultiLine" Height="60px" Width="400px" runat="server"></asp:TextBox>                
                    </td>
                </tr>            
                <tr>
                    <td align="left" colspan="2">
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="btnAsignar" Text="Reasignar" class="boton" 
                            runat="server" onclick="btnAsignar_Click" />                        
                    </td>
                </tr>            
            </table>
        </asp:Panel>  
        <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
            TargetControlID="btnReasignar"
            PopupControlID="pnlReasignar" 
            CancelControlID="lblClose2"
            BackgroundCssClass="ModalBackground"
            DropShadow="true" />
            
        <asp:Panel ID="pnlAgenda" runat="server" Width="410px" HorizontalAlign="Center" CssClass="ModalPopup">
            <table width="100%">
              <asp:Label ID="lblClose3" Text="X" runat="server" CssClass="closebtn"></asp:Label>
                <tr>
                    <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">DIRECCION:</td>
                    <td width="75%" align="left"><b><asp:Label ID="txtDireccionCliente" runat="server"></asp:Label></b></td>
                </tr>
                <tr>
                    <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">TELEFONO:</td>
                    <td width="75%" align="left"><b><asp:Label ID="txtTelefonoCliente" runat="server"></asp:Label></b></td>
                </tr>
                <tr>
                    <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">E-MAIL:</td>
                    <td width="75%" align="left"><b><asp:Label ID="txtMailCliente" runat="server"></asp:Label></b></td>
                </tr>                     
            </table>
        </asp:Panel>
        <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
            TargetControlID="btnAbrirAgenda"
            PopupControlID="pnlAgenda" 
            CancelControlID="lblClose3"
            BackgroundCssClass="ModalBackground"
            DropShadow="true" />
            
        <asp:Panel ID="pnlEditar" runat="server" Width="410px" HorizontalAlign="Center" CssClass="ModalPopup">
            <table width="100%">
              <asp:Label ID="lblClose6" Text="X" runat="server" CssClass="closebtn"></asp:Label>
                <tr>
                    <td colspan="2" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:left;">DESCRIPCION:</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtEditDescripcion" runat="server" TextMode="MultiLine" 
                            Height="100px" Width="400px"></asp:TextBox>
                    </td>
                </tr> 
                <tr>
                    <td>
                        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" 
                            onclick="btnAceptar_Click" />
                        </td>
                        <td>
                        <asp:Button ID="bntCancelar" runat="server" Text="Cancelar" 
                                onclick="bntCancelar_Click" />
                    </td>
                </tr>                   
            </table>
        </asp:Panel>
        <ajax:ModalPopupExtender ID="ModalPopupExtender6" runat="server" 
            TargetControlID="btnEditar" 
            PopupControlID="pnlEditar" 
            CancelControlID="lblClose6" 
            BackgroundCssClass="ModalBackground">            
        </ajax:ModalPopupExtender>
        
        <asp:Panel ID="pnlEstado" runat="server" Width="410px" HorizontalAlign="Center" CssClass="ModalPopup">
            <table width="100%">
                <asp:Label ID="lblClose4" Text="X" runat="server" CssClass="closebtn"></asp:Label> 
                <tr>
                    <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:left;">CAMBIO DE ESTADO:</td>
                    <td width="50%" align="left">
                        <asp:DropDownList ID="ddlEstados" runat="server" PostBack="False" Width="250">
                            <asp:ListItem Text="Seleccione un estado..." Value="0" />
                        </asp:DropDownList>  
                    </td>
                </tr>   
                <tr>
                    <td colspan="2" style="padding-top: 10px">
                        <asp:Button ID="btnAceptarEstado" runat="server" Text="Aceptar" 
                            onclick="btnAceptarEstado_Click" />
                    </td>
                </tr>               
            </table>
        </asp:Panel>
        <ajax:ModalPopupExtender ID="ModalPopupExtender3" runat="server" 
            TargetControlID="btnEstado"
            PopupControlID="pnlEstado" 
            CancelControlID="lblClose4"
            BackgroundCssClass="ModalBackground"
            DropShadow="true" />
            
        <%--<asp:Panel ID="pnlCompras" runat="server" Width="1000px" Height="300px" HorizontalAlign="Center" CssClass="ModalPopup" Wrap="False">
            <table>
                <tr>
                    <td align="right" width="1000px"><asp:Label ID="lblClose4" Text="X" runat="server" CssClass="boton"></asp:Label></td>
                </tr>
            </table>
            <asp:ListView ID="lvItems" runat="server" InsertItemPosition="FirstItem" oniteminserting="lvItems_ItemInserting">
            <LayoutTemplate>
                <div class="PrettyGrid"> 
                    <div style="overflow:auto; height: 290px;">         
                    <table border="0" cellpadding="1" width="100%">                        
                        <thead>
                            <tr>                             
                                <th style="width: 20%" class="column_head">NOMBRE</th>
                                <th style="width: 20%" class="column_head">DESCRIPCION</th>
                                <th style="width: 20%" class="column_head">CANTIDAD</th>
                                <th style="width: 20%" class="column_head">COSTO</th>
                                <th style="width: 10%" class="column_head">PRECIO CLIENTE</th>
                                <th style="width: 10%" class="column_head"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>                        
                    </table>
                    </div>                
               </div>
            </LayoutTemplate>
            <ItemTemplate>                   
                <tr style="background-color:#FFFFFF; cursor:pointer;" onclick="Visible(<%# Eval("id")%> )">
                    <td align="left"><asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("Nombre") %>' /></td>
                    <td align="left"><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("Descripcion") %>' /></td>
                    <td><asp:Label ID="lbCantidad" runat="Server" Text='<%#Eval("Cantidad") %>' /></td> 
                    <td><asp:Label ID="lbCosto" runat="server" Text='<%#Eval("Costo") %>' /></td>  
                    <td><asp:Label ID="lbPrecioCl" runat="server" Text='<%#Eval("precioCliente") %>' /></td> 
                    <td></td>
                </tr>
            </ItemTemplate>
            
            <InsertItemTemplate>
                <tr style="background-color:#D3D3D3">
                    <td>
                        <asp:Label runat="server" ID="lbNombre" AssociatedControlID="txtNombre"/>
                        <asp:TextBox ID="txtNombre" runat="server" Text='<%#Bind("Nombre") %>' /><br />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbDescripcion" AssociatedControlID="txtDescripcion"/>
                        <asp:TextBox ID="txtDescripcion" runat="server" Text='<%#Bind("Descripcion") %>' /><br />
                    <td>
                        <asp:Label runat="server" ID="lbCantidad" AssociatedControlID="txtCantidad"/>
                        <asp:TextBox ID="txtCantidad" runat="server" Text='<%#Bind("Cantidad") %>' />
                        <ajax:FilteredTextBoxExtender ID="ftbe" runat="server" TargetControlID="txtCantidad" FilterType="Numbers"/>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbCosto" AssociatedControlID="txtCosto"/>
                        <asp:TextBox ID="txtCosto" runat="server" Text='<%#Bind("Costo") %>' />
                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtCosto" FilterType="Numbers"/>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbPrecioCliente" AssociatedControlID="txtPrecioCliente"/>
                        <asp:TextBox ID="txtPrecioCliente" runat="server" Text='<%#Bind("PrecioCliente") %>' />
                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtPrecioCliente" FilterType="Numbers"/>
                    </td>
                    <td><asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert" Text="Insertar" /></td>
                  </td>
                  <td></td>
                </tr>
            </InsertItemTemplate>
                                                 
            <AlternatingItemTemplate>
                <tr style="background-color:#EFEFEF; cursor:pointer;" onclick="Visible(<%# Eval("id")%> )">
                    <td align="left"><asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("Nombre") %>' /></td>
                    <td align="left"><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("Descripcion") %>' /></td>
                    <td><asp:Label ID="lbCantidad" runat="Server" Text='<%#Eval("Cantidad") %>' /></td>   
                    <td><asp:Label ID="lbcosto" runat="server" Text='<%#Eval("Costo") %>' /></td>  
                    <td><asp:Label ID="lbPrecioCl" runat="server" Text='<%#Eval("precioCliente") %>' /></td> 
                    <td></td>
                </tr>             
            </AlternatingItemTemplate>  
                          
            <EmptyDataTemplate>
                <table id="Table1" width="100%" runat="server">
                    <tr>
                        <td align="center"><b>No data found.<b/></td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView> 
        
        </asp:Panel>--%>  
        <%--<ajax:ModalPopupExtender ID="ModalPopupExtender3" runat="server" 
            TargetControlID="btnVer"
            PopupControlID="pnlCompras" 
            CancelControlID="lblClose4"
            BackgroundCssClass="ModalBackground"
            DropShadow="true" />--%>
        <%--  <ajax:ModalPopupExtender ID="ModalPopupExtender4" runat="server" 
            TargetControlID="btnAgregar"
            PopupControlID="pnlCompras" 
            CancelControlID="lblClose4"
            BackgroundCssClass="ModalBackground"
            DropShadow="true" />--%>
    </div>
        
    <div>
        <input type="hidden" id="txtId" runat="server" />
    </div>
    
    <div align="left">  
        <%--<cr:crystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource1" 
            Width="894px" DisplayToolbar="False" Visible="false" />
        <cr:crystalReportSource ID="CrystalReportSource1" runat="server" 
            Visible="false">
            <Report FileName="OrdenTrabajo.rpt"></Report>
        </cr:crystalReportSource>--%>
    </div>
</section>
</asp:Content>
