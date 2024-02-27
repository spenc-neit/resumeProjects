using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace labMonitor.Models
{
    public class LabDAL
    {
        private string GetConnected()
        {
            return "Server= sql.neit.edu\\studentsqlserver,4500; Database=SE265_LabMonitorProj; User Id=SE265_LabMonitorProj;Password=FaridRyanSpencer;";
        }

        public Lab GetLabByID(int? id)
        {
            Lab lab = new Lab();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "SELECT * FROM Lab WHERE labID = @labID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@labID", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        lab.labID = Convert.ToInt32(rdr["labID"]);
                        lab.labName = rdr["labName"].ToString();
                        lab.labRoom = rdr["labRoom"].ToString();
                        lab.deptHead = Convert.ToInt32(rdr["deptHead"]);
                        lab.deptID = Convert.ToInt32(rdr["deptID"]);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return lab;
        }

        public Lab GetEmployeeLab(int? id)
        {
            Lab lab = new Lab();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "SELECT * FROM Lab WHERE deptHead = @deptHead";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@deptHead", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        lab.labID = Convert.ToInt32(rdr["labID"]);
                        lab.labName = rdr["labName"].ToString();
                        lab.labRoom = rdr["labRoom"].ToString();
                        lab.deptHead = Convert.ToInt32(rdr["deptHead"]);
                        lab.deptID = Convert.ToInt32(rdr["deptID"]);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return lab;
        }

        public Lab GetLabByDeptID(int? id)
        {
            Lab lab = new Lab();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "SELECT * FROM Lab WHERE deptID = @deptID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@deptID", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        lab.labID = Convert.ToInt32(rdr["labID"]);
                        lab.labName = rdr["labName"].ToString();
                        lab.labRoom = rdr["labRoom"].ToString();
                        lab.deptHead = Convert.ToInt32(rdr["deptHead"]);
                        lab.deptID = Convert.ToInt32(rdr["deptID"]);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return lab;
        }

        public List<Lab> GetAllLabs()
        {
            List<Lab> labs = new List<Lab>();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "SELECT * FROM Lab";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Lab lab = new Lab();
                        lab.labID = Convert.ToInt32(rdr["labID"]);
                        lab.labName = rdr["labName"].ToString();
                        lab.labRoom = rdr["labRoom"].ToString();
                        lab.deptHead = Convert.ToInt32(rdr["deptHead"]);
                        lab.deptID = Convert.ToInt32(rdr["deptID"]);

                        labs.Add(lab);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return labs;
        }

        public void AddLab(Lab lab)
        {

            using (SqlConnection connection = new SqlConnection(GetConnected()))
            {
                // Open the database connection
                connection.Open();

                // Define the SQL query to insert a new lab record
                string insertSql = "INSERT INTO Lab (labName, labRoom, deptHead, deptID) VALUES (@labName, @labRoom, @deptHead, @deptID)";

                // Create a new SqlCommand object to execute the insert query
                using (SqlCommand command = new SqlCommand(insertSql, connection))
                {
                    // Set the parameter values for the insert query
                    command.Parameters.AddWithValue("@labName", lab.labName);
                    command.Parameters.AddWithValue("@labRoom", lab.labRoom);
                    command.Parameters.AddWithValue("@deptHead", lab.deptHead);
                    command.Parameters.AddWithValue("@deptID", lab.deptID);

                    // Execute the insert query
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateLab(Lab lab)
        {
            using (SqlConnection connection = new SqlConnection(GetConnected()))
            {
                // Open the database connection
                connection.Open();

                // Define the SQL query to update an existing lab record
                string updateSql = "UPDATE Lab SET labName = @labName, labRoom = @labRoom, deptHead = @deptHead, deptID = @deptID WHERE labID = @labID";

                // Create a new SqlCommand object to execute the update query
                using (SqlCommand command = new SqlCommand(updateSql, connection))
                {
                    // Set the parameter values for the update query
                    command.Parameters.AddWithValue("@labID", lab.labID);
                    command.Parameters.AddWithValue("@labName", lab.labName);
                    command.Parameters.AddWithValue("@labRoom", lab.labRoom);
                    command.Parameters.AddWithValue("@deptHead", lab.deptHead);
                    command.Parameters.AddWithValue("@deptID", lab.deptID);

                    // Execute the update query
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RemoveLab(int labID)
        {
            using (SqlConnection connection = new SqlConnection(GetConnected()))
            {
                // Open the database connection
                connection.Open();
                string deleteSql = "DELETE FROM Lab WHERE labID = @labID";

                // Create a new SqlCommand object to execute the delete query
                using (SqlCommand command = new SqlCommand(deleteSql, connection))
                {
                    // Set the parameter value for the labID to delete
                    command.Parameters.AddWithValue("@labID", labID);

                    // Execute the delete query
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool DepartmentExists(int deptID)
        {
            using (SqlConnection connection = new SqlConnection(GetConnected()))
            {
                // Open the database connection
                connection.Open();

                // Create a SQL query to check if the record has a "deptID" key
                string query = "SELECT COUNT(*) FROM Lab WHERE deptID=@deptID";

                // Create a command object with the query and connection
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add a parameter for the record ID
                    command.Parameters.AddWithValue("@deptID", deptID);

                    // Execute the query and get the result
                    int count = (int)command.ExecuteScalar();

                    // If the count is greater than zero, the record has a "deptID" key
                    if (count > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UserExists(int deptHead)
        {
            using (SqlConnection connection = new SqlConnection(GetConnected()))
            {
                // Open the database connection
                connection.Open();

                // Create a SQL query to check if the record has a "deptID" key
                string query = "SELECT COUNT(*) FROM Lab WHERE deptHead=@deptHead";

                // Create a command object with the query and connection
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add a parameter for the record ID
                    command.Parameters.AddWithValue("@deptHead", deptHead);

                    // Execute the query and get the result
                    int count = (int)command.ExecuteScalar();

                    // If the count is greater than zero, the record has a "deptID" key
                    if (count > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}