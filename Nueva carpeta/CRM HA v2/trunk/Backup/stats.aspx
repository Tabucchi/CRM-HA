<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="stats.aspx.cs" Inherits="crm.stats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type='text/javascript' src='https://www.google.com/jsapi'></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section>
    <div>
        <h2>Estadísticas</h2>   
    </div>
    
    <div align="center" style="width:85%">
        <div style="float:left; padding-left:66px" >
            <div class="titulo1">
                <div class="lfloat" style="margin-top:18px">
                    <font style="font-family:Calibri; font-weight:bold; font-size:18px;">Distribución de trabajo</font>                                             
                </div>
            </div>
           <div id="pieChart" style="width: 400px; height: 280px; padding-top:5px"></div>
        </div>
        
        <div style="float:left; padding-left: 63px;">
            <div id="rankingChart" style="width: 326px; height: 240px; float:left">               
                <div class="titulo1">
                    <div class="lfloat" style="margin-top:18px">
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

        <div style="float:right; ">
            <div class="titulo1" style="width:150px">
                <div class="lfloat" style="margin-top:18px">
                    <font style="font-family:Calibri; font-weight:bold; font-size:18px;">Tickets abiertos</font>                                             
                </div>
            </div>
            <div id="chartClock" align="left" style="width: 100px; height: 80px; padding-top:10px;"></div>
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
</section>
</asp:Content>
