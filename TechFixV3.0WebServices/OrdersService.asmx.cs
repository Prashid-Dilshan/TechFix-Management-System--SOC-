using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;

namespace TechFixV3._0WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class OrdersService : WebService
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=TechFix;Integrated Security=True;";

        public OrdersService()
        {
            CreateOrdersTableIfNotExists();
        }

        private void CreateOrdersTableIfNotExists()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string createOrdersTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Orders' AND xtype='U')
                    CREATE TABLE Orders (
                        OrderId INT IDENTITY(1,1) PRIMARY KEY,
                        AdminId INT,
                        SupplierId INT,
                        ItemId INT,
                        Quantity INT,
                        Status NVARCHAR(50),
                        CreatedAt DATETIME DEFAULT GETDATE(),
                        UpdatedAt DATETIME DEFAULT GETDATE()
                    )";
                using (SqlCommand command = new SqlCommand(createOrdersTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        [WebMethod]
        public List<Order> GetOrders()
        {
            var orders = new List<Order>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Orders";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            OrderId = Convert.ToInt32(reader["OrderId"]),
                            AdminId = Convert.ToInt32(reader["AdminId"]),
                            SupplierId = Convert.ToInt32(reader["SupplierId"]),
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Status = reader["Status"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        });
                    }
                }
            }
            return orders;
        }

        [WebMethod]
        public List<Order> GetOrdersBySupplierId(int supplierId)
        {
            var orders = new List<Order>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Orders WHERE SupplierId = @SupplierId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SupplierId", supplierId);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            OrderId = Convert.ToInt32(reader["OrderId"]),
                            AdminId = Convert.ToInt32(reader["AdminId"]),
                            SupplierId = Convert.ToInt32(reader["SupplierId"]),
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Status = reader["Status"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        });
                    }
                }
            }
            return orders; // Return the list of orders for the specified supplier
        }

        [WebMethod]
        public Order GetOrderById(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Orders WHERE OrderId = @OrderId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Order
                        {
                            OrderId = Convert.ToInt32(reader["OrderId"]),
                            AdminId = Convert.ToInt32(reader["AdminId"]),
                            SupplierId = Convert.ToInt32(reader["SupplierId"]),
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Status = reader["Status"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                        };
                    }
                }
            }
            return null; // Return null if the order is not found
        }

        [WebMethod]
        public string AddOrder(int adminId, int supplierId, int itemId, int quantity, string status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Orders (AdminId, SupplierId, ItemId, Quantity, Status)
                    VALUES (@AdminId, @SupplierId, @ItemId, @Quantity, @Status)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AdminId", adminId);
                    command.Parameters.AddWithValue("@SupplierId", supplierId);
                    command.Parameters.AddWithValue("@ItemId", itemId);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Status", status);

                    try
                    {
                        command.ExecuteNonQuery();
                        return "Order added successfully.";
                    }
                    catch (SqlException)
                    {
                        return "Error adding order.";
                    }
                }
            }
        }

        [WebMethod]
        public string UpdateOrder(int orderId, int adminId, int supplierId, int itemId, int quantity, string status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Orders SET
                    AdminId = @AdminId,
                    SupplierId = @SupplierId,
                    ItemId = @ItemId,
                    Quantity = @Quantity,
                    Status = @Status,
                    UpdatedAt = GETDATE()
                    WHERE OrderId = @OrderId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.Parameters.AddWithValue("@AdminId", adminId);
                    command.Parameters.AddWithValue("@SupplierId", supplierId);
                    command.Parameters.AddWithValue("@ItemId", itemId);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Status", status);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0 ? "Order updated successfully." : "Order not found.";
                    }
                    catch (SqlException)
                    {
                        return "Error updating order.";
                    }
                }
            }
        }

        [WebMethod]
        public string DeleteOrder(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Orders WHERE OrderId = @OrderId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0 ? "Order deleted successfully." : "Order not found.";
                    }
                    catch (SqlException)
                    {
                        return "Error deleting order.";
                    }
                }
            }
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int AdminId { get; set; }
        public int SupplierId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string ItemName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}





















