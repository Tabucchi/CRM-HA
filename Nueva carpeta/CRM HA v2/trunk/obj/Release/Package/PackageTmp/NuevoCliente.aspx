<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="NuevoCliente.aspx.cs" Inherits="crm.NuevoCliente" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var updateProgress = null;

    function postbackButtonClick() {
        updateProgress = $find("<%= UpdateProgress1.ClientID %>");
        window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
        return true;
    }
</script>

<script src="js/jquery.mask.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('.cuit').mask('99-99999999-9');
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>

<asp:UpdatePanel ID="pnlOperacionVenta" runat="server">
    <ContentTemplate>
        <div id="maincol" style="height:600px; width: 100%;"> 
            <section>
                <div class="formHolder" id="searchBoxTop">
                    <div class="headOptions headLine">
                        <h2>Nuevo Cliente</h2>
                    </div>
                </div>
            </section>

            <section>
                <div class="formHolder" style="padding: 22px 25px 12px;">
                    <div><modaltitle style="text-align:left">Tipo de cliente</modaltitle></div><hr> 
                    <div style="float:left; width: 263px;">
                        <label style="width: 65%;">
                            <asp:DropDownList ID="cbTipoCliente" runat="server" CssClass="dropDownList dropDownListForm" AutoPostBack="true" OnSelectedIndexChanged="cbTipoCliente_SelectedIndexChanged" style="width: 270px; margin-top: 2px;"></asp:DropDownList>
                            <asp:RequiredFieldValidator id="rfvTipoCliente" runat="server" style="width: 256px;"
                                ControlToValidate="cbTipoCliente" InitialValue="Seleccione un tipo de cliente..." ErrorMessage="Campo obligatorio"
                                Validationgroup="validateCliente" Display="Dynamic" SetFocusOnError="True" CssClass="alert borderValidator">
                            </asp:RequiredFieldValidator>
                        </label> 
                    </div>
                </div>
            </section>

            <asp:UpdatePanel ID="pnlPersonaFisica" runat="server" Visible="false">
                <ContentTemplate>
                    <section>
                        <div class="formHolder"  style="padding: 22px 25px 12px;">
                            <div><modalTitle style="text-align:left">Persona física</modalTitle></div><hr />
                            <div class="divFormInLine">
                                <div>
                                    <label>
                                        Apellido:
                                        <asp:TextBox ID="txtApellido" runat="server" CssClass="textBox textBoxForm" TabIndex="1" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                        <asp:RequiredFieldValidator id="rfvNombre" runat="server" ControlToValidate="txtApellido" 
                                            ErrorMessage="Campo obligatorio" CssClass="alert borderValidator" style="width: 220px;"
                                            Validationgroup="validateCliente" Display="Dynamic" SetFocusOnError="True">
                                        </asp:RequiredFieldValidator>
                                    </label> 
                                </div>
                            </div>
                            <div class="divFormInLine">
                                <div>
                                    <label>
                                        Nombre:
                                        <asp:TextBox ID="txtNombre" runat="server" CssClass="textBox textBoxForm" TabIndex="2" style="width: 220px; margin-top: 2px;"></asp:TextBox>                                    
                                    </label> 
                                </div>
                            </div>
                            <div class="divFormInLine">
                                <div>
                                    <label>
                                        Carácter:
                                        <br />
                                        <asp:DropDownList ID="cbCaracter" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="3" style="width: 234px; margin-top:2px">
                                            <asp:ListItem Value="1">Título personal</asp:ListItem>
                                            <asp:ListItem Value="2">En comisión</asp:ListItem>
                                        </asp:DropDownList>
                                    </label> 
                                </div>
                            </div>  
                            <div class="divFormInLine">
                                <label>
                                    Cuit: 
                                    <asp:TextBox ID="txtCuit" runat="server" CssClass="textBox textBoxForm cuit" TabIndex="4" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                </label>
                            </div>                      
                        </div>

                        <div class="formHolder"  style="padding: 22px 25px 12px;">
                            <div class="divFormInLine">
                                <label>
                                    Condición de IVA: 
                                    <asp:DropDownList ID="ddlCondicionIva" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="5" style="width: 234px; margin-top:2px">
                                        <asp:ListItem Value="1">Consumidor final</asp:ListItem>
                                        <asp:ListItem Value="2">Exento</asp:ListItem>
                                        <asp:ListItem Value="3">Monotributista</asp:ListItem>
                                        <asp:ListItem Value="4">No responsable</asp:ListItem>
                                        <asp:ListItem Value="5">Responsable inscripto</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator id="rfvCondicionIva" runat="server"
                                        ControlToValidate="ddlCondicionIva" InitialValue="0"
                                        ErrorMessage="Campo obligatorio" CssClass="alert borderValidator" style="width: 220px;"
                                        Validationgroup="validateCliente" Display="Dynamic" SetFocusOnError="True">
                                    </asp:RequiredFieldValidator> 
                                </label>
                            </div>
                        </div>

                        <div class="formHolder"  style="padding: 22px 25px 12px;">
                            <div><modaltitle style="text-align:left; font-size: 16px !important;">Documento</modaltitle></div>
                            <hr class="line">
                            <div class="divFormInLine">
                                <div>
                                    <label>
                                        Tipo:
                                        <asp:DropDownList ID="cbTipoDoc" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="6" style="width: 234px; margin-top:2px">
                                            <asp:ListItem Value="1">DNI</asp:ListItem>
                                            <asp:ListItem Value="2">CI</asp:ListItem>
                                            <asp:ListItem Value="3">LC</asp:ListItem>
                                            <asp:ListItem Value="4">LE</asp:ListItem>
                                        </asp:DropDownList>
                                    </label>
                                </div>
                            </div>                        
                            <div class="divFormInLine">
                                <div>
                                    <label>
                                        Número:
                                        <asp:TextBox ID="txtNroDoc" runat="server" CssClass="textBox textBoxForm" TabIndex="7" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                    </label>
                                </div>
                            </div>
                        </div>
                        
                        <div class="formHolder"  style="padding: 22px 25px 12px;">
                            <div><modaltitle style="text-align:left; font-size: 16px !important;">Domicilio</modaltitle></div>
                            <hr class="line">
                            <div class="divFormInLine">
                                <label>
                                    Calle: 
                                    <asp:TextBox ID="txtCalle" runat="server" CssClass="textBox textBoxForm" TabIndex="8" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                </label>
                            </div>
                            <div class="divFormInLine">
                                <label>
                                    Dirección: 
                                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="textBox textBoxForm" TabIndex="9" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                </label>
                            </div>                       
                            <div class="divFormInLine">
                                <label>
                                    Código Postal: 
                                    <asp:TextBox ID="txtCodPostal" runat="server" CssClass="textBox textBoxForm" TabIndex="10" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                </label>
                            </div>
                        </div>

                        <div class="formHolder"  style="padding: 22px 25px 12px;">
                             <div class="divFormInLine">
                                <label>
                                    Provincia: 
                                    <asp:DropDownList ID="cbProvincia" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="11" style="width: 234px; margin-top:2px">
                                        <asp:ListItem Value="1">Capital Federal</asp:ListItem>
                                        <asp:ListItem Value="2">Buenos Aires</asp:ListItem>
                                        <asp:ListItem Value="3">Catamarca</asp:ListItem>
                                        <asp:ListItem Value="4">Córdoba</asp:ListItem>
                                        <asp:ListItem Value="5">Corrientes</asp:ListItem>
                                        <asp:ListItem Value="6">Chaco</asp:ListItem>
                                        <asp:ListItem Value="7">Chubut</asp:ListItem>
                                        <asp:ListItem Value="8">Entre Ríos</asp:ListItem>
                                        <asp:ListItem Value="9">Formosa</asp:ListItem>
                                        <asp:ListItem Value="10">Jujuy</asp:ListItem>
                                        <asp:ListItem Value="11">La Pampa</asp:ListItem>
                                        <asp:ListItem Value="12">La Rioja</asp:ListItem>
                                        <asp:ListItem Value="13">Mendoza</asp:ListItem>
                                        <asp:ListItem Value="14">Misiones</asp:ListItem>
                                        <asp:ListItem Value="15">Neuquén</asp:ListItem>
                                        <asp:ListItem Value="16">Río Negro</asp:ListItem>
                                        <asp:ListItem Value="17">Salta</asp:ListItem>
                                        <asp:ListItem Value="18">San Juan</asp:ListItem>
                                        <asp:ListItem Value="19">San Luis</asp:ListItem>
                                        <asp:ListItem Value="20">Santa Cruz</asp:ListItem>
                                        <asp:ListItem Value="21">Santa Fe</asp:ListItem>
                                        <asp:ListItem Value="22">Santiago del Estero</asp:ListItem>
                                        <asp:ListItem Value="23">Tierra del Fuego</asp:ListItem>
                                        <asp:ListItem Value="24">Tucumán</asp:ListItem>
                                    </asp:DropDownList>
                                </label>
                            </div>
                            <div class="divFormInLine">
                                <label>
                                    Ciudad: 
                                    <asp:TextBox ID="txtCiudad" runat="server" CssClass="textBox textBoxForm" TabIndex="12" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                </label>
                            </div>
                        </div>
                        
                        <div class="formHolder"  style="padding: 22px 25px 12px;">
                            <div><modaltitle style="text-align:left; font-size: 16px !important;">Contacto</modaltitle></div>
                            <hr class="line">
                            <div class="divFormInLine">
                                <label>
                                    Teléfono: 
                                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="textBox textBoxForm" TabIndex="13" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                </label>
                            </div>
                            <div class="divFormInLine">
                                <label>
                                    Email: 
                                    <asp:TextBox ID="txtMail" runat="server" CssClass="textBox textBoxForm" TabIndex="14" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                </label>
                            </div>
                        </div>

                        <div class="formHolder"  style="padding: 22px 25px 12px;">
                            <div><modaltitle style="text-align:left; font-size: 16px !important;">Apoderado</modaltitle></div>
                            <hr class="line">
                            <div class="divFormInLine">
                                <label style="width: 100%;">
                                    <font style="font-style: italic; font-size: 14px;">¿El cliente representa a un apoderado?</font> 
                                    <asp:DropDownList ID="cbApoderado" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="15" style="width: 234px; margin-top:2px" AutoPostBack="True" OnSelectedIndexChanged="ddlCondicion_SelectedIndexChanged">
                                        <asp:ListItem Value="1">No</asp:ListItem>
                                        <asp:ListItem Value="2">Si</asp:ListItem>
                                    </asp:DropDownList>
                                </label>
                            </div>
                        </div>
                    </section>

                    <section>
                        <asp:UpdatePanel ID="pnlApoderadoFisica" runat="server" Visible="false">
                            <ContentTemplate>
                                <div class="formHolder"  style="padding: 22px 25px 12px;">
                                    <div><modaltitle style="text-align:left; font-size: 16px !important;">Datos apoderado</modaltitle></div>
                                    <hr class="line">
                                    <div class="divFormInLine">
                                        <div>
                                            <label>
                                                Razon social:
                                                <asp:TextBox ID="txtNombreApoderadoFisica" runat="server" CssClass="textBox textBoxForm" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                                <asp:RequiredFieldValidator id="rfvNombreApoderadoFisica" runat="server"
                                                    ControlToValidate="txtNombreApoderadoFisica" 
                                                    ErrorMessage="Campo obligatorio" CssClass="alert borderValidator" style="width: 220px;"
                                                    Validationgroup="validateCliente" Display="Dynamic" SetFocusOnError="True">
                                                </asp:RequiredFieldValidator>
                                            </label> 
                                        </div>
                                    </div> 
                                    <div class="divFormInLine">
                                        <label>
                                            Cuit: 
                                            <asp:TextBox ID="txtCuitApoderadoFisica" runat="server" CssClass="textBox textBoxForm cuit" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                        </label>
                                    </div> 
                                </div>
                                <div class="formHolder"  style="padding: 22px 25px 12px;">
                                    <div><modaltitle style="text-align:left; font-size: 16px !important;">Documento</modaltitle></div>
                                    <hr class="line">
                                    <div class="divFormInLine">
                                        <div>
                                            <label>
                                                Tipo:
                                                <br />
                                                <asp:DropDownList ID="cbTipoDocApoderadoFisica" runat="server" CssClass="dropDownList dropDownListForm" style="width: 234px; margin-top:2px">
                                                    <asp:ListItem Value="1">DNI</asp:ListItem>
                                                    <asp:ListItem Value="2">CI</asp:ListItem>
                                                    <asp:ListItem Value="3">LC</asp:ListItem>
                                                    <asp:ListItem Value="4">LE</asp:ListItem>
                                                </asp:DropDownList>
                                            </label>
                                        </div>
                                    </div>                        
                                    <div class="divFormInLine">
                                        <div>
                                            <label>
                                                Número:
                                                <asp:TextBox ID="txtNroDocApoderadoFisica" runat="server" CssClass="textBox textBoxForm" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            
                                <div class="formHolder"  style="padding: 22px 25px 12px;">
                                    <div><modaltitle style="text-align:left; font-size: 16px !important;">Domicilio</modaltitle></div>
                                    <hr class="line">
                                    <div class="divFormInLine">
                                        <label>
                                            Calle: 
                                            <asp:TextBox ID="txtCalleApoderadoFisica" runat="server" CssClass="textBox textBoxForm" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                        </label>
                                    </div>
                                    <div class="divFormInLine">
                                        <label>
                                            Dirección: 
                                            <asp:TextBox ID="txtDireccionApoderadoFisica" runat="server" CssClass="textBox textBoxForm" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                        </label>
                                    </div>
                       
                                    <div class="divFormInLine">
                                        <label>
                                            Código Postal: 
                                            <asp:TextBox ID="txtCodPostalApoderadoFisica" runat="server" CssClass="textBox textBoxForm" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                        </label>
                                    </div>
                                </div>
                            
                                <div class="formHolder"  style="padding: 22px 25px 12px;">
                                     <div class="divFormInLine">
                                        <label>
                                            Provincia: 
                                            <asp:DropDownList ID="cbProvinciaApoderadoFisica" runat="server" CssClass="dropDownList dropDownListForm" style="width: 234px; margin-top:2px">
                                                <asp:ListItem Value="1">Capital Federal</asp:ListItem>
                                                <asp:ListItem Value="2">Buenos Aires</asp:ListItem>
                                                <asp:ListItem Value="3">Catamarca</asp:ListItem>
                                                <asp:ListItem Value="4">Córdoba</asp:ListItem>
                                                <asp:ListItem Value="5">Corrientes</asp:ListItem>
                                                <asp:ListItem Value="6">Chaco</asp:ListItem>
                                                <asp:ListItem Value="7">Chubut</asp:ListItem>
                                                <asp:ListItem Value="8">Entre Ríos</asp:ListItem>
                                                <asp:ListItem Value="9">Formosa</asp:ListItem>
                                                <asp:ListItem Value="10">Jujuy</asp:ListItem>
                                                <asp:ListItem Value="11">La Pampa</asp:ListItem>
                                                <asp:ListItem Value="12">La Rioja</asp:ListItem>
                                                <asp:ListItem Value="13">Mendoza</asp:ListItem>
                                                <asp:ListItem Value="14">Misiones</asp:ListItem>
                                                <asp:ListItem Value="15">Neuquén</asp:ListItem>
                                                <asp:ListItem Value="16">Río Negro</asp:ListItem>
                                                <asp:ListItem Value="17">Salta</asp:ListItem>
                                                <asp:ListItem Value="18">San Juan</asp:ListItem>
                                                <asp:ListItem Value="19">San Luis</asp:ListItem>
                                                <asp:ListItem Value="20">Santa Cruz</asp:ListItem>
                                                <asp:ListItem Value="21">Santa Fe</asp:ListItem>
                                                <asp:ListItem Value="22">Santiago del Estero</asp:ListItem>
                                                <asp:ListItem Value="23">Tierra del Fuego</asp:ListItem>
                                                <asp:ListItem Value="24">Tucumán</asp:ListItem>
                                            </asp:DropDownList>
                                        </label>
                                    </div>
                                    <div class="divFormInLine">
                                        <label>
                                            Ciudad: 
                                            <asp:TextBox ID="txtCiudadApoderadoFisica" runat="server" CssClass="textBox textBoxForm" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                        </label>
                                    </div>
                                </div>

                                <div class="formHolder"  style="padding: 22px 25px 12px;">
                                    <div><modaltitle style="text-align:left; font-size: 16px !important;">Contacto</modaltitle></div>
                                    <hr class="line">
                                    <div class="divFormInLine">
                                        <label>
                                            Teléfono: 
                                            <asp:TextBox ID="txtTelefonoApoderadoFisica" runat="server" CssClass="textBox textBoxForm" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                        </label>
                                    </div>
                                    <div class="divFormInLine">
                                        <label>
                                            Email: 
                                            <asp:TextBox ID="txtMailApoderadoFisica" runat="server" CssClass="textBox textBoxForm" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                        </label>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </section>

                    <section>
                        <div class="formHolder"  style="padding: 22px 25px 12px;">
                            <div><modalTitle style="text-align:left">Comentarios</modalTitle></div><hr />
                            <div class="divFormInLine">
                                <div>
                                    <label>
                                        <asp:TextBox ID="txtComentarioFisica" runat="server" CssClass="textBox textBoxForm" TextMode="MultiLine" style="width: 900px; margin-top: 2px;"></asp:TextBox>
                                    </label> 
                                </div>
                            </div>                     
                        </div>
                        <div class="formHolder" style="padding: 8px 25px 12px;">
                            <font style="font-size: 13px; color: red;">Máximo de caracteres: 2500</font>
                        </div>
                    </section>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="pnlPersonaJuridica" runat="server" Visible="false">
                <ContentTemplate>
                    <section>
                    <div class="formHolder"  style="padding: 22px 25px 12px;">
                        <div><modalTitle style="text-align:left">Persona jurídica</modalTitle></div><hr />
                        <div class="divFormInLine">
                            <div>
                                <label>
                                    Razón social:
                                    <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="textBox textBoxForm" TabIndex="16" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator id="rfvRazonSocial" runat="server"
                                        ControlToValidate="txtRazonSocial" CssClass="alert borderValidator" style="width: 220px;"
                                        ErrorMessage="Campo obligatorio"
                                        Validationgroup="validateCliente" Display="Dynamic" SetFocusOnError="True">
                                    </asp:RequiredFieldValidator>
                                </label> 
                            </div>
                        </div>
                        <div class="divFormInLine">
                            <label>
                                Condición de IVA: 
                                <asp:DropDownList ID="ddlCondicionIvaJuridica" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="17" style="width: 234px; margin-top:2px"></asp:DropDownList>
                                <asp:RequiredFieldValidator id="rfvCondicionIvaJuridica" runat="server"
                                    ControlToValidate="ddlCondicionIvaJuridica" InitialValue="0" CssClass="alert borderValidator" style="width: 220px;"
                                    ErrorMessage="Campo obligatorio"
                                    Validationgroup="validateCliente" Display="Dynamic" SetFocusOnError="True">
                                </asp:RequiredFieldValidator> 
                            </label>
                        </div>
                        <div class="divFormInLine">
                            <label>
                                Cuit: 
                                <asp:TextBox ID="txtCuitJuridica" runat="server" CssClass="textBox textBoxForm cuit" TabIndex="18" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                            </label>
                        </div>                      
                    </div>
                        
                    <div class="formHolder"  style="padding: 22px 25px 12px;">
                        <div><modaltitle style="text-align:left; font-size: 16px !important;">Domicilio</modaltitle></div>
                        <hr class="line">
                        <div class="divFormInLine">
                            <label>
                                Calle: 
                                <asp:TextBox ID="txtCalleJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="19" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                            </label>
                        </div>
                        <div class="divFormInLine">
                            <label>
                                Dirección: 
                                <asp:TextBox ID="txtDireccionJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="20" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                            </label>
                        </div>
                       
                        <div class="divFormInLine">
                            <label>
                                Código Postal: 
                                <asp:TextBox ID="txtCodPostalJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="21" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                            </label>
                        </div>
                    </div>

                    <div class="formHolder"  style="padding: 22px 25px 12px;">
                         <div class="divFormInLine">
                            <label>
                                Provincia: 
                                <asp:DropDownList ID="cbProvinciaJuridica" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="22" style="width: 234px; margin-top:2px">
                                    <asp:ListItem Value="1">Capital Federal</asp:ListItem>
                                    <asp:ListItem Value="2">Buenos Aires</asp:ListItem>
                                    <asp:ListItem Value="3">Catamarca</asp:ListItem>
                                    <asp:ListItem Value="4">Córdoba</asp:ListItem>
                                    <asp:ListItem Value="5">Corrientes</asp:ListItem>
                                    <asp:ListItem Value="6">Chaco</asp:ListItem>
                                    <asp:ListItem Value="7">Chubut</asp:ListItem>
                                    <asp:ListItem Value="8">Entre Ríos</asp:ListItem>
                                    <asp:ListItem Value="9">Formosa</asp:ListItem>
                                    <asp:ListItem Value="10">Jujuy</asp:ListItem>
                                    <asp:ListItem Value="11">La Pampa</asp:ListItem>
                                    <asp:ListItem Value="12">La Rioja</asp:ListItem>
                                    <asp:ListItem Value="13">Mendoza</asp:ListItem>
                                    <asp:ListItem Value="14">Misiones</asp:ListItem>
                                    <asp:ListItem Value="15">Neuquén</asp:ListItem>
                                    <asp:ListItem Value="16">Río Negro</asp:ListItem>
                                    <asp:ListItem Value="17">Salta</asp:ListItem>
                                    <asp:ListItem Value="18">San Juan</asp:ListItem>
                                    <asp:ListItem Value="19">San Luis</asp:ListItem>
                                    <asp:ListItem Value="20">Santa Cruz</asp:ListItem>
                                    <asp:ListItem Value="21">Santa Fe</asp:ListItem>
                                    <asp:ListItem Value="22">Santiago del Estero</asp:ListItem>
                                    <asp:ListItem Value="23">Tierra del Fuego</asp:ListItem>
                                    <asp:ListItem Value="24">Tucumán</asp:ListItem>
                                </asp:DropDownList>
                            </label>
                        </div>
                        <div class="divFormInLine">
                            <label>
                                Ciudad: 
                                <asp:TextBox ID="txtCiudadJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="23" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                            </label>
                        </div>
                    </div>
                        
                    <div class="formHolder"  style="padding: 22px 25px 12px;">
                        <div><modaltitle style="text-align:left; font-size: 16px !important;">Contacto</modaltitle></div>
                        <hr class="line">
                        <div class="divFormInLine">
                            <label>
                                Teléfono: 
                                <asp:TextBox ID="txtTelefonoJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="24" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                            </label>
                        </div>
                        <div class="divFormInLine">
                            <label>
                                Email: 
                                <asp:TextBox ID="txtMailJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="25" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                            </label>
                        </div>
                    </div>
                    </section>

                    <section>
                    <asp:UpdatePanel ID="pnlApoderadoJuridica" runat="server">
                        <ContentTemplate>
                            <div class="formHolder"  style="padding: 22px 25px 12px;">
                                <div><modaltitle style="text-align:left; font-size: 16px !important;">Datos apoderado</modaltitle></div>
                                <hr class="line">
                                <div class="divFormInLine">
                                    <div>
                                        <label>
                                            Nombre y Apellido:
                                            <asp:TextBox ID="txtNombreApoderadoJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="26" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                            <asp:RequiredFieldValidator id="rfvNombreApoderadoJuridica" runat="server"
                                                ControlToValidate="txtNombreApoderadoJuridica" CssClass="alert borderValidator" style="width: 220px;"
                                                ErrorMessage="Campo obligatorio"
                                                Validationgroup="validateCliente" Display="Dynamic" SetFocusOnError="True">
                                            </asp:RequiredFieldValidator>
                                        </label> 
                                    </div>
                                </div> 
                                <div class="divFormInLine">
                                    <label>
                                        Cuit: 
                                        <asp:TextBox ID="txtCuitApoderadoJuridica" runat="server" CssClass="textBox textBoxForm cuit" TabIndex="27" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                        <asp:RequiredFieldValidator id="rfvCuitApoderadoJuridica" runat="server"
                                            ControlToValidate="txtCuitApoderadoJuridica" CssClass="alert borderValidator" style="width: 220px;"
                                            ErrorMessage="Campo obligatorio"
                                            Validationgroup="validateCliente" Display="Dynamic" SetFocusOnError="True">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </div> 
                            </div>
                            <div class="formHolder"  style="padding: 22px 25px 12px;">
                                <div><modaltitle style="text-align:left; font-size: 16px !important;">Documento</modaltitle></div>
                                <hr class="line">
                                <div class="divFormInLine">
                                    <div>
                                        <label>
                                            Tipo:
                                            <br />
                                            <asp:DropDownList ID="cbTipoDocApoderadoJuridica" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="28" style="width: 234px; margin-top:2px">
                                                <asp:ListItem Value="1">DNI</asp:ListItem>
                                                <asp:ListItem Value="2">CI</asp:ListItem>
                                                <asp:ListItem Value="3">LC</asp:ListItem>
                                                <asp:ListItem Value="4">LE</asp:ListItem>
                                            </asp:DropDownList>
                                        </label>
                                    </div>
                                </div>
                        
                                <div class="divFormInLine">
                                    <div>
                                        <label>
                                            Número:
                                            <asp:TextBox ID="txtNroDocApoderadoJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="29" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                        </label>
                                    </div>
                                </div>
                            </div>

                            <div class="formHolder"  style="padding: 22px 25px 12px;">
                                <div><modaltitle style="text-align:left; font-size: 16px !important;">Domicilio</modaltitle></div>
                                <hr class="line">
                                <div class="divFormInLine">
                                    <label>
                                        Calle: 
                                        <asp:TextBox ID="txtCalleApoderadoJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="30" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                    </label>
                                </div>
                                <div class="divFormInLine">
                                    <label>
                                        Dirección: 
                                        <asp:TextBox ID="txtDireccionApoderadoJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="31" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                    </label>
                                </div>
                       
                                <div class="divFormInLine">
                                    <label>
                                        Código Postal: 
                                        <asp:TextBox ID="txtCodPostalApoderadoJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="32" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                    </label>
                                </div>
                            </div>

                            <div class="formHolder"  style="padding: 22px 25px 12px;">
                                 <div class="divFormInLine">
                                    <label>
                                        Provincia: 
                                        <asp:DropDownList ID="cbProvinciaApoderadoJuridica" runat="server" CssClass="dropDownList dropDownListForm" TabIndex="33" style="width: 234px; margin-top:2px">
                                            <asp:ListItem Value="1">Capital Federal</asp:ListItem>
                                            <asp:ListItem Value="2">Buenos Aires</asp:ListItem>
                                            <asp:ListItem Value="3">Catamarca</asp:ListItem>
                                            <asp:ListItem Value="4">Córdoba</asp:ListItem>
                                            <asp:ListItem Value="5">Corrientes</asp:ListItem>
                                            <asp:ListItem Value="6">Chaco</asp:ListItem>
                                            <asp:ListItem Value="7">Chubut</asp:ListItem>
                                            <asp:ListItem Value="8">Entre Ríos</asp:ListItem>
                                            <asp:ListItem Value="9">Formosa</asp:ListItem>
                                            <asp:ListItem Value="10">Jujuy</asp:ListItem>
                                            <asp:ListItem Value="11">La Pampa</asp:ListItem>
                                            <asp:ListItem Value="12">La Rioja</asp:ListItem>
                                            <asp:ListItem Value="13">Mendoza</asp:ListItem>
                                            <asp:ListItem Value="14">Misiones</asp:ListItem>
                                            <asp:ListItem Value="15">Neuquén</asp:ListItem>
                                            <asp:ListItem Value="16">Río Negro</asp:ListItem>
                                            <asp:ListItem Value="17">Salta</asp:ListItem>
                                            <asp:ListItem Value="18">San Juan</asp:ListItem>
                                            <asp:ListItem Value="19">San Luis</asp:ListItem>
                                            <asp:ListItem Value="20">Santa Cruz</asp:ListItem>
                                            <asp:ListItem Value="21">Santa Fe</asp:ListItem>
                                            <asp:ListItem Value="22">Santiago del Estero</asp:ListItem>
                                            <asp:ListItem Value="23">Tierra del Fuego</asp:ListItem>
                                            <asp:ListItem Value="24">Tucumán</asp:ListItem>
                                        </asp:DropDownList>
                                    </label>
                                </div>
                                <div class="divFormInLine">
                                    <label>
                                        Ciudad: 
                                        <asp:TextBox ID="txtCiudadApoderadoJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="34" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                    </label>
                                </div>
                            </div>

                            <div class="formHolder"  style="padding: 22px 25px 12px;">
                                <div><modaltitle style="text-align:left; font-size: 16px !important;">Contacto</modaltitle></div>
                                <hr class="line">
                                <div class="divFormInLine">
                                    <label>
                                        Teléfono: 
                                        <asp:TextBox ID="txtTelefonoApoderadoJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="35" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                    </label>
                                </div>
                                <div class="divFormInLine">
                                    <label>
                                        Email: 
                                        <asp:TextBox ID="txtMailApoderadoJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="36" style="width: 220px; margin-top: 2px;"></asp:TextBox>
                                    </label>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </section> 
                    
                    <section>
                        <div class="formHolder"  style="padding: 22px 25px 12px;">
                            <div><modalTitle style="text-align:left">Comentarios</modalTitle></div><hr />
                            <div class="divFormInLine">
                                <div>
                                    <label>
                                        <asp:TextBox ID="txtComentarioJuridica" runat="server" CssClass="textBox textBoxForm" TabIndex="37" TextMode="MultiLine" style="width: 900px; margin-top: 2px;"></asp:TextBox>
                                    </label> 
                                </div>
                            </div>                     
                        </div>
                        <div class="formHolder" style="padding: 8px 25px 12px;">
                            <font style="font-size: 13px; color: red;">Máximo de caracteres: 2500</font>
                        </div>
                    </section>               
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Panel ID="pnlCuit" runat="server" Visible="false">
                <section>
                <div runat="server" class="formHolderAlert alert" style="padding: 14px 25px 12px;">
                    <asp:Label runat="server">El nro. de CUIT es incorrecto</asp:Label>
                </div>
                </section>
            </asp:Panel>
            
            <section>
                <asp:UpdatePanel ID="pnlFinalizarCliente" runat="server">
                    <ContentTemplate>
                        <div runat="server" class="formHolder" style="padding: 22px 14px 12px;">
                            <div runat="server">
                                <div style="float:left">
                                    <label><asp:Button ID="btnFinalizar" Text="Finalizar" CssClass="formBtnNar" runat="server" style="float:left; margin-top: -4px; margin-left: 15px;" Validationgroup="validateCliente" OnClick="btnFinalizar_Click"/></label>
                                </div>
                                <div style="float:right">
                                    <asp:Button ID="btnVolver" Text="Volver a la agenda" CssClass="formBtnGrey1" runat="server" style="float:left; margin-left:15px; margin-top: -5px;" OnClick="btnVolver_Click"/>
                                </div>                            
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </section>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<div style="float:left">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="pnlFinalizarCliente">
        <ProgressTemplate>
            <div class="overlay">
                <div class="overlayContent">
                    <div style="float:left;"><img src="images/ring_loading.gif"  width="300px" class="loading100" ImageAlign="left"  /></div>
                    <div style="float:left; padding: 8px 0 0 10px">
                        <h2> Procesando... </h2>
                    </div>                                    
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
</asp:Content>
