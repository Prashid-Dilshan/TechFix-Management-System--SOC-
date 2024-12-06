using System;
using System.Web.UI;
using TechFixV3._0Client.OrdersServiceReference; // Ensure the service reference is correct

namespace TechFixV3._0Client.Admin
{
    public partial class ManageOrders : System.Web.UI.Page
    {
        private OrdersServiceSoapClient ordersService = new OrdersServiceSoapClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindOrdersGrid();
            }
        }

        private void BindOrdersGrid()
        {
            // Fetch orders and bind to the GridView
            var orders = ordersService.GetOrders(); // Ensure this method exists in your service
            OrdersGridView.DataSource = orders;
            OrdersGridView.DataBind();
        }
    }
}
