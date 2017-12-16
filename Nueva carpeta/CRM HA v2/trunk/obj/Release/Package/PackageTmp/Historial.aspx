<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Historial.aspx.cs" Inherits="crm.Historial" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
         
    <asp:UpdatePanel ID="pnlHistorial" runat="server">
        <ContentTemplate>
            <section>
                <div class="formHolder" id="searchBoxTop">
                    <div class="headOptions headLine">
                        <div style="float:left"><h2>Historial</h2></div>
                        <div style="float:right; padding-top: 5px;">
                            <div style="float:left; margin-right: 6px;">
                                <label style="width:20%">
                                    <asp:DropDownList ID="cbProyecto" Width="230px" runat="server" TabIndex="1"></asp:DropDownList>
                                </label>
                            </div>
                            <div style="float:right">
                                <label style="width:20%">
                                    <asp:DropDownList ID="cbHistorial" style="width: 230px; margin-right: 82px;" runat="server" TabIndex="2">
                                        <asp:ListItem Value="">Seleccione el motivo...</asp:ListItem>
                                        <asp:ListItem Value="Precio">Evaluación de precios</asp:ListItem>
                                        <asp:ListItem Value="Unidad_modificada">Seleccione el motivo...</asp:ListItem>
                                    </asp:DropDownList>
                                </label>
                                <asp:Button ID="btnBuscar" Text="Buscar" CssClass="formBtnNar" runat="server" style="margin-left: -90px; margin-top: -1px;" OnClick="btnBuscar_Click" />
                            </div>                           
                        </div>
                    </div>
                </div>          
            </section> 
               
            <asp:Panel ID="pnlUnidades" runat="server" Visible="false">
                <section>
                    <div class="formHolder" style="padding: 2px 25px 0px;">   
                        <div><modaltitle style="text-align:left; font-size: 16px !important;">Filtrar por unidad</modaltitle></div>    
                        <hr class="line">                                     
                        <div class="divFormInLine">
                            <div>
                                <label>
                                    Nivel:
                                    <asp:DropDownList ID="cbNivel" runat="server" CssClass="dropDownList" TabIndex="3" style="width: 270px; margin-top: 2px;" AutoPostBack="true" OnSelectedIndexChanged="cbNivel_SelectedIndexChanged">
                                        <asp:ListItem>Seleccione un nivel...</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator id="rfv4" runat="server" style="width: 256px;"
                                        ControlToValidate="cbNivel" InitialValue="-1" ErrorMessage="Seleccione un nivel"
                                        Validationgroup="CustomerUnidadOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                    </asp:RequiredFieldValidator>
                                </label> 
                            </div>                        
                        </div>

                        <div class="divFormInLine">
                            <div>
                                <label>
                                    Nro. de Unidad:
                                    <asp:DropDownList ID="cbUnidad" runat="server" CssClass="dropDownList" TabIndex="4" style="width: 270px; margin-top: 2px;" Enabled="false">
                                        <asp:ListItem>Seleccione una unidad...</asp:ListItem>
                                    </asp:DropDownList>
                                        <asp:RequiredFieldValidator id="rfv5" runat="server" style="width: 256px;"
                                        ControlToValidate="cbUnidad" InitialValue="0" ErrorMessage="Seleccione una unidad"
                                        Validationgroup="CustomerUnidadOV" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                                    </asp:RequiredFieldValidator>
                                </label>
                            </div>                           
                        </div>

                        <div class="divFormInLine">
                            <div style="float:left">
                                <label style="margin-top: 35px;"><asp:Button ID="btnBuscarUnidad" Text="Buscar" CssClass="formBtnNar" runat="server" style="float:left; margin-top: -4px;" Validationgroup="CustomerUnidadOV" OnClick="btnBuscarUnidad_Click"/></label>
                            </div>
                        </div>
                        <div class="divFormInLine" style="width: 315px !important;">
                            <div style="float:right; width: 160px">
                                <label style="margin-top: 35px;">
                                    <asp:Button ID="btnVerTodos" Text="Ver todos" CssClass="formBtnGrey1" runat="server" style="margin-left: -14px; margin-top: -4px;" OnClick="btnVerTodos_Click"/>
                                </label>
                                <label style="margin-top: 35px;">
                                    <asp:LinkButton ID="btnImprimir" Text="Imprimir" CssClass="formBtnNar" runat="server" style="float:left; margin-top: -4px;" OnClick="btnImprimir_Click"/>
                                </label>
                            </div>
                        </div>
                    </div>
                </section> 
            </asp:Panel>

            <asp:Panel ID="pnlMensaje" runat="server"> 
                <section>
                    <div runat="server" class="formHolderMessage" style="padding: 15px 21px 15px;">
                        <div class="messageFont">Seleccione una obra y el motivo de la búsqueda</div>
                    </div>
                </section>
            </asp:Panel>

            <asp:ListView ID="lvHistorial" runat="server">
                <LayoutTemplate>
                    <section>
                        <table style="margin-top:-25px">
                            <thead id="tableHead">
                                <tr>
                                    <td style="width: 1%; text-align: center">FECHA</td>
                                    <td style="width: 14%; text-align: center">MOTIVO</td>
                                    <td style="width: 18%; text-align: center">PROYECTO</td>
                                    <td style="width: 6%; text-align: center">CÓDIGO U.F.</td>
                                    <td style="width: 6%; text-align: center">NRO. UNIDAD</td>
                                    <td style="width: 11%; text-align: center">VALOR ORIGINAL</td>
                                    <td style="width: 11%; text-align: center">ÚLTIMA MODIFICACIÓN <sub style="vertical-align: sub;font-size: smaller;">1</sub></td>
                                    <td style="width: 11%; text-align: center">VALOR NUEVO <sub style="vertical-align: sub;font-size: smaller;">2</sub></td>
                                    <td style="width: 8%; text-align: center">% <sub style="vertical-align: sub;font-size: smaller;">(2/1)</sub></td>
                                    <td style="width: 14%; text-align: center">USUARIO</td>
                                    <td style="width: 1px; text-align: center"></td>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </tbody>
                        </table>
                    </section>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer">
                        <td style="text-align: center">
                            <asp:Label ID="Label11" runat="Server" Text='<%#Eval("Fecha", "{0:d}") %>'/>
                        </td> 
                        <td style="text-align: left">
                            <a href="#" alt="Motivo" class="tooltip tooltipColor">
                                <asp:Label ID="Label3" runat="Server" Text='<%#Eval("Motivo") %>'/>
                            </a>
                        </td>   
                        <td style="text-align: left">
                            <a href="#" alt="Proyecto" class="tooltip tooltipColor">
                                <asp:Label ID="Label4" runat="Server" Text='<%#Eval("GetProyecto") %>'/>
                            </a>
                        </td> 
                        <td style="text-align: center">
                            <a href="#" alt="Código U.F." class="tooltip tooltipColor">
                                <asp:Label ID="Label5" runat="Server" Text='<%#Eval("CodUF") %>'/>
                            </a>
                        </td>    
                        <td style="text-align: center">
                            <a href="#" alt="Nro. Unidad" class="tooltip tooltipColor">
                                <asp:Label ID="Label7" runat="Server" Text='<%#Eval("NroUnidad") %>'/>
                            </a>
                        </td>    
                        <td style="text-align: right">
                            <a href="#" alt="Valor antiguo" class="tooltip tooltipColor">
                                <asp:Label ID="Label13" runat="Server" Text='<%#Eval("GetPrecioOriginal", "{0:#,#}") %>'/>
                            </a>
                        </td>       
                        <td style="text-align: right">
                            <a href="#" alt="Valor antiguo" class="tooltip tooltipColor">
                                <asp:Label ID="Label1" runat="Server" Text='<%#Eval("ValorViejo", "{0:#,#}") %>'/>
                            </a>
                        </td>
                        <td style="text-align: right">
                            <a href="#" alt="Valor nuevo" class="tooltip tooltipColor">
                                <asp:Label ID="Label2" runat="Server" Text='<%#Eval("ValorNuevo",  "{0:#,#}") %>'/>
                            </a>
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="Label15" runat="Server" Text='<%#Eval("PorcentajePrecio",  "{0:#,#}") %>'/>
                        </td>

                        <td style="text-align: center">
                            <a href="#" alt="Usuario" class="tooltip tooltipColor">
                                <asp:Label ID="Label12" runat="Server" Text='<%#Eval("GetUsuario") %>'/>
                            </a>
                        </td>
                        <td>
                            <asp:LinkButton ID="btnDetalle" runat="server" CssClass="detailBtn" CommandName="Detalle" Text="Detalle" ToolTip="Detalle" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CodUF")%>'/>
                        </td>
                    </tr>
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
        </ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btnImprimir" />
            <asp:PostBackTrigger ControlID="btnVerTodos" />
        </Triggers>
    </asp:UpdatePanel>      
        
    <div style="float:left">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlHistorial">
            <ProgressTemplate>
                <div class="overlay">
                    <div class="overlayContent">
                        <div style="float:left;"><img src="images/ring_loading.gif"  width="300px" class="loading100" ImageAlign="left"  /></div>
                        <div style="float:left; padding: 8px 0 0 10px">
                            <h2> Buscando... </h2>
                        </div>                                    
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>         
    
    <CR:crystalreportviewer ID="CrystalReportViewer" runat="server" 
        AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource" 
        Width="894px" DisplayToolbar="False" Visible="false" />
    <CR:crystalreportsource ID="CrystalReportSource" runat="server" Visible="false">
        <Report FileName="Reportes/Historial.rpt"></Report>
    </CR:crystalreportsource>
</asp:Content>
