<%@ Page Language="C#" Title="Lab Monitors" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="MonitorsEdit.aspx.cs" Inherits="labMonitor.MonitorsEdit" %>




<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<div>
    <%--Show when user is department head or admin --%>
    <h2 runat="server" id="welcome">Lab Monitors</h2>
    <div id="headerButton">
        <h3 style="margin:0;">Current Monitors</h3>
        <asp:Button Text="Add new Monitor" runat="server" class="button" style="width: 200px; height: 30px;" OnClick="Show_Form"/>
    </div>
    
    <div class="monitor" runat="server">
        <asp:GridView ID="DGLabMonitors" runat="server" AutoGenerateColumns="false" OnRowCommand="Remove_User" class="stTable" border="0">
        <Columns>
            <asp:BoundField DataField="userID" HeaderText="Student ID" />
            <asp:BoundField DataField="userFName" HeaderText="Name" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="btnRemove" runat="server" CommandName="RemoveUser" CommandArgument='<%#Eval("userID")%>'>
                        <asp:Image ID="X" runat="server" ImageUrl="/images/x.png" class="xbutton" style="border-width: 0px;" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </div>
    <div id="MonitorForm" class="MonitorForm" runat="server" Visible="false">
        <h3 id="formHeader">Add Lab Monitor</h3>
        <label for="txtStudentID">Student ID</label>
        <br />
        <asp:TextBox ID="txtStudentID" runat="server" class="formfield"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
            ControlToValidate="txtStudentID" runat="server"
            ErrorMessage="Not a valid student ID"
            ValidationExpression="\d+" class="warning">
        </asp:RegularExpressionValidator>
        <label for="txtStudentFirst">Student First Name</label>
        <asp:TextBox ID="txtStudentFirst" runat="server" class="formfield"></asp:TextBox>
        <label for="txtStudentLast" style="margin-top:20px;">Student Last Name</label>
        <asp:TextBox ID="txtStudentLast" runat="server" class="formfield"></asp:TextBox>
        <asp:Label ID="lblWarning" CssClass="warning" runat="server" Visible="false"></asp:Label>
        <div id="formButtons">
            <asp:Button class="button popoutButton" OnClick="Search_Users" Text="Search" runat="server" style="border-radius: 20%;"/>
            <asp:Button class="button popoutButton" OnClick="Add_Monitor" Text="Add" runat="server" style="border-radius: 20%;"/>
        </div>
        <asp:GridView ID="GridResults" runat="server" AutoGenerateColumns="false" Visible="false" AutoGenerateSelectButton="true" class="miniTable" OnSelectedIndexChanged="Populate_User">
                <Columns>
            <asp:BoundField DataField="userID" HeaderText="Student ID" />
            <asp:BoundField DataField="userFName" HeaderText="First Name" />
            <asp:BoundField DataField="userLName" HeaderText="Last Name" />
            </Columns>
        </asp:GridView>
    </div>

    <script>
        var e = document.querySelector(".stTable")
        e.removeAttribute("rules")
    </script>
    
</div>

</asp:Content>