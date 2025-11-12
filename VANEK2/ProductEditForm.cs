using System;
using System.Drawing;
using System.Windows.Forms;
using VANEK2.Models;

namespace VANEK2
{
    public partial class ProductEditForm : Form
    {
        private Label lblName;
        private Label lblCategory;
        private Label lblPrice;
        private Label lblQuantity;
        private Label lblDescription;
        private TextBox txtName;
        private TextBox txtCategory;
        private NumericUpDown numPrice;
        private NumericUpDown numQuantity;
        private TextBox txtDescription;
        private Button btnSave;
        private Button btnCancel;
        private Product product;
        private DatabaseHelper dbHelper;

        public ProductEditForm(Product product = null)
        {
            dbHelper = new DatabaseHelper();
            this.product = product;
            InitializeComponent();
            ApplyModernStyle();
            if (product != null)
            {
                LoadProduct();
            }
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            
            // Стиль кнопок
            btnSave.BackColor = Color.FromArgb(46, 125, 50);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            btnCancel.BackColor = Color.FromArgb(120, 120, 120);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
        }

        private void InitializeComponent()
        {
            this.lblName = new Label();
            this.lblCategory = new Label();
            this.lblPrice = new Label();
            this.lblQuantity = new Label();
            this.lblDescription = new Label();
            this.txtName = new TextBox();
            this.txtCategory = new TextBox();
            this.numPrice = new NumericUpDown();
            this.numQuantity = new NumericUpDown();
            this.txtDescription = new TextBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(20, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(60, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Название:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(120, 17);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(300, 20);
            this.txtName.TabIndex = 1;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(20, 55);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(63, 13);
            this.lblCategory.TabIndex = 2;
            this.lblCategory.Text = "Категория:";
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(120, 52);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(300, 20);
            this.txtCategory.TabIndex = 3;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(20, 90);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(36, 13);
            this.lblPrice.TabIndex = 4;
            this.lblPrice.Text = "Цена:";
            // 
            // numPrice
            // 
            this.numPrice.DecimalPlaces = 2;
            this.numPrice.Location = new System.Drawing.Point(120, 87);
            this.numPrice.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new System.Drawing.Size(300, 20);
            this.numPrice.TabIndex = 5;
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(20, 125);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(69, 13);
            this.lblQuantity.TabIndex = 6;
            this.lblQuantity.Text = "Количество:";
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(120, 122);
            this.numQuantity.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numQuantity.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(300, 20);
            this.numQuantity.TabIndex = 7;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(20, 160);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.TabIndex = 8;
            this.lblDescription.Text = "Описание:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(120, 157);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(300, 100);
            this.txtDescription.TabIndex = 9;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(265, 280);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(345, 280);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // ProductEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 330);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.numPrice);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductEditForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = product == null ? "Добавить товар" : "Изменить товар";
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadProduct()
        {
            txtName.Text = product.Name;
            txtCategory.Text = product.Category ?? "";
            numPrice.Value = product.Price;
            numQuantity.Value = product.Quantity;
            txtDescription.Text = product.Description ?? "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (product == null)
                {
                    product = new Product();
                }

                product.Name = txtName.Text.Trim();
                product.Category = txtCategory.Text.Trim();
                product.Price = numPrice.Value;
                product.Quantity = (int)numQuantity.Value;
                product.Description = txtDescription.Text.Trim();

                if (product.Id == 0)
                {
                    dbHelper.AddProduct(product);
                    MessageBox.Show("Товар успешно добавлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dbHelper.UpdateProduct(product);
                    MessageBox.Show("Товар успешно изменен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

