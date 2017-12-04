<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Proyecto.aspx.cs" Inherits="crm.Proyecto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

    <script type="text/javascript">
        // simple redirect to your detail page, passing the selected ID 
        function redir(id) {
            window.location.href = "Unidad.aspx?idProyecto=" + id;
        }
    </script>

     <style>
        .valerror[style*="inline"]
        {
            display:block !Important;
            color: #a94442 !Important;
            width: 286px;
            padding-top: 5px;
            padding-bottom: 5px;
            padding-left: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>

    <section>
        <div class="formHolder" id="searchBoxTop">
            <div class="headOptions headLine">
                <h2>Obras</h2>
                <div style="float:right;">
                    <div style="float:left">
                        <label class="rigthLabel" style="width: 37%;">
                            <asp:Button ID="btnResumen" runat="server" Text="Resumen de Obras" CssClass="formBtnNar" OnClick="btnResumen_Click" style="margin-right: 10px; height: 35px;" />
                        </label>
                    </div>
                    <div style="float:right">
                        <asp:Panel ID="pnlAgregarObra" runat="server">
                            <b><asp:LinkButton ID="btnNuevoProyecto" runat="server" CssClass="formBtnGrey" Text="Agregar nueva obra"></asp:LinkButton></b>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </section>
        
    <asp:ListView ID="lvProyectos" runat="server" OnItemCommand="lvProyectos_ItemCommand" OnItemDataBound="lvProyectos_ItemDataBound">
        <LayoutTemplate>
            <section>            
                <table style="width:100%;"">                            
                    <thead id="tableHead">
                        <tr>     
                            <td style="width: 18%">DESCRIPCIÓN</td> 
                            <td style="width: 10%; text-align: center">CANT. UNIDADES DISPONIBLES</td>
                            <td style="width: 10%; text-align: center">VALOR M2 (DÓLAR)</td>
                            <td style="width: 12%; text-align: center">SUP. TOTAL</td> 
                            <td style="width: 12%; text-align: center">SUP. TOTAL DISPONIBLES</td>
                            <td style="width: 12%; text-align: center">SUP. TOTAL RESERVADOS</td> 
                            <td style="width: 12%; text-align: center">SUP. TOTAL VENDIDOS</td> 
                            <td style="width: 8%;" id="itemPlaceholder1"></td>
                        </tr>
                    </thead>
                </table>
                <div class="tableBody">
                    <table style="width:100%">
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>
                    </table>
                </div> 
                <div class="tableFoot">
                    <table style="width:100%">
                        <tfoot class="footerTable">
                        <tr>
                            <td style="width: 18%;"><b>TOTALES</b></td>
                            <td style="width: 10%; text-align: center"><b><asp:Label ID="lbTotalCantUnidades" runat="server"></asp:Label></b></td>
                            <td style="width: 10%; text-align: center"><b><asp:Label ID="lbTotalValorM2" runat="server"></asp:Label></b></td>
                            <td style="width: 12%; text-align: center"><b><asp:Label ID="lbTotalSupTotal" runat="server"></asp:Label></b></td>
                            <td style="width: 12%; text-align: center"><b><asp:Label ID="lbTotalSupTotalDisponibles" runat="server"></asp:Label></b></td>
                            <td style="width: 12%; text-align: center"><b><asp:Label ID="lbTotalSupTotalReservados" runat="server"></asp:Label></b></td>
                            <td style="width: 12%; text-align: center"><b><asp:Label ID="lbTotalSupTotalVendidos" runat="server"></asp:Label></b></td>
                            <td style="width: 8%;"></td>
                        </tr>
                    </tfoot>       
                    </table>
                </div>  
            </section>   
        </LayoutTemplate>                
        
        <ItemTemplate>   
            <tr onclick="redir('<%# Eval("id") %>');" style="cursor: pointer">
                <td style="text-align: left; width: 18%"><asp:Label runat="Server" Text='<%#Eval("descripcion") %>' /></td>
                <td style="text-align: center; width: 10%; padding-left: 21px;"><asp:Label ID="lbCantUnidades" runat="Server" Text='<%#Eval("CantUnidadesDisponibles") %>' /></td>
                <td style="text-align: center; width: 10%; padding-left: 25px;"><asp:Label ID="lbValorM2" runat="Server" Text='<%#Eval("ValorM2PorObra") %>' /></td>
                <td style="text-align: center; width: 12%; padding-left: 29px;"><asp:Label ID="lbSupTotal" runat="Server" Text='<%#Eval("GetSupTotal") %>' /></td>
                <td style="text-align: center; width: 12%; padding-left: 33px;"><asp:Label ID="lbSupTotalDisponibles" runat="Server" Text='<%#Eval("SupTotalDisponible") %>' /></td>
                <td style="text-align: center; width: 12%; padding-left: 38px;"><asp:Label ID="lbSupTotalReservados" runat="Server" Text='<%#Eval("SupTotalReservado") %>' /></td>
                <td style="text-align: center; width: 12%; padding-left: 42px;"><asp:Label ID="lbSupTotalVendidos" runat="Server" Text='<%#Eval("SupTotalVendido") %>' /></td>
                <td style="width: 8%">
                    <asp:LinkButton ID="btnEditar" runat="server" CssClass="editBtn" CommandName="Editar" Text="Editar" ToolTip="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'/>
                    <a class="detailBtn" href="Unidad.aspx?idProyecto=<%# Eval("id") %>"></a>                    
                </td>
            </tr>                
        </ItemTemplate>        
                                       
        <EmptyDataTemplate>
            <table style="width:100%" runat="server">
                <tr>
                    <td align="center"><b>No se encontraron obras registradas.<b/></td>
                </tr>
            </table>
        </EmptyDataTemplate>
    </asp:ListView>

    <section>
        <asp:UpdatePanel ID="pnlFinalizarProyecto" runat="server" style="width:410px" HorizontalAlign="Center" class="modal">
            <ContentTemplate>
                <table width="100%">               
                    <tr>
                        <td colspan="2"><modalTitle><b>Nueva Obra</b></modalTitle></td>
                    </tr> 
                    <tr id="filaQuienCerroTicket" runat="server">
                        <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            Descripción: 
                        </td>
                        <td>
                            <div>
                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="textBoxForm"></asp:TextBox>
                                <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                                ControlToValidate="txtDescripcion"
                                ErrorMessage="Ingrese una descripción"
                                Validationgroup="CCInfoGroup" Display="Dynamic" SetFocusOnError="True" CssClass="alert valerror borderValidator"/>
                            </div>                               
                        </td>
                    </tr>
                    <tr>   
                        <td colspan="2">
                            <div>
                                <div style="float:left">
                                </div>
                                <div align="right" style="float:right">
                                    <asp:Button ID="btnCerrar" CssClass="btnClose" runat="server" Text="Cerrar" CommandArgument="Nueva" OnClick="btnCerrar_Click"  />
                                    <asp:Button ID="btnFinalizar" runat="server"  Text="Finalizar" CssClass="formBtnNar" style="margin-left:15px" CausesValidation="true" Validationgroup="CCInfoGroup" OnClick="btnFinalizar_Click"/>
                                </div>
                            </div>
                        </td>           
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>   
        <ajax:ModalPopupExtender ID="ModalProyecto" runat="server" 
            TargetControlID="btnNuevoProyecto"
            PopupControlID="pnlFinalizarProyecto" 
            BackgroundCssClass="ModalBackground"
            DropShadow="true" /> 
    </section>
    
    <section>
        <asp:UpdatePanel ID="pnlEditar" runat="server" style="width:410px" HorizontalAlign="Center" class="modal">
                <ContentTemplate>
                    <table width="100%">               
                        <tr>
                            <td colspan="2"><modalTitle><b>Editar Obra</b></modalTitle></td>
                        </tr> 
                        <tr id="Tr1" runat="server">
                            <td style="width:25%; color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                                Descripción: 
                            </td>
                            <td>
                                <asp:TextBox ID="txtEditDescripcion" runat="server" CssClass="textBoxForm"></asp:TextBox>  
                                <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtEditDescripcion"
                                    ErrorMessage="Ingrese una descripción"
                                    Validationgroup="EditDescripcionGroup" Display="Dynamic" SetFocusOnError="True" CssClass="alert valerror borderValidator"/>
                            </td>
                        </tr>
                        <tr>   
                            <td colspan="2">
                                <div>
                                    <div style="float:left">
                                        <asp:Button ID="btnDelete" runat="server" Text="Eliminar" CssClass="btnDelete" style="margin-left:16px" OnClick="btnDelete_Click"/>
                                    </div>
                                    <div align="right" style="float:right">
                                        <asp:Button ID="Button1" CssClass="btnClose" runat="server" Text="Cerrar" CommandArgument="Editar" OnClick="btnCerrar_Click"  />
                                        <asp:Button ID="btnGuardar" runat="server"  Text="Guardar" CssClass="formBtnNar" style="margin-left:15px" CausesValidation="true" Validationgroup="EditDescripcionGroup" OnClick="btnGuardar_Click"/>
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

    <section>
        <asp:UpdatePanel ID="pnlDelete" runat="server" style="width:410px" HorizontalAlign="Center" class="modal modalDelete">
            <ContentTemplate>
                <table width="100%" style="margin-bottom:0px">               
                    <tr>
                        <td colspan="2"><modalTitle><b>Eliminar Obra</b></modalTitle></td>
                    </tr> 
                    <tr>
                        <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            <div align="center"><h4>¿Desea eliminar la obra <asp:Label ID="lbDeleteProyecto" runat="server"></asp:Label>?</h4></div>
                        </td>
                    </tr> 
                    <tr>   
                        <td colspan="2">
                            <div align="center">
                                <asp:Button ID="btnYes" runat="server" CssClass="btnYes" Text="Si" Onclick="btnEliminar_Click" />
                                <asp:Button ID="btnNo" runat="server" CssClass="btnNo" Text="No" CommandArgument="Eliminar" OnClick="btnCerrar_Click"/>
                            </div>
                        </td>           
                    </tr>
                </table>
                <asp:Panel ID="pnlMensajeError" runat="server" Visible="false">
                    <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px; width: 751px;">
                        <div>No se puede eliminar esta obra porque tiene asociados un cliente o más a algunas de sus unidades.</div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>   
        <asp:HiddenField ID="HiddenField2" runat="server" />
        <ajax:ModalPopupExtender ID="modalDelete" runat="server" 
            TargetControlID="HiddenField2"
            PopupControlID="pnlDelete" 
            BackgroundCssClass="ModalBackground"
            DropShadow="true" /> 
    </section>
    
    <asp:HiddenField ID="hfId" runat="server" />
</asp:Content>

