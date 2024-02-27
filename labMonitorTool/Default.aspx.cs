using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using labMonitor.Models;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using OfficeOpenXml;
using System.Globalization;

namespace labMonitor
{
    public partial class _Default : Page
    {
        DepartmentDAL departmentFactory = new DepartmentDAL();
        ScheduleDAL scheduleFactory = new ScheduleDAL();
        LabDAL labFactory = new LabDAL();
        UserDAL userFactory = new UserDAL();
        LogDAL logFactory = new LogDAL();

        private string GetConnected()
        {
            return "Server= sql.neit.edu\\studentsqlserver,4500; Database=SE265_LabMonitorProj; User Id=SE265_LabMonitorProj;Password=FaridRyanSpencer;";
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // Получаем данные из базы данных
            DateTime startDate = DateTime.Now.AddDays(-7); // начало периода
            DateTime endDate = DateTime.Now; // конец периода
            int departmentID = 0; // идентификатор отдела
            LogDAL logFactory = new LogDAL();
            List<Log> logs = logFactory.GetLogsBetween(startDate, endDate, departmentID);

            // Создаем новую серию данных
            Series series = new Series("Student Count");
            series.ChartType = SeriesChartType.Column;

            // Добавляем данные в серию
            for (int i = 0; i < 7; i++)
            {
                DateTime date = startDate.AddDays(i);
                int count = logs.Count(log => log.timeIn.Date == date.Date);
                series.Points.AddXY(date.ToString("dddd"), count);
            }

            // Добавляем серию в график
            Chart1.Series.Clear();
            Chart1.Series.Add(series);

            // Set gradient colors
            series.Color = Color.FromArgb(128, 0, 136, 204);
            series.BackGradientStyle = GradientStyle.TopBottom;
            series.BackSecondaryColor = Color.FromArgb(128, 0, 206, 255);

            // Настраиваем график
            Chart1.ChartAreas[0].AxisX.Interval = 1;
            Chart1.ChartAreas[0].AxisX.Title = "Day of Week";
            Chart1.ChartAreas[0].AxisY.Title = "Number of Students";
            Chart1.Width = 800;
            Chart1.Height = 600;
            Chart1.BackColor = Color.WhiteSmoke;
            Chart1.BorderlineDashStyle = ChartDashStyle.Solid;
            Chart1.BorderlineColor = Color.FromArgb(198, 198, 198);
            Chart1.BorderlineWidth = 3;
            Chart1.Titles.Add("Student Check In/Out Report");

            Chart1.Visible = true;









            if (Session["User"] != null)
            {
                var user = Session["User"] as labMonitor.Models.User;
                welcome.InnerText = "Welcome, " + user.userFName;
                if (user.userPrivilege < 2)
                {
                    head.Visible = false;
                }
                if (user.userPrivilege == 0) // do this in a switch case once everyone finishes their view
                {
                    if (!IsPostBack)
                    {
                        studentview.Visible = true;
                        List<Lab> labs = labFactory.GetAllLabs();

                        foreach (Lab lab in labs)
                        {
                            scheduleLiteral.Text += "<div class='labcard'>";
                            scheduleLiteral.Text += "<div class='htags'>\n <h3 class='lbn'>" + lab.labName + "</h3><h3 class='rn'>" + lab.labRoom + "</h3></div>";
                            scheduleLiteral.Text += "<div class='cardcontent'>";
                            scheduleLiteral.Text += "<div class='schcard'>";
                            string[] operatingHours = scheduleFactory.GetDeptSchedule(lab.deptID).Split(',');
                            string[] daysOfWeek = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
                            for (int i = 0; i < daysOfWeek.Length; i++)
                            {
                                scheduleLiteral.Text += "<p>" + daysOfWeek[i] + ": " + Helpers.FormatOperatingHours(operatingHours[i]) + "</p>";
                            }
                            scheduleLiteral.Text += "</div>";
                            scheduleLiteral.Text += "<div class='imgbk'> <img src='images/image 39.png' /></div>";
                            scheduleLiteral.Text += "</div></div>";
                        }
                    }

                }

                if (user.userPrivilege == 1)
                {
                    submitButton.Text = "Submit";
                    monitorview.Visible = true;
                    permissionCheck.InnerText = user.userPrivilege.ToString();

                    UpdateGrid();
                }
            }
            else
            {
                Response.Redirect("Login");
            }


        }

        private static DateTime GetDateForDayOfWeek(string dayOfWeek, DateTime baseDate)
        {
            int daysUntilDayOfWeek = ((int)Enum.Parse(typeof(DayOfWeek), dayOfWeek) - (int)baseDate.DayOfWeek + 7) % 7;
            return baseDate.AddDays(daysUntilDayOfWeek).Date;
        }

        public static DateTime StartOfDay(DateTime theDate)
        {
            return theDate.Date;
        }

        public static DateTime EndOfDay(DateTime theDate)
        {
            return theDate.Date.AddDays(1).AddTicks(-1);
        }

        private void UpdateGrid()
        {
            var user = Session["User"] as User;
            var dept = user.userDept;
            var sod = StartOfDay(DateTime.Now);
            var eod = EndOfDay(DateTime.Now);
            List<Log> logs = (List<Log>)logFactory.GetLogsBetween(sod, eod, dept);
            // Gather certain properties from the object so it doesn't display all attributes
            DGlogs.DataSource = logs.Select(o => new Log()
            { logID = o.logID, studentName = o.studentName, studentID = o.studentID, timeIn = o.timeIn, timeOut = o.timeOut }).ToList();
            DGlogs.DataBind();
        }

