<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Agenda" Codebehind="Agenda.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="js/jquery.mask.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.cuit').mask('99-99999999-9');
        });
    </script>

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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>

    <section>
        <div class="formHolder" id="searchBoxTop">
            <div class="headOptions headLine">
                <h2>Agenda:</h2>

                <label style="padding: 5px 0px 0px 20px; width:32%">
                <asp:DropDownList ID="cbEmpresa" Width="77%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbEmpresa_SelectedIndexChanged" ><asp:ListItem Text="Todas" Value="0" /></asp:DropDownList>
                </label>
                <div runat="server" id="divEncabezado" style="float:right; width:356px; padding-top: 14px;">                    
                        <div style="float:left; position:absolute;">
                            <div style="float:left; margin-top: -7px;">
                                <asp:Button ID="btnDescargar" runat="server" Text="Descargar listado" CssClass="formBtnNar" OnClick="btnDescargar_Click" />
                            </div>
                            <asp:Panel ID="pnlFormulario" runat="server" Visible="false">
                                <div style="float:right; margin-left: 154px; margin-top: -23px;">
                                    <a alt="Imprimir formulario nuevo cliente" class="tooltip tooltipColor">
                                        <asp:ImageButton ID="btnImprimirCliente" ImageUrl="~/images/iconPrint.png" runat="server" CssClass="iconPrint" style="margin-right: 15px;" OnClick="btnImprimirCliente_Click"/>
                                    </a>
                                </div> 
                            </asp:Panel>                           
                        </div>
                    <asp:Panel ID="pnlFormulario1" runat="server" Visible="false">
                        <div style="float:right">
                            <b>
                                <a href="NuevoCliente.aspx" class="formBtnGrey" style="margin-top: -10px;">Agregar nuevo cliente</a>
                            </b>
                       </div>
                    </asp:Panel> 
                </div>
            </div>
        </div>
    </section>
    
    <asp:ListView ID="lvEmpresas" runat="server" OnItemCommand="lvProyectos_ItemCommand" DataKeyNames="id" OnPreRender="ListPager_PreRender" OnItemDataBound="lvEmpresas_ItemDataBound">
            <LayoutTemplate>
                <ul id="accordionList">
                    <li id="listHead">
                        <span style="width:18%" class="headColumnHead">CLIENTE</span>
                        <span style="width:09%;" class="headColumnHead">DOCUMENTO</span>
                        <span style="width:10%;" class="headColumnHead">TELÉFONO</span>
                        <span style="width:18%;" class="headColumnHead">MAIL</span>
                        <span style="width:09%;" class="headColumnHead">CUIT</span>            
                        <span style="width:10%;" class="headColumnHead">CONDICIÓN DE IVA</span>                
                        <span style="width:08%;" class="headColumnHead">CARÁCTER</span>
                        <span style="width:06%; font-size: 11px" class="headColumnHead">UNIDADES ADQUIRIDAS</span>
                        <span style="width:02%;" class="headColumnHead"></span>
                    </li>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />  
                </ul>                        
            </LayoutTemplate> 
            <ItemTemplate>               
                <li>
                    <div class="accordionButton">
                        <span style="width: 18%" class="listCel"><%#Eval("GetNombreCompleto") %></span>
                        <span style="width: 09%" class="listCel"><%#Eval("GetTipoDoc") %>&nbsp;<%#Eval("Documento") %></span>
                        <span style="width: 10%" class="listCel"><%#Eval("telefono") %></span>
                        <span style="width: 18%" class="listCel"><%#Eval("Mail") %></span>
                        <span style="width: 09%" class="listCel"><%#Eval("cuit") %></span>
                        <span style="width: 10%" class="listCel"><%#Eval("GetCondicionIva") %></span>   
                        <span style="width: 08%" class="listCel"><%#Eval("GetCaracter") %></span>                 
                        <span style="width: 04%" class="listCel"><asp:LinkButton ID="btnDetalle" runat="server" CssClass="detailBtn" CommandName="Detalle" Text="Detalle" ToolTip="Detalle" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'/></span>
                        <span style="width: 04%" class="listCel">
                            <asp:Panel ID="pnlEditar" runat="server" Visible="true">
                                <a id="btnEditar" class="editBtn"  href="EditarCliente.aspx?idCliente=<%# Eval("id") %>"></a>
                            </asp:Panel>
                        </span>
                    </div>
		            <div class="accordionContent">
                        <asp:Panel ID="Panel1" runat="server">
                            <div>
                                <p style="border-top: none; padding-top: 10px; margin-top: -9px;"><strong>DIRECCIÓN:</strong><span><%# Eval("domicilio.Calle")%>&nbsp;<%# Eval("domicilio.Direccion")%></span></p>
                            </div>
                            <div>
                                <p class="col3" style="padding-bottom: 12px !important;"><strong>CIUDAD:</strong><span><%# Eval("domicilio.Ciudad")%></span></p>
                                <p class="col3" style="padding-bottom: 12px !important;"><strong>COD. POSTAL:</strong><span><%# Eval("domicilio.CodPostal")%></span></p>
                                <p class="col3" style="padding-bottom: 12px !important;"><strong>PROVINCIA:</strong><span><%# Eval("GetProvincia")%></span></p>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="asd" runat="server">
                            <div>
                                <p style="border-top: 1px dotted #666; padding-top: 20px;">
                                    <strong style="color: #666; font-style: italic;">APODERADO</strong>
                                </p>
                                <p class="col3">
                                    <strong>RAZÓN SOCIAL:</strong><span><%# Eval("apoderadoClass.RazonSocial")%></span>
                                </p>
                                <p class="col3">
                                    <strong>DOCUMENTO:</strong><span><%# Eval("apoderadoClass.GetTipoDoc")%>&nbsp;<%# Eval("apoderadoClass.Documento")%></span>
                                </p>
                                <p class="col3">
                                    <strong>CUIT:</strong><span><%# Eval("apoderadoClass.Cuit")%></span>
                                </p>
                                <p class="col3">
                                    <strong>TELÉFONO:</strong><span><%# Eval("apoderadoClass.Telefono")%></span>
                                </p>
                                <p class="col3">
                                    <strong>MAIL:</strong><span><%# Eval("apoderadoClass.Mail")%></span>
                                </p>

                                <p class="col3">
                                    <strong>DIRECCIÓN:</strong><span><%# Eval("domicilioApoderado.Direccion")%></span>
                                </p>
                                <p class="col3">
                                    <strong>CIUDAD:</strong><span><%# Eval("domicilioApoderado.Ciudad")%></span>
                                </p>
                                <p class="col3">
                                    <strong>COD. POSTAL:</strong><span><%# Eval("domicilioApoderado.CodPostal")%></span>
                                </p>
                                <p class="col3">
                                    <strong>PROVINCIA:</strong><span><%# Eval("GetProvinciaApoderado")%></span>
                                </p>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server">
                            <div>
                                <p style="border-top: 1px dotted #666; padding-top: 20px;">
                                    <strong style="color: #666; font-style: italic;">COMENTARIOS</strong>
                                </p>
                            </div>
                            <div>
                                <p class="col3" style="padding-bottom: 12px !important; width:100% !important"><span style="width:100%"><%# Eval("Comentarios")%></span></p>
                            </div>
                        </asp:Panel>
                    </div>
                </li>               
            </ItemTemplate>              
            <EmptyDataTemplate>
                <table id="Table1" style="width:100%" runat="server">
                    <tr>
                        <td align="center">No data found.</td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>     
         
    <section>
    <asp:Panel ID="pnlDetalle" runat="server" HorizontalAlign="Center" CssClass="modal"  style="width: 850px; top:-60px">
        <table width="100%">               
            <tr>
                <td colspan="2"><modalTitle><b>Propiedades de <asp:Label ID="lbCliente" runat="server"/></b></modalTitle></td>
            </tr> 
            <tr>
                <td colspan="2">
                    <asp:ListView ID="lvDetalle" runat="server">
                        <LayoutTemplate>
                            <section>
                                <table>
                                    <thead id="tableHead">
                                        <tr>
                                            <td style="width: 14%; text-align:center">Obra</td>
                                            <td style="width: 4%; text-align:center">Fecha de adquisición</td>
                                            <td style="width: 5%; text-align:center">Cod U.F.</td>
                                            <td style="width: 5%; text-align:center">Nivel</td>
                                            <td style="width: 4%; text-align:center">Nro. Unidad</td>
                                            <td style="width: 4%; text-align:center">Estado</td>
                                            <td style="width: 4%; text-align:center">Moneda</td>
                                            <td style="width: 8%; text-align:center">Valor</td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </tbody>
                                </table>
                            </section>
                        </LayoutTemplate>
                        <ItemTemplate>               
                <li>
                    <div class="accordionButton">
                        <span style="width: 18%" id="lbCliente1" class="listCel">
                            <asp:Label ID="lbCliente" runat="Server" Text='<%#Eval("GetNombreCompleto") %>' />
                            <%--<%#Eval("GetNombreCompleto") %>--%>
                        </span>
                        <span style="width: 09%" class="listCel">
                            <asp:Label ID="lbTipoDoc" runat="Server" Text='<%#Eval("GetTipoDoc") %>' />&nbsp;
                            <asp:Label ID="lbDocumento" runat="Server" Text='<%#Eval("Documento") %>' />
                            <%--<%#Eval("GetTipoDoc") %>&nbsp;<%#Eval("Documento") %>--%>
                        </span>
                        <span style="width: 10%" class="listCel">
                            <asp:Label ID="lbTelefono" runat="Server" Text='<%#Eval("telefono") %>' />
                           <%-- <%#Eval("telefono") %>--%>
                        </span>
                        <span style="width: 18%" class="listCel">
                            <asp:Label ID="lbMail" runat="Server" Text='<%#Eval("Mail") %>' />
                            <%--<%#Eval("Mail") %>--%>
                        </span>
                        <span style="width: 09%" class="listCel">
                            <asp:Label ID="lbCuit" runat="Server" Text='<%#Eval("cuit") %>' />
                            <%--<%#Eval("cuit") %>--%>
                        </span>
                        <span style="width: 10%" class="listCel">
                            <asp:Label ID="lbCondicionIva" runat="Server" Text='<%#Eval("GetCondicionIva") %>' />
                            <%--<%#Eval("GetCondicionIva") %>--%>
                        </span>   
                        <span style="width: 08%" class="listCel">
                            <asp:Label ID="lbCaracter" runat="Server" Text='<%#Eval("GetCaracter") %>' />
                            <%--<%#Eval("GetCaracter") %>--%>
                        </span>                 
                        <span style="width: 04%" class="listCel"><asp:LinkButton ID="btnDetalle" runat="server" CssClass="detailBtn" CommandName="Detalle" Text="Detalle" ToolTip="Detalle" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'/></span>
                        <span style="width: 04%" class="listCel">
                            <asp:Panel ID="pnlEditar" runat="server" Visible="true">
                                <a id="btnEditar" class="editBtn"  href="EditarCliente.aspx?idCliente=<%# Eval("id") %>"></a>
                            </asp:Panel>
                        </span>
                    </div>
		            <div class="accordionContent">
                        <asp:Panel ID="Panel1" runat="server">
                            <div>
                                <p style="border-top: none; padding-top: 10px; margin-top: -9px;"><strong>DIRECCIÓN:</strong><span><%# Eval("domicilio.Calle")%>&nbsp;<%# Eval("domicilio.Direccion")%></span></p>
                            </div>
                            <div>
                                <p class="col3" style="padding-bottom: 12px !important;"><strong>CIUDAD:</strong><span><%# Eval("domicilio.Ciudad")%></span></p>
                                <p class="col3" style="padding-bottom: 12px !important;"><strong>COD. POSTAL:</strong><span><%# Eval("domicilio.CodPostal")%></span></p>
                                <p class="col3" style="padding-bottom: 12px !important;"><strong>PROVINCIA:</strong><span><%# Eval("GetProvincia")%></span></p>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="asd" runat="server">
                            <div>
                                <p style="border-top: 1px dotted #666; padding-top: 20px;">
                                    <strong style="color: #666; font-style: italic;">APODERADO</strong>
                                </p>
                                <p class="col3">
                                    <strong>RAZÓN SOCIAL:</strong><span><%# Eval("apoderadoClass.RazonSocial")%></span>
                                </p>
                                <p class="col3">
                                    <strong>DOCUMENTO:</strong><span><%# Eval("apoderadoClass.GetTipoDoc")%>&nbsp;<%# Eval("apoderadoClass.Documento")%></span>
                                </p>
                                <p class="col3">
                                    <strong>CUIT:</strong><span><%# Eval("apoderadoClass.Cuit")%></span>
                                </p>
                                <p class="col3">
                                    <strong>TELÉFONO:</strong><span><%# Eval("apoderadoClass.Telefono")%></span>
                                </p>
                                <p class="col3">
                                    <strong>MAIL:</strong><span><%# Eval("apoderadoClass.Mail")%></span>
                                </p>

                                <p class="col3">
                                    <strong>DIRECCIÓN:</strong><span><%# Eval("domicilioApoderado.Direccion")%></span>
                                </p>
                                <p class="col3">
                                    <strong>CIUDAD:</strong><span><%# Eval("domicilioApoderado.Ciudad")%></span>
                                </p>
                                <p class="col3">
                                    <strong>COD. POSTAL:</strong><span><%# Eval("domicilioApoderado.CodPostal")%></span>
                                </p>
                                <p class="col3">
                                    <strong>PROVINCIA:</strong><span><%# Eval("GetProvinciaApoderado")%></span>
                                </p>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server">
                            <div>
                                <p style="border-top: 1px dotted #666; padding-top: 20px;">
                                    <strong style="color: #666; font-style: italic;">COMENTARIOS</strong>
                                </p>
                            </div>
                            <div>
                                <p class="col3" style="padding-bottom: 12px !important; width:100% !important"><span style="width:100%"><%# Eval("Comentarios")%></span></p>
                            </div>
                        </asp:Panel>
                    </div>
                </li>               
            </ItemTemplate>
                        <EmptyDataTemplate>
                            <section>
                                <table id="Table1" style="width:100%" runat="server">
                                    <tr>
                                        <td align="center"><b>No se encontraron unidades registradas.<b/></td>
                                    </tr>
                                </table>
                            </section>
                        </EmptyDataTemplate>
                    </asp:ListView>                        
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div align="center">
                        <asp:Button ID="btnCerrarObras" runat="server" CssClass="btnClose" Text="Cerrar" OnClick="btnCerrarObras_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>   
    <asp:HiddenField ID="hfdDetalle" runat="server" />
    <ajax:ModalPopupExtender ID="ModalDetalle" runat="server"
        TargetControlID="hfdDetalle"
        PopupControlID="pnlDetalle" 
        BackgroundCssClass="ModalBackground"
        DropShadow="true" /> 
    </section>

    <CR:CrystalReportSource ID="CrystalReportSource" runat="server" Visible="false">
        <Report FileName="Reportes/Cliente.rpt"></Report>
    </CR:CrystalReportSource>

    <CR:CrystalReportSource ID="CrystalReportSource1" runat="server" Visible="false">
        <Report FileName="Reportes/Agenda.rpt"></Report>
    </CR:CrystalReportSource>

    <asp:HiddenField ID="hfIdCliente" runat="server" />
</asp:Content>