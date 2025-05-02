using static System.Console;

using BethanysPieShop.InventoryManagement.Domain.General;
using BethanysPieShop.InventoryManagement.Domain.OrderManagement;
using BethanysPieShop.InventoryManagement.Domain.ProductManagement;

namespace BethanysPieShop.InventoryManagement;

internal class Utilities
{
    private static List<Product> inventory = new();
    private static List<Order> orders = new();

    internal static void InitializeStock()
    {
        BoxedProduct bp = new BoxedProduct(
            6, "Eggs", "Lorem ipsum",
            new Price() { ItemPrice = 10, Currency = Currency.Euro },
            200, 100
        );

        bp.IncreaseStock(100);
        bp.UseProduct(10);

        ProductRepository productRepository = new();
        inventory = productRepository.LoadProductsFromFile();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Loaded {inventory.Count} products");
        
        Console.WriteLine("Press enter to continue");
        Console.ResetColor();
        
        Console.ReadLine();
    }

    internal static void ShowMainMenu()
    {
        ResetColor();
        Clear();
        WriteLine("********************");
        WriteLine("* Select an action *");
        WriteLine("********************");
        
        WriteLine("1: Inventory management");
        WriteLine("2: Order management");
        WriteLine("3: Settings");
        WriteLine("4: Save all data");
        WriteLine("0: Close application");
        
        Write("Your selection: ");
        
        string? userSelection = ReadLine();
        switch (userSelection)
        {
            case "1":
                ShowInventoryManagementMenu();
                break;
            case "2":
                ShowOrderManagementMenu();
                break;
            case "3":
                ShowSettingsMenu();
                break;
            case "4":
                // SaveAllData();
                break;
            case "0":
                break;
            default:
                WriteLine("Invalid selection, please try again.");
                break;
        }
    }

    private static void ShowInventoryManagementMenu()
    {
        string? userSelection;

        do
        {
            ResetColor();
            Clear();
            WriteLine("************************");
            WriteLine("* Inventory management *");
            WriteLine("************************");

            ShowAllProductsOverview();
            
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine("What do you want to do?");
            ResetColor();
            
            WriteLine("1: View details of product");
            WriteLine("2: Add new product");
            WriteLine("3: Clone product");
            WriteLine("4: View products with low stock");
            WriteLine("0: Back to main menu");
            
            Write("Your selection: ");
            userSelection = ReadLine();

            switch (userSelection)
            {
                case "1":
                    ShowDetailsAndUserProduct();
                    break;
                case "2":
                    // ShowCreateNewProduct();
                    break;
                case "3":
                    // ShowCloneExistingProduct();
                    break;
                case "4":
                    ShowProductsLowOnStock();
                    break;
                default:
                    WriteLine("Invalid selection, please try again.");
                    break;
            }
        }
        while (userSelection != "0");
        ShowMainMenu();
    }

    private static void ShowAllProductsOverview()
    {
        foreach (var product in inventory)
        {
            WriteLine(product.DisplayDetailsShort());
            WriteLine();
        }
    }

    private static void ShowDetailsAndUserProduct()
    {
        string? userSelection = string.Empty;
        
        WriteLine("Enter the ID of product: ");
        string? selectedProductId = ReadLine();

        if (selectedProductId != null)
        {
            Product? selectedProduct = inventory.FirstOrDefault(p => p.Id == int.Parse(selectedProductId));

            if (selectedProduct != null)
            {
                WriteLine(selectedProduct.DisplayDetailsFull());
                
                WriteLine("\nWhat do you want to do?");
                WriteLine("1: Use product");
                WriteLine("0: Back to inventory overview");
                
                Write("Your selection: ");
                userSelection = ReadLine();

                if (userSelection == "1")
                {
                    WriteLine("How many products do you want to use?");
                    int amount = int.Parse(Console.ReadLine() ?? "0");
                    
                    selectedProduct.UseProduct(amount);
                    
                    ReadLine();
                }
            }
        }
        else
        {
            WriteLine("Non-existing product selected. Please try again.");
        }
    }

    private static void ShowProductsLowOnStock()
    {
        List<Product> lowOnStockProducts = inventory.Where(p => p.IsBellowStockThreshold).ToList();

        if (lowOnStockProducts.Count > 0)
        {
            WriteLine("The following items are low on the stock, order more soon!");

            foreach (var product in lowOnStockProducts)
            {
                WriteLine(product.DisplayDetailsShort());
                WriteLine();
            }
        }
        else
        {
            WriteLine("No items are currently low on stock.");
        }
        
        ReadLine();
    }

