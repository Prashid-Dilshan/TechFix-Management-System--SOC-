using System;
using System.Web;

namespace TechFixV3._0Client.Supplier
{
    public partial class SupplierMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Code for Page Load (if any)
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            // Clear the session to log the user out
            Session.Clear();
            Session.Abandon();

            // Clear cookies by setting their expiration date to a past time
            ClearCookies();

            // Redirect to the login page after logging out
            Response.Redirect("~/Login.aspx");
        }

        // Method to clear cookies
        private void ClearCookies()
        {
            if (Request.Cookies["UserId"] != null)
            {
                HttpCookie userIdCookie = new HttpCookie("UserId");
                userIdCookie.Expires = DateTime.Now.AddDays(-1); // Set to a past date to expire the cookie
                Response.Cookies.Add(userIdCookie);
            }

            if (Request.Cookies["Username"] != null)
            {
                HttpCookie usernameCookie = new HttpCookie("Username");
                usernameCookie.Expires = DateTime.Now.AddDays(-1); // Set to a past date to expire the cookie
                Response.Cookies.Add(usernameCookie);
            }

            if (Request.Cookies["Role"] != null)
            {
                HttpCookie roleCookie = new HttpCookie("Role");
                roleCookie.Expires = DateTime.Now.AddDays(-1); // Set to a past date to expire the cookie
                Response.Cookies.Add(roleCookie);
            }
        }
    }
}
