<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="PasswordMgr.aspx.cs" Inherits="labMonitor.PasswordMgr" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<link rel="stylesheet" href="Content/Admin.css" />--%>
    <link href="Content/PasswordMgr.css" rel="stylesheet" type="text/css" />
    
    <h1 style="font-size:40px;">Password Manager</h1>

    <div class="user-search">
        <h2 style="font-size:24px;">Search User</h2>
<asp:TextBox ID="txtUserId" runat="server" placeholder="User ID" />
<asp:RegularExpressionValidator ID="RegExValidatorUserId" runat="server" 
                                ControlToValidate="txtUserId" 
                                ErrorMessage="Please enter a valid numeric User ID." 
                                ValidationExpression="^\d+$" 
                                ForeColor="Red" />
        <asp:TextBox ID="txtFirstName" runat="server" placeholder="First Name" />
        <asp:TextBox ID="txtLastName" runat="server" placeholder="Last Name" />
        <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click" CssClass="button" />
    </div>

    <asp:GridView ID="GridViewUsers" runat="server" AutoGenerateColumns="false" class="stTable" OnRowCommand="User_Command">
        <Columns>
            <asp:BoundField DataField="userID" HeaderText="User ID" DataFormatString="{0:00000000}" />
            <asp:BoundField DataField="userFName" HeaderText="First Name" />
            <asp:BoundField DataField="userLName" HeaderText="Last Name" />
            <asp:BoundField DataField="userDept" HeaderText="Department" />

            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:Button ID="btnResetPassword" runat="server" CommandName="ResetPassword" CommandArgument='<%# Eval("UserId") %>' Text="Reset Password" OnClientClick="return confirm('Are you sure you want to reset the password for this user?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
