namespace BethanysPieShop.InventoryManagement.Domain.General;

public class Price
{
    public double ItemPrice { get; set; }
    public Currency Currency { get; set; }

    public Price() { }
    public Price(double price, Currency currency)
    {
        ItemPrice = price;
        Currency = currency;
    }

    public override string ToString()
    {
        return $"{ItemPrice} {Currency}";
    }
}
