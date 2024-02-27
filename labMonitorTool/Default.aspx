<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="labMonitor._Default" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"  
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %> 


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="/Content/DefaultM.css" />

<div>
    <h2 runat="server" id="welcome"></h2>
    <%--Show when user is department head or admin --%>
    <div id="head" runat="server">
    <asp:Chart ID="Chart1" runat="server">
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
                <AxisX Title="Year" Interval="1"></AxisX>
                <AxisY Title="Number of Students"></AxisY>
            </asp:ChartArea>
        </ChartAreas>
        <Series>
            <asp:Series Name="Student Count" ChartType="Column"></asp:Series>
        </Series>
    </asp:Chart>    
    </div>
    <div id="monitor" runat="server">

    </div>

            <%-- Head View--%>
           <div class="headview" >
               <h1></h1>

           </div>
           
           <%-- Admin View--%>
           <div class="adminview">
               <h1></h1>

           </div>

           <%-- Monitor View--%>
            <div id="monitorview" class="monitorview" runat="server" visible="false">
                
                <p id="permissionCheck" runat="server" style="display:none;"></p>
               
               <div id="headerButton">
                    <h3 style="margin:0;">Logs for today</h3>
                    <asp:Button OnClick="btnAdd_Click" Text="Add new log" runat="server" class="button newStudentButton" />
               </div>


               <div id="gridviewDiv">
                <asp:GridView ID="DGlogs" runat="server" AutoGenerateColumns="false" OnRowCommand="LogsCommand" class="gridview" border="0">
                    <Columns>
                        <asp:BoundField DataField="logID" HeaderText ="log ID" Visible="false" />
                        <asp:BoundField DataField="studentName" HeaderText="Student Name" />
                        <asp:BoundField DataField="studentID" HeaderText="ID" />
                        <asp:BoundField DataField="timeIn" HeaderText="Time In" />
                        <asp:BoundField DataField="timeOut" HeaderText="Time Out" />
                        <%--<% if (dtTimeIn.Value == null) { %>--%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton id="btnClockOut" runat="server" CommandName="ClockOut" CommandArgument='<%#Eval("logID")%>'>
                                        <asp:Image ID="imgClockOut" runat="server" ImageUrl="/images/clockout.png" class="xbutton" style="border-width:0px;" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                         <%--<%} %>--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnPencil" runat="server" CommandName="EditLog" CommandArgument='<%#Eval("logID")%>'>
                                    <asp:Image ID="imgPencil" runat="server" ImageUrl="/images/pencil icon.png" class="xbutton" style="border-width:0px;" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
               </asp:GridView>
               </div>


               <div runat="server" id="logForm" visible="false" class="MonitorForm">
                   <asp:HiddenField ID="action" runat="server" />
                   <asp:HiddenField ID="selectedID" runat="server" />
                   <asp:HiddenField ID="logID" runat="server" />
                   <h2 id="formHeader" runat="server">Log</h2>

                   <label for="txtStudentID" class="formlabel">Student ID*</label>
                   <br />
                   <asp:TextBox ID="txtStudentID" runat="server" class="formfield"></asp:TextBox>
                   <asp:Label ID="lblIDWarning" runat="server" class="warning" Visible="false"></asp:Label>
                    
                   <br />
                   <label for="txtStudentName" class="formlabel">Student Name*</label>
                   <asp:TextBox ID="txtStudentName" runat="server" class="formfield"></asp:TextBox>
                   <asp:Label ID="lblNameWarning" runat="server" class="warning" Visible="false"></asp:Label>
                   
                   <br />
                   <label for="dtTimeIn" class="formlabel">Time In*</label>
                   <input type="time" class="formcontrol" id="dtTimeIn" runat="server" />
                   <asp:Calendar Visible="false" ID="dateTimeIn" runat="server" />
                   <asp:Label ID="lblInWarning" runat="server" class="warning" Visible="false"></asp:Label>
                   
                   <br />
                   <label for="dtTimeOut" class="formlabel" >Time Out</label>
                   <input type="time" class="formcontrol out" id="dtTimeOut" runat="server" />
                   <asp:Calendar Visible="false" ID="dateTimeOut" runat="server" />
                   <asp:Label ID="lblOutWarning" runat="server" class="warning" Visible="false"></asp:Label>
                   
                   <br />
                   <label for="txtItems" class="formlabel">Items the student borrowed</label>
                   <asp:TextBox ID="txtItems" runat="server" class="formfield"></asp:TextBox>
                   <asp:Label ID="lblItemsWarning" runat="server" class="warning" Visible="false"></asp:Label>

                   <div class="formBottom formfield formlabel">

                        <p class="requiredText"><i>* required fields</i></p>
                   
                        <asp:Button runat="server" ID="submitButton" class="button popoutButton" OnClick="submitButton_Click"/>
                   
                   </div>
                </div>

           </div> <%--end of monitorview--%>

               <%-- Student View--%>
           <div class="studentview" runat="server" id="studentview" visible="false">
               <h1>Dashboard</h1>
               <div class="dbgrid">
                   <asp:Literal runat="server" ID="scheduleLiteral"></asp:Literal>
               </div>
           </div>
    </div>

    

    <script>
        //if monitor. did this in case my JS would mess up other pages
        if (document.querySelector("#MainContent_permissionCheck").innerText == 1) {
            var txtID = document.querySelector('#MainContent_txtStudentID')
            if (txtID != null) {
                var txtIDValue = txtID.getAttribute("value")
                if (txtIDValue != "" && txtIDValue != null) {
                    txtID.setAttribute('value', txtIDValue.padStart(9, "0"))
                }
            }
            var timeIn = document.querySelectorAll('td:nth-child(3)')
            var timeOut = document.querySelectorAll("td:nth-child(4)")
            var outButtons = document.querySelectorAll("td:nth-child(5) a")
            var ids = document.querySelectorAll('td:nth-child(2)')
            for (let i = 0; i < timeOut.length; i++) {
                if (timeOut[i].innerText == "1/1/0001 12:00:00 AM") {
                    timeOut[i].innerText = "--"
                }
                else {
                    outButtons[i].setAttribute("style", "visibility:hidden;")
                    fullOutDate = timeOut[i].innerText.split(' ')
                    timeOut[i].innerText = `${fullOutDate[1]} ${fullOutDate[2]}`
                }
                fullInDate = timeIn[i].innerText.split(' ')
                timeIn[i].innerText = `${fullInDate[1]} ${fullInDate[2]}`
                ids[i].innerText = ids[i].innerText.padStart(9, '0')
            }
        }
        
    </script>
</asp:Content>
