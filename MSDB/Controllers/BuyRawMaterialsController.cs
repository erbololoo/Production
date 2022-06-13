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
    public class BuyRawMaterialsController : Controller
    {
        private readonly callme6lackContext _context;

        public BuyRawMaterialsController(callme6lackContext context)
        {
            _context = context;
        }

        // GET: BuyRawMaterials
        public async Task<IActionResult> Index()
        {
            callme6lackContext db = new callme6lackContext();
            Units units = new Units();
            var un = db.Units.FromSqlInterpolated($"SelectAllUnits").ToList();
            List<Units> units1 = new List<Units>();
            int i = 0;
            while (un.Count > i)
            {
                units1.Add(new Units()
                {
                    Id = Convert.ToInt32(un[i].Id),
                    Unit = un[i].Unit.ToString(),
                });
                i++;
            }

            var BRm = db.BuyRawMaterial.FromSqlInterpolated($"SelectAllBuyRawMaterials").ToList();
            List<BuyRawMaterial> buyRawMaterials = new List<BuyRawMaterial>();
            int k = 0;
            while (BRm.Count > k)
            {
                foreach (Units val in units1)
                {
                    if (val.Id == BRm[k].Unites)
                    {
                        units = val;
                        buyRawMaterials.Add(new BuyRawMaterial()
                        {
                            Id = Convert.ToInt32(BRm[k].Id),
                            Name = (BRm[k].Name).ToString(),
                            Unites=Convert.ToInt32(BRm[k].Unites),
                            Sum= Convert.ToDouble(BRm[k].Sum),
                            Amount=Convert.ToDouble(BRm[k].Amount),
                            UnitNavigation=units,
                        });
                    }
                }
                k++;
            }
            return View(buyRawMaterials);
        }

        // GET: BuyRawMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyRawMaterial = await _context.BuyRawMaterial
                .Include(b => b.UnitNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buyRawMaterial == null)
            {
                return NotFound();
            }

            return View(buyRawMaterial);
        }

        // GET: BuyRawMaterials/Create
        public IActionResult Create()
        {
            callme6lackContext db = new callme6lackContext();

            var BR = db.BuyRawMaterial.FromSqlInterpolated($"SelectAllBuyRawMaterials").ToList();

            BuyRawMaterial buyRawMaterial= new BuyRawMaterial();

            List<BuyRawMaterial> buyRawMaterials1 = new List<BuyRawMaterial>();

            foreach (var val in BR)
            {
                buyRawMaterials1.Add(new BuyRawMaterial()
                {
                    Id = val.Id,
                    Name= val.Name,
                    Unites= val.Unites,
                    Sum= val.Sum,
                    Amount=val.Amount
                });
            }
            ViewData["Unites"] = new SelectList(_context.Units, "Id", "Unit");
            return View();
        }

        // POST: BuyRawMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Unites,Sum,Amount")] BuyRawMaterial buyRawMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlInterpolated($"InsertBuyRawMaterials @Name={buyRawMaterial.Name}, @Unites={buyRawMaterial.Unites}, @Sum={buyRawMaterial.Sum},@Amount={buyRawMaterial.Amount}");
                return RedirectToAction(nameof(Index));
            }
            ViewData["Unites"] = new SelectList(_context.Units, "Id", "Unit", buyRawMaterial.Unites);
            return View(buyRawMaterial);
        }

        // GET: BuyRawMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            /*if (id == null)
            {
                return NotFound();
            }

            var buyRawMaterial = await _context.BuyRawMaterial.FindAsync(id);
            if (buyRawMaterial == null)
            {
                return NotFound();
            }*/
            var buyRawMaterials1 = _context.BuyRawMaterial.FromSqlInterpolated($"SelectIDBuyRawMaterials @ID={id}").ToList();
            BuyRawMaterial buyRawMaterial1 = new BuyRawMaterial();
            foreach (var val in buyRawMaterials1)
            {
                buyRawMaterial1.Id = val.Id;
                buyRawMaterial1.Name = val.Name;
                buyRawMaterial1.Unites = val.Unites;
                buyRawMaterial1.Sum = val.Sum;
                buyRawMaterial1.Amount = val.Amount;
            }
            ViewData["Unites"] = new SelectList(_context.Units, "Id", "Unit", buyRawMaterial1.Unites);
            return View(buyRawMaterial1);
        }

        // POST: BuyRawMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Unites,Sum,Amount")] BuyRawMaterial buyRawMaterial)
        {
            if (id != buyRawMaterial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlInterpolated($"UpdateBuyRawMaterials @ID={id},@Name={buyRawMaterial.Name}, @Unites={buyRawMaterial.Unites},@Sum={buyRawMaterial.Sum},@Amount={buyRawMaterial.Amount}");
                /*
                try
                {
                    _context.Update(buyRawMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuyRawMaterialExists(buyRawMaterial.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                */

                return RedirectToAction(nameof(Index));
            }
            ViewData["Unites"] = new SelectList(_context.Units, "Id", "Unit", buyRawMaterial.Unites);
            return View(buyRawMaterial);
        }

        // GET: BuyRawMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*if (id == null)
            {
                return NotFound();
            }

            var buyRawMaterial = await _context.BuyRawMaterial
                .Include(b => b.UnitNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buyRawMaterial == null)
            {
                return NotFound();
            }

            return View(buyRawMaterial);*/
            var buyRawMaterials1 = _context.BuyRawMaterial.FromSqlInterpolated($"SelectIDBuyRawMaterials @ID={id}").ToList();
            BuyRawMaterial buyRawMaterial1 = new BuyRawMaterial();
            foreach (var val in buyRawMaterials1)
            {
                buyRawMaterial1.Id = val.Id;
                buyRawMaterial1.Name = val.Name;
                buyRawMaterial1.Unites = val.Unites;
                buyRawMaterial1.Sum = val.Sum;
                buyRawMaterial1.Amount = val.Amount;
            }
            return View(buyRawMaterial1);
        }

        // POST: BuyRawMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            callme6lackContext callme6LackContext = new callme6lackContext();
            callme6LackContext.Database.ExecuteSqlInterpolated($"DeleteBuyRawMaterials @ID={id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
