<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CC.aspx.cs" Inherits="crm.CC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    
    <section>
        <div class="headOptions">
            <h2>Cuentas</h2>

             <div style="float:right;">
                <b><asp:LinkButton ID="btnNuevoProyecto" runat="server"  CssClass="formBtnGrey" Text="Agregar unidad funcional"></asp:LinkButton></b>
            </div>
        </div>

        <asp:Panel runat="server" DefaultButton="btnBuscar">
        <div class="formHolder" id="searchBoxTop">
            <div>
                <label class="col3">
                    <span>Obra</span>
                    <asp:DropDownList ID="ddlObra" runat="server" Height="32px"></asp:DropDownList>
                </label>

                <label class="col3">
                    <span style="margin-right: -2px; margin-left: 12px; padding-top: 2px;">Cliente</span>
                    <asp:DropDownList ID="ddlEmpresa" Width="58%" Height="34px" runat="server">
                        <asp:ListItem Text="Todas" Value="0" />
                    </asp:DropDownList>
                </label>
                <label class="col3">
                    <span>Estado</span>
                    <asp:DropDownList ID="ddlEstado" runat="server" Height="32px"><%--<asp:ListItem Text="Todas" Value="2" />--%></asp:DropDownList>
                </label>
                <label class="col3 rigthLabel">
                    <asp:Button ID="btnBuscar" Text="Buscar" class="formBtnNar" runat="server" onclick="btnBuscar_Click" />
                </label>
            </div>
        </div>
        </asp:Panel>
    </section>
                    
    <asp:ListView ID="lvClientes" runat="server"
        oniteminserting="lvClientes_ItemInserting" 
        onitemcanceling="lvClientes_ItemCanceling" 
        onitemdeleting="lvClientes_ItemDeleting" onitemediting="lvClientes_ItemEditing" 
        onitemupdating="lvClientes_ItemUpdating">
        <LayoutTemplate>
            <section>            
                <table style="width:100%">                            
                    <thead id="tableHead">
                        <tr>     
                            <td style="width:15px">CLIENTE</td>                         
                            <td style="width:25px">PROYECTO</td>   
                            <td style="width:20px">TOTAL</td>
                            <td style="width:20px">SALDO</td>
                            <td style="width:15px">ESTADO</td>
                            <td style="width:5px"></td>
                        </t>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>                        
                </table>
            </section>   
        </LayoutTemplate>
                
        <ItemTemplate>                   
            <tr onclick="Visible(<%# Eval("id")%> )">
                <td align="left"><asp:Label ID="Label3" runat="Server" Text='<%#Eval("GetEmpresa") %>' /></td>
                <td align="left"><asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("GetProyecto") %>' />&nbsp;(Unidad Funcional)</td>
                <td align="left">$&nbsp;<asp:Label ID="Label1" runat="Server" Text='<%#Eval("GetTotal") %>' /></td>
                <td align="left">$&nbsp;<asp:Label ID="Label2" runat="Server" Text='<%#Eval("saldo") %>' /></td>
                <td align="left"><asp:Label ID="Label4" runat="Server" Text='<%#Eval("GetEstado") %>' /></td>
                <td>
                    <a  class="detailBtn" href="DetalleCuota.aspx?idCC=<%# Eval("id") %>"></a>
                </td>
            </tr>
        </ItemTemplate>
                                       
        <EmptyDataTemplate>
            <table id="Table1" width="100%" runat="server">
                <tr>
                    <td align="center"><b>No data found.<b/></td>
                </tr>
            </table>
        </EmptyDataTemplate>
    </asp:ListView>

    <section>
        <asp:UpdatePanel ID="pnlFinalizarProyecto" runat="server" Width="410px" HorizontalAlign="Center" CssClass="ModalPopup" style="background-color:white">
            <ContentTemplate>
            <%--<asp:Panel ID="pnlFinalizarProyecto" runat="server" Width="410px" CssClass="ModalPopup">--%>
                <table width="100%">               
                    <%--<asp:Label ID="lblClose" Text="X" runat="server" CssClass="closebtn"></asp:Label>--%>
                    <tr>
                        <td colspan="2" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:center;"><b>Nueva Cuenta Corriente</b></td>
                    </tr> 
                    <tr id="filaQuienCerroTicket" runat="server">
                        <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Cliente: 
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlComboEmpresa" runat="server" Width="200px" required/>
                        </td>
                    </tr>
                    <tr>
                        <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Obra: 
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlComboProyectos" runat="server" Width="200px">
                            </asp:DropDownList>&nbsp (Unidad funcional)
                        </td>
                    </tr>
                    <tr>
                        <td width="35%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Monto:
                        </td>
                        <td>
                            <asp:TextBox ID="txtMonto" runat="server" Width="185px"></asp:TextBox>&nbsp (pesos)
                        </td>
                    </tr>
                    <tr>
                        <td width="35%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Forma de pago:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlComboFormaPago" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlFormaPago_SelectedIndexChanged">
                            </asp:DropDownList> 
                        </td>
                    </tr>
                    <tr>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;" width="25%">Cantidad de cuotas: </td>
                        <td>
                            <asp:TextBox ID="txtCantCuotas" runat="server" Width="185px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;" width="25%">Anticipo: </td>
                        <td>
                            <asp:TextBox ID="txtAnticipo" runat="server" Width="185px" Enabled="false"></asp:TextBox> &nbsp; $ 
                        </td>
                    </tr>
                    <tr>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;" width="25%">Fecha vencimiento: </td>
                        <td>
                            <asp:TextBox ID="txtFechaVencimiento" runat="server" style="width:350px"></asp:TextBox>
                            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="orange" TargetControlID="txtFechaHasta" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" /> 
                        </td>
                    </tr>
                    <tr>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;" width="35%">CAC: </td>
                        <td>
                            <asp:DropDownList ID="ddlComboCac" runat="server" Width="200px">
                            </asp:DropDownList> &nbsp; $ 
                        </td>
                    </tr>
                    <tr>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;" width="25%">Gastos administrativos: </td>
                        <td>
                            <asp:TextBox ID="txtComision" runat="server" Width="185px"></asp:TextBox>&nbsp; %
                        </td>
                    </tr>
                    <tr>   
                        <td colspan="2">
                            <div align="left">
                                <asp:Label ID="lbMensajeCombo" runat="server" CssClass="tituloMensaje" Text="Complete todos los campos" Visible="false"></asp:Label>
                            </div>
                            <div align="right">
                                <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" OnClick="btnCerrar_Click"  />&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnFinalizarCC" runat="server" Text="Finalizar" OnClick="btnFinalizarCC_Click"/>
                            </div>
                        </td>           
                    </tr>
                </table>
            <%--</asp:Panel>--%>
            </ContentTemplate>
        </asp:UpdatePanel>   
        <ajax:ModalPopupExtender ID="ModalPopupExtender" runat="server" 
            TargetControlID="btnNuevoProyecto"
            PopupControlID="pnlFinalizarProyecto" 
            BackgroundCssClass="ModalBackground"
            DropShadow="true" /> 
    </section>
</asp:Content>
