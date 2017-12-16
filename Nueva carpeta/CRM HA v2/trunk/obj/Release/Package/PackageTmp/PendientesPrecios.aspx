<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="PendientesPrecios.aspx.cs" Inherits="crm.PendientesPrecios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

    <script src="js/jquery.mask.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.decimal').mask("#.##0,00", { reverse: true });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <section>
                <div class="formHolder" id="searchBoxTop">
                    <div class="headOptions headLine">
                        <h2>Nuevos precios a confirmar</h2>
                        <div style="float:right;">
                            <div style="float:right">
                                <b><asp:Button ID="btnConfirmar" Text="Confirmar" CssClass="formBtnNar2" runat="server" OnClick="btnConfirmar_Click" style="margin-right: 10px;"/></b>
                                <b><asp:Button ID="btnCancelar" Text="Cancelar" CssClass="formBtnGrey1" runat="server" OnClick="btnCancelar_Click"/></b>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <asp:Panel ID="pnlMensajeCancelar" runat="server" Visible="false">
                <section>
                    <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px;">
                        <div style="float:left; margin-top: 8px;">¿Esta seguro que desea no válidar las operaciones de ventas seleccionadas en la lista?</div>
                        <div style="float:right">
                            <asp:Button ID="btnCancelarSi" Text="Si" CssClass="formBtnNar2" runat="server" OnClick="btnCancelarSi_Click" style="margin-right: 11px;"/></b>
                            <asp:Button ID="btnCancelarNo" Text="No" CssClass="formBtnGrey1" runat="server" OnClick="btnCancelarNo_Click" style="margin-right: 35px;"/></b>
                        </div>
                    </div>
                </section>
            </asp:Panel>
                
            <asp:ListView ID="lvActualizacionPrecios" runat="server">
                    <LayoutTemplate>
                        <section>            
                            <table style="width:100%">                            
                                <thead id="tableHead">
                                    <tr>
                                        <td style="width: 1%; text-align:center"><input id="checkall" type="checkbox" onchange="selectUnselectAll(checked);" /></td>
                                        <td style="width: 10%; text-align:center">COD. UF</td>
                                        <td style="width: 20%; text-align:center">OBRA</td>
                                        <td style="width: 9%; text-align:center">MONEDA</td>
                                        <td style="width: 25%; text-align:center">VALOR ANTIGUO</td>
                                        <td style="width: 25%; text-align:center">VALOR NUEVO</td>
                                        <td style="width: 9%; text-align:center">ESTADO</td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                </tbody>
                            </table>
                        </section> 
                    </LayoutTemplate>
                    <ItemTemplate>
                       <tr id="row<%# Eval("id")%>" style="color:#b40b0b;">
                            <td>
                                <asp:CheckBox ID="chBox" runat="server" />
                                <asp:Label ID="lbId" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                            </td>
                            <td style="text-align:center">
                                <asp:Label ID="lbCodUF" runat="server" Text='<%# Eval("CodigoUF") %>'></asp:Label>
                            </td>
                            <td style="text-align:center">
                                <%# Eval("GetProyecto")%>
                                <asp:Label ID="lbIdProyecto" runat="server" Text='<%# Eval("IdProyecto")%>' Visible="false"></asp:Label>                                
                            </td>
                            <td style="text-align:center">
                                <%# Eval("GetMoneda")%>
                            </td>
                            <td style="text-align:right">
                                <asp:Label ID="lbValorViejo" runat="server" Text='<%# Eval("ValorViejo", "{0:#,#}")%>'></asp:Label>                                 
                            </td>
                            <td style="text-align:right">
                                <asp:TextBox ID="lbValorNuevo" runat="server" CssClass="decimal" Text='<%# Eval("ValorNuevo")%>' style="width: 98%; text-align:right"></asp:TextBox> 
                            </td>
                            <td style="text-align:center">
                                <%# Eval("GetEstado") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <section>
                            <table id="Table1" style="width: 100%" runat="server">
                                <tr>
                                    <td style="text-align:center"><b>No se encontraron nuevos precios.<b/></td>
                                </tr>
                            </table>
                        </section>
                    </EmptyDataTemplate>
                </asp:ListView>    
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnConfirmar" />             
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
