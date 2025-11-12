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
            ApplyModernStyle();
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            tabControl.BackColor = Color.White;
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
    }
}
