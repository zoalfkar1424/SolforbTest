using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SolforbTest.DBContext;
using SolforbTest.Interfaces;
using SolforbTest.Models;

namespace SolforbTest.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly IOrderItem _orderItemRepo;
        private readonly IOrder _orderRepo;
        private readonly IProvider _providerRepo;
        public OrderItemsController(IOrderItem OrderItemRepo, IOrder OrderRepo, IProvider ProviderRepo)
        {
            _orderItemRepo = OrderItemRepo;
            _orderRepo = OrderRepo;
            _providerRepo = ProviderRepo;
        }

        // GET: OrderItems
        public IActionResult Index(int orderId)
        {
            Order order = _orderRepo.GetOrder(orderId);
            ViewBag.orderNumber = order.Number;
            ViewBag.ProviderName = _providerRepo.GetProvider(order.ProviderId).Name;
            ViewBag.orderDate = order.Date.ToShortDateString();
            return View(_orderItemRepo.GetAllOrderItems(orderId));
        }

        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int id)
        {
            OrderItem OrderItem = _orderItemRepo.GetOrderItem(id);
            return View(OrderItem);
        }

        // GET: OrderItems/Create
        public IActionResult Create()
        {
            //ViewData["OrderItemId"] = _orderItemRepo.GetAllOrderItems();
            OrderItem OrderItem = new OrderItem();
            return View(OrderItem);
        }

        // POST: OrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderItem OrderItem)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                bolret = _orderItemRepo.Create(OrderItem);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }


            if (bolret == false)
            {
                errMessage = errMessage + " " + _orderItemRepo.GetErrors();

                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(OrderItem);
            }
            else
            {
                TempData["SuccessMessage"] = "" + OrderItem.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            OrderItem OrderItem = _orderItemRepo.GetOrderItem(id);
            //ViewData["OrderItemId"] = _orderItemRepo.GetAllOrderItems();
            TempData.Keep();
            return View(OrderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OrderItem OrderItem)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                bolret = _orderItemRepo.Edit(OrderItem);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }


            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];


            if (bolret == false)
            {
                errMessage = errMessage + " " + _orderItemRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(OrderItem);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        // GET: OrderItems/Delete/5
        public IActionResult Delete(int id)
        {
            OrderItem OrderItem = _orderItemRepo.GetOrderItem(id);
            //ViewData["OrderItemId"] = _orderItemRepo.GetAllOrderItems();
            TempData.Keep();
            return View(OrderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(OrderItem OrderItem)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                bolret = _orderItemRepo.Delete(OrderItem);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(OrderItem);
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];


            if (bolret == false)
            {
                errMessage = errMessage + " " + _orderItemRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(OrderItem);
            }
            else
            {
                TempData["SuccessMessage"] = OrderItem.Name + " Deleted Successfully";
                return RedirectToAction(nameof(Index), new { pg = currentPage });
            }
        }
        
    }
}
