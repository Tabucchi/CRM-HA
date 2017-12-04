<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="AgendaClientesPotenciales" Title="Página sin título" Codebehind="AgendaClientesPosibles.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <Ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
        <Services>
            <asp:ServiceReference Path="MyAutocompleteService.asmx" />
        </Services>
    </Ajax:ToolkitScriptManager>
    <section>
        <asp:Panel ID="filtro" runat="server" DefaultButton="btnBuscar" CssClass="headOptions">
            <h2>Filtro:</h2>
            <div style="float:right;">
                EMPRESA:                                      
                <asp:TextBox ID="txtEmpresa" CssClass="inputField" runat="server" Width="300px"></asp:TextBox>
                <div style="float:right; margin-left:10px;">
                    <asp:Button ID="btnBuscar" CssClass="formBtnGrey" runat="server" Text="Buscar" onclick="btnBuscar_Click"></asp:Button>
                    <asp:LinkButton ID="btnAgregar" CssClass="formBtnGrey" runat="server">Agregar Cliente</asp:LinkButton>
                </div>
            </div> 
            <ajax:AutoCompleteExtender runat="server"  ID="autoComplete1" 
                TargetControlID="txtEmpresa" 
                ServiceMethod="GetEmpresasPosibles" 
                ServicePath="MyAutocompleteService.asmx" 
                MinimumPrefixLength="2"                            
                CompletionInterval="200"
                EnableCaching="true" 
                CompletionSetCount="10"
                UseContextKey ="True">                     
            </ajax:AutoCompleteExtender>
        </asp:Panel>
               
        <asp:ListView ID="lvEmpresas" runat="server" DataKeyNames="id" OnPreRender="ListPager_PreRender">
            <LayoutTemplate>                      
                    <table border="0" cellpadding="1">                            
                        <thead id="tableHead">
                            <tr>                             
                                <td>EMPRESA</td>
                                <td>RUBRO</td>
                                <td>CONTACTO</td>
                                <!-- <td>PUESTO CONTACTO</td> -->
                                <td>DIRECCION</td>
                                <td>TELEFONO</td>
                                <td>ESTADO</td>
                                <td>&nbsp;</td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>                        
                    </table>
                </section>
            </LayoutTemplate>

            <ItemTemplate>                   
                <tr id="row<%# Eval("id")%>">
                    <td align="left">
                        <asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' Visible="false" />
                        <asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("GetEmpresa") %>' />
                    </td>
                    <td align="left"><asp:Label ID="lbRubro" runat="Server" Text='<%#Eval("Rubro") %>' /></td>
                    <td align="left"><asp:Label ID="lbContacto" runat="Server" Text='<%#Eval("Contacto") %>' /></td>
                    <!-- <td align="left"><asp:Label ID="lbPuestoContacto" runat="Server" Text='<%#Eval("PuestoContacto") %>' /></td> -->
                    <td><asp:Label ID="lbDireccion" runat="Server" Text='<%#Eval("GetDireccion") %>' /></td>
                    <td><asp:Label ID="lbTelefono" runat="Server" Text='<%#Eval("GetTelefono") %>' /></td>
                    <td><asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>' /></td>
                    <td><a href="ClientePosible.aspx?idCliente=<%# Eval("id") %>"  class="detailBtn" >Detalles</a></td>
                </tr>
            </ItemTemplate>
                    
            <EditItemTemplate>
                <tr style="background-color: #ADD8E6">
                    <td>
                        <asp:TextBox ID="txtEditId" runat="server" Text='<%#Bind("id") %>' Visible="false"/>
                        <asp:TextBox ID="txtEditEmpresa" runat="server" Width="120px" Text='<%#Bind("GetEmpresa") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtEditRubro" runat="server" Width="120px" Text='<%#Bind("Rubro") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtEditContacto" runat="server" Width="100px" Text='<%#Bind("Contacto") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtEditPuestoContacto" runat="server" Width="110px" Text='<%#Bind("PuestoContacto") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtEditDireccion" runat="server" Width="100px" Text='<%#Bind("GetDireccion") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtEditTelefono" runat="server" MaxLength="15" Width="90px" Text='<%#Bind("GetTelefono") %>' />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEditEstado" runat="server" SelectedValue='<%#Bind("idEstado")%>'>
                            <asp:ListItem Value="0">Seleccione un estado...</asp:ListItem>
                            <asp:ListItem Value="1">Interesa</asp:ListItem>
                            <asp:ListItem Value="2">No interesa</asp:ListItem>
                        </asp:DropDownList>
                    </td>                        
                    <td>
                        <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" Text="Actualizar" />&nbsp;
                        <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" Text="Eliminar" />&nbsp;
                        <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancelar" />
                    </td>
                </tr>
            </EditItemTemplate>

            <EmptyDataTemplate>
                <table id="Table1" width="100%" runat="server">
                    <tr>
                        <td align="center"><b>No data found.<b/></td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
            
        <div style="text-align:right; padding:20px 0px 0px 20px;">Paginas:
            <asp:DataPager ID="_moviesGridDataPager" PageSize="30" runat="server" PagedControlID="lvEmpresas">
                <Fields><asp:NumericPagerField/></Fields>
            </asp:DataPager>
        </div>
         
    <asp:Panel ID="pnlNuevoCliente" runat="server" Width="850px" HorizontalAlign="Center" CssClass="ModalPopup">
        <section>
            <table width="100%">

                    <asp:Label ID="lblClose" Text="X" CssClass="closebtn" runat="server"></asp:Label>
                
                <tr>
                    <td align="left" style="width:140px"><b>Empresa:</b></td>
                    <td align="left"><asp:TextBox ID="txtNombEmpresa" runat="server" Width="204px" MaxLength="64"/></td>
          
                    <td align="left"><b>Rubro:</b></td>
                    <td align="left"><asp:TextBox ID="txtRubro" runat="server" Width="204px" MaxLength="60"/></td>
                </tr>
                <tr>
                    <td align="left"><b>Contacto:</b></td>
                    <td align="left"><asp:TextBox ID="txtContacto" runat="server" Width="204px" MaxLength="60"/></td>
          
                    <td align="left"><b>Puesto Contacto:</b></td>
                    <td align="left"><asp:TextBox ID="txtPuestoContacto" runat="server" Width="204px" MaxLength="60"/></td>
                </tr>
                <tr>
                    <td align="left"><b>Dirección:</b></td>
                    <td align="left"><asp:TextBox ID="txtDireccion" runat="server" Width="204px" MaxLength="64"/></td>
           
                    <td align="left"><b>Teléfono:</b></td>
                    <td align="left"><asp:TextBox ID="txtTelefono" runat="server" Width="204px" MaxLength="32"/></td>
                </tr>
                <tr>
                    <td align="left"><b>Mail:</b></td>
                    <td align="left"><asp:TextBox ID="txtMail" runat="server" Width="204px" MaxLength="64"/></td>
          
                    <td align="left"><b>Contacto Vinculado:</b></td>
                    <td align="left"><asp:TextBox ID="txtContactoVinculado" runat="server" Width="204px" MaxLength="60"/></td>
                 </tr>
                <tr>
                    <td align="left"><b>Conseguido por:</b></td>
                    <td align="left"><asp:DropDownList ID="ddlConseguidoPor" runat="server" Width="210px"/></td>
           
                    <td align="left"><b>Acción 1:</b></td>
                    <td align="left"><asp:DropDownList ID="ddlAccion" runat="server" Width="210px"/></td>
                </tr>
                <tr>
                    <td align="left"><b>Estado:</b></td>
                    <td align="left">
                        <asp:DropDownList ID="ddlEstado" runat="server" Width="210px">
                            <asp:ListItem Value="0">Seleccione un estado...</asp:ListItem>
                            <asp:ListItem Value="1">Interesa</asp:ListItem>
                            <asp:ListItem Value="2">No interesa</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="spacer">
                    <td colspan="4" > <hr style="opacity:0.2" /> </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnAceptar" runat="server" class="boton" Text="Aceptar" onclick="btnAceptar_Click" />
                    </td>
                </tr>
            </table>
        </section>
    </asp:Panel>   
    <ajax:ModalPopupExtender ID="ModalPopupExtender" runat="server" 
        TargetControlID="btnAgregar"
        PopupControlID="pnlNuevoCliente" 
        CancelControlID="lblClose"
        BackgroundCssClass="ModalBackground"
        DropShadow="true" />

</asp:Content>

