<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ResumenCuotasObra.aspx.cs" Inherits="crm.ResumenCuotasObra" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/orange.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    
    <section>
        <div class="formHolder formHolderCalendar" id="searchBoxTop">
            <div class="headOptions headLine">
                <a href="#" class="toggleFiltr" style="margin-top: 9px; margin-right:5px">v</a>
                <h2>Resumen Cuotas a cobrar por Obra</h2>
            </div>
            <div class="hideOpt" style="width: 100%;">
                <label class="col3">
                    <span>FECHA</span>
                    <asp:TextBox ID="txtFechaDesde" runat="server" class="textBox textBoxForm" placeholder="Desde" style="width: 106px; margin-right: 5px;"></asp:TextBox>
                    <ajax:CalendarExtender ID="ce1" runat="server" CssClass="orange" TargetControlID="txtFechaDesde" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  

                    <asp:TextBox ID="txtFechaHasta" runat="server" class="textBox textBoxForm" placeholder="Hasta" style="width: 106px;"></asp:TextBox>
                    <ajax:CalendarExtender ID="ce2" runat="server" CssClass="orange" TargetControlID="txtFechaHasta" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />                  
                </label>
            </div>

            <div class="hideOpt footerLine" style="width: 100%;">
                <label class="leftLabel" style="width: 17%;">
                    <asp:Button ID="btnBuscar" Text="Buscar" CssClass="formBtnNar" runat="server" OnClick="btnBuscar_Click" style="margin-right: 32px !important;"/>
                    <asp:Button ID="btnVerTodos" Text="Ver Todos" CssClass="formBtnGrey1" runat="server" OnClick="btnVerTodos_Click"/>
                </label>
            </div>
        </div>
    </section>

    <asp:Panel runat="server" Visible="false"> 
        <asp:FileUpload ID="fileArchivo" runat="server" />
    </asp:Panel>

    <asp:ListView ID="lvResumen" runat="server">
        <LayoutTemplate>
            <section>            
                <table style="width:100%">                            
                    <thead id="tableHead">
                        <tr>    
                            <td style="width: 5%; text-align: center"></td> 
                            <td style="width: 15%; text-align: center">FECHA</td>
                            <td style="width: 15%; text-align: center">ARCHIVO</td>
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
                <td style="text-align: center">
                    <%# Container.DataItemIndex + 1 %>
                </td>
                <td style="text-align: center">
                    <asp:Label ID="Label1" runat="Server" Text='<%#Eval("GetFecha") %>' />
                </td>
                <td style="text-align: center">
                    <asp:LinkButton ID="lkbDescargar" runat="server" class="detailBtn" style="margin-right: auto !important;" OnClick="lkbDescargar_Click" CommandArgument='<%# Eval("Id") %>'/>
                </td>
            </tr>
        </ItemTemplate>
                                       
        <EmptyDataTemplate>
            <section>
                <table style="width:100%" runat="server">
                    <tr>
                        <td style="text-align: center"><b>No se encontraron archivos.<b/></td>
                    </tr>
                </table>
            </section>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Content>
