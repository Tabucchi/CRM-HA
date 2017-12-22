<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CuotasAnticipo.ascx.cs" Inherits="crm.Controles.CuotasAnticipo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<link href="../css/masterStyle.css" rel="stylesheet" />

<asp:HiddenField ID="hfCC" runat="server" />
<asp:HiddenField ID="hfFormaPagoOv" runat="server" />
<asp:HiddenField ID="hfCantCuotasAdelantadas" runat="server" />
<div style="margin-bottom:4%">
<asp:ListView ID="lvCuotas" runat="server">
    <LayoutTemplate>
        <section>
            <table style="margin-top:9px">
                <thead id="tableHead">
                    <tr>
                        <td></td>
                        <td>CAC (%)</td>
                        <td style="width: 10%;">SALDO AJUSTADO</td>
                        <td>MONTO</td>
                        <td style="width: 8%">GASTOS ADTVO.</td>
                        <td style="width: 8%;">1re Venc.</td>
                        <td>IMPORTE</td>
                        <td style="width: 8%;">2do Venc.</td>
                        <td>IMPORTE</td>
                        <td>ESTADO</td>
                        <td>RECIBO</td>
                        <td>SALDO</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </tbody>
            </table>
        </section>
    </LayoutTemplate>
    <ItemTemplate>
        <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer">
            <td align="left">
                <a href="#" alt="Nro" class="tooltip tooltipColor">
                    <asp:Label ID="lbId" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                    <asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("nro") %>' />
                </a>
            </td>
            <td align="left">
                <a href="#" alt="CAC (%)" class="tooltip tooltipColor">
                    <asp:Label ID="lbCAC" runat="Server" Text='<%#Eval("GetVariacionCAC") %>' />
                </a>
            </td>
            <td align="left">
                <a href="#" alt="Saldo ajustado" class="tooltip tooltipColor">
                    <asp:Label ID="lbSaldoPendiente" runat="Server" Text='<%#Eval("GetMontoAjustado") %>' />
                </a>
            </td>
            <td align="left">
                <a href="#" alt="Monto" class="tooltip tooltipColor">
                    <asp:Label ID="lbMonto" runat="Server" Text='<%#Eval("GetMonto1") %>' />
                </a>
            </td>
            <td align="left">
                <a href="#" alt="Gastos adtvo." class="tooltip tooltipColor">
                    <asp:Label ID="lbComision" runat="Server" Text='<%#Eval("GetTotalComision") %>' />
                </a>
            </td>
            <td align="left">
                <a href="#" alt="1re vencimiento" class="tooltip tooltipColor">
                    <asp:Label ID="lbFecha1venc" runat="Server" Text='<%#Eval("FechaVencimiento1", "{0:dd/MM/yyyy}") %>' />
                </a>
            </td>
            <td align="left">
                <a href="#" alt="Importe 1re vencimiento" class="tooltip tooltipColor">
                    $&nbsp;<asp:Label ID="lbVencimiento1" runat="Server" Text='<%#Eval("GetVencimiento1") %>' />
                </a>
            </td>
            <td align="left">
                <a href="#" alt="2do vencimiento" class="tooltip tooltipColor">
                    <asp:Label ID="lbFecha2venc" runat="Server" Text='<%#Eval("FechaVencimiento2", "{0:dd/MM/yyyy}") %>' />
                </a>
            </td>
            <td align="left">
                <a href="#" alt="Importe 2do vencimiento" class="tooltip tooltipColor">
                    $&nbsp;<asp:Label ID="lbMontoAjustado" runat="Server" Text='<%#Eval("GetVencimiento2") %>' />
                </a>
            </td>
            <td align="left">
                <a href="#" alt="Estado" class="tooltip tooltipColor">
                    <asp:Label ID="lbEstado" runat="Server" Text='<%#Eval("GetEstado") %>' />
                </a>
            </td>
            <td align="left">
                <a href="#" alt="Recibo" class="tooltip tooltipColor">
                    <asp:Label ID="lbRecibo" runat="Server" Text='<%#Eval("GetReciboSinCero") %>' />
                </a>
            </td>
            <td align="left">
                <a href="#" alt="Saldo" class="tooltip tooltipColor">
                    <asp:Label ID="Label1" runat="Server" Text='<%#Eval("GetSaldo") %>' />
                </a>
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>
</div>
