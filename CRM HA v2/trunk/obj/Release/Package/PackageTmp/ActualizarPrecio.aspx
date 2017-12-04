<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ActualizarPrecio.aspx.cs" Inherits="crm.ActualizarPrecio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var updateProgress = null;

    function postbackButtonClick() {
        updateProgress = $find("<%= UpdateProgress1.ClientID %>");
        window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
        return true;
    }
</script>
    
<script>
    function fileSelected() {
        $("#ctl00_ContentPlaceHolder1_btnProcesar").removeAttr("disabled");
        $("#ctl00_ContentPlaceHolder1_btnProcesar").removeClass("formBtnGrey2");
        $("#ctl00_ContentPlaceHolder1_btnProcesar").addClass("formBtnGrey1");
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
            
    <div id="maincol" style="height:600px"> 
        <section>
            <h2>Actualización de precios</h2>
            <div class="formHolder" style="padding: 22px 25px 12px;">
                <div>
                    <div>
                        <div style="float:left;width: 5%;"><modaltitle style="text-align:left;">Paso 1:</modaltitle></div>                                
                        <div align="left" class="h7" style="float:right;width: 94%;margin-top: 9px;">descargue el archivo de Excel con las unidades disponibles que podrán se actualizadasr</div>          
                    </div>
                    <br /><br /><hr class="line marginBottom20"/>
                    <div style="float:left; margin-bottom: 4px;">
                        <asp:LinkButton ID="btnExportar" runat="server" CssClass="formBtnGrey1" Text="Exportar unidades" OnClick="btnExportar_Click"></asp:LinkButton>    
                    </div>  
                </div>
            </div>
        </section>

        <section>
            <div class="formHolder" style="padding: 22px 25px 12px;">
                <div>
                    <div>
                        <div style="float:left;width: 5%;"><modaltitle style="text-align:left;">Paso 2:</modaltitle></div>  
                        <div align="left" class="h7" style="float:right;width: 94%;margin-top: 9px;">ingrese en la columna "VALOR" del Excel el nuevo importe </div>                               
                    </div>
                    <br /><br /><hr class="line marginBottom20"/>
                </div>
            </div>
        </section>

        <section>
            <div class="formHolder" style="padding: 22px 25px 12px;">
                <div>
                    <div>
                        <div style="float:left;width: 5%;"><modaltitle style="text-align:left;">Paso 3:</modaltitle></div>  
                        <div align="left" class="h7" style="float:right;width: 94%;margin-top: 9px;">seleccione el archivo actualizado y a continuación haga click en Procesar</div>                                
                    </div>
                    <br /><br /><hr class="line marginBottom20"/>
                    <asp:UpdatePanel ID="pnlCargaMasiva" runat="server" style="display:block">
                        <ContentTemplate>
                            <div>                        
                                <div>
                                    <label>
                                        <span visible="false">ARCHIVO</span>
                                        <label style="width:70%" class="FileUpload">
                                            <asp:FileUpload ID="btnFileUpload" onchange="fileSelected()" hasfile="false" runat="server" style="margin-top: -3px;"/>                               
                                        </label>                                      
                                    </label>
                                    <label>
                                        <asp:Button ID="btnProcesar" runat="server" Text="Procesar" CssClass="formBtnNar" style="float:left" OnClick="btnProcesar_Click" OnClientClick="return postbackButtonClick();"/>
                                    </label>
                                </div>                           
                            </div>  
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnProcesar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </section>

        <asp:Panel ID="pnlOkExcel" runat="server" Visible="false">
            <section>
                <div runat="server" class="formHolder messageOk" style="padding: 14px 25px 12px; background-color: #dff0d8; border-color: #d6e9c6;">
                    <div>
                        <asp:Label ID="lbMensaje" runat="server" CssClass="messageOk"></asp:Label>
                    </div>
                </div>
            </section>
        </asp:Panel>

        <asp:Panel ID="pnlErrorExcel" runat="server" Visible="false"> 
            <section>
                <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px;">
                    <div style="margin-left: 7px;">
                        <asp:Label ID="lbMensajeError" runat="server"></asp:Label>
                    </div>
                </div>
            </section>
        </asp:Panel>

        <section>
            <div runat="server" class="formHolder" style="padding: 22px 14px 12px;">
                <div runat="server">
                    <div style="float:left">
                        <asp:Button ID="btnVolver" Text="Volver al listado de unidades" CssClass="formBtnGrey1" runat="server" style="float:right; margin-left:15px; margin-top: -5px;" Onclick="btnVolver_Click" />
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
                        <div style="float:left;"><img src="images/ring_loading.gif"  width="300px" class="loading100" ImageAlign="left" /></div>
                        <div style="float:left; padding: 8px 0 0 10px">
                            <h2> Procesando... </h2>
                        </div>                                    
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
</asp:Content>
