using System;
using System.Web;
using TechFixV3._0Client.AuthenticationServiceReference;

namespace TechFixV3._0Client
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear the error message when the page is first loaded
            if (!IsPostBack)
            {
                MessageLabel.Text = "";
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordTextBox.Text;

            // Basic validation for empty input fields
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageLabel.Text = "Username and password cannot be empty.";
                return;
            }

            // Using block ensures that the AuthenticationServiceSoapClient is disposed of properly
            using (AuthenticationServiceSoapClient authService = new AuthenticationServiceSoapClient())
            {
                try
                {
                    // Call the web service and validate login credentials
                    var loginResult = authService.Login(username, password);

                    if (loginResult != null && loginResult.UserId > 0)
                    {
                        // Store login details using cookies
                        SetUserCookies(loginResult);

                        // Redirect the user based on their role
                        RedirectUser(loginResult.Role);
                    }
                    else
                    {
                        MessageLabel.Text = "Invalid username or password. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    // Log error details
                    LogError(ex);
                    MessageLabel.Text = "An unexpected error occurred. Please try again later.";
                }
            }
        }

        // Method to set user information in cookies
        private void SetUserCookies(LoginResult loginResult)
        {
            HttpCookie userIdCookie = new HttpCookie("UserId", loginResult.UserId.ToString());
            HttpCookie roleCookie = new HttpCookie("Role", loginResult.Role);

            // Set cookies to expire in 1 hour
            userIdCookie.Expires = DateTime.Now.AddHours(1);
            roleCookie.Expires = DateTime.Now.AddHours(1);

            // Add cookies to the response
            Response.Cookies.Add(userIdCookie);
            Response.Cookies.Add(roleCookie);
        }

        // Method to redirect user based on their role
        private void RedirectUser(string role)
        {
            switch (role)
            {
                case "Admin":
                    Response.Redirect("~/Admin/AdminDashboard.aspx");
                    break;
                case "Supplier":
                    Response.Redirect("~/Supplier/SupplierDashboard.aspx");
                    break;
                default:
                    MessageLabel.Text = "User role not recognized.";
                    break;
            }
        }

        // Method to log errors for troubleshooting
        private void LogError(Exception ex)
        {
            // Log the error to debug or to a file for troubleshooting
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
    }
}
