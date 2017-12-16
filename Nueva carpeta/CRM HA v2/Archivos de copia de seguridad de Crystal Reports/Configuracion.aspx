<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Configuracion.aspx.cs" Inherits="crm.Configuracion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/orange1.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" /> 
    <div style="width:100%">
    <div style="float:left; width:48%">
        <section>
            <div class="formHolder" id="searchBoxTop1">
                <div class="formHolderLine">
                    <h2>Proyectos</h2>
                </div>
            </div>
        </section>
        
        <asp:ListView ID="lvProyecto" runat="server"
            onitemediting="lvProyecto_ItemEditing"
            onitemupdating="lvProyecto_ItemUpdating"
            onitemcanceling="lvProyecto_ItemCanceling"
            onitemdeleting="lvProyecto_ItemDeleting"
            oniteminserting="lvProyecto_ItemInserting" 
            InsertItemPosition="FirstItem" 
            DataKeyNames="id" 
            OnPreRender="ListPager_PreRender">
            <LayoutTemplate>      
                <section>     
                    <table>                            
                        <thead id="tableHead">
                            <tr>                           
                                <td>DESCRIPCIÓN</td>
                                <td style="width:20%"></td>
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
                        <asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("descripcion") %>' />
                    </td> 
                    <td style="width:50px"><asp:LinkButton ID="EditButton" class="editBtn" runat="Server" Text="Editar" CommandName="Edit" /></td>
                </tr>
            </ItemTemplate>
            
            <EditItemTemplate>
                <tr class="editRow">
                <td>
                    <asp:TextBox ID="txtEditId" runat="server" Text='<%#Bind("id") %>' Enabled="False" Visible="false"/>
                    <asp:TextBox ID="txtEditDescripcion" runat="server" Text='<%#Bind("descripcion") %>' />
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
                        <asp:Label runat="server" ID="lbDescripcion1" AssociatedControlID="txtDescripcion"/>
                        <asp:TextBox ID="txtDescripcion" runat="server" Text='<%#Bind("descripcion") %>' />
                    </td>
                    <td><asp:LinkButton ID="InsertButton" runat="server" class="addBtn" CommandName="Insert" Text="Insertar" /></td>
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
    </div>

    <div style="float:right; width:48%">
        <section>
            <div class="formHolder" id="searchBoxTop">
                <div class="formHolderLine">
                    <h2>Índice CAC</h2>
                </div>
            </div>
        </section>
        
        <asp:ListView ID="lvCAC" runat="server" 
            onitemediting="lvCAC_ItemEditing"
            onitemupdating="lvCAC_ItemUpdating"
            onitemcanceling="lvCAC_ItemCanceling"
            oniteminserting="lvCAC_ItemInserting" 
            InsertItemPosition="FirstItem" 
            DataKeyNames="id" 
            OnPreRender="ListPager_PreRender">
            <LayoutTemplate>      
                <section>     
                    <table>                            
                        <thead id="tableHead">
                            <tr>                           
                                <td>AÑO</td>
                                <td>MES</td>
                                <td>VALOR</td>
                                <td style="width:20%"></td>
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
                        <asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("Fecha", "{0:yyyy}") %>' />
                    </td>
                    <td><asp:Label ID="lbDireccion" runat="Server" Text='<%#Eval("Fecha", "{0:MMMM}") %>' /></td>
                    <td><asp:Label ID="lbTelefono" runat="Server" Text='<%#Eval("valor") %>' /></td>  
                    <td style="width:50px"><asp:LinkButton ID="EditButton" class="editBtn" runat="Server" Text="Editar" CommandName="Edit" /></td>
                </tr>
            </ItemTemplate>
            
            <EditItemTemplate>
                <tr class="editRow">
                <td>
                    <asp:TextBox ID="txtEditId" runat="server" Text='<%#Bind("id") %>' Enabled="False" Visible="false"/>
                </td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtEditValor" runat="server" Text='<%#Bind("valor") %>' />
                </td>
                <td> 
                    <asp:LinkButton ID="CancelButton" runat="server" class="cancelBtn" CommandName="Cancel" Text="Cancelar" /> 
                    <asp:LinkButton ID="UpdateButton" runat="server" class="saveBtn" CommandName="Update" Text="Actualizar" /> 
                </td>
                </tr>
            </EditItemTemplate>
                           
            <InsertItemTemplate>
                <tr class="editRow">
                    <td colspan="2">
                        <asp:TextBox ID="txtFecha" runat="server" style="width:288px"></asp:TextBox>
                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="orange" TargetControlID="txtFecha" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbTelefono" AssociatedControlID="txtValor"/>
                        <asp:TextBox ID="txtValor" runat="server" Text='<%#Bind("valor") %>' />
                    </td>
                    <td><asp:LinkButton ID="InsertButton" runat="server" class="addBtn" CommandName="Insert" Text="Insertar" /></td>
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
    </div>
    </div>
</asp:Content>
