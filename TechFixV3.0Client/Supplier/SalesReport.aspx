<%@ Page Title="Sales Report" Language="C#" MasterPageFile="~/Supplier/SupplierMaster.master" AutoEventWireup="true" CodeBehind="SalesReport.aspx.cs" Inherits="TechFixV3._0Client.Supplier.SalesReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Sales Report</h2>
    <p>Below is a summary of your stock and sales data.</p>

    <!-- Sales Report Grid -->
    <div class="sales-report-section">
        <asp:GridView ID="SalesReportGridView" runat="server" AutoGenerateColumns="False" CssClass="table">
            <Columns>
                <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                <asp:BoundField DataField="StockQuantity" HeaderText="Stock Quantity" />
                <asp:BoundField DataField="SoldQuantity" HeaderText="Sold Quantity" />
                <asp:BoundField DataField="TotalSales" HeaderText="Total Sales (LKR)" DataFormatString="LKR {0:N2}" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Total Revenue Section -->
    <div class="total-revenue-section">
        <h3>Total Revenue</h3>
        <p>Your total revenue from sales: <asp:Label ID="TotalRevenueLabel" runat="server" Text="LKR 0.00" CssClass="total-revenue" /></p>
    </div>
</asp:Content>