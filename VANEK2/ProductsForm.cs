using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VANEK2.Models;

namespace VANEK2
{
    public partial class ProductsForm : Form
    {
        private DataGridView dgvProducts;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private TextBox txtSearch;
        private Label lblSearch;
        private DatabaseHelper dbHelper;

        public ProductsForm()
        {
            dbHelper = new DatabaseHelper();
            InitializeComponent();
            ApplyPermissions(); // Применяем права ДО применения стилей
            ApplyModernStyle();
            LoadProducts();
        }

        private void ApplyPermissions()
        {
            // Проверяем, что пользователь залогинен
            if (!AuthHelper.IsLoggedIn)
            {
                // Если не залогинен, скрываем все кнопки управления
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                this.Text = "Управление товарами (Не авторизован)";
                return;
            }

            // Администратор и Менеджер имеют полный доступ к управлению товарами
            bool canManage = AuthHelper.CanManageProducts;
            bool isAdmin = AuthHelper.IsAdmin;
            bool isManager = AuthHelper.IsManager;
            
            // Отладочная информация
            System.Diagnostics.Debug.WriteLine($"ApplyPermissions: IsLoggedIn={AuthHelper.IsLoggedIn}, IsAdmin={isAdmin}, IsManager={isManager}, CanManage={canManage}, Role={AuthHelper.CurrentUser?.Role}");
            
            // Устанавливаем видимость и доступность кнопок
            // ВАЖНО: Устанавливаем Visible и Enabled явно
            btnAdd.Visible = canManage;
            btnEdit.Visible = canManage;
            btnDelete.Visible = canManage;
            btnAdd.Enabled = canManage;
            btnEdit.Enabled = canManage;
            btnDelete.Enabled = canManage;
            
            // Принудительно обновляем отображение
            this.Invalidate();
            this.Update();
            
            // Визуальная индикация роли
            if (isAdmin)
            {
                this.Text = "Управление товарами (Администратор) - Полный доступ";
            }
            else if (isManager && !isAdmin)
            {
                this.Text = "Управление товарами (Менеджер)";
            }
            else
            {
                this.Text = "Просмотр товаров (Продавец) - Только просмотр";
            }
            
            // Дополнительная проверка для отладки
            System.Diagnostics.Debug.WriteLine($"Кнопки после ApplyPermissions: Add={btnAdd.Visible}, Edit={btnEdit.Visible}, Delete={btnDelete.Visible}");
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            
            // Стиль кнопок (применяем стили ко всем кнопкам для правильного отображения)
            btnAdd.BackColor = Color.FromArgb(46, 125, 50);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            btnEdit.BackColor = Color.FromArgb(25, 118, 210);
            btnEdit.ForeColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            btnDelete.BackColor = Color.FromArgb(198, 40, 40);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            btnRefresh.BackColor = Color.FromArgb(70, 130, 180);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            // Стиль DataGridView
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.GridColor = Color.FromArgb(230, 230, 230);
            dgvProducts.DefaultCellStyle.BackColor = Color.White;
            dgvProducts.DefaultCellStyle.ForeColor = Color.Black;
            dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 130, 180);
            dgvProducts.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(70, 130, 180);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
        }

        private void InitializeComponent()
        {
            this.dgvProducts = new DataGridView();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnRefresh = new Button();
            this.txtSearch = new TextBox();
            this.lblSearch = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(10, 15);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(42, 13);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Поиск:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(58, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(300, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(370, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 25);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new EventHandler(btnRefresh_Click);
            // 
            // Примечание: кнопки управления товарами (Add, Edit, Delete) 
            // будут показаны/скрыты в методе ApplyPermissions() в зависимости от роли пользователя
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(480, 10);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 25);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = true; // По умолчанию видима
            this.btnAdd.Enabled = true; // По умолчанию включена
            this.btnAdd.Click += new EventHandler(btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(590, 10);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 25);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Изменить";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Visible = true; // По умолчанию видима
            this.btnEdit.Enabled = true; // По умолчанию включена
            this.btnEdit.Click += new EventHandler(btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(700, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 25);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = true; // По умолчанию видима
            this.btnDelete.Enabled = true; // По умолчанию включена
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new System.Drawing.Point(10, 45);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(970, 520);
            this.dgvProducts.TabIndex = 6;
            this.dgvProducts.Dock = DockStyle.Fill;
            // 
            // ProductsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 570);
            this.Controls.Add(this.dgvProducts);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblSearch);
            this.Name = "ProductsForm";
            this.Text = "Управление товарами";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadProducts()
        {
            try
            {
                var products = dbHelper.GetAllProducts();
                dgvProducts.DataSource = products.Select(p => new
                {
                    p.Id,
                    Название = p.Name,
                    Категория = p.Category,
                    Цена = p.Price,
                    Количество = p.Quantity,
                    Описание = p.Description
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dgvProducts.DataSource != null)
            {
                var products = dbHelper.GetAllProducts();
                var searchText = txtSearch.Text.ToLower();
                var filtered = products.Where(p =>
                    p.Name.ToLower().Contains(searchText) ||
                    (p.Category != null && p.Category.ToLower().Contains(searchText)) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchText))
                ).Select(p => new
                {
                    p.Id,
                    Название = p.Name,
                    Категория = p.Category,
                    Цена = p.Price,
                    Количество = p.Quantity,
                    Описание = p.Description
                }).ToList();
                dgvProducts.DataSource = filtered;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadProducts();
            // Обновляем права доступа при обновлении
            ApplyPermissions();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Дополнительная проверка прав доступа
            if (!AuthHelper.CanManageProducts)
            {
                MessageBox.Show("У вас нет прав для добавления товаров", "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var form = new ProductEditForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Дополнительная проверка прав доступа
            if (!AuthHelper.CanManageProducts)
            {
                MessageBox.Show("У вас нет прав для редактирования товаров", "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvProducts.SelectedRows.Count > 0)
            {
                var selectedRow = dgvProducts.SelectedRows[0];
                int productId = (int)selectedRow.Cells["Id"].Value;
                var product = dbHelper.GetProductById(productId);
                if (product != null)
                {
                    using (var form = new ProductEditForm(product))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadProducts();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Дополнительная проверка прав доступа
            if (!AuthHelper.CanManageProducts)
            {
                MessageBox.Show("У вас нет прав для удаления товаров", "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvProducts.SelectedRows.Count > 0)
            {
                var selectedRow = dgvProducts.SelectedRows[0];
                int productId = (int)selectedRow.Cells["Id"].Value;
                string productName = selectedRow.Cells["Название"].Value.ToString();

                // Дополнительное предупреждение для администратора
                string message = $"Вы уверены, что хотите удалить товар '{productName}'?";
                if (AuthHelper.IsAdmin)
                {
                    message += "\n\nВнимание: Вы выполняете операцию как администратор. Это действие нельзя отменить.";
                }

                if (MessageBox.Show(message, "Подтверждение удаления", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        dbHelper.DeleteProduct(productId);
                        LoadProducts();
                        MessageBox.Show("Товар успешно удален", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления товара: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

