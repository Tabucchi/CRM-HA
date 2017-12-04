<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Inventario.aspx.cs" Inherits="crm.Inventario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    function onlyNumbers(evt) {
        var theEvent = evt || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        var regex = /[0-9]/;
        if (!regex.test(key) && theEvent.keyCode != 8) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    };
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <div class="formHolder" id="searchBoxTop">
            <div class="formHolderLine">
                <h2>Carga:</h2>
            </div>

            <div>
            <label>
                <span >CATEGORIA</span>
                <asp:DropDownList ID="cbCategoria" runat="server" Width="38%"> <asp:ListItem Text="Mesa" 
                        Value="1" /></asp:DropDownList>
            </label>  

            <label>
                <span>VALOR</span>
                <asp:TextBox ID="txtValor" runat="server" onkeypress='onlyNumbers(event)'></asp:TextBox>
            </label>
            <label>
                <span>CANTIDADES</span>
                <asp:TextBox ID="txtCantUnidades" runat="server" onkeypress='onlyNumbers(event)'></asp:TextBox>
            </label>
            <label>
                <span style="width:13%">IMAGEN</span>
                <asp:FileUpload ID="fileArchivo" runat="server" />
            </label>            
            
            <label>
                <span>DESCRIPCIÓN</span>
                <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine"></asp:TextBox>
            </label>
             </div>

            <label>
                <asp:Label ID="lbMensaje" runat="server" Visible="False" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300" Width="50%"></asp:Label>
            </label>
            <label class="col3 rigthLabel">
                <asp:Button ID="btnBuscar" Text="Cargar" class="formBtnNar" runat="server" onclick="btnCargar_Click" />              
            </label>
        </div>
    </section> 
   
    <%--<ul id="accordionList" style="display:block;" class="invisible">
        <li id="listHead" style="padding:0px;">
            <span style="width:4%">ID</span>
            <span style="width:13%">DESCRIPCION</span>
            <span style="width:11%">CATEGORIA</span>
            <span style="width:10%">NÚMERO</span>
            <span style="width:10%">UNIDADES</span>
            <span style="width:7%">VALOR</span>
            <span style="width:9%">EMPRESA</span>
            <span style="width:18%">RESPONSABLE</span>
            <span style="width:7%">IMAGEN</span> 
            </li>
    </ul>--%>

    <asp:ListView ID="lvInventario" runat="server" DataKeyNames="id">
        <LayoutTemplate>      
            <section style="margin-bottom:18px">     
                <table>                            
                    <thead id="tableHead">
                        <tr>
                            <td>ID</td>                              
                            <td>DESCRIPCION</td>
                            <td>CATEGORIA</td>                            
                            <td>NÚMERO</td>
                            <td>UNIDADES</td>
                            <td>VALOR</td>
                            <td>EMPRESA</td>
                            <td>RESPONSABLE</td>
                            <td>IMAGEN</td>
                            <td></td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>                        
                </table>
            </section>
        </LayoutTemplate>
        <ItemTemplate>                   
            <tr style="cursor:pointer;" onclick="Visible(<%# Eval("id")%> )" >
                <td><asp:Label ID="lbId" runat="Server" Text='<%#Eval("id") %>' Enabled="False" /></td>
                <td align="left"><asp:Label ID="lbDescripcion" runat="Server" Text='<%#Eval("descripcion") %>' /></td>
                <td><asp:Label ID="lbCategoria" runat="Server" Text='<%#Eval("GetCategoria") %>' /></td>
                <td><asp:Label ID="lbNumero" runat="Server" Text='<%#Eval("numero") %>' /></td>   
                <td><asp:Label ID="lbUnidades" runat="Server" Text='<%#Eval("cantUnidades") %>' /></td>
                <td><asp:Label ID="lbValor" runat="Server" Text='<%#Eval("valor") %>' /></td>
                <td><asp:Label ID="lbEmpresa" runat="Server" Text='<%#Eval("GetEmpresa") %>' /></td>
                <td><asp:Label ID="lbResponsable" runat="Server" Text='<%#Eval("GetResponsable") %>' /></td>
                <td>
                    <asp:LinkButton ID="lnkImagen" Text="Imagen" class="detailBtn" OnClick="lnkImagen_Click" CommandArgument='<%# Eval("IdImagen") %>' runat="server"></asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="lnkDelete" Text="Eliminar" class="deleteBtn" OnClick="lnkEliminar_Click" runat="server" CommandArgument='<%# Eval("id") %>'></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server">
                <tr>
                    <td align="center"><b>No data found.<b/></td>
                </tr>
            </table>
        </EmptyDataTemplate>
    </asp:ListView>

    <div class="rfloat" align="right" style="width:100%; padding-left:-2px">
        <font style="font-family: Open Sans Condensed; font-size:18px; color: #666666">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>Total:</b>&nbsp;$<asp:Label ID="lbTotal" runat="server"></asp:Label>
        </font>
    </div>
</asp:Content>
