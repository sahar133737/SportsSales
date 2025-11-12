using System;
using System.Data.SqlClient;
using VANEK2.Models;

namespace VANEK2
{
    public static class AuthHelper
    {
        private static User currentUser;
        private static string connectionString = @"Server=SAHAR\SQLSERVER;Database=SportInventoryDB;Integrated Security=true;";

        public static User CurrentUser
        {
            get { return currentUser; }
        }

        public static bool IsLoggedIn
        {
            get { return currentUser != null && currentUser.IsActive; }
        }

        public static bool IsAdmin
        {
            get { return IsLoggedIn && currentUser.Role == "Admin"; }
        }

        public static bool IsManager
        {
            get { return IsLoggedIn && (currentUser.Role == "Admin" || currentUser.Role == "Manager"); }
        }

        public static bool CanManageProducts
        {
            get { return IsManager; } // Возвращает true для Admin и Manager
        }

        public static bool CanDeleteProducts
        {
            get { return IsAdmin || IsManager; } // Явная проверка для удаления
        }

        public static bool CanEditProducts
        {
            get { return IsAdmin || IsManager; } // Явная проверка для редактирования
        }

        public static bool CanAddProducts
        {
            get { return IsAdmin || IsManager; } // Явная проверка для добавления
        }

        public static bool CanViewReports
        {
            get { return IsLoggedIn; }
        }

        public static bool Login(string username, string password)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "SELECT * FROM Users WHERE Username = @Username AND IsActive = 1",
                        connection);
                    command.Parameters.AddWithValue("@Username", username);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = reader["Password"].ToString();
                            
                            // Простая проверка пароля (в реальном приложении нужно использовать хеширование)
                            if (storedPassword == password)
                            {
                                currentUser = new User
                                {
                                    Id = (int)reader["Id"],
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    Role = reader["Role"].ToString(),
                                    IsActive = (bool)reader["IsActive"],
                                    CreatedDate = (DateTime)reader["CreatedDate"]
                                };
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка входа: {ex.Message}");
            }
            
            return false;
        }

        public static void Logout()
        {
            currentUser = null;
        }
    }
}

