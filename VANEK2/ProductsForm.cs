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
            ApplyModernStyle(); // Сначала применяем стили
            ApplyPermissions(); // Затем применяем права (это может изменить видимость)
            LoadProducts();
            
            // Принудительно обновляем форму после инициализации
            this.Refresh();
            
            // Дополнительная проверка после загрузки
            this.Shown += ProductsForm_Shown;
        }
        
        private void ProductsForm_Shown(object sender, EventArgs e)
        {
            // Обновляем позиции кнопок после показа формы
            UpdateButtonPositions();
            
            // Обновляем размер DataGridView
            if (dgvProducts != null && this.ClientSize.Width > 0 && this.ClientSize.Height > 0)
            {
                dgvProducts.Size = new System.Drawing.Size(
                    this.ClientSize.Width - 20, 
                    this.ClientSize.Height - 60
                );
            }
            
            // После показа формы еще раз применяем права доступа
            ApplyPermissions();
            
            // Обновляем layout после показа формы
            this.PerformLayout();
            this.Invalidate();
            this.Update();
            
            // Проверяем видимость кнопок
            if (AuthHelper.CanManageProducts)
            {
                System.Diagnostics.Debug.WriteLine($"Форма показана. Кнопки должны быть видны: Add={btnAdd.Visible}, Edit={btnEdit.Visible}, Delete={btnDelete.Visible}");
            }
        }
        
        private void UpdateButtonPositions()
        {
            // Обновляем позиции кнопок справа налево с большими отступами
            int buttonWidth = 110;
            int buttonSpacing = 25; // Увеличенный отступ между кнопками для лучшего разделения
            int rightMargin = 20;
            
            btnDelete.Location = new System.Drawing.Point(this.ClientSize.Width - rightMargin - buttonWidth, 10);
            btnEdit.Location = new System.Drawing.Point(this.ClientSize.Width - rightMargin - buttonWidth * 2 - buttonSpacing, 10);
            btnAdd.Location = new System.Drawing.Point(this.ClientSize.Width - rightMargin - buttonWidth * 3 - buttonSpacing * 2, 10);
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (btnAdd != null && btnEdit != null && btnDelete != null)
            {
                UpdateButtonPositions();
            }
            
            // Обновляем размер DataGridView при изменении размера формы
            if (dgvProducts != null && this.ClientSize.Width > 0 && this.ClientSize.Height > 0)
            {
                dgvProducts.Size = new System.Drawing.Size(
                    this.ClientSize.Width - 20, 
                    this.ClientSize.Height - 60
                );
            }
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
            System.Diagnostics.Debug.WriteLine($"Кнопки в Controls: Add={this.Controls.Contains(btnAdd)}, Edit={this.Controls.Contains(btnEdit)}, Delete={this.Controls.Contains(btnDelete)}");
            System.Diagnostics.Debug.WriteLine($"Позиции кнопок: Add=({btnAdd.Location.X},{btnAdd.Location.Y}), Edit=({btnEdit.Location.X},{btnEdit.Location.Y}), Delete=({btnDelete.Location.X},{btnDelete.Location.Y})");
            
            // Принудительно показываем кнопки если у пользователя есть права
            if (canManage)
            {
                // Убеждаемся, что кнопки видны и на переднем плане
                btnAdd.BringToFront();
                btnEdit.BringToFront();
                btnDelete.BringToFront();
                
                // Принудительно устанавливаем видимость
                btnAdd.Show();
                btnEdit.Show();
                btnDelete.Show();
                
                // Обновляем родительский контейнер
                if (this.Parent != null)
                {
                    this.Parent.Invalidate();
                    this.Parent.Update();
                }
            }
            
            // Принудительное обновление формы
            this.Invalidate();
            this.Update();
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            
            // Стиль кнопки "Добавить" - зеленая с эффектами
            if (btnAdd != null)
            {
                btnAdd.BackColor = Color.FromArgb(46, 125, 50);
                btnAdd.ForeColor = Color.White;
                btnAdd.FlatStyle = FlatStyle.Flat;
                btnAdd.FlatAppearance.BorderSize = 0;
                btnAdd.FlatAppearance.MouseOverBackColor = Color.FromArgb(56, 142, 60);
                btnAdd.FlatAppearance.MouseDownBackColor = Color.FromArgb(27, 94, 32);
                btnAdd.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
                btnAdd.Cursor = Cursors.Hand;
                // Добавляем небольшую тень через Padding
                btnAdd.Padding = new Padding(0, 0, 0, 2);
            }

            // Стиль кнопки "Изменить" - синяя с эффектами
            if (btnEdit != null)
            {
                btnEdit.BackColor = Color.FromArgb(25, 118, 210);
                btnEdit.ForeColor = Color.White;
                btnEdit.FlatStyle = FlatStyle.Flat;
                btnEdit.FlatAppearance.BorderSize = 0;
                btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 136, 229);
                btnEdit.FlatAppearance.MouseDownBackColor = Color.FromArgb(13, 71, 161);
                btnEdit.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
                btnEdit.Cursor = Cursors.Hand;
                btnEdit.Padding = new Padding(0, 0, 0, 2);
            }

            // Стиль кнопки "Удалить" - красная с эффектами
            if (btnDelete != null)
            {
                btnDelete.BackColor = Color.FromArgb(198, 40, 40);
                btnDelete.ForeColor = Color.White;
                btnDelete.FlatStyle = FlatStyle.Flat;
                btnDelete.FlatAppearance.BorderSize = 0;
                btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(211, 47, 47);
                btnDelete.FlatAppearance.MouseDownBackColor = Color.FromArgb(183, 28, 28);
                btnDelete.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
                btnDelete.Cursor = Cursors.Hand;
                btnDelete.Padding = new Padding(0, 0, 0, 2);
            }

            // Стиль кнопки "Обновить"
            if (btnRefresh != null)
            {
                btnRefresh.BackColor = Color.FromArgb(70, 130, 180);
                btnRefresh.ForeColor = Color.White;
                btnRefresh.FlatStyle = FlatStyle.Flat;
                btnRefresh.FlatAppearance.BorderSize = 0;
                btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(85, 150, 205);
                btnRefresh.FlatAppearance.MouseDownBackColor = Color.FromArgb(55, 110, 155);
                btnRefresh.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
                btnRefresh.Cursor = Cursors.Hand;
            }

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
            this.lblSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(58, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(270, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.btnRefresh.Click += new EventHandler(btnRefresh_Click);
            
            // 
            // btnDelete (самая правая кнопка)
            // 
            this.btnDelete.Size = new System.Drawing.Size(110, 35);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = true;
            this.btnDelete.Enabled = true;
            this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnDelete.Location = new System.Drawing.Point(800, 10); // Временная позиция, будет обновлена
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            
            // 
            // btnEdit (слева от Delete)
            // 
            this.btnEdit.Size = new System.Drawing.Size(110, 35);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Изменить";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Visible = true;
            this.btnEdit.Enabled = true;
            this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnEdit.Location = new System.Drawing.Point(690, 10); // Временная позиция, будет обновлена
            this.btnEdit.Click += new EventHandler(btnEdit_Click);
            
            // 
            // btnAdd (слева от Edit)
            // 
            this.btnAdd.Size = new System.Drawing.Size(110, 35);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = true;
            this.btnAdd.Enabled = true;
            this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnAdd.Location = new System.Drawing.Point(580, 10); // Временная позиция, будет обновлена
            this.btnAdd.Click += new EventHandler(btnAdd_Click);
            
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
                | AnchorStyles.Left) 
                | AnchorStyles.Right)));
            this.dgvProducts.Location = new System.Drawing.Point(10, 50);
            this.dgvProducts.Margin = new Padding(0);
            this.dgvProducts.Dock = DockStyle.None; // Убеждаемся, что Dock не установлен
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.TabIndex = 6;
            
            // 
            // ProductsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(10);
            
            // ВАЖНО: Добавляем контролы в правильном порядке
            // Сначала добавляем DataGridView (он будет внизу по Z-order)
            this.Controls.Add(this.dgvProducts);
            
            // Затем добавляем кнопки и элементы управления (они будут поверх DataGridView)
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblSearch);
            
            // Устанавливаем порядок отображения (Z-order) - кнопки должны быть поверх DataGridView
            this.Controls.SetChildIndex(this.dgvProducts, 0);
            this.Controls.SetChildIndex(this.lblSearch, 1);
            this.Controls.SetChildIndex(this.txtSearch, 2);
            this.Controls.SetChildIndex(this.btnRefresh, 3);
            this.Controls.SetChildIndex(this.btnAdd, 4);
            this.Controls.SetChildIndex(this.btnEdit, 5);
            this.Controls.SetChildIndex(this.btnDelete, 6);
            
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
                
                // Скрываем колонку ID
                if (dgvProducts.Columns["Id"] != null)
                {
                    dgvProducts.Columns["Id"].Visible = false;
                }
                
                // Убеждаемся, что DataGridView растягивается правильно
                dgvProducts.Size = new System.Drawing.Size(
                    this.ClientSize.Width - 20, 
                    this.ClientSize.Height - 60
                );
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
                
                // Скрываем колонку ID после фильтрации
                if (dgvProducts.Columns["Id"] != null)
                {
                    dgvProducts.Columns["Id"].Visible = false;
                }
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

