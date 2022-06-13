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
    public class BudgetsController : Controller
    {
        private readonly callme6lackContext _context;

        public BudgetsController(callme6lackContext context)
        {
            _context = context;
        }

        // GET: Budgets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Budget.ToListAsync());
        }

        // GET: Budgets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budget
                .FirstOrDefaultAsync(m => m.Id == id);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // GET: Budgets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SumBudget,Procent,Bonus")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlRaw($"InsertBudgets @SumBudgets={budget.SumBudget},@Prosent={budget.Procent},@Bonus={budget.Bonus}");
                return RedirectToAction(nameof(Index));
            }
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {/*
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budget.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }*/
            var budget = _context.Budget.FromSqlInterpolated($"SelectIDBudgets @ID={id}").ToList();
            Budget budget1 = new Budget();
            foreach(var val in budget)
            {
                budget1.Id = val.Id;
                budget1.SumBudget = val.SumBudget;
                budget1.Procent = val.Procent;
                budget1.Bonus = val.Bonus;
            }
            return View(budget1);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SumBudget,Procent,Bonus")] Budget budget)
        {
            if (id != budget.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlRaw($"UpdateBudgets @ID={id}, @SumBudgets={budget.SumBudget},@Prosent={budget.Procent},@Bonus={budget.Bonus}");
                return RedirectToAction(nameof(Index));
                /*
                try
                {
                    _context.Update(budget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetExists(budget.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
                */
            }
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budget
                .FirstOrDefaultAsync(m => m.Id == id);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
            */
            var budget = _context.Budget.FromSqlInterpolated($"SelectIDBudgets @ID={id}").ToList();
            Budget budget1 = new Budget();
            foreach(var val in budget)
            {
                budget1.Id = val.Id;
                budget1.SumBudget = val.SumBudget;
                budget1.Procent = val.Procent;
                budget1.Bonus = val.Bonus;
            }
            return View(budget1);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            callme6lackContext callme6LackContext = new callme6lackContext();
            callme6LackContext.Database.ExecuteSqlInterpolated($"DeleteBudgets @ID={id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
