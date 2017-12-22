<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ResponsableEmpresa.aspx.cs" Inherits="crm.ResponsableEmpresa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ObjectDataSource ID="odsEmpresa" runat="server" SelectMethod="GetListaEmpresas" TypeName="cProveedor"></asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsUsuario" runat="server" SelectMethod="GetListaUsuarios" TypeName="cProveedor"></asp:ObjectDataSource>

<section>
    <div class="formHolder" id="searchBoxTop">
        <div class="formHolderLine">
            <h2>Filtro:</h2>
            <div style="float:right;">
                <label >
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/images/ExcelIcon.png" Height="16" Width="16" />
                    &nbsp;
                    <asp:LinkButton ID="lkbExcelReporte" runat="server" Font-Bold="True" 
                        Font-Underline="False" ForeColor="Black" 
                    onclick="lkbExcelReporte_Click" >Reporte</asp:LinkButton> 
                </label>
            </div>
        </div>
        <label class="col3"><span>EMPRESA</span><asp:DropDownList ID="cbEmpresa" runat="server" DataSourceID="odsEmpresa" DataTextField="nombre" DataValueField="id" width="200px"><asp:ListItem Text="Todas" Value="0" /></asp:DropDownList></label>
        <label class="col3"><span>USUARIO</span><asp:DropDownList ID="cbUsuario" runat="server" DataSourceID="odsUsuario" DataTextField="nombre" DataValueField="id" width="200px"><asp:ListItem Text="Todas" Value="0" /></asp:DropDownList></label>

        <label class="colSmall rigthLabel">
            <asp:Button ID="btnBuscar" Text="Buscar" class="formBtnNar" runat="server" onclick="btnBuscar_Click" />                     
        </label>
    </div>
</section>

<div align="center" style="width: 70%; padding-left:174px">
    <asp:ListView ID="lvEmpresas" runat="server" DataKeyNames="id" 
            OnPreRender="ListPager_PreRender" InsertItemPosition="FirstItem" 
            onitemcanceling="lvEmpresas_ItemCanceling" 
            onitemediting="lvEmpresas_ItemEditing" 
            oniteminserting="lvEmpresas_ItemInserting" 
            onitemupdating="lvEmpresas_ItemUpdating" 
            onitemdeleting="lvEmpresas_ItemDeleting" >
        <LayoutTemplate>      
            <section>     
                <table>                            
                    <thead id="tableHead">
                        <tr>                             
                            <td>EMPRESA</td>
                            <td>RESPONSABLE</td>
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
            <tr style="cursor:pointer;" onclick="Visible(<%# Eval("id")%> )" >
                <td align="left">
                    <asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' Visible="False" />
                    <asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("GetNombreEmpresa") %>' />
                </td>
                <td><asp:Label ID="lbDireccion" runat="Server" Text='<%#Eval("GetNombreUsuario") %>' /></td>
                <td><asp:LinkButton ID="EditButton" class="editBtn" runat="Server" Text="Editar" CommandName="Edit" /></td>
            </tr>
        </ItemTemplate>
            
        <EditItemTemplate>
            <tr class="editRow">
            <td>
                <asp:TextBox ID="txtEditId" runat="server" Text='<%#Bind("id") %>' Visible="False" style="width:30px;" />
                <asp:DropDownList ID="cbEditEmpresa" runat="server" DataSourceID="odsEmpresa" DataTextField="nombre" DataValueField="id" SelectedValue='<%#Bind("idEmpresa") %>' width="200px"/>
            </td>
            <td>
                <asp:DropDownList ID="cbEditUsuario" runat="server" DataSourceID="odsUsuario" DataTextField="nombre" DataValueField="id" SelectedValue='<%#Bind("idUsuario") %>' width="200px"/>
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
                   <asp:DropDownList ID="cbIngresarEmpresa" runat="server" DataSourceID="odsEmpresa" DataTextField="nombre" DataValueField="id" width="200px"/>
                <td>
                   <asp:DropDownList ID="cbIngresarUsuario" runat="server" DataSourceID="odsUsuario" DataTextField="nombre" DataValueField="id" width="200px"/>
                </td>
                <td><asp:LinkButton ID="InsertButton" runat="server" class="addBtn" CommandName="Insert" Text="Insertar" /></td>
                </td>
            </tr>
        </InsertItemTemplate> 
          
        <EmptyDataTemplate>
            <table id="Table1" runat="server">
                <tr>
                    <td align="center"><b>No data found.<b/></td>
                </tr>
            </table>
        </EmptyDataTemplate>

    </asp:ListView>
</div>
</asp:Content>
