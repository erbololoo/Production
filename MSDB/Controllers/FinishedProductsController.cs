using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSDB;

namespace MSDB.Controllers
{
    public class FinishedProductsController : Controller
    {
        private readonly callme6lackContext _context;

        public FinishedProductsController(callme6lackContext context)
        {
            _context = context;
        }

        // GET: FinishedProducts
        public async Task<IActionResult> Index()
        {
            var callme6lackContext = _context.FinishedProducts.Include(f => f.UnitNavigation);
            return View(await callme6lackContext.ToListAsync());
        }

        // GET: FinishedProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finishedProducts = await _context.FinishedProducts
                .Include(f => f.UnitNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (finishedProducts == null)
            {
                return NotFound();
            }

            return View(finishedProducts);
        }

        // GET: FinishedProducts/Create
        public IActionResult Create()
        {
            callme6lackContext db = new callme6lackContext();
            var un = db.Units.FromSqlInterpolated($"SelectAllUnits").ToList();

            Units units = new Units();

            foreach (var val in un)
            {
                units.Id = val.Id;
                units.Unit = val.Unit;
            }

            var FP = db.FinishedProducts.FromSqlInterpolated($"SelectAllFinishedProducts").ToList();

            FinishedProducts finishedProducts = new FinishedProducts();

            List<FinishedProducts> finishedProducts1 = new List<FinishedProducts>();

            foreach (var val in FP)
            {
                finishedProducts1.Add(new FinishedProducts()
                {
                    Id = val.Id,
                    Name = val.Name,
                    Unites = val.Unites,
                    Sum = val.Sum,
                    Amount=val.Amount
                });
            }

            ViewData["Unites"] = new SelectList(_context.Units, "Id", "Unit");
            return View();
        }

        // POST: FinishedProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Unites,Sum,Amount")] FinishedProducts finishedProducts)
        {
            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlInterpolated($"InsertFinishedProducts @Name={finishedProducts.Name}, @Unites={finishedProducts.Unites}, @Sum={finishedProducts.Sum},@Amount={finishedProducts.Amount}");
                return RedirectToAction(nameof(Index));
            }
            ViewData["Unites"] = new SelectList(_context.Units, "Id", "Id", finishedProducts.Unites);
            return View(finishedProducts);
        }

        // GET: FinishedProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var finishedProducts = await _context.FinishedProducts.FindAsync(id);
            //if (finishedProducts == null)
            //{
            //    return NotFound();
            //}
            var finishedProducts = _context.FinishedProducts.FromSqlInterpolated($"SelectIDFinishedProducts @ID={id}").ToList();
            FinishedProducts finishedProducts1 = new FinishedProducts();
            foreach (var val in finishedProducts)
            {
                finishedProducts1.Id = val.Id;
                finishedProducts1.Name = val.Name;
                finishedProducts1.Unites = val.Unites;
                finishedProducts1.Sum = val.Sum;
                finishedProducts1.Amount = val.Amount;
            }
            ViewData["Unites"] = new SelectList(_context.Units, "Id", "Unit", finishedProducts1.Unites);
            return View(finishedProducts1);
        }

        // POST: FinishedProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Unites,Sum,Amount")] FinishedProducts finishedProducts)
        {
            if (id != finishedProducts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlInterpolated($"UpdateFinishedProducts @ID={id},@Name={finishedProducts.Name}, @Unites={finishedProducts.Unites},@Sum={finishedProducts.Sum}, @Amount={finishedProducts.Amount}");
                //try
                //{
                //    _context.Update(finishedProducts);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!FinishedProductsExists(finishedProducts.Id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                return RedirectToAction(nameof(Index));
            }
            ViewData["Unites"] = new SelectList(_context.Units, "Id", "Unit", finishedProducts.Unites);
            return View(finishedProducts);
        }

        // GET: FinishedProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var finishedProducts = await _context.FinishedProducts
            //    .Include(f => f.UnitNavigation)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (finishedProducts == null)
            //{
            //    return NotFound();
            //}

            //return View(finishedProducts);
            var finishedProducts = _context.FinishedProducts.FromSqlInterpolated($"SelectIDFinishedProducts @ID={id}").ToList();
            FinishedProducts finishedProducts1 = new FinishedProducts();
            foreach (var val in finishedProducts)
            {
                finishedProducts1.Id = val.Id;
                finishedProducts1.Name = val.Name;
                finishedProducts1.Unites = val.Unites;
                finishedProducts1.Sum = val.Sum;
                finishedProducts1.Amount = val.Amount;
            }
            return View(finishedProducts1);
        }

        // POST: FinishedProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            callme6lackContext callme6LackContext = new callme6lackContext();
            callme6LackContext.Database.ExecuteSqlInterpolated($"DeleteFinishedProducts @ID={id}");
            return RedirectToAction(nameof(Index));
        }

    }
}
