<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="labMonitor.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="~/Content/ChangePW.css" rel="stylesheet" type="text/css" />
</head>


<body class="login">
    <div class="centered">
        <img src="images/logo (1).png" id="neitlogo" />
    </div>
    <h1 class="centered" id="lblCPW">Change Password</h1>
    <form id="form1" runat="server">
      <div class="start">
          <label for="txtPassword">New Password</label>
      </div>
      <div class="centered">
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
      </div>
      <div class="start">
          <label id="cmpw" for="txtConfirm">Confirm Password</label>
      </div>
      <div class="centered">
        <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password"></asp:TextBox>
      </div>
      <div class="centered">
          <asp:Label ID="lblWarning" CssClass="warning" runat="server" Visible="false"></asp:Label>
          <br />
        <asp:Button ID="btnSubmit" runat="server" Text="Change" OnClick="changePassword" />
      </div>
    </form>
</body>
</html>
