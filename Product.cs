﻿using System.ComponentModel.DataAnnotations;

namespace DemoCRUD
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

}