using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using labMonitor.Models;
using System.Web.UI.WebControls;

namespace labMonitor
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login");
            }
            else
            {
                UserDAL userFactory = new UserDAL();
                var user = Session["User"] as labMonitor.Models.User;
                lblUserName.Text = user.userFName;
                userName.InnerText = user.userFName;
                string avaPath = userFactory.GetPicture(user.userID);
                avatar.ImageUrl = avaPath;
                avaImage.ImageUrl = avaPath;
                switch (user.userPrivilege)
                {
                    case 1:
                        logHistory.Visible = true;
                        schedule.Visible = true;
                        break;
                    case 2:
                        monitor.Visible = true;
                        reports.Visible = true;
                        schedule.Visible = true;
                        break;
                    case 3:
                        monitor.Visible = true;
                        reports.Visible = true;
                        schedule.Visible = true;
                        admin.Visible = true;
                        passwordmgr.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void LogOut(object sender, ImageClickEventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Login");
        }

        protected void GoHome(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Default");
        }

        public void ShowHideBar(object sender, ImageClickEventArgs e)
        {
            avabar.Visible = !avabar.Visible;
        }

        protected void MonitorsEdit(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("MonitorsEdit");
        }

        protected void Reports(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Reports");
        }

        protected void LogHistory(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("LogView");
        }

        protected void Schedule(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Calendar");
        }

        protected void Admin(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Admin");
        }

        protected void PasswordMgr(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("PasswordMgr");
        }
    }
}