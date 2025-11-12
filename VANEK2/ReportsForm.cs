using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VANEK2.Models;

namespace VANEK2
{
    public partial class ReportsForm : Form
    {
        private DataGridView dgvReport;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Label lblFromDate;
        private Label lblToDate;
        private Button btnGenerateReport;
        private Button btnExport;
        private ComboBox cmbReportType;
        private Label lblReportType;
        private Panel pnlStatistics;
        private Label lblTotalSales;
        private Label lblTotalSalesValue;
        private Label lblSalesCount;
        private Label lblSalesCountValue;
        private Label lblAverageSale;
        private Label lblAverageSaleValue;
        private NumericUpDown numLowStockThreshold;
        private Label lblLowStockThreshold;
        private DatabaseHelper dbHelper;

        public ReportsForm()
        {
            dbHelper = new DatabaseHelper();
            InitializeComponent();
            LoadReport();
            ApplyModernStyle();
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            pnlStatistics.BackColor = Color.White;
            pnlStatistics.BorderStyle = BorderStyle.FixedSingle;
            
            // Стиль кнопок
            btnGenerateReport.BackColor = Color.FromArgb(70, 130, 180);
            btnGenerateReport.ForeColor = Color.White;
            btnGenerateReport.FlatStyle = FlatStyle.Flat;
            btnGenerateReport.FlatAppearance.BorderSize = 0;
            btnGenerateReport.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            btnExport.BackColor = Color.FromArgb(46, 125, 50);
            btnExport.ForeColor = Color.White;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            // Стиль DataGridView
            dgvReport.BackgroundColor = Color.White;
            dgvReport.BorderStyle = BorderStyle.None;
            dgvReport.GridColor = Color.FromArgb(230, 230, 230);
            dgvReport.DefaultCellStyle.BackColor = Color.White;
            dgvReport.DefaultCellStyle.ForeColor = Color.Black;
            dgvReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 130, 180);
            dgvReport.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(70, 130, 180);
            dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReport.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            dgvReport.EnableHeadersVisualStyles = false;
            dgvReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
        }

