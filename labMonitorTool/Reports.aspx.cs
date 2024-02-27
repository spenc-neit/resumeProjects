using labMonitor.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Globalization;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using OfficeOpenXml;
using System.Linq;

namespace labMonitor
{
    public partial class Reports : System.Web.UI.Page
    {
        LogDAL factory = new LogDAL();
        User user = new User();
        
        private string GetConnected()
        {
            return "Server= sql.neit.edu\\studentsqlserver,4500; Database=SE265_LabMonitorProj; User Id=SE265_LabMonitorProj;Password=FaridRyanSpencer;";
        }

        private string Filept()
        {
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(downloadsPath, "myfile.csv");

            return filePath;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            user = Session["User"] as labMonitor.Models.User;
            // Connection string for your database
            string connectionString = GetConnected();
            if (!IsPostBack)
            {
                // SQL query to select data from table
                string query = "SELECT DISTINCT YEAR(timein) AS Year FROM Log ORDER BY Year ASC";
                string countQuery = "SELECT YEAR(timein) AS Year, COUNT(DISTINCT studentID) AS 'Student Count' FROM Log WHERE timein IS NOT NULL GROUP BY YEAR(timein)";

                // Create SQL connection and command objects
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    // Create SqlDataAdapter and DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    // Fill DataTable with data from SQL
                    adapter.Fill(dataTable);

                    // Get the min and max year values
                    int minYear = Convert.ToInt32(dataTable.Rows[0]["Year"]);
                    int maxYear = Convert.ToInt32(dataTable.Rows[dataTable.Rows.Count - 1]["Year"]);

                    // Create a new DataTable to hold the final results
                    DataTable finalTable = new DataTable();
                    finalTable.Columns.Add("Year", typeof(int));
                    finalTable.Columns.Add("Student Count", typeof(int));

                    // Create SQL command to get student count for each year
                    command = new SqlCommand(countQuery, connection);
                    adapter.SelectCommand = command;
                    dataTable.Clear();
                    adapter.Fill(dataTable);

                    // Fill in the missing years with 0 counts
                    for (int year = minYear; year <= maxYear; year++)
                    {
                        DataRow[] rows = dataTable.Select("Year = " + year);
                        int count = 0;
                        if (rows.Length > 0)
                        {
                            count = Convert.ToInt32(rows[0]["Student Count"]);
                        }
                        finalTable.Rows.Add(year, count);
                    }

                    // Bind chart to DataTable
                    Chart1.DataSource = finalTable;
                    Chart1.DataBind();

                    Series series = new Series();

                    // Set gradient colors
                    series.Color = Color.FromArgb(128, 0, 136, 204);
                    series.BackGradientStyle = GradientStyle.TopBottom;
                    series.BackSecondaryColor = Color.FromArgb(128, 0, 206, 255);

                    // Set ChartArea background color and border style
                    Chart1.ChartAreas[0].BackColor = Color.White;
                    Chart1.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
                    Chart1.ChartAreas[0].BorderColor = Color.Black;

                    // Set Series color, border width, and label format
                    Chart1.Series[0].Color = Color.FromArgb(128, 0, 136, 204);
                    Chart1.Series[0].BorderWidth = 5;
                    Chart1.Series[0].LabelFormat = "#";

                    // Set XValueMember and YValueMember for the series
                    Chart1.Series[0].XValueMember = "Year";
                    Chart1.Series[0].YValueMembers = "Student Count";

                    // Format AxisX to show all years and display total number of people
                    Chart1.ChartAreas[0].AxisX.Minimum = minYear;
                    Chart1.ChartAreas[0].AxisX.Maximum = maxYear;
                    Chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Years;
                    Chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy";
                    Chart1.ChartAreas[0].AxisX.Interval = 1;
                    Chart1.ChartAreas[0].AxisX.Title = "Year";
                    Chart1.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 18, FontStyle.Bold);
                    Chart1.ChartAreas[0].AxisX.TitleForeColor = Color.Black;

                    // Format AxisY to display number of people
                    Chart1.ChartAreas[0].AxisY.Title = "Number of People";
                    Chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 18, FontStyle.Bold);
                    Chart1.ChartAreas[0].AxisY.TitleForeColor = Color.Black;

