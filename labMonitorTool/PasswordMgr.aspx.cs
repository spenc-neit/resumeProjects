using labMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace labMonitor
{
    public partial class PasswordMgr : System.Web.UI.Page
    {
        //DALs to connect with the database
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
            }
        }
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string id = txtUserId.Text;
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;

            UserDAL userFactory = new UserDAL(); // Assuming UserDAL is the class with the methods

            if (!string.IsNullOrEmpty(id))
            {
                int userId;
                if (int.TryParse(id, out userId))
                {
                    var user = userFactory.GetOneUser(userId);
                    GridViewUsers.DataSource = new List<User> { user };
                }
                else
                {
                    // Handle the case where ID is not a valid integer
                    // Maybe display an error message
                }
            }
            else
            {
                var users = userFactory.SearchUsersByFullName(firstName, lastName);
                GridViewUsers.DataSource = users;
            }

            GridViewUsers.DataBind();
        }

        protected void UsersGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewUsers.EditIndex = e.NewEditIndex;
            // Rebind data to GridView
        }

        protected void UsersGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get User ID from the GridView
            int userID = Convert.ToInt32(GridViewUsers.DataKeys[e.RowIndex].Value);

            // Get updated data from GridView and update user details
            // Call methods from UserDAL to update user permission and password

            GridViewUsers.EditIndex = -1;
            // Rebind data to GridView
        }

        protected void UsersGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewUsers.EditIndex = -1;
            // Rebind data to GridView
        }

        protected void User_Command(object sender, GridViewCommandEventArgs e)
        {
            int userID = Convert.ToInt32(e.CommandArgument); // Assuming userID is the CommandArgument

            switch (e.CommandName)
            {
                case "EditPermissions":
                    // Extract the new permission level from somewhere, e.g., a control in the GridView
                    int newPermissionLevel = 0;
                    userFactory.ChangePermission(userID, newPermissionLevel);
                    break;

                case "ResetPassword":                    
                    User user = userFactory.GetOneUser(userID); // Get the User object for the specified userID
                    userFactory.ChangePassword(user, "password");
                    break;

                    // Handle other commands if needed
            }

            // Optionally rebind your GridView to reflect any changes
            // BindGridViewData();
        }



    }
}