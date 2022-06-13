using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MSDB;

namespace MSDB.Controllers
{
    public class SaleOfProductsController : Controller
    {
        private readonly callme6lackContext _context;

        public SaleOfProductsController(callme6lackContext context)
        {
            _context = context;
        }

        // GET: SaleOfProducts
        public async Task<IActionResult> Index()
        {
            callme6lackContext db = new callme6lackContext();
            Employees employees = new Employees();
            var em = db.Employees.FromSqlInterpolated($"SelectEmployee").ToList();
            List<Employees> employees1 = new List<Employees>();

            int i = 0;
            while (em.Count > i)
            {
                employees1.Add(new Employees()
                {
                    Id = Convert.ToInt32(em[i].Id),
                    Fio = em[i].Fio.ToString()
                });
                i++;
            }

            FinishedProducts finishedProducts = new FinishedProducts();
            var FP = db.FinishedProducts.FromSqlInterpolated($"SelectAllFinishedProducts").ToList();
            List<FinishedProducts> finishedProducts1 = new List<FinishedProducts>();

            int k = 0;
            while (FP.Count > k)
            {
                finishedProducts1.Add(new FinishedProducts()
                {
                    Id = Convert.ToInt32(FP[k].Id),
                    Name = FP[k].Name.ToString()
                });
                k++;
            }

            var SP = db.SaleOfProducts.FromSqlInterpolated($"SelectAllSaleOfProducts").ToList();
            List<SaleOfProducts> saleOfProducts = new List<SaleOfProducts>();

            int m = 0;
            while (SP.Count > m)
            {
                foreach (Employees val in employees1)
                {
                    foreach (FinishedProducts val2 in finishedProducts1)
                    {
                        if (val.Id == SP[m].Employee && val2.Id == SP[m].Product)
                        {
                            employees = val;
                            finishedProducts = val2;
                            saleOfProducts.Add(new SaleOfProducts()
                            {
                                Id = Convert.ToInt32(SP[m].Id),
                                Product=Convert.ToInt32(SP[m].Product),
                                Amount = Convert.ToDouble(SP[m].Amount),
                                Sum = Convert.ToDouble(SP[m].Sum),
                                Date = Convert.ToDateTime(SP[m].Date),
                                Employee = Convert.ToInt32(SP[m].Employee),
                                EmployeeNavigation = employees,
                                ProductNavigation=finishedProducts,
                            });
                        }
                    }
                }
                m++;
            }
            return View(saleOfProducts);
        }

        // GET: SaleOfProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleOfProducts = await _context.SaleOfProducts
                .Include(s => s.EmployeeNavigation)
                .Include(s => s.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saleOfProducts == null)
            {
                return NotFound();
            }

            return View(saleOfProducts);
        }

