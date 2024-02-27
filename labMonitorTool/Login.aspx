<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="labMonitor.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="~/Content/Login.css" rel="stylesheet" type="text/css" />
</head>
<body class="login">
    <form id="form1" runat="server">
        <div class="centered">
            <img src="images/neit-logo.png" />
        </div>
        <div class ="centered">
            <label for="txtUsername">Username:</label>
        </div>
        <div class ="centered">
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
        </div>
        <div class="centered">
            <label for="txtPassword">Password:</label>
        </div>
        <div class ="centered">
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" ></asp:TextBox>
        </div>
        <div class="centered">
            <asp:Label ID="lblFeedback" runat="server" Text=""></asp:Label>
        </div>
        <div class ="centered">
            <asp:Button ID="btnSubmit" runat="server" Text="Login" OnClick="login_Click" />
        </div>
    </form>
</body>
</html>
