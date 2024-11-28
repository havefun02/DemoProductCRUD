using System.ComponentModel.DataAnnotations;

namespace DemoCRUD
{
    public class StockQuantityValidationAttribute:ValidationAttribute
    {
        public StockQuantityValidationAttribute() : base("StockQuantity must be greater than 0.")
        {
        }
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is int stockQuantity)
            {
                return stockQuantity > 0;
            }

            return false;
        }

    }
}