        // GET: SaleOfProducts/Create
        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio");
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name");
            return View();
        }

        // POST: SaleOfProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Product,Amount,Sum,Date,Employee")] SaleOfProducts saleOfProducts)
        {
            if (ModelState.IsValid)
            {
                SqlParameter sqlParameterId = new SqlParameter("@ID", saleOfProducts.Product);
                SqlParameter sqlParameter = new SqlParameter("@Amount", saleOfProducts.Amount);

                var sqlParameterOut = new SqlParameter
                {
                    ParameterName = "@Percent",
                    DbType = DbType.Int32,
                    Size = 11,
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw($"SaleOfProductsSum @ID, @Amount, @Percent OUT",sqlParameterId, sqlParameter, sqlParameterOut).ToString();
                int p = Convert.ToInt32(sqlParameterOut.Value);

                if (p == 0)
                {
                    _context.Database.ExecuteSqlInterpolated($"InsertSaleOfProducts @Product={saleOfProducts.Product},@Amount={saleOfProducts.Amount},@Sum={saleOfProducts.Sum},@Date={saleOfProducts.Date},@Employee={saleOfProducts.Employee}");
                    return RedirectToAction(nameof(Index));
                }
                else if(p == 1)
                {
                    ViewBag.Massage = "Не хватает продукта!";
                }



                //    var ID = saleOfProducts.Id;
                //    var salecount = saleOfProducts.Amount;
                //    var prodId = saleOfProducts.Product;

                //    callme6lackContext db = new callme6lackContext();
                //    List<Budget> But = new List<Budget>();
                //    List<FinishedProducts> finproducts = new List<FinishedProducts>();

                //    finproducts = (from col in _context.FinishedProducts
                //                   where col.Id == prodId
                //                   select col).ToList();

                //    decimal s1 = Convert.ToDecimal(finproducts[0].Amount);
                //    decimal sumprod = Convert.ToDecimal(finproducts[0].Sum);

                //    But = (from Budgetamount in _context.Budget
                //           select Budgetamount).ToList();

                //    decimal s = Convert.ToDecimal(But[0].SumBudget);
                //    decimal prosent = Convert.ToDecimal(But[0].Procent);
                //    var sebestoimost = Convert.ToDecimal(sumprod) / Convert.ToDecimal(s1);

                //    decimal valprodcount = 0, valsumprod = 0, nadbavka = 0, colsumpr = 0;
                //    if (Convert.ToDecimal(s1) < Convert.ToDecimal(salecount))
                //    {
                //        ViewBag.Message = "Ошибка!У вас не хватает количества готовой продукции!";
                //    }
                //    else
                //    {
                //        valprodcount = Convert.ToDecimal(s1) - Convert.ToDecimal(salecount);
                //        valsumprod = (Convert.ToDecimal(salecount) * Convert.ToDecimal(sebestoimost));
                //        nadbavka = valsumprod + (valsumprod * Convert.ToDecimal(prosent)) / 100;
                //        colsumpr = (Convert.ToDecimal(sebestoimost) * Convert.ToDecimal(salecount));

                //        //Бюджет
                //        var col = db.Budget
                //            .Where(c => c.SumBudget == (double?)s)
                //            .FirstOrDefault();
                //        col.SumBudget = col.SumBudget + (float?)nadbavka;
                //        db.SaveChanges();

                //        //Готовая продукция
                //        var finprodcount = db.FinishedProducts
                //            .Where(r => r.Id == prodId)
                //            .FirstOrDefault();
                //        finprodcount.Amount = (float?)valprodcount;

                //        var finprodsum = db.FinishedProducts
                //            .Where(r => r.Id == prodId)
                //            .FirstOrDefault();
                //        finprodsum.Sum = finprodsum.Sum - (double?)valsumprod;
                //        db.SaveChanges();

                //        saleOfProducts.Sum = (double?)nadbavka;
                //        db.SaveChanges();
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio", saleOfProducts.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", saleOfProducts.Product);
            return View(saleOfProducts);
        }

        // GET: SaleOfProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleOfProducts = await _context.SaleOfProducts.FindAsync(id);
            if (saleOfProducts == null)
            {
                return NotFound();
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio", saleOfProducts.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", saleOfProducts.Product);
            return View(saleOfProducts);
        }

        // POST: SaleOfProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Product,Amount,Sum,Date,Employee")] SaleOfProducts saleOfProducts)
        {
            if (id != saleOfProducts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saleOfProducts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleOfProductsExists(saleOfProducts.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio", saleOfProducts.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", saleOfProducts.Product);
            return View(saleOfProducts);
        }

        // GET: SaleOfProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleOfProducts = await _context.SaleOfProducts
                .Include(s => s.EmployeeNavigation)
                .Include(s => s.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saleOfProducts == null)
            {
                return NotFound();
            }

            return View(saleOfProducts);
        }

        // POST: SaleOfProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var saleOfProducts = await _context.SaleOfProducts.FindAsync(id);
            _context.SaleOfProducts.Remove(saleOfProducts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleOfProductsExists(int id)
        {
            return _context.SaleOfProducts.Any(e => e.Id == id);
        }
    }
}
