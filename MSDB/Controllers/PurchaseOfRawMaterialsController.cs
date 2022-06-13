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
    public class PurchaseOfRawMaterialsController : Controller
    {
        private readonly callme6lackContext _context;

        public PurchaseOfRawMaterialsController(callme6lackContext context)
        {
            _context = context;
        }

        // GET: PurchaseOfRawMaterials
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

            var PRM = db.PurchaseOfRawMaterials.FromSqlInterpolated($"SelectAllPurchaseOfRawMaterials").ToList();
            List<PurchaseOfRawMaterials> purchaseOfRawMaterials = new List<PurchaseOfRawMaterials>();

            int m = 0;
            while (PRM.Count > m)
            {
                foreach (Employees val in employees1)
                {
                    foreach (BuyRawMaterial val2 in buyRawMaterial1)
                    {
                        if (val.Id == PRM[m].Employee && val2.Id == PRM[m].RawMaterial)
                        {
                            employees = val;
                            buyRawMaterial = val2;
                            purchaseOfRawMaterials.Add(new PurchaseOfRawMaterials()
                            {
                                Id = Convert.ToInt32(PRM[m].Id),
                                RawMaterial = Convert.ToInt32(PRM[m].RawMaterial),
                                Amount = Convert.ToDouble(PRM[m].Amount),
                                Sum = Convert.ToDouble(PRM[m].Sum),
                                Date= Convert.ToDateTime(PRM[m].Date),
                                Employee=Convert.ToInt32(PRM[m].Employee),
                                EmployeeNavigation=employees,
                                RawMaterialNavigation = buyRawMaterial,
                            });
                        }
                    }
                }
                m++;
            }
            return View(purchaseOfRawMaterials);
        }

        // GET: PurchaseOfRawMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOfRawMaterials = await _context.PurchaseOfRawMaterials
                .Include(p => p.EmployeeNavigation)
                .Include(p => p.RawMaterialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseOfRawMaterials == null)
            {
                return NotFound();
            }

            return View(purchaseOfRawMaterials);
        }

        // GET: PurchaseOfRawMaterials/Create
        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio");
            ViewData["RawMaterial"] = new SelectList(_context.BuyRawMaterial, "Id", "Name");
            return View();
        }

        // POST: PurchaseOfRawMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RawMaterial,Amount,Sum,Date,Employee")] PurchaseOfRawMaterials purchaseOfRawMaterials)
        {
            if (ModelState.IsValid)
            {
                SqlParameter sqlParameter = new SqlParameter("@Sum", purchaseOfRawMaterials.Sum);
                var sqlParameterOut = new SqlParameter
                {
                    ParameterName = "@Percent",
                    DbType = DbType.Int32,
                    Size = 11,
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw($"PurchaseOfRawMaterialsSum @Sum, @Percent OUT", sqlParameter, sqlParameterOut).ToString();
                var p = Convert.ToString(sqlParameterOut.Value);

                if (p == "")
                {
                    _context.Database.ExecuteSqlInterpolated($"InsertPurchaseOfRawMaterials @RawMaterial={purchaseOfRawMaterials.RawMaterial},@Amount={purchaseOfRawMaterials.Amount},@Sum={purchaseOfRawMaterials.Sum},@Date={purchaseOfRawMaterials.Date},@Employee={purchaseOfRawMaterials.Employee}");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Massage = "Ошибка!Введенная сумма превышает бюджет! Введите сумму меньше, чем бюджет!";
                }
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio", purchaseOfRawMaterials.Employee);
            ViewData["RawMaterial"] = new SelectList(_context.BuyRawMaterial, "Id", "Name", purchaseOfRawMaterials.RawMaterial);
            return View(purchaseOfRawMaterials);
            //var ID = purchaseOfRawMaterials.RawMaterial;
            //var Amo = purchaseOfRawMaterials.Amount;
            //var Idpur = purchaseOfRawMaterials.Id;

            //callme6lackContext db = new callme6lackContext();
            //List<Budget> Bud = new List<Budget>();

            //Bud = (from SumBudget in _context.Budget
            //       select SumBudget).ToList();

            //List<BuyRawMaterial> Rawmaterials = new List<BuyRawMaterial>();

            //Rawmaterials = (from Amount in _context.BuyRawMaterial
            //                 where Amount.Id == ID
            //                 select Amount).ToList();

            //PurchaseOfRawMaterials Rawmaterials1 = new PurchaseOfRawMaterials();
            //Budget budgets = new Budget();

            //decimal s1 = Convert.ToDecimal(Rawmaterials[0].Amount);
            //decimal s = Convert.ToDecimal(Bud[0].SumBudget);
            //decimal s2 = Convert.ToDecimal(Rawmaterials[0].Sum); 
            //var Sum = purchaseOfRawMaterials.Sum;
            //decimal valsum, valcount, valrawsum;
            //if (Convert.ToDecimal(s) < Convert.ToDecimal(Sum))
            //{
            //    ViewBag.Massage ="Ошибка!Введенная сумма превышает бюджет! Введите сумму меньше, чем бюджет!";
            //}
            //else
            //{
            //    valsum = Convert.ToDecimal(s) - Convert.ToDecimal(Sum);
            //    valcount = Convert.ToDecimal(s1) + Convert.ToDecimal(Amo);
            //    valrawsum = Convert.ToDecimal(s2) + Convert.ToDecimal(Sum); 
            //    var col = db.Budget
            //        .Where(c => c.SumBudget == (double?)s)
            //        .FirstOrDefault();
            //    col.SumBudget = (float)valsum;
            //    db.SaveChanges();

            //    var colRaw = db.BuyRawMaterial
            //        .Where(r => r.Id == ID)
            //        .FirstOrDefault();
            //    colRaw.Amount = (double?)valcount;
            //    db.SaveChanges();

            //    var colraws = db.BuyRawMaterial
            //        .Where(x => x.Id == ID)
            //        .FirstOrDefault();
            //    colraws.Sum = (double?)valrawsum;
            //    db.SaveChanges();

            //    _context.Add(purchaseOfRawMaterials);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

        }

        // GET: PurchaseOfRawMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var PRM = _context.PurchaseOfRawMaterials.FromSqlInterpolated($"SelectIDPurchaseOfRawMaterials @ID={id}").ToList();
            PurchaseOfRawMaterials purchaseOfRawMaterials = new PurchaseOfRawMaterials();
            foreach (var val in PRM)
            {
                purchaseOfRawMaterials.Id = val.Id;
                purchaseOfRawMaterials.RawMaterial = val.RawMaterial;
                purchaseOfRawMaterials.Amount = val.Amount;
                purchaseOfRawMaterials.Sum = val.Sum;
                purchaseOfRawMaterials.Date = val.Date;
                purchaseOfRawMaterials.Employee = val.Employee;
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio", purchaseOfRawMaterials.Employee);
            ViewData["RawMaterial"] = new SelectList(_context.BuyRawMaterial, "Id", "Name", purchaseOfRawMaterials.RawMaterial);
            return View(purchaseOfRawMaterials);
        }

        // POST: PurchaseOfRawMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RawMaterial,Amount,Sum,Date,Employee")] PurchaseOfRawMaterials purchaseOfRawMaterials)
        {
            if (id != purchaseOfRawMaterials.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlInterpolated($"UpdatePurchaseOfRawMaterials @ID={id},@RawMaterail={purchaseOfRawMaterials.RawMaterial}, @Amount={purchaseOfRawMaterials.Amount}, @Sum={purchaseOfRawMaterials.Sum}, @Date={purchaseOfRawMaterials.Date}, @Employee={purchaseOfRawMaterials.Employee}");
                //try
                //{
                //    _context.Update(purchaseOfRawMaterials);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!PurchaseOfRawMaterialsExists(purchaseOfRawMaterials.Id))
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
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio", purchaseOfRawMaterials.Employee);
            ViewData["RawMaterial"] = new SelectList(_context.BuyRawMaterial, "Id", "Name", purchaseOfRawMaterials.RawMaterial);
            return View(purchaseOfRawMaterials);
        }

        // GET: PurchaseOfRawMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var purchaseOfRawMaterials = await _context.PurchaseOfRawMaterials
            //    .Include(p => p.EmployeeNavigation)
            //    .Include(p => p.RawMaterialNavigation)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (purchaseOfRawMaterials == null)
            //{
            //    return NotFound();
            //}

            //return View(purchaseOfRawMaterials);
            var PRM = _context.PurchaseOfRawMaterials.FromSqlInterpolated($"SelectIDPurchaseOfRawMaterials @ID={id}").ToList();
            PurchaseOfRawMaterials purchaseOfRawMaterials = new PurchaseOfRawMaterials();
            foreach (var val in PRM)
            {
                purchaseOfRawMaterials.Id = val.Id;
                purchaseOfRawMaterials.RawMaterial = val.RawMaterial;
                purchaseOfRawMaterials.Amount = val.Amount;
                purchaseOfRawMaterials.Sum = val.Sum;
                purchaseOfRawMaterials.Date = val.Date;
                purchaseOfRawMaterials.Employee = val.Employee;
            }
            return View(purchaseOfRawMaterials);
        }

        // POST: PurchaseOfRawMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            callme6lackContext callme6LackContext = new callme6lackContext();
            callme6LackContext.Database.ExecuteSqlInterpolated($"DeletePurchaseOfRawMaterials @ID={id}");
            return RedirectToAction(nameof(Index));
        }

    }
}
