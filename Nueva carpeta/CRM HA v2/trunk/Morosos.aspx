<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Morosos.aspx.cs" Inherits="crm.Morosos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <asp:HiddenField ID="hfTotal" runat="server" />
    <asp:HiddenField ID="hfIdCC" runat="server" />

    <section>
        <div class="headOptions">
            <div style="float:left"><h2>Deudas pendientes</h2></div>
            <div style="float:right">
                <label class="rigthLabel" style="width: 37%;">
                    <asp:Button ID="btnDescargar" runat="server" Text="Descargar" CssClass="formBtnNar" OnClick="btnDescargar_Click" />
                </label>
            </div>
        </div>
        <div class="formHolder">
            <div class="headOptions headLine" style="padding-bottom: 0px;">
                <div align="left" class="h7" style="float:left;width: 94%;margin-top: 9px;">Listado de clientes que adeudan más de dos cuotas</div>
            </div>
        </div>
    </section>
        
    <asp:ListView ID="lvMorosos" runat="server" OnItemCommand="lvMorosos_ItemCommand"> 
            <LayoutTemplate>
                <section>
                    <table style="margin-top:-25px">
                        <thead id="tableHead">
                            <tr>
                                <td style="width: 10%; text-align: center">CLIENTE</td>
                                <td style="width: 10%; text-align: center">MONTO (PESOS)</td>
                                <td style="width: 4%; text-align: center">HISTORIAL DE PAGOS</td> 
                                <td style="width: 4%; text-align: center">COMENTARIOS</td>
                            </tr>
                        </thead>
                        <tbody style="height:80px; overflow:scroll">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </tbody>  
                        <tfoot class="footerTable">
                            <tr>
                                <td style="width: 3%;"></td>
                                <td style="width: 3%; text-align: center;"><b><asp:Label ID="lbTotal" runat="server"></asp:Label></b></td>
                                <td style="width: 3%; text-align:right"></td>
                                <td style="width: 3%; text-align:right"></td>
                            </tr>
                        </tfoot>   
                    </table>
                </section>
            </LayoutTemplate>
            <ItemTemplate>
                <tr style="cursor: pointer;">             
                    <td style="text-align: center">
                        <asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("empresa") %>'/>
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto") %>'/>
                    </td>
                    <td style="text-align: right">
                        <a class="detailBtn" href="DetalleCuota2.aspx?idCC=<%# Eval("idCuentaCorriente") %>" style="margin-right: 40%;"></a>
                    </td>   
                    <td>
                        <asp:LinkButton ID="btnComentarioCuota" runat="server" CssClass="saveComment" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "idCuentaCorriente")%>' CommandName="Comentario" Text="Comentario" ToolTip="Agregar comentario" style="margin-right: 40%;" />
                    </td>         
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <section>
                    <table style="width:100%; margin-top: -34px;" runat="server">
                        <tr>
                            <td style="text-align:center"><b>No se encontraron Morosos.<b/></td>
                        </tr>
                    </table>
                </section>
            </EmptyDataTemplate>
        </asp:ListView> 

        <asp:Panel ID="pnlComentario" runat="server" HorizontalAlign="Center" CssClass="modal" style="background-color:white; width:800px; top: -100px;">
            <table style="width: 100%">               
                <asp:Label ID="lblClose" Text="X" runat="server" CssClass="closebtn"></asp:Label>           
                <tr>
                    <td colspan="2">
                        <section>
                            <div class="headOptions">
                                <h2 style="text-align: center; float: inherit; padding: 11px 0px 0px 0px;">Morosos</h2>
                                <hr />
                            </div>
                        </section>
                    </td>
                </tr> 
                <tr>
                    <td colspan="2">
                        <label style="width:100%;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" style="overflow-y: scroll; height: 350px;">
                                <ContentTemplate>
                                    <section>
                                        <div style="padding: 2px 4px 0px 11px;">
                                            <asp:Repeater ID="rptComentarios" runat="server">
                                                <HeaderTemplate></HeaderTemplate>
                                                <ItemTemplate>
                                                    <li style="text-align: left; margin-bottom:-16px">
                                                        <b style="margin-left: -5px; font-size: 16px;"><%#Eval("comentario") %>.
                                                        <br />
                                                        <div style="font-weight: inherit; font-size: 14px; color: #666; font-style: italic"><b><%#Eval("GetUsuario")%></b>&nbsp;-&nbsp;<b><%#Eval("Fecha", "{0:dddd}")%>, <%#Eval("Fecha", "{0:dd}")%> de <%#Eval("Fecha", "{0:MMMM}")%> de <%#Eval("Fecha", "{0:yyyy}")%> a las <%#Eval("Fecha", "{0:hh:mm tt}")%></b></div>
                                                    </li>
                                                </ItemTemplate>
                                                <FooterTemplate></FooterTemplate>
                                                <SeparatorTemplate></SeparatorTemplate>
                                            </asp:Repeater>
                                            </div>
                                        </section>
                                    </ContentTemplate>
                            </asp:UpdatePanel>   
                        </label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                        <asp:Panel ID="pnlNuevoComentario" CssClass="comments" runat="server" DefaultButton="btnAgregarComentario">
                            <label>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" style="width:100%">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtComentario" runat="server" CssClass="textBox textBoxForm" TextMode="MultiLine" style="width:93%" placeholder="Ingrese un comentario..."></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>                                                       
                            </label>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <section>
                            <div class="formHolder" style="-webkit-box-shadow: inherit; box-shadow: inherit; overflow: inherit; padding-top:4px !important; padding-right: 22px !important;">
                                <label style="width: 100%;"><asp:Button ID="btnAgregarComentario" runat="server" CssClass="formBtnNar" Text="Agregar" UseSubmitBehavior="true" OnClick="btnAgregarComentario_Click"/></label>
                            </div>
                        </section>
                    </td>
                </tr>
            </table>
        </asp:Panel>   
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <ajax:ModalPopupExtender ID="ModalPopupExtender" runat="server" 
            TargetControlID="HiddenField1"
            PopupControlID="pnlComentario" 
            CancelControlID="lblClose"        
            BackgroundCssClass="ModalBackground"
            DropShadow="true" />
           
<CR:crystalreportviewer ID="CrystalReportViewer" runat="server" 
    AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource" 
    Width="894px" DisplayToolbar="False" Visible="false" />
<CR:crystalreportsource ID="CrystalReportSource" runat="server" 
    Visible="false">
    <Report FileName="Reportes/Moroso.rpt">
    </Report>
</CR:crystalreportsource>
</asp:Content>
