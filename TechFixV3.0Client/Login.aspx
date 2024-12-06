<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TechFixV3._0Client.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login - TechFix</title>
    <link href="~/Content/login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <!-- Information about TechFix -->
            <div class="techfix-info">
                <h1>Welcome to TechFix</h1>
                <p>TechFix is your trusted partner for reliable technical solutions. We connect suppliers and administrators with ease, fostering smooth operations and a dependable experience for all users.</p>
            </div>

            <!-- Login Form -->
            <div class="login-form">
                <h2>Login to TechFix</h2>
                <asp:Label ID="MessageLabel" runat="server" Text="" CssClass="error-message" />

                <div class="input-group">
                    <asp:Label ID="UsernameLabel" runat="server" Text="Username :" AssociatedControlID="UsernameTextBox" />
                    <asp:TextBox ID="UsernameTextBox" runat="server" CssClass="input-field"></asp:TextBox>
                </div>

                <div class="input-group">
                    <asp:Label ID="PasswordLabel" runat="server" Text="Password :" AssociatedControlID="PasswordTextBox" />
                    <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" CssClass="input-field"></asp:TextBox>
                </div>

                <div class="login-button">
                    <asp:Button ID="LoginButton" runat="server" Text="Login" CssClass="submit-button" OnClick="LoginButton_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>