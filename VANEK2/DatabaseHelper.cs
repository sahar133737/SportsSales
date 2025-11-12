using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using VANEK2.Models;

namespace VANEK2
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper()
        {
            connectionString = @"Server=SAHAR\SQLSERVER;Database=SportInventoryDB;Integrated Security=true;";
            //if (string.IsNullOrEmpty(connectionString))
            //{
            //    connectionString = "Data Source=SAHAR\\SQLSERVER;Initial Catalog=SportInventoryDB;Integrated Security=True;Connect Timeout=30";
            //}
        }

        // Методы для работы с товарами
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Products ORDER BY Name", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Category = reader["Category"]?.ToString() ?? "",
                            Price = (decimal)reader["Price"],
                            Quantity = (int)reader["Quantity"],
                            Description = reader["Description"]?.ToString() ?? "",
                            CreatedDate = (DateTime)reader["CreatedDate"]
                        });
                    }
                }
            }
            return products;
        }

        public Product GetProductById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Products WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Product
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Category = reader["Category"]?.ToString() ?? "",
                            Price = (decimal)reader["Price"],
                            Quantity = (int)reader["Quantity"],
                            Description = reader["Description"]?.ToString() ?? "",
                            CreatedDate = (DateTime)reader["CreatedDate"]
                        };
                    }
                }
            }
            return null;
        }

        public int AddProduct(Product product)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Products (Name, Category, Price, Quantity, Description) " +
                    "VALUES (@Name, @Category, @Price, @Quantity, @Description); SELECT SCOPE_IDENTITY();",
                    connection);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Category", (object)product.Category ?? DBNull.Value);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@Description", (object)product.Description ?? DBNull.Value);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Products SET Name = @Name, Category = @Category, Price = @Price, " +
                    "Quantity = @Quantity, Description = @Description WHERE Id = @Id",
                    connection);
                command.Parameters.AddWithValue("@Id", product.Id);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Category", (object)product.Category ?? DBNull.Value);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@Description", (object)product.Description ?? DBNull.Value);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Products WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        // Методы для работы с продажами
        public int CreateSale(Sale sale)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Создаем продажу
                        var saleCommand = new SqlCommand(
                            "INSERT INTO Sales (SaleDate, TotalAmount, CustomerName, Notes) " +
                            "VALUES (@SaleDate, @TotalAmount, @CustomerName, @Notes); SELECT SCOPE_IDENTITY();",
                            connection, transaction);
                        saleCommand.Parameters.AddWithValue("@SaleDate", sale.SaleDate);
                        saleCommand.Parameters.AddWithValue("@TotalAmount", sale.TotalAmount);
                        saleCommand.Parameters.AddWithValue("@CustomerName", (object)sale.CustomerName ?? DBNull.Value);
                        saleCommand.Parameters.AddWithValue("@Notes", (object)sale.Notes ?? DBNull.Value);
                        int saleId = Convert.ToInt32(saleCommand.ExecuteScalar());

                        // Добавляем позиции продажи и обновляем количество товаров
                        foreach (var item in sale.Items)
                        {
                            var itemCommand = new SqlCommand(
                                "INSERT INTO SaleItems (SaleId, ProductId, Quantity, UnitPrice, TotalPrice) " +
                                "VALUES (@SaleId, @ProductId, @Quantity, @UnitPrice, @TotalPrice)",
                                connection, transaction);
                            itemCommand.Parameters.AddWithValue("@SaleId", saleId);
                            itemCommand.Parameters.AddWithValue("@ProductId", item.ProductId);
                            itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                            itemCommand.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                            itemCommand.Parameters.AddWithValue("@TotalPrice", item.TotalPrice);
                            itemCommand.ExecuteNonQuery();

                            // Уменьшаем количество товара на складе
                            var updateCommand = new SqlCommand(
                                "UPDATE Products SET Quantity = Quantity - @Quantity WHERE Id = @ProductId",
                                connection, transaction);
                            updateCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                            updateCommand.Parameters.AddWithValue("@ProductId", item.ProductId);
                            updateCommand.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return saleId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<Sale> GetAllSales(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var sales = new List<Sale>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Sales WHERE 1=1";
                if (fromDate.HasValue)
                    query += " AND SaleDate >= @FromDate";
                if (toDate.HasValue)
                    query += " AND SaleDate <= @ToDate";
                query += " ORDER BY SaleDate DESC";

                var command = new SqlCommand(query, connection);
                if (fromDate.HasValue)
                    command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    command.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var sale = new Sale
                        {
                            Id = (int)reader["Id"],
                            SaleDate = (DateTime)reader["SaleDate"],
                            TotalAmount = (decimal)reader["TotalAmount"],
                            CustomerName = reader["CustomerName"]?.ToString() ?? "",
                            Notes = reader["Notes"]?.ToString() ?? ""
                        };
                        sales.Add(sale);
                    }
                }

                // Загружаем позиции для каждой продажи
                foreach (var sale in sales)
                {
                    sale.Items = GetSaleItems(sale.Id);
                }
            }
            return sales;
        }

        private List<SaleItem> GetSaleItems(int saleId)
        {
            var items = new List<SaleItem>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "SELECT si.*, p.Name as ProductName FROM SaleItems si " +
                    "INNER JOIN Products p ON si.ProductId = p.Id WHERE si.SaleId = @SaleId",
                    connection);
                command.Parameters.AddWithValue("@SaleId", saleId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new SaleItem
                        {
                            Id = (int)reader["Id"],
                            SaleId = (int)reader["SaleId"],
                            ProductId = (int)reader["ProductId"],
                            ProductName = reader["ProductName"].ToString(),
                            Quantity = (int)reader["Quantity"],
                            UnitPrice = (decimal)reader["UnitPrice"],
                            TotalPrice = (decimal)reader["TotalPrice"]
                        });
                    }
                }
            }
            return items;
        }

        // Методы для отчетов
        public decimal GetTotalSales(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT SUM(TotalAmount) FROM Sales WHERE 1=1";
                if (fromDate.HasValue)
                    query += " AND SaleDate >= @FromDate";
                if (toDate.HasValue)
                    query += " AND SaleDate <= @ToDate";

                var command = new SqlCommand(query, connection);
                if (fromDate.HasValue)
                    command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    command.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                var result = command.ExecuteScalar();
                return result != DBNull.Value ? (decimal)result : 0;
            }
        }

        public int GetTotalSalesCount(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Sales WHERE 1=1";
                if (fromDate.HasValue)
                    query += " AND SaleDate >= @FromDate";
                if (toDate.HasValue)
                    query += " AND SaleDate <= @ToDate";

                var command = new SqlCommand(query, connection);
                if (fromDate.HasValue)
                    command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    command.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                return (int)command.ExecuteScalar();
            }
        }

        public DataTable GetTopSellingProducts(DateTime? fromDate = null, DateTime? toDate = null, int topCount = 10)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT TOP (@TopCount) 
                        p.Name, 
                        p.Category,
                        SUM(si.Quantity) as TotalQuantity,
                        SUM(si.TotalPrice) as TotalRevenue
                    FROM SaleItems si
                    INNER JOIN Sales s ON si.SaleId = s.Id
                    INNER JOIN Products p ON si.ProductId = p.Id
                    WHERE 1=1";
                
                if (fromDate.HasValue)
                    query += " AND s.SaleDate >= @FromDate";
                if (toDate.HasValue)
                    query += " AND s.SaleDate <= @ToDate";
                
                query += @"
                    GROUP BY p.Name, p.Category
                    ORDER BY TotalQuantity DESC";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TopCount", topCount);
                if (fromDate.HasValue)
                    command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    command.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Отчет по продажам по категориям
        public DataTable GetSalesByCategory(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        p.Category as Категория,
                        COUNT(DISTINCT s.Id) as КоличествоПродаж,
                        SUM(si.Quantity) as ОбщееКоличество,
                        SUM(si.TotalPrice) as ОбщаяВыручка,
                        AVG(si.UnitPrice) as СредняяЦена
                    FROM SaleItems si
                    INNER JOIN Sales s ON si.SaleId = s.Id
                    INNER JOIN Products p ON si.ProductId = p.Id
                    WHERE 1=1";
                
                if (fromDate.HasValue)
                    query += " AND s.SaleDate >= @FromDate";
                if (toDate.HasValue)
                    query += " AND s.SaleDate <= @ToDate";
                
                query += @"
                    GROUP BY p.Category
                    ORDER BY ОбщаяВыручка DESC";

                var command = new SqlCommand(query, connection);
                if (fromDate.HasValue)
                    command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    command.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Отчет по продажам по дням
        public DataTable GetSalesByDay(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        CAST(SaleDate AS DATE) as Дата,
                        COUNT(*) as КоличествоПродаж,
                        SUM(TotalAmount) as СуммаПродаж,
                        AVG(TotalAmount) as СреднийЧек
                    FROM Sales
                    WHERE 1=1";
                
                if (fromDate.HasValue)
                    query += " AND SaleDate >= @FromDate";
                if (toDate.HasValue)
                    query += " AND SaleDate <= @ToDate";
                
                query += @"
                    GROUP BY CAST(SaleDate AS DATE)
                    ORDER BY Дата DESC";

                var command = new SqlCommand(query, connection);
                if (fromDate.HasValue)
                    command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    command.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Отчет по остаткам товаров
        public DataTable GetProductsStockReport()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        Name as Название,
                        Category as Категория,
                        Price as Цена,
                        Quantity as Остаток,
                        (Price * Quantity) as СтоимостьОстатка
                    FROM Products
                    ORDER BY Quantity ASC, Name";

                var command = new SqlCommand(query, connection);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Отчет по клиентам
        public DataTable GetCustomersReport(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        CustomerName as Клиент,
                        COUNT(*) as КоличествоПокупок,
                        SUM(TotalAmount) as ОбщаяСумма,
                        AVG(TotalAmount) as СреднийЧек,
                        MAX(SaleDate) as ПоследняяПокупка
                    FROM Sales
                    WHERE CustomerName IS NOT NULL AND CustomerName != ''
                    AND 1=1";
                
                if (fromDate.HasValue)
                    query += " AND SaleDate >= @FromDate";
                if (toDate.HasValue)
                    query += " AND SaleDate <= @ToDate";
                
                query += @"
                    GROUP BY CustomerName
                    ORDER BY ОбщаяСумма DESC";

                var command = new SqlCommand(query, connection);
                if (fromDate.HasValue)
                    command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    command.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Отчет по товарам с низким остатком
        public DataTable GetLowStockProducts(int threshold = 10)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        Name as Название,
                        Category as Категория,
                        Price as Цена,
                        Quantity as Остаток
                    FROM Products
                    WHERE Quantity <= @Threshold
                    ORDER BY Quantity ASC, Name";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Threshold", threshold);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Отчет по продажам по месяцам
        public DataTable GetSalesByMonth(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        FORMAT(SaleDate, 'yyyy-MM') as Месяц,
                        COUNT(*) as КоличествоПродаж,
                        SUM(TotalAmount) as СуммаПродаж,
                        AVG(TotalAmount) as СреднийЧек,
                        MIN(TotalAmount) as МинимальныйЧек,
                        MAX(TotalAmount) as МаксимальныйЧек
                    FROM Sales
                    WHERE 1=1";
                
                if (fromDate.HasValue)
                    query += " AND SaleDate >= @FromDate";
                if (toDate.HasValue)
                    query += " AND SaleDate <= @ToDate";
                
                query += @"
                    GROUP BY FORMAT(SaleDate, 'yyyy-MM')
                    ORDER BY Месяц DESC";

                var command = new SqlCommand(query, connection);
                if (fromDate.HasValue)
                    command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    command.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Отчет по прибыльности товаров
        public DataTable GetProductProfitability(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        p.Name as Товар,
                        p.Category as Категория,
                        p.Price as ЦенаЗакупки,
                        SUM(si.Quantity) as Продано,
                        SUM(si.TotalPrice) as Выручка,
                        (SUM(si.TotalPrice) - (p.Price * SUM(si.Quantity))) as Прибыль,
                        (SUM(si.TotalPrice) / NULLIF(SUM(si.Quantity), 0)) as СредняяЦенаПродажи
                    FROM SaleItems si
                    INNER JOIN Sales s ON si.SaleId = s.Id
                    INNER JOIN Products p ON si.ProductId = p.Id
                    WHERE 1=1";
                
                if (fromDate.HasValue)
                    query += " AND s.SaleDate >= @FromDate";
                if (toDate.HasValue)
                    query += " AND s.SaleDate <= @ToDate";
                
                query += @"
                    GROUP BY p.Name, p.Category, p.Price
                    ORDER BY Прибыль DESC";

                var command = new SqlCommand(query, connection);
                if (fromDate.HasValue)
                    command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    command.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Отчет по оборачиваемости товаров
        public DataTable GetProductTurnover(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        p.Name as Товар,
                        p.Category as Категория,
                        p.Quantity as ТекущийОстаток,
                        SUM(si.Quantity) as Продано,
                        CASE 
                            WHEN p.Quantity > 0 THEN CAST(SUM(si.Quantity) AS FLOAT) / p.Quantity
                            ELSE 0
                        END as Оборачиваемость,
                        AVG(DATEDIFF(day, s.SaleDate, GETDATE())) as СреднийСрокХранения
                    FROM Products p
                    LEFT JOIN SaleItems si ON p.Id = si.ProductId
                    LEFT JOIN Sales s ON si.SaleId = s.Id";
                
                if (fromDate.HasValue || toDate.HasValue)
                {
                    query += " WHERE 1=1";
                    if (fromDate.HasValue)
                        query += " AND s.SaleDate >= @FromDate";
                    if (toDate.HasValue)
                        query += " AND s.SaleDate <= @ToDate";
                }
                
                query += @"
                    GROUP BY p.Name, p.Category, p.Quantity
                    ORDER BY Оборачиваемость DESC";

                var command = new SqlCommand(query, connection);
                if (fromDate.HasValue)
                    command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    command.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Отчет по динамике продаж
        public DataTable GetSalesDynamics(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        CAST(SaleDate AS DATE) as Дата,
                        COUNT(*) as КоличествоПродаж,
                        SUM(TotalAmount) as СуммаПродаж,
                        COUNT(DISTINCT CustomerName) as УникальныхКлиентов,
                        AVG(TotalAmount) as СреднийЧек
                    FROM Sales
                    WHERE 1=1";
                
                if (fromDate.HasValue)
                    query += " AND SaleDate >= @FromDate";
                if (toDate.HasValue)
                    query += " AND SaleDate <= @ToDate";
                
                query += @"
                    GROUP BY CAST(SaleDate AS DATE)
                    ORDER BY Дата DESC";

                var command = new SqlCommand(query, connection);
                if (fromDate.HasValue)
                    command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    command.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Отчет по среднему чеку
        public DataTable GetAverageCheckReport(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        FORMAT(SaleDate, 'yyyy-MM-dd') as Дата,
                        COUNT(*) as КоличествоЧеков,
                        AVG(TotalAmount) as СреднийЧек,
                        MIN(TotalAmount) as МинимальныйЧек,
                        MAX(TotalAmount) as МаксимальныйЧек,
                        SUM(TotalAmount) as ОбщаяСумма
                    FROM Sales
                    WHERE 1=1";
                
                if (fromDate.HasValue)
                    query += " AND SaleDate >= @FromDate";
                if (toDate.HasValue)
                    query += " AND SaleDate <= @ToDate";
                
                query += @"
                    GROUP BY FORMAT(SaleDate, 'yyyy-MM-dd')
                    ORDER BY Дата DESC";

                var command = new SqlCommand(query, connection);
                if (fromDate.HasValue)
                    command.Parameters.AddWithValue("@FromDate", fromDate.Value);
                if (toDate.HasValue)
                    command.Parameters.AddWithValue("@ToDate", toDate.Value.AddDays(1).AddSeconds(-1));

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Отчет по остаткам по категориям
        public DataTable GetStockByCategory()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        Category as Категория,
                        COUNT(*) as КоличествоТоваров,
                        SUM(Quantity) as ОбщийОстаток,
                        AVG(Price) as СредняяЦена,
                        SUM(Price * Quantity) as СтоимостьОстатка
                    FROM Products
                    WHERE Category IS NOT NULL
                    GROUP BY Category
                    ORDER BY СтоимостьОстатка DESC";

                var command = new SqlCommand(query, connection);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }
}