        private void InitializeComponent()
        {
            this.dgvReport = new DataGridView();
            this.dtpFromDate = new DateTimePicker();
            this.dtpToDate = new DateTimePicker();
            this.lblFromDate = new Label();
            this.lblToDate = new Label();
            this.btnGenerateReport = new Button();
            this.btnExport = new Button();
            this.cmbReportType = new ComboBox();
            this.lblReportType = new Label();
            this.pnlStatistics = new Panel();
            this.lblTotalSales = new Label();
            this.lblTotalSalesValue = new Label();
            this.lblSalesCount = new Label();
            this.lblSalesCountValue = new Label();
            this.lblAverageSale = new Label();
            this.lblAverageSaleValue = new Label();
            this.numLowStockThreshold = new NumericUpDown();
            this.lblLowStockThreshold = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLowStockThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // lblReportType
            // 
            this.lblReportType.AutoSize = true;
            this.lblReportType.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.lblReportType.Location = new System.Drawing.Point(15, 20);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(75, 15);
            this.lblReportType.TabIndex = 0;
            this.lblReportType.Text = "Тип отчета:";
            // 
            // cmbReportType
            // 
            this.cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbReportType.Font = new Font("Microsoft Sans Serif", 9F);
            this.cmbReportType.FormattingEnabled = true;
            this.cmbReportType.Items.AddRange(new object[] {
            "Все продажи",
            "Топ продаваемых товаров",
            "Продажи по категориям",
            "Продажи по дням",
            "Остатки товаров",
            "Товары с низким остатком",
            "Отчет по клиентам"});
            this.cmbReportType.Location = new System.Drawing.Point(95, 17);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new System.Drawing.Size(220, 23);
            this.cmbReportType.TabIndex = 1;
            this.cmbReportType.SelectedIndex = 0;
            this.cmbReportType.SelectedIndexChanged += new EventHandler(cmbReportType_SelectedIndexChanged);
            // 
            // lblLowStockThreshold
            // 
            this.lblLowStockThreshold.AutoSize = true;
            this.lblLowStockThreshold.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblLowStockThreshold.Location = new System.Drawing.Point(330, 20);
            this.lblLowStockThreshold.Name = "lblLowStockThreshold";
            this.lblLowStockThreshold.Size = new System.Drawing.Size(95, 15);
            this.lblLowStockThreshold.TabIndex = 2;
            this.lblLowStockThreshold.Text = "Порог остатка:";
            this.lblLowStockThreshold.Visible = false;
            // 
            // numLowStockThreshold
            // 
            this.numLowStockThreshold.Font = new Font("Microsoft Sans Serif", 9F);
            this.numLowStockThreshold.Location = new System.Drawing.Point(430, 17);
            this.numLowStockThreshold.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numLowStockThreshold.Name = "numLowStockThreshold";
            this.numLowStockThreshold.Size = new System.Drawing.Size(80, 21);
            this.numLowStockThreshold.TabIndex = 3;
            this.numLowStockThreshold.Value = new decimal(new int[] { 10, 0, 0, 0 });
            this.numLowStockThreshold.Visible = false;
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblFromDate.Location = new System.Drawing.Point(530, 20);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(20, 15);
            this.lblFromDate.TabIndex = 4;
            this.lblFromDate.Text = "С:";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Font = new Font("Microsoft Sans Serif", 9F);
            this.dtpFromDate.Location = new System.Drawing.Point(555, 17);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(150, 21);
            this.dtpFromDate.TabIndex = 5;
            this.dtpFromDate.Value = DateTime.Now.AddMonths(-1);
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblToDate.Location = new System.Drawing.Point(715, 20);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(25, 15);
            this.lblToDate.TabIndex = 6;
            this.lblToDate.Text = "По:";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Font = new Font("Microsoft Sans Serif", 9F);
            this.dtpToDate.Location = new System.Drawing.Point(745, 17);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(150, 21);
            this.dtpToDate.TabIndex = 7;
            this.dtpToDate.Value = DateTime.Now;
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Location = new System.Drawing.Point(905, 15);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(120, 28);
            this.btnGenerateReport.TabIndex = 8;
            this.btnGenerateReport.Text = "Сформировать";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new EventHandler(btnGenerateReport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(1030, 15);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 28);
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "Экспорт";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new EventHandler(btnExport_Click);
            // 
            // pnlStatistics
            // 
            this.pnlStatistics.Controls.Add(this.lblAverageSaleValue);
            this.pnlStatistics.Controls.Add(this.lblAverageSale);
            this.pnlStatistics.Controls.Add(this.lblSalesCountValue);
            this.pnlStatistics.Controls.Add(this.lblSalesCount);
            this.pnlStatistics.Controls.Add(this.lblTotalSalesValue);
            this.pnlStatistics.Controls.Add(this.lblTotalSales);
            this.pnlStatistics.Location = new System.Drawing.Point(15, 55);
            this.pnlStatistics.Name = "pnlStatistics";
            this.pnlStatistics.Size = new System.Drawing.Size(1115, 50);
            this.pnlStatistics.TabIndex = 10;
            // 
            // lblTotalSales
            // 
            this.lblTotalSales.AutoSize = true;
            this.lblTotalSales.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.lblTotalSales.Location = new System.Drawing.Point(15, 15);
            this.lblTotalSales.Name = "lblTotalSales";
            this.lblTotalSales.Size = new System.Drawing.Size(110, 17);
            this.lblTotalSales.TabIndex = 0;
            this.lblTotalSales.Text = "Общая сумма:";
            // 
            // lblTotalSalesValue
            // 
            this.lblTotalSalesValue.AutoSize = true;
            this.lblTotalSalesValue.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.lblTotalSalesValue.ForeColor = Color.FromArgb(46, 125, 50);
            this.lblTotalSalesValue.Location = new System.Drawing.Point(130, 15);
            this.lblTotalSalesValue.Name = "lblTotalSalesValue";
            this.lblTotalSalesValue.Size = new System.Drawing.Size(32, 17);
            this.lblTotalSalesValue.TabIndex = 1;
            this.lblTotalSalesValue.Text = "0 ₽";
            // 
            // lblSalesCount
            // 
            this.lblSalesCount.AutoSize = true;
            this.lblSalesCount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.lblSalesCount.Location = new System.Drawing.Point(300, 15);
            this.lblSalesCount.Name = "lblSalesCount";
            this.lblSalesCount.Size = new System.Drawing.Size(100, 17);
            this.lblSalesCount.TabIndex = 2;
            this.lblSalesCount.Text = "Количество:";
            // 
            // lblSalesCountValue
            // 
            this.lblSalesCountValue.AutoSize = true;
            this.lblSalesCountValue.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.lblSalesCountValue.ForeColor = Color.FromArgb(25, 118, 210);
            this.lblSalesCountValue.Location = new System.Drawing.Point(405, 15);
            this.lblSalesCountValue.Name = "lblSalesCountValue";
            this.lblSalesCountValue.Size = new System.Drawing.Size(16, 17);
            this.lblSalesCountValue.TabIndex = 3;
            this.lblSalesCountValue.Text = "0";
            // 
            // lblAverageSale
            // 
            this.lblAverageSale.AutoSize = true;
            this.lblAverageSale.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.lblAverageSale.Location = new System.Drawing.Point(550, 15);
            this.lblAverageSale.Name = "lblAverageSale";
            this.lblAverageSale.Size = new System.Drawing.Size(95, 17);
            this.lblAverageSale.TabIndex = 4;
            this.lblAverageSale.Text = "Средний чек:";
            // 
            // lblAverageSaleValue
            // 
            this.lblAverageSaleValue.AutoSize = true;
            this.lblAverageSaleValue.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.lblAverageSaleValue.ForeColor = Color.FromArgb(156, 39, 176);
            this.lblAverageSaleValue.Location = new System.Drawing.Point(650, 15);
            this.lblAverageSaleValue.Name = "lblAverageSaleValue";
            this.lblAverageSaleValue.Size = new System.Drawing.Size(32, 17);
            this.lblAverageSaleValue.TabIndex = 5;
            this.lblAverageSaleValue.Text = "0 ₽";
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Location = new System.Drawing.Point(15, 115);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.Size = new System.Drawing.Size(1115, 450);
            this.dgvReport.TabIndex = 11;
            this.dgvReport.Dock = DockStyle.Fill;
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 570);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.pnlStatistics);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnGenerateReport);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.lblFromDate);
            this.Controls.Add(this.numLowStockThreshold);
            this.Controls.Add(this.lblLowStockThreshold);
            this.Controls.Add(this.cmbReportType);
            this.Controls.Add(this.lblReportType);
            this.Name = "ReportsForm";
            this.Text = "Отчеты";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLowStockThreshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isLowStockReport = cmbReportType.SelectedIndex == 5;
            lblLowStockThreshold.Visible = isLowStockReport;
            numLowStockThreshold.Visible = isLowStockReport;
            
            bool needsDateFilter = cmbReportType.SelectedIndex != 4 && cmbReportType.SelectedIndex != 5;
            lblFromDate.Visible = needsDateFilter;
            dtpFromDate.Visible = needsDateFilter;
            lblToDate.Visible = needsDateFilter;
            dtpToDate.Visible = needsDateFilter;
        }

        private void LoadReport()
        {
            try
            {
                DataTable reportData = null;
                bool showStatistics = true;

                switch (cmbReportType.SelectedIndex)
                {
                    case 0: // Все продажи
                        var sales = dbHelper.GetAllSales(dtpFromDate.Value, dtpToDate.Value);
                        dgvReport.DataSource = sales.Select(s => new
                        {
                            s.Id,
                            Дата = s.SaleDate.ToString("dd.MM.yyyy HH:mm"),
                            Сумма = s.TotalAmount,
                            Клиент = s.CustomerName ?? "",
                            Примечания = s.Notes ?? ""
                        }).ToList();
                        break;

                    case 1: // Топ продаваемых товаров
                        reportData = dbHelper.GetTopSellingProducts(dtpFromDate.Value, dtpToDate.Value, 20);
                        dgvReport.DataSource = reportData;
                        break;

                    case 2: // Продажи по категориям
                        reportData = dbHelper.GetSalesByCategory(dtpFromDate.Value, dtpToDate.Value);
                        dgvReport.DataSource = reportData;
                        break;

                    case 3: // Продажи по дням
                        reportData = dbHelper.GetSalesByDay(dtpFromDate.Value, dtpToDate.Value);
                        dgvReport.DataSource = reportData;
                        break;

                    case 4: // Остатки товаров
                        reportData = dbHelper.GetProductsStockReport();
                        dgvReport.DataSource = reportData;
                        showStatistics = false;
                        break;

                    case 5: // Товары с низким остатком
                        reportData = dbHelper.GetLowStockProducts((int)numLowStockThreshold.Value);
                        dgvReport.DataSource = reportData;
                        showStatistics = false;
                        break;

                    case 6: // Отчет по клиентам
                        reportData = dbHelper.GetCustomersReport(dtpFromDate.Value, dtpToDate.Value);
                        dgvReport.DataSource = reportData;
                        break;
                }

                // Форматирование числовых колонок
                FormatDataGridView();

                // Обновляем статистику
                if (showStatistics)
                {
                    decimal totalSales = dbHelper.GetTotalSales(dtpFromDate.Value, dtpToDate.Value);
                    int salesCount = dbHelper.GetTotalSalesCount(dtpFromDate.Value, dtpToDate.Value);
                    decimal averageSale = salesCount > 0 ? totalSales / salesCount : 0;

                    lblTotalSalesValue.Text = $"{totalSales:N2} ₽";
                    lblSalesCountValue.Text = salesCount.ToString();
                    lblAverageSaleValue.Text = $"{averageSale:N2} ₽";
                }
                else
                {
                    lblTotalSalesValue.Text = "—";
                    lblSalesCountValue.Text = "—";
                    lblAverageSaleValue.Text = "—";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки отчета: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            foreach (DataGridViewColumn column in dgvReport.Columns)
            {
                if (column.Name.Contains("Сумма") || column.Name.Contains("Цена") || column.Name.Contains("Выручка") || 
                    column.Name.Contains("Стоимость") || column.Name.Contains("Чек") || column.Name.Contains("Revenue"))
                {
                    column.DefaultCellStyle.Format = "N2";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (column.Name.Contains("Количество") || column.Name.Contains("Остаток") || column.Name.Contains("Quantity"))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvReport.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
                saveDialog.FileName = $"Отчет_{cmbReportType.Text}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        System.IO.StreamWriter writer = new System.IO.StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8);
                        
                        // Заголовки
                        for (int i = 0; i < dgvReport.Columns.Count; i++)
                        {
                            if (i > 0) writer.Write(";");
                            writer.Write(dgvReport.Columns[i].HeaderText);
                        }
                        writer.WriteLine();
                        
                        // Данные
                        foreach (DataGridViewRow row in dgvReport.Rows)
                        {
                            for (int i = 0; i < dgvReport.Columns.Count; i++)
                            {
                                if (i > 0) writer.Write(";");
                                writer.Write(row.Cells[i].Value?.ToString() ?? "");
                            }
                            writer.WriteLine();
                        }
                        
                        writer.Close();
                        MessageBox.Show("Отчет успешно экспортирован", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
