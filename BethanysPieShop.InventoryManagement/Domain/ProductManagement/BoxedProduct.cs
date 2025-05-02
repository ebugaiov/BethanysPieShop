using System.Text;
using BethanysPieShop.InventoryManagement.Domain.General;

namespace BethanysPieShop.InventoryManagement.Domain.ProductManagement;

public class BoxedProduct : Product
{
    private int amountPerBox;

    public int AmountPerBox
    {
        get { return amountPerBox; }
        set
        {
            amountPerBox = value;
        }
    }

    public BoxedProduct(int id, string name, string? description, Price price, int maxAmountInStock, int amountPerBox)
        : base(id, name, description, price, UnitType.PerBox, maxAmountInStock)
    {
        AmountPerBox = amountPerBox;
    }

    public string DisplayBoxedProductDetails()
    {
        StringBuilder sb = new();

        sb.Append("Boxed Product\n");
        sb.Append($"{Id} {Name} \n{Description}\n{Price}\n{AmountInStock} item(s) in stock");

        if (IsBellowStockThreshold)
        {
            sb.Append("\n!!STOCK LOW!!");
        }

        return sb.ToString();
    }

    public void UseBoxedProduct(int items)
    {
        int smallestMultiple = 0;
        int batchSize;

        while (true)
        {
            smallestMultiple++;
            if (smallestMultiple * AmountPerBox > items)
            {
                batchSize = smallestMultiple * AmountPerBox;
                break;
            }
        }
        
        UseProduct(batchSize);
    }
}