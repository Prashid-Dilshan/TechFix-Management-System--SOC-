<%@ Page Title="Order Management" Language="C#" MasterPageFile="~/Supplier/SupplierMaster.master" AutoEventWireup="true" CodeBehind="OrderManagement.aspx.cs" Inherits="TechFixV3._0Client.Supplier.OrderManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Order Management</h2>
    <p>Here you can view and update the status of orders placed by the admin.</p>

    <!-- Orders Grid -->
    <div class="order-management-section">
        <asp:GridView ID="OrdersGridView" runat="server" AutoGenerateColumns="False" CssClass="table" OnRowCommand="OrdersGridView_RowCommand">
            <Columns>
                <asp:BoundField DataField="CreatedAt" HeaderText="Order Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="OrderId" HeaderText="Order ID" />

                <asp:TemplateField HeaderText="Admin ID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="AdminIdLabel" runat="server" Text='<%# Eval("AdminId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Supplier ID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="SupplierIdLabel" runat="server" Text='<%# Eval("SupplierId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Item ID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="ItemIdLabel" runat="server" Text='<%# Eval("ItemId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="ItemName" HeaderText="Item Name" />

                <asp:TemplateField HeaderText="Ordered Quantity">
                    <ItemTemplate>
                        <asp:Label ID="QuantityLabel" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Order Status">
                    <ItemTemplate>
                        <asp:DropDownList ID="OrderStatusDropDown" runat="server" CssClass="input-field" SelectedValue='<%# Eval("Status") %>'>
                            <asp:ListItem Text="Pending" Value="Pending" />
                            <asp:ListItem Text="Shipped" Value="Shipped" />
                            <asp:ListItem Text="Delivered" Value="Delivered" />
                            <asp:ListItem Text="Rejected" Value="Rejected" />
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="UpdateButton" runat="server" Text="Update Status" CommandName="UpdateStatus" CommandArgument='<%# Eval("OrderId") %>' CssClass="submit-button" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>