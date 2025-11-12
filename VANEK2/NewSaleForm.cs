using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VANEK2.Models;

namespace VANEK2
{
    public partial class NewSaleForm : Form
    {
        private DataGridView dgvCart;
        private ComboBox cmbProduct;
        private NumericUpDown numQuantity;
        private Button btnAddToCart;
        private Button btnRemoveFromCart;
        private Button btnCompleteSale;
        private Button btnCancel;
        private TextBox txtCustomerName;
        private TextBox txtNotes;
        private Label lblCustomerName;
        private Label lblNotes;
        private Label lblTotal;
        private Label lblTotalValue;
        private DatabaseHelper dbHelper;
        private List<SaleItem> cartItems;
        private List<Product> availableProducts;

        public NewSaleForm()
        {
            dbHelper = new DatabaseHelper();
            cartItems = new List<SaleItem>();
            InitializeComponent();
            ApplyModernStyle();
            LoadProducts();
            UpdateTotal();
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            
            // Стиль кнопок
            btnAddToCart.BackColor = Color.FromArgb(46, 125, 50);
            btnAddToCart.ForeColor = Color.White;
            btnAddToCart.FlatStyle = FlatStyle.Flat;
            btnAddToCart.FlatAppearance.BorderSize = 0;
            btnAddToCart.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            btnRemoveFromCart.BackColor = Color.FromArgb(198, 40, 40);
            btnRemoveFromCart.ForeColor = Color.White;
            btnRemoveFromCart.FlatStyle = FlatStyle.Flat;
            btnRemoveFromCart.FlatAppearance.BorderSize = 0;
            btnRemoveFromCart.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            btnCompleteSale.BackColor = Color.FromArgb(46, 125, 50);
            btnCompleteSale.ForeColor = Color.White;
            btnCompleteSale.FlatStyle = FlatStyle.Flat;
            btnCompleteSale.FlatAppearance.BorderSize = 0;
            btnCompleteSale.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);

