<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Configuracion.aspx.cs" Inherits="crm.Configuracion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/orange.css" rel="stylesheet" />
    <link href="../css/masterStyle.css" rel="stylesheet" />
    <link href="css/Datepicker.css" rel="stylesheet" />
    <style>
        .ui-datepicker-calendar {
        display: none;
        }
    </style>

    <script src="js/jquery-1.8.2.js"></script>
    <script src="js/jquery-ui.js"></script>
    
    <script type="text/javascript">
        $(function ($) {
            $.datepicker.regional['es'] = {
                closeText: 'Cerrar',
                prevText: '<Ant',
                nextText: 'Sig>',
                currentText: 'Hoy',
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
                weekHeader: 'Sm',
                dateFormat: 'dd/mm/yy',
                firstDay: 1,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: ''
            };
            $.datepicker.setDefaults($.datepicker.regional['es']);
        });

        $(function () {
            $('.datepicker').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'MM yy',
                onClose: function (dateText, inst) {
                    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    $(this).datepicker('setDate', new Date(year, month, 1));
                },
                beforeShow: function (input, inst) {
                    if ((datestr = $(this).val()).length > 0) {
                        year = datestr.substring(datestr.length - 4, datestr.length);
                        month = jQuery.inArray(datestr.substring(0, datestr.length - 5), $(this).datepicker('option', 'monthNames'));
                        $(this).datepicker('option', 'defaultDate', new Date(year, month, 1));
                        $(this).datepicker('setDate', new Date(year, month, 1));
                    }
                }
            });
        });
    </script>

    <script src="js/jquery.mask.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.decimal').mask("#.##0,00", { reverse: true });
        });
    </script>

    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress2.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
        
        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress4.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress5.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress6.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress7.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" /> 

<div>
    <section>
        <div class="headOptions">
            <div style="float:left"><h2>Índices</h2></div>
        </div>       
    </section>
</div>

