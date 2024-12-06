using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFixV3._0Client.OrdersServiceReference;
using TechFixV3._0Client.InventoryServiceReference;

namespace TechFixV3._0Client.Supplier
{
    public partial class OrderManagement : System.Web.UI.Page
    {
        private OrdersServiceSoapClient ordersService = new OrdersServiceSoapClient();
        private InventoryServiceSoapClient inventoryService = new InventoryServiceSoapClient(); // Create an instance of InventoryService

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get the Supplier ID from the cookies
                int supplierId = GetUserIdFromCookies();
                BindOrdersGrid(supplierId);
            }
        }

        private void BindOrdersGrid(int supplierId)
        {
            // Fetch orders specific to the supplier from the service
            var orders = ordersService.GetOrdersBySupplierId(supplierId);
            foreach (var order in orders)
            {
                // Fetch item name using the inventory service
                var inventoryItem = inventoryService.GetInventoryById(order.ItemId);
                order.ItemName = inventoryItem?.ItemName ?? "Unknown";
            }
            // Bind the orders to the GridView
            OrdersGridView.DataSource = orders;
            OrdersGridView.DataBind();
        }

        protected void OrdersGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateStatus")
            {
                // Retrieve the Order ID from the CommandArgument
                int orderId = Convert.ToInt32(e.CommandArgument);

                // Find the row in which the button was clicked
                GridViewRow row = (GridViewRow)((Button)e.CommandSource).NamingContainer;

                // Find controls and retrieve necessary values
                DropDownList statusDropDown = (DropDownList)row.FindControl("OrderStatusDropDown");
                Label adminIdLabel = (Label)row.FindControl("AdminIdLabel");
                Label supplierIdLabel = (Label)row.FindControl("SupplierIdLabel");
                Label itemIdLabel = (Label)row.FindControl("ItemIdLabel");
                Label quantityLabel = (Label)row.FindControl("QuantityLabel");

                // Check for nulls and avoid NullReferenceException
                if (adminIdLabel == null || supplierIdLabel == null || itemIdLabel == null || quantityLabel == null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: Unable to retrieve order details.');", true);
                    return;
                }

                // Get the selected status and other required values
                string newStatus = statusDropDown.SelectedValue;
                int adminId = Convert.ToInt32(adminIdLabel.Text);
                int supplierId = Convert.ToInt32(supplierIdLabel.Text);
                int itemId = Convert.ToInt32(itemIdLabel.Text);
                int quantity = Convert.ToInt32(quantityLabel.Text);

                // Call the web service to update the order status
                string result = ordersService.UpdateOrder(orderId, adminId, supplierId, itemId, quantity, newStatus);

                // Show a success or error message
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + result + "');", true);

                // Rebind the grid to reflect the updated statuses
                BindOrdersGrid(supplierId);
            }
        }

        // Helper method to retrieve SupplierID from cookies
        private int GetUserIdFromCookies()
        {
            if (Request.Cookies["UserId"] != null)
            {
                return int.Parse(Request.Cookies["UserId"].Value);
            }

            return 0;
        }
    }
}