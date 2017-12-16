<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageCliente.master" AutoEventWireup="true" Inherits="EstadisticasClientes" Codebehind="EstadisticasClientes.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div style="font-size: xx-small">   
    <link href="Estilos/Tab.css" rel="stylesheet" type="text/css" />    
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    
    <div id="chartClock" style="width: auto; height: auto;"></div> 

    <%--<div class="pal grayArea uiBoxGray noborder">
            
        <div class="lfloat">
            <div style="height:40px;  border-bottom: 1px solid #E2E2E2;">
                <div class="lfloat" style="margin-top:18px">
                    <font style="font-family:Calibri; font-weight:bold; font-size:18px;">Tickets (Último mes)</font>
                </div>
            </div>
            <asp:Panel ID="pnlUltimoMes" runat="server" Visible="false" Height="25px">
                <asp:Label ID="lbMensajeUltimoMes" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
            </asp:Panel>
            
            <asp:Chart ID="PieChart" runat="server" BackColor="242, 242, 242" Height="190px" Width="439px">
                <Legends>
                    <asp:Legend BackColor="242, 242, 242" Name="Legend1" 
                        TitleFont="Microsoft Sans Serif, 11pt, style=Bold">
                        <Position Height="100" Width="50" X="53" Y="3" />
                    </asp:Legend>
                </Legends>
                <Series>
                    <asp:Series Name="Series1" ChartType="Pie" Legend="Legend1">
                    </asp:Series>
                </Series>
                <MapAreas>
                    <asp:MapArea Coordinates="0,0,0,0" Shape="Circle" />
                </MapAreas>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BackColor="242, 242, 242">
                        <Area3DStyle Enable3D="True" Inclination="60" />
                        <Position Height="100" Width="60" />
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </div>
            
        <div class="rfloat" style="font-size:12px; width:400px">
            <div style="height:40px;  border-bottom: 1px solid #E2E2E2;">
                <div class="lfloat" style="margin-top:18px">
                    <font style="font-family:Calibri; font-weight:bold; font-size:18px;">Top 5 (Último Trimestre)</font>                      
                </div>
            </div>
            <div class="lfloat" align="left">
                <asp:Repeater ID="rptRanking" runat="server" >
                    <HeaderTemplate><br /></HeaderTemplate>
                    <ItemTemplate>
                            <ItemTemplate>
                                    <%#Container.ItemIndex + 1 %>.&nbsp;
                                    <b><%#cCliente.Load(Convert.ToString(DataBinder.Eval(Container.DataItem, "idCliente"))).Nombre %> </b>
                                    &nbsp;&nbsp;<i><%#DataBinder.Eval(Container.DataItem, "Cant")%> Tickets</i>                                 
                            </ItemTemplate>
                    </ItemTemplate>

                    <FooterTemplate></ul></FooterTemplate>
                    <SeparatorTemplate>
                        <hr />
                    </SeparatorTemplate>
                </asp:Repeater>
            </div>
        </div> 
    
        <div style="width:100%; height:440px; padding: 0px 0px 0px 0px; clear: both; text-align:left;">
        
        <hr />        

        <table width="100%">
            <tr align="right">
                <td style="padding: 0 5px 0 5px">USUARIO</td>
                <td align="left">
                    <asp:DropDownList ID="cbCliente" runat="server" Width="200px" 
                        AutoPostBack="True"></asp:DropDownList>
                </td>
                <td><asp:RadioButtonList ID="rbMeses" runat="server" RepeatDirection="Horizontal" 
                        onselectedindexchanged="rbMeses_SelectedIndexChanged" AutoPostBack="True" 
                        Font-Bold="True" Width="100%">
                    <asp:ListItem>ÚLTIMO MES</asp:ListItem>
                    <asp:ListItem>TRIMESTRE</asp:ListItem>
                    <asp:ListItem Selected="True">SEMESTRE</asp:ListItem>
                    <asp:ListItem>ÚLTIMO AÑO</asp:ListItem>
                    <asp:ListItem>HISTORICO</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>        

        <ajax:TabContainer ID="TabContainer1" runat="server"
            Height="370px"
            Width="100%"
            ActiveTabIndex="0"       
            OnDemand="true"     
            TabStripPlacement="Top"
            ScrollBars="None"
            UseVerticalStripPlacement="false"
            VerticalStripWidth="120px"
            Enabled="true" class="ajax__tab_xp ajax__tab_body" CssClass="" 
                AutoPostBack="FALSE" >
            
            <ajax:TabPanel ID="TabPanel1" runat="server" 
                    HeaderText="Tickets por meses"
                    ScrollBars="Auto"        
                    OnDemandMode="Once">
                <HeaderTemplate>Tickets por meses</HeaderTemplate>
                
                    <ContentTemplate>
                        <div></div>
                        <div style="width:100%;"><hr /></div>

                        <div align="center">
                            <asp:Chart ID="ChartCliente" runat="server" BackColor="242, 242, 242" style="margin-top: 0px" Width="800px">
                                <Series><asp:Series ChartArea="ChartArea1" Name="Series1"></asp:Series></Series>
                                <ChartAreas>
                                    <asp:ChartArea BackColor="242, 242, 242" Name="ChartArea1" ShadowColor="242, 242, 242">
                                    <Area3DStyle Enable3D="True" />
                                    <AxisY Title="Cantidad de Pedidos Ingresados" TitleForeColor="112, 111, 111"></AxisY>
                                    <AxisX IsLabelAutoFit="False"><LabelStyle Angle="-45" Interval="1" IntervalOffset="1" /></AxisX>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        
                            <div align="center" style="width:100%;">
                                <asp:Panel ID="pnlRegistros" runat="server" CssClass="Tab">
                                    <table>
                                        <tr>
                                            <td>Registros Encontrados: <b><asp:Label ID="lbCantRegistros" runat="server" /></td>
                                            <td style="width: 20px"></td><td>Promedio mensual: <b><asp:Label ID="lbPromedioMensual" runat="server" /></td>
                                        </tr>
                                     </table>
                                </asp:Panel>
                            </div>
                            <asp:Label ID="lbMensajeError" runat="server" Font-Bold="True" Font-Size="Small" 
                                                    Visible="false"></asp:Label>
                        </div>
                                                
                    </ContentTemplate>
            