            btnCancel.BackColor = Color.FromArgb(120, 120, 120);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            // Стиль DataGridView
            dgvCart.BackgroundColor = Color.White;
            dgvCart.BorderStyle = BorderStyle.None;
            dgvCart.GridColor = Color.FromArgb(230, 230, 230);
            dgvCart.DefaultCellStyle.BackColor = Color.White;
            dgvCart.DefaultCellStyle.ForeColor = Color.Black;
            dgvCart.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 130, 180);
            dgvCart.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvCart.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(70, 130, 180);
            dgvCart.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCart.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);

            // Стиль метки итого
            lblTotal.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            lblTotalValue.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            lblTotalValue.ForeColor = Color.FromArgb(46, 125, 50);
        }

        private void InitializeComponent()
        {
            this.dgvCart = new DataGridView();
            this.cmbProduct = new ComboBox();
            this.numQuantity = new NumericUpDown();
            this.btnAddToCart = new Button();
            this.btnRemoveFromCart = new Button();
            this.btnCompleteSale = new Button();
            this.btnCancel = new Button();
            this.txtCustomerName = new TextBox();
            this.txtNotes = new TextBox();
            this.lblCustomerName = new Label();
            this.lblNotes = new Label();
            this.lblTotal = new Label();
            this.lblTotalValue = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Location = new System.Drawing.Point(20, 20);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(47, 13);
            this.lblCustomerName.TabIndex = 0;
            this.lblCustomerName.Text = "Клиент:";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Location = new System.Drawing.Point(80, 17);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(300, 20);
            this.txtCustomerName.TabIndex = 1;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(400, 20);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(70, 13);
            this.lblNotes.TabIndex = 2;
            this.lblNotes.Text = "Примечания:";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(480, 17);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(300, 20);
            this.txtNotes.TabIndex = 3;
            // 
            // cmbProduct
            // 
            this.cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(20, 50);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(300, 21);
            this.cmbProduct.TabIndex = 4;
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(330, 50);
            this.numQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(100, 20);
            this.numQuantity.TabIndex = 5;
            this.numQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnAddToCart
            // 
            this.btnAddToCart.Location = new System.Drawing.Point(440, 48);
            this.btnAddToCart.Name = "btnAddToCart";
            this.btnAddToCart.Size = new System.Drawing.Size(120, 25);
            this.btnAddToCart.TabIndex = 6;
            this.btnAddToCart.Text = "Добавить";
            this.btnAddToCart.UseVisualStyleBackColor = true;
            this.btnAddToCart.Click += new EventHandler(btnAddToCart_Click);
            // 
            // dgvCart
            // 
            this.dgvCart.AllowUserToAddRows = false;
            this.dgvCart.AllowUserToDeleteRows = false;
            this.dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCart.Location = new System.Drawing.Point(20, 85);
            this.dgvCart.Name = "dgvCart";
            this.dgvCart.ReadOnly = true;
            this.dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCart.Size = new System.Drawing.Size(760, 300);
            this.dgvCart.TabIndex = 7;
            // 
            // btnRemoveFromCart
            // 
            this.btnRemoveFromCart.Location = new System.Drawing.Point(20, 395);
            this.btnRemoveFromCart.Name = "btnRemoveFromCart";
            this.btnRemoveFromCart.Size = new System.Drawing.Size(120, 30);
            this.btnRemoveFromCart.TabIndex = 8;
            this.btnRemoveFromCart.Text = "Удалить";
            this.btnRemoveFromCart.UseVisualStyleBackColor = true;
            this.btnRemoveFromCart.Click += new EventHandler(btnRemoveFromCart_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(500, 400);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(60, 17);
            this.lblTotal.TabIndex = 9;
            this.lblTotal.Text = "Итого:";
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalValue.Location = new System.Drawing.Point(570, 400);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(36, 17);
            this.lblTotalValue.TabIndex = 10;
            this.lblTotalValue.Text = "0 ₽";
            // 
            // btnCompleteSale
            // 
            this.btnCompleteSale.Location = new System.Drawing.Point(600, 440);
            this.btnCompleteSale.Name = "btnCompleteSale";
            this.btnCompleteSale.Size = new System.Drawing.Size(90, 35);
            this.btnCompleteSale.TabIndex = 11;
            this.btnCompleteSale.Text = "Оформить";
            this.btnCompleteSale.UseVisualStyleBackColor = true;
            this.btnCompleteSale.Click += new EventHandler(btnCompleteSale_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(690, 440);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 35);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // NewSaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 490);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCompleteSale);
            this.Controls.Add(this.lblTotalValue);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnRemoveFromCart);
            this.Controls.Add(this.dgvCart);
            this.Controls.Add(this.btnAddToCart);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.cmbProduct);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtCustomerName);
            this.Controls.Add(this.lblCustomerName);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewSaleForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Новая продажа";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadProducts()
        {
            try
            {
                availableProducts = dbHelper.GetAllProducts().Where(p => p.Quantity > 0).ToList();
                cmbProduct.DataSource = availableProducts;
                cmbProduct.DisplayMember = "Name";
                cmbProduct.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var product = (Product)cmbProduct.SelectedItem;
            int quantity = (int)numQuantity.Value;

            if (quantity > product.Quantity)
            {
                MessageBox.Show($"Недостаточно товара на складе. Доступно: {product.Quantity}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existingItem = cartItems.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                if (existingItem.Quantity + quantity > product.Quantity)
                {
                    MessageBox.Show($"Недостаточно товара на складе. Доступно: {product.Quantity}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                cartItems.Add(new SaleItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    TotalPrice = quantity * product.Price
                });
            }

            UpdateCartDisplay();
            UpdateTotal();
        }

        private void btnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count > 0)
            {
                int index = dgvCart.SelectedRows[0].Index;
                cartItems.RemoveAt(index);
                UpdateCartDisplay();
                UpdateTotal();
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateCartDisplay()
        {
            dgvCart.DataSource = cartItems.Select(i => new
            {
                Товар = i.ProductName,
                Количество = i.Quantity,
                Цена = i.UnitPrice,
                Сумма = i.TotalPrice
            }).ToList();
        }

        private void UpdateTotal()
        {
            decimal total = cartItems.Sum(i => i.TotalPrice);
            lblTotalValue.Text = $"{total:N2} ₽";
        }

        private void btnCompleteSale_Click(object sender, EventArgs e)
        {
            if (cartItems.Count == 0)
            {
                MessageBox.Show("Добавьте товары в корзину", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var sale = new Sale
                {
                    SaleDate = DateTime.Now,
                    TotalAmount = cartItems.Sum(i => i.TotalPrice),
                    CustomerName = txtCustomerName.Text.Trim(),
                    Notes = txtNotes.Text.Trim(),
                    Items = cartItems
                };

                dbHelper.CreateSale(sale);
                MessageBox.Show("Продажа успешно оформлена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка оформления продажи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

