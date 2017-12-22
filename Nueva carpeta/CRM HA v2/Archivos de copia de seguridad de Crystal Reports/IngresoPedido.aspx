 <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="IngresoPedido" Culture="auto" UICulture="auto" CodeBehind="IngresoPedido.aspx.cs" %>

<%@ Register Src="~/sidebar.ascx" TagName="sidebar" TagPrefix="crm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
    <style type="text/css">
        .UpdateProgressContent {
            padding: 40px;
            border: 1px dashed #C0C0C0;
            background-color: #FFFFFF;
            width: 400px;
            text-align: center;
            vertical-align: bottom;
            z-index: 1001;
            top: 34%;
            margin: 0px;
            margin-left: -245px;
            position: absolute;
        }

        .UpdateProgressBackground {
            margin: 0px;
            padding: 0px;
            top: 0px;
            bottom: 0px;
            left: 0px;
            right: 0px;
            position: absolute;
            z-index: 1000;
            background-color: #cccccc;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>

    <link href="css/orange.css" rel="stylesheet" />
    
    <script src="js/jquery-1.10.2.js"></script>
    <script src="js/jquery.ui.core.js"></script>
    <script src="js/jquery.ui.widget.js"></script>

    <script src="js/autocomplete/jquery-1.4.1.min.js"></script>
    <link href="js/autocomplete/jquery.autocomplete.css" rel="stylesheet" />
    <script src="js/autocomplete/jquery.autocomplete.js"></script>
    
    <script>
        $(document).ready(function () {
            $("input").focusout(function () {
                document.getElementById('<%=btnResponsable.ClientID%>').click();
            });
        });
    </script>
        
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtSearch.ClientID%>").autocomplete('/Web-Service/Search_CS.ashx');

            $("#<%=txtSearch.ClientID%>").focusout(function () {
                document.getElementById('<%=btnResponsable.ClientID%>').click();
            });
        });
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" /> 
    <div id="maincol" style="height:600px">        
        <section>
            <h2>INFORMACIÓN DEL TICKET</h2>
                <div class="formHolder" style="padding: 22px 25px 12px;"> 
                    <label>
                        <span visible="false">OBRA</span>
                        <asp:DropDownList ID="cbProyecto" runat="server" CssClass="txtFocus" TabIndex="1">
                        </asp:DropDownList> 
                        <asp:RequiredFieldValidator ID="RequiredFieldGroups" ValidationGroup="DatosTickets" ErrorMessage="Seleccione una obra"
                                ControlToValidate="cbProyecto" InitialValue="0" runat="server" Display="Dynamic" />
                    </label> 

                    <label>
                        <span visible="false">CLIENTE</span>
                        <asp:TextBox ID="txtSearch" name="txtSearch" runat="server" class="txtFocus" style="width: 60%" TabIndex="2"></asp:TextBox>                                         
                        <asp:LinkButton ID="btnNuevoCliente" runat="server" class="addUser" Text="Actualizar" ToolTip="Agregar nuevo cliente" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="DatosTickets" ErrorMessage="Ingrese un cliente" style="margin-left:169px"
                                ControlToValidate="txtSearch" runat="server" Display="Dynamic" />      
                    </label>                         
                             
                    <label>
                        <span>CATEGORIA</span> 
                        <asp:DropDownList ID="cbCategoria" runat="server" CssClass="txtFocus" TabIndex="3">
                            <asp:ListItem Text="Seleccione una Categoría..." Value="0" />
                        </asp:DropDownList> 
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="DatosTickets" ErrorMessage="Seleccione una categoría"
                                ControlToValidate="cbCategoria" InitialValue="0" runat="server" Display="Dynamic" />
                    </label>
                    <label></label>

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate> 
                    <label>
                        <span>PRIORIDAD</span>
                        <asp:DropDownList ID="cbPrioridad" runat="server" class="txtFocus" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="cbPrioridad_SelectedIndexChanged"></asp:DropDownList>
                    </label>

                    <label>
                        <span>FECHA LIMITE</span>
                        <label style="line-height: 14px;">
                            <asp:TextBox ID="txtFechaHasta" runat="server" style="width:350px"></asp:TextBox>
                            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="orange" TargetControlID="txtFechaHasta" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />
                        </label> 
                    </label>      
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnResponsable" EventName="Click"/>
                    </Triggers>
                </asp:UpdatePanel> 
                    
                </div>
         
                <div>
                    <asp:Panel ID="pnlNuevoCliente" runat="server" Width="410px" HorizontalAlign="Center" CssClass="ModalPopup">
                        <table style="width:100%">
                            <asp:Label ID="lblClose1" Text="X" runat="server" CssClass="closebtn"></asp:Label>
                            <tr>
                                <td colspan="2" style="color: #706F6F; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; text-align: center;"><b>NUEVO CLIENTE</b></td>
                            </tr>
                            <tr>
                                <td style="color: #706F6F; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; text-align: right; width:25%">NOMBRE:</td>
                                <td style="width:75%;" >
                                    <asp:TextBox ID="txtNombreCliente" runat="server" CssClass="textbox" Width="200px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="color: #706F6F; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; text-align: right; width:25%">EMPRESA:</td>
                                <td style="width:75%">
                                    <asp:DropDownList ID="cbEmpresa" Width="214px" runat="server">
                                        <asp:ListItem Text="Todas" Value="0" />
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="color: #706F6F; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; text-align: right; width:25%">TELÉFONO:</td>
                                <td style="width:75%">
                                    <asp:TextBox ID="txtInternoCliente" runat="server" CssClass="textbox" Width="200px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="color: #706F6F; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; text-align: right; width:25%">MAIL:</td>
                                <td style="width:75%">
                                    <asp:TextBox ID="txtMailCliente" runat="server" CssClass="textbox" Width="200px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="padding-left: 71px">
                                    <asp:Button ID="btnAceptarCliente" runat="server" Text="Aceptar" OnClick="btnAceptarCliente_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <Ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                        TargetControlID="btnNuevoCliente"
                        PopupControlID="pnlNuevoCliente"
                        CancelControlID="lblClose1"
                        BackgroundCssClass="ModalBackground"
                        DropShadow="true" />
                </div>

                <div>
                    <asp:Panel ID="pnlAgregarMail" runat="server" Width="410px" HorizontalAlign="Center" CssClass="ModalPopup">
                        <table style="width:100%">
                            <asp:Label ID="lblClose2" Text="X" runat="server" CssClass="closebtn"></asp:Label>
                            <tr>
                                <td colspan="2" style="color: #706F6F; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; text-align: center;"><b>El cliente no poseé un E-Mail registrado</b></td>
                            </tr>
                            <tr>
                                <td style="color: #706F6F; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; text-align: right; width:25%">Cliente:</td>
                                <td style="width:75%"><asp:Label ID="lblCliente" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="color: #706F6F; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; text-align: right; width:25%">Mail:</td>
                                <td style="width:75%">
                                    <asp:TextBox ID="txtNewMail" runat="server" CssClass="textbox" Width="200px"></asp:TextBox>
                                    <asp:Label ID="Label1" CssClass="tituloMensaje" runat="server" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:75%" colspan="2"><asp:Label ID="lblMensajeErrorNewMail" CssClass="tituloMensaje" runat="server" style="margin-left: 103px;"></asp:Label></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="padding-left: 71px">
                                    <asp:Button ID="btnAgregarMail" runat="server" Text="Aceptar" OnClick="btnAgregarMail_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <Ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server"
                        TargetControlID="HiddenField1"
                        PopupControlID="pnlAgregarMail"
                        CancelControlID="lblClose2"
                        BackgroundCssClass="ModalBackground"
                        DropShadow="true" />
                </div>
        </section>

        <section>
            <div class="formHolder">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate> 
                        <label>
                            <span>TITULO</span>
                            <asp:TextBox ID="txtTitulo" runat="server" CssClass="textbox txtFocus" Width="60%" TabIndex="6"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="DatosTickets" ErrorMessage="Ingrese un título" style="margin-left:169px"
                                ControlToValidate="txtTitulo" runat="server" Display="Dynamic" />
                        </label>
                        <label class="rigthLabel">
                            <asp:Label ID="lbInfoAdicionalResponsable" runat="server">RESPONSABLE</asp:Label>
                            <asp:DropDownList ID="cbResponsable" runat="server" TabIndex="6" CssClass="txtFocus">
                                <asp:ListItem Text="Seleccione un Responsable..." Value="0" />
                            </asp:DropDownList> 
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="DatosTickets" ErrorMessage="Seleccione un responsable"
                                ControlToValidate="cbResponsable" InitialValue="0" runat="server" Display="Dynamic" /> 
                        </label>
                        <label style="width:134%" >
                            <span style="width:157px">DESCRIPCION</span>
                            <asp:TextBox ID="txtDescripcion" Rows="3" CssClass="textbox txtFocus" TextMode="MultiLine" runat="server" TabIndex="7" Width="957px"></asp:TextBox>
                            <br /><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="DatosTickets" ErrorMessage="Ingrese una descripción" style="margin-left:169px; margin-bottom:5px"
                                ControlToValidate="txtDescripcion" runat="server" Display="Dynamic" />
                        </label>  
                        </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnResponsable" EventName="Click"/>
                    </Triggers>
                </asp:UpdatePanel>     
            </div>

            <div class="formHolder">                            
                <label style="font-size: x-small; color: #FF0000; font-weight: bold">                    
                    Todos los campos son obligatorios.
                </label>
                <label class="rigthLabel">                    
                    <asp:Button ID="btnCargar" Text="Cargar" class="formBtnNar" runat="server"  OnClick="btnCargar_Click" ValidationGroup="DatosTickets" />                     
                </label>
            </div>
        </section>
        
        <asp:Button ID="btnResponsable" runat="server" Text="Button"  OnClick="btnResponsable_Click" style="visibility:hidden; width:inherit"/> 
    </div>
</asp:Content>

