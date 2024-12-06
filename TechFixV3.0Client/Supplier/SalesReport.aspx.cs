using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFixV3._0Client.ReportsServiceReference;

namespace TechFixV3._0Client.Supplier
{
    public partial class SalesReport : System.Web.UI.Page
    {
        private ReportsServiceSoapClient reportsService = new ReportsServiceSoapClient(); // Create an instance of ReportsService

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get the Supplier ID from cookies (instead of session for consistency with other pages)
                int supplierId = GetUserIdFromCookies();
                BindSalesReportGrid(supplierId);
            }
        }

        private void BindSalesReportGrid(int supplierId)
        {
            // Fetch sales report data for the specific supplier
            var salesData = reportsService.GetSalesReportBySupplierId(supplierId); // Call the method in ReportsService

            // Bind the sales data to the GridView
            SalesReportGridView.DataSource = salesData;
            SalesReportGridView.DataBind();

            // Calculate total revenue
            decimal totalRevenue = 0;
            foreach (var salesReportItem in salesData)
            {
                totalRevenue += salesReportItem.TotalSales;
            }

            // Display the total revenue in LKR
            TotalRevenueLabel.Text = $"LKR {totalRevenue:N2}";
        }

        // Helper method to retrieve SupplierID from cookies
        private int GetUserIdFromCookies()
        {
            if (Request.Cookies["UserId"] != null)
            {
                return int.Parse(Request.Cookies["UserId"].Value);
            }
            return 0; // Default value if not found
        }
    }

    // Class to hold sales report data (this is already part of your service return type, ensure it's consistent)
    public class SalesReportItem
    {
        public string ItemName { get; set; }
        public int StockQuantity { get; set; }
        public int SoldQuantity { get; set; }
        public decimal TotalSales { get; set; }
    }
}