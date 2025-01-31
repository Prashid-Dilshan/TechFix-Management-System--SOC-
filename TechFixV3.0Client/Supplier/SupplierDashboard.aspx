<%@ Page Language="C#" MasterPageFile="~/Supplier/SupplierMaster.master" AutoEventWireup="true" CodeBehind="SupplierDashboard.aspx.cs" Inherits="TechFixV3._0Client.Supplier.SupplierDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="dashboard-title">Welcome to the Supplier Dashboard</h2>
    <p class="dashboard-intro">This is the main supplier dashboard where you can manage your product store, track orders, and view sales reports.</p>

    <div class="supplier-sections">
        <div class="section">
            <h3><i class="fas fa-store" style="color: #2ecc71;"></i> Product Store</h3>
            <p>Browse, add, and manage products available in the store for your customers.</p>
            <a href="ProductStore.aspx" class="btn">Go to Product Store</a>
        </div>

        <div class="section">
            <h3><i class="fas fa-box" style="color: #2ecc71;"></i> Order Management</h3>
            <p>Track and manage all your orders efficiently, update order statuses, and notify customers.</p>
            <a href="OrderManagement.aspx" class="btn">Go to Order Management</a>
        </div>

        <div class="section">
            <h3><i class="fas fa-chart-line" style="color: #2ecc71;"></i> Sales Report</h3>
            <p>View detailed reports of your sales performance and analyze trends.</p>
            <a href="SalesReport.aspx" class="btn">Go to Sales Report</a>
        </div>
    </div>
</asp:Content>