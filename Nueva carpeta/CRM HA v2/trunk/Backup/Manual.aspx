<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Manual" Codebehind="Manual.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager> 
    <link id="link1" rel="stylesheet" href="Estilos/Calendario.css" type="text/css" runat="server" /> 
    <asp:Panel ID="filtro" runat="server" DefaultButton="btnBuscar">
        <div class="headOptions">
            <h2>EMPRESA</h2>
            <asp:TextBox ID="txtEmpresa" CssClass="floatInput" runat="server"></asp:TextBox>
            <ajax:AutoCompleteExtender runat="server"  ID="autoComplete1" 
                        TargetControlID="txtEmpresa" 
                        ServiceMethod="GetEmpresasAutoCompletar" 
                        ServicePath="MyAutocompleteService.asmx" 
                        MinimumPrefixLength="2"                            
                        CompletionInterval="200"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        UseContextKey ="True">                     
                </ajax:AutoCompleteExtender>
            <h2>MANUAL</h2>
            <asp:TextBox ID="txtManual" CssClass="floatInput" runat="server" ></asp:TextBox>
            <ajax:AutoCompleteExtender runat="server"  ID="autoComplete2" 
                        TargetControlID="txtManual" 
                        ServiceMethod="GetManualesPosibles" 
                        ServicePath="MyAutocompleteService.asmx" 
                        MinimumPrefixLength="2"                            
                        CompletionInterval="200"
                        EnableCaching="true" 
                        CompletionSetCount="10"
                        UseContextKey ="True">                     
                </ajax:AutoCompleteExtender>
            <div style="float:right">
                <asp:Button ID="btnBuscar" CssClass="formBtnGrey" runat="server" Text="Buscar" onclick="btnBuscar_Click"></asp:Button>
                <asp:Button ID="btnNuevoManual" runat="server" Text="Nuevo Manual" CssClass="formBtnGrey" class="boton" onclick="btnNuevoManual_Click" />
            </div>
        </div>
    </asp:Panel>
    
    <asp:ListView ID="lvManuales" runat="server" OnPreRender="ListPager_PreRender">
        <LayoutTemplate>
                <table border="0" cellpadding="1">                            
                    <thead>
                        <tr>
                            <td>ID</td>                              
                            <td>TITULO</td>
                            <td>FECHA</td>
                            <td>USUARIO</td>
                            <td>EMPRESA</td>
                            <td></td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>                        
                </table>
        </LayoutTemplate>
        <ItemTemplate>                   
            <tr onclick="Visible(<%# Eval("id")%> )">
                <td><asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' Enabled="False" /></td>
                <td><asp:Label ID="lbTitulo" runat="Server" Text='<%#Eval("titulo") %>' /></td>
                <td><asp:Label ID="lbFecha" runat="Server" Text='<%#Eval("Fecha", "{0:d}") %>' /></td>
                <td><asp:Label ID="lbUsuario" runat="Server" Text='<%#Eval("GetUsuario") %>' /></td> 
                <td><asp:Label ID="lbEmpresa" runat="server" Text='<%#Eval("GetEmpresa") %>' /></td>  
                <td><a href="DetalleManual.aspx?id=<%# Eval("id") %>" class="detailBtn" >Ver</a></td>
            </tr>
        </ItemTemplate>
                                                 
        <EmptyDataTemplate>
            <table id="Table1" width="100%" runat="server">
                <tr>
                    <td align="center"><b>No data found.<b/></td>
                </tr>
            </table>
        </EmptyDataTemplate>
    </asp:ListView>
    <br />
    <div class="rfloat" align="right">Paginas:
        <asp:DataPager ID="_moviesGridDataPager" PageSize="25" runat="server" PagedControlID="lvManuales">
            <Fields><asp:NumericPagerField/></Fields>
        </asp:DataPager>
    </div>
</section>
</asp:Content>


