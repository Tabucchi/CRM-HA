<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CargaUnidad.aspx.cs" Inherits="crm.CargaUnidad" %>
 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/masterStyle.css" rel="stylesheet" />
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
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" />
    
    <div id="maincol" style="height:600px; width: 100%;">        
        <section>
            <h2>Carga de unidades</h2>
                <div class="formHolder" style="padding: 22px 25px 12px;"> 
                    <div>
                        <div>
                            <div style="float:left;width: 5%;"><modaltitle style="text-align:left;">Paso 1:</modaltitle></div>
                            <div align="left" class="h7" style="float:right;width: 94%;margin-top: 9px;">seleccione la moneda con la cual se cotizará, y el tipo de unidad que contine la obra</div>
                        </div>
                    </div>
                    <br /><br /><hr class="line marginBottom20"/>
                    <div class="borderSeparatorRight" style="float:left; width: 263px;">
                        <label style="width: 65%;">
                            Seleccione el tipo de moneda:<br />
                            <asp:DropDownList ID="cbMoneda" runat="server" TabIndex="1" style="width: 218px; margin-top: 2px;">
                                <asp:ListItem Value="0">Dólar (u$s)</asp:ListItem>
                                <asp:ListItem Value="1">Pesos ($)</asp:ListItem>
                            </asp:DropDownList>
                        </label> 
                    </div>
                    <div class="borderSeparatorRight" style="float:left; width: 460px;">
                        <label style="margin-left: 50px;">
                            Tipo de unidad:
                            <asp:DropDownList ID="cbUnidadFuncional" runat="server" TabIndex="1" style="width: 367px; margin-top: 2px;"/>
                            <asp:RequiredFieldValidator id="rfvUnidadFuncional" runat="server" style="width: 353px; border-top-right-radius: 3px;
                                border-bottom-right-radius: 3px; border-right: 1px dotted #ebccd1;"
                                ControlToValidate="cbUnidadFuncional"
                                ErrorMessage="Seleccione una unidad funcional"
                                InitialValue="0"
                                Validationgroup="DatosTickets"
                                Display="Dynamic" SetFocusOnError="True" CssClass="alert borderRightValidator" >
                            </asp:RequiredFieldValidator>
                        </label> 
                    </div>
                    <div style="float:right">
                        <label style="width: 392px; margin-left: 50px; margin-right: 12px;">
                            Proporción de de semi cubiertos (%)
                            <asp:TextBox ID="txtProporcion" runat="server" Width="250px" CssClass="textBox textBoxForm" Text="0" onkeypress="return onKeyDecimal(event,this);"></asp:TextBox>
                        </label> 
                    </div>
                </div>
        </section>
             
        <section>
            <div class="formHolder"  style="padding: 22px 25px 0px;">
                <div>
                    <div>
                        <div style="float:left;width: 5%;"><modaltitle style="text-align:left;">Paso 2:</modaltitle></div>
                        <div align="left" class="h7" style="float:right;width: 94%;margin-top: 9px;">descargue el archivo con el formato correspondiente para cargar las unidades</div>
                    </div>
                </div>
                <br /><br /><hr class="line marginBottom20"/>
                <div style="float:left; margin-bottom: 20px;">
                    <asp:LinkButton ID="btnExportar" runat="server" CssClass="formBtnGrey1" Text="Descargar" OnClick="btnExportar_Click"></asp:LinkButton>    
                </div> 
            </div>
        </section>   

        <section>
            <div class="formHolder"  style="padding: 22px 25px 0px;">
                <div>
                    <div>
                        <div style="float:left;width: 5%;"><modaltitle style="text-align:left;">Paso 3:</modaltitle></div>
                        <div align="left" class="h7" style="float:right;width: 94%;margin-top: 9px;">seleccione el archivo y presione en el botón Cargar</div>
                    </div>
                </div>
                <br /><br /><hr class="line marginBottom20"/>
                <asp:UpdatePanel ID="pnlCargaMasiva" runat="server" style="display:block">
                    <ContentTemplate>
                        <label>
                            <span visible="false">ARCHIVO</span>
                            <label style="width:70%" class="FileUpload">
                                <asp:FileUpload ID="fileArchivo2" runat="server" style="margin-top: -3px;"/>
                            </label>                                      
                        </label> 
                        <label>
                            <asp:Button ID="btnCargar" Text="Cargar" CssClass="formBtnNar" runat="server" style="float:left" ValidationGroup="DatosTickets" OnClick="btnCargar_Click" OnClientClick="return postbackButtonClick();"/>
                        </label> 
                        <label>
                            <asp:RequiredFieldValidator ID="rfvUnidades"  ValidationGroup="DatosTickets" style="width: 656px; margin-top: -20px; border-top-right-radius: 3px;
                                border-bottom-right-radius: 3px; border-right: 1px dotted #ebccd1;"
                                runat="server"
                                ErrorMessage="Ingrese el archivo con las unidades que desea registrar" ControlToValidate="fileArchivo2"
                                Display="Dynamic" SetFocusOnError="True" CssClass="alert" />
                        </label>
                    </ContentTemplate>                            
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnCargar" />
                    </Triggers>
                </asp:UpdatePanel>                                            
            </div>
        </section>
                
        <asp:Panel ID="pnlOkExcel" runat="server" Visible="false">
            <section>
                <div runat="server" class="formHolder messageOk" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                    <div>
                        <asp:Label ID="lbMensaje" runat="server" CssClass="messageOk" style="margin-left: 7px;"></asp:Label>
                    </div>
                </div>
            </section>
        </asp:Panel>

        <asp:Panel ID="pnlErrorExcel" runat="server" Visible="false"> 
            <section>
                <div runat="server" class="formHolderAlert alert" style="padding: 22px 25px 12px;">
                    <div style="margin-left: 7px;"><h2>Errores en la carga del Excel:</h2></div>
                    <div style="margin-left: 7px;">
                        <asp:Literal id="litMarkup" runat="server"/>
                    </div>
                </div>
            </section>
        </asp:Panel>

        <section>
            <div runat="server" class="formHolder" style="padding: 22px 14px 12px;">
                <div runat="server">
                    <div style="float:left">
                        <asp:Button ID="btnVolver" Text="Volver al listado de unidades" CssClass="formBtnGrey1" runat="server" style="float:right; margin-left:15px; margin-top: -5px;" Onclick="btnVolver_Click"/>
                    </div>                            
                </div>
            </div>
        </section>
    </div>    
        
    <div style="float:left">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlCargaMasiva">
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

</asp:Content>

