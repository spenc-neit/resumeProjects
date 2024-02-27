using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using labMonitor.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Web.UI.HtmlControls;

namespace labMonitor
{
    public partial class LogHistory : System.Web.UI.Page
    {
        LogDAL logFactory = new LogDAL();
        UserDAL userFactory = new UserDAL();

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
            if (calSearch.Value == "")
            {
                calSearch.Value = DateTime.Today.ToString();
            }
            logs404.Visible = false;
            var dept = user.userDept;
            var sod = StartOfDay(DateTime.Parse(calSearch.Value));
            var eod = EndOfDay(DateTime.Parse(calSearch.Value));
            List<Log> logs = (List<Log>)logFactory.GetLogsBetween(sod, eod, dept);
            DGlogs.DataSource = logs.Select(o => new Log()
            {
                logID = o.logID,
                studentName = o.studentName,
                studentID = o.studentID,
                timeIn = o.timeIn,
                timeOut = o.timeOut
            }).ToList();
            DGlogs.DataBind();
            if (logs.Count == 0)
            {
                logs404.Visible = true;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = Session["User"] as User;
            if (Session["User"] != null && user.userPrivilege == 1)
            {
                submitButton.Text = "Submit";
                UpdateGrid();
            }
            else
            {
                Response.Redirect("Login");
            }
        }

        protected void LogsCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditLog")
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
                formHeader.InnerText = "Update Log";
                //calFilter.SelectedDate = tLog.timeIn.Date;
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
            if (!warning)
            {
                User tUser = Session["User"] as User;
                Log tLog = new Log();
                tLog.studentID = Int32.Parse(txtStudentID.Text);
                tLog.studentName = txtStudentName.Text;
                tLog.deptID = tUser.userDept;
                tLog.timeIn = DateTime.Parse(dtTimeIn.Value);
                tLog.itemsBorrowed = txtItems.Text;
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

        protected void searchButton_Click(object sender, EventArgs e)
        {
            UpdateGrid();
        }
    }
}