<div style="width:100%; margin-top: -22px;">
    <div style="float:left; width:48%">
        <section>
            <div class="formHolder" id="searchBoxTop1">
                <div class="formHolderLine">
                    <h2>Valor Dólar</h2>
                </div>
            </div>
        </section>
        
        <asp:ListView ID="lvValorDolar" runat="server"  DataKeyNames="id">
            <LayoutTemplate>      
                <section style="overflow-y: scroll; overflow: auto; height: 723px;">     
                    <table>                            
                        <thead id="tableHead">
                            <tr>                           
                                <td style="width:50%">FECHA</td>
                                <td style="width:50%">VALOR</td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>                        
                    </table>
                </section>
            </LayoutTemplate>
            <ItemTemplate>                   
                <tr style="cursor:pointer;" onclick="Visible(<%# Eval("id")%> )" >
                    <td align="left">
                        <%#Eval("GetRegisterDate") %>
                    </td> 
                    <td style="width:50px">
                        <%#Eval("ValorDolar") %>
                    </td>
                </tr>
            </ItemTemplate>  
        
            <EmptyDataTemplate>
                <table runat="server">
                    <tr>
                        <td align="center"><b>No se encontró ningún valor.<b/></td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
    </div>

    <div style="float:right; width:48%">
        <div>
            <section>
            <div class="formHolder">
                <div class="formHolderLine">
                    <h2>Índice CAC</h2>
                    <div style="float:right">
                        <div style="float:left">
                            <asp:Panel ID="pnlbtnModificar" runat="server" Visible="false">
                                <asp:Button ID="btnModificarCAC" runat="server" CssClass="formBtnNar" Text="Modificar índice" style="border: 1px solid rgba(0, 0, 0, 0.2); margin-right: 15px;" OnClick="btnModificarCAC_Click"/>
                            </asp:Panel>
                        </div>
                        <div style="float:right">
                            <asp:Button ID="btnIngresar" runat="server" CssClass="formBtnNar" Text="Ingresar nuevo índice" style="border: 1px solid rgba(0, 0, 0, 0.2);" OnClick="btnIngresar_Click"/>
                        </div>
                    </div>
                </div>
            </div>
        </section>
            <asp:ListView ID="lvCAC" runat="server" 
            DataKeyNames="id" 
            OnPreRender="ListPager_PreRender">
            <LayoutTemplate>      
                <section style="overflow-y: scroll; overflow: auto; height: 304px;">     
                    <table>                            
                        <thead id="tableHead">
                            <tr>                           
                                <td>AÑO</td>
                                <td>MES</td>
                                <td>VALOR</td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>                        
                    </table>
                </section>
            </LayoutTemplate>
            <ItemTemplate>                   
                <tr style="cursor:pointer;" onclick="Visible(<%# Eval("id")%> )" >
                    <td align="left">
                        <asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' Enabled="False" Visible="false"/>
                        <asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("Fecha", "{0:yyyy}") %>' />
                    </td>
                    <td><asp:Label ID="lbDireccion" runat="Server" Text='<%#Eval("Fecha", "{0:MMMM}") %>' /></td>
                    <td><asp:Label ID="lbTelefono" runat="Server" Text='<%#Eval("valor") %>' /></td>  
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <table runat="server">
                    <tr>
                        <td align="center"><b>No se encontraron índices CAC<b/></td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
        </div>

        <div>
            <section>
                <div class="formHolder">
                    <div class="formHolderLine">
                        <h2>Índice UVA</h2>
                        <div style="float:right">
                            <%--<div style="float:left">
                                <asp:Button ID="btnRenovarUVA" runat="server" CssClass="formBtnNar" Text="Renovar servicio" style="border: 1px solid rgba(0, 0, 0, 0.2); margin-right: 10px;"/>
                            </div>--%>
                            <div style="float:left">
                                <asp:Panel ID="pnlbtnModificarUVA" runat="server" Visible="false">
                                    <asp:Button ID="btnModificarUVA" runat="server" CssClass="formBtnNar" Text="Modificar índice" style="border: 1px solid rgba(0, 0, 0, 0.2); margin-right: 15px;" OnClick="btnModificarUVA_Click"/>
                                </asp:Panel>
                            </div>
                            <div style="float:right">
                                <asp:Button ID="btnIngresarUVA" runat="server" CssClass="formBtnNar" Text="Ingresar nuevo índice" style="border: 1px solid rgba(0, 0, 0, 0.2);" OnClick="btnIngresarUVA_Click"/>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <asp:ListView ID="lvUVA" runat="server" 
            DataKeyNames="id" 
            OnPreRender="ListPager_PreRender">
            <LayoutTemplate>      
                <section style="overflow-y: scroll; overflow: auto; height: 304px;">     
                    <table>                            
                        <thead id="tableHead">
                            <tr>                           
                                <td>AÑO</td>
                                <td>MES</td>
                                <td>VALOR</td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>                        
                    </table>
                </section>
            </LayoutTemplate>
            <ItemTemplate>                   
                <tr style="cursor:pointer;" onclick="Visible(<%# Eval("id")%> )" >
                    <td align="left">
                        <asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' Enabled="False" Visible="false"/>
                        <asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("Fecha", "{0:yyyy}") %>' />
                    </td>
                    <td><asp:Label ID="lbDireccion" runat="Server" Text='<%#Eval("Fecha", "{0:MMMM}") %>' /></td>
                    <td><asp:Label ID="lbTelefono" runat="Server" Text='<%#Eval("valor") %>' /></td>  
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <table runat="server">
                    <tr>
                        <td align="center"><b>No encontraron índices UVA.<b/></td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
        </div>
    </div>
    
    <%--<section>
        <asp:UpdatePanel ID="pnlServicio" runat="server" style="width:640px" HorizontalAlign="Center" class="modal">
            <ContentTemplate>
                <table width="100%"> 
                    <asp:Label ID="lblCloseServicio" Text="X" runat="server" CssClass="closebtn1" style="right: -11px !important;"></asp:Label>             
                    <tr>
                        <td colspan="2"><modalTitle><b>Renovar servicio</b></modalTitle></td>
                    </tr> 
                    <tr id="filaQuienCerroTicket" runat="server">
                        <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            <b>Fecha de vencimiento:</b>
                        </td>
                        <td style="width:55%">
                            <asp:TextBox ID="txtFechaVenc" runat="server" CssClass="textBox" TabIndex="7" style="width:400px"></asp:TextBox>
                            <ajax:CalendarExtender ID="CalendarExtender8" runat="server" CssClass="orange" TargetControlID="txtFechaVenc" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                            <asp:RequiredFieldValidator id="rfv100" runat="server"
                                ControlToValidate="txtFechaVenc"
                                ErrorMessage="Campo obligatorio"
                                Validationgroup="vgServicio"
                                Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                style="width: 256px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                            </asp:RequiredFieldValidator>  
                        </td>
                    </tr>
                    <tr>   
                        <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            <b>Header:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHeader" CssClass="textBoxForm" Width="400px" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator id="rfv26" runat="server"
                                ControlToValidate="txtHeader" ErrorMessage="Agregue el header"  InitialValue="0"
                                Validationgroup="vgServicio" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator"
                                style="width: 256px; display: block; float: left; padding: 6px 0px 6px 12px; -webkit-border-top-left-radius: 3px; -webkit-border-bottom-left-radius: 3px; -moz-border-radius-topleft: 3px; -moz-border-radius-bottomleft: 3px; border-top-left-radius: 3px; border-bottom-left-radius: 3px;">
                            </asp:RequiredFieldValidator>
                        </td>           
                    </tr>
                    <tr class="spacer">
                        <td colspan="2" > <hr /> </td>
                    </tr>
                    <asp:Panel ID="pnlMensajeServicio" runat="server" Visible="false">
                        <tr>
                            <td colspan="2" style="height: 23px;">
                                <div runat="server" class="formHolderAlert alertGreyLighten" style="background-color: #e8f5e9 !important; padding: 14px 25px 12px; margin-top: -10px; margin-left: -12px; width: 95%;">
                                    <div>El servicio fue renovado exitosamente.</div>
                                </div>
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>            
                        <td colspan="2">
                            <div>
                                <div align="center" style="float:left">
                                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlServicio">
                                        <ProgressTemplate>
                                            <script type="text/javascript">document.write("<div class='UpdateProgressBackground'></div>");</script>
                                            <center>
                                                <div class="UpdateProgressContent">
                                                    <div style="float:left;"><img src="images/ring_loading.gif" width="300px" style="height: 30px; width:30px" ImageAlign="left"  /></div>
                                                    <div style="float:right; padding-left: 10px;">
                                                        <font style="font-size: 25px; color: #B7B7B7; font-weight: bold;"> Procesando... </font>
                                                    </div>                                    
                                                </div>
                                            </center>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <div style="float:right">
                                    <asp:Button ID="btnFinalizarServicio" runat="server" CssClass="formBtnNar" Text="Aceptar" Validationgroup="vgServicio" OnClick="btnFinalizarServicio_Click"/>
                                </div>
                            </div>
                        </td>            
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>   
        <ajax:ModalPopupExtender ID="mpeServicio" runat="server" 
            TargetControlID="btnRenovarUVA"
            PopupControlID="pnlServicio" 
            CancelControlID="lblCloseServicio"
            BackgroundCssClass="ModalBackground"
            DropShadow="true" /> 
    </section>--%>
    
    <div>
        <section>
        <asp:UpdatePanel ID="pnlIndiceCAC" runat="server" Width="410px" HorizontalAlign="Center" class="modal" style="background-color:white">
            <ContentTemplate>
                <table style="width: 100%">               
                    <asp:Label ID="lblCloseIndice" Text="X" runat="server" CssClass="closebtn1" style="right: -11px !important;"></asp:Label>
                    <tr>
                        <td colspan="2"><modalTitle><b>Ingrese el índice CAC</b></modalTitle></td>
                    </tr> 
                    <tr>
                        <td style="width:25%; font-size:18px; text-align:right;">
                            <b>Mes:</b>
                        </td>
                        <td>
                            <asp:Label ID="lbFechaIndice" runat="server" style="font-size: 18px;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Índice: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtIndiceCAC" CssClass="textBoxForm decimal" Width="400px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Confirme el índice: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmIndiceCAC" CssClass="textBoxForm decimal" Width="400px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="spacer">
                        <td colspan="2" > <hr /> </td>
                    </tr>
                    <asp:Panel ID="pnlMensajeIndiceCAC" runat="server" Visible="false">
                        <tr>
                            <td colspan="2" style="height: 23px;">
                                <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; margin-top: -10px; margin-left: -12px; width: 95%; display: table; margin: 0 auto;">
                                    <div><asp:Label ID="lbMensaje" runat="server"></asp:Label></div>
                                </div>
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>            
                        <td colspan="2">
                            <div align="center" style="display: table; margin: 0 auto;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlIndiceCAC">
                                    <ProgressTemplate>
                                        <script type="text/javascript">document.write("<div class='UpdateProgressBackground'></div>");</script>
                                        <center>
                                            <div class="UpdateProgressContent">
                                                <div style="float:left;"><img src="images/ring_loading.gif" width="300px" style="height: 30px; width:30px" ImageAlign="left"  /></div>
                                                <div style="float:right; padding: 0px 0 0 10px">
                                                    <font style="font-size: 25px; color: #B7B7B7; font-weight: bold;"> Procesando... </font>
                                                </div>                                    
                                            </div>
                                        </center>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="display: table; margin: 0 auto;">
                                <asp:Button ID="btnFinalizarIndice" runat="server" CssClass="formBtnNar" Text="Aceptar" OnClick="btnFinalizarIndice_Click"/>
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" style="margin-left:15px; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; border: 1px solid #ccc; background-color:white" OnClick="btnCancelar_Click"/>
                            </div>
                        </td>            
                    </tr>
                   <%-- <asp:Panel ID="pnlMensajeIndiceCAC" runat="server" Visible="false">
                        <tr>
                            <td colspan="2" style="height: 23px;">
                                <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; margin-top: -10px; margin-left: -12px; width: 95%;">
                                    <div style="display: table; margin: 0 auto;"><asp:Label ID="lbMensaje" runat="server"></asp:Label></div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="display: table; margin: 0 auto;">
                                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlIndiceCAC">
                                        <ProgressTemplate>
                                            <script type="text/javascript">document.write("<div class='UpdateProgressBackground'></div>");</script>
                                            <center>
                                                <div class="UpdateProgressContent">
                                                    <div style="float:left;"><img src="images/ring_loading.gif" width="300px" style="height: 30px; width:30px" ImageAlign="left"  /></div>
                                                    <div style="float:right; padding: 0px 0 0 10px">
                                                        <font style="font-size: 25px; color: #B7B7B7; font-weight: bold;"> Procesando... </font>
                                                    </div>                                    
                                                </div>
                                            </center>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="display: table; margin: 0 auto; margin-top: 20px;">
                                    <asp:Button ID="btnIndiceSi" runat="server" CssClass="formBtnNar" Text="Si" style="width: 80px; height: 40px;" OnClick="btnIndiceSi_Click"/>
                                    <asp:Button ID="btnIndiceNo" runat="server" Text="No" style="width: 80px; height: 40px; margin-left:35px; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; border: 1px solid #ccc; background-color:white" OnClick="btnCancelar_Click"/>
                                </div>
                            </td>
                        </tr>
                    </asp:Panel>--%>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>   
        <asp:HiddenField ID="hfIndiceCAC" runat="server" />
        <ajax:ModalPopupExtender ID="mpeIndice" runat="server" 
            TargetControlID="hfIndiceCAC"
            PopupControlID="pnlIndiceCAC" 
            CancelControlID="lblCloseIndice"
            BackgroundCssClass="ModalBackground" /> 
        </section>
    </div>

    <div>
        <section>
        <asp:UpdatePanel ID="pnlIndiceCACM" runat="server" Width="410px" HorizontalAlign="Center" class="modal" style="background-color:white">
            <ContentTemplate>
                <table style="width: 100%">               
                    <asp:Label ID="lblCloseIndiceM" Text="X" runat="server" CssClass="closebtn1" style="right: -11px !important;"></asp:Label>
                    <tr>
                        <td colspan="2"><modalTitle><b>Ingrese el índice CAC</b></modalTitle></td>
                    </tr> 
                    <tr>
                        <td style="width:25%; font-size:18px; text-align:right;">
                            <b>Mes:</b>
                        </td>
                        <td>
                            <asp:Label ID="lbFechaIndiceM" runat="server" style="font-size: 18px;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Índice: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtIndiceCACM" CssClass="textBoxForm decimal" Width="400px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Confirme el índice: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmIndiceCACM" CssClass="textBoxForm decimal" Width="400px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="spacer">
                        <td colspan="2" > <hr /> </td>
                    </tr>
                    <asp:Panel ID="pnlBotonesM" runat="server" Visible="true">
                        <tr>            
                            <td colspan="2">
                                <div align="center" style="display: table; margin: 0 auto;">
                                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlIndiceCACM">
                                        <ProgressTemplate>
                                            <script type="text/javascript">document.write("<div class='UpdateProgressBackground'></div>");</script>
                                            <center>
                                                <div class="UpdateProgressContent">
                                                    <div style="float:left;"><img src="images/ring_loading.gif" width="300px" style="height: 30px; width:30px" ImageAlign="left"  /></div>
                                                    <div style="float:right; padding: 0px 0 0 10px">
                                                        <font style="font-size: 25px; color: #B7B7B7; font-weight: bold;"> Procesando... </font>
                                                    </div>                                    
                                                </div>
                                            </center>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="display: table; margin: 0 auto;">
                                    <asp:Button ID="btnFinalizarIndiceM" runat="server" CssClass="formBtnNar" Text="Aceptar" OnClick="btnFinalizarIndiceM_Click"/>
                                    <asp:Button ID="btnCancelarM" runat="server" Text="Cancelar" style="margin-left:15px; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; border: 1px solid #ccc; background-color:white" OnClick="btnCancelarM_Click"/>
                                </div>
                            </td>            
                        </tr>
                    </asp:Panel>
                    <asp:Panel ID="pnlMensajeIndiceCACM" runat="server" Visible="false">
                        <tr>
                            <td colspan="2" style="height: 23px;">
                                <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; margin-top: -10px; margin-left: -12px; width: 95%;">
                                    <div style="display: table; margin: 0 auto;"><asp:Label ID="lbMensajeM" runat="server"></asp:Label></div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="display: table; margin: 0 auto;">
                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlIndiceCACM">
                                        <ProgressTemplate>
                                            <script type="text/javascript">document.write("<div class='UpdateProgressBackground'></div>");</script>
                                            <center>
                                                <div class="UpdateProgressContent">
                                                    <div style="float:left;"><img src="images/ring_loading.gif" width="300px" style="height: 30px; width:30px" ImageAlign="left"  /></div>
                                                    <div style="float:right; padding: 0px 0 0 10px">
                                                        <font style="font-size: 25px; color: #B7B7B7; font-weight: bold;"> Procesando... </font>
                                                    </div>                                    
                                                </div>
                                            </center>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="display: table; margin: 0 auto; margin-top: 20px;">
                                    <asp:Button ID="btnIndiceSiM" runat="server" CssClass="formBtnNar" Text="Si" style="width: 80px; height: 40px;" OnClick="btnIndiceSiM_Click"/>
                                    <asp:Button ID="btnIndiceNoM" runat="server" Text="No" style="width: 80px; height: 40px; margin-left:35px; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; border: 1px solid #ccc; background-color:white" OnClick="btnCancelarM_Click"/>
                                </div>
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>   
        <asp:HiddenField ID="hfIndiceCACM" runat="server" />
        <ajax:ModalPopupExtender ID="mpeIndiceM" runat="server" 
            TargetControlID="hfIndiceCACM"
            PopupControlID="pnlIndiceCACM" 
            CancelControlID="lblCloseIndiceM"
            BackgroundCssClass="ModalBackground" /> 
        </section>
    </div> 

