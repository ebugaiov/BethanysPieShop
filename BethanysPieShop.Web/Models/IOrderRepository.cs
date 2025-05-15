namespace BethanysPieShop.Web.Models;

public interface IOrderRepository
{
    void CreateOrder(Order order);
}