using labMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace labMonitor
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login");
            }
        }

        protected void changePassword(object sender, EventArgs e)
        {
            string password = txtPassword.Text;
            string confirm = txtConfirm.Text;
            UserDAL userFactory = new UserDAL();
            lblWarning.Visible = false;

            if (password == confirm)
            {
                if (password.Length < 8)
                {
                    lblWarning.Text = "Password must be at least 8 characters long!";
                    lblWarning.Visible = true;
                }
                else
                {
                    User user = Session["User"] as User;
                    if (userFactory.ChangePassword(user, password))
                    {
                        Session.Abandon();
                        // Write an alert to the page so that they can log back in
                        Response.Write("<script>alert('You must log back in after changing password.');window.location = 'Login.aspx';</script>");
                    }
                    else
                    {
                        lblWarning.Text = "There was a problem changing your password. Try again later.";
                        lblWarning.Visible = true;
                    }

                    //Response.Redirect("Login");
                }

            }
            else
            {
                lblWarning.Text = "Passwords don't match!";
                lblWarning.Visible = true;
            }
        }

    }
}