using labMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace labMonitor
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void login_Click(object sender, EventArgs e)
        {
            UserDAL userDAL = new UserDAL();
            int userID = 0;
            if (Int32.TryParse(txtUsername.Text, out userID)) // all user names should be a combination of student id or employee id
            {
                string password = txtPassword.Text;
                if (userDAL.ValidateCredentials(userID, password))
                {
                    User user = userDAL.GetOneUser(userID);
                    Session["User"] = user;
                    var user1 = Session["User"] as User;
                    Session["test"] = (User)(Session["User"]);
                    Response.Redirect("Default.aspx");
                }
            }
            lblFeedback.Text = "<p class='warning'>Invalid username or password.</p>";
        }

    }
}