</ajax:TabPanel>
            <ajax:TabPanel ID="TabPanel2" runat="server" HeaderText="Tickets por Estado">
                <HeaderTemplate>Tickets por Estado</HeaderTemplate>
                

<ContentTemplate><div style="width:100%;"><hr /></div><div align="center">
<asp:Chart ID="ChartEstado" runat="server" BackColor="242, 242, 242" style="margin-top: 0px" Width="800px"><Series>
    <asp:Series ChartArea="ChartArea1" Name="Series1" ChartType="StackedColumn"></asp:Series>
    <asp:Series ChartArea="ChartArea1" Name="Series2" ChartType="StackedColumn">
    </asp:Series>
    </Series><ChartAreas><asp:ChartArea BackColor="242, 242, 242" Name="ChartArea1" ShadowColor="252, 172, 55"><AxisY Title="Cantidad de Pedidos Ingresados" TitleForeColor="112, 111, 111"></AxisY><AxisX IsLabelAutoFit="False"><LabelStyle Angle="-45" Interval="1" IntervalOffset="1" /></AxisX><Area3DStyle Enable3D="True" /></asp:ChartArea></ChartAreas></asp:Chart>
    
    <div style="padding: 0px 0 0px 0; font-size: xx-small;">
        <asp:Panel ID="pnlRefenciaEstados" runat="server">
        <table>
            <tr>
                <td style="background-color: #FCAC37" width="20px"></td>
                <td>
                    Tickets Nuevos
                </td>
                <td style="width: 20px"></td>
                <td style="background-color: #3782EE" width="20px"></td>
                <td>
                    Tickets Finalizados
                </td>
            </tr>
            <tr><td colspan="4" style="height: 1px"></td></tr>
        </table>
        </asp:Panel>
        
        <asp:Panel ID="pnlEstado" runat="server" CssClass="Tab">
            <table>
                <tr>
                    <td>Registros Encontrados: <b><asp:Label ID="lbCantRegistrosEstado" runat="server"></asp:Label></b></td>
                    <td style="width: 20px"></td>
                    <td>Tickets Nuevos: <b><asp:Label ID="lbTicketsNuevos" runat="server" /></td>
                    <td style="width: 20px"></td>
                    <td>Tickets Finalizados: <b><asp:Label ID="lbTicketsFinalizados" runat="server" /></td>
                </tr>
            </table>
         </asp:Panel>
         <asp:Label ID="lbMensajeErrorEstado" runat="server" Font-Bold="True" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    
    </div>
        </ContentTemplate>
            

