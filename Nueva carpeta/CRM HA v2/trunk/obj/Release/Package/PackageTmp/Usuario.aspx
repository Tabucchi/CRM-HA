<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Usuario" Codebehind="Usuario.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</ajax:ToolkitScriptManager> 
<asp:ObjectDataSource ID="odsCategoria" runat="server" SelectMethod="GetListaCategoria" TypeName="cCampoGenerico"></asp:ObjectDataSource>

<asp:Panel ID="pnlMensaje" runat="server" Visible="false">
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" 
                    Text="La contraseña debe tener un mínimo de 6 caracteres alfanuméricos." 
                    Font-Bold="True" ForeColor="#FF3300"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
                
<asp:ListView ID="lvUsuarios" runat="server" 
    onitemediting="lvUsuarios_ItemEditing"
    onitemupdating="lvUsuarios_ItemUpdating"
    onitemcanceling="lvUsuarios_ItemCanceling"
    onitemdeleting="lvUsuarios_ItemDeleting"
    oniteminserting="lvUsuarios_ItemInserting" 
    InsertItemPosition="FirstItem" 
    DataKeyNames="id" 
    OnPreRender="ListPager_PreRender">
    <LayoutTemplate>
        <section>
            <table>                            
                <thead id="tableHead">
                    <tr>                             
                        <td style="width:25%">NOMBRE</td>
                        <td style="width:6%">USUARIO</td>
                        <td style="width:6%">CLAVE</td>
                        <td style="width:25%">MAIL</td>
                        <td style="width:14%">CATEGORIA</td>
                        <td style="width:10%"></td>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </tbody>                        
            </table>
        </section>
    </LayoutTemplate>
    <ItemTemplate>                 
        <tr onclick="Visible(<%# Eval("id")%> )">
            <td>
                <asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' Enabled="False" Visible="false"/>
                <asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("nombre") %>' />
            </td>
            <td><asp:Label ID="lbUsuario" runat="Server" Text='<%#Eval("usuario") %>' /></td>
            <td><asp:Label ID="lbClave" runat="Server" Text='<%#Eval("clave") %>' /></td> 
            <td><asp:Label ID="lbMail" runat="Server" Text='<%#Eval("mail") %>' /></td>  
            <td><asp:Label ID="lbCategoria" runat="Server" Text='<%#Eval("GetCategoria") %>' /></td>
            <td><asp:LinkButton ID="EditButton" runat="Server" Text="Editar" CommandName="Edit" class="editBtn" /></td>
        </tr>
    </ItemTemplate>
            
    <EditItemTemplate>
        <tr class="editRow">
        <td>
            <asp:TextBox ID="txtEditId" runat="server" Text='<%#Bind("id") %>' Visible="false" Enabled="False" style="width:30px;" />
            <asp:TextBox ID="txtEditNombre" runat="server" Text='<%#Bind("nombre") %>' style="width: 265px;"/>
        </td>
        <td><asp:TextBox ID="txtEditUsuario" runat="server" Text='<%#Bind("usuario") %>' style="width: 120px;"/></td>
        <td><asp:TextBox ID="txtEditClave" runat="server" Text='<%#Bind("clave") %>' style="width: 120px;"/></td>
        <td><asp:TextBox ID="txtEditMail" runat="server" Text='<%#Bind("mail") %>' style="width: 265px;"/></td>
        <td>
            <asp:DropDownList ID="ddlEditCategoria" runat="server" SelectedValue='<%#Bind("idCategoria") %>' class="dropDownList" style="width: 150px">
                    <asp:ListItem Value="1" Text="Administración" />
                    <asp:ListItem Value="2" Text="Gerencia" />
                    <asp:ListItem Value="3" Text="Usuario" />
                    <asp:ListItem Value="4" Text="Vendedor" />
            </asp:DropDownList> 
        </td>
        <td>
            <a href="#" alt="Eliminar" class="tooltip tooltipColor">
                <asp:LinkButton ID="DeleteButton" runat="server" class="deleteBtn" CommandName="Delete" Text="Eliminar" ToolTip="Eliminar" />
            </a>
            <a href="#" alt="Cancelar" class="tooltip tooltipColor">
                <asp:LinkButton ID="CancelButton" runat="server" class="cancelBtn" CommandName="Cancel" Text="Cancelar" ToolTip="Cancelar" />
            </a>
            <a href="#" alt="Guardar" class="tooltip tooltipColor">
                <asp:LinkButton ID="UpdateButton" runat="server" class="saveBtn" CommandName="Update" Text="Actualizar" ToolTip="Guardar" />
            </a>
        </td>
        </tr>
    </EditItemTemplate>
                           
    <InsertItemTemplate>
        <tr class="editRow">
            <td>
                <asp:Label runat="server" ID="lbNombre" AssociatedControlID="txtNombre"/>
                <asp:TextBox ID="txtNombre" runat="server" Text='<%#Bind("nombre") %>' style="width: 265px;"/>
            <td>
                <asp:Label runat="server" ID="lbUsuario" AssociatedControlID="txtUsuario"/>
                <asp:TextBox ID="txtUsuario" runat="server" Text='<%#Bind("usuario") %>' style="width: 120px;"/>
            </td>
            <td style="height:45px">
                <asp:Label runat="server" ID="lbClave" AssociatedControlID="txtClave"/>
                <asp:TextBox ID="txtClave" runat="server" Text='<%#Bind("clave") %>' style="width: 120px;"/>
            </td>
            <td>
                <asp:Label runat="server" ID="lbMail" AssociatedControlID="txtMail"/>
                <asp:TextBox ID="txtMail" runat="server" Text='<%#Bind("mail") %>' style="width: 265px;"/>
            </td>
            <td>
                <asp:DropDownList ID="ddlEditCategoria" runat="server" class="dropDownList" DataSourceID="odsCategoria" 
                            DataTextField="descripcion" DataValueField="id" 
                            SelectedValue='<%#Bind("id")%>' style="width: 150px"/>
            </td>
            <td><asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert" Text="Insertar" class="addBtn" ValidationGroup="rfvInsertPass"/></td>
            </td>
        </tr>
    </InsertItemTemplate>
                          
    <EmptyDataTemplate>
        <table id="Table1" width="100%" runat="server">
            <tr>
                <td><b>No data found.<b/></td>
            </tr>
        </table>
    </EmptyDataTemplate>
</asp:ListView>         
        
</asp:Content>
