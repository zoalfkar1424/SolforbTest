using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SolforbTest.DBContext;
using SolforbTest.Interfaces;
using SolforbTest.Models;

namespace SolforbTest.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrder _orderRepo;
        private readonly IProvider _providerRepo;

        public OrdersController(IOrder orderRepo,IProvider providerRepo)
        {
            _orderRepo = orderRepo;
            _providerRepo = providerRepo;
        }

        // GET: Orders
        public IActionResult Index(string orderNumber , string orderItemName, DateTime fromDate , DateTime todate,string orderItemUnit, List<int> providerlist)
        {
            ViewData["orderslist"] = GetOrdersList();
            ViewData["providerlist"] = GetProvidersList();
            ViewBag.todate = DateTime.Now.Date.ToShortDateString();
            return View(_orderRepo.GetAllOrdersFiltered( orderNumber,  orderItemName,  fromDate,  todate,  orderItemUnit, providerlist));
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Order order = _orderRepo.GetOrder(id);
            ViewData["ProviderId"] = _providerRepo.GetAllProviders();
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.ProviderId = GetProvidersList();
            Order order = new Order();
            order.OrderItems.Add(new OrderItem() { Id = 1 });
            return View(order);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderItems.RemoveAll(a => a.Quantity == 0);
                bool bolret = false;
                string errMessage = "";
                try
                {
                    bolret = _orderRepo.Create(order);
                }
                catch (Exception ex)
                {
                    errMessage = errMessage + " " + ex.Message;
                }


                if (bolret == false)
                {
                    ModelState.AddModelError("", errMessage);
                    return View(order);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return View(order);
            }

        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Order order = _orderRepo.GetOrder(id);
           
            ViewBag.ProviderId = GetProvidersList();
            TempData.Keep();
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderItems.RemoveAll(a => a.Quantity == 0);
                bool bolret = false;
                string errMessage = "";

                try
                {
                    bolret = _orderRepo.Edit(order);
                }
                catch (Exception ex)
                {
                    errMessage = errMessage + " " + ex.Message;
                }
                if (bolret == false)
                {
                    errMessage = errMessage + " " + _orderRepo.GetErrors();
                    ModelState.AddModelError("", errMessage);
                    return View(order);
                }
                else
                    return RedirectToAction(nameof(Index));
            }
            else
            {              
                return View(order);
            }
        }

        // GET: Orders/Delete/5
        public IActionResult Delete(int id)
        {
            Order order = _orderRepo.GetOrder(id);
            ViewData["ProviderId"] = _providerRepo.GetAllProviders();
            TempData.Keep();
            return View(order);
        }
    
        // POST: Orders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Order order)
        {
        bool bolret = false;
        string errMessage = "";
        try
        {
            bolret = _orderRepo.Delete(order);
        }
        catch (Exception ex)
        {
            errMessage = ex.Message;
            ModelState.AddModelError("", errMessage);
            return View(order);
        }

      
        if (bolret == false)
        {
            errMessage = errMessage + " " + _orderRepo.GetErrors();
            ModelState.AddModelError("", errMessage);
            return View(order);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
        }
        private List<SelectListItem> GetProvidersList()
        {
            var lstItems = new List<SelectListItem>();

            var providers = _providerRepo.GetAllProviders();
            lstItems = providers.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Provider ----"
            };

            lstItems.Insert(0, defItem);

            return lstItems;
        }
        private List<SelectListItem> GetOrdersList()
        {
            var lstOrders = new List<SelectListItem>();

            var orders = _orderRepo.GetAllOrders();
            lstOrders = orders.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Number
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Order ----"
            };

            lstOrders.Insert(0, defItem);

            return lstOrders;
        }
    }
}
