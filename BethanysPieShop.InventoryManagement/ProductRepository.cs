using BethanysPieShop.InventoryManagement.Domain.General;
using BethanysPieShop.InventoryManagement.Domain.ProductManagement;

namespace BethanysPieShop.InventoryManagement;

internal class ProductRepository
{
    private string directory = @"/Users/jerrye/Downloads/";
    private string fileName = "products.txt";
    
    private void CheckForExistingProductFile()
    {
        string path = $"{directory}{fileName}";
        
        bool existingFileFound = File.Exists(path);
        Console.WriteLine(existingFileFound);

        if (!existingFileFound)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            using FileStream file = File.Create(path);
        }
    }

    public List<Product> LoadProductsFromFile()
    {
        List<Product> products = new List<Product>();
        string path = $"{directory}{fileName}";

        try
        {
            CheckForExistingProductFile();

            string[] productsAsString = File.ReadAllLines(path);
            foreach (string line in productsAsString)
            {
                string[] productSplits = line.Split(';');

                bool success = int.TryParse(productSplits[0], out int productId);
                if (!success)
                    productId = 0;
                
                string name = productSplits[1];
                string description = productSplits[2];
                
                success = int.TryParse(productSplits[3], out int maxItemsInStock);
                if (!success)
                    maxItemsInStock = 100;
                
                success = int.TryParse(productSplits[4], out int itemPrice);
                if (!success)
                    itemPrice = 0;
                
                success = Enum.TryParse(productSplits[5], out Currency currency);
                if (!success)
                    currency = Currency.Dollar;
                
                success = Enum.TryParse(productSplits[6], out UnitType unitType);
                if (!success)
                    unitType = UnitType.PerItem;
                
                Product product = new Product(
                    productId, name, description,
                    new Price() { ItemPrice = itemPrice, Currency = currency },
                    unitType, maxItemsInStock
                    );
                
                products.Add(product);
            }
        }
        catch (IndexOutOfRangeException iex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Something went wrong parsing the file, please check the data");
            Console.WriteLine(iex.Message);
        }
        catch (FileNotFoundException fnfex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The file couldn't be found");
            Console.WriteLine(fnfex.Message);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Something went wrong while loading the file");
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Console.ResetColor();
        }
        
        return products;
    }
}