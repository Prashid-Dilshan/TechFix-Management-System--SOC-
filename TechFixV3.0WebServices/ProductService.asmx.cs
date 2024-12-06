using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;

namespace TechFixV3._0WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ProductService : WebService
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=TechFix;Integrated Security=True;";

        public ProductService()
        {
            CreateProductTableIfNotExists();
        }

        private void CreateProductTableIfNotExists()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string createProductTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
                    CREATE TABLE Products (
                        ProductId INT IDENTITY(1,1) PRIMARY KEY,
                        ItemName NVARCHAR(100),
                        Quantity INT,
                        Price DECIMAL(18,2),
                        Discount DECIMAL(5,2),
                        SupplierId INT,
                        CreatedAt DATETIME DEFAULT GETDATE(),
                        UpdatedAt DATETIME DEFAULT GETDATE()
                    )";
                using (SqlCommand command = new SqlCommand(createProductTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        [WebMethod]
        public string AddProduct(string itemName, int quantity, decimal price, decimal discount, int supplierId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Products (ItemName, Quantity, Price, Discount, SupplierId)
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
                        return "Product added successfully.";
                    }
                    catch (SqlException)
                    {
                        return "Error adding product.";
                    }
                }
            }
        }

        [WebMethod]
        public string DeleteProduct(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Products WHERE ProductId = @ProductId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0 ? "Product deleted successfully." : "Product not found.";
                    }
                    catch (SqlException)
                    {
                        return "Error deleting product.";
                    }
                }
            }
        }

        [WebMethod]
        public List<Product> GetProducts()
        {
            var products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Products";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
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
            return products;
        }

        [WebMethod]
        public List<Product> GetProductsBySupplierId(int supplierId)
        {
            var products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Products WHERE SupplierId = @SupplierId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SupplierId", supplierId);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
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
            return products;
        }

        [WebMethod]
        public Product GetProductById(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Products WHERE ProductId = @ProductId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Product
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
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
            return null; // Return null if the product is not found
        }

        [WebMethod]
        public string UpdateProduct(int productId, string itemName, int quantity, decimal price, decimal discount, int supplierId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Products SET
                    ItemName = @ItemName,
                    Quantity = @Quantity,
                    Price = @Price,
                    Discount = @Discount,
                    SupplierId = @SupplierId,
                    UpdatedAt = GETDATE()
                    WHERE ProductId = @ProductId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@ItemName", itemName);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Discount", discount);
                    command.Parameters.AddWithValue("@SupplierId", supplierId);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0 ? "Product updated successfully." : "Product not found.";
                    }
                    catch (SqlException)
                    {
                        return "Error updating product.";
                    }
                }
            }
        }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int SupplierId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
