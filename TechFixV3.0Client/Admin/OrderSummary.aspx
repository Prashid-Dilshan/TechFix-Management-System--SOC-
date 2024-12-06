<%@ Page Title="Order Summary" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeBehind="OrderSummary.aspx.cs" Inherits="TechFixV3._0Client.Admin.ManageOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Order Summary</h2>

    <!-- Orders Summary Section -->
    <div class="orders-summary-section">
        <asp:GridView ID="OrdersGridView" runat="server" AutoGenerateColumns="False" CssClass="table">
            <Columns>
                <asp:BoundField DataField="OrderId" HeaderText="Order ID" />
                <asp:BoundField DataField="ItemId" HeaderText="Item ID" />
                <asp:BoundField DataField="AdminId" HeaderText="Admin ID" />
                <asp:BoundField DataField="SupplierId" HeaderText="Supplier ID" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity Ordered" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="CreatedAt" HeaderText="Created At" DataFormatString="{0:MM/dd/yyyy}" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
