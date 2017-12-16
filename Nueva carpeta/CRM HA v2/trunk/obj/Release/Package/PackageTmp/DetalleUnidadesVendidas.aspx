<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="DetalleUnidadesVendidas.aspx.cs" Inherits="crm.DetalleUnidadesVendidas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/orange.css" rel="stylesheet" />
<%--<script type="text/javascript">
    var updateProgress = null;

    function postbackButtonClick() {
        updateProgress = $find("<%= UpdateProgress1.ClientID %>");
        window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
        return true;
    }
</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" />
    
    <%--<asp:UpdatePanel ID="pnlHistorial" runat="server">
        <ContentTemplate>--%>
            <section>
                <div class="headOptions">
                    <div style="float:left"><h2>Detalle Unidades Vendidas&nbsp;<asp:Label ID="lbProyecto" runat="server"></asp:Label></h2></div>
                    <div style="float:right">
                        <label class="rigthLabel" style="width: 37%;">
                            <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="formBtnNar" OnClick="btnDescargar_Click" />
                        </label>
                    </div>
                </div>
            </section>
            <%--<section>
                <div class="formHolder" id="searchBoxTop">
                    <div class="headOptions headLine">
                        <div style="float:left"><h2>Unidades Vendidas</h2></div>
                        <div style="float:right; padding-top: 5px;">
                            <div style="float:left; margin-right: 80px;">
                                <label style="width:20%">
                                    <asp:DropDownList ID="cbProyecto" Width="230px" runat="server" TabIndex="1"></asp:DropDownList>
                                </label>
                            </div>
                            <div style="float:right">
                                <asp:UpdatePanel ID="pnlBuscar" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBuscar" Text="Buscar" CssClass="formBtnNar" runat="server" style="margin-left: -90px; margin-top: -1px;" OnClick="btnBuscar_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>                           
                        </div>
                    </div>
                </div>          
            </section> --%>

            <%--<asp:Panel ID="pnlListView" runat="server">--%>
                <asp:ListView ID="lvUnidades" runat="server" OnItemCommand="lvUnidades_ItemCommand">
                    <LayoutTemplate>
                        <section>            
                            <table>                            
                                <thead id="tableHead">
                                    <tr>     
                                        <td class="fontListview" style="width: 20%; text-align: center">CLIENTE</td>
                                        <td class="fontListview" style="width: 10%; text-align: center">CÓDIGO U.F.</td>
                                        <td class="fontListview" style="width: 10%; text-align: center">NIVEL</td>
                                        <td class="fontListview" style="width: 10%; text-align: center">NRO. UNIDAD</td>
                                        <td class="fontListview" style="width: 10%; text-align: center">FECHA DEL BOLETO</td>    
                                        <td class="fontListview" style="width: 10%; text-align: center">FECHA DE POSESIÓN</td>
                                        <td class="fontListview" style="width: 10%; text-align: center">FECHA DE ESCRITURA</td>
                                        <td class="fontListview" style="width: 10%; text-align: center">EDITAR FECHA</td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                </tbody>                       
                            </table>
                        </section>   
                    </LayoutTemplate>
                
                    <ItemTemplate>                
                        <tr onclick="redir('<%# Eval("idProyecto") %>');" style="cursor: pointer">  
                            <td class="fontListview" style="text-align: center">
                                <asp:Label ID="lbCliente" runat="Server" Text='<%#Eval("GetEmpresa") %>' />
                            </td> 
                            <td class="fontListview" style="text-align: center">
                                <asp:Label ID="lbCodUF" runat="Server" Text='<%#Eval("CodigoUF") %>' />
                            </td>
                            <td class="fontListview" style="text-align: center">
                                <asp:Label ID="lbNivel" runat="Server" Text='<%#Eval("Nivel") %>' />
                            </td>
                            <td class="fontListview" style="text-align: center">
                                <asp:Label ID="lbNroUnidad" runat="Server" Text='<%#Eval("NroUnidad") %>' />
                            </td>
                            <td class="fontListview" style="text-align: center">
                                <asp:Label ID="lbFechaBoleto" runat="Server" Text='<%#Eval("GetFechaBoleto") %>' />
                            </td>  
                            <td class="fontListview" style="text-align: center">
                                <asp:Label ID="lbFechaPosesion" runat="Server" Text='<%#Eval("GetFechaPosesion") %>' />                    
                            </td> 
                            <td class="fontListview" style="text-align: center">
                                <asp:Label ID="lbFechaEscritura" runat="Server" Text='<%#Eval("GetFechaEscritura") %>' />                    
                            </td> 
                            <td>
                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="editBtn" CommandName="Editar" Text="Editar" ToolTip="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>'/>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            <%--</asp:Panel>--%>
        <%--</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDescargar" />
        </Triggers>
    </asp:UpdatePanel>--%>

    <section>
        <asp:UpdatePanel ID="pnlEditar" runat="server" style="width:410px" HorizontalAlign="Center" class="modal">
                <ContentTemplate>
                    <table width="100%">               
                        <tr>
                            <td colspan="2"><modalTitle><b>Editar Fechas</b></modalTitle></td>
                        </tr> 
                        <tr id="Tr1" runat="server">
                            <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                                Fecha de posesión: 
                            </td>
                            <td>
                                <asp:TextBox ID="txtEditFechaPosesion" runat="server" CssClass="textBoxForm"></asp:TextBox>  
                                <ajax:CalendarExtender ID="cefp" runat="server" CssClass="orange" TargetControlID="txtEditFechaPosesion" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />
                            </td>
                        </tr>
                        <tr id="Tr2" runat="server">
                            <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                                Fecha de escritura: 
                            </td>
                            <td>
                                <asp:TextBox ID="txtEditFechaEscritura" runat="server" CssClass="textBoxForm"></asp:TextBox> 
                                <ajax:CalendarExtender ID="cefe" runat="server" CssClass="orange" TargetControlID="txtEditFechaEscritura" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />
                            </td>
                        </tr>
                        <tr>   
                            <td colspan="2">
                                <div>
                                    <div align="right" style="float:right">
                                        <asp:Button ID="btnCerrarEditar" CssClass="btnClose" runat="server" Text="Cerrar" CommandArgument="Editar" OnClick="btnCerrarEditar_Click"/>
                                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="formBtnNar" style="margin-left:15px" CausesValidation="true" OnClick="btnGuardar_Click"/>
                                    </div>
                                </div>
                            </td>           
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>   
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <ajax:ModalPopupExtender ID="ModalEdit" runat="server" 
            TargetControlID="HiddenField1"
            PopupControlID="pnlEditar" 
            BackgroundCssClass="ModalBackground"
            DropShadow="true" /> 
    </section>

    <asp:HiddenField ID="hfId" runat="server" />


    <section>
        <asp:UpdatePanel ID="pnlMensaje" runat="server" style="width:410px" HorizontalAlign="Center" class="modal">
                <ContentTemplate>
                    <table width="100%">               
                        <tr>
                            <td colspan="2"><modalTitle><b>Aviso</b></modalTitle></td>
                        </tr> 
                        <tr runat="server">
                            <td style="width: 100%; color: #706F6F; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 17px; text-align: center; padding: 30px;">
                                La unidad seleccionda no posee boleto
                            </td>
                        </tr>
                        <tr>   
                            <td colspan="2">
                                <div>
                                    <div align="right" style="float:right">
                                        <asp:Button ID="btnCerrarMensaje" CssClass="btnClose" runat="server" Text="Cerrar" OnClick="btnCerrarMensaje_Click"/>
                                    </div>
                                </div>
                            </td>           
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>   
        <asp:HiddenField ID="HiddenField2" runat="server" />
        <ajax:ModalPopupExtender ID="ModalMensaje" runat="server" 
            TargetControlID="HiddenField2"
            PopupControlID="pnlMensaje" 
            BackgroundCssClass="ModalBackground"
            DropShadow="true" /> 
    </section>
        
        



























<CR:crystalreportviewer ID="CrystalReportViewer" runat="server" 
    AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource" 
    Width="894px" DisplayToolbar="False" Visible="false" />
<CR:crystalreportsource ID="CrystalReportSource" runat="server" 
    Visible="false">
    <Report FileName="Reportes/UnidadesVendidasPorObra.rpt"></Report>
</CR:crystalreportsource>

<%--<div style="float:left">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlBuscar">
        <ProgressTemplate>
            <div class="overlay">
                <div class="overlayContent">
                    <div style="float:left;"><img src="images/ring_loading.gif" width="300px" class="loading100" ImageAlign="left"  /></div>
                    <div style="float:left; padding: 8px 0 0 10px">
                        <h2> Buscando... </h2>
                    </div>                                    
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>--%>
</asp:Content>
