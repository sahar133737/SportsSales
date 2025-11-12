using System;
using System.Drawing;
using System.Windows.Forms;

namespace VANEK2
{
    public partial class Form1 : Form
    {
        private ProductsForm productsForm;
        private SalesForm salesForm;
        private ReportsForm reportsForm;

        public Form1()
        {
            InitializeComponent();
            InitializeForms();
            UpdateUserInfo();
            ApplyModernStyle();
        }

        private void UpdateUserInfo()
        {
            if (AuthHelper.IsLoggedIn)
            {
                string roleText = AuthHelper.CurrentUser.Role == "Admin" ? "Администратор" :
                                 AuthHelper.CurrentUser.Role == "Manager" ? "Менеджер" : "Продавец";
                
                // Визуальная индикация для администратора
                if (AuthHelper.IsAdmin)
                {
                    lblUserInfo.Text = $"👑 {AuthHelper.CurrentUser.FullName} ({roleText}) - Полный доступ";
                    lblUserInfo.ForeColor = Color.FromArgb(198, 40, 40); // Красный цвет для администратора
                }
                else if (AuthHelper.IsManager && !AuthHelper.IsAdmin)
                {
                    lblUserInfo.Text = $"🔧 {AuthHelper.CurrentUser.FullName} ({roleText}) - Управление товарами";
                    lblUserInfo.ForeColor = Color.FromArgb(25, 118, 210); // Синий цвет для менеджера
                }
                else
                {
                    lblUserInfo.Text = $"👤 {AuthHelper.CurrentUser.FullName} ({roleText}) - Только продажи";
                    lblUserInfo.ForeColor = Color.FromArgb(70, 130, 180); // Голубой цвет для продавца
                }
            }
            else
            {
                lblUserInfo.Text = "Не авторизован";
                lblUserInfo.ForeColor = Color.Gray;
            }
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            tabControl.BackColor = Color.White;
            
            if (lblUserInfo != null)
            {
                lblUserInfo.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
                // Цвет будет установлен в UpdateUserInfo() в зависимости от роли
            }
            
            if (btnLogout != null)
            {
                btnLogout.BackColor = Color.FromArgb(198, 40, 40);
                btnLogout.ForeColor = Color.White;
                btnLogout.FlatStyle = FlatStyle.Flat;
                btnLogout.FlatAppearance.BorderSize = 0;
                btnLogout.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            }
        }

        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle tabBounds = tabControl.GetTabRect(e.Index);

            if (e.Index == tabControl.SelectedIndex)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(70, 130, 180)), tabBounds);
                e.Graphics.DrawString(tabPage.Text, tabControl.Font, new SolidBrush(Color.White), tabBounds, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), tabBounds);
                e.Graphics.DrawString(tabPage.Text, tabControl.Font, new SolidBrush(Color.Black), tabBounds, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
        }

        private void InitializeForms()
        {
            // Освобождаем старые формы, если они существуют
            if (productsForm != null)
            {
                productsForm.Dispose();
            }
            if (salesForm != null)
            {
                salesForm.Dispose();
            }
            if (reportsForm != null)
            {
                reportsForm.Dispose();
            }

            // Создаем формы для каждой вкладки
            productsForm = new ProductsForm();
            productsForm.TopLevel = false;
            productsForm.FormBorderStyle = FormBorderStyle.None;
            productsForm.Dock = DockStyle.Fill;
            tabProducts.Controls.Add(productsForm);
            productsForm.Show();

            salesForm = new SalesForm();
            salesForm.TopLevel = false;
            salesForm.FormBorderStyle = FormBorderStyle.None;
            salesForm.Dock = DockStyle.Fill;
            tabSales.Controls.Add(salesForm);
            salesForm.Show();

            reportsForm = new ReportsForm();
            reportsForm.TopLevel = false;
            reportsForm.FormBorderStyle = FormBorderStyle.None;
            reportsForm.Dock = DockStyle.Fill;
            tabReports.Controls.Add(reportsForm);
            reportsForm.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                AuthHelper.Logout();
                this.Hide();
                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        // Обновляем информацию о пользователе и перезагружаем формы
                        UpdateUserInfo();
                        // Пересоздаем формы для обновления прав доступа
                        tabProducts.Controls.Clear();
                        tabSales.Controls.Clear();
                        tabReports.Controls.Clear();
                        InitializeForms();
                        this.Show();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
        }
    }
}