</ajax:TabPanel>
            <ajax:TabPanel ID="TabPanel3" runat="server" HeaderText="Tickets por Prioridad">
                <ContentTemplate><div style="width:100%;"><hr /></div><div align="center"><asp:Chart ID="ChartPrioridad" runat="server" BackColor="242, 242, 242" 
                                style="margin-top: 0px" Width="800px"><Series>
                        <asp:Series ChartArea="ChartArea1" Name="Series1" ChartType="StackedColumn"></asp:Series>
                        <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" Name="Series2">
                        </asp:Series>
                        <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" Name="Series3">
                        </asp:Series>
                        <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" Name="Series4">
                        </asp:Series>
                        <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" Name="Series5">
                        </asp:Series>
                    </Series><ChartAreas><asp:ChartArea BackColor="242, 242, 242" Name="ChartArea1" 
                                                    ShadowColor="242, 242, 242"><AxisY Title="Cantidad de Pedidos Ingresados" TitleForeColor="112, 111, 111"></AxisY><AxisX IsLabelAutoFit="False"><LabelStyle Angle="-45" Interval="1" IntervalOffset="1" ></LabelStyle></AxisX><Area3DStyle Enable3D="True" ></area3dstyle></asp:ChartArea></ChartAreas></asp:Chart>
                                                    
                    <div style="padding: 0px 0 0px 0; font-size: xx-small;">
                        <asp:Panel ID="pnlRefenciaPrioridades" runat="server">
                        <table>
                            <tr>
                                <td style="background-color: #3782EE" width="20px"></td>
                                <td>
                                    Tickets Sin Urgencia
                                </td>
                                <td style="background-color: #FCAC37" width="20px"></td>
                                <td>
                                    Tickets Inmediatos
                                </td>
                                <td style="background-color: #FF7445" width="20px"></td>
                                <td>
                                    Tickets 24 hs.
                                </td>
                                <td style="background-color: #810081" width="20px"></td>
                                <td>
                                    Tickets 48 hs.
                                </td>
                                <td style="background-color: #CECECE" width="20px"></td>
                                <td>
                                    Tickets Próxima Visita
                                </td>
                            </tr>
                            <tr><td colspan="10" style="height: 1px"></td></tr>
                        </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlPrioridad" runat="server" CssClass="Tab">
                       <table>
                            <tr>
                                <td>Registros Encontrados: <b><asp:Label ID="lbCantRegistrosPrioridad" runat="server" /></td>
                                <td style="width: 10px"></td>
                                <td>Sin Urgencia: <b><asp:Label ID="lbSinUrgencia" runat="server" /></td>
                                <td style="width: 10px"></td>
                                <td>Inmediatos: <b><asp:Label ID="lbInmediato" runat="server" /></td>
                                <td style="width: 10px"></td>
                                <td>24 hs: <b><asp:Label ID="lb24hs" runat="server" /></td>
                                <td style="width: 10px"></td>
                                <td>48 hs: <b><asp:Label ID="lb48hs" runat="server" /></td>
                                <td style="width: 10px"></td>
                                <td>Próxima Visita: <b><asp:Label ID="lbProximaVisita" runat="server" /></td>
                            </tr>
                       </table>
                       </asp:Panel>                            
                                                <asp:Label ID="lbMensajeErrorPrioridad" runat="server" Font-Bold="True" Font-Size="Small" 
                                        Visible="False"></asp:Label>    
                                                    </div>
                                                    </div>
                                                    
                       </ContentTemplate>
            


</ajax:TabPanel>
            
            <ajax:TabPanel ID="TabPanel4" runat="server" HeaderText="Total Pedidos de Cliente" Width="600px">
                <ContentTemplate>
                
                <div style="width:100%;"><hr /></div>
                <div align="center">
                <asp:Chart ID="ChartTotalPedidosCliente" runat="server" BackColor="242, 242, 242" Width="800px"><Series><asp:Series Name="Series1" ChartArea="ChartArea1"></asp:Series></Series><ChartAreas><asp:ChartArea Name="ChartArea1" BackColor="242, 242, 242" 
                                    ShadowColor="242, 242, 242"><AxisY Title="Cantidad de Tickets Ingresados" TitleForeColor="112, 111, 111"></AxisY><AxisX IsLabelAutoFit="False"><LabelStyle Interval="1" IntervalOffset="1" Angle="-45" /></AxisX><Area3DStyle Enable3D="True" /></asp:ChartArea></ChartAreas></asp:Chart>
                                    
                                    <div align="center" style="width:100%;">
                                        <asp:Panel ID="pnlCantPedidosClientes" runat="server" CssClass="Tab">
                                            <table><tr><td>Registros Encontrados: <b><asp:Label ID="lbCantRegistros_CantPedidosClientes" runat="server" /></td></tr></table>
                                        </asp:Panel>
                                        <asp:Label ID="lbMensajeError_CantCliente" runat="server" Font-Bold="True" Font-Size="Small" 
                                            Visible="False"></asp:Label>
                                    </div>
                                    </div>
                                    
                                    </ContentTemplate>
            


</ajax:TabPanel>
        </ajax:TabContainer>
        </div>
    </div>--%>
</div>
</asp:Content>