        protected void LogsCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ClockOut")
            {
                string[] parameters = e.CommandArgument.ToString().Split(',');
                int id = Int32.Parse(parameters[0]);
                logFactory.ClockOut(id);
                UpdateGrid();
            }
            else if (e.CommandName == "EditLog")
            {
                string[] parameters = e.CommandArgument.ToString().Split(',');
                Log tLog = logFactory.GetALog(Int32.Parse(parameters[0]));
                logID.Value = parameters[0];
                User tUser = userFactory.GetOneUser(tLog.studentID);
                txtStudentID.Text = tLog.studentID.ToString();
                txtStudentName.Text = tUser.userFName + " " + tUser.userLName;
                dtTimeIn.Value = tLog.timeIn.TimeOfDay.ToString();
                dtTimeOut.Value = tLog.timeOut.TimeOfDay.ToString();
                txtItems.Text = tLog.itemsBorrowed;
                selectedID.Value = tUser.userID.ToString();
                action.Value = "Update";
                formHeader.InnerText = action.Value + " Log";
                Page.DataBind();
                logForm.Visible = true;

            }
        }

        protected void submitButton_Click(object sender, EventArgs e)
        {
            bool warning = false;
            lblIDWarning.Visible = false;
            lblIDWarning.Text = "";
            lblInWarning.Visible = false;
            lblInWarning.Text = "";
            lblItemsWarning.Visible = false;
            lblItemsWarning.Text = "";
            lblNameWarning.Visible = false;
            lblNameWarning.Text = "";
            lblOutWarning.Visible = false;
            lblOutWarning.Text = "";

            if (txtStudentID.Text.Length != 9)
            {
                warning = true;
                lblIDWarning.Visible = true;
                lblIDWarning.Text = "ID numbers are 9 characters long.";
            }
            if (txtStudentName.Text.Length < 1)
            {
                warning = true;
                lblNameWarning.Visible = true;
                lblNameWarning.Text = "Name is required.";
            }
            if(dtTimeIn.Value == "")
            {
                warning = true;
                lblInWarning.Visible = true;
                lblInWarning.Text = "Time in is required.";
            }
            if (!warning)
            {
                User tUser = Session["User"] as User;
                Log tLog = new Log();
                tLog.studentID = Int32.Parse(txtStudentID.Text);
                tLog.studentName = txtStudentName.Text;
                tLog.deptID = tUser.userDept;
                tLog.timeIn = DateTime.Parse(dtTimeIn.Value);
                tLog.itemsBorrowed = txtItems.Text;
                if (action.Value == "Add")
                {
                    var temp = DateTime.Parse(dtTimeIn.Value);
                    var tempSpan = new TimeSpan(temp.Hour, temp.Minute, 0);
                    tLog.timeIn = DateTime.Now.Date + tempSpan;

                    if (dtTimeOut.Value != "")
                    {
                        temp = DateTime.Parse(dtTimeOut.Value);
                        tempSpan = new TimeSpan(temp.Hour, temp.Minute, 0);
                        tLog.timeOut = DateTime.Now.Date + tempSpan;
                    }

                    if (!userFactory.doesUserExist(tLog.studentID))
                    {
                        User newUser = new User();
                        var monitor = Session["User"] as User;
                        var nameList = txtStudentName.Text.Split(' ');
                        newUser.userDept = monitor.userDept;
                        newUser.userFName = nameList[0];
                        newUser.userLName = nameList[1];
                        newUser.userPassword = "Hx/xnGxwOSB4lq1DVtqAo6sUTzU=";
                        newUser.userSalt = "/W5B3ypqGFeSRXeB5vNhDA==";
                        newUser.userPrivilege = 0;
                        newUser.userID = tLog.studentID;
                        userFactory.addNonexistentUser(newUser);
                    }

                    logFactory.AddLog(tLog);

                }
                else if (action.Value == "Update")
                {
                    tLog.logID = Int32.Parse(logID.Value);
                    if (dtTimeOut.Value != "")
                    {
                        var temp = DateTime.Parse(dtTimeOut.Value);
                        var tempSpan = new TimeSpan(temp.Hour, temp.Minute, 0);
                        tLog.timeOut = DateTime.Now.Date + tempSpan;
                    }
                    logFactory.ModifyLog(tLog);
                }
                UpdateGrid();
                logForm.Visible = false;
            }

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            action.Value = "Add";
            formHeader.InnerText = action.Value + " Lab";
            txtStudentID.Text = "";
            txtStudentName.Text = "";
            dtTimeIn.Value = "";
            dateTimeIn.SelectedDate = DateTime.Now;
            dtTimeOut.Value = "";
            dateTimeOut.SelectedDate = DateTime.Now;
            txtItems.Text = "";
            Page.DataBind();
            logForm.Visible = true;
        }


        /*
        public static void CreateScheduleDiv(List<string> schedules, HtmlGenericControl parentDiv)
        {
            string[] daysOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            for (int i = 0; i < schedules.Count; i++)
            {
                string operatingHours = GetOperatingHours(schedules[i]);

                // Create the cardcontent div
                HtmlGenericControl cardContentDiv = new HtmlGenericControl("div");
                cardContentDiv.Attributes["class"] = "cardcontent";

                // Create the schcard div
                HtmlGenericControl schCardDiv = new HtmlGenericControl("div");
                schCardDiv.Attributes["class"] = "schcard";

                // Create the p elements for each day of the week
                for (int j = 0; j < daysOfWeek.Length; j++)
                {
                    HtmlGenericControl dayP = new HtmlGenericControl("p");
                    dayP.InnerHtml = daysOfWeek[j] + ": " + operatingHours.Split(',')[j];
                    schCardDiv.Controls.Add(dayP);
                }

                // Add the schcard div to the cardcontent div
                cardContentDiv.Controls.Add(schCardDiv);

                // Add the cardcontent div to the parent div
                parentDiv.Controls.Add(cardContentDiv);
            }

        }        */
    }
}