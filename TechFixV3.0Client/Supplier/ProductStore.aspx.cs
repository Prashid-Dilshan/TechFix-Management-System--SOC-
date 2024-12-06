using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFixV3._0Client.ProductServiceReference;

namespace TechFixV3._0Client.Supplier
{
    public partial class ProductStore : System.Web.UI.Page
    {
        private ProductServiceSoapClient productService = new ProductServiceSoapClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int supplierId = GetSupplierIdFromCookies();
                BindProductsGrid(supplierId);
            }
        }

        private void BindProductsGrid(int supplierId, string searchQuery = "")
        {
            var products = productService.GetProductsBySupplierId(supplierId).ToList();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p => p.ItemName.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            StockGridView.DataSource = products;
            StockGridView.DataBind();
        }

        protected void AddItemButton_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                string itemName = ItemNameTextBox.Text;
                int quantity = int.Parse(QuantityTextBox.Text);
                decimal price = decimal.Parse(PriceTextBox.Text);
                decimal discount = decimal.Parse(DiscountTextBox.Text);
                int supplierId = GetSupplierIdFromCookies();

                string result = productService.AddProduct(itemName, quantity, price, discount, supplierId);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{result}');", true);

                ClearInputFields();
                BindProductsGrid(supplierId);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please fill in all fields correctly.');", true);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            int supplierId = GetSupplierIdFromCookies();
            string searchQuery = SearchTextBox.Text.Trim();
            BindProductsGrid(supplierId, searchQuery);
        }

        protected void StockGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockGridView.EditIndex = e.NewEditIndex;
            BindProductsGrid(GetSupplierIdFromCookies());
        }

        protected void StockGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int productId = Convert.ToInt32(StockGridView.DataKeys[e.RowIndex].Value);
            GridViewRow row = StockGridView.Rows[e.RowIndex];
            string itemName = ((TextBox)row.Cells[1].Controls[0]).Text;
            int quantity = int.Parse(((TextBox)row.Cells[2].Controls[0]).Text);
            decimal price = decimal.Parse(((TextBox)row.Cells[3].Controls[0]).Text);
            decimal discount = decimal.Parse(((TextBox)row.Cells[4].Controls[0]).Text);

            try
            {
                string result = productService.UpdateProduct(productId, itemName, quantity, price, discount, GetSupplierIdFromCookies());
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{result}');", true);

                StockGridView.EditIndex = -1;
                BindProductsGrid(GetSupplierIdFromCookies());
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error updating product: {ex.Message}');", true);
            }
        }

        protected void StockGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockGridView.EditIndex = -1;
            BindProductsGrid(GetSupplierIdFromCookies());
        }

        protected void StockGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productId = Convert.ToInt32(StockGridView.DataKeys[e.RowIndex].Value);
            string result = productService.DeleteProduct(productId);
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{result}');", true);

            BindProductsGrid(GetSupplierIdFromCookies());
        }

        private bool IsValidInput()
        {
            if (string.IsNullOrWhiteSpace(ItemNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                string.IsNullOrWhiteSpace(DiscountTextBox.Text))
            {
                return false;
            }

            if (!int.TryParse(QuantityTextBox.Text, out _) ||
                !decimal.TryParse(PriceTextBox.Text, out _) ||
                !decimal.TryParse(DiscountTextBox.Text, out _))
            {
                return false;
            }

            return true;
        }

        private void ClearInputFields()
        {
            ItemNameTextBox.Text = "";
            QuantityTextBox.Text = "";
            PriceTextBox.Text = "";
            DiscountTextBox.Text = "";
        }

        private int GetSupplierIdFromCookies()
        {
            if (Request.Cookies["UserId"] != null)
            {
                return int.Parse(Request.Cookies["UserId"].Value);
            }

            return 0;
        }
    }
}