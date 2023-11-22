using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SolforbTest.DBContext;
using SolforbTest.Interfaces;
using SolforbTest.Models;
using System.Drawing.Printing;
using static NuGet.Packaging.PackagingConstants;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SolforbTest.Repositories
{
    public class OrderRepo : IOrder
    {
        private readonly ApplicationDBcontext _context;
        private string _errors = "";
        public OrderRepo(ApplicationDBcontext context)
        {
            _context = context;
        }
        public List<Order> GetAllOrders()
        {
            return _context.Order.ToList();
        }
            public List<Order> GetAllOrdersFiltered(string orderNumber, string orderItemName, DateTime fromDate, DateTime todate, string orderItemUnit, List<int> providerlist)
        {
            IQueryable<Order> orders = _context.Order;

            if (orderItemName != "" && orderItemName != null)
            {
                var items = orders.SelectMany(o => o.OrderItems);
                orders = (from o in items
                          where o.Name.Contains(orderItemName)
                          select o.Order).Distinct();
            }
            if (orderItemUnit != "" && orderItemUnit != null)
            {
                orders = (from o in orders.SelectMany(o => o.OrderItems)
                          where o.Unit.Contains(orderItemUnit)
                          select o.Order).Distinct();
            }

            if (orderNumber != "" && orderNumber != null)
            {
                orders = orders.Where(n => n.Number.Contains(orderNumber))
                    ;
            }

            
            orders = orders.Where(n => n.Date>= fromDate && n.Date <=todate.AddDays(1));
            

            if (providerlist.Count != 0) {
                orders = orders.Where(n => providerlist.Contains(n.ProviderId)); 
                    
            }           

            return orders.ToList();
        }
        public Order GetOrder(int id)
        {
            Order order = _context.Order.Include(a=>a.OrderItems).Where(u => u.Id == id).FirstOrDefault();
            return order;
        }

        public bool Create(Order order)
        {
            try
            {
                _context.Order.Add(order);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errors = "Create Failed - Sql Exception Occured , Error Info : " + ex.Message;
                return false;
            }
        }

        public bool Edit(Order order)
        {
            try
            {
                List<OrderItem> items = _context.OrderItem.Where(n=>n.OrderId==order.Id).ToList();
                _context.OrderItem.RemoveRange(items);
                _context.SaveChanges();
                _context.Order.Attach(order);
                _context.Entry(order).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errors = "Update Failed - Sql Exception Occured , Error Info : " + ex.Message;
                return false;
            }
        }

        public bool Delete(Order order)
        {
            try
            {
                _context.Order.Attach(order);
                _context.Entry(order).State = EntityState.Deleted;
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
