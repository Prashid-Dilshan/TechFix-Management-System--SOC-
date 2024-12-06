using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;

namespace TechFixV3._0WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class InventoryService : WebService
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=TechFix;Integrated Security=True;";

        public InventoryService()
        {
            CreateInventoryTableIfNotExists();
        }

        private void CreateInventoryTableIfNotExists()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string createInventoryTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Inventory' AND xtype='U')
                    CREATE TABLE Inventory (
                        ItemId INT IDENTITY(1,1) PRIMARY KEY,
                        ItemName NVARCHAR(100),
                        Quantity INT,
                        Price DECIMAL(18,2),
                        Discount DECIMAL(5,2),
                        SupplierId INT,
                        CreatedAt DATETIME DEFAULT GETDATE(),
                        UpdatedAt DATETIME DEFAULT GETDATE()
                    )";
                using (SqlCommand command = new SqlCommand(createInventoryTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        [WebMethod]
        public List<InventoryItem> GetInventory()
        {
            var inventory = new List<InventoryItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Inventory";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        inventory.Add(new InventoryItem
                        {
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            ItemName = reader["ItemName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToDecimal(reader["Price"]),
                            Discount = Convert.ToDecimal(reader["Discount"]),
                            SupplierId = Convert.ToInt32(reader["SupplierId"]),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        });
                    }
                }
            }
            return inventory;
        }

        [WebMethod]
        public InventoryItem GetInventoryById(int itemId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Inventory WHERE ItemId = @ItemId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemId", itemId);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new InventoryItem
                        {
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            ItemName = reader["ItemName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToDecimal(reader["Price"]),
                            Discount = Convert.ToDecimal(reader["Discount"]),
                            SupplierId = Convert.ToInt32(reader["SupplierId"]),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };
                    }
                }
            }
            return null; // Return null if the item is not found
        }

        [WebMethod]
        public string UpdateInventory(int itemId, int quantity, decimal price, decimal discount, int supplierId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Inventory SET
                    Quantity = @Quantity,
                    Price = @Price,
                    Discount = @Discount,
                    SupplierId = @SupplierId,
                    UpdatedAt = GETDATE()
                    WHERE ItemId = @ItemId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemId", itemId);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Discount", discount);
                    command.Parameters.AddWithValue("@SupplierId", supplierId);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0 ? "Inventory updated successfully." : "Item not found.";
                    }
                    catch (SqlException ex)
                    {
                        // Log error (optional)
                        return "Error updating inventory: " + ex.Message;
                    }
                }
            }
        }

        [WebMethod]
        public string AddInventoryItem(string itemName, int quantity, decimal price, decimal discount, int supplierId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Inventory (ItemName, Quantity, Price, Discount, SupplierId)
                    VALUES (@ItemName, @Quantity, @Price, @Discount, @SupplierId)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemName", itemName);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Discount", discount);
                    command.Parameters.AddWithValue("@SupplierId", supplierId);

                    try
                    {
                        command.ExecuteNonQuery();
                        return "Inventory item added successfully.";
                    }
                    catch (SqlException ex)
                    {
                        // Log error (optional)
                        return "Error adding inventory item: " + ex.Message;
                    }
                }
            }
        }

        [WebMethod]
        public string DeleteInventoryItem(int itemId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Inventory WHERE ItemId = @ItemId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemId", itemId);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0 ? "Inventory item deleted successfully." : "Item not found.";
                    }
                    catch (SqlException ex)
                    {
                        // Log error (optional)
                        return "Error deleting inventory item: " + ex.Message;
                    }
                }
            }
        }
    }

    public class InventoryItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int SupplierId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}


