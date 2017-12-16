<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Cliente" Codebehind="Cliente.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <section><h2>Empresa: <asp:Label ID="lbEmpresa" runat="server" Font-Bold="True"></asp:Label><a href="Agenda.aspx" class="formBtnGrey">< Volver a la Agenda</a></h2></section>  
                
    <asp:ListView ID="lvClientes" runat="server"
        oniteminserting="lvClientes_ItemInserting" 
        InsertItemPosition="FirstItem" 
        onitemcanceling="lvClientes_ItemCanceling" 
        onitemdeleting="lvClientes_ItemDeleting" onitemediting="lvClientes_ItemEditing" 
        onitemupdating="lvClientes_ItemUpdating">
        <LayoutTemplate>
            <section>            
                <table >                            
                    <thead id="tableHead">
                        <tr>
                            <td >ID</td>                              
                            <td >NOMBRE</td>
                            <td >INTERNO</td>
                            <td >MAIL</td>
                            <td ></td>
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
                <td><asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' Enabled="False" /></td>
                <td align="left"><asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("nombre") %>' /></td>
                <td><asp:Label ID="lbInterno" runat="Server" Text='<%#Eval("interno") %>' /></td>
                <td><asp:Label ID="lbMail" runat="Server" Text='<%#Eval("mail") %>' /></td> 
                <td><asp:LinkButton ID="EditButton" runat="Server" Text="Editar" CommandName="Edit"  class="editBtn" CausesValidation="false" ValidationGroup="rfvMail"/></td>
            </tr>
        </ItemTemplate>
                
        <EditItemTemplate>
            <tr class="editRow">
                <td>
                    <asp:TextBox ID="txtEditId" runat="server" Text='<%#Bind("id") %>' Enabled="False" />
                </td>
                <td>
                    <asp:TextBox ID="txtEditNombre" runat="server" Text='<%#Bind("nombre") %>' />
                </td>
                <td>
                    <asp:TextBox ID="txtEditInterno" runat="server" Text='<%#Bind("interno") %>' />
                </td>
                <td>
                    <asp:TextBox ID="txtEditMail" runat="server" Text='<%#Bind("mail") %>' />
                      
                    <asp:RequiredFieldValidator ID="rfvEditMail" runat="server" Display="None" ErrorMessage="Por favor, ingrese el mail del cliente." ControlToValidate="txtEditMail" ValidationGroup="rfvEditMail"></asp:RequiredFieldValidator> 
                    <ajax:ValidatorCalloutExtender 
                    runat="Server"
                    ID="rfv2"
                    TargetControlID="rfvEditMail" 
                    Width="100px"
                    HighlightCssClass="highlight" 
                    PopupPosition="Right" />
                      
                    <ajax:FilteredTextBoxExtender ID="ValidarMail" runat="server"
                        TargetControlID="txtEditMail" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" InvalidChars="´`^+=\/*()'?¿[]{}¨Ç:;,áéíóúºª!|·" FilterMode="InvalidChars">
                    </ajax:FilteredTextBoxExtender>
                </td>
                <td>
                    <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" class="deleteBtn" Text="Delete" CausesValidation="false" ValidationGroup="rfvMail"/>&nbsp;
                    <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" class="cancelBtn" Text="Cancelar" CausesValidation="true" ValidationGroup="rfvEditMail"/>&nbsp;
                    <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" class="saveBtn" Text="Actualizar" CausesValidation="true" ValidationGroup="rfvEditMail"/>
                </td>
            </tr>
        </EditItemTemplate>
                               
        <InsertItemTemplate>
            <tr class="editRow">
                <td></td>
                <td>
                    <asp:Label runat="server" ID="lbNombre" AssociatedControlID="txtNombre"/>
                    <asp:TextBox ID="txtNombre" runat="server" Text='<%#Bind("nombre") %>' /><br />
                <td>
                    <asp:Label runat="server" ID="lbInterno" AssociatedControlID="txtInterno"/>
                    <asp:TextBox ID="txtInterno" runat="server" Text='<%#Bind("interno") %>' />
                </td>
                <td>
                    <asp:Label runat="server" ID="lbMail" AssociatedControlID="txtMail"/>
                    <asp:TextBox ID="txtMail" runat="server" Text='<%#Bind("mail") %>' />
                            
                    <asp:RequiredFieldValidator ID="rfvMail" runat="server" Display="None" ErrorMessage="Por favor, ingrese el mail del cliente." ControlToValidate="txtMail" ValidationGroup="rfvMail"></asp:RequiredFieldValidator> 
                    <ajax:ValidatorCalloutExtender 
                        runat="Server"
                        ID="rfv1"
                        TargetControlID="rfvMail" 
                        Width="200px"
                        HighlightCssClass="highlight" 
                        PopupPosition="Right"/>
                                                        
                    <ajax:FilteredTextBoxExtender ID="ValidarMail" runat="server"
                        TargetControlID="txtMail" FilterType="UppercaseLetters, LowercaseLetters, Numbers, Custom" 
                        InvalidChars="´`^+=\/*()'?¿[]{}¨Ç:;,áéíóúºª!|·" FilterMode="InvalidChars"/>
                </td>
                <td><asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert" Text="Insertar" class="addBtn"  CausesValidation="true" ValidationGroup="rfvMail"/></td>
                </td>
            </tr>
        </InsertItemTemplate>
                              
        <EmptyDataTemplate>
            <table id="Table1" width="100%" runat="server">
                <tr>
                    <td align="center"><b>No data found.<b/></td>
                </tr>
            </table>
        </EmptyDataTemplate>
    </asp:ListView>    
</asp:Content>