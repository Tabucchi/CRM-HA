<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="crm.Test" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />

    <br /><br /><br /><br />
    <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />

    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>


</asp:Content>
