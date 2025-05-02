using System.Text;

using BethanysPieShop.InventoryManagement.Domain.General;

namespace BethanysPieShop.InventoryManagement.Domain.ProductManagement;

public partial class Product
{
    // Fields
    private string name = string.Empty;
    private string? description;
    
    private int maxItemInStock = 0;
    
    // Properties
    public int Id { get; set; }

    public string Name
    {
        get => name;
        set => name = value.Length > 50 ? value[..50] : value;
    }

    public string? Description
    {
        get => description;
        set
        {
            if (value == null)
            { 
                description = string.Empty;
            }
            else
            {
                description = value.Length > 250 ? value[..250] : value;
            }
        }
    }
    
    public UnitType UnitType { get; set; }
    public int AmountInStock { get; private set; }
    public bool IsBellowStockThreshold { get; private set; }
    public Price Price { get; set; }
    
    // Constructors
    public Product(int id) : this(id, string.Empty) { }
    
    public Product(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public Product(int id, string name, string? description, Price price, UnitType unitType, int maxAmountInStock)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        UnitType = unitType;
        
        maxItemInStock = maxAmountInStock;
        
        UpdateLowStock();
    }

    // Methods
    public void UseProduct(int items)
    {
        if (items <= AmountInStock)
        {
            AmountInStock -= items;
            
            UpdateLowStock();
            
            Log($"Amout in stock updated. Now {AmountInStock} items in stock.");
        }
        else
        {
            Log($"Not enough items on stock for {CreateSimpleProductRepresentation()}." +
                $"{AmountInStock} available but {items} requested.");
        }
    }

    public void IncreaseStock()
    {
        AmountInStock++;
    }

    public void IncreaseStock(int amount)
    {
        int newStock = AmountInStock + amount;

        if (newStock <= maxItemInStock)
        {
            AmountInStock += amount;
        }
        else
        {
            AmountInStock = maxItemInStock;  // we only store the possible items, overstock isn't stored
            Log($"{CreateSimpleProductRepresentation()} stock overflow." +
                $"{newStock - AmountInStock} item(s) ordered that couldn't be stored.");
        }

        if (AmountInStock > StockThreshold)
        {
            IsBellowStockThreshold = false;
        }
    }

    protected void DecreaseStock(int items, string reason)
    {
        if (items <= AmountInStock)
        {
            AmountInStock -= items;
        }
        else
        {
            AmountInStock = 0;
        }
        
        UpdateLowStock();
        
        Log(reason);
    }

    public string DisplayDetailsShort()
    {
        return $"{Id}. {name} \n{AmountInStock} item(s) in stock.";
    }

    public string DisplayDetailsFull()
    {
        StringBuilder sb = new StringBuilder();
        
        sb.Append($"{Id} {Name} \n{Description}\n{Price}\n{AmountInStock} item(s) in stock.");

        if (IsBellowStockThreshold)
        {
            sb.Append("\n!!STOCK LOW!!");
        }
        
        return sb.ToString();
    }

    public string DisplayDetailsFull(string extraDetails)
    {
        StringBuilder sb = new();
        sb.Append($"{Id} {Name} \n{Description}\n{Price}\n{AmountInStock} item(s) in stock.");
        sb.Append(extraDetails);

        if (IsBellowStockThreshold)
        {
            sb.Append("\n!!STOCK LOW!!");
        }
        
        return sb.ToString();
    }  
}
