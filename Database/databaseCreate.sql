-- Create Database
CREATE DATABASE MidtermDB;
GO

USE MidtermDB;
GO

-- Create Tables
CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME
);

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(18,2) NOT NULL,
    CategoryId INT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);
GO

-- Stored Procedures for Categories
CREATE PROCEDURE sp_GetAllCategories
AS
BEGIN
    SELECT * FROM Categories;
END
GO

CREATE PROCEDURE sp_GetCategoryById
    @Id INT
AS
BEGIN
    SELECT * FROM Categories WHERE Id = @Id;
END
GO

CREATE PROCEDURE sp_CreateCategory
    @Id INT = NULL,
    @Name NVARCHAR(100),
    @Description NVARCHAR(500),
    @CreatedAt DATETIME,
    @UpdatedAt DATETIME = NULL
AS
BEGIN
    INSERT INTO Categories (Name, Description, CreatedAt, UpdatedAt)
    VALUES (@Name, @Description, @CreatedAt, @UpdatedAt);
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_UpdateCategory
    @Id INT,
    @Name NVARCHAR(100),
    @Description NVARCHAR(500),
    @CreatedAt DATETIME,
    @UpdatedAt DATETIME
AS
BEGIN
    UPDATE Categories
    SET Name = @Name,
        Description = @Description,
        CreatedAt = @CreatedAt,
        UpdatedAt = @UpdatedAt
    WHERE Id = @Id;
    
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE sp_DeleteCategory
    @Id INT
AS
BEGIN
    DELETE FROM Categories WHERE Id = @Id;
    SELECT @@ROWCOUNT;
END
GO

-- Stored Procedures for Products
CREATE PROCEDURE sp_GetAllProducts
AS
BEGIN
    SELECT p.*, c.Name as CategoryName
    FROM Products p
    LEFT JOIN Categories c ON p.CategoryId = c.Id;
END
GO

CREATE PROCEDURE sp_GetProductById
    @Id INT
AS
BEGIN
    SELECT p.*, c.Name as CategoryName
    FROM Products p
    LEFT JOIN Categories c ON p.CategoryId = c.Id
    WHERE p.Id = @Id;
END
GO

CREATE PROCEDURE sp_GetProductsByCategory
    @CategoryId INT
AS
BEGIN
    SELECT p.*, c.Name as CategoryName
    FROM Products p
    LEFT JOIN Categories c ON p.CategoryId = c.Id
    WHERE p.CategoryId = @CategoryId;
END
GO

CREATE PROCEDURE sp_CreateProduct
    @Id INT = NULL,
    @Name NVARCHAR(100),
    @Description NVARCHAR(500),
    @Price DECIMAL(18,2),
    @CategoryId INT,
    @CreatedAt DATETIME,
    @UpdatedAt DATETIME = NULL
AS
BEGIN
    INSERT INTO Products (Name, Description, Price, CategoryId, CreatedAt, UpdatedAt)
    VALUES (@Name, @Description, @Price, @CategoryId, @CreatedAt, @UpdatedAt);
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_UpdateProduct
    @Id INT,
    @Name NVARCHAR(100),
    @Description NVARCHAR(500),
    @Price DECIMAL(18,2),
    @CategoryId INT,
    @CreatedAt DATETIME,
    @UpdatedAt DATETIME
AS
BEGIN
    UPDATE Products
    SET Name = @Name,
        Description = @Description,
        Price = @Price,
        CategoryId = @CategoryId,
        CreatedAt = @CreatedAt,
        UpdatedAt = @UpdatedAt
    WHERE Id = @Id;
    
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE sp_DeleteProduct
    @Id INT
AS
BEGIN
    DELETE FROM Products WHERE Id = @Id;
    SELECT @@ROWCOUNT;
END
GO 