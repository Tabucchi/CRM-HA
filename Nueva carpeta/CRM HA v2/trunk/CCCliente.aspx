<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="CCCliente.aspx.cs" Inherits="crm.CCCliente" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
    <link href="css/orange.css" rel="stylesheet" />

    <script type="text/javascript">
        // simple redirect to your detail page, passing the selected ID 
        function redir(id) {
            window.location.href = "DetalleCuota.aspx?idCC=" + id;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    
    <section>
        <div class="headOptions">
            <h2>Cuentas</h2>
        </div>

        <asp:Panel runat="server" DefaultButton="btnBuscar">
        <div class="formHolder" id="searchBoxTop">
            <div>
                <label class="col3">
                    <span>Estado</span>
                    <asp:DropDownList ID="ddlEstado" runat="server" Height="32px"><%--<asp:ListItem Text="Todas" Value="2" />--%></asp:DropDownList>
                </label>
                <label class="col3 rigthLabel">
                    <asp:Button ID="btnBuscar" Text="Buscar" class="formBtnNar" runat="server" onclick="btnBuscar_Click" />
                </label>
            </div>
        </div>
        </asp:Panel>
    </section>
                    
    <asp:ListView ID="lvClientes" runat="server">
        <LayoutTemplate>
            <section>            
                <table style="width:100%">                            
                    <thead id="tableHead">
                        <tr>     
                            <td style="width:15px">CLIENTE</td>                         
                            <td style="width:25px">PROYECTO</td>   
                            <td style="width:20px">TOTAL</td>
                            <td style="width:20px">SALDO</td>
                            <td style="width:15px">ESTADO</td>
                            <td style="width:5px"></td>
                        </t>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>                        
                </table>
            </section>   
        </LayoutTemplate>
                
        <ItemTemplate>                   
            <tr onclick="redir('<%# Eval("id") %>');" style="cursor: pointer">
                <td align="left"><asp:Label ID="Label3" runat="Server" Text='<%#Eval("GetEmpresa") %>' /></td>
                <td align="left"><asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("GetProyecto") %>' />&nbsp;(Unidad Funcional)</td>
                <td align="left">$&nbsp;<asp:Label ID="Label1" runat="Server" Text='<%#Eval("GetTotal") %>' /></td>
                <td align="left">$&nbsp;<asp:Label ID="Label2" runat="Server" Text='<%#Eval("saldo") %>' /></td>
                <td align="left"><asp:Label ID="Label4" runat="Server" Text='<%#Eval("GetEstado") %>' /></td>
                <td>
                    <a  class="detailBtn" href="DetalleCuotaCliente.aspx?idCC=<%# Eval("id") %>"></a>
                </td>
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
</asp:Content>
