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
    public class IngredientsController : Controller
    {
        private readonly callme6lackContext _context;

        public IngredientsController(callme6lackContext context)
        {
            _context = context;
        }

        // GET: Ingredients
        public async Task<IActionResult> Index(int Searching)
        {
            callme6lackContext db = new callme6lackContext();
            FinishedProducts finishedProducts = new FinishedProducts();
            var FP = db.FinishedProducts.FromSqlInterpolated($"SelectAllFinishedProducts").ToList();
            List<FinishedProducts> finishedProducts1 = new List<FinishedProducts>();

            int i = 0;
            while (FP.Count > i)
            {
                finishedProducts1.Add(new FinishedProducts()
                {
                    Id = Convert.ToInt32(FP[i].Id),
                    Name = FP[i].Name.ToString()
                }) ;
                i++;
            }

            BuyRawMaterial buyRawMaterial = new BuyRawMaterial();
            var BRM = db.BuyRawMaterial.FromSqlInterpolated($"SelectAllBuyRawMaterials").ToList();
            List<BuyRawMaterial> buyRawMaterial1 = new List<BuyRawMaterial>();

            int k = 0;
            while (BRM.Count > k)
            {
                buyRawMaterial1.Add(new BuyRawMaterial()
                {
                    Id = Convert.ToInt32(BRM[k].Id),
                    Name = BRM[k].Name.ToString()
                });
                k++;
            }

            var In = db.Ingredients.FromSqlInterpolated($"SelectAllIngredients").ToList();
            List<Ingredients> ingredients = new List<Ingredients>();

            int m = 0;
            while(In.Count>m)
            {
                foreach(FinishedProducts val in finishedProducts1)
                {
                    foreach(BuyRawMaterial val2 in buyRawMaterial1)
                    {
                        if(val.Id==In[m].Product && val2.Id == In[m].RawMaterial)
                        {
                            finishedProducts = val;
                            buyRawMaterial = val2;
                            ingredients.Add(new Ingredients()
                            {
                                Id = Convert.ToInt32(In[m].Id),
                                Product = Convert.ToInt32(In[m].Product),
                                RawMaterial = Convert.ToInt32(In[m].RawMaterial),
                                Amount = Convert.ToDouble(In[m].Amount),
                                ProductNavigation = finishedProducts,
                                RawMaterialNavigation = buyRawMaterial,
                            });
                        }
                    }
                }
                m++;
            }

            ViewData["Product"] = new SelectList(finishedProducts1, "Id", "Name");
            if(Searching!=0)
            {
                var s = _context.Ingredients.FromSqlInterpolated($"Searching @search={ Searching}").ToList();
                List<Ingredients> se = new List<Ingredients>();

                int n = 0;
                while (s.Count > n)
                {
                    foreach (FinishedProducts val in finishedProducts1)
                    {
                        foreach (BuyRawMaterial val2 in buyRawMaterial1)
                        {
                            if (val.Id == s[n].Product && val2.Id == s[n].RawMaterial)
                            {
                                finishedProducts = val;
                                buyRawMaterial = val2;
                                se.Add(new Ingredients()
                                {
                                    Id = Convert.ToInt32(s[n].Id),
                                    Product = Convert.ToInt32(s[n].Product),
                                    RawMaterial = Convert.ToInt32(s[n].RawMaterial),
                                    Amount = Convert.ToDouble(s[n].Amount),
                                    ProductNavigation = finishedProducts,
                                    RawMaterialNavigation = buyRawMaterial,
                                });
                            }
                        }
                    }
                    n++;
                }
                return View(se);
            }
            else
            {
                return View(ingredients);
            }
            //ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name");
            //var callme6lackContext = _context.Ingredients.Include(i => i.ProductNavigation).Include(i => i.RawMaterialNavigation);
            //return View(await callme6lackContext.ToListAsync());
        }

        // GET: Ingredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredients = await _context.Ingredients
                .Include(i => i.ProductNavigation)
                .Include(i => i.RawMaterialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingredients == null)
            {
                return NotFound();
            }

            return View(ingredients);
        }

        // GET: Ingredients/Create
        public IActionResult Create(int ID)
        {
            callme6lackContext db = new callme6lackContext();

            var In = db.Ingredients.FromSqlInterpolated($"SelectAllIngredients").ToList();

            List<Ingredients> ingredients = new List<Ingredients>();

            //foreach (var val in In)
            //{
            //    if (Searching ==val.Product)
            //    {
            //        ingredients.Add(new Ingredients()
            //        {
            //            Id = val.Id,
            //            Product = val.Product,
            //            RawMaterial = val.RawMaterial,
            //            Amount = val.Amount
            //        });
            //    }
            //}

            ViewData["Product"] = new SelectList(_context.FinishedProducts.Where(e=>e.Id==ID), "Id", "Name");
            ViewData["RawMaterial"] = new SelectList(_context.BuyRawMaterial, "Id", "Name");
            return View();
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Product,RawMaterial,Amount")] Ingredients ingredients)
        {
            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlInterpolated($"InsertIngredients @Product={ingredients.Product}, @RawMaterial={ingredients.RawMaterial}, @Amount={ingredients.Amount}");
                return RedirectToAction("Index", new { Searching= ingredients.Product});
            }
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Id", ingredients.Product);
            ViewData["RawMaterial"] = new SelectList(_context.BuyRawMaterial, "Id", "Id", ingredients.RawMaterial);
            return View(ingredients);
        }

        // GET: Ingredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var ingredients = await _context.Ingredients.FindAsync(id);
            //if (ingredients == null)
            //{
            //    return NotFound();
            //}
            var In = _context.Ingredients.FromSqlInterpolated($"SelectIDIngredients @ID={id}").ToList();
            Ingredients ingredients = new Ingredients();
            foreach (var val in In)
            {
                ingredients.Id = val.Id;
                ingredients.Product = val.Product;
                ingredients.RawMaterial = val.RawMaterial;
                ingredients.Amount = val.Amount;
            }
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", ingredients.Product);
            ViewData["RawMaterial"] = new SelectList(_context.BuyRawMaterial, "Id", "Name", ingredients.RawMaterial);
            return View(ingredients);
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Product,RawMaterial,Amount")] Ingredients ingredients)
        {
            if (id != ingredients.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlInterpolated($"UpdateIngredients @ID={id},@Product={ingredients.Product}, @RawMaterial={ingredients.RawMaterial}, @Amount={ingredients.Amount}");
                //try
                //{
                //    _context.Update(ingredients);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!IngredientsExists(ingredients.Id))
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
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", ingredients.Product);
            ViewData["RawMaterial"] = new SelectList(_context.BuyRawMaterial, "Id", "Name", ingredients.RawMaterial);
            return View(ingredients);
        }

        // GET: Ingredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var ingredients = await _context.Ingredients
            //    .Include(i => i.ProductNavigation)
            //    .Include(i => i.RawMaterialNavigation)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (ingredients == null)
            //{
            //    return NotFound();
            //}

            //return View(ingredients);
            var ingredients= _context.Ingredients.FromSqlInterpolated($"SelectIDIngredients @ID={id}").ToList();
            Ingredients ingredients1 = new Ingredients();
            foreach (var val in ingredients)
            {
                ingredients1.Id = val.Id;
                ingredients1.Product = val.Product;
                ingredients1.RawMaterial = val.RawMaterial;
                ingredients1.Amount = val.Amount;
            }
            return View(ingredients1);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            callme6lackContext callme6LackContext = new callme6lackContext();
            callme6LackContext.Database.ExecuteSqlInterpolated($"DeleteIngredients @ID={id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