                    Chart1.Width = 800;
                    Chart1.Height = 600;
                    Chart1.BackColor = Color.WhiteSmoke;
                    Chart1.BorderlineDashStyle = ChartDashStyle.Solid;
                    Chart1.BorderlineColor = Color.FromArgb(198, 198, 198);
                    Chart1.BorderlineWidth = 3;
                    Chart1.Titles.Add("Student Check In/Out Report");

                }

            }

        }

        private List<Log> GetLogs()
        {
            List<Log> logs = new List<Log>();
            DateTime dt;
            switch (schsel.SelectedIndex)
            {
                // Day
                case 0:
                    // Get selected date from input
                    if (dtpc.Text != "")
                    {
                        dt = DateTime.ParseExact(dtpc.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        logs = factory.GetLogsBetween(dt, dt.Date.AddDays(1), user.userDept);
                    }
                    break;
                // Week
                case 1:
                    // Set the parameters for the start and end of the week
                    String[] weekText = wkpc.Text.Split('-');
                    if (DateTime.TryParseExact(weekText[0], "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        int weeksToAdvance = Int32.Parse(weekText[1].Replace("W", ""));
                        logs = factory.GetLogsBetween(dt, dt.AddDays(weeksToAdvance * 7), user.userDept);
                    }
                    break;
                // Month
                case 2:
                    dt = DateTime.ParseExact(mtpc.Text, "yyyy-MM", CultureInfo.InvariantCulture);
                    logs = factory.GetLogsBetween(dt, dt.AddMonths(1).AddDays(-1), user.userDept);
                    break;
                // Term
                case 3:
                    if (stday.Text != "" && endday.Text != "")
                    {
                        dt = DateTime.ParseExact(stday.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime end = DateTime.ParseExact(endday.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        logs = factory.GetLogsBetween(dt, end, user.userDept);
                    }
                    break;
                // All
                default:
                    logs = factory.GetAllLogs(user.userDept);
                    break;
            }
            return logs;
        }

        protected void dtpc_OnDateChanged(object sender, EventArgs e)
        {
            DateTime dt = DateTime.ParseExact(dtpc.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime startTime = dt.Date.AddHours(9);
            DateTime endTime = dt.Date.AddHours(21);

            List<Log> logs = GetLogs();

            // Group the logs by hour and count the number of logs for each hour within the specified range
            var logCounts = logs.GroupBy(log => log.timeIn.Hour)
                                .Select(g => new { Hour = g.Key, Count = g.Count() })
                                .OrderBy(x => x.Hour) // Sort by hour for good measure
                                .ToList();

            // Create a new list with the data in the correct format for the chart's Points.DataBind() method
            List<object> data = new List<object>();
            foreach (var item in logCounts)
            {
                data.Add(new { Hour = dt.Date.AddHours(item.Hour), Count = item.Count });
            }

            // Create a new series for the hourly data and bind the data to it
            Series hourlySeries = new Series("Hourly Count");
            hourlySeries.Points.DataBind(data, "Hour", "Count", "");
            hourlySeries.Color = Color.FromArgb(128, 0, 136, 204);
            hourlySeries.BackGradientStyle = GradientStyle.TopBottom;
            hourlySeries.BackSecondaryColor = Color.FromArgb(128, 0, 206, 255);
            Chart1.Series.Clear();
            Chart1.Series.Add(hourlySeries);

            // Set the axis titles
            Chart1.ChartAreas["ChartArea1"].AxisX.Title = "Time";
            Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Count";


            // Настраиваем график
            Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            Chart1.ChartAreas[0].AxisX.Interval = 1;
            Chart1.ChartAreas[0].AxisX.Title = "Hours";
            Chart1.ChartAreas[0].AxisX.LabelStyle.Format = "h tt";
            Chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            Chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Hours;
            Chart1.ChartAreas[0].AxisX.Minimum = startTime.ToOADate();
            Chart1.ChartAreas[0].AxisX.Maximum = endTime.ToOADate();
            Chart1.ChartAreas[0].AxisY.Title = "Count of Students";
            Chart1.Width = 800;
            Chart1.Height = 600;
            Chart1.BackColor = Color.WhiteSmoke;
            Chart1.BorderlineDashStyle = ChartDashStyle.Solid;
            Chart1.BorderlineColor = Color.FromArgb(198, 198, 198);
            Chart1.BorderlineWidth = 3;
            Chart1.Titles.Add("Student Check In/Out Report");

            Chart1.DataBind();
        }


        protected void wkpc_OnDateChanged(object sender, EventArgs e)
        {
            string[] weekText = wkpc.Text.Split('-');
            DateTime dt;
            if (DateTime.TryParseExact(weekText[0], "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                int weeksToAdvance = Int32.Parse(weekText[1].Replace("W", ""));
                List<Log> logs = factory.GetLogsBetween(dt, dt.AddDays(weeksToAdvance * 7), 0);

                // Group the logs by date and count the number of logs for each date
                var logCounts = logs.GroupBy(log => log.timeIn.Date)
                                    .Where(g => g.Key >= dt && g.Key <= dt.AddDays(weeksToAdvance * 7))
                                    .OrderBy(g => g.Key)
                                    .Select(g => new { DayOfWeek = g.Key.ToString("dddd"), Count = g.Count() });

                // Bind the log counts to the chart series
                Chart1.Series["Student Count"].Points.DataBind(logCounts, "DayOfWeek", "Count", "");

                // Set the axis titles
                Chart1.ChartAreas["ChartArea1"].AxisX.Title = "Day Of The Week";
                Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Count";
                Chart1.DataBind();
                Series series = new Series();
                // Set gradient colors
                Chart1.Series[0].Color = Color.FromArgb(128, 0, 136, 204);
                series.BackGradientStyle = GradientStyle.TopBottom;
                series.BackSecondaryColor = Color.FromArgb(128, 0, 206, 255);



                Chart1.Width = 800;
                Chart1.Height = 600;
                Chart1.BackColor = Color.WhiteSmoke;
                Chart1.BorderlineDashStyle = ChartDashStyle.Solid;
                Chart1.BorderlineColor = Color.FromArgb(198, 198, 198);
                Chart1.BorderlineWidth = 3;
                Chart1.Titles.Add("Student Check In/Out Report");
            }
        }

        protected void mtpc_OnDateChanged(object sender, EventArgs e)
        {
            DateTime dt = DateTime.ParseExact(mtpc.Text, "yyyy-MM", CultureInfo.InvariantCulture);
            List<Log> logs = factory.GetLogsBetween(dt, dt.AddMonths(1).AddDays(-1), user.userDept);

            // Get all days of the selected month
            var allDays = Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month))
                                     .Select(day => new DateTime(dt.Year, dt.Month, day));

            // Group the logs by date and count the number of logs for each date
            var logCounts = allDays.GroupJoin(logs, day => day.Date, log => log.timeIn.Date,
                                              (day, logsForDay) => new { Day = day, Count = logsForDay.Count() })
                                   .Select(d => new { DayOfMonth = d.Day.Day, Count = d.Count });

            // Bind the log counts to the chart series
            Chart1.Series["Student Count"].Points.DataBind(logCounts, "DayOfMonth", "Count", "");

            // Set the axis titles
            Chart1.ChartAreas["ChartArea1"].AxisX.Title = "Days of the Month";
            Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Count";

            // Set the X axis labels format
            Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "d";
            Chart1.Series[0].Color = Color.FromArgb(128, 0, 136, 204);
            Chart1.Width = 800;
            Chart1.Height = 600;
            Chart1.BackColor = Color.WhiteSmoke;
            Chart1.BorderlineDashStyle = ChartDashStyle.Solid;
            Chart1.BorderlineColor = Color.FromArgb(198, 198, 198);
            Chart1.BorderlineWidth = 3;
            Chart1.Titles.Add("Student Check In/Out Report");

            Chart1.DataBind();
        }

        protected void stday_OnDateChanged(object sender, EventArgs e)
        {
            // Check if both stday and endday have valid date values
            if (!string.IsNullOrEmpty(stday.Text) && !string.IsNullOrEmpty(endday.Text))
            {
                DateTime dt = DateTime.ParseExact(stday.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime end = DateTime.ParseExact(endday.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                List<Log> logs = factory.GetLogsBetween(dt, end, user.userDept);

                int logCount = logs.Count();
                string labelFormat = "dd/MM/yyyy";

                if (logCount <= 15)
                {
                    // Show dates with format "dd/MM/yyyy"
                    var logCounts = logs.GroupBy(log => log.timeIn.Date)
                                        .Select(g => new { Date = g.Key.ToString(labelFormat), Count = g.Count() })
                                        .OrderBy(x => x.Date) // Sort by hour for good measure
                                                    .ToList();
                    Chart1.Series["Student Count"].Points.DataBind(logCounts, "Date", "Count", "");

                    // Set the axis labels format
                    Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = labelFormat;
                }
                else
                {
                    TimeSpan timeSpan = end - dt;
                    if (timeSpan.TotalDays <= 105)
                    {
                        // Show dates by week
                        labelFormat = "dd/MM";
                        var logCounts = logs.GroupBy(log => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(log.timeIn.Date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday))
                                            .Select(g => new { Date = "Week " + g.Key, Count = g.Count() });

                        Chart1.Series["Student Count"].Points.DataBind(logCounts, "Date", "Count", "");

                        // Set the axis labels format
                        Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = labelFormat;
                    }
                    else if (timeSpan.TotalDays <= 5475)
                    {
                        // Show dates by month
                        labelFormat = "MMM yyyy";
                        var logCounts = logs.GroupBy(log => new DateTime(log.timeIn.Year, log.timeIn.Month, 1))
                                            .Select(g => new { Date = g.Key.ToString(labelFormat), Count = g.Count() });

                        Chart1.Series["Student Count"].Points.DataBind(logCounts, "Date", "Count", "");

                        // Set the axis labels format
                        Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = labelFormat;
                    }
                    else
                    {
                        // Show dates by year
                        labelFormat = "yyyy";
                        var logCounts = logs.GroupBy(log => new DateTime(log.timeIn.Year, 1, 1))
                                            .Select(g => new { Date = g.Key.ToString(labelFormat), Count = g.Count() });

                        Chart1.Series["Student Count"].Points.DataBind(logCounts, "Date", "Count", "");

                        // Set the axis labels format
                        Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = labelFormat;
                    }
                }
                Series series = new Series();
                // Set gradient colors
                
                series.BackGradientStyle = GradientStyle.TopBottom;
                series.BackSecondaryColor = Color.FromArgb(128, 0, 206, 255);
                Chart1.Series[0].Color = Color.FromArgb(128, 0, 136, 204);
                // Set the axis titles
                Chart1.ChartAreas["ChartArea1"].AxisX.Title = "Date";
                Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Count";

                // Set the X axis labels format
                Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "d";

                Chart1.Width = 1300;
                Chart1.Height = 600;
                Chart1.BackColor = Color.WhiteSmoke;
                Chart1.BorderlineDashStyle = ChartDashStyle.Solid;
                Chart1.BorderlineColor = Color.FromArgb(198, 198, 198);
                Chart1.BorderlineWidth = 3;
                Chart1.Titles.Add("Student Check In/Out Report");


                Chart1.DataBind();
            }
        }




        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {

            switch (schsel.SelectedIndex)
            {
                case 0: // Day
                    dtpc.Visible = true;
                    wkpc.Visible = false;
                    mtpc.Visible = false;
                    stday.Visible = false;
                    endday.Visible = false;

                    

                    break;
                case 1: // Week
                    dtpc.Visible = false;
                    wkpc.Visible = true;
                    mtpc.Visible = false;
                    stday.Visible = false;
                    endday.Visible = false;

                    

                    break;
                case 2: // Month
                    dtpc.Visible = false;
                    wkpc.Visible = false;
                    mtpc.Visible = true;
                    stday.Visible = false;
                    endday.Visible = false;

                    
                    break;
                case 3: // Term
                    dtpc.Visible = false;
                    wkpc.Visible = false;
                    mtpc.Visible = false;
                    stday.Visible = true;
                    endday.Visible = true;

                    

                    break;
                case 4: // All
                    dtpc.Visible = false;
                    wkpc.Visible = false;
                    mtpc.Visible = false;
                    stday.Visible = false;
                    endday.Visible = false;

                    
                    break;
                default:
                    dtpc.Visible = false;
                    wkpc.Visible = false;
                    mtpc.Visible = false;
                    stday.Visible = false;
                    endday.Visible = false;
                    break;
            }
        }

        protected void ExportData(object sender, EventArgs e)
        {
            List<Log> logs = new List<Log>();
            // File path for CSV file
            string filePath = Filept();
            // DT oject to store dates

            DateTime dt = new DateTime();
            switch (schsel.SelectedIndex)
            {
                // Day
                case 0:
                    // Get selected date from input
                    dt = DateTime.ParseExact(dtpc.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    logs = factory.GetLogsBetween(dt, dt.Date.AddDays(1), user.userDept);

                    break;
                // Week
                case 1:
                    // Set the parameters for the start and end of the week
                    String[] weekText = wkpc.Text.Split('-');
                    if (DateTime.TryParseExact(weekText[0], "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        int weeksToAdvance = Int32.Parse(weekText[1].Replace("W", ""));
                        logs = factory.GetLogsBetween(dt, dt.AddDays(weeksToAdvance * 7), user.userDept);
                    }
                    break;
                // Month
                case 2:
                    dt = DateTime.ParseExact(mtpc.Text, "yyyy-MM", CultureInfo.InvariantCulture);
                    logs = factory.GetLogsBetween(dt, dt.AddMonths(1).AddDays(-1), user.userDept);
                    break;
                // Term
                case 3:
                    dt = DateTime.ParseExact(stday.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime end = DateTime.ParseExact(endday.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    logs = factory.GetLogsBetween(dt, end, user.userDept);
                    break;
                // All
                default:
                    logs = factory.GetAllLogs(user.userDept);
                    break;
            }
            switch (dpfltp.SelectedIndex)
            {
                // PDF
                case 0:
                    break;
                // CSV
                case 1:
                    // Create new file and open for writing
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        // Write column headers to CSV file
                        writer.WriteLine("studentID,timein,timeout,itemsBorrowed");

                        for (int i = 0; i < logs.Count; i++)
                        {
                            Log log = logs[i];
                            // Write values to CSV file
                            writer.WriteLine("{0},{1},{2},{3}", log.studentID, log.timeIn.ToString("MM/dd/yyyy HH:mm:ss"), log.timeOut.ToString("MM/dd/yyyy HH:mm:ss"), log.itemsBorrowed);
                        }
                        // Close CSV file
                        writer.Close();
                    }

                    // Set response headers
                    Response.Clear();
                    Response.ContentType = "text/csv";
                    Response.AddHeader("content-disposition", "attachment;filename=report.csv");

                    // Write CSV file contents to response stream
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    Response.BinaryWrite(fileBytes);
                    Response.Flush();
                    Response.End();
                    break;
                // XLXS
                case 2:
                    // See: https://epplussoftware.com/developers/licenseexception
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    // Create a new Excel package
                    using (var package = new ExcelPackage())
                    {
                        // Create a new worksheet
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                        // Write the headers
                        worksheet.Cells[1, 1].Value = "studentID";
                        worksheet.Cells[1, 2].Value = "Time In";
                        worksheet.Cells[1, 3].Value = "Time Out";
                        worksheet.Cells[1, 4].Value = "Items Borrowed";

                        // Write the data
                        for (int i = 0; i < logs.Count; i++)
                        {
                            worksheet.Cells[i + 2, 1].Value = logs[i].studentID;
                            worksheet.Cells[i + 2, 2].Value = logs[i].timeIn.ToString("MM/dd/yyyy HH:mm:ss");
                            worksheet.Cells[i + 2, 3].Value = logs[i].timeOut.ToString("MM/dd/yyyy HH:mm:ss");
                            worksheet.Cells[i + 2, 4].Value = logs[i].itemsBorrowed;
                        }

                        // Save the file
                        var stream = new MemoryStream();
                        package.SaveAs(stream);
                        byte[] data = stream.ToArray();

                        // Download the file
                        Response.Clear();
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("Content-Disposition", "attachment; filename=report.xlsx");
                        Response.BinaryWrite(data);
                        Response.End();
                    }
                    break;
                default:
                    break;
            }

        }
    }
}