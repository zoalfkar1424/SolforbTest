using Microsoft.EntityFrameworkCore;
using SolforbTest.DBContext;
using SolforbTest.Interfaces;
using SolforbTest.Models;

namespace SolforbTest.Repositories
{
    public class OrderItemRepo : IOrderItem
    {
        private readonly ApplicationDBcontext _context;
        private string _errors = "";
        public OrderItemRepo(ApplicationDBcontext context)
        {
            _context = context;
        }
        public List<OrderItem> GetAllOrderItems(int orderId)
        {
            IQueryable<OrderItem> OrderItems = _context.OrderItem.Where(n=>n.OrderId==orderId);
            return OrderItems.ToList();
        }
        public OrderItem GetOrderItem(int id)
        {
            OrderItem OrderItem = _context.OrderItem.Where(u => u.Id == id).FirstOrDefault();
            return OrderItem;
        }
        public bool Create(OrderItem OrderItem)
        {
            try
            {
                _context.OrderItem.Add(OrderItem);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errors = "Create Failed - Sql Exception Occured , Error Info : " + ex.Message;
                return false;
            }
        }

        public bool Edit(OrderItem OrderItem)
        {
            try
            {
                _context.OrderItem.Attach(OrderItem);
                _context.Entry(OrderItem).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errors = "Update Failed - Sql Exception Occured , Error Info : " + ex.Message;
                return false;
            }
        }

        public bool Delete(OrderItem OrderItem)
        {
            try
            {
                _context.OrderItem.Attach(OrderItem);
                _context.Entry(OrderItem).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                    _errors = "Delete Failed - Sql Exception Occured , Error Info : " + ex.InnerException.Message;
                else
                    _errors = "Delete Failed - Sql Exception Occured , Error Info : " + ex.Message;
                return false;
            }
        }

        public string GetErrors()
        {
            return _errors;
        }
    }
}
