using System;
using System.Windows.Forms;

namespace VANEK2
{
    // Вспомогательный класс для тестирования прав доступа
    public static class TestPermissions
    {
        public static void ShowCurrentPermissions()
        {
            if (!AuthHelper.IsLoggedIn)
            {
                MessageBox.Show("Пользователь не авторизован", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string info = $"Текущий пользователь: {AuthHelper.CurrentUser.FullName}\n";
            info += $"Роль: {AuthHelper.CurrentUser.Role}\n";
            info += $"Активен: {AuthHelper.CurrentUser.IsActive}\n\n";
            info += $"IsLoggedIn: {AuthHelper.IsLoggedIn}\n";
            info += $"IsAdmin: {AuthHelper.IsAdmin}\n";
            info += $"IsManager: {AuthHelper.IsManager}\n";
            info += $"CanManageProducts: {AuthHelper.CanManageProducts}\n";
            info += $"CanAddProducts: {AuthHelper.CanAddProducts}\n";
            info += $"CanEditProducts: {AuthHelper.CanEditProducts}\n";
            info += $"CanDeleteProducts: {AuthHelper.CanDeleteProducts}\n";
            info += $"CanViewReports: {AuthHelper.CanViewReports}";

            MessageBox.Show(info, "Информация о правах доступа", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

