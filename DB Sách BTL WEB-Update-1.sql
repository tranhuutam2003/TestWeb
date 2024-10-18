create database bookvip
use bookvip

-- Bảng Users để quản lý người dùng (bao gồm cả khách hàng và admin)
CREATE TABLE Users (
    FullName NVARCHAR(100),
    Email NVARCHAR(100) UNIQUE,
    PhoneNumber CHAR(10),
    Address NVARCHAR(255),
    Password NVARCHAR(255),  -- Mã hóa mật khẩu
    Role INT,  -- 0: Khách hàng, 1: Admin
    CONSTRAINT pk_user PRIMARY KEY (PhoneNumber)
);

-- Bảng Categories (Danh mục)
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100)
);

-- Bảng Books (Sách)
CREATE TABLE Books (
    BookID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255),
    Author NVARCHAR(100),
    Publisher NVARCHAR(100),
    PublishedDate DATE,
    CategoryID INT,
    Price DECIMAL(10, 2),
    StockQuantity INT,
    Description NVARCHAR(1000),
    ImageURL NVARCHAR(255),
    CONSTRAINT fk_categories_books FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Bảng Cart (Giỏ hàng)
CREATE TABLE Cart (
    CartID INT PRIMARY KEY IDENTITY(1,1),
    PhoneNumber CHAR(10),
    CONSTRAINT fk_users_cart FOREIGN KEY (PhoneNumber) REFERENCES Users(PhoneNumber)
);

-- Bảng CartItems (Chi tiết giỏ hàng)
CREATE TABLE CartItems (
    CartItemID INT PRIMARY KEY IDENTITY(1,1),
    CartID INT,
    BookID INT,
    Quantity INT,
    CONSTRAINT fk_cart_cartitems FOREIGN KEY (CartID) REFERENCES Cart(CartID),
    CONSTRAINT fk_books_cartitems FOREIGN KEY (BookID) REFERENCES Books(BookID)
);

-- Bảng Orders (Đơn hàng)
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    PhoneNumber CHAR(10),
    OrderDate DATE,
    TotalAmount DECIMAL(10, 2),
    Status NVARCHAR(50),
    CONSTRAINT fk_users_orders FOREIGN KEY (PhoneNumber) REFERENCES Users(PhoneNumber)
);

-- Bảng OrderDetails (Chi tiết đơn hàng)
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT,
    BookID INT,
    Quantity INT,
    Price DECIMAL(10, 2),
    CONSTRAINT fk_orders_orderdetails FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    CONSTRAINT fk_books_orderdetails FOREIGN KEY (BookID) REFERENCES Books(BookID)
);

-- Bảng Payments (Thanh toán)
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT,
    PaymentDate DATE,
    PaymentMethod NVARCHAR(50),
    PaymentStatus NVARCHAR(50),
    CONSTRAINT fk_orders_payments FOREIGN KEY (OrderID) REFERENCES Orders(OrderID)
);

-- Bảng Reviews (Đánh giá)
CREATE TABLE Reviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    BookID INT,
    PhoneNumber CHAR(10),
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(1000),
    ReviewDate DATE,
    CONSTRAINT fk_books_reviews FOREIGN KEY (BookID) REFERENCES Books(BookID),
    CONSTRAINT fk_users_reviews FOREIGN KEY (PhoneNumber) REFERENCES Users(PhoneNumber)
);

INSERT INTO Categories (CategoryName) 
VALUES
( 'Fiction'),
('Science'),
( 'History'),
( 'Math');

exec sp_adduser 'User','User'
grant all on database::[bookvip] to [User]