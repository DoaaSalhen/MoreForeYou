using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Implementation;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreForYou.Controllers
{
    public class PositionController : Controller
    {
        private readonly IPositionService _PositionService;

        public PositionController(IPositionService PositionService)
        {
            _PositionService = PositionService;
        }
        // GET: PositionController
        public ActionResult Index()
        {
            Task<List<PositionModel>> positions = _PositionService.GetAllPositions();
            return View(positions.Result);
        }

        // GET: PositionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PositionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PositionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PositionModel Model)
        {
            try
            {
                Model.CreatedDate = DateTime.Now;
                Model.UpdatedDate = DateTime.Now;
                Model.IsVisible = true;
                Model.IsDelted = false;
                var response = _PositionService.CreatePosition(Model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PositionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PositionController/Edit/5
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

        // GET: PositionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PositionController/Delete/5
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
