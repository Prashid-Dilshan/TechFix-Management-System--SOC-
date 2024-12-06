using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;

namespace TechFixV3._0WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ReportsService : WebService
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=TechFix;Integrated Security=True;";

        public ReportsService()
        {
            // Initialize any necessary components or data structures
        }

        // Method to get sales report for a specific supplier
        [WebMethod]
        public List<SalesReportItem> GetSalesReportBySupplierId(int supplierId)
        {
            var salesData = new List<SalesReportItem>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Query to get order details with product and inventory info
                string query = @"
                    SELECT 
                        p.ProductId,
                        p.ItemName,
                        i.Quantity AS StockQuantity,
                        SUM(o.Quantity) AS SoldQuantity,
                        SUM(o.Quantity * p.Price) AS TotalSales
                    FROM Orders o
                    INNER JOIN Products p ON o.ItemId = p.ProductId
                    INNER JOIN Inventory i ON p.ProductId = i.ItemId
                    WHERE p.SupplierId = @SupplierId
                    GROUP BY p.ProductId, p.ItemName, i.Quantity";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SupplierId", supplierId);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        salesData.Add(new SalesReportItem
                        {
                            ItemId = Convert.ToInt32(reader["ProductId"]),
                            ItemName = reader["ItemName"].ToString(),
                            StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                            SoldQuantity = Convert.ToInt32(reader["SoldQuantity"]),
                            TotalSales = Convert.ToDecimal(reader["TotalSales"])
                        });
                    }
                }
            }
            return salesData; // Return the list of sales report items
        }
    }

    // Class to hold sales report data
    public class SalesReportItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int StockQuantity { get; set; }
        public int SoldQuantity { get; set; }
        public decimal TotalSales { get; set; }
    }
}