-- Скрипт создания базы данных для системы управления продажами спортивного инвентаря

-- Создание базы данных
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SportInventoryDB')
BEGIN
    CREATE DATABASE SportInventoryDB;
END
GO

USE SportInventoryDB;
GO

-- Таблица товаров (спортивный инвентарь)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Products] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL,
        [Category] NVARCHAR(100) NULL,
        [Price] DECIMAL(18,2) NOT NULL,
        [Quantity] INT NOT NULL DEFAULT 0,
        [Description] NVARCHAR(500) NULL,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- Таблица продаж
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sales]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Sales] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [SaleDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [TotalAmount] DECIMAL(18,2) NOT NULL,
        [CustomerName] NVARCHAR(200) NULL,
        [Notes] NVARCHAR(500) NULL
    );
END
GO

-- Таблица позиций продажи
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaleItems]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[SaleItems] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [SaleId] INT NOT NULL,
        [ProductId] INT NOT NULL,
        [Quantity] INT NOT NULL,
        [UnitPrice] DECIMAL(18,2) NOT NULL,
        [TotalPrice] DECIMAL(18,2) NOT NULL,
        FOREIGN KEY ([SaleId]) REFERENCES [dbo].[Sales]([Id]) ON DELETE CASCADE,
        FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products]([Id])
    );
END
GO

-- Индексы для улучшения производительности
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Sales_SaleDate')
BEGIN
    CREATE INDEX IX_Sales_SaleDate ON [dbo].[Sales]([SaleDate]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SaleItems_SaleId')
BEGIN
    CREATE INDEX IX_SaleItems_SaleId ON [dbo].[SaleItems]([SaleId]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SaleItems_ProductId')
BEGIN
    CREATE INDEX IX_SaleItems_ProductId ON [dbo].[SaleItems]([ProductId]);
END
GO

-- Таблица пользователей
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Username] NVARCHAR(50) NOT NULL UNIQUE,
        [Password] NVARCHAR(255) NOT NULL,
        [FullName] NVARCHAR(100) NOT NULL,
        [Role] NVARCHAR(20) NOT NULL DEFAULT 'Seller',
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- Индекс для пользователей
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Username')
BEGIN
    CREATE INDEX IX_Users_Username ON [dbo].[Users]([Username]);
END
GO

-- Вставка тестовых данных
IF NOT EXISTS (SELECT * FROM [dbo].[Products])
BEGIN
    INSERT INTO [dbo].[Products] ([Name], [Category], [Price], [Quantity], [Description]) VALUES
    (N'Футбольный мяч', N'Мячи', 1500.00, 20, N'Профессиональный футбольный мяч'),
    (N'Баскетбольный мяч', N'Мячи', 2000.00, 15, N'Официальный размер баскетбольного мяча'),
    (N'Теннисная ракетка', N'Ракетки', 3500.00, 10, N'Графитовая ракетка для тенниса'),
    (N'Гантели 5кг', N'Тренажеры', 2500.00, 30, N'Набор гантелей 5кг'),
    (N'Скакалка', N'Аксессуары', 500.00, 50, N'Профессиональная скакалка'),
    (N'Коврик для йоги', N'Аксессуары', 1200.00, 25, N'Антискользящий коврик');
END
GO

-- Вставка тестовых пользователей (пароль: admin123 и seller123)
IF NOT EXISTS (SELECT * FROM [dbo].[Users])
BEGIN
    INSERT INTO [dbo].[Users] ([Username], [Password], [FullName], [Role], [IsActive]) VALUES
    (N'admin', N'admin123', N'Администратор', N'Admin', 1),
    (N'seller', N'seller123', N'Продавец', N'Seller', 1),
    (N'manager', N'manager123', N'Менеджер', N'Manager', 1);
END
GO

