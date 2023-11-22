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
    public class ProvidersController : Controller
    {
        private readonly IProvider _providerRepo;

        public ProvidersController(IProvider providerRepo)
        {
            _providerRepo = providerRepo;
        }

        // GET: Providers
        public IActionResult Index(string SearchText = "")
        {
            return View(_providerRepo.GetAllProviders(SearchText));
        }

        // GET: Providers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Provider Provider = _providerRepo.GetProvider(id);           
            return View(Provider);
        }

        // GET: Providers/Create
        public IActionResult Create()
        {
            ViewData["ProviderId"] = _providerRepo.GetAllProviders();
            Provider Provider = new Provider();          
            return View(Provider);
        }

        // POST: Providers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Provider Provider)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                bolret = _providerRepo.Create(Provider);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }


            if (bolret == false)
            {
                errMessage = errMessage + " " + _providerRepo.GetErrors();

                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(Provider);
            }
            else
            {
                TempData["SuccessMessage"] = "" + Provider.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Providers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Provider Provider = _providerRepo.GetProvider(id);
            ViewData["ProviderId"] = _providerRepo.GetAllProviders();
            TempData.Keep();
            return View(Provider);
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Provider Provider)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                bolret = _providerRepo.Edit(Provider);
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
                errMessage = errMessage + " " + _providerRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(Provider);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        // GET: Providers/Delete/5
        public IActionResult Delete(int id)
        {
            Provider Provider = _providerRepo.GetProvider(id);
            ViewData["ProviderId"] = _providerRepo.GetAllProviders();
            TempData.Keep();
            return View(Provider);
        }

        // POST: Providers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Provider Provider)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                bolret = _providerRepo.Delete(Provider);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(Provider);
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];


            if (bolret == false)
            {
                errMessage = errMessage + " " + _providerRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(Provider);
            }
            else
            {
                TempData["SuccessMessage"] = Provider.Name + " Deleted Successfully";
                return RedirectToAction(nameof(Index), new { pg = currentPage });
            }
        }
    }
}
