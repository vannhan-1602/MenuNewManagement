-- ============================================================
-- MenuNewsManagement - SQL Server Database Script
-- Chạy script này TRƯỚC khi scaffold EF Core DB First
-- ============================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'MenuNewsDb')
BEGIN
    CREATE DATABASE MenuNewsDb;
END
GO

USE MenuNewsDb;
GO

-- Bảng Menus
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Menus')
BEGIN
    CREATE TABLE Menus (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Name        NVARCHAR(200)  NOT NULL,
        Description NVARCHAR(500)  NULL,
        CreatedDate DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
        IsDeleted   BIT            NOT NULL DEFAULT 0,
        DeletedAt   DATETIME2      NULL
    );
END
GO

-- Bảng News
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'News')
BEGIN
    CREATE TABLE News (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Title       NVARCHAR(300)  NOT NULL,
        Content     NVARCHAR(MAX)  NOT NULL,
        Author      NVARCHAR(150)  NOT NULL,
        CreatedDate DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
        IsDeleted   BIT            NOT NULL DEFAULT 0,
        DeletedAt   DATETIME2      NULL
    );
END
GO

-- Bảng trung gian Many-to-Many: MenuNews
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MenuNews')
BEGIN
    CREATE TABLE MenuNews (
        MenuId INT NOT NULL,
        NewsId INT NOT NULL,
        CONSTRAINT PK_MenuNews PRIMARY KEY (MenuId, NewsId),
        CONSTRAINT FK_MenuNews_Menus FOREIGN KEY (MenuId) REFERENCES Menus(Id) ON DELETE CASCADE,
        CONSTRAINT FK_MenuNews_News  FOREIGN KEY (NewsId) REFERENCES News(Id)  ON DELETE CASCADE
    );

    CREATE INDEX IX_MenuNews_NewsId ON MenuNews(NewsId);
END
GO

-- Dữ liệu mẫu để demo
IF NOT EXISTS (SELECT 1 FROM Menus)
BEGIN
    INSERT INTO Menus (Name, Description, CreatedDate)
    VALUES
        (N'Tin tức công nghệ', N'Menu các bài viết về công nghệ', GETUTCDATE()),
        (N'Tin tức kinh doanh', N'Menu các bài viết kinh doanh', GETUTCDATE());

    INSERT INTO News (Title, Content, Author, CreatedDate)
    VALUES
        (N'ASP.NET Core 8 ra mắt', N'ASP.NET Core 8 mang đến nhiều cải tiến về performance và developer experience cho enterprise applications.', N'admin', GETUTCDATE()),
        (N'Clean Architecture trong thực tế', N'Clean Architecture giúp tách biệt business logic khỏi infrastructure, dễ test và bảo trì.', N'admin', GETUTCDATE());

    INSERT INTO MenuNews (MenuId, NewsId) VALUES (1, 1), (1, 2), (2, 2);
END
GO

PRINT 'Database MenuNewsDb created successfully.';
