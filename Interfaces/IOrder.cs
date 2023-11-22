using Microsoft.Data.SqlClient;
using SolforbTest.Models;

namespace SolforbTest.Interfaces
{
    public interface IOrder
    {
        public List<Order> GetAllOrdersFiltered(string orderNumber, string orderItemName, DateTime fromDate, DateTime todate, string orderItemUnit, List<int> providerlist);
        public List<Order> GetAllOrders();
        Order GetOrder(int id);

        bool Create(Order order);

        bool Edit(Order order);

        bool Delete(Order order);


        public string GetErrors();
    }
}
