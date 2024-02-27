using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using labMonitor.Models;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace labMonitor
{
    public partial class Calendar : System.Web.UI.Page
    {
        public Color color = Color.White;
        List<string> daysOfWeek = new List<string>()
            {
                "Sunday",
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday"
            };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User"] != null)
                {
                    var user = Session["User"] as labMonitor.Models.User;
                    if (user.userPrivilege < 1)
                    {
                        Response.Redirect("Login");
                    }
                }
                else
                {
                    Response.Redirect("Login");
                }
                ScheduleGrid.DataSource = GenerateGrid();
                ScheduleGrid.DataBind();
                var test = Session["User"] as labMonitor.Models.User;
                permission.InnerText = test.userPrivilege.ToString();
                // Hack to pass variables to Javascript so that the event listener can check if the user is leaving before they publish the schedule
                //ClientScript.RegisterClientScriptBlock(GetType(), "isEdited", "var isEdited = false;", false);
                isEdited.Value = "false";
                //isEdited

                if(test.userPrivilege < 2)
                {
                    calPublish.Visible = false;
                }
            }
        }

        /*
         * Format the data columns. This has to be done programatically since we cannot just connect it to a table from a db
         */
        private DataTable GenerateGrid()
        {
            UserDAL userFactory = new UserDAL();
            ScheduleDAL scheduleFactory = new ScheduleDAL();
            var user = Session["User"] as labMonitor.Models.User;
            List<User> monitors = (List<User>)userFactory.GetMonitorsByDept(user.userDept); // get all the users for the department to populate them on the datagrid

            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("studentName", typeof(string));
            dt.Columns.Add(dc);
            // Format the header columns
            foreach (String day in daysOfWeek)
            {
                dc = new DataColumn(day, typeof(string));
                dt.Columns.Add(dc);
            }
            foreach (User monitor in monitors)
            {
                DataRow dr = dt.NewRow();
                String schedule = scheduleFactory.GetUserSchedule(monitor);
                dr["studentName"] = monitor.userFName + " " + monitor.userLName;
                String[] splitSchedule = schedule.Split(',');
                for (int i = 0; i < splitSchedule.Length; i++)
                {
                    dr[daysOfWeek[i]] = splitSchedule[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /*
         * This function gets the cell value given the row index and column index
         */
        protected string GetCellValue(int rowIndex, int colIndex)
        {
            // Check if the row index is valid
            if (rowIndex < 0 || rowIndex >= ScheduleGrid.Items.Count)
                return "";

            // Get the row from the DataGrid
            var row = ScheduleGrid.Items[rowIndex];

            // Ignore the header column
            var cell = row.Cells[colIndex + 1];
            // Find the LinkButton control in the cell control
            var linkButton = cell.Controls.OfType<LinkButton>().FirstOrDefault();

            // Get the text of the LinkButton control
            if (linkButton != null)
            {
                var text = linkButton.Text;
                return text;
            }
            return "";
        }

        protected void SetDataGridCellText(int rowIndex, int colIndex, string value)
        {
            // Check if the row index is valid
            if (rowIndex < 0 || rowIndex >= ScheduleGrid.Items.Count)
                return;
            ScheduleGrid.Items[rowIndex].Cells[colIndex + 1].Controls.OfType<LinkButton>().FirstOrDefault().Text = value;
        }


        private void PopulateForm(int row, int col)
        {
            // Set up factories
            UserDAL userFactory = new UserDAL();
            ScheduleDAL scheduleFactory = new ScheduleDAL();
            var user = Session["User"] as labMonitor.Models.User;
            lblWarning.Visible = false;
            List<User> monitors = (List<User>)userFactory.GetMonitorsByDept(user.userDept); // get all the users for the department to populate them on the datagrid
            ScheduleForm.Visible = true;
            lblStudent.InnerText = monitors[row].userFName + " " + monitors[row].userLName;
            lblDay.InnerText = "Schedule for:" + daysOfWeek[col];
            string daySchedule = GetCellValue(row, col);
            if (daySchedule == "off")
            {
                start.Value = "00:00";
                end.Value = "00:00";
            }
            else
            {
                String[] splitSchedule = daySchedule.Split('-');
                start.Value = splitSchedule[0];
                end.Value = splitSchedule[1];
            }
            coords.Value = String.Format("{0},{1}", row, col);
        }

        protected void OnSelectedCell(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "GetCellValue")
            {
                // Get the row index and column index from the CommandArgument
                string[] args = e.CommandArgument.ToString().Split(',');
                int rowIndex = int.Parse(args[0]);
                int colIndex = int.Parse(args[1]);
                PopulateForm(rowIndex, colIndex);
            }
        }


        protected void Submit(object sender, EventArgs e)
        {
            lblWarning.Visible = false;
            String[] coordinates = coords.Value.Split(',');
            String timeIn = start.Value;
            String timeOut = end.Value;
            var dt_timeIn = DateTime.ParseExact(timeIn, "H:mm", null, System.Globalization.DateTimeStyles.None);
            var dt_timeOut = DateTime.ParseExact(timeOut, "H:mm", null, System.Globalization.DateTimeStyles.None);
            if (dt_timeIn > dt_timeOut)
            {
                lblWarning.Text = "Start time cannot be after end time.";
                lblWarning.Visible = true;
            }
            else if (dt_timeIn == dt_timeOut)
            {
                lblWarning.Text = "Start time and end time cannot be the same! Click on remove if you'd like to make the user off";
                lblWarning.Visible = true;
            }
            else
            {
                if (checkRepeat.Checked)
                {
                    // Set the schedule for Monday-Friday
                    for (int i = 0; i < 5; i++) 
                    {
                        SetDataGridCellText(Int32.Parse(coordinates[0]), i + 1, timeIn + "-" + timeOut);
                    }
                }
                else
                {
                    SetDataGridCellText(Int32.Parse(coordinates[0]), Int32.Parse(coordinates[1]), timeIn + "-" + timeOut);
                }

                //ClientScript.RegisterClientScriptBlock(GetType(), "isEdited", "var isEdited = true;", true);
                isEdited.Value = "true";
            }

        }

        protected void Remove(object sender, EventArgs e)
        {
            String[] coordinates = coords.Value.Split(',');
            SetDataGridCellText(Int32.Parse(coordinates[0]), Int32.Parse(coordinates[1]), "off");
            //ClientScript.RegisterClientScriptBlock(GetType(), "isEdited", "var isEdited = true;", true);
            isEdited.Value = "true";
        }

        protected void Publish(object sender, EventArgs e)
        {
            UserDAL userFactory = new UserDAL();
            ScheduleDAL scheduleFactory = new ScheduleDAL();
            var user = Session["User"] as labMonitor.Models.User;
            List<User> monitors = (List<User>)userFactory.GetMonitorsByDept(user.userDept); // get all the users for the department to populate them on the datagrid
            // loop through all the monitors and update their schedule
            for (int i = 0; i < monitors.Count; i++)
            {
                string tSchedule = "";
                for (int j = 0; j < 7; j++)
                {
                    tSchedule += GetCellValue(i, j) + ",";
                }
                scheduleFactory.SetUserSchedule(monitors[i], tSchedule.Substring(0, tSchedule.Length - 1)); // strip the last comma off from the string
                //ClientScript.RegisterClientScriptBlock(GetType(), "isEdited", "var isEdited = false;", false);
                isEdited.Value = "false";
                //Console.WriteLine(tSchedule);
            }
        }
    }
}