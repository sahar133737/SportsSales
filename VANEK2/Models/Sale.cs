using System;
using System.Collections.Generic;

namespace VANEK2.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerName { get; set; }
        public string Notes { get; set; }
        public List<SaleItem> Items { get; set; }

        public Sale()
        {
            Items = new List<SaleItem>();
        }
    }
}

