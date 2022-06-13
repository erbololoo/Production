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
    public class ProdutionsController : Controller
    {
        private readonly callme6lackContext _context;

        public ProdutionsController(callme6lackContext context)
        {
            _context = context;
        }

        // GET: Produtions
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

            var PR = db.Prodution.FromSqlInterpolated($"SelectAllProdution").ToList();
            List<Prodution> produtions = new List<Prodution>();

            int m = 0;
            while (PR.Count > m)
            {
                foreach (Employees val in employees1)
                {
                    foreach (FinishedProducts val2 in finishedProducts1)
                    {
                        if (val.Id == PR[m].Employee && val2.Id == PR[m].Product)
                        {
                            employees = val;
                            finishedProducts = val2;
                            produtions.Add(new Prodution()
                            {
                                Id = Convert.ToInt32(PR[m].Id),
                                Product = Convert.ToInt32(PR[m].Product),
                                Amount = Convert.ToDouble(PR[m].Amount),
                                Date = Convert.ToDateTime(PR[m].Date),
                                Employee = Convert.ToInt32(PR[m].Employee),
                                EmployeeNavigation = employees,
                                ProductNavigation = finishedProducts,
                            });
                        }
                    }
                }
                m++;
            }
            return View(produtions);
        }

        // GET: Produtions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodution = await _context.Prodution
                .Include(p => p.EmployeeNavigation)
                .Include(p => p.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prodution == null)
            {
                return NotFound();
            }

            return View(prodution);
        }

        // GET: Produtions/Create
        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio");
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name");
            return View();
        }

        // POST: Produtions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Product,Amount,Date,Employee")] Prodution prodution)
        {
            if (ModelState.IsValid)
            {
                SqlParameter sqlParameterD = new SqlParameter("@ID", prodution.Product);
                SqlParameter sqlParameter = new SqlParameter("@Amount", prodution.Amount);
                var sqlParameterOut = new SqlParameter
                {
                    ParameterName = "@Boolean",
                    DbType = DbType.Int32,
                    Size = 11,
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw($"ProdutionSum @ID, @Amount, @Boolean OUT", sqlParameterD, sqlParameter, sqlParameterOut).ToString();
                int p = Convert.ToInt32(sqlParameterOut.Value);

                if (p == 0)
                {
                    _context.Database.ExecuteSqlInterpolated($"InsertProdution @Product={prodution.Product},@Amount={prodution.Amount},@Date={prodution.Date},@Employee={prodution.Employee}");
                    return RedirectToAction(nameof(Index));
                }
                else if (p==1)
                {
                    ViewBag.Massage = "Не хватает сырья!";
                }
                //var сountPR = prodution.Amount;
                //var ID = prodution.Product;

                //callme6lackContext db = new callme6lackContext();
                //List<BuyRawMaterial> rawmaterials = new List<BuyRawMaterial>();
                //List<Ingredients> ingredients2 = new List<Ingredients>();
                //List<FinishedProducts> finproducts = new List<FinishedProducts>();

                //var pr = ((from id in _context.Ingredients
                //           join c in _context.BuyRawMaterial on id.RawMaterial equals c.Id
                //           where (c.Amount < id.Amount * сountPR) && (id.Product == ID)
                //           select id)).ToList();

                //var SummaRaw = ((from value in _context.BuyRawMaterial
                //                 join c in _context.Ingredients on value.Id equals c.RawMaterial
                //                 where c.Product == ID
                //                 select ((float?)value.Sum / value.Amount * c.Amount * сountPR))).ToList();



                //var Summa = ((from value in _context.BuyRawMaterial
                //              join c in _context.Ingredients on value.Id equals c.RawMaterial
                //              where c.Product == ID
                //              select ((float?)value.Sum / value.Amount * c.Amount * сountPR))).Sum();

                //rawmaterials = ((from value in _context.BuyRawMaterial
                //                 join c in _context.Ingredients on value.Id equals c.RawMaterial
                //                 where c.Product == ID
                //                 select value).ToList());

                //var countIngr = ((from value in _context.Ingredients
                //                  join c in _context.BuyRawMaterial on value.RawMaterial equals c.Id
                //                  where value.Product == ID
                //                  select value.Amount * сountPR).ToList());



                //finproducts = ((from col in _context.FinishedProducts
                //                where col.Id == ID
                //                select col).ToList());

                //decimal sumfinpr = Convert.ToDecimal(finproducts[0].Sum);
                //decimal countfinpr = Convert.ToDecimal(finproducts[0].Amount);

                //if (pr.Count != 0)
                //{
                //    ViewBag.Message = "Ошибка!У вас недостаточное количество сырья!";
                //}
                //else
                //{
                //    decimal summ = sumfinpr + Convert.ToDecimal(Summa);
                //    decimal colfin = countfinpr + Convert.ToDecimal(сountPR);
                //    var Updateprod = db.FinishedProducts
                //        .Where(id => id.Id == ID)
                //        .First();
                //    Updateprod.Sum = (double?)summ;
                //    Updateprod.Amount = (float?)colfin;
                //    db.SaveChanges();

                //    int i = 0;
                //    while (SummaRaw.Count > i)
                //    {
                //        decimal v = (decimal)SummaRaw[i].Value;
                //        decimal col = (decimal)countIngr[i].Value;
                //        int IDraw = Convert.ToInt32(rawmaterials[i].Id);
                //        var UpdateRawMaterial = db.BuyRawMaterial
                //       .Where(id => id.Id == IDraw)
                //       .First();
                //        UpdateRawMaterial.Sum = (double?)(Convert.ToDecimal(UpdateRawMaterial.Sum) - Convert.ToDecimal(v));
                //        UpdateRawMaterial.Amount = (float?)UpdateRawMaterial.Amount - (float?)col;
                //        db.SaveChanges();
                //        i++;
                //    }

            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio", prodution.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", prodution.Product);
            return View(prodution);
        }

        // GET: Produtions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodution = await _context.Prodution.FindAsync(id);
            if (prodution == null)
            {
                return NotFound();
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio", prodution.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", prodution.Product);
            return View(prodution);
        }

        // POST: Produtions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Product,Amount,Date,Employee")] Prodution prodution)
        {
            if (id != prodution.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prodution);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutionExists(prodution.Id))
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
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio", prodution.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", prodution.Product);
            return View(prodution);
        }

        // GET: Produtions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodution = await _context.Prodution
                .Include(p => p.EmployeeNavigation)
                .Include(p => p.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prodution == null)
            {
                return NotFound();
            }

            return View(prodution);
        }

        // POST: Produtions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prodution = await _context.Prodution.FindAsync(id);
            _context.Prodution.Remove(prodution);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutionExists(int id)
        {
            return _context.Prodution.Any(e => e.Id == id);
        }
    }
}
