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
                        <td>NOMBRE</td>
                        <td>USUARIO</td>
                        <td>CLAVE</td>
                        <td>MAIL</td>
                        <td>CATEGORIA</td>
                        <td></td>
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
            <td><asp:Label ID="lbCategoria" runat="Server" Text='<%#Eval("tipoCategoria") %>' /></td>
            <td><asp:LinkButton ID="EditButton" runat="Server" Text="Editar" CommandName="Edit" class="editBtn" /></td>
        </tr>
    </ItemTemplate>
            
    <EditItemTemplate>
        <tr class="editRow">
        <td>
            <asp:TextBox ID="txtEditId" runat="server" Text='<%#Bind("id") %>' Enabled="False" style="width:30px;" />
            <asp:TextBox ID="txtEditNombre" runat="server" Text='<%#Bind("nombre") %>' />
        </td>
        <td><asp:TextBox ID="txtEditUsuario" runat="server" Text='<%#Bind("usuario") %>' /></td>
        <td><asp:TextBox ID="txtEditClave" runat="server" Text='<%#Bind("clave") %>' /></td>
            <ajax:PasswordStrength ID="ps1" runat="server"
                TargetControlID="txtEditClave"
                DisplayPosition="BelowLeft"
                StrengthIndicatorType="Text"
                TextStrengthDescriptions="6 caracteres alfanuméricos;OK"
                TextStrengthDescriptionStyles="VeryPoorStrength;ExcellentStrength"
                RequiresUpperAndLowerCaseCharacters="false" PreferredPasswordLength="6" MinimumNumericCharacters="0" MinimumLowerCaseCharacters="1"/>
        <td><asp:TextBox ID="txtEditMail" runat="server" Text='<%#Bind("mail") %>' /></td>
        <td>
            <asp:DropDownList ID="ddlEditCategoria" runat="server" DataSourceID="odsCategoria" 
                            DataTextField="tipo" DataValueField="id" 
                            SelectedValue='<%#Bind("idCategoria")%>' />
        </td>
        <td>
            <asp:LinkButton ID="DeleteButton" runat="server" class="deleteBtn" CommandName="Delete" Text="Eliminar" />
            <asp:LinkButton ID="CancelButton" runat="server" class="cancelBtn" CommandName="Cancel" Text="Cancelar" />
            <asp:LinkButton ID="UpdateButton" runat="server" class="saveBtn" CommandName="Update" Text="Actualizar" />
        </td>
        </tr>
    </EditItemTemplate>
                           
    <InsertItemTemplate>
        <tr class="editRow">
            <td>
                <asp:Label runat="server" ID="lbNombre" AssociatedControlID="txtNombre"/>
                <asp:TextBox ID="txtNombre" runat="server" Text='<%#Bind("nombre") %>' />
            <td>
                <asp:Label runat="server" ID="lbUsuario" AssociatedControlID="txtUsuario"/>
                <asp:TextBox ID="txtUsuario" runat="server" Text='<%#Bind("usuario") %>' />
            </td>
            <td style="height:45px">
                <asp:Label runat="server" ID="lbClave" AssociatedControlID="txtClave"/>
                <asp:TextBox ID="txtClave" runat="server" Text='<%#Bind("clave") %>'/>
                <br />
                <ajax:PasswordStrength ID="ps1" runat="server"
                    TargetControlID="txtClave"
                    DisplayPosition="BelowLeft"
                    StrengthIndicatorType="Text"
                    TextStrengthDescriptions="6 caracteres alfanuméricos;OK"
                    TextStrengthDescriptionStyles="VeryPoorStrength;ExcellentStrength"
                    RequiresUpperAndLowerCaseCharacters="false" PreferredPasswordLength="6" MinimumNumericCharacters="1" MinimumLowerCaseCharacters="1"/>
            </td>
            <td>
                <asp:Label runat="server" ID="lbMail" AssociatedControlID="txtMail"/>
                <asp:TextBox ID="txtMail" runat="server" Text='<%#Bind("mail") %>' />
            </td>
            <td>
                <asp:DropDownList ID="ddlEditCategoria" runat="server" DataSourceID="odsCategoria" 
                            DataTextField="tipo" DataValueField="id" 
                            SelectedValue='<%#Bind("id")%>' />
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
