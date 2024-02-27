using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace labMonitor.Models
{
    public class LogDAL
    {

        //AddLog(objLog Log)
        //This will add a log to the table.

        //ModifyLog(objLog Log)
        //This will modify a log entry in the table.

        //TimeOut(int id)
        //This will modify a log entry’s timeout field to current time

        //GetLogsBetween(datetime start, datetime end, int dept)
        //This will fetch all records between the passed start and end date and filters based on department.


        private string GetConnected()
        {
            return "Server= sql.neit.edu\\studentsqlserver,4500; Database=SE265_LabMonitorProj; User Id=SE265_LabMonitorProj;Password=FaridRyanSpencer;";
        }

        public void AddLog(Log tba) //tba = to be added
        {
            using (SqlConnection conn = new SqlConnection(GetConnected()))
            {
                conn.Open();

                string sql;

                if (tba.timeOut != DateTime.Parse("1/1/0001 12:00:00 AM"))
                {
                    sql = "INSERT INTO Log (studentID, deptID, timeIn, timeOut, itemsBorrowed) VALUES (@paraStudent, @paraDept, @paraIn, @paraOut, @paraItems)";
                }
                else
                {
                    sql = "INSERT INTO Log (studentID, deptID, timeIn, itemsBorrowed) VALUES (@paraStudent, @paraDept, @paraIn, @paraItems)";
                }

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@paraStudent", tba.studentID);
                    comm.Parameters.AddWithValue("@paraDept", tba.deptID);
                    comm.Parameters.AddWithValue("@paraIn", tba.timeIn);
                    if (tba.timeOut != DateTime.Parse("1/1/0001 12:00:00 AM"))
                    {
                        comm.Parameters.AddWithValue("@paraOut", tba.timeOut);
                    }
                    comm.Parameters.AddWithValue("@paraItems", tba.itemsBorrowed);

                    comm.ExecuteNonQuery();
                }
            }
        }

        public void ModifyLog(Log tbu) //tbu = to be updated
        {
            using (SqlConnection conn = new SqlConnection(GetConnected()))
            {
                conn.Open();
                string sql = "UPDATE Log SET studentID = @paraStudent, deptID = @paraDept, timeIn = @paraIn, timeOut = @paraOut, itemsBorrowed = @paraItems WHERE logID = @paraID";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@paraID", tbu.logID);
                    comm.Parameters.AddWithValue("@paraStudent", tbu.studentID);
                    comm.Parameters.AddWithValue("@paraDept", tbu.deptID);
                    comm.Parameters.AddWithValue("@paraIn", tbu.timeIn);
                    comm.Parameters.AddWithValue("@paraOut", tbu.timeOut);
                    comm.Parameters.AddWithValue("@paraItems", tbu.itemsBorrowed);

                    comm.ExecuteNonQuery();
                }
            }
        }

        public void ClockOut(int id) //runs when the clock out button is hit
        {
            using (SqlConnection conn = new SqlConnection(GetConnected()))
            {
                conn.Open();
                string sql = "UPDATE Log SET timeOut = @paraOut WHERE logID = @paraID";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@paraID", id);
                    comm.Parameters.AddWithValue("@paraOut", DateTime.Now);

                    comm.ExecuteNonQuery();
                }
            }
        }

        public List<Log> GetAllLogs(int dept)
        {
            List<Log> results = new List<Log>();

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnected()))
                {
                    string sql = "SELECT logID, studentID, deptID, timeIn, timeOut, itemsBorrowed FROM Log WHERE deptID = @paraDept";
                    SqlCommand comm = new SqlCommand(sql, conn);
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@paraDept", dept);
                    conn.Open();
                    SqlDataReader rdr = comm.ExecuteReader();

                    while (rdr.Read())
                    {
                        UserDAL getUser = new UserDAL();
                        Log temp = new Log();
                        temp.logID = Convert.ToInt32(rdr["logID"]);
                        temp.studentID = Convert.ToInt32(rdr["studentID"].ToString());
                        var test = temp.studentID;
                        User grabbed = getUser.GetOneUser(temp.studentID);
                        string tempNameContainer = grabbed.userFName + " " + grabbed.userLName;
                        temp.studentName = tempNameContainer;
                        temp.deptID = Convert.ToInt32(rdr["deptID"].ToString());
                        temp.timeIn = Convert.ToDateTime(rdr["timeIn"].ToString());
                        if (rdr["timeOut"].ToString() != "")
                        {
                            temp.timeOut = Convert.ToDateTime(rdr["timeOut"].ToString());
                        }
                        temp.itemsBorrowed = rdr["itemsBorrowed"].ToString();

                        results.Add(temp);
                    }
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return results;
        }

        public List<Log> GetLogsBetween(DateTime start, DateTime end, int dept)
        {
            List<Log> results = new List<Log>();

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnected()))
                {
                    string sql = "SELECT logID, studentID, deptID, timeIn, timeOut, itemsBorrowed FROM Log WHERE (timeIn BETWEEN @paraStart AND @paraEnd) AND deptID = @paraDept";
                    SqlCommand comm = new SqlCommand(sql, conn);
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@paraDept", dept);
                    comm.Parameters.AddWithValue("@paraStart", start);
                    comm.Parameters.AddWithValue("@paraEnd", end);
                    conn.Open();
                    SqlDataReader rdr = comm.ExecuteReader();

                    while (rdr.Read())
                    {
                        UserDAL getUser = new UserDAL();
                        Log temp = new Log();
                        temp.logID = Convert.ToInt32(rdr["logID"]);
                        temp.studentID = Convert.ToInt32(rdr["studentID"].ToString());
                        var test = temp.studentID;
                        User grabbed = getUser.GetOneUser(temp.studentID);
                        string tempNameContainer = grabbed.userFName + " " + grabbed.userLName;
                        temp.studentName = tempNameContainer;
                        temp.deptID = Convert.ToInt32(rdr["deptID"].ToString());
                        temp.timeIn = Convert.ToDateTime(rdr["timeIn"].ToString());
                        if (rdr["timeOut"].ToString() != "")
                        {
                            temp.timeOut = Convert.ToDateTime(rdr["timeOut"].ToString());
                        }
                        temp.itemsBorrowed = rdr["itemsBorrowed"].ToString();

                        results.Add(temp);
                    }
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return results;
        }

        public Log GetALog(int id)
        {
            Log result = new Log();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnected()))
                {
                    string sql = "SELECT * FROM Log WHERE logID = @logID";
                    SqlCommand comm = new SqlCommand(sql, conn);
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@logID", id);
                    conn.Open();
                    SqlDataReader rdr = comm.ExecuteReader();
                    while (rdr.Read())
                    {
                        result.logID = Convert.ToInt32(rdr["logID"]);
                        result.studentID = Convert.ToInt32(rdr["studentID"].ToString());
                        //result.studentName = rdr["studentName"].ToString();
                        result.deptID = Convert.ToInt32(rdr["deptID"].ToString());
                        result.timeIn = Convert.ToDateTime(rdr["timeIn"].ToString());
                        result.timeOut = Convert.ToDateTime(rdr["timeOut"].ToString());
                        result.itemsBorrowed = rdr["itemsBorrowed"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("----EXCEPTION----");
                System.Diagnostics.Debug.WriteLine(e);
                System.Diagnostics.Debug.WriteLine("----END EXCEPTION----");
            }
            return result;
        }
    }


}