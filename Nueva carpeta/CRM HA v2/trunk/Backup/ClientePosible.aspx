<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ClientePosible" Title="Página sin título" Codebehind="ClientePosible.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <section>
        <div class="headOptions">
            <h2><asp:Label ID="lblEmpresa" runat="server"></asp:Label></h2>
            <div style="float:right;">
                <a href="AgendaClientesPosibles.aspx" class="formBtnGrey">< Agenda Clientes Futuros</a>
                <asp:Button ID="Button1" runat="server" Text="Confirmar Nuevo Cliente" CssClass="formBtnGrey" onclick="btnConfirmar_Click" />
            </div>
        </div>
        <div class="formHolder">
            <label><h2>DATOS DE LA EMPRESA</h2></label>
            <label class="rigthLabel">                
                <asp:Button ID="btnEditarEmpresa" runat="server" CssClass="formBtnNar" Text="Editar" OnClick="btnEditarEmpresa_Click" />
                <asp:Button ID="btnAceptarEmpresa" runat="server" CssClass="formBtnNar" Text="Aceptar" OnClick="btnAceptarEmpresa_Click" Visible="false" ></asp:Button>
            </label>
            <asp:Panel ID="pnlEmpresa" runat="server">
                <p class="col3"><strong>NOMBRE: </strong> <asp:Label ID="lblNombre" runat="server"></asp:Label> </p>
                <p class="col3"><strong>RUBRO: </strong><asp:Label ID="lblRubro" runat="server"></asp:Label> </p>
                <p class="col3"><strong>DIRECCION: </strong> <asp:Label ID="lblDireccion" runat="server"></asp:Label> </p>
                <p class="col3"><strong>TELÉFONO: </strong> <asp:Label ID="lblTelefono" runat="server"></asp:Label> </p>
                <p class="col3"><strong>MAIL: </strong>  <asp:Label ID="lblMail" runat="server"/> </p>
            </asp:Panel>
            <asp:Panel ID="pnlEmpresaEdit" runat="server" Visible="false">
               <p class="col3"><strong>NOMBRE:</strong> <asp:TextBox ID="txtNombre" runat="server" Width="210px" MaxLength="64"/> </p>
               <p class="col3"><strong>RUBRO: </strong> <asp:TextBox ID="txtRubro" runat="server" Width="210px" MaxLength="60"/> </p>
               <p class="col3"><strong>DIRECCION: </strong> <asp:TextBox ID="txtDireccion" runat="server" Width="210px" MaxLength="64"/> </p>
               <p class="col3"><strong>TELÉFONO:</strong> <asp:TextBox ID="txtTelefono" runat="server" Width="210px" MaxLength="32"/> </p>
               <p class="col3"><strong>MAIL: </strong> <asp:TextBox ID="txtMail" runat="server" Width="210px" MaxLength="64" /> </p>
            </asp:Panel>
        </div>
        <div class="formHolder">
            <label><h2>DATOS DE CONTACTO</h2></label>
            <label class="rigthLabel">
                <asp:Button ID="btnEditContacto" runat="server" Text="Editar" CssClass="formBtnNar" onclick="btnEditContacto_Click" />
                <asp:Button ID="btnAceptarContacto" runat="server" Text="Aceptar" Visible="false" CssClass="formBtnNar" onclick="btnAceptarContacto_Click" /> 
            </label>
            <asp:Panel ID="pnlContacto" runat="server">
                <p class="col3"><strong>CONTACTO:</strong><asp:Label ID="lblContacto" runat="server"></asp:Label></p>
                <p class="col3"><strong>PUESTO CONTACTO:</strong><asp:Label ID="lblPuestoContacto" runat="server"></asp:Label></p>
                <p class="col3"><strong>CONTACTO VINCULO:</strong><asp:Label ID="lblContactoVinculo" runat="server"></asp:Label></p>
                <p class="col3"><strong>CONSEGUIDO POR:</strong><asp:Label ID="lblConseguidoPor" runat="server"></asp:Label></p>
            </asp:Panel>
            <asp:Panel ID="pnlContactoEdit" runat="server" Visible="false">
                <p class="col3"><strong>CONTACTO:</strong><asp:TextBox ID="txtContacto" runat="server" Width="210px" MaxLength="60"/></p>
                <p class="col3"><strong>CONTACTO:</strong><asp:TextBox ID="txtPuestoContacto" runat="server" Width="210px" MaxLength="60"/></p>
                <p class="col3"><strong>VINCULO:</strong><asp:TextBox ID="txtContactoVinculo" runat="server" Width="210px" MaxLength="60"/></p>
                <p class="col3"><strong>CONSEGUIDO:</strong><asp:DropDownList ID="ddlEditConseguidoPor" runat="server" Width="210px"/></p>
            </asp:Panel>
        </div>
        <div class="formHolder">
            <label><h2>ACCIONES</h2></label>
            <label class="rigthLabel">
                <asp:Button ID="btnEditAcciones" runat="server" CssClass="formBtnNar" Text="Editar" onclick="btnEditAcciones_Click" />
                <asp:Button ID="btnAceptarAcciones" runat="server" CssClass="formBtnNar" Text="Aceptar" Visible="false" onclick="btnAceptarAcciones_Click" /> 
                <asp:LinkButton ID="btnAgregarAccion" CssClass="formBtnNar" runat="server">Agregar nueva acción</asp:LinkButton>
            </label>
            <asp:Panel ID="pnlAcciones" runat="server">
                <p><strong>ACCIÓN 1:</strong>
                <asp:Label ID="lblAccion1" runat="server"></asp:Label>
                <asp:Image ID="ImageAccion1" runat="server" Height="20px" Width="20px" ImageUrl="Imagenes/imageTrue.png" />
                </p><p><strong>ACCIÓN 2:</strong>
                <asp:Label ID="lblAccion2" runat="server"></asp:Label>
                <asp:Image ID="ImageAccion2" runat="server" Height="20px" Width="20px"/>
                </p><p><strong>ACCIÓN 3:</strong>
                <asp:Label ID="lblAccion3" runat="server"></asp:Label>
                <asp:Image ID="ImageAccion3" runat="server" Height="20px" Width="20px"/>
                </p><p><strong>ACCIÓN 4:</strong>
                <asp:Label ID="lblAccion4" runat="server"></asp:Label>
                <asp:Image ID="ImageAccion4" runat="server" Height="20px" Width="20px"/>
                </p>
            </asp:Panel>
            <asp:Panel ID="pnlEditAcciones" runat="server" Visible="false">
                 <p><strong>ACCIÓN 1:</strong>
                <asp:DropDownList ID="ddlAccion1" runat="server" Width="210px"/>
                <asp:ImageButton ID="btnImageTrue1" runat="server" ImageUrl="Images/imageTrue.png" Height="20px" Width="20px" onclick="btnImageTrue1_Click"/>
                <asp:ImageButton ID="btnImageFalse1" runat="server" ImageUrl="Images/imageFalse.png" Height="20px" Width="20px" onclick="btnImageFalse1_Click"/>
                 <p><strong>ACCIÓN 2:</strong>
                <asp:DropDownList ID="ddlAccion2" runat="server" Width="210px"/>
                <asp:ImageButton ID="btnImageTrue2" runat="server" ImageUrl="Images/imageTrue.png" Height="20px" Width="20px" onclick="btnImageTrue2_Click"/>
                <asp:ImageButton ID="btnImageFalse2" runat="server" ImageUrl="Images/imageFalse.png" Height="20px" Width="20px" onclick="btnImageFalse2_Click"/>
                 <p><strong>ACCIÓN 3:</strong>
                <asp:DropDownList ID="ddlAccion3" runat="server" Width="210px"/>
                <asp:ImageButton ID="btnImageTrue3" runat="server" ImageUrl="Images/imageTrue.png" Height="20px" Width="20px" onclick="btnImageTrue3_Click"/>
                <asp:ImageButton ID="btnImageFalse3" runat="server" ImageUrl="Images/imageFalse.png" Height="20px" Width="20px" onclick="btnImageFalse3_Click"/>
                 <p><strong>ACCIÓN 4:</strong>
                <asp:DropDownList ID="ddlAccion4" runat="server" Width="210px"/>
                <asp:ImageButton ID="btnImageTrue4" runat="server" ImageUrl="Images/imageTrue.png" Height="20px" Width="20px" onclick="btnImageTrue4_Click"/>
                <asp:ImageButton ID="btnImageFalse4" runat="server" ImageUrl="Images/imageFalse.png" Height="20px" Width="20px" onclick="btnImageFalse4_Click"/>
                </p>
            </asp:Panel>
        </div>
        <div class="formHolder">
            <p class="col3"><strong>ESTADO:</strong>
            <asp:DropDownList ID="ddlEstado" runat="server" Width="210px" onselectedindexchanged="ddlEstado_SelectedIndexChanged" AutoPostBack="True">
                <asp:ListItem Value="0">Seleccione un estado...</asp:ListItem>
                <asp:ListItem Value="1">Interesa</asp:ListItem>
                <asp:ListItem Value="2">No interesa</asp:ListItem>
            </asp:DropDownList>
            </p>
        </div>
    </section>                
    
    <asp:Panel ID="pnlNuevaAccion" runat="server" Width="350px" HorizontalAlign="Center" CssClass="ModalPopup">
        <table width="100%">
            <tr>
                <td align="right" colspan="2"><asp:Label ID="lblClose" Text="X" runat="server" CssClass="boton"></asp:Label>
                </td>
            </tr>
            <tr style="background-color: #FFE117">
                <td colspan="2" style="color:#706F6F; font-family:Tahoma,Verdana,Arial,Helvetica,sans-serif; font-size:12px; text-align:center;"><b>Nueva Acción</b></td>
            </tr> 
            <tr>
                <td align="left" style="width:116px"><b>Acción:</b></td>
                <td align="left"><asp:TextBox ID="txtNuevaAccion" runat="server" Width="222px"/></td>
            </tr>
            <tr class="spacer">
                <td colspan="2" > <hr /> </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnAceptar" runat="server" class="boton" Text="Aceptar" 
                        onclick="btnAceptar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>   
    <ajax:ModalPopupExtender ID="ModalPopupExtender" runat="server" 
        TargetControlID="btnAgregarAccion"
        PopupControlID="pnlNuevaAccion" 
        CancelControlID="lblClose"
        BackgroundCssClass="ModalBackground"
        DropShadow="true" />
</asp:Content>

