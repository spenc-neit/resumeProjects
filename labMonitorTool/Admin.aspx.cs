using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using labMonitor.Models;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace labMonitor
{
    public partial class Admin : System.Web.UI.Page
    {
        // Data access layers to connect with the database
        DepartmentDAL deptFactory = new DepartmentDAL();
        LabDAL labFactory = new LabDAL();
        UserDAL userFactory = new UserDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] != null)
            {
                var user = Session["User"] as labMonitor.Models.User;
                if (user.userPrivilege < 3)
                {
                    Response.Redirect("Default");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        PopulateDropDown();
                    }
                    // Update the page
                    UpdateGrid();

                }
            }
            else
            {
                Response.Redirect("Login");
            }
        }
        private void PopulateDropDown()
        {
            List<Department> departmentList = deptFactory.GetAllDepartments();
            // bind the result to the ComboBox's DataSource property
            comboDept.DataSource = departmentList;

            // set the DataTextField property to the departmentName field
            comboDept.DataTextField = "deptName";

            // set the DataValueField property to the departmentID field
            comboDept.DataValueField = "deptID";

            // call the DataBind() method to bind the data to the ComboBox
            comboDept.DataBind();
        }
        private void UpdateGrid()
        {
            List<Lab> labs = (List<Lab>)labFactory.GetAllLabs();
            // Gather certain properties from the object so it doesn't display all attributes
            DGLabs.DataSource = labs.Select(o => new Lab()
            { labID = o.labID, labName = o.labName, labRoom = o.labRoom}).ToList();
            //DGLabMonitors.DataSource = userFactory.GetMonitorsByDept(user.userDept);
            DGLabs.DataBind();
        }

        protected void ShowAddForm(object sender, EventArgs e)
        {
            action.Value = "Add";
            // H2 tags don't seem to play nicely with razer, so we'll have to manually set it here and in the edit button
            formHeader.InnerText = action.Value + " Lab";
            // Clear forms
            txtLabName.Text = "";
            txtRoom.Text = "";
            txtDept.Text = "";
            comboDept.SelectedIndex = 0;
            // Databind the page so that the action can be updated
            Page.DataBind(); 
            labForm.Visible = true;
        }

        protected void Populate_User(object sender, EventArgs e)
        {
            // Get the values from the selected row in the DataGrid
            string userFName = GridResults.SelectedRow.Cells[2].Text;
            string userLName = GridResults.SelectedRow.Cells[3].Text;
            int userID = Int32.Parse(GridResults.SelectedRow.Cells[1].Text);

            // Set the values of the fields on the page
            txtDept.Text = userFName + " " + userLName;
            selectedID.Value = userID.ToString();
        }

        protected void SearchUsers(object sender, ImageClickEventArgs e)
        {
            String[] searchSplit = txtDept.Text.Split(' ');
            List<User> teachers = new List<User>();
            if (searchSplit.Length >= 2)
            {
                if (searchSplit.Length > 2)
                {
                    for (int i = 2; i < searchSplit.Length; i++)
                    {
                        // Append to the last name search
                        searchSplit[1] += searchSplit[i];
                    }

                    // Remove the remaining items in the array
                    Array.Resize(ref searchSplit, 2);
                }
                teachers = (List<User>)userFactory.SearchTeachersByFullName(searchSplit[0], searchSplit[1]);
            }
            else if(searchSplit.Length == 1)
            {
                teachers = (List<User>)userFactory.SearchTeachersByFullName(searchSplit[0], "");
            }
            // Else, the search criteria is empty, let's query all
            else
            {
                teachers = (List<User>)userFactory.SearchTeachersByFullName("", "");
            }
            // If there are results, then show the datagrid. Else, let's hide it
            if (teachers.Count > 0)
            {
                GridResults.DataSource = teachers.Select(o => new User()
                { userID = o.userID, userFName = o.userFName, userLName = o.userLName }).ToList();

                GridResults.DataBind();
                GridResults.Visible = true;
            }
            else
            {
                GridResults.Visible = false;
            }


        }

        protected void Lab_Command(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveLab")
            {
                string[] parameters = e.CommandArgument.ToString().Split(',');
                int id = Int32.Parse(parameters[0]);
                labFactory.RemoveLab(id);
                UpdateGrid();
            }
            else if (e.CommandName == "EditLab")
            {
                string[] parameters = e.CommandArgument.ToString().Split(',');
                Lab tLab = labFactory.GetLabByID(Int32.Parse(parameters[0]));
                labID.Value = parameters[0];
                txtLabName.Text = tLab.labName;
                txtRoom.Text = tLab.labRoom;
                // Get the department head
                User tUser = userFactory.GetOneUser(tLab.deptHead);
                txtDept.Text = tUser.userFName + " " + tUser.userLName;
                // Hidden fields need to be casted to string for some reason
                selectedID.Value = tUser.userID.ToString();
                comboDept.SelectedValue = tUser.userDept.ToString();
                //comboDept.SelectedIndex = 0;
                action.Value = "Update";
                formHeader.InnerText = action.Value + " Lab";
                // Databind the page so that the action can be updated
                Page.DataBind();
                labForm.Visible = true;
            }
        }

        protected void actionButton_Click(object sender, EventArgs e)
        {
            bool warning = false;
            // Clear all warnings
            lblNameWarning.Visible = false;
            lblRoomWarning.Visible = false;
            lblHeadWarning.Visible = false;
            lblDeptWarning.Visible = false;
            lblNameWarning.Text = "";
            lblRoomWarning.Text = "";
            lblHeadWarning.Text = "";
            lblDeptWarning.Text = "";

            if (txtLabName.Text.Length < 1 || txtLabName.Text.Length > 32)
            {
                warning = true;
                lblNameWarning.Visible = true;
                lblNameWarning.Text = "Lab name must be between [1-32] characters!";
            }
            if (txtRoom.Text.Length < 1 || txtRoom.Text.Length > 16)
            {
                warning = true;
                lblRoomWarning.Visible = true;
                lblRoomWarning.Text = "Lab room# must be between [1-16] characters!";
            }
            if (selectedID.Value == "")
            {
                warning = true;
                lblHeadWarning.Text = "Please select a employee from the list.\nContact admin if employee doesn't show up";
                lblHeadWarning.Visible = true;
            }
            else if (labFactory.UserExists(Int32.Parse(selectedID.Value)))
            {
                Lab tlab = labFactory.GetEmployeeLab(Int32.Parse(selectedID.Value));
                /*
                 * If the user is adding the lab and they already exist, then give error
                 * If the user is updating the lab and they're not the correct user, then give error
                 */
                if (action.Value.ToString() == "Add" || tlab.deptHead != Int32.Parse(selectedID.Value))
                {
                    warning = true;
                    User tUser = userFactory.GetOneUser(Int32.Parse(selectedID.Value));
                    Department tDepartment = deptFactory.GetDeptByID(tUser.userDept);
                    lblHeadWarning.Text = tUser.userFName + " " + tUser.userLName + " is already a department head for " + tDepartment.deptName + " Department, please change them first!";
                    lblHeadWarning.Visible = true;
                }

            }
            int deptID = Int32.Parse(comboDept.SelectedValue);
            if (labFactory.DepartmentExists(deptID))
            {
                Lab tlab = labFactory.GetLabByDeptID(deptID);
                /*
                 * If the user is adding the lab and they already exist, then give error
                 * If the user is updating the lab and they're not the correct user, then give error
                 */
                if (action.Value.ToString() == "Add" || tlab.labID != Int32.Parse(labID.Value))
                {
                    warning = true;
                    lblDeptWarning.Text = "This department already has a lab!";
                    lblDeptWarning.Visible = true;
                }

            }
            if (!warning)
            {
                Lab tLab = new Lab();
                User tUser = userFactory.GetOneUser(Int32.Parse(selectedID.Value));
                tLab.labName = txtLabName.Text;
                tLab.labRoom = txtRoom.Text;
                tLab.deptHead = Int32.Parse(selectedID.Value);
                tLab.deptID = Int32.Parse(comboDept.SelectedValue);
                userFactory.ChangeDept(tUser, deptID);
                if (action.Value.ToString() == "Add")
                {
                    labFactory.AddLab(tLab);
                }
                // Action update
                else
                {
                    tLab.labID = Int32.Parse(labID.Value);
                    labFactory.UpdateLab(tLab);
                }
                UpdateGrid();
                labForm.Visible = false;
            }

        }
    }
}