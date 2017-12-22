<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="stats.aspx.cs" Inherits="crm.stats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type='text/javascript' src='https://www.google.com/jsapi'></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section>
    <div>
        <h2>Estadísticas</h2>   
    </div>
        
    <div style="width:100%">
        <div align="center" style="padding-top: 10px;">
            <div class="Etiquetas" style="float:left; padding-left:66px">
                <div class="lfloat EtiquetasBody EtiquetasFinance">
                    <h5 style="text-align: right;font-size: 19px;letter-spacing: 0;font-weight: 300;padding-right: 25px;">Total a cobrar</h5>  
                    $<span style="padding-top: 25px;text-align: right;font-size: 34px;line-height: 36px;letter-spacing: -1px;margin-bottom: 0;font-weight: 300;padding-right: 25px;" data-counter="counterup" data-value="12,5">
                        <asp:Label ID="lbTotalCobrar" runat="server"></asp:Label>
                    </span>                                                             
                </div>
            </div>
        </div>

        <div id="StackedUnidades"></div>
    </div>
    <hr />
    <div style="width:100%">
    <div align="center" style="padding-top: 10px;">             
        <div style="width: 326px; height: 240px; float:left; padding-left:70px">
            <div aling="center" class="titulo1">
                <div>
                    <font style="font-family:Calibri; font-weight:bold; font-size:18px;">Total a cobrar por Proyecto</font>                                             
                </div>
            </div>
            <div style="float:left; padding-left:66px" >
                <asp:Repeater ID="rptTotalACobrarProyecto" runat="server" >
                    <ItemTemplate>
                        <%#Container.ItemIndex + 1 %>.&nbsp;
                        <b><%# DataBinder.Eval(Container.DataItem, "nombreProyecto") %> </b>
                        &nbsp;&nbsp;$ <i><%#DataBinder.Eval(Container.DataItem, "cant")%></i>
                    </ItemTemplate>
                    <FooterTemplate> </ul> </FooterTemplate>
                    <SeparatorTemplate> <hr /> </SeparatorTemplate>
                </asp:Repeater>
             </div>
        </div>
        <div style="width: 280px; height: 240px; float:right; margin-right: 50px;">
            <div aling="center" class="titulo1">
                <div>
                    <font style="font-family:Calibri; font-weight:bold; font-size:18px;">Total a cobrar por Empresa</font>                                             
                </div>
            </div>
            <div style="float:left; padding-left: 42px;" >
                <asp:Repeater ID="rptTotalACobrarCliente" runat="server" >
                    <ItemTemplate>
                        <%#Container.ItemIndex + 1 %>.&nbsp;
                        <b><%# DataBinder.Eval(Container.DataItem, "nombreEmpresa") %> </b>
                        &nbsp;&nbsp;$ <i><%#DataBinder.Eval(Container.DataItem, "cant")%></i>
                    </ItemTemplate>
                    <FooterTemplate> </ul> </FooterTemplate>
                    <SeparatorTemplate> <hr /> </SeparatorTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>

    <div align="center" style="padding-top:25%;">
        <div style="float:left;" >
            <div class="titulo1">
                <div class="lfloat">
                    <font style="font-family:Calibri; font-weight:bold; font-size:18px;">Distribución de trabajo</font>                                             
                </div>
            </div>
           <div id="pieChart" style="width: 400px; height: 280px; padding-top:5px"></div>
        </div>        
        <div style="float:left; padding-left: 55px;">
            <div id="rankingChart" style="width: 326px; height: 240px; float:left">               
                <div class="titulo1">
                    <div class="lfloat">
                        <font style="font-family:Calibri; font-weight:bold; font-size:18px;">Top 5 (Último Trimestre)</font>                                             
                    </div>
                </div>
                <div class="lfloat" align="left">
                    <asp:Repeater ID="rptRanking" runat="server" >
                        <HeaderTemplate><br /></HeaderTemplate>
                        <ItemTemplate>
                                        <%#Container.ItemIndex + 1 %>.&nbsp;
                                        <b><%# DataBinder.Eval(Container.DataItem, "nombreEmpresa") %> </b>
                                        &nbsp;&nbsp;<i><%#DataBinder.Eval(Container.DataItem, "Cant")%>Tickets</i>
                        </ItemTemplate>

                        <FooterTemplate> </ul> </FooterTemplate>
                        <SeparatorTemplate> <hr /> </SeparatorTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
        <div style="float:right;">
            <div class="Etiquetas" style="float:left">
                <div class="lfloat EtiquetasBody EtiquetasTicket">
                    <h5 style="text-align: right;font-size: 19px;letter-spacing: 0;font-weight: 300;padding-right: 25px;">Tickets Abiertos</h5>  
                    <span style="padding-top: 25px;text-align: right;font-size: 34px;line-height: 36px;letter-spacing: -1px;margin-bottom: 0;font-weight: 300;padding-right: 25px;" data-counter="counterup" data-value="12,5">
                        <asp:Label ID="lbTicketAbiertos" runat="server"></asp:Label>
                    </span>                                                             
                </div>
            </div>  
        </div>
    </div>

    <div style="padding-top:28%;">
        <div class="titulo1" style="width:100%;">
            <div class="lfloat" style="margin-top:18px;">
                <font style="font-family:Calibri; font-weight:bold; font-size:18px;">Tickets por mes</font>                                             
            </div>
        </div>
        <div align="center" style="padding-top: 5px;">
        <asp:RadioButtonList ID="rbMeses" runat="server" RepeatDirection="Horizontal"  style="width:50%"
            AutoPostBack="True" onselectedindexchanged="rbMeses_SelectedIndexChanged">
            <asp:ListItem>SEMESTRE</asp:ListItem>
            <asp:ListItem Selected="True">ÚLTIMO AÑO</asp:ListItem>
            <asp:ListItem>HISTORICO</asp:ListItem>
        </asp:RadioButtonList>
        </div>
    </div>

    <div id="barChart" style="width: auto; height: 500px;"></div>

    <div align="center">
        <font style="font-family: Open Sans Condensed; font-size:22px; color: #666666"><b>Cantidad Total:</b>&nbsp;&nbsp;<asp:Label ID="lbCantRegistros" runat="server"></asp:Label></font> &nbsp&nbsp
        <font style="font-family: Open Sans Condensed; font-size:22px; color: #666666"><b>Promedio:</b>&nbsp;&nbsp;<asp:Label ID="lbPromedioMensaul" runat="server"></asp:Label></font>
    </div>
    </div>

</section>
</asp:Content>
