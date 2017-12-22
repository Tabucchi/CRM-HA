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
                            <td >CONTRASEÑA</td>                                    
                            <td >AUTORIZACION</td>
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
                <td><asp:Label ID="lbClave" runat="Server" Text='<%#Eval("claveSistema") %>' /></td>
                <td><asp:Label ID="lbAutorizacion" runat="Server" Text='<%#Eval("GetAutorizacion") %>' /></td>  
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
                    <asp:TextBox ID="txtEditClave" runat="server" Text='<%#Bind("claveSistema") %>' />
                </td>
                <td>
                    <asp:CheckBox ID="chEditAutorizacion" runat="server" Checked='<%#Bind("GetAutorizacionBool") %>' />
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
                <td>
                    <asp:Label runat="server" ID="lbClave" AssociatedControlID="txtClave"/>
                    <asp:TextBox ID="txtClave" runat="server" Text='<%#Bind("interno") %>' />
                </td>
                <td>
                    <asp:Label runat="server" ID="lbAutorizacion" AssociatedControlID="chbAutorizacion"/>
                    <asp:CheckBox ID="chbAutorizacion" runat="server" Checked='<%#Bind("GetAutorizacionBool") %>'/>
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
   
    <asp:Panel ID="filtro" runat="server" DefaultButton="btnIngresarDatos">
        <section>
            <div class="formHolder">
                <h3><strong>DATOS TECNICOS:</strong> Para ver la siguiente información ingrese la contraseña:</h3>
                <label>
                    <input id="login" type="password" tabindex="2" name="login" runat="server"> 
                    <asp:Button ID="btnIngresarDatos" runat="server" Text="Ingresar" class="formBtnNar" onclick="btnIngresarDatos_Click"/>
                </label>
                &nbsp;&nbsp;<label class="rigthLabel"><asp:Label ID="lbErrorLog" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#CC3300" Visible="False"></asp:Label>
                    <asp:Button ID="btnGuardarDatos" runat="server" Text="Guardar" class="formBtnNar" OnClick="btnGuardarDatos_Click"/>
                </label>
            </div>
        </section>
    <cc1:Editor ID="htmlEditor" runat="server" Width="100%" Height="350px" Visible="false" /></td></tr>    
    </asp:Panel>
</asp:Content>