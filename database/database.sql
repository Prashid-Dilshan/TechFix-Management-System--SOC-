

USE TechFix;

-- Drop existing tables if they exist (to avoid conflicts during re-creation)
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Inventory;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Users;

-- Create Users Table
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255),
    Role NVARCHAR(50) NOT NULL CHECK (Role IN ('Admin', 'Supplier')),
    Name NVARCHAR(100),
    Location NVARCHAR(100),
    ContactNumber NVARCHAR(20),
    Email NVARCHAR(100) UNIQUE,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- Sample Data for Users
INSERT INTO Users (Username, Password, Role, Name, Location, ContactNumber, Email)
VALUES 
('admin1', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', 'Admin', 'John Doe', 'New York', '1234567890', 'admin1@example.com'),
('supplier1', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', 'Supplier', 'Alice Smith', 'Los Angeles', '0987654321', 'supplier1@example.com'),
('supplier2', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', 'Supplier', 'Bob Johnson', 'San Francisco', '5555555555', 'supplier2@example.com'),
('admin2', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=', 'Admin', 'Jane Doe', 'Chicago', '1112223333', 'admin2@example.com');

-- Create Products Table
CREATE TABLE Products (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ItemName NVARCHAR(100),
    Quantity INT CHECK (Quantity >= 0),
    Price DECIMAL(18,2) CHECK (Price >= 0),
    Discount DECIMAL(5,2) CHECK (Discount >= 0 AND Discount <= 100),
    SupplierId INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SupplierId) REFERENCES Users(Id)
);

-- Sample Data for Products with correct Supplier IDs using LKR
INSERT INTO Products (ItemName, Quantity, Price, Discount, SupplierId)
VALUES
('External Hard Drive 1TB', 40, 15000.00, 10.00, 2),    -- SupplierId 2
('Wireless Mouse', 60, 2500.00, 15.00, 2),             -- SupplierId 2
('Mechanical Keyboard', 50, 7000.00, 12.00, 2),        -- SupplierId 2
('USB-C Hub', 35, 4500.00, 8.00, 2),                   -- SupplierId 2
('Bluetooth Speaker', 25, 5500.00, 10.00, 2),          -- SupplierId 2
('Webcam HD', 45, 8500.00, 5.00, 2),                   -- SupplierId 2
('Gaming Headset', 30, 8000.00, 10.00, 2),             -- SupplierId 2
('External SSD 512GB', 50, 18000.00, 7.00, 2),         -- SupplierId 2
('USB Flash Drive 64GB', 100, 1200.00, 5.00, 2),       -- SupplierId 2
('Portable Charger 10000mAh', 40, 3000.00, 8.00, 2),   -- SupplierId 2
('Monitor 24-inch', 25, 30000.00, 12.00, 2),           -- SupplierId 2
('Router Dual-Band', 30, 7000.00, 10.00, 2),           -- SupplierId 2
('Power Bank 20000mAh', 30, 4500.00, 10.00, 2),        -- SupplierId 2
('Laptop Cooling Pad', 25, 3500.00, 5.00, 2),          -- SupplierId 2
('HDMI Cable 2m', 75, 1500.00, 10.00, 2),              -- SupplierId 2
('Tablet 10-inch', 50, 60000.00, 8.00, 3),             -- SupplierId 3
('Printer All-in-One', 20, 25000.00, 12.00, 3),        -- SupplierId 3
('Wireless Keyboard', 40, 4000.00, 8.00, 3),           -- SupplierId 3
('Smartwatch', 35, 20000.00, 10.00, 3),                -- SupplierId 3
('Graphics Tablet', 15, 30000.00, 5.00, 3),            -- SupplierId 3
('Ethernet Cable 10m', 80, 800.00, 5.00, 3),           -- SupplierId 3
('Docking Station', 20, 10000.00, 8.00, 3),            -- SupplierId 3
('Wi-Fi Range Extender', 30, 5000.00, 10.00, 3),       -- SupplierId 3
('HDMI Splitter', 25, 3500.00, 5.00, 3),               -- SupplierId 3
('UPS Backup 1000VA', 15, 15000.00, 10.00, 3),         -- SupplierId 3
('Computer Speakers', 35, 7500.00, 7.00, 3),           -- SupplierId 3
('Monitor 27-inch', 20, 40000.00, 12.00, 3),           -- SupplierId 3
('SSD 1TB', 25, 25000.00, 8.00, 3),                    -- SupplierId 3
('Wireless Earbuds', 50, 9500.00, 7.00, 3),            -- SupplierId 3
('External DVD Writer', 30, 5500.00, 5.00, 3);         -- SupplierId 3

-- Create Inventory Table
CREATE TABLE Inventory (
    ItemId INT IDENTITY(1,1) PRIMARY KEY,
    ItemName NVARCHAR(100),
    Quantity INT CHECK (Quantity >= 0),
    Price DECIMAL(18,2) CHECK (Price >= 0),
    Discount DECIMAL(5,2) CHECK (Discount >= 0 AND Discount <= 100),
    SupplierId INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SupplierId) REFERENCES Users(Id)
);

-- Sample Data for Inventory with consistent Supplier IDs
INSERT INTO Inventory (ItemName, Quantity, Price, Discount, SupplierId)
VALUES 
('External Hard Drive 1TB', 30, 15000.00, 10.00, 2),  -- Corresponds to ProductId 1
('Wireless Mouse', 55, 2500.00, 15.00, 2),            -- Corresponds to ProductId 2
('Mechanical Keyboard', 45, 7000.00, 12.00, 2),       -- Corresponds to ProductId 3
('USB-C Hub', 30, 4500.00, 8.00, 2),                  -- Corresponds to ProductId 4
('Tablet 10-inch', 40, 60000.00, 8.00, 3);            -- Corresponds to ProductId 16

-- Create Orders Table
CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    AdminId INT,   -- Foreign key reference to Users
    SupplierId INT,  -- Foreign key reference to Users
    ItemId INT,   -- Foreign key reference to Inventory
    Quantity INT CHECK (Quantity > 0),
    Status NVARCHAR(50) CHECK (Status IN ('Pending', 'Shipped', 'Delivered', 'Rejected')),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AdminId) REFERENCES Users(Id),
    FOREIGN KEY (SupplierId) REFERENCES Users(Id),
    FOREIGN KEY (ItemId) REFERENCES Inventory(ItemId)
);

-- Sample Data for Orders with correct IDs
INSERT INTO Orders (AdminId, SupplierId, ItemId, Quantity, Status)
VALUES 
(1, 2, 1, 10, 'Pending'),  -- AdminId 1 (John Doe), SupplierId 2 (Alice Smith), ItemId 1 (External Hard Drive 1TB)
(1, 2, 2, 20, 'Shipped'),  -- AdminId 1 (John Doe), SupplierId 2 (Alice Smith), ItemId 2 (Wireless Mouse)
(1, 2, 3, 5, 'Delivered'), -- AdminId 1 (John Doe), SupplierId 2 (Alice Smith), ItemId 3 (Mechanical Keyboard)
(4, 3, 5, 15, 'Pending'),  -- AdminId 4 (Jane Doe), SupplierId 3 (Bob Johnson), ItemId 5 (Tablet 10-inch)
(4, 3, 4, 10, 'Shipped');  -- AdminId 4 (Jane Doe), SupplierId 3 (Bob Johnson), ItemId 4 (USB-C Hub)