<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePostVenta.Master" AutoEventWireup="true" CodeBehind="PostVenta.aspx.cs" Inherits="crm.PostVenta" %>
<%@ Register Src="~/sidebar.ascx" TagName="sidebar" TagPrefix="crm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" /> 
    <asp:UpdatePanel ID="pnlConsulta" runat="server" style="display:block">
        <ContentTemplate>
            <div id="maincol" style="height:600px">        
                <section>
                    <h2>SERVICIO POST-VENTA</h2>
                        <div class="formHolder" style="padding: 22px 25px 12px;"> 
                            <label>
                                <span visible="false">OBRA</span>
                                <asp:DropDownList ID="cbProyecto" runat="server" CssClass="txtFocus" TabIndex="1" style="width: 364px;"></asp:DropDownList> 
                                <asp:RequiredFieldValidator ID="RequiredFieldGroups" ValidationGroup="DatosTickets" ErrorMessage="Seleccione una obra..." style="background-color: #f2dede; border-radius: 3px;
                                        border: 1px solid rgb(242, 222, 222); width: 352px;"
                                        ControlToValidate="cbProyecto" InitialValue="0" runat="server" Display="Dynamic" />
                            </label> 

                            <label>
                                <span>UNIDAD FUNCIONAL</span> 
                                <asp:TextBox ID="txtUF" name="txtSearch" runat="server" CssClass="textBox textBoxForm txtFocus" style="width: 60%" TabIndex="3"></asp:TextBox> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="DatosTickets" ErrorMessage="Ingrese su unidad funcional" 
                                    style="background-color: #f2dede; border-radius: 3px; border: 1px solid rgb(242, 222, 222); width: 352px; margin-left: 168px;"
                                    ControlToValidate="txtUF" runat="server" Display="Dynamic" CssClass="borderValidator" />
                            </label>

                            <label>
                                <span visible="false">NOMBRE Y APELLIDO</span>
                                <asp:TextBox ID="txtSearch" name="txtSearch" runat="server" CssClass="textBox textBoxForm txtFocus" style="width: 60%" TabIndex="3"></asp:TextBox> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="DatosTickets" ErrorMessage="Ingrese su nombre y apellido" 
                                    style="background-color: #f2dede; border-radius: 3px; border: 1px solid rgb(242, 222, 222); width: 352px; margin-left: 168px;"
                                    ControlToValidate="txtSearch" runat="server" Display="Dynamic" CssClass="borderValidator" />      
                            </label>                          
                    
                            <label>
                                <span visible="false">MAIL</span>
                                <asp:TextBox ID="txtMail" name="txtSearch" runat="server" CssClass="textBox textBoxForm txtFocus" style="width: 60%" TabIndex="3"></asp:TextBox> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="DatosTickets" ErrorMessage="Ingrese su mail"
                                    style="background-color: #f2dede; border-radius: 3px; border: 1px solid rgb(242, 222, 222); width: 352px; margin-left: 168px;"
                                    ControlToValidate="txtMail" runat="server" Display="Dynamic" /> 
                            </label>

                            <label>
                                <span>TELÉFONO</span>
                                <asp:TextBox ID="txtTelefono" name="txtSearch" runat="server" CssClass="textBox textBoxForm txtFocus" style="width: 60%" TabIndex="3"></asp:TextBox> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="DatosTickets" ErrorMessage="Ingrese su teléfono"
                                    style="background-color: #f2dede; border-radius: 3px; border: 1px solid rgb(242, 222, 222); width: 352px; margin-left: 168px;"
                                    ControlToValidate="txtTelefono" runat="server" Display="Dynamic" />  
                            </label>
                        </div>
                </section>

                <section>
                    <div class="formHolder">
                        <label style="width:134%" >
                            <span style="width:157px">DESCRIPCION</span>
                            <asp:TextBox ID="txtDescripcion" Rows="3" CssClass="textBox textBoxForm txtFocus" TextMode="MultiLine" runat="server" TabIndex="7" Width="957px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="DatosTickets" ErrorMessage="Ingrese una descripción"
                                style="background-color: #f2dede; border-radius: 3px; border: 1px solid rgb(242, 222, 222); width: 957px; margin-left: 168px;"
                                ControlToValidate="txtDescripcion" runat="server" Display="Dynamic" />
                        </label>    
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div style="float:left">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlConsulta">
            <ProgressTemplate>
                <div class="overlay">
                    <div class="overlayContent">
                        <div style="float:left;"><img src="images/ring_loading.gif"  width="300px" class="loading100" ImageAlign="left"  /></div>
                        <div style="float:left; padding: 8px 0 0 10px; margin-left: 120px; margin-top: -98px;">
                            <h2 style="width: 400px;"> Enviando su consulta... </h2>
                        </div>                                    
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div> 
</asp:Content>

