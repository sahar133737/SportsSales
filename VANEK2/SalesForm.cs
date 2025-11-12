using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VANEK2.Models;

namespace VANEK2
{
    public partial class SalesForm : Form
    {
        private DataGridView dgvSales;
        private DataGridView dgvSaleItems;
        private Button btnNewSale;
        private Button btnRefresh;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Label lblFromDate;
        private Label lblToDate;
        private DatabaseHelper dbHelper;

        public SalesForm()
        {
            dbHelper = new DatabaseHelper();
            InitializeComponent();
            ApplyModernStyle();
            LoadSales();
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            
            // Стиль кнопок
            btnNewSale.BackColor = Color.FromArgb(46, 125, 50);
            btnNewSale.ForeColor = Color.White;
            btnNewSale.FlatStyle = FlatStyle.Flat;
            btnNewSale.FlatAppearance.BorderSize = 0;
            btnNewSale.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            btnRefresh.BackColor = Color.FromArgb(70, 130, 180);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            // Стиль DataGridView
            StyleDataGridView(dgvSales);
            StyleDataGridView(dgvSaleItems);
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = Color.FromArgb(230, 230, 230);
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 130, 180);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(70, 130, 180);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
        }

        private void InitializeComponent()
        {
            this.dgvSales = new DataGridView();
            this.dgvSaleItems = new DataGridView();
            this.btnNewSale = new Button();
            this.btnRefresh = new Button();
            this.dtpFromDate = new DateTimePicker();
            this.dtpToDate = new DateTimePicker();
            this.lblFromDate = new Label();
            this.lblToDate = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSaleItems)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new System.Drawing.Point(10, 15);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(14, 13);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "С:";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Location = new System.Drawing.Point(30, 12);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(150, 20);
            this.dtpFromDate.TabIndex = 1;
            this.dtpFromDate.Value = DateTime.Now.AddMonths(-1);
            this.dtpFromDate.ValueChanged += new EventHandler(FilterChanged);
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(190, 15);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(21, 13);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "По:";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Location = new System.Drawing.Point(217, 12);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(150, 20);
            this.dtpToDate.TabIndex = 3;
            this.dtpToDate.Value = DateTime.Now;
            this.dtpToDate.ValueChanged += new EventHandler(FilterChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(380, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 25);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new EventHandler(btnRefresh_Click);
            // 
            // btnNewSale
            // 
            this.btnNewSale.Location = new System.Drawing.Point(490, 10);
            this.btnNewSale.Name = "btnNewSale";
            this.btnNewSale.Size = new System.Drawing.Size(120, 25);
            this.btnNewSale.TabIndex = 5;
            this.btnNewSale.Text = "Новая продажа";
            this.btnNewSale.UseVisualStyleBackColor = true;
            this.btnNewSale.Click += new EventHandler(btnNewSale_Click);
            // 
            // dgvSales
            // 
            this.dgvSales.AllowUserToAddRows = false;
            this.dgvSales.AllowUserToDeleteRows = false;
            this.dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSales.Location = new System.Drawing.Point(10, 45);
            this.dgvSales.Name = "dgvSales";
            this.dgvSales.ReadOnly = true;
            this.dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvSales.Size = new System.Drawing.Size(600, 250);
            this.dgvSales.TabIndex = 6;
            this.dgvSales.SelectionChanged += new EventHandler(dgvSales_SelectionChanged);
            // 
            // dgvSaleItems
            // 
            this.dgvSaleItems.AllowUserToAddRows = false;
            this.dgvSaleItems.AllowUserToDeleteRows = false;
            this.dgvSaleItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSaleItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSaleItems.Location = new System.Drawing.Point(10, 305);
            this.dgvSaleItems.Name = "dgvSaleItems";
            this.dgvSaleItems.ReadOnly = true;
            this.dgvSaleItems.Size = new System.Drawing.Size(970, 260);
            this.dgvSaleItems.TabIndex = 7;
            // 
            // SalesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 570);
            this.Controls.Add(this.dgvSaleItems);
            this.Controls.Add(this.dgvSales);
            this.Controls.Add(this.btnNewSale);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.lblFromDate);
            this.Name = "SalesForm";
            this.Text = "Продажи";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSaleItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadSales()
        {
            try
            {
                var sales = dbHelper.GetAllSales(dtpFromDate.Value, dtpToDate.Value);
                dgvSales.DataSource = sales.Select(s => new
                {
                    s.Id,
                    Дата = s.SaleDate.ToString("dd.MM.yyyy HH:mm"),
                    Сумма = s.TotalAmount,
                    Клиент = s.CustomerName ?? "",
                    Примечания = s.Notes ?? ""
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продаж: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSales_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSales.SelectedRows.Count > 0)
            {
                var selectedRow = dgvSales.SelectedRows[0];
                int saleId = (int)selectedRow.Cells["Id"].Value;
                LoadSaleItems(saleId);
            }
            else
            {
                dgvSaleItems.DataSource = null;
            }
        }

        private void LoadSaleItems(int saleId)
        {
            try
            {
                var sales = dbHelper.GetAllSales();
                var sale = sales.FirstOrDefault(s => s.Id == saleId);
                if (sale != null && sale.Items != null)
                {
                    dgvSaleItems.DataSource = sale.Items.Select(i => new
                    {
                        Товар = i.ProductName,
                        Количество = i.Quantity,
                        Цена = i.UnitPrice,
                        Сумма = i.TotalPrice
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки позиций: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            LoadSales();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSales();
        }

        private void btnNewSale_Click(object sender, EventArgs e)
        {
            using (var form = new NewSaleForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadSales();
                }
            }
        }
    }
}

