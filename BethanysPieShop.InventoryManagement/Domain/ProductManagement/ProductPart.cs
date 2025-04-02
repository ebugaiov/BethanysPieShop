namespace BethanysPieShop.InventoryManagement.Domain.ProductManagement;

public partial class Product
{
    public static int StockThreshold = 5;

    public static void ChangeStockThreshold(int newStockThreshold)  // this method can work only with static data(property)
    {
        if (newStockThreshold < 0)
            StockThreshold = newStockThreshold;
    }
    
     public void UpdateLowStock()
    {
        if (AmountInStock < StockThreshold) // for now a fixed value
        {
            IsBellowStockThreshold = true;
        }
    }

    private void Log(string message)
    {
        // this could be written to a file
        Console.WriteLine(message);
    }
    
    private string CreateSimpleProductRepresentation()
    {
        return $"Product {Id} ({name})";
    }
}