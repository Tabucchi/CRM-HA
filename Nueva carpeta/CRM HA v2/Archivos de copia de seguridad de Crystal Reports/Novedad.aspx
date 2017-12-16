<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Novedad" Codebehind="Novedad.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>        
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager> 
    <link id="link1" rel="stylesheet" href="Estilos/Calendario.css" type="text/css" runat="server" />
    
    <div class="pal grayArea uiBoxGray noborder">
        <asp:ListView ID="lvNovedades" runat="server"
            InsertItemPosition="FirstItem"
            onitemediting="lvNovedades_ItemEditing"
            onitemupdating="lvNovedades_ItemUpdating"
            onitemcanceling="lvNovedades_ItemCanceling"
            onitemdeleting="lvNovedades_ItemDeleting" oniteminserting="lvNovedades_ItemInserting" 
            DataKeyNames="id" 
            OnPreRender="ListPager_PreRender">    
            <LayoutTemplate>
                <div class="PrettyGrid">
                    <div style="overflow:auto; height: 200px">             
                    <table border="0" cellpadding="1">                            
                        <thead>
                            <tr>
                                <th style="width:8%; height:20px;" class="column_head">ID</th>                              
                                <th style="width: 15%" class="column_head">DESCRIPCION</th>
                                <th style="width: 10%" class="column_head">FECHA</th>
                                <th style="width: 10%" class="column_head">USUARIO</th>
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
                    <td align="left"><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("descripcion") %>' /></td>
                    <td><asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("fecha") %>' /></td>
                    <td><asp:Label ID="lbUsuario" runat="Server" Text='<%#Eval("Usuario") %>' /></td>   
                    <td><asp:LinkButton ID="EditButton" runat="Server" Text="Editar" CommandName="Edit" /></td>
                </tr>
            </ItemTemplate>
            
            <EditItemTemplate>
                <tr style="background-color: #ADD8E6">
                <td>
                  <asp:TextBox ID="txtEditId" runat="server" Text='<%#Bind("id") %>' Enabled="False" />
                </td>
                <td>
                  <asp:TextBox ID="txtEditDescripcion" runat="server" Text='<%#Bind("descripcion") %>' />
                </td>
                <td>
                  <asp:TextBox ID="txtEditFecha" runat="server" Text='<%#Bind("fecha") %>'/>                  
                  <ajax:CalendarExtender ID="CalendarEdit" TargetControlID="txtEditFecha" Format="dd/MM/yyyy" runat="server" CssClass="MyCalendar"/>      
                </td>
                <td>
                  <asp:TextBox ID="txtEditUsuario" runat="server" Text='<%#Bind("Usuario") %>' Enabled="False" />
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
                        <asp:Label runat="server" ID="lbDescripcion" AssociatedControlID="txtDescripcion"/>
                        <asp:TextBox ID="txtDescripcion" runat="server" Text='<%#Bind("descripcion") %>' /><br />
                    <td>
                        <asp:Label runat="server" ID="lbFecha" AssociatedControlID="txtFecha"/>
                        <asp:TextBox ID="txtFecha" runat="server" Text='<%#Bind("fecha") %>' />
                        <ajax:CalendarExtender ID="CalendarInsert" TargetControlID="txtFecha" Format="d/MM/yyyy" runat="server" CssClass="MyCalendar"/>
                    </td>
                    <td></td>
                    <td><asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert" Text="Insertar" /></td>
                  </td>
                </tr>
            </InsertItemTemplate>
                         
            <AlternatingItemTemplate>
                <tr style="background-color:#EFEFEF; cursor:pointer;" onclick="Visible(<%# Eval("id")%> )">
                    <td><asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' /></td>
                    <td align="left"><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("descripcion") %>' /></td>
                    <td><asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("fecha") %>' /></td>
                    <td><asp:Label ID="lbUsuario" runat="Server" Text='<%#Eval("Usuario") %>' /></td>   
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
        </br>
        <div class="rfloat" align="right" style="width:48%;">Paginas:
            <asp:DataPager ID="_moviesGridDataPager" PageSize="20" runat="server" PagedControlID="lvNovedades">
                <Fields><asp:NumericPagerField/></Fields>
            </asp:DataPager>
        </div>    
    </div>    
</div>
</asp:Content>

