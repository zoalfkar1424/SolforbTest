using SolforbTest.Models;

namespace SolforbTest.Interfaces
{
    public interface IOrderItem
    {
        public List<OrderItem> GetAllOrderItems(int orderId);
        OrderItem GetOrderItem(int id);

        bool Create(OrderItem OrderItem);

        bool Edit(OrderItem OrderItem);

        bool Delete(OrderItem OrderItem);

        public string GetErrors();
    }
}
