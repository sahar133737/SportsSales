using System;

namespace VANEK2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public enum UserRole
    {
        Admin,      // Администратор - полный доступ
        Manager,    // Менеджер - может управлять товарами и просматривать отчеты
        Seller      // Продавец - только продажи и просмотр товаров
    }
}

