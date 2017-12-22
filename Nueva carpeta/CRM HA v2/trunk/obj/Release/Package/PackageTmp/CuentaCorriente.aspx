﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CuentaCorriente.aspx.cs" Inherits="crm.CuentaCorriente" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        // simple redirect to your detail page, passing the selected ID 
        function redir(id) {
            window.location.href = "DetalleCC.aspx?idCC=" + id;
        }
    </script>

    <script src="js/autocomplete/jquery-1.4.1.min.js"></script>
    <link href="js/autocomplete/jquery.autocomplete.css" rel="stylesheet" />
    <script src="js/autocomplete/jquery.autocomplete.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtSearch.ClientID%>").autocomplete('/Web-Service/Search_CS.ashx');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <section>
        <div class="formHolder" id="searchBoxTop">
            <div class="headOptions headLine">
                <h2>Cuentas Corrientes</h2>              
                
                <div style="float: right; margin-top: 3px;">
                    <label class="col2" style="width:510px">
                        <asp:Label ID="lbCantUnidades" runat="server" style="width: 15% !important;" Text="Clientes"></asp:Label>
                        <%--<asp:DropDownList ID="cbEmpresa" runat="server" style="width:246px" AutoPostBack="True" OnSelectedIndexChanged="cbEmpresa_SelectedIndexChanged"/>--%>  
                        
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>                       
                                <asp:TextBox ID="txtSearch" name="txtSearch" runat="server" style="width: 44%" CssClass="textBox textBoxForm" ToolTip="1"></asp:TextBox>  
                                <asp:Button ID="btnBuscar" runat="server" CssClass="formBtnNar" Text="Buscar" OnClick="btnBuscar_Click" style="float: left; margin-left: 8px;" ToolTip="2"/> 
                                <asp:Button ID="btnVerTodos" Text="Ver todos" CssClass="formBtnGrey1" runat="server" OnClick="btnVerTodos_Click" style="margin-left: 8px;" ToolTip="3"/>                  
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnBuscar"/>
                                <asp:PostBackTrigger ControlID="btnVerTodos"/>
                            </Triggers>
                        </asp:UpdatePanel> 
                                            
                    </label>
                </div>
            </div>
        </div>
    </section>
                 
    <asp:ListView ID="lvCC" runat="server">
        <LayoutTemplate>
            <section>            
                <table style="width:100%">                            
                    <thead id="tableHead">
                        <tr>     
                            <td style="text-align: center; width:30%;">CLIENTE</td>
                            <td style="text-align: center; width:22%;">SALDO EN CUENTA CORRIENTE</td>
                            <td style="width:6%;"></td>
                        </tr>
                    </thead>
                </table>
                <div class="tableBody">
                    <table style="width:100%">
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>    
                    </table>
                </div>
                <div class="tableFoot">
                    <table style="width:100%">
                        <tfoot class="footerTable">
                            <tr>
                                <td style="width: 30%;"></td>                           
                                <td style="width: 21%; text-align:right; padding-right: 14px;"><b><asp:Label ID="lbTotalCtaCte" runat="server"></asp:Label></b></td>
                                <td style="width: 8%;"></td> 
                            </tr>
                        </tfoot>                   
                    </table>
                </div>
            </section>   
        </LayoutTemplate>
                        
        <ItemTemplate>                             
            <tr onclick="redir('<%# Eval("id") %>');" style="cursor: pointer">
                <td style="text-align: left; width:30%;">
                    <asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("GetEmpresa") %>' />
                </td>
                <td style="text-align: right; width:21%;">
                    <asp:Label ID="lbSaldo" runat="Server" Text='<%#Eval("GetSaldo", "{0:#,#.00}") %>' />
                </td>
                <td style="width:6%;">
                    <a class="detailBtn" href="DetalleCC.aspx?idCC=<%# Eval("id") %>"></a>
                </td>
            </tr>
        </ItemTemplate>
                         
        <EmptyDataTemplate>
            <section>
                <table id="Table1" width="100%" runat="server">
                    <tr>
                        <td align="center"><b>No se encontraron cuentas corrientes registradas.<b/></td>
                    </tr>
                </table>
            </section>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Content>