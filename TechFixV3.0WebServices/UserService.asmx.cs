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
    public class UserService : WebService
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=TechFix;Integrated Security=True;";

        public UserService()
        {
            CreateUsersTableIfNotExists();
        }

        private void CreateUsersTableIfNotExists()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string createUsersTableQuery = @"
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
                    )";
                using (SqlCommand command = new SqlCommand(createUsersTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        [WebMethod]
        public List<User> GetUsers()
        {
            var users = new List<User>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            Name = reader["Name"].ToString(),
                            Role = reader["Role"].ToString(),
                            Location = reader["Location"].ToString(),
                            ContactNumber = reader["ContactNumber"].ToString(),
                            Email = reader["Email"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        });
                    }
                }
            }
            return users;
        }

        [WebMethod]
        public User GetUserById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            Name = reader["Name"].ToString(),
                            Role = reader["Role"].ToString(),
                            Location = reader["Location"].ToString(),
                            ContactNumber = reader["ContactNumber"].ToString(),
                            Email = reader["Email"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };
                    }
                }
            }
            return null;
        }

        [WebMethod]
        public List<User> GetUsersByRole(string role)
        {
            var users = new List<User>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Role = @Role";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Role", role);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            Name = reader["Name"].ToString(),
                            Role = reader["Role"].ToString(),
                            Location = reader["Location"].ToString(),
                            ContactNumber = reader["ContactNumber"].ToString(),
                            Email = reader["Email"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        });
                    }
                }
            }
            return users;
        }

        [WebMethod]
        public string AddUser(string username, string password, string role, string name, string location, string contactNumber, string email)
        {
            string hashedPassword = HashPassword(password);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Users (Username, Password, Role, Name, Location, ContactNumber, Email)
                    VALUES (@Username, @Password, @Role, @Name, @Location, @ContactNumber, @Email)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@ContactNumber", contactNumber);
                    command.Parameters.AddWithValue("@Email", email);

                    try
                    {
                        command.ExecuteNonQuery();
                        return "User added successfully.";
                    }
                    catch (SqlException ex)
                    {
                        return $"Error adding user: {ex.Message}";
                    }
                }
            }
        }

        [WebMethod]
        public string UpdateUser(int id, string username, string password, string role, string name, string location, string contactNumber, string email)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // If password is provided, update it, otherwise keep the existing password
                string query;
                if (!string.IsNullOrEmpty(password))
                {
                    string hashedPassword = HashPassword(password);
                    query = @"
                        UPDATE Users SET
                        Username = @Username,
                        Password = @Password,
                        Role = @Role,
                        Name = @Name,
                        Location = @Location,
                        ContactNumber = @ContactNumber,
                        Email = @Email,
                        UpdatedAt = GETDATE()
                        WHERE Id = @Id";
                }
                else
                {
                    query = @"
                        UPDATE Users SET
                        Username = @Username,
                        Role = @Role,
                        Name = @Name,
                        Location = @Location,
                        ContactNumber = @ContactNumber,
                        Email = @Email,
                        UpdatedAt = GETDATE()
                        WHERE Id = @Id";
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@ContactNumber", contactNumber);
                    command.Parameters.AddWithValue("@Email", email);

                    if (!string.IsNullOrEmpty(password))
                    {
                        command.Parameters.AddWithValue("@Password", HashPassword(password));
                    }

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0 ? "User updated successfully." : "User not found.";
                    }
                    catch (SqlException ex)
                    {
                        return $"Error updating user: {ex.Message}";
                    }
                }
            }
        }

        [WebMethod]
        public string DeleteUser(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0 ? "User deleted successfully." : "User not found.";
                    }
                    catch (SqlException ex)
                    {
                        return $"Error deleting user: {ex.Message}";
                    }
                }
            }
        }

        private string HashPassword(string password)
        {
            // Ensure password is not null
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Location { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
