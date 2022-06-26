using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreForYou.Controllers
{
    public class NationalityController : Controller
    {
        private readonly INationalityService _NationalityService;

        public NationalityController(INationalityService NationalityService)
        {
            _NationalityService = NationalityService;
        }
        // GET: NationalityController
        public ActionResult Index()
        {
            Task<List<NationalityModel>> nationalities = _NationalityService.GetAllNationalities();
            return View(nationalities.Result);
        }

        // GET: NationalityController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NationalityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NationalityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NationalityModel Model)
        {
            try
            {
                Model.CreatedDate = DateTime.Now;
                Model.UpdatedDate = DateTime.Now;
                Model.IsVisible = true;
                Model.IsDelted = false;
                var response = _NationalityService.CreateNationality(Model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NationalityController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NationalityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NationalityController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NationalityController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