    private static void ShowOrderManagementMenu()
    {
        string? userSelection = string.Empty;

        do
        {
            ResetColor();
            Clear();
            WriteLine("********************");
            WriteLine("* Select an action *");
            WriteLine("********************");
            
            WriteLine("1: Open order overview");
            WriteLine("2: Add new order");
            WriteLine("0: Back to main menu");
            
            Write("Your selection: ");
            userSelection = ReadLine();

            switch (userSelection)
            {
                case "1":
                    ShowOpenOrderOverview();
                    break;
                case "2":
                    ShopAddNewOreder();
                    break;
                default:
                    WriteLine("Invalid selection, please try again.");
                    break;
            }
        }
        while (userSelection != "0");
        ShowMainMenu();
    }

    private static void ShowOpenOrderOverview()
    {
        ShowFulfilledOrders();

        if (orders.Count > 0)
        {
            WriteLine("Open orders:");

            foreach (var order in orders)
            {
                WriteLine(order.ShowOrderDetails());
                WriteLine();
            }
        }
        else
        {
            WriteLine("There are no open orders.");
        }

        ReadLine();
    }

    private static void ShowFulfilledOrders()
    {
        WriteLine("Checking fulfilled orders...");
        foreach (var order in orders)
        {
            if (!order.Fulfilled && order.OrderFulfilmentDate < DateTime.Now)
            {
                foreach (var orderItem in order.OrderItems)
                {
                    Product? selectedProduct = inventory.FirstOrDefault(p => p.Id == orderItem.ProductId);
                    if (selectedProduct != null)
                        selectedProduct.IncreaseStock(orderItem.AmountOrdered);
                }
                order.Fulfilled = true;
            }
        }
        orders.RemoveAll(order => order.Fulfilled);
        
        WriteLine("Fulfilled orders checked");
    }

    private static void ShopAddNewOreder()
    {
        Order newOrder = new Order();
        string? selectedProductId = string.Empty;
        
        ForegroundColor = ConsoleColor.Yellow;
        WriteLine("Creating new order...");
        ResetColor();

        do
        {
            ShowAllProductsOverview();
            
            WriteLine("Which product do you want to order? (enter 0 to stop adding new products to the order)");
            
            Write("Enter the ID of product: ");
            selectedProductId = ReadLine();

            if (selectedProductId != "0")
            {
                Product? selectedProduct = inventory.FirstOrDefault(p => p.Id == int.Parse(selectedProductId));

                if (selectedProduct != null)
                {
                    Write("How many product do you want to order?: ");
                    int amountOrdered = int.Parse(Console.ReadLine() ?? "0");
                
                    OrderItem orderItem = new OrderItem
                    {
                        ProductId = selectedProduct.Id,
                        ProductName = selectedProduct.Name,
                        AmountOrdered = amountOrdered,
                    };    
                }
                
            }
        }
        while (selectedProductId != "0");
        
        WriteLine("Creating new order...");
        
        orders.Add(newOrder);
        
        WriteLine("Order now created");
        ReadLine();
    }

    private static void ShowSettingsMenu()
    {
        string? userSelection;

        do
        {
            ResetColor();
            Clear();
            WriteLine("************");
            WriteLine("* Settings *");
            WriteLine("************");
            
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine("What do you want to do?");
            ResetColor();
            
            WriteLine("1: Change stock threshold");
            WriteLine("0: Back to main meny");
            
            Write("Your selection: ");
            userSelection = ReadLine();

            switch (userSelection)
            {
                case "1":
                    ShowChangeStockThreshold();
                    break;
                default:
                    WriteLine("Invalid selection, please try again.");
                    break;
            }
        }
        while (userSelection != "0");
        ShowMainMenu();
    }

    private static void ShowChangeStockThreshold()
    {
        WriteLine($"Enter the new stock threshold " +
                  $"(current value: {Product.StockThreshold}). " +
                  $"This applies to all products!");
        Write("New value: ");
        int newValue = int.Parse(ReadLine() ?? "0");
        Product.StockThreshold = newValue;
        WriteLine($"New stock threshold set to {Product.StockThreshold}");

        foreach (var product in inventory)
        {
            product.UpdateLowStock();
        }
    }
}
