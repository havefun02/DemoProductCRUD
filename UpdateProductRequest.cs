using System.ComponentModel.DataAnnotations;

namespace DemoCRUD
{
    public class UpdateProductRequest
    {
        public string ProductName { get; set; }=string.Empty;
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        [StockQuantityValidation]
        public int StockQuantity { get; set; }
    }

}
