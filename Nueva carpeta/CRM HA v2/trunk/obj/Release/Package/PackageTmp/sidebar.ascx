<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sidebar.ascx.cs" Inherits="crm.sidebar" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
            <aside>
                    <Ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
                        <Services>
                            <asp:ServiceReference Path="MyAutocompleteService.asmx" />
                        </Services>
                    </Ajax:ToolkitScriptManager>
				<%--<div class="sideGroup">
					<div class="sideHeader">
						<h3>Novedades</h3>
						<ul class="options">
							<li><asp:LinkButton ID="btnAdd" runat="server" class="optAdd">Añadir</asp:LinkButton>
                            </li>
							<li><a href="Novedad.aspx" class="optMore">Ver+</a></li>
						</ul>
					</div>
					<ul id="novedades">
                        <asp:Repeater ID="rptNovedades" runat="server">
                            <ItemTemplate>
                                <itemtemplate>
                                    <li>
                                    <h4><%#((cNovedad)Container.DataItem).GetNombrUsuario %>:</h4>                                   
                                    <p><%#((cNovedad)Container.DataItem).Descripcion%>.</p>
                                    <span class="footer"><%#((cNovedad)Container.DataItem).Fecha.ToLongDateString()%></span>
                                    </li>
                                </itemtemplate>
                            </ItemTemplate>
                        </asp:Repeater>
					</ul>
				</div>--%>

                <div class="sideGroup">
					<div class="sideHeader" style="border-bottom: 1px solid rgba(205, 205, 205, 0.42);">
						<h3>Nuevo Ticket</h3>
					</div>
                    <div style="margin-bottom: 10px; margin-top: 20px;">
                        <asp:Button ID="btnCargar" Text="Cargar" class="formBtnNar1" runat="server" OnClick="btnCargar_Click"  />
                    </div>
                </div>

				<div class="sideGroup">
					<div class="sideHeader">
						<h3>Manuales</h3>
						<ul class="options">
							<li><a href="DetalleManual.aspx?id=" class="optAdd">Añadir</a></li>
							<li><a href="Manual.aspx" class="optMore">Ver+</a></li>
						</ul>
					</div>
					<ul id="manuales">
                         <asp:ListView ID="lvManuales" runat="server">
                            <ItemTemplate>              
                                <li> 
                                    <div>                        
                                        <a href="DetalleManual.aspx?id=<%# Eval("id") %>"><asp:Label ID="lbTitulo" runat="Server" Text='<%#Eval("titulo") %>' /></a>
                                        <p><asp:Label ID="lbEmpresa" runat="server" Text='<%#Eval("GetEmpresa") %>' /></p>
                                        <span class="footer"><asp:Label ID="lbUsuario" runat="Server" Text='<%#Eval("GetUsuario") %>' />, <asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:d}") %>' /></span>  
                                        <span class="manual-ico"></span>
                                    </div>
                                </li>
                            </ItemTemplate>                          
                        </asp:ListView>
					</ul>
				</div>

                <%--<asp:Panel ID="pnlNuevaNovedad" runat="server" Width="450px" HorizontalAlign="Center" CssClass="ModalPopup">
                    <section>
                        <table width="100%">
                                <asp:Label ID="lblClose" Text="X" runat="server" CssClass="closebtn" ></asp:Label>
                            <tr>
                                <td><b>Fecha</b></td>
                                <td>
                                    <asp:TextBox ID="txtFecha" runat="server" />
                                    <ajax:CalendarExtender ID="CalendarInsert" TargetControlID="txtFecha" Format="d/MM/yyyy" runat="server" CssClass="MyCalendar"/>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Descripción:</b></td>
                                <td><asp:TextBox ID="txtDescripcion" runat="server" Width="204px" 
                                        MaxLength="64" TextMode="MultiLine"/></td>
                            </tr>
                            
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </section>
                </asp:Panel>   
                <ajax:ModalPopupExtender ID="ModalPopupExtender" runat="server" 
                    TargetControlID="btnAdd"
                    PopupControlID="pnlNuevaNovedad" 
                    CancelControlID="lblClose"
                    BackgroundCssClass="ModalBackground"
                    DropShadow="true" />--%>
			</aside>