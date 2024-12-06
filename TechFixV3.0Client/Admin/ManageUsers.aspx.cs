using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFixV3._0Client.UserServiceReference;

namespace TechFixV3._0Client.Admin
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        private UserServiceSoapClient userService = new UserServiceSoapClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUserGrid();
            }
        }

        private void BindUserGrid(string searchUsername = "")
        {
            // Fetch users and filter based on search query if provided
            var users = userService.GetUsers();

            if (!string.IsNullOrEmpty(searchUsername))
            {
                users = users.Where(u => u.Username.IndexOf(searchUsername, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();
            }

            UsersGridView.DataSource = users;
            UsersGridView.DataBind();
        }

        protected void SearchUserButton_Click(object sender, EventArgs e)
        {
            // Get the search query from the TextBox
            string searchUsername = SearchUsernameTextBox.Text.Trim();
            BindUserGrid(searchUsername);
        }

        protected void AddUserButton_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (IsValidInput())
            {
                string username = UsernameTextBox.Text;
                string password = PasswordTextBox.Text;
                string role = RoleDropDown.SelectedValue;
                string location = LocationTextBox.Text;
                string contact = ContactTextBox.Text;
                string email = EmailTextBox.Text;

                // Call the web service to add the user
                string result = userService.AddUser(username, password, role, username, location, contact, email);
                // Display alert with the result message
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + result + "');", true);

                // Refresh the GridView
                BindUserGrid();
            }
        }

        protected void UsersGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Set GridView to Edit Mode
            UsersGridView.EditIndex = e.NewEditIndex;
            BindUserGrid();
        }

        protected void UsersGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Cancel Edit Mode
            UsersGridView.EditIndex = -1;
            BindUserGrid();
        }

        protected void UsersGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Retrieve the User ID from DataKey
            int userId = (int)UsersGridView.DataKeys[e.RowIndex].Value;

            // Retrieve the updated values from the GridView
            GridViewRow row = UsersGridView.Rows[e.RowIndex];
            string username = ((TextBox)row.FindControl("UsernameTextBox")).Text;
            string password = ((TextBox)row.FindControl("PasswordTextBox")).Text;
            string role = ((DropDownList)row.FindControl("RoleDropDown")).SelectedValue;
            string location = ((TextBox)row.FindControl("LocationTextBox")).Text;
            string contact = ((TextBox)row.FindControl("ContactTextBox")).Text;
            string email = ((TextBox)row.FindControl("EmailTextBox")).Text;

            // Update the user via the web service
            string result = userService.UpdateUser(userId, username, password, role, username, location, contact, email);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + result + "');", true);

            // Set GridView back to normal mode
            UsersGridView.EditIndex = -1;
            BindUserGrid();
        }

        protected void UsersGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = (int)UsersGridView.DataKeys[e.RowIndex].Value;
            string result = userService.DeleteUser(userId);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + result + "');", true);

            // Refresh the GridView after deletion
            BindUserGrid();
        }

        private bool IsValidInput()
        {
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Text) ||
                string.IsNullOrWhiteSpace(LocationTextBox.Text) ||
                string.IsNullOrWhiteSpace(ContactTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please fill in all fields.');", true);
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(EmailTextBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid email format.');", true);
                return false;
            }

            return true;
        }
    }
}