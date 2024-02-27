using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace labMonitor.Models
{
    public class ScheduleDAL
    {
        private string GetConnected()
        {
            return "Server= sql.neit.edu\\studentsqlserver,4500; Database=SE265_LabMonitorProj; User Id=SE265_LabMonitorProj;Password=FaridRyanSpencer;";
        }

        public void GenerateSchedule(User user)
        {
            string textSchedule = "off,off,off,off,off,off,off";
            using (SqlConnection con = new SqlConnection(GetConnected()))
            {
                string sql = "INSERT INTO schedule (student_ID, text_Schedule, dept_ID) VALUES (@student_ID, @text_Schedule, @dept_ID)";
                try
                {
                    using (SqlCommand command = new SqlCommand(sql, con))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@student_ID", user.userID);
                        command.Parameters.AddWithValue("@dept_ID", user.userDept);
                        command.Parameters.AddWithValue("@text_Schedule", textSchedule);
                        con.Open();

                        command.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public String GetUserSchedule(User user)
        {
            string timeSchedule = "off,off,off,off,off,off,off";
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "SELECT * FROM Schedule WHERE student_ID = @userID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@userID", user.userID);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read()) // if the schedule doesn't exist, then generate it
                    {
                        timeSchedule = rdr["text_Schedule"].ToString();
                        return timeSchedule;
                    }
                    else
                    {
                        GenerateSchedule(user); // user doesn't have a schedule, so generate it for the db
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return timeSchedule; // this will return if the schedule doesn't exist for the user, it'll generate the schedule in the else clause and return a blank schedule back to Calendar.aspx
        }


        public void SetUserSchedule(User user, string timeSchedule)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "UPDATE Schedule SET text_Schedule = @text_Schedule WHERE student_ID = @userID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@userID", user.userID);
                    cmd.Parameters.AddWithValue("@text_Schedule", timeSchedule);
                    cmd.CommandText = strSQL;
                    cmd.CommandType = CommandType.Text;
                    // fill parameters with form values

                    // perform the update
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static string GetOperatingHours(List<string> schedules)
        {
            string[] operatingHours = new string[7];

            for (int dayIndex = 0; dayIndex < 7; dayIndex++)
            {
                List<string> hoursList = new List<string>();
                foreach (string schedule in schedules)
                {
                    string[] scheduleSplit = schedule.Split(',');
                    string hours = scheduleSplit[dayIndex].Trim();
                    if (hours != "off")
                    {
                        hoursList.Add(hours);
                    }
                }
                if (hoursList.Count > 0)
                {
                    // Convert each string in the hourList to DateTime objects representing the start and end time
                    List<DateTime[]> shifts = hoursList.Select(s =>
                    {
                        string[] parts = s.Split('-');
                        DateTime start = DateTime.ParseExact(parts[0], "HH:mm", CultureInfo.InvariantCulture);
                        DateTime end = DateTime.ParseExact(parts[1], "HH:mm", CultureInfo.InvariantCulture);
                        return new DateTime[] { start, end };
                    }).ToList();

                    // Find the earliest start time and latest end time across all shifts
                    DateTime earliestStart = shifts.Min(shift => shift[0]);
                    DateTime latestEnd = shifts.Max(shift => shift[1]);

                    // Combine the earliest start and latest end time as a string in the format "HH:mm-HH:mm"
                    operatingHours[dayIndex] = earliestStart.ToString("HH:mm") + "-" + latestEnd.ToString("HH:mm"); ;
                }
                else
                {
                    operatingHours[dayIndex] = "off";
                }
            }

            return string.Join(",", operatingHours);
        }

        public string GetDeptSchedule(int dept)
        {
            string operatingSchedule = "off,off,off,off,off,off,off";
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "SELECT * FROM Schedule WHERE dept_ID = @dept_ID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@dept_ID", dept);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    List<string> dept_schedule = new List<string>();
                    // Loop through everyone's schedule in the department
                    while (rdr.Read())
                    {
                        dept_schedule.Add(rdr["text_Schedule"].ToString());
                    }
                    operatingSchedule = GetOperatingHours(dept_schedule);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return operatingSchedule; // this will return if the schedule doesn't exist for the user, it'll generate the schedule in the else clause and return a blank schedule back to Calendar.aspx
        }
    }
}