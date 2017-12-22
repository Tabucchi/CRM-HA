<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Compra" Codebehind="Compra.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>   
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <asp:ObjectDataSource ID="odsProveedor" runat="server" SelectMethod="GetListaProveedor" TypeName="cProveedor"></asp:ObjectDataSource>
    <div class="pal grayArea uiBoxGray noborder">
        <table width="100%">
            <tr>
                <td height="35px" class="titulo1">
                    <asp:Label ID="lbTitulo1" runat="server" Text="Nuevo pedido de Compra"></asp:Label>
                    <asp:Label ID="lbTitulo2" runat="server" Text="Cotizador" Visible="false"></asp:Label>
                </td> 
                <td align="right"> <asp:LinkButton ID="lkbVolver" runat="server" onclick="lkbVolver_Click">Volver a Compras</asp:LinkButton>  </td>
            </tr>
            <tr>
                <td colspan="2"><b><hr/></b></td>
            </tr>
            <tr>   
                <asp:Panel ID="pnlTicketNroCompra" runat="server">
                    <td colspan="4" align="left" style="padding-left:16px;">
                        <asp:Label ID="lbTituloNroCompra" runat="server" Text="NRO. COMPRA:" Font-Bold="True"></asp:Label>&nbsp;
                        <asp:Label ID="lbNroCompra" runat="server"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <b>CLIENTE:</b> &nbsp;
                        <asp:Label ID="lbEmpresa" runat="server"></asp:Label>&nbsp;
                        <asp:Label ID="lbCliente" runat="server"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lbTituloTicket" runat="server" Text="TICKET:" Font-Bold="True"></asp:Label>
                        <asp:Label ID="lbTicket" runat="server"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lkbVer" runat="server" onclick="verDetalle_Click">Ver</asp:LinkButton>
                    </td>
                 </asp:Panel>
            </tr>
        </table>
        
        <div>
            <asp:Panel ID="pnlFiltro" runat="server">
                <table width="100%"> 
                    <tr>
                        <td align="left" style="width:50px; padding-left: 27px;"><b>EMPRESA:</b></td>
                        <td align="left">
                            <asp:DropDownList ID="cbEmpresa" Width="200px" runat="server" 
                                onselectedindexchanged="cbEmpresa_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                            &nbsp;&nbsp;
                            <b>CLIENTE:</b>
                            <asp:DropDownList ID="cbCliente" Width="200px" runat="server" ></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
               
        <asp:ListView ID="lvNuevaCompra" runat="server" DataKeyNames="id"
            onitemupdating="lvNuevaCompra_ItemUpdating"
            oniteminserting="lvNuevaCompra_ItemInserting" 
            onitemediting="lvNuevaCompra_ItemEditing"
            InsertItemPosition="FirstItem" onitemdeleting="lvNuevaCompra_ItemDeleting">
            <LayoutTemplate>
                <div class="PrettyGrid">                
                    <div style="overflow:auto; height:250px">             
                    <table border="0" cellpadding="1">                            
                        <thead>
                            <tr>
                                <th style="width: 2%; height:20px;" class="column_head">CANTIDAD</th> 
                                <th style="width: 15%; height:20px;" class="column_head">DESCRIPCION</th>
                                <th style="width: 10%; height:20px;" class="column_head">PROVEEDOR</th>
                                <th style="width: 8%; height:20px;" class="column_head">IMPORTE FINAL PROVEEDOR</th>
                                <th style="width: 8%; height:20px;" class="column_head">IMPORTE FINAL CLIENTE</th>
                                <th style="width: 8%; height:20px;" class="column_head">NRO. PEDIDO PROVEEDOR</th>
                                <th style="width: 4%" class="column_head"></th>
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
                <tr style="background-color:#FFFFFF; cursor:pointer;" onclick="Visible(<%#Eval("Id")%> )">
                    <td><asp:Label ID="lbCantidad" runat="Server" Text='<%#Eval("cantidad") %>' /></td>
                    <td><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("descripcion") %>' /></td>
                    <td><asp:Label ID="lbSistema" runat="Server" Text='<%#Eval("GetProveedor") %>' /></td>
                                       
                    <td align="center">
                        <asp:Panel ID="pnlImporteProveedor" runat="server" >
                            <asp:Label ID="lbImporteProveedor" runat="server" Text='<%#Eval("ImporteProveedor") %>'></asp:Label>
                        </asp:Panel>
                     </td>
                    
                    <td align="center">
                        <asp:Panel ID="pnlImporteCliente" runat="server" >
                            <asp:Label ID="lbImporteCliente" runat="server" Text='<%#Eval("ImporteCliente") %>'></asp:Label>
                        </asp:Panel>
                     </td>
                                        
                    <td><asp:Label ID="lbNroPedidoProveedor" runat="Server" Text='<%#Eval("GetNroPedidoProveedor") %>' /></td>
                    <td>
                      <asp:LinkButton ID="EditButton" runat="Server" Text="Editar" CommandName="Edit" />
                    </td>
                </tr>
            </ItemTemplate>
            
            <EditItemTemplate>
                <tr style="background-color: #ADD8E6">
                <td>
                  <asp:TextBox ID="txtEditCantidad" runat="server" Text='<%#Bind("cantidad") %>' width="50px"/>
                </td>
                <td>
                  <asp:TextBox ID="txtEditDescripcion" runat="server" Text='<%#Bind("descripcion") %>' width="308px"/>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEditProveedor" runat="server" DataSourceID="odsProveedor" DataTextField="nombre" DataValueField="id" SelectedValue='<%#Bind("idProveedor") %>' width="155px"/>
                </td>
                <td>
                  <asp:TextBox ID="txtEditImporteProveedor" runat="server" Text='<%#Bind("importeProveedor") %>' width="100px"/>  
                  <ajax:FilteredTextBoxExtender ID="ftbe3" runat="server"
                            TargetControlID="txtEditImporteProveedor"         
                            FilterType="Custom, Numbers"
                            ValidChars=","/>
                </td>
                <td>
                    <asp:TextBox ID="txtEditImporteCliente" runat="server" Text='<%#Bind("importeCliente") %>' width="100px"/>
                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                            TargetControlID="txtEditImporteCliente"         
                            FilterType="Custom, Numbers"
                            ValidChars=","/>
                </td>
                <td>
                    <asp:TextBox ID="txtEditNroPedido" runat="server" Text='<%#Bind("GetNroPedidoProveedor") %>' width="100px"/>
                </td>
                <td>
                  <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" Text="Cambiar" />&nbsp;
                  <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" Text="Eliminar" />
                </td>
              </tr>
            </EditItemTemplate>
                           
            <InsertItemTemplate>
                <tr style="background-color:#D3D3D3">
                    <td>
                        <asp:Label runat="server" ID="lbCantidad" AssociatedControlID="txtCantidad"/>
                        <asp:TextBox ID="txtCantidad" runat="server" Text='<%#Bind("cantidad") %>' width="50px"/><br />
                        <ajax:FilteredTextBoxExtender ID="ftbe" runat="server"
                            TargetControlID="txtCantidad"         
                            FilterType="Custom, Numbers"
                            ValidChars=","/>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbDescripcion" AssociatedControlID="txtDescripcion"/>
                        <asp:TextBox ID="txtDescripcion" runat="server" Text='<%#Bind("descripcion") %>' width="308px"/><br />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEditProveedor" runat="server" DataSourceID="odsProveedor" DataTextField="nombre" DataValueField="id"/>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbimporteProveedor" AssociatedControlID="_txtImporteProveedor"/>
                        <asp:TextBox ID="_txtImporteProveedor" runat="server" Text='<%#Bind("importeProveedor") %>' width="100px"/>
                        <ajax:FilteredTextBoxExtender ID="ftbe1" runat="server"
                            TargetControlID="_txtImporteProveedor"         
                            FilterType="Custom, Numbers"
                            ValidChars=","/>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbImporteCliente" AssociatedControlID="_txtImporteCliente"/>
                        <asp:TextBox ID="_txtImporteCliente" runat="server" Text='<%#Bind("importeProveedor") %>' width="100px"/>
                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                            TargetControlID="_txtImporteCliente"         
                            FilterType="Custom, Numbers"
                            ValidChars=","/>
                    </td>
                    <td>                        
                        <asp:Label runat="server" ID="lbnroPedidoProveedor" AssociatedControlID="_txtNroPedidoProveedor"/>
                        <asp:TextBox ID="_txtNroPedidoProveedor" runat="server" Text='<%#Bind("nroPedidoProveedor") %>' width="100px"/><br />
                    </td>
                    <td><asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert" Text="Insertar" /></td>
                </tr>
            </InsertItemTemplate>
                         
            <AlternatingItemTemplate>
                <tr style="background-color:#EFEFEF; cursor:pointer;" onclick="Visible(<%#Eval("Id")%> )">
                    <td><asp:Label ID="lbCantidad" runat="Server" Text='<%#Eval("cantidad")%>' /></td>
                    <td><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("descripcion")%>' /></td>
                    <td><asp:Label ID="lbSistema" runat="Server" Text='<%#Eval("GetProveedor") %>' /></td>
                    <td align="center">
                        <asp:Panel ID="pnlImporteProveedor" runat="server" >
                            <asp:Label ID="lbImporteProveedor" runat="server" Text='<%#Eval("ImporteProveedor") %>'></asp:Label>
                        </asp:Panel>
                     </td>
                    
                    <td align="center">
                        <asp:Panel ID="pnlImporteCliente" runat="server" >
                             <asp:Label ID="lbImporteCliente" runat="server" Text='<%#Eval("ImporteCliente") %>'></asp:Label>
                        </asp:Panel>
                     </td>
                    <td><asp:Label ID="lbNroPedidoProveedor" runat="Server" Text='<%#Eval("GetNroPedidoProveedor") %>' /></td>
                    <td>
                        <asp:LinkButton ID="EditButton" runat="Server" Text="Editar" CommandName="Edit" />
                    </td>
                </tr>             
            </AlternatingItemTemplate>
        </asp:ListView>
                        
        <table width="100%">
            <tr>
                <td align="left">
                    <asp:CheckBox ID="chbIva" runat="server" Text="IVA incluido"/>
                </td>
                <td colspan="2" align="right">
                    <asp:Label ID="lbMensajeMail" runat="server" 
                        Text="Se ha enviado la cotización al cliente." Font-Bold="True" 
                        ForeColor="#B9BE42" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr><td colspan="3"><hr /></td></tr>
            <tr>
                <asp:Panel ID="pnlTotales" runat="server" Visible="false">
                    <td align="left" style="padding-left: 20px; width:252px">
                        <br />
                        <asp:Label ID="lbTituloTotalProveedor" runat="server" Text="Total Proveedor: $" Font-Bold="True" Font-Size="Small"></asp:Label>
                        <asp:Label ID="lbTotalProveedor" runat="server" Font-Bold="True" Font-Size="Small" Visible="true"></asp:Label>
                    </td>
                    <td align="left" style="padding-left: 20px; width:252px">
                        <br />
                        <asp:Label ID="lbTituloTotalCliente" runat="server" Text="Total Cliente: $" Font-Bold="True" Font-Size="Small"></asp:Label>
                        <asp:Label ID="lbTotalCliente" runat="server" Font-Bold="True" Font-Size="Small" Visible="true"></asp:Label>
                    </td>
                </asp:Panel>
                <asp:Panel ID="pnlEnviarCotizacion" runat="server" Visible="false">
                    <td align="right" style="width:700px; padding-top:10px">          
                         <asp:ImageButton ID="btnPDF" runat="server"  
                                    Height="22px" Width="22px" ImageUrl="Imagenes/pdf_logo.png" 
                             onclick="btnPDF_Click" />
                        &nbsp;&nbsp;
                        <b>ENVIAR A:</b>
                        <asp:DropDownList ID="cbClienteEnvio" Width="200px" runat="server" />
                        <asp:Button ID="btnEnviar" runat="server" Text="Enviar" 
                            style="cursor: pointer; font-size: 11px; font-weight: bold; color: #333333; padding: 2px 1px; margin-left: 15px;" 
                            onclick="btnEnviar_Click" /> 
                    </td>
                 </asp:Panel>
            </tr>
            <tr><td colspan="3"><hr /></td></tr>   
            <tr>
                <td align="left" style="padding-left:20px" colspan="3">    
                    <asp:Label ID="lbUsuarioTitulo" runat="server" Font-Bold=true Text="USUARIO:"></asp:Label> &nbsp;
                    <asp:Label ID="lbUsuario" runat="server"></asp:Label>
                
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label2" runat="server" Font-Bold=true Text="FECHA:"></asp:Label> &nbsp;<asp:Label ID="lbFecha" runat="server"></asp:Label>
                
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbEstadoTitulo" runat="server" Font-Bold=true Text="ESTADO:"></asp:Label> &nbsp;
                    <asp:Label ID="lbEstado" runat="server"></asp:Label>
                </td>
            </tr>
                        
            <tr><td colspan="3"><hr /></td></tr>
            <tr width="700px">
                <td align="left" width="286px" colspan="2">
                    <asp:Button ID="btnSolicitarCotizacion" runat="server" Text="Solicitar Cotización" 
                        ValidationGroup="camposObligatorios"                         
                        style="cursor: pointer; font-size: 11px; font-weight: bold; color: #FF3300; padding: 2px 1px; margin-left: 4px;" 
                        Font-Bold="False" onclick="btnSolicitarCotizacion_Click"/>
                    
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" onclick="btnGuardar_Click" ValidationGroup="camposObligatorios" 
                        style="cursor: pointer; font-size: 11px; font-weight: bold; color: #333333; padding: 2px 1px; margin-left: 10px;" 
                        Font-Bold="False" Visible="false"/>
                    
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                        style="cursor: pointer; font-size: 11px; font-weight: bold; color: #333333; padding: 2px 1px; margin-left: 10px;"
                        onclick="btnCancelar_Click" />   
                    
                    <asp:Label ID="lbMensajeGuardar" runat="server" Text="Compra guardada" Visible="false" style="padding-left: 5px"
                        ForeColor="#B9BE42" Font-Bold="True"></asp:Label>  
                    <asp:Label ID="lbMensajeSolicitud" runat="server" Text="Solicitud guardada" Visible="false"
                        ForeColor="#B9BE42" Font-Bold="True"></asp:Label> 
                    <br />                     
                </td>
                <td align="right" style="padding-bottom: 5px">  
                    <asp:Label ID="lbMensajeEstado" runat="server" 
                        Text="Esta compra esta a la espera de ser aprobada" Font-Bold="True" 
                        ForeColor="#B9BE42" Visible="False"></asp:Label>
                        
                    <asp:Label ID="lbMensajeEntregado" runat="server" Text="Esta compra ya fue entregada" Font-Bold="True" 
                        ForeColor="#B9BE42" Visible="False"></asp:Label>
                        
                    <asp:Label ID="lbMensajeRechazada" runat="server" Text="Esta compra fue rechazada" Font-Bold="True" 
                        ForeColor="#B9BE42" Visible="False"></asp:Label>
                        
                    <asp:Button ID="btnAprobado" runat="server" Text="Aprobado" 
                        style="cursor: pointer; font-size: 11px; font-weight: bold; color: #333333; padding: 2px 1px; margin-left: 15px;" 
                        onclick="btnAprobado_Click" />    
                        
                    <asp:Button ID="btnStock" runat="server" Text="En stock" Visible="false"
                        style="cursor: pointer; font-size: 11px; font-weight: bold; color: #333333; padding: 2px 1px; margin-left: 12px;" 
                        onclick="btnStock_Click"/>
                    <asp:Button ID="btnEntregado" runat="server" Text="Entregado" Enabled="false" Visible="false"
                        style="cursor: pointer; font-size: 11px; font-weight: bold; color: #333333; padding: 2px 1px; margin-left: 12px;" 
                        onclick="btnEntregado_Click"/>
                    <asp:Button ID="btnRechazar" runat="server" Text="Rechazar" Enable="false"
                        Visible="False" onclick="btnRechazar_Click"
                        style="cursor: pointer; font-size: 11px; font-weight: bold; color: #333333; padding: 2px 1px; margin-left: 12px;"/>
                </td>
            </tr>
        </table>
    </div>    
    <div>
        <input type="hidden" id="txtId" runat="server" />
    </div>
    <%--<div align="left">       
        <cr:crystalreportviewer ID="CrystalReportViewer2" runat="server" 
            AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource2" 
            Width="894px" DisplayToolbar="False" Visible="false" />
        <cr:crystalreportsource ID="CrystalReportSource2" runat="server" 
            Visible="false">
            <Report FileName="Cotizacion.rpt">
            </Report>
        </cr:crystalreportsource>    
    </div>  --%> 
</div>
</asp:Content>