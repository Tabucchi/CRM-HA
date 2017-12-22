<%@ Page Language="C#" AutoEventWireup="true" Inherits="Login" Codebehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>HA:CRM</title>
<link href='http://fonts.googleapis.com/css?family=Lato:300italic,300,900' rel='stylesheet' type='text/css'/>
<link href='http://fonts.googleapis.com/css?family=Open+Sans+Condensed:300,300italic,700' rel='stylesheet' type='text/css'/>
<link rel="stylesheet" type="text/css" href="css/masterStyle.css">
</head>
<body >
    <form id="form1" runat="server">
<div id="loginTop">
	<div id="loginContent">
		<img class="loginLogo" src="images/logoLogin.png"/>
		<h2>Sistema de gestión y administración de clientes</h2>
		<p>Accediendo a HA CRM contará con un complejo conjunto de herramientas eficaces para el control de sus empresa y su relación con sus abonados.</p>
		<ul id="crmOptions">
			<li><img src="images/login_11.png" class="optionsIcon">
				<p><strong>Tickets</strong>Seguimientos y gestión de pedidos.</p></li>
			<li><img src="images/login_13.png" class="optionsIcon">
				<p><strong>Agenda</strong>Organización de clientes y contactos.</p></li>
			<li><img src="images/login_15.png" class="optionsIcon">
				<p><strong>Usuarios</strong>Cuentas y permisos de distintos niveles. </p></li>
		</ul>
	</div>
    <div id="loginForm">
    	<span>Login</span>
    	<label><input type="text" id="nombreUsuario" runat="server" tabindex="1" placeholder="Nombre de Usuario"/></label>
    	<label><input type="password" id="pass" runat="server" tabindex="2" placeholder="Contraseña"/></label>
    	<label class="btnLabel">
        <asp:Button Text="Go!" class="loginButtom" runat="server" ID="btnlogin"  onclick="btnLogin_Click" />
        </label>
        
    <asp:Panel ID="mensajeError" runat="server" Visible="false">
    <strong> Las credenciales son incorrectas. </strong><br /> Por favor, intente de nuevo
    </asp:Panel>
    </div>
</div>
<div id="loginBottom"> 
	<div class="loginTweets">
		
	</div>
	<div id="loginFooter">http://crmha.naex.com.ar © 2016</div>
</div>
</form>
</body>
</html>