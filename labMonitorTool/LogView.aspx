<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LogView.aspx.cs" Inherits="labMonitor.LogHistory" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="/Content/LogView.css" />

    <h3 style="font-size:40px;">Log History</h3>

    <div class="calAndButton">
        <input type="date" ID="calSearch" runat="server" value="<%#DateTime.Today %>"/>
        <asp:Button ID="searchButton" Text="Search" runat="server" OnClick="searchButton_Click" class="button buttonSearch" />
    </div>

               <asp:GridView ID="DGlogs" runat="server" AutoGenerateColumns="false" OnRowCommand="LogsCommand" class="historyTable">
                    <Columns>
                        <asp:BoundField DataField="logID" HeaderText ="log ID" Visible="false" />
                        <asp:BoundField DataField="studentName" HeaderText="Student Name" />
                        <asp:BoundField DataField="studentID" HeaderText="ID" />
                        <asp:BoundField DataField="timeIn" HeaderText="Time In" />
                        <asp:BoundField DataField="timeOut" HeaderText="Time Out" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnPencil" runat="server" CommandName="EditLog" CommandArgument='<%#Eval("logID")%>'>
                                    <asp:Image ID="imgPencil" runat="server" ImageUrl="/images/pencil icon.png" class="xbutton" style="border-width:0px;" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
               </asp:GridView>
               <h4 id="logs404" runat="server">No logs found for this date.</h4>

    <div runat="server" id="logForm" visible="false" class="MonitorForm">
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
                   <label for="dtTimeOut" class="formlabel">Time Out</label>
                   <input type="time" class="formcontrol out" id="dtTimeOut" runat="server" />
                   <asp:Calendar Visible="false" ID="dateTimeOut" runat="server" />
                   <asp:Label ID="lblOutWarning" runat="server" class="warning" Visible="false"></asp:Label>
                   
                   <br />
                   <label for="txtItems" class="formlabel">Items the student borrowed</label>
                   <asp:TextBox ID="txtItems" runat="server" class="formfield"></asp:TextBox>
                   <asp:Label ID="lblItemsWarning" runat="server" class="warning" Visible="false"></asp:Label>
                   

                    <div class="formBottom formfield formlabel">

                        <p class="requiredText"><i>* required fields</i></p>

                        <asp:Button runat="server" ID="submitButton" OnClick="submitButton_Click" class="button popoutButton" />
                    </div>

                </div>


        


    <script>
        var txtID = document.querySelector('#MainContent_txtStudentID')
        if (txtID != null)
        {
            var txtIDValue = txtID.getAttribute("value")
            if (txtIDValue != "") {
                txtID.setAttribute('value', txtIDValue.padStart(9, "0"))
            }
        }
        var timeIn = document.querySelectorAll('.historyTable td:nth-child(3)')
        var timeOut = document.querySelectorAll(".historyTable td:nth-child(4)")
        var ids = document.querySelectorAll('.historyTable td:nth-child(2)')
        for (let i = 0; i < timeOut.length; i++) {
            if (timeOut[i].innerText == "1/1/0001 12:00:00 AM") {
                timeOut[i].innerText = "--"
            }
            else {
                fullOutDate = timeOut[i].innerText.split(' ')
                timeOut[i].innerText = `${fullOutDate[1]} ${fullOutDate[2]}`
            }
            fullInDate = timeIn[i].innerText.split(' ')
            timeIn[i].innerText = `${fullInDate[1]} ${fullInDate[2]}`
            ids[i].innerText = ids[i].innerText.padStart(9, '0')
        }
    </script>

</asp:Content>
