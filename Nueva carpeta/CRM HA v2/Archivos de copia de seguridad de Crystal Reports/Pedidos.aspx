<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Pedidos" Codebehind="Pedidos.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</ajax:ToolkitScriptManager>

<link href="css/orange.css" rel="stylesheet" />

<script type="text/javascript" language="JavaScript">
    var fila = '';
    function Visible(__id) {
        if (fila != '') {
            document.getElementById('fila' + fila).className = '';
            document.getElementById('fila' + fila).className = 'invisible';
            document.getElementById('img' + fila).src = 'Imagenes/expand.gif';
        }
        if (fila != __id) {
            fila = __id;
            document.getElementById('fila' + fila).className = '';
            document.getElementById('img' + fila).src = 'Imagenes/collapse.gif';
        }
        else
            fila = '';
    }

    function SetEstado(_id, _idEstado) {
        if (_idEstado == '0')
            document.getElementById('row' + _id).className = 'red';
        if (_idEstado == '1')
            document.getElementById('row' + _id).className = 'blue';
        if (_idEstado == '2')
            document.getElementById('row' + _id).className = 'green'; 
    }  	
</script>

<section>
    <div class="formHolder" id="searchBoxTop">
        <div class="formHolderLine" style="margin-bottom:0px">
            <h2>Filtro:</h2>
        </div>
        <div>
            <label class="col3"><span>OBRA</span><asp:DropDownList ID="cbProyecto" runat="server"><asp:ListItem Text="Todas" Value="0" /></asp:DropDownList></label>

            <label class="col3"><span>CLIENTE</span><asp:DropDownList ID="cbEmpresa" runat="server"><asp:ListItem Text="Todas" Value="0" /></asp:DropDownList></label>
            
            <label class="col3"><span>ESTADO</span>
                <asp:DropDownList ID="cbEstado" runat="server"> <asp:ListItem Text="Todas" Value="0" /></asp:DropDownList>
            </label>
            
            <label class="col3"><span>FECHA DESDE</span>
                <label style="line-height: 14px;">
                    <asp:TextBox ID="txtFechaDesde" runat="server"></asp:TextBox>
                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="orange" TargetControlID="txtFechaDesde" Format="dd/MM/yyyy" PopupButtonID="imgCalendarH" />
                </label>
            </label>
            
            <label class="col3"><span>HASTA</span>
                <label style="line-height: 14px;">
                    <asp:TextBox ID="txtFechaHasta" runat="server"></asp:TextBox>
                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="orange" TargetControlID="txtFechaHasta" Format="dd/MM/yyyy" PopupButtonID="imgCalendarD" />
                </label>
            </label>     
            <label class="rigthLabel" style="width: 23%; margin-right: 3%;">
                <asp:Button ID="btnBuscar" Text="Buscar" class="formBtnNar" runat="server" onclick="btnBuscar_Click" />
                <asp:Button ID="LinkButton1" Text="Ver Todos" class="formBtnGrey" runat="server" onclick="btnVerTodas_Click" style="float:right; margin-left:15px;" />                    
            </label>
        </div>
    </div>
</section>   
         
<asp:ListView ID="lvTickets" runat="server">
    <LayoutTemplate>
        <ul id="accordionList">
            <li id="listHead">
                <span style="width:05%" class="listCel">ID</span>
                <span style="width:09%;" class="listCel">Fecha ingreso</span>
                <span style="width:14%;" class="listCel">Cliente</span>
                <span style="width:14%;" class="listCel">Obra</span>
                <span style="width:09%;" class="listCel">Teléfono</span>               
                <span style="width:18%;" class="listCel">Titulo</span>                
                <span style="width:08%" class="listCel">Fecha venc.</span>
                <span style="width:10%" class="listCel">Responsable</span>
                <span style="width:03%;" class="listCel"></span>
            </li>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />  
        </ul>                        
    </LayoutTemplate>               
    <ItemTemplate>
            <li>
                <div class="accordionButton">
                    <span style="width:05%" class="listCel"><%# Eval("id") %></span>
                    <span style="width:09%" class="listCel"><%# Eval("Fecha", "{0:d}")%></span>
                    <span style="width:14%" class="listCel"><%# Eval("GetEmpresa") %></span>
                    <span style="width:14%" class="listCel"><%# Eval("GetProyecto") %></span>
                    <span style="width:09%" class="listCel"><%# Eval("GetTelefono")%></span>
                    <span style="width:18%" class="listCel"><%# Eval("Titulo") %></span>                    
                    <span style="width:08%" class="listCel"><%# Eval("GetFechaLimite") %></span>
                    <span style="width:10%" class="listCel"><%# Eval("GetResponsableNombre") %></span>
                    <span style="width:03%" class="listCel">
                        <asp:LinkButton ID="lnkDetalles" Text="Detalles" class="detailBtn" OnClick="lnkDetalles_Click" CommandArgument=<%# Eval("id") %> runat="server"></asp:LinkButton>
                    </span>
                </div>
		        <div class="accordionContent">
                    <p class="col3"><strong>CLIENTE:</strong><span><%# Eval("GetClienteNombre")%></span></p>
                    <p class="col3"><strong>CATEGORIA:</strong><span><%# Eval("GetCategoria")%></span></p>
                    <p class="col3"><strong>CARGADO POR:</strong><span><%# Eval("GetUsuario")%></span></p>
                    <p><strong>DESCRIPCION:</strong><span><%# Eval("Descripcion")%></span></p>
                </div>
            </li>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" width="100%" runat="server">
            <tr>
                <td align="center">No data found.</td>
            </tr>
        </table>
    </EmptyDataTemplate>
</asp:ListView>

<div style="width:100%">
    <font style="font-family: Open Sans Condensed; font-size:14px; color: #666666">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>Registros Encontrados:</b>&nbsp;<asp:Label ID="lbCantRegistros" runat="server"></asp:Label>
    </font>
</div>
</asp:Content>