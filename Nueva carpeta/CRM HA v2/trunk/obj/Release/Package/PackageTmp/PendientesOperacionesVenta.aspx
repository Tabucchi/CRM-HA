<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="PendientesOperacionesVenta.aspx.cs" Inherits="crm.PendientesOperacionesVenta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function redir(id) {
            window.location.href = "DetalleOperacionVenta.aspx?idOV=" + id;
        }
    </script>

    <script>
        function selectUnselectAll(val) {
            if (val == true) {
                $('input:checkbox').prop('checked', true);
                $('input:checkbox').attr('checked', true);
            }
            else {
                $('input:checkbox').prop('checked', false);
                $('input:checkbox').attr('checked', false);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <section>
                <div class="formHolder" id="searchBoxTop">
                    <div class="headOptions headLine">
                        <h2>Nuevas operaciones de venta a confirmar</h2>
                        <div style="float:right;">
                            <div style="float:right">
                                <b><asp:Button ID="btnConfirmarOV" Text="Validar" CssClass="formBtnNar2" runat="server" OnClick="btnConfirmarOV_Click" style="margin-right: 10px;"/></b>
                                <b><asp:Button ID="btnCancelar" Text="Cancelar" CssClass="formBtnGrey1" runat="server" OnClick="btnCancelar_Click"/></b>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

             <asp:Panel ID="pnlMensajeCancelar" runat="server" Visible="false">
                <section>
                    <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px;">
                        <div style="float:left; margin-top: 8px;">¿Esta seguro que no desea válidar las operaciones de ventas seleccionadas en la lista?</div>
                        <div style="float:right">
                            <asp:Button ID="btnCancelarSi" Text="Si" CssClass="formBtnNar2" runat="server" OnClick="btnCancelarSi_Click" style="margin-right: 11px;"/></b>
                            <asp:Button ID="btnCancelarNo" Text="No" CssClass="formBtnGrey1" runat="server" OnClick="btnCancelarNo_Click" style="margin-right: 35px;"/></b>
                        </div>
                    </div>
                </section>
            </asp:Panel>
                
            <asp:ListView ID="lvOperacionVenta" runat="server">
                <LayoutTemplate>
                    <section>            
                        <table style="width:100%">                            
                            <thead id="tableHead">
                                <tr>     
                                    <td style="width: 1%; text-align:center"><input id="checkall" type="checkbox" onchange="selectUnselectAll(checked);" /></td>
                                    <td style="width: 22%; text-align:center">CLIENTE</td>
                                    <td style="width: 22%; text-align:center">OBRA</td>                            
                                    <td style="width: 20px; text-align:right">PRECIO BASE</td>
                                    <td style="width: 12px; text-align:right">PRECIO ACORDADO</td>
                                    <td style="width: 20px; text-align:center">MONEDA ACORDADA</td>
                                    <td style="width: 20px; text-align:center">ESTADO</td>
                                    <td style="width: 2%; text-align:center"></td>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </tbody>                        
                        </table>
                    </section>   
                </LayoutTemplate>                
                <ItemTemplate>                   
                    <tr style="cursor: pointer">
                        <td style="text-align:center">
                            <asp:CheckBox ID="chBoxOV" runat="server" />
                            <asp:Label ID="lbId" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                        </td>
                        <td style="text-align:center">
                            <a href="#" alt="Cliente" class="tooltip tooltipColor">
                                <asp:Label ID="Label1" runat="Server" Text='<%#Eval("GetEmpresa") %>' />
                            </a>
                        </td>
                        <td style="text-align:center">
                            <a href="#" alt="Obra" class="tooltip tooltipColor">
                                <asp:Label ID="lbNombre" runat="Server" Text='<%#Eval("GetProyecto") %>' />
                            </a>
                        </td>
                        <td style="text-align:right">
                            <a href="#" alt="Moneda acordada" class="tooltip tooltipColor">
                                <asp:Label ID="Label4" runat="Server" Text='<%#Eval("GetPrecioBase") %>' />
                            </a>
                        </td>
                        <td style="text-align:right">
                            <a href="#" alt="Moneda acordada" class="tooltip tooltipColor">
                                <asp:Label ID="Label5" runat="Server" Text='<%#Eval("GetPrecioAcordado") %>' />
                            </a>
                        </td>
                        <td style="text-align:center">
                            <a href="#" alt="Moneda acordada" class="tooltip tooltipColor">
                                <asp:Label ID="Label2" runat="Server" Text='<%#Eval("GetMoneda") %>' />
                            </a>
                        </td>
                        <td style="text-align:center">
                            <a href="#" alt="Estado" class="tooltip tooltipColor">
                                <asp:Label ID="Label3" runat="Server" Text='<%#Eval("GetEstado") %>' />
                            </a>
                        </td>
                        <td style="text-align:center">
                            <a class="detailBtn" href="DetalleOperacionVenta.aspx?idOV=<%# Eval("id") %>&default=true"></a>
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <section>
                        <table id="Table1" style="width:100%" runat="server">
                            <tr>
                                <td style="text-align:center"><b>No se encontraron nuevas operaciones de venta.<b/></td>
                            </tr>
                        </table>
                    </section>
                </EmptyDataTemplate>
            </asp:ListView>     
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnConfirmarOV" />             
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

