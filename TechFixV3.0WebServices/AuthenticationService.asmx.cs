using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Services;

namespace TechFixV3._0WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class AuthenticationService : WebService
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=TechFix;Integrated Security=True;";
        private const string AdminRole = "Admin";
        private const string ProcurementRole = "Procurement";
        private const string SupplierRole = "Supplier";

        public AuthenticationService()
        {
            CreateTablesIfNotExists();
        }

        [WebMethod]
        public LoginResult Login(string username, string password)
        {
            if (ValidateUser(username, password, out string role, out int userId))
            {
                return new LoginResult
                {
                    Message = $"Login successful. Welcome, {username} ({role}).",
                    UserId = userId,
                    Role = role
                };
            }
            else
            {
                return new LoginResult
                {
                    Message = "Invalid credentials.",
                    UserId = 0,
                    Role = null
                };
            }
        }

        private void CreateTablesIfNotExists()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                    CREATE TABLE Users (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Username NVARCHAR(50) UNIQUE NOT NULL,
                        Password NVARCHAR(255),
                        Role NVARCHAR(50),
                        Name NVARCHAR(100),
                        Location NVARCHAR(100),
                        ContactNumber NVARCHAR(20),
                        Email NVARCHAR(100) UNIQUE,
                        CreatedAt DATETIME DEFAULT GETDATE(),
                        UpdatedAt DATETIME DEFAULT GETDATE()
                    );";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private bool ValidateUser(string username, string password, out string role, out int userId)
        {
            role = null;
            userId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Role, Password FROM Users WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = (int)reader["Id"];
                            role = reader["Role"].ToString();

                            string hashedPassword = reader["Password"].ToString();
                            if (VerifyPassword(password, hashedPassword))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            string hashedInputPassword = HashPassword(password);
            return hashedInputPassword == hashedPassword;
        }
    }

    public class LoginResult
    {
        public string Message { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}
