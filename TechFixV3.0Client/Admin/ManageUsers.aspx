<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="TechFixV3._0Client.Admin.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Users</h2>

    <!-- Search Section -->
    <div class="search-user-section">
        <div class="form-group">
            <asp:TextBox ID="SearchUsernameTextBox" runat="server" CssClass="input-field" placeholder="Search by Username..." />
        </div>
        <div class="form-group">
            <asp:Button ID="Button1" runat="server" Text="Search" CssClass="submit-button" OnClick="SearchUserButton_Click" />
        </div>
    </div>

    <!-- Display Users Section -->
    <div class="user-list-section">
        <h3>Users</h3>
        <asp:GridView ID="UsersGridView" runat="server" AutoGenerateColumns="False" CssClass="table" 
            OnRowEditing="UsersGridView_RowEditing" 
            OnRowDeleting="UsersGridView_RowDeleting" 
            OnRowCancelingEdit="UsersGridView_RowCancelingEdit"
            OnRowUpdating="UsersGridView_RowUpdating"
            DataKeyNames="Id">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True" />
                <asp:TemplateField HeaderText="Username">
                    <ItemTemplate>
                        <%# Eval("Username") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="UsernameTextBox" runat="server" Text='<%# Bind("Username") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Password">
                    <ItemTemplate>
                        <asp:Label ID="PasswordLabel" runat="server" Text="*****" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" CssClass="input-field" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Role">
                    <ItemTemplate>
                        <%# Eval("Role") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="RoleDropDown" runat="server" CssClass="input-field">
                            <asp:ListItem Text="Admin" Value="Admin" />
                            <asp:ListItem Text="Supplier" Value="Supplier" />
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Location">
                    <ItemTemplate>
                        <%# Eval("Location") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="LocationTextBox" runat="server" Text='<%# Bind("Location") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Contact">
                    <ItemTemplate>
                        <%# Eval("ContactNumber") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="ContactTextBox" runat="server" Text='<%# Bind("ContactNumber") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Email">
                    <ItemTemplate>
                        <%# Eval("Email") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="EmailTextBox" runat="server" Text='<%# Bind("Email") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>



    <!-- Add New User Section -->
    <div class="add-user-section">
        <h3>Add New User</h3>
        <asp:Label ID="MessageLabel" runat="server" ForeColor="Red" />
        <div class="form-group">
            <asp:Label ID="UsernameLabel" runat="server" Text="Username:" AssociatedControlID="UsernameTextBox" />
            <asp:TextBox ID="UsernameTextBox" runat="server" CssClass="input-field" />
        </div>

        <div class="form-group">
            <asp:Label ID="PasswordLabel" runat="server" Text="Password:" AssociatedControlID="PasswordTextBox" />
            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" CssClass="input-field" />
        </div>

        <div class="form-group">
            <asp:Label ID="RoleLabel" runat="server" Text="Role:" AssociatedControlID="RoleDropDown" />
            <asp:DropDownList ID="RoleDropDown" runat="server" CssClass="input-field">
                <asp:ListItem Text="Admin" Value="Admin" />
                <asp:ListItem Text="Supplier" Value="Supplier" />
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <asp:Label ID="LocationLabel" runat="server" Text="Location:" AssociatedControlID="LocationTextBox" />
            <asp:TextBox ID="LocationTextBox" runat="server" CssClass="input-field" />
        </div>

        <div class="form-group">
            <asp:Label ID="ContactLabel" runat="server" Text="Contact:" AssociatedControlID="ContactTextBox" />
            <asp:TextBox ID="ContactTextBox" runat="server" CssClass="input-field" />
        </div>

        <div class="form-group">
            <asp:Label ID="EmailLabel" runat="server" Text="Email:" AssociatedControlID="EmailTextBox" />
            <asp:TextBox ID="EmailTextBox" runat="server" CssClass="input-field" />
        </div>

        <div class="form-group">
            <asp:Button ID="AddUserButton" runat="server" Text="Add User" CssClass="submit-button" OnClick="AddUserButton_Click" />
        </div>
    </div>
</asp:Content>