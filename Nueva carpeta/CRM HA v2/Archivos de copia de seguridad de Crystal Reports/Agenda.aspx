<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Agenda" Codebehind="Agenda.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section>
        <div class="formHolder" id="searchBoxTop">
            <div class="formHolderLine">
                <h2>Filtro:</h2>

                 <label style="padding: 1px 0px 0px 0px; width:29%">
                <span style="margin-right: 12px; margin-left: 12px; padding-top: 2px;">EMPRESA</span>
                <asp:DropDownList ID="cbEmpresa" Width="77%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbEmpresa_SelectedIndexChanged" ><asp:ListItem Text="Todas" Value="0" /></asp:DropDownList>
            </label>
                <div style="float:right;">
                    <label>
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/images/ExcelIcon.png" Height="16" Width="16" />
                        &nbsp;
                        <asp:LinkButton ID="lkbExcelReporte" runat="server" Font-Bold="True" 
                            Font-Underline="False" ForeColor="Black" onclick="lkbExcelReporte_Click">Reporte</asp:LinkButton> 
                    </label>
                </div>
            </div>
        </div>
    </section>

   <%-- <ul id="accordionList" style="display:block;" class="invisible">
        <li id="listHead" style="padding:0px;">
            <span style="width:3%">ID</span> 
            <span style="width:21%">EMPRESA</span> 
            <span style="width:20%">DIRECCION</span> 
            <span style="width:14%">TELEFONO</span> 
            <span style="width:14%">CUIT</span> 
            <span style="width:15%">DOMINIO MAIL</span> 
        </li>
    </ul>--%>
    
    <asp:ListView ID="lvEmpresas" runat="server" 
        onitemediting="lvEmpresas_ItemEditing"
        onitemupdating="lvEmpresas_ItemUpdating"
        onitemcanceling="lvEmpresas_ItemCanceling"
        onitemdeleting="lvEmpresas_ItemDeleting"
        oniteminserting="lvEmpresas_ItemInserting" 
        InsertItemPosition="FirstItem" 
        DataKeyNames="id" 
        OnPreRender="ListPager_PreRender">
        <LayoutTemplate>      
            <section>     
                <table>                            
                    <thead id="tableHead">
                        <tr>                           
                            <td>EMPRESA</td>
                            <td>DIRECCION</td>
                            <td>TELEFONO</td>
                            <td>CUIT</td>
                            <td>MAIL</td>
                            <td style="width:8%"></td>
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
                    <asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("nombre") %>' />
                </td>
                <td><asp:Label ID="lbDireccion" runat="Server" Text='<%#Eval("direccion") %>' /></td>
                <td><asp:Label ID="lbTelefono" runat="Server" Text='<%#Eval("telefono") %>' /></td>   
                <td><asp:Label ID="lbCuit" runat="Server" Text='<%#Eval("cuit") %>' /></td>
                <td><asp:Label ID="lbDominio" runat="Server" Text='<%#Eval("Mail") %>' /></td>
                <td style="width:50px"><a  class="detailBtn"  href="Cliente.aspx?idEmpresa=<%# Eval("id") %>">Ver Info</a><asp:LinkButton ID="EditButton" class="editBtn" runat="Server" Text="Editar" CommandName="Edit" /></td>
            </tr>
        </ItemTemplate>
            
        <EditItemTemplate>
            <tr class="editRow">
            <td>
                <asp:TextBox ID="txtEditId" runat="server" Text='<%#Bind("id") %>' Enabled="False" Visible="false"/>
                <asp:TextBox ID="txtEditNombre" runat="server" Text='<%#Bind("nombre") %>' />
            </td>
            <td>
                <asp:TextBox ID="txtEditDireccion" runat="server" Text='<%#Bind("direccion") %>' />
            </td>
            <td>
                <asp:TextBox ID="txtEditTelefono" runat="server" Text='<%#Bind("telefono") %>' />
            </td>
            <td>
                <asp:TextBox ID="txtEditCuit" runat="server" MaxLength="13" Text='<%#Bind("cuit") %>' />
            </td>
            <td>
                <asp:TextBox ID="txtEditDominio" runat="server" MaxLength="25" Text='<%#Bind("Mail") %>' />
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
                    <asp:Label runat="server" ID="lbCuit" AssociatedControlID="txtCuit"/>
                    <asp:TextBox ID="txtCuit" runat="server" Text='<%#Bind("cuit") %>' />
                </td>
                <td>
                    <asp:Label runat="server" ID="Label1" AssociatedControlID="txtCuit"/>
                    <asp:TextBox ID="txtDominio" runat="server" Text='<%#Bind("Mail") %>' />
                </td>
                <td><asp:LinkButton ID="InsertButton" runat="server" class="addBtn" CommandName="Insert" Text="Insertar" /></td>
                </td>
            </tr>
        </InsertItemTemplate>       
        
        <EmptyDataTemplate>
            <table runat="server">
                <tr>
                    <td align="center"><b>No data found.<b/></td>
                </tr>
            </table>
        </EmptyDataTemplate>
    </asp:ListView>
      
</asp:Content>