</div>
    
    <div>
        <section>
            <asp:UpdatePanel ID="pnlIndiceUVA" runat="server" Width="410px" HorizontalAlign="Center" class="modal" style="background-color:white">
                <ContentTemplate>
                    <table style="width: 100%">  
                        <asp:Label ID="lblCloseIndiceUVA" Text="X" runat="server" CssClass="closebtn1" style="right: -11px !important;"></asp:Label> 
                        <tr>
                            <td colspan="2"><modalTitle><b>Ingrese el índice UVA</b></modalTitle></td>
                        </tr> 
                        <tr>
                            <td style="width:25%; font-size:18px; text-align:right;">
                                <b>Mes:</b>
                            </td>
                            <td>
                                <asp:Label ID="lbFechaIndiceUVA" runat="server" style="font-size: 18px;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                                Índice: 
                            </td>
                            <td>
                                <asp:TextBox ID="txtIndiceUVA" CssClass="textBoxForm decimal" Width="400px" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                                Confirme el índice: 
                            </td>
                            <td>
                                <asp:TextBox ID="txtConfirmIndiceUVA" CssClass="textBoxForm decimal" Width="400px" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="spacer">
                            <td colspan="2" > <hr /> </td>
                        </tr>
                        <asp:Panel ID="pnlMensajeIndiceUVA" runat="server" Visible="false">
                            <tr>
                                <td colspan="2" style="height: 23px;">
                                    <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; margin-top: -10px; margin-left: -12px; width: 95%;">
                                        <div><asp:Label ID="lbMensajeUVA" runat="server"></asp:Label></div>
                                    </div>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>            
                        <td>
                            <div align="center">
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlIndiceUVA">
                                    <ProgressTemplate>
                                        <script type="text/javascript">document.write("<div class='UpdateProgressBackground'></div>");</script>
                                        <center>
                                            <div class="UpdateProgressContent">
                                                <div style="float:left;"><img src="images/ring_loading.gif" width="300px" style="height: 30px; width:30px" ImageAlign="left"  /></div>
                                                <div style="float:right; padding: 8px 0 0 10px">
                                                    <font style="font-size: 25px; color: #B7B7B7; font-weight: bold;"> Procesando... </font>
                                                </div>                                    
                                            </div>
                                        </center>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </td>
                        <td>
                            <div style="float:right">
                                <asp:Button ID="btnFinalizarIndiceUVA" runat="server" CssClass="formBtnNar" Text="Aceptar" OnClick="btnFinalizarIndiceUVA_Click"/>
                                <asp:Button ID="btnCancelarUVA" runat="server" Text="Cancelar" style="margin-left:15px; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; border: 1px solid #ccc; background-color:white" OnClick="btnCancelarUVA_Click"/>
                            </div>
                        </td>            
                    </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </section>
        <asp:HiddenField ID="hfUva" runat="server" />
        <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
            TargetControlID="hfUva"
            PopupControlID="pnlIndiceUVA" 
            BackgroundCssClass="ModalBackground" /> 
            <%--<section>
        <asp:UpdatePanel ID="pnlIndiceUVA" runat="server" Width="410px" HorizontalAlign="Center" class="modal" style="background-color:white">
            <ContentTemplate>
                <table style="width: 100%">               
                    <asp:Label ID="Label1" Text="X" runat="server" CssClass="lblCloseIndiceUVA" style="right: -11px !important;"></asp:Label>
                    <tr>
                        <td colspan="2"><modalTitle><b>Ingrese el índice UVA</b></modalTitle></td>
                    </tr> 
                     <tr>
                    <td style="width:25%; font-size:18px; text-align:right;">
                        <b>Mes:</b>
                    </td>
                    <td>
                        <asp:Label ID="lbFechaIndiceUVA" runat="server" style="font-size: 18px;"></asp:Label>
                    </td>
                </tr>
                    <tr>
                        <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Índice: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtIndiceUVA" CssClass="textBoxForm decimal" Width="400px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Confirme el índice: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmIndiceUVA" CssClass="textBoxForm decimal" Width="400px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="spacer">
                        <td colspan="2" > <hr /> </td>
                    </tr>
                    <asp:Panel ID="pnlMensajeIndiceUVA" runat="server" Visible="false">
                        <tr>
                            <td colspan="2" style="height: 23px;">
                                <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; margin-top: -10px; margin-left: -12px; width: 95%;">
                                    <div><asp:Label ID="lbMensajeUVA" runat="server"></asp:Label></div>
                                </div>
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>            
                        <td>
                            <div align="center">
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlIndiceUVA">
                                    <ProgressTemplate>
                                        <script type="text/javascript">document.write("<div class='UpdateProgressBackground'></div>");</script>
                                        <center>
                                            <div class="UpdateProgressContent">
                                                <div style="float:left;"><img src="images/ring_loading.gif" width="300px" style="height: 30px; width:30px" ImageAlign="left"  /></div>
                                                <div style="float:right; padding: 8px 0 0 10px">
                                                    <font style="font-size: 25px; color: #B7B7B7; font-weight: bold;"> Procesando... </font>
                                                </div>                                    
                                            </div>
                                        </center>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </td>
                        <td>
                            <div style="float:right">
                                <asp:Button ID="btnFinalizarIndiceUVA" runat="server" CssClass="formBtnNar" Text="Aceptar" OnClick="btnFinalizarIndiceUVA_Click"/>
                                <asp:Button ID="btnCancelarUVA" runat="server" Text="Cancelar" style="margin-left:15px; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; border: 1px solid #ccc; background-color:white" OnClick="btnCancelarUVA_Click"/>
                            </div>
                        </td>            
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>   
        <asp:HiddenField ID="hfIndiceUVA" runat="server" />
        <ajax:ModalPopupExtender ID="mpeIndiceUVA" runat="server" 
            TargetControlID="btnIngresarUVA"
            PopupControlID="pnlIndiceUVA" 
            CancelControlID="lblCloseIndiceUVA"
            BackgroundCssClass="ModalBackground"
            DropShadow="true" /> 
    </section>--%>
    </div>

    <div>
        <section>
        <asp:UpdatePanel ID="pnlIndiceUVAM" runat="server" Width="410px" HorizontalAlign="Center" class="modal" style="background-color:white">
            <ContentTemplate>
                <table style="width: 100%">               
                    <asp:Label ID="lblCloseIndiceUVAM" Text="X" runat="server" CssClass="closebtn1" style="right: -11px !important;"></asp:Label>
                    <tr>
                        <td colspan="2"><modalTitle><b>Ingrese el índice UVA</b></modalTitle></td>
                    </tr> 
                    <tr>
                        <td style="width:25%; font-size:18px; text-align:right;">
                            <b>Mes:</b>
                        </td>
                        <td>
                            <asp:Label ID="lbFechaIndiceUVAM" runat="server" style="font-size: 18px;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Índice: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtIndiceUVAM" CssClass="textBoxForm decimal" Width="400px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Confirme el índice: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmIndiceUVAM" CssClass="textBoxForm decimal" Width="400px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="spacer">
                        <td colspan="2" > <hr /> </td>
                    </tr>
                    <asp:Panel ID="pnlBotonesUVAM" runat="server" Visible="true">
                        <tr>            
                            <td colspan="2">
                                <div align="center" style="display: table; margin: 0 auto;">
                                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlIndiceUVAM">
                                        <ProgressTemplate>
                                            <script type="text/javascript">document.write("<div class='UpdateProgressBackground'></div>");</script>
                                            <center>
                                                <div class="UpdateProgressContent">
                                                    <div style="float:left;"><img src="images/ring_loading.gif" width="300px" style="height: 30px; width:30px" ImageAlign="left"  /></div>
                                                    <div style="float:right; padding: 0px 0 0 10px">
                                                        <font style="font-size: 25px; color: #B7B7B7; font-weight: bold;"> Procesando... </font>
                                                    </div>                                    
                                                </div>
                                            </center>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="display: table; margin: 0 auto;">
                                    <asp:Button ID="btnFinalizarIndiceUVAM" runat="server" CssClass="formBtnNar" Text="Aceptar" OnClick="btnFinalizarIndiceUVAM_Click"/>
                                    <asp:Button ID="btnCancelarUVAM" runat="server" Text="Cancelar" style="margin-left:15px; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; border: 1px solid #ccc; background-color:white" OnClick="btnCancelarUVAM_Click"/>
                                </div>
                            </td>            
                        </tr>
                    </asp:Panel>
                    <asp:Panel ID="pnlMensajeIndiceUVAM" runat="server" Visible="false">
                        <tr>
                            <td colspan="2" style="height: 23px;">
                                <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; margin-top: -10px; margin-left: -12px; width: 95%;">
                                    <div style="display: table; margin: 0 auto;"><asp:Label ID="lbMensajeUVAM" runat="server"></asp:Label></div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="display: table; margin: 0 auto;">
                                    <asp:UpdateProgress ID="UpdateProgress7" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlIndiceUVAM">
                                        <ProgressTemplate>
                                            <script type="text/javascript">document.write("<div class='UpdateProgressBackground'></div>");</script>
                                            <center>
                                                <div class="UpdateProgressContent">
                                                    <div style="float:left;"><img src="images/ring_loading.gif" width="300px" style="height: 30px; width:30px" ImageAlign="left"  /></div>
                                                    <div style="float:right; padding: 0px 0 0 10px">
                                                        <font style="font-size: 25px; color: #B7B7B7; font-weight: bold;"> Procesando... </font>
                                                    </div>                                    
                                                </div>
                                            </center>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="display: table; margin: 0 auto; margin-top: 20px;">
                                    <asp:Button ID="btnIndiceSiUVAM" runat="server" CssClass="formBtnNar" Text="Si" style="width: 80px; height: 40px;" OnClick="btnIndiceSiUVAM_Click"/>
                                    <asp:Button ID="btnIndiceNoUVAM" runat="server" Text="No" style="width: 80px; height: 40px; margin-left:35px; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; border: 1px solid #ccc; background-color:white" OnClick="btnCancelarUVAM_Click"/>
                                </div>
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>   
        <asp:HiddenField ID="hfIndiceUVAM" runat="server" />
        <ajax:ModalPopupExtender ID="mpeIndiceUVAM" runat="server" 
            TargetControlID="hfIndiceUVAM"
            PopupControlID="pnlIndiceUVAM" 
            CancelControlID="lblCloseIndiceUVAM"
            BackgroundCssClass="ModalBackground" /> 
        </section>
    </div> 
        
</asp:Content>

