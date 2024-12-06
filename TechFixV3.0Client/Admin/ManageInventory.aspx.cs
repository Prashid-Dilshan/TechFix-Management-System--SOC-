using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFixV3._0Client.InventoryServiceReference;
using TechFixV3._0Client.ProductServiceReference;
using TechFixV3._0Client.UserServiceReference;
using TechFixV3._0Client.OrdersServiceReference;

namespace TechFixV3._0Client.Admin
{
    public partial class ManageInventory : System.Web.UI.Page
    {
        private InventoryServiceSoapClient inventoryService = new InventoryServiceSoapClient();
        private ProductServiceSoapClient productService = new ProductServiceSoapClient();
        private UserServiceSoapClient userService = new UserServiceSoapClient();
        private OrdersServiceSoapClient ordersService = new OrdersServiceSoapClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSupplierGrid();
                BindSupplierProductsGrid();
                BindInventoryGrid();
                BindSupplierDropDown();
            }
        }

        private void BindSupplierGrid()
        {
            var suppliers = userService.GetUsersByRole("Supplier");
            SupplierGridView.DataSource = suppliers;
            SupplierGridView.DataBind();
        }

        private void BindSupplierProductsGrid()
        {
            var products = productService.GetProducts();
            SupplierProductsGridView.DataSource = products;
            SupplierProductsGridView.DataBind();
        }

        private void BindInventoryGrid()
        {
            var inventoryItems = inventoryService.GetInventory();
            InventoryGridView.DataSource = inventoryItems;
            InventoryGridView.DataBind();
        }

        private void BindSupplierDropDown()
        {
            var suppliers = userService.GetUsersByRole("Supplier");

            SupplierDropDown.DataSource = suppliers;
            SupplierDropDown.DataTextField = "Name";
            SupplierDropDown.DataValueField = "Id";
            SupplierDropDown.DataBind();

            SupplierDropDown.Items.Insert(0, new ListItem("Select Supplier", ""));
        }

        protected void SupplierDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindNewItemDropDown(); // Bind the products based on the selected supplier
            BindSupplierProductGrid(); // Update the supplier products grid
        }

        private void BindNewItemDropDown()
        {
            int selectedSupplierId = int.Parse(SupplierDropDown.SelectedValue);
            var products = productService.GetProductsBySupplierId(selectedSupplierId); // Ensure this method exists
            NewItemDropDown.DataSource = products;
            NewItemDropDown.DataTextField = "ItemName";
            NewItemDropDown.DataValueField = "ProductId";
            NewItemDropDown.DataBind();
        }

        private void BindSupplierProductGrid()
        {
            int selectedSupplierId = int.Parse(SupplierDropDown.SelectedValue);
            var products = productService.GetProductsBySupplierId(selectedSupplierId); // Ensure this method exists
            SupplierProductsGridView.DataSource = products;
            SupplierProductsGridView.DataBind();
        }

        protected void NewItemDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedProductId = int.Parse(NewItemDropDown.SelectedValue);
            var product = productService.GetProductById(selectedProductId); // Ensure this method exists

            if (product != null)
            {
                NewPriceTextBox.Text = product.Price.ToString("F2");
                NewDiscountTextBox.Text = product.Discount.ToString("F2");
            }
        }

        protected void AddItemButton_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                string itemName = NewItemDropDown.SelectedItem.Text;
                int quantity = int.Parse(NewQuantityTextBox.Text);
                decimal pricePerUnit = decimal.Parse(NewPriceTextBox.Text);
                decimal discount = decimal.Parse(NewDiscountTextBox.Text);
                int supplierId = int.Parse(SupplierDropDown.SelectedValue);

                // Calculate total price by multiplying the unit price by quantity
                decimal totalPrice = pricePerUnit * quantity;

                // Check current stock
                int selectedProductId = int.Parse(NewItemDropDown.SelectedValue);
                var product = productService.GetProductById(selectedProductId);

                if (product != null && quantity > product.Quantity)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: Requested quantity exceeds available stock. Current stock is " + product.Quantity + ".');", true);
                    return;
                }

                try
                {
                    // Proceed to add the item if quantity is valid
                    string inventoryResult = inventoryService.AddInventoryItem(itemName, quantity, totalPrice, discount, supplierId);
                    if (inventoryResult.ToLower().Contains("success"))
                    {
                        int adminId = GetAdminIdFromCookies();
                        if (adminId == 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: Admin ID is not set. Please login again.');", true);
                            return;
                        }

                        // Log for debugging
                        System.Diagnostics.Debug.WriteLine($"Attempting to create order - AdminID: {adminId}, SupplierID: {supplierId}, ProductID: {selectedProductId}, Quantity: {quantity}, Status: Pending");

                        string orderStatus = "Pending";
                        string orderResult = ordersService.AddOrder(adminId, supplierId, selectedProductId, quantity, orderStatus);

                        if (orderResult.ToLower().Contains("success"))
                        {
                            // Reduce the product stock
                            int updatedQuantity = product.Quantity - quantity;
                            string updateResult = productService.UpdateProduct(selectedProductId, product.ItemName, updatedQuantity, product.Price, product.Discount, product.SupplierId);

                            if (updateResult.ToLower().Contains("success"))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Inventory and Order added successfully. Stock updated.');", true);
                            }
                            else
                            {
                                // Rollback inventory addition if stock update fails
                                inventoryService.DeleteInventoryItem(selectedProductId);
                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to update product stock. Please check the system. Inventory addition rolled back.');", true);
                            }
                        }
                        else
                        {
                            // Rollback inventory addition if order creation fails
                            inventoryService.DeleteInventoryItem(selectedProductId);
                            System.Diagnostics.Debug.WriteLine($"Failed to create order: {orderResult}. Please check if admin, supplier, and product are correctly set.");
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Failed to create order: {orderResult}. Please check if admin, supplier, and product are correctly set.');", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to add inventory item: " + inventoryResult + "');", true);
                    }

                    // Refresh grids
                    BindSupplierProductsGrid();
                    BindInventoryGrid();
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('An error occurred: " + ex.Message + "');", true);
                }
            }
        }

        protected void InventoryGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Retrieve the Item ID from DataKey
            int itemId = (int)InventoryGridView.DataKeys[e.RowIndex].Value;

            // Get the current inventory item details
            var inventoryItem = inventoryService.GetInventoryById(itemId);
            if (inventoryItem != null)
            {
                int productId = GetProductIdByItemName(inventoryItem.ItemName, inventoryItem.SupplierId);

                if (productId > 0)
                {
                    // Increase the product stock by the quantity of the inventory item being deleted
                    var product = productService.GetProductById(productId);
                    if (product != null)
                    {
                        int updatedQuantity = product.Quantity + inventoryItem.Quantity;
                        string updateResult = productService.UpdateProduct(productId, product.ItemName, updatedQuantity, product.Price, product.Discount, product.SupplierId);

                        if (!updateResult.ToLower().Contains("success"))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to restore product stock.');", true);
                            return;
                        }
                    }
                }
            }

            // Delete the inventory item
            string result = inventoryService.DeleteInventoryItem(itemId);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + result + "');", true);

            // Refresh the GridView after deletion
            BindInventoryGrid();
        }

        private int GetProductIdByItemName(string itemName, int supplierId)
        {
            var products = productService.GetProductsBySupplierId(supplierId);
            foreach (var product in products)
            {
                if (product.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    return product.ProductId;
                }
            }
            return 0;
        }

        private bool IsValidInput()
        {
            // Check if all required fields are filled out for adding an item
            if (string.IsNullOrWhiteSpace(NewItemDropDown.SelectedValue) ||
                string.IsNullOrWhiteSpace(NewQuantityTextBox.Text) ||
                string.IsNullOrWhiteSpace(NewPriceTextBox.Text) ||
                string.IsNullOrWhiteSpace(NewDiscountTextBox.Text) ||
                string.IsNullOrWhiteSpace(SupplierDropDown.SelectedValue))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please fill in all fields.');", true);
                return false;
            }

            // Validate quantity
            if (!int.TryParse(NewQuantityTextBox.Text, out _))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid quantity format.');", true);
                return false;
            }

            return true;
        }

        // Retrieve Admin ID from cookies
        private int GetAdminIdFromCookies()
        {
            if (Request.Cookies["UserId"] != null)
            {
                return Convert.ToInt32(Request.Cookies["UserId"].Value);
            }
            return 0; // Return 0 if not found, meaning an error
        }
    }
}