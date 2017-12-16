<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Unidad.aspx.cs" Inherits="crm.Unidad" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .valerror[style*="inline"]
        {
            display:block !Important;
            color: #a94442 !Important;
            width: 286px;
            padding-top: 5px;
            padding-bottom: 5px;
            padding-left: 5px;
        }
    </style>

    <script type="text/javascript">
        // simple redirect to your detail page, passing the selected ID 
        function redirAgenda(idEmpresa) {
            if (idEmpresa != "-")
                window.location.href = "Agenda.aspx?idEmpresa=" + idEmpresa;
        }
    </script>
    
    <script src="js/jquery.mask.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.decimal').mask("#.##0,00", { reverse: true });
        });

        $(document).ready(function () {
            $('.entero').mask("#.##0", { reverse: true });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>            
    <asp:HiddenField ID="hfSupTotalProyecto" runat="server" />   
    <asp:HiddenField ID="hfMoneda" runat="server" /> 
        
    <section>        
        <div class="formHolder" id="searchBoxTop">
            <div class="headOptions headLine">
                <a href="#" alt="Filtro" class="toggleFiltr" style="margin-top: 9px; margin-right:5px">v</a>
                <h2><asp:Label ID="lbProyecto" runat="server"></asp:Label></h2>              
                
                <div style="float: right; margin-top: 3px;">
                    <label class="col2" style="width:405px">
                        <asp:Label ID="lbCantUnidades" runat="server" style="width: 30% !important;"></asp:Label>
                        <asp:DropDownList ID="cbFiltro" runat="server" style="width:246px" AutoPostBack="True" OnSelectedIndexChanged="cbFiltro_SelectedIndexChanged">
                            <asp:ListItem Text="Unidad" Value="0" />
                            <asp:ListItem Text="Menor por m²" Value="1" />
                            <asp:ListItem Text="Mayor por m²" Value="2" />
                            <asp:ListItem Text="Menor precio" Value="3" />
                            <asp:ListItem Text="Mayor precio" Value="4" />                            
                        </asp:DropDownList>
                    </label>
                </div>
            </div>

            <div class="hideOpt" style="width: 100%;">
                <div class="formHolder" style="padding: 22px 25px 12px; box-shadow: inherit;">
                    <div class="divFormInLine" style="width: 350px !important;">
                        <div>
                            <label class="col3" style="width: 90% !important;">
                                <span style="width: 30% !important;">ESTADO</span>
                                <asp:DropDownList ID="cbFiltroEstado" runat="server" style="width: 200px;"><asp:ListItem Text="Todos" Value="0" /></asp:DropDownList>
                            </label>
                        </div>
                    </div>

                    <div class="divFormInLine" style="width: 350px !important;">
                        <div>
                            <label class="col3" style="width: 90% !important;">
                                <span style="width: 30% !important;">TIPO UNIDAD</span>
                                <asp:DropDownList ID="cbFiltroUnidad" runat="server" style="width: 200px;"/>
                            </label>
                        </div>
                    </div>

                    <div class="divFormInLine" style="width: 350px !important;">
                        <div>   
                            <label class="col3" style="width: 90% !important;">
                                <span style="width: 30% !important;">AMBIENTE</span>
                                <asp:DropDownList ID="cbFiltroAmbiente" runat="server" style="width: 200px;"/>
                            </label>
                        </div>
                    </div>
                </div>

                <div class="formHolder" style="padding: 22px 25px 12px; box-shadow: inherit;">
                    <div class="divFormInLine" style="width: 350px !important;">
                        <div>
                            <label class="col3" style="width: 90% !important;">
                                <span style="width: 30% !important;">SUPERFICIE</span>
                                <label style="line-height: 14px;">
                                <asp:DropDownList ID="cbFiltroSup" runat="server" style="width: 200px;">
                                    <asp:ListItem Value="supTotal">Total</asp:ListItem>
                                    <asp:ListItem Value="supCubierta">Cubierta</asp:ListItem>
                                    <asp:ListItem Value="supSemiDescubierta">Balcon</asp:ListItem>
                                    <asp:ListItem Value="SupDescubierta">Terraza</asp:ListItem>
                                </asp:DropDownList>
                                </label>
                            </label>
                        </div>
                    </div>

                    <div class="divFormInLine" style="width: 350px !important;">
                        <div>
                            <label class="col3" style="width: 90% !important;">
                                <span style="width: 30% !important;">M²</span>
                                <label style="line-height: 14px;">
                                    <asp:TextBox ID="txtMinM2" runat="server" CssClass="textBox textBoxForm decimal" placeholder="Min." style="width: 186px;"></asp:TextBox>
                                    <asp:TextBox ID="txtMaxM2" runat="server" CssClass="textBox textBoxForm decimal" placeholder="Max." style="width: 186px;"></asp:TextBox>
                                </label>
                            </label>
                        </div>
                    </div>
                
                    <div class="divFormInLine" style="width: 350px !important;">
                        <div>
                            <label class="col3" style="width: 90% !important;">
                                <span style="width: 30% !important;">PRECIO</span>
                                <label style="line-height: 14px;">
                                    <asp:TextBox ID="txtMinPrecio" runat="server" CssClass="textBox textBoxForm entero" style="width: 186px;" placeholder="Min."></asp:TextBox>
                                    <asp:TextBox ID="txtMaxPrecio" runat="server" CssClass="textBox textBoxForm entero" style="width: 186px;" placeholder="Max."></asp:TextBox>
                                </label>
                            </label> 
                        </div>
                    </div>
                </div>   
            </div>       
                                 
            <div class="hideOpt footerLine" style="width: 100%;">
                <div class="divFormInLine" style="width: 580px !important;">
                    <div>
                        <asp:Panel ID="pnlAlta" runat="server">
                            <label class="leftLabel" style="width: 42%; margin-left: -3px;">
                                <b><asp:LinkButton ID="btnNuevaUnidad" runat="server" CssClass="formBtnGrey1" Text="Alta / Modificación de unidades" OnClick="btnNuevaUnidad_Click"></asp:LinkButton></b>
                            </label>
                        </asp:Panel>
                        <asp:Panel ID="pnlPrecio" runat="server">
                            <label style="width: 26%; margin-left: -12px;">
                                <b><asp:LinkButton ID="btnActualizarPrecio" runat="server" CssClass="formBtnGrey1" style="margin-left:15px;" Text="Actualizar precios" OnClick="btnActualizarPrecio_Click"></asp:LinkButton></b>
                            </label>
                        </asp:Panel>
                        <label style="width: 26%; margin-left: -56px;">
                            <asp:LinkButton ID="btnExportar" runat="server"  CssClass="formBtnNar" style="margin-left:15px;" Text="Imprimir" OnClick="btnExportar_Click"></asp:LinkButton>
                        </label>
                    </div>
                </div>
                   
                <div class="divFormInLine" style="width: 350px !important;">
                    <div>
                        <label align="center" class="col3 borderSeparator" style="width: 285px; margin-left: 22px;">
                            <div style="width: 380px; margin-left: 15px;">
                                <span style="width: 30%;">Moneda:</span>
                                <asp:Label id="lbMoneda" runat="server" style="width: 30%; background-color: white; border: 1px dotted rgba(0, 0, 0, 0.1);"></asp:Label>
                            </div>
                        </label>
                    </div>
                </div>

                <div class="divFormInLine" style="width: 218px !important;">
                    <div>
                        <label class="rigthLabel" style="width: 37%;">
                            <asp:Button ID="btnBuscar" Text="Buscar" CssClass="formBtnNar" runat="server" OnClick="btnBuscar_Click" />
                        </label>
                        <label class="rigthLabel" style="width: 17%;">
                            <asp:Button ID="btnVerTodos" Text="Ver Todos" CssClass="formBtnGrey1" runat="server" style="float:right; margin-left:15px;" Onclick="btnVerTodos_Click"/>
                        </label>
                    </div>
                </div>
            </div>  
        </div>                          
    </section> 
            
    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <section>
            <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px;">
                <div>No se pueden ingresar nuevas unidades, dado que superarían los metros totales válidos.</div>
            </div>
        </section>
        <section>
            <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px;">
                <div>
                    En caso que algunas de las unidades fueron modificadas ingresar  
                    <b><asp:LinkButton ID="lkbCargaModificacion" runat="server" OnClick="lkbCargaModificacion_Click" CssClass="link"> aquí</asp:LinkButton></b>
                </div>
            </div>
        </section>
    </asp:Panel>

    <asp:Panel ID="pnlMensajePrecios" runat="server" Visible="false">
        <section>
            <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px;">
                <div>Los precios de las unidades disponibles fueron actualizados, y se encuentran a la espera de ser confirmados por un administrador</div>
            </div>
        </section>
    </asp:Panel>
    
    <asp:ListView ID="lvUnidades" runat="server" OnItemDataBound="lvUnidades_ItemDataBound" OnItemCommand="lvUnidades_ItemCommand" OnLayoutCreated="lvUnidades_LayoutCreated">
        <LayoutTemplate>
            <section>
                <%--<table style="width:100%">                            
                    <thead id="tableHead">
                        <tr>     
                            <td style="width: 1%; text-align: center">Cod U.F.</td>
                            <td style="width: 6%; text-align: center">Tipo Unidad</td>
                            <td style="width: 10%; text-align: center">Nivel</td>
                            <td style="width: 4%; text-align: center">Nro. Unidad</td>
                            <td style="width: 6%; text-align: center">Amb</td>
                            <td style="width: 9%; text-align: center">Sup. Cubierta</td>
                            <td style="width: 4%; text-align: center">Balcon</td>
                            <td style="width: 4%; text-align: center">Terraza</td>
                            <td style="width: 9%; text-align: center">Sup. Total</td>
                            <td style="width: 5%; text-align: center">%</td>
                            <td style="width: 8%; text-align: center">Valor</td>
                            <td style="width: 6%; text-align: center">Estado</td>
                            <td style="width: 16%; text-align: center">Cliente</td>
                            <td style="width: 1%;"></td>
                            <td style="width: 1%;"></td>
                        </tr>
                    </thead>
                </table>
                <div class="tableBody">
                    <table style="width:100%">
                        <tbody>
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </tbody>    
                    </table>
                </div>
                <div class="tableFoot">
                    <table style="width:100%">
                        <tfoot class="footerTable">
                            <tr>
                                <td style="width: 1%; text-align: center"></td>
                            <td style="width: 6%; text-align: center"></td>
                            <td style="width: 10%; text-align: center"></td>
                            <td style="width: 4%; text-align: center"></td>
                            <td style="width: 6%; text-align: center"></td>
                            <td style="width: 9%; text-align: center"><a href="#" alt="Total Sup. Cubierta (m²)" class="tooltipTop tooltipColor" style="margin-left: -10px;"><b><asp:Label ID="lbSupCubierta" runat="server"></asp:Label></b></a></td>
                            <td style="width: 4%; text-align: center"><a href="#" alt="Total Sup. Balcon (m²)" class="tooltipTop tooltipColor" style="margin-left: -10px;"><b><asp:Label ID="lbSupBalcon" runat="server"></asp:Label></b></a></td>
                            <td style="width: 4%; text-align: center"><a href="#" alt="Total Sup. Terraza (m²)" class="tooltipTop tooltipColor" style="margin-left: -10px;"><b><asp:Label ID="lbSupTerraza" runat="server"></asp:Label></b></a></td>
                            <td style="width: 9%; text-align: center"><a href="#" alt="Total Sup. Total (m²)" class="tooltipTop tooltipColor" style="margin-left: -8px;"><b><asp:Label ID="lbSupTotal" runat="server"></asp:Label></b></a></td>
                            <td style="width: 5%; text-align: center"><a href="#" alt="Total Porcentaje (%)" class="tooltipTop tooltipColor" style="margin-left: -10px;"><b><asp:Label ID="lbPorcentaje" runat="server"></asp:Label></b></a></td>
                            <td style="width: 8%; text-align: right"><a href="#" alt="Total Valor" class="tooltipTop tooltipColor" style="margin-left: -6px;"><b><asp:Label ID="lbPrecioBase" runat="server"></asp:Label></b></a></td>
                            <td style="width: 6%; text-align: center"></td>
                            <td style="width: 16%; text-align: center"></td>
                            <td style="width: 1%; text-align: center"></td>
                            <td style="width: 1%; text-align: center"></td>
                            </tr>
                        </tfoot>                   
                    </table>
                </div>--%>




                <table style="margin-top:-25px">
                    <thead id="tableHead">
                       <tr>
                            <td style="width: 1%; text-align: center">Cod U.F.</td>
                            <td style="width: 6%; text-align: center">Tipo Unidad</td>
                            <td style="width: 10%; text-align: center">Nivel</td>
                            <td style="width: 4%; text-align: center">Nro. Unidad</td>
                            <td style="width: 6%; text-align: center">Amb</td>
                            <td style="width: 9%; text-align: center">Sup. Cubierta</td>
                            <td style="width: 4%; text-align: center">Balcon</td>
                            <td style="width: 4%; text-align: center">Terraza</td>
                            <td style="width: 9%; text-align: center">Sup. Total</td>
                            <td style="width: 5%; text-align: center">%</td>
                            <td style="width: 8%; text-align: center">Valor&nbsp;(<asp:Label ID="lbMoneda" runat="server"/>)</td>
                            <td style="width: 6%; text-align: center">Estado</td>
                            <td style="width: 16%; text-align: center">Cliente</td>
                            <td style="width: 1%;"></td>
                        </tr>
                    </thead>
                    <tbody style="height:80px; overflow:scroll">
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </tbody>
                    <tfoot class="footerTable">
                        <tr>
                            <td style="width: 1%; text-align: center"></td>
                            <td style="width: 6%; text-align: center"></td>
                            <td style="width: 10%; text-align: center"></td>
                            <td style="width: 4%; text-align: center"></td>
                            <td style="width: 6%; text-align: center"></td>
                            <td style="width: 9%; text-align: center"><a href="#" alt="Total Sup. Cubierta (m²)" class="tooltipTop tooltipColor" style="margin-left: -10px;"><b><asp:Label ID="lbTotalSupCubierta" runat="server"></asp:Label></b></a></td>
                            <td style="width: 4%; text-align: center"><a href="#" alt="Total Sup. Balcon (m²)" class="tooltipTop tooltipColor" style="margin-left: -10px;"><b><asp:Label ID="lbTotalSupBalcon" runat="server"></asp:Label></b></a></td>
                            <td style="width: 4%; text-align: center"><a href="#" alt="Total Sup. Terraza (m²)" class="tooltipTop tooltipColor" style="margin-left: -10px;"><b><asp:Label ID="lbTotalSupTerraza" runat="server"></asp:Label></b></a></td>
                            <td style="width: 9%; text-align: center"><a href="#" alt="Total Sup. Total (m²)" class="tooltipTop tooltipColor" style="margin-left: -8px;"><b><asp:Label ID="lbTotalSupTotal" runat="server"></asp:Label></b></a></td>
                            <td style="width: 5%; text-align: center"><a href="#" alt="Total Porcentaje (%)" class="tooltipTop tooltipColor" style="margin-left: -10px;"><b><asp:Label ID="lbTotalPorcentaje" runat="server"></asp:Label></b></a></td>
                            <td style="width: 8%; text-align: right"><a href="#" alt="Total Valor" class="tooltipTop tooltipColor" style="margin-left: -6px;"><b><asp:Label ID="lbTotalPrecioBase" runat="server"></asp:Label></b></a></td>
                            <td style="width: 6%; text-align: center"></td>
                            <td style="width: 16%; text-align: center"></td>
                            <td style="width: 1%; text-align: center"></td>
                        </tr>
                    </tfoot>
                </table>
            </section>
        </LayoutTemplate>
        <ItemTemplate>
            <tr onclick='Visible(<%# Eval("id")%> )' style="cursor: pointer;">
                <td style="text-align: center; width: 1%;">
                    <asp:Label ID="Label11" runat="Server" Text='<%#Eval("CodigoUF", "{0:#,#.00}") %>'/>
                </td> 
                <td style="text-align: center; width: 6%;">
                    <asp:Label ID="Label3" runat="Server" Text='<%#Eval("UnidadFuncional") %>'/>
                </td>                
                <td style="text-align: center; width: 10%;">
                    <asp:Label ID="Label1" runat="Server" Text='<%#Eval("Nivel") %>' ToolTip="Nivel"/>
                </td>
                <td style="text-align: center; width: 4%;">
                    <asp:Label ID="Label2" runat="Server" Text='<%#Eval("NroUnidad") %>' ToolTip="Nro. Unidad"/>
                </td>
                <td style="text-align: center; width: 6%;">
                    <asp:Label ID="Label12" runat="Server" Text='<%#Eval("Ambiente") %>' ToolTip="Nro. Unidad"/>
                </td>
                <td style="text-align: center; width: 9%;">
                    <asp:Label ID="lbSupCubierta" runat="Server" Text='<%#Eval("GetSupCubierta", "{0:0.##}") %>' ToolTip="Sup. Cubierta" />
                </td>
                <td style="text-align: center; width: 4%;">
                    <asp:Label ID="lbSupSemiDescubierta" runat="Server" Text='<%#Eval("GetSupSemiDescubierta", "{0:0.##}") %>' ToolTip="Sup. Descubierta" />
                </td>
                <td style="text-align: center; width: 4%;">
                    <asp:Label ID="lbSupDescubierta" runat="Server" Text='<%#Eval("GetSupDescubierta", "{0:0.##}") %>' ToolTip="Sup. Descubierta" />
                </td>
                <td style="text-align: center; width: 9%;">
                    <asp:Label ID="lbSupTotal" runat="Server" Text='<%#Eval("SupTotal") %>' ToolTip="Sup. Total"/>
                </td>
                <td style="text-align: center; width: 5%;">
                    <asp:Label ID="lbPorcentaje" runat="Server" Text='<%#Eval("Porcentaje") %>' ToolTip="Porcentaje" />
                </td>
                <td style="text-align: right; width: 8%;">
                    <asp:Label ID="lbPrecioBase" runat="Server" Text='<%#Eval("GetPrecioBase") %>'/>
                </td>
                <td style="text-align: center; width: 6%;">
                    <asp:Label ID="Label7" runat="Server" Text='<%#Eval("GetEstado") %>' ToolTip="Estado"/>
                </td>
                <td style="text-align: center; width: 16%;" onclick="redirAgenda('<%# Eval("idEmpresa") %>');">
                    <asp:Label ID="Label10" runat="Server" Text='<%#Eval("GetEmpresa") %>' ToolTip="Estado"/>
                </td>
                <td style="width: 1%;">
                    <asp:LinkButton ID="btnEditar" runat="server" CssClass="editBtn" CommandName="Editar" Text="Editar" ToolTip="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'/>                   
                </td>                   
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <section>
                <table style="width:100%" runat="server">
                    <tr>
                        <td style="text-align:center"><b>No se encontraron unidades registradas.<b/></td>
                    </tr>
                </table>
            </section>
        </EmptyDataTemplate>
    </asp:ListView>
     
    <section>
        <div runat="server" class="formHolderAlert alertGreyLighten" style="padding: 14px 25px 12px; margin-top: -26px;">
            <h4>Última actualización:</h4>
            <div>
                Fecha: &nbsp;<b><asp:Label ID="lbHistorialFecha" runat="server" /></b><br />
                Motivo: &nbsp;<b><asp:Label ID="lbHistorialMotivo" runat="server" /></b><br />
            </div>
            <div style="padding-top: 8px;">
                Si desea ver el historial completo de las actualizaciones ingrese <b><a href="Historial.aspx" class="link">aquí</a></b>
            </div>
        </div>
    </section>
 
    <section>
        <asp:UpdatePanel ID="pnlEdit" runat="server" Width="410px" HorizontalAlign="Center" class="modal">
            <ContentTemplate>
                <table width="100%">               
                    <tr>
                        <td colspan="4"><modalTitle><b>Editar Unidad</b></modalTitle></td>
                    </tr> 
                    <tr>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                            Código U.F.: 
                        </td>
                        <td>
                            <asp:Label ID="txtEditCodUF" runat="server" style="margin-left: 4px; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; font-weight: bold; color: #666;"></asp:Label>
                        </td>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                            Tipo de Unidad: 
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEditUnidadFuncional" runat="server" CssClass="dropDownList dropDownListForm" style="position:relative; width: 235px;"/>
                            <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                                ControlToValidate="ddlEditUnidadFuncional" InitialValue="0" ErrorMessage="Seleccione una tipo de unidad"
                                Validationgroup="EditGroup" Display="Dynamic" SetFocusOnError="True" CssClass="alert valerror borderValidator">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr> 
                    <tr>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                            Nivel: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtEditNivel" runat="server" CssClass="textBoxForm" style="position:relative; width: 220px;"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server"
                                ControlToValidate="txtEditNivel"
                                ErrorMessage="Ingrese el nivel"
                                Validationgroup="EditGroup" Display="Dynamic" SetFocusOnError="True" CssClass="alert valerror borderValidator"/>
                        </td>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                            Nro. Unidad: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtEditNroUnidad" runat="server" CssClass="textBoxForm" style="position:relative; width: 220px;"></asp:TextBox>
                            <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server"
                                ControlToValidate="txtEditNroUnidad"
                                ErrorMessage="Ingrese el nro. Unidad"
                                Validationgroup="EditGroup" Display="Dynamic" SetFocusOnError="True" CssClass="alert valerror borderValidator"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                            Ambiente: 
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtEditAmbiente" runat="server" CssClass="textBoxForm" style="position:relative; width: 220px;"></asp:TextBox>
                            <asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server"
                                ControlToValidate="txtEditAmbiente"
                                ErrorMessage="Ingrese el ambiente"
                                Validationgroup="EditGroup" Display="Dynamic" SetFocusOnError="True" CssClass="alert valerror borderValidator"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                            Sup. Cubierta: 
                        </td>
                        <td>
                            <asp:Label ID="lbEditSupCubierta" runat="server" style="margin-left: 4px; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; font-weight: bold; color: #666;"></asp:Label>
                        </td>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                            Balcón: 
                        </td>
                        <td>
                            <asp:Label ID="lbEditSupSemiDescubierta" runat="server" style="margin-left: 4px; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; font-weight: bold; color: #666;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                            Terraza: 
                        </td>
                        <td>
                            <asp:Label ID="lbEditSupDescubierta" runat="server" style="margin-left: 4px; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; font-weight: bold; color: #666;"></asp:Label>
                        </td>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                            Sup. Total: 
                        </td>
                        <td>
                            <asp:Label ID="lbEditSupTotal" runat="server" style="margin-left: 4px; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; font-weight: bold; color: #666;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                            Tipo de moneda: 
                        </td>
                        <td>
                            <asp:Label ID="txtEditMoneda" runat="server" style="margin-left: 4px; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; font-weight: bold; color: #666;"></asp:Label>
                        </td>
                        <asp:Panel ID="pnlValor" runat="server">
                            <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                                Valor: 
                            </td>
                            <td>
                                <asp:Label ID="txtEditPrecioBase" runat="server" style="margin-left: 4px; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; font-weight: bold; color: #666;"></asp:Label>
                            </td>
                        </asp:Panel>
                    </tr>
                    <tr>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                            Vendedor: 
                        </td>
                        <td>
                            <asp:Label ID="lbVendedor" runat="server" style="margin-left: 4px; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; font-weight: bold; color: #666;"></asp:Label>
                        </td>
                        <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                            Estado: 
                        </td>
                        <td>
                            <asp:Panel ID="pnlEditEstado" runat="server" Visible="false">
                                <asp:Label ID="lbEstado" runat="server" style="margin-left: 4px; font-family: Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; font-weight: bold; color: #666;"></asp:Label>
                            </asp:Panel>
                            <asp:Panel ID="pnlEditEstadoCombo" runat="server" Visible="false">
                                <asp:DropDownList ID="cbEditEstado" CssClass="dropDownList dropDownListForm" runat="server" style="width: 235px;"></asp:DropDownList>
                            </asp:Panel>
                        </td>
                     </tr>
                    <tr>
                        <asp:Panel ID="pnlEditCliente" runat="server" Visible="false">
                            <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                                Cliente: 
                            </td>
                            <td>
                                <asp:DropDownList ID="cbEditCliente" CssClass="dropDownList dropDownListForm" runat="server" style="width: 235px;">
                                </asp:DropDownList>
                            </td>
                        </asp:Panel>
                        <asp:Panel ID="pnlEditValor" runat="server" Visible="false">
                            <td style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right; width: 16%;">
                                Precio Base: 
                            </td>
                            <td>
                                <asp:TextBox ID="txtEditPrecioBaseVendido" runat="server" CssClass="textBoxForm" onkeypress="return onKeyDecimal(event,this);" style="position:relative; width: 220px;"></asp:TextBox>
                                <asp:RequiredFieldValidator id="RequiredFieldValidator10" runat="server"
                                ControlToValidate="txtEditPrecioBaseVendido"
                                ErrorMessage="Ingrese el valor de la unidad"
                                Validationgroup="EditGroup" Display="Dynamic" SetFocusOnError="True" CssClass="alert valerror borderValidator"/>
                            </td>
                        </asp:Panel>
                    </tr>
                    <tr>   
                        <td colspan="4">
                            <div align="left" style="float:left; width: 22%;">
                                <asp:Label ID="lbEditMensaje" runat="server" CssClass="messageError" style="margin-left: 75px;"></asp:Label>
                            </div>
                            <div align="center" style="position:relative">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlEdit">
                                    <ProgressTemplate>
                                            <div>
                                                <div style="float:left;"><img src="images/ring_loading.gif"  width="300px" style="height: 50px; width:50px" ImageAlign="left"  /></div>
                                                <div style="float:left; padding: 8px 0 0 10px">
                                                    <font style="font-size: 25px; color: #B7B7B7; font-weight: bold;"> Guardando cambios... </font>
                                                </div>                                    
                                            </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <div align="right" style="float:right">
                                <asp:Button ID="Button1" runat="server" CssClass="btnClose" Text="Cerrar" CommandArgument="Editar" OnClick="btnCerrar_Click" />
                                <asp:Button ID="btnGuardarEdit" runat="server" Text="Guardar" CssClass="formBtnNar" style="margin-left:15px" CausesValidation="true" Validationgroup="EditGroup" OnClick="btnGuardarEdit_Click"/>
                            </div>
                        </td>           
                    </tr>
                    <asp:Panel ID="pnlMensajeError" runat="server" Visible="false">
                        <tr class="formHolderAlert alert" style="padding: 14px 25px 12px;">
                            <td colspan="4">
                                Ya existe una unidad en el mismo nivel y nro. de unidad
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>   
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <ajax:ModalPopupExtender ID="modalEdit" runat="server" 
            TargetControlID="HiddenField1"
            PopupControlID="pnlEdit" 
            BackgroundCssClass="ModalBackground"
            DropShadow="true" /> 
    </section>

    <section>
        <asp:UpdatePanel ID="pnlDelete" runat="server" Width="410px" HorizontalAlign="Center" class="modal modalDelete">
            <ContentTemplate>
                <table width="100%" style="margin-bottom:0px">               
                    <tr>
                        <td colspan="2"><modalTitle><b>Eliminar Unidad</b></modalTitle></td>
                    </tr> 
                    <tr>
                        <td width="25%" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:right;">
                            <div align="center"><h4>¿Desea eliminar la unidad <asp:Label ID="lbDeleteUnidad" runat="server"></asp:Label>?</h4></div>
                        </td>
                    </tr> 
                    <tr>   
                        <td colspan="2">
                            <div align="center">
                                <asp:Button ID="btnYes" runat="server" CssClass="btnYes" Text="Si" Onclick="btnEliminar_Click" />
                                <asp:Button ID="btnNo" runat="server" CssClass="btnNo" Text="No" CommandArgument="Eliminar" OnClick="btnCerrar_Click"/>
                            </div>
                        </td>           
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>   
        <asp:HiddenField ID="HiddenField2" runat="server" />
        <ajax:ModalPopupExtender ID="modalDelete" runat="server" 
            TargetControlID="HiddenField2"
            PopupControlID="pnlDelete" 
            BackgroundCssClass="ModalBackground"
            DropShadow="true" /> 
    </section>

    <asp:HiddenField ID="hfId" runat="server" />

    <CR:crystalreportviewer ID="CrystalReportViewer" runat="server" AutoDataBind="True" Height="1200px" ReportSourceID="CrystalReportSource" Width="894px" DisplayToolbar="False" Visible="false" />
    <CR:crystalreportsource ID="CrystalReportSource" runat="server" Visible="false">
        <Report FileName="Reportes/Unidades1.rpt"></Report>
    </CR:crystalreportsource>
</asp:Content>
