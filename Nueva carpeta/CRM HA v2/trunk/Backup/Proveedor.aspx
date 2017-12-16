<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Proveedor" Codebehind="Proveedor.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>   
    <div class="pal grayArea uiBoxGray noborder">
        <div>
            <asp:Panel ID="filtro" runat="server" DefaultButton="btnBuscar">
                <table>            
                    <tr>
                        <td align="left"><b>Filtro:</b></td>
                    </tr>
                    <tr>
                        <td align="left">PROVEEDOR</td>
                        <td align="left">
                            <asp:DropDownList ID="cbProveedor" Width="200px" runat="server" >                            
                                 <asp:ListItem Text="Todos" Value="0" />
                            </asp:DropDownList>
                        </td>
                        <td><asp:Button ID="btnBuscar" Text="Buscar" class="boton" runat="server" 
                                onclick="btnBuscar_Click" /></td>
                        <td width="700px" align="right">
                            <asp:LinkButton ID="lkbVolverCompra" runat="server" 
                                onclick="lkbVolverCompra_Click">Volver a compras</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    
        <asp:ListView ID="lvProveedores" runat="server" DataKeyNames="id"
            OnPreRender="ListPager_PreRender"
            onitemediting="lvProveedores_ItemEditing"
            onitemupdating="lvProveedores_ItemUpdating"
            onitemcanceling="lvProveedores_ItemCanceling"
            onitemdeleting="lvProveedores_ItemDeleting"
            oniteminserting="lvProveedores_ItemInserting" 
            InsertItemPosition="FirstItem">
            <LayoutTemplate>
                <div class="PrettyGrid">                
                    <div style="overflow:auto; height:375px">             
                    <table border="0" cellpadding="1">                            
                        <thead>
                            <tr>
                                <th style="width:8%; height:20px;" class="column_head">ID</th>                              
                                <th style="width: 15%" class="column_head">PROVEEDOR</th>
                                <th style="width: 10%" class="column_head">DIRECCION</th>
                                <th style="width: 10%" class="column_head">TELEFONO</th>
                                <th style="width: 10%" class="column_head">MAIL</th>
                                <th style="width: 15%" class="column_head"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>                        
                    </table>
                    </div>                   
                </div>
            </LayoutTemplate>
            <ItemTemplate>                   
                <tr style="background-color:#FFFFFF; cursor:pointer;" onclick="Visible(<%# Eval("id")%> )">
                    <td><asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' Enabled="False" /></td>
                    <td><asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("nombre") %>' /></td>
                    <td><asp:Label ID="lbDireccion" runat="Server" Text='<%#Eval("direccion") %>' /></td>
                    <td><asp:Label ID="lbTelefono" runat="Server" Text='<%#Eval("telefono") %>' /></td>   
                    <td><asp:Label ID="lbMail" runat="Server" Text='<%#Eval("mail") %>' /></td>
                    <td><asp:LinkButton ID="EditButton" runat="Server" Text="Editar" CommandName="Edit" /></td>
                </tr>
            </ItemTemplate>
            
            <EditItemTemplate>
                <tr style="background-color: #ADD8E6">
                <td>
                  <asp:TextBox ID="txtEditId" runat="server" Text='<%#Bind("id") %>' Enabled="False" />
                </td>
                <td>
                  <asp:TextBox ID="txtEditNombre" runat="server" Text='<%#Bind("nombre") %>' />
                </td>
                <td>
                  <asp:TextBox ID="txtEditDireccion" runat="server" Text='<%#Bind("direccion") %>' />
                </td>
                <td>
                  <asp:TextBox ID="txtEditTelefono" runat="server" Text='<%#Bind("telefono") %>' />
                </td>
                <td>
                  <asp:TextBox ID="txtEditMail" runat="server" MaxLength="13" Text='<%#Bind("mail") %>' />
                </td>
                <td>
                  <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" Text="Actualizar" />&nbsp;
                  <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" Text="Eliminar" />&nbsp;
                  <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancelar" />
                </td>
              </tr>
            </EditItemTemplate>
                           
            <InsertItemTemplate>
                <tr style="background-color:#D3D3D3">
                    <td></td>
                    <td>
                        <asp:Label runat="server" ID="lbNombre" AssociatedControlID="txtNombre"/>
                        <asp:TextBox ID="txtNombre" runat="server" Text='<%#Bind("nombre") %>' /><br />
                    <td>
                        <asp:Label runat="server" ID="lbDireccion" AssociatedControlID="txtDireccion"/>
                        <asp:TextBox ID="txtDireccion" runat="server" Text='<%#Bind("direccion") %>' />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbTelefono" AssociatedControlID="txtTelefono"/>
                        <asp:TextBox ID="txtTelefono" runat="server" Text='<%#Bind("telefono") %>' />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbCuit" AssociatedControlID="txtMail"/>
                        <asp:TextBox ID="txtMail" runat="server" Text='<%#Bind("mail") %>' />
                    </td>
                    <td><asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert" Text="Insertar" /></td>
                  </td>
                </tr>
            </InsertItemTemplate>
                         
            <AlternatingItemTemplate>
                <tr style="background-color:#EFEFEF; cursor:pointer;" onclick="Visible(<%# Eval("id")%> )">
                    <td><asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' /></td>
                    <td><asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("nombre") %>' /></td>
                    <td><asp:Label ID="lbDireccion" runat="Server" Text='<%#Eval("direccion") %>' /></td>
                    <td><asp:Label ID="lbTelefono" runat="Server" Text='<%#Eval("telefono") %>' /></td>   
                    <td><asp:Label ID="lbMail" runat="Server" Text='<%#Eval("mail") %>' /></td> 
                    <td><asp:LinkButton ID="EditButton" runat="Server" Text="Editar" CommandName="Edit" /></td>
                </tr>             
            </AlternatingItemTemplate>  
                          
            <EmptyDataTemplate>
                <table id="Table1" width="100%" runat="server">
                    <tr>
                        <td align="center"><b>No data found.<b/></td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
        <table width="100%">
            <tr width="700px">
                <td align="left" >
                    <asp:LinkButton ID="lbVerTodos" runat="server" onclick="lbVerTodos_Click">Ver Todos</asp:LinkButton>
                </td>
                <td align="right">
                    Páginas:
                    <asp:DataPager ID="_moviesGridDataPager" PageSize="25" runat="server" PagedControlID="lvProveedores">
                        <Fields><asp:NumericPagerField/></Fields>
                    </asp:DataPager>
                </td>
            </tr>
        </table>
    </div>
</div>
</asp:Content>

