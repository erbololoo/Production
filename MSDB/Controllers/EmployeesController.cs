using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MSDB;

namespace MSDB.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly callme6lackContext _context;

        public EmployeesController(callme6lackContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            callme6lackContext db = new callme6lackContext();
            Positions positions = new Positions();
            var pos = db.Positions.FromSqlInterpolated($"SelectAllPositions").ToList();
            List<Positions> positions1 = new List<Positions>();
            int i = 0;
            while(pos.Count>i)
            {
                positions1.Add(new Positions()
                {
                    Id = Convert.ToInt32(pos[i].Id),
                    Position = pos[i].Position.ToString(),
                }) ;
                i++;
            }

            var em = db.Employees.FromSqlInterpolated($"SelectEmployee").ToList();
            List<Employees> employees1 = new List<Employees>();
            int k = 0;
            while (em.Count > k)
            {
                foreach(Positions val in positions1)
                {
                    if (val.Id == em[k].Position)
                    {
                        positions = val;
                        employees1.Add(new Employees()
                        {
                            Id = Convert.ToInt32(em[k].Id),
                            Fio = (em[k].Fio).ToString(),
                            Position = Convert.ToInt32(em[k].Position),
                            Salary = Convert.ToDouble(em[k].Salary),
                            Adress = (em[k].Adress).ToString(),
                            PhoneNumber = (em[k].PhoneNumber).ToString(),
                            PositionNavigation = positions,
                        });
                    }
                }
                k++;
            }
            return View(employees1);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .Include(e => e.PositionNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            callme6lackContext db = new callme6lackContext();
            var em = db.Employees.FromSqlInterpolated($"SelectEmployee").ToList();

            Employees employees = new Employees();

            List<Employees> employees1 = new List<Employees>();

            foreach(var val in em)
            {
                employees1.Add(new Employees()
                {
                    Id = val.Id,
                    Fio = val.Fio,
                    Position = val.Position,
                    Salary = val.Salary,
                    Adress = val.Adress,
                    PhoneNumber = val.PhoneNumber,
                });
            }

            ViewData["Position"] = new SelectList(_context.Positions, "Id", "Position");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fio,Position,Salary,Adress,PhoneNumber")] Employees employees)
        {
            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlInterpolated($"InsertEmployee @Fio={employees.Fio}, @Positons={employees.Position}, @Salary={employees.Salary},@Adress={employees.Adress}, @PhoneNumber={employees.PhoneNumber}");
                return RedirectToAction(nameof(Index));
            }
            ViewData["Position"] = new SelectList(_context.Positions, "Id", "Id", employees.Position);
            return View(employees);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            /*if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }*/
            var employees = _context.Employees.FromSqlInterpolated($"SelectIDEmployee @ID={id}").ToList();
            Employees employees1 = new Employees();
            foreach (var val in employees)
            {
                employees1.Id = val.Id;
                employees1.Fio = val.Fio;
                employees1.Position = val.Position;
                employees1.Salary = val.Salary;
                employees1.Adress = val.Adress;
                employees1.PhoneNumber = val.PhoneNumber;
            }
            ViewData["Position"] = new SelectList(_context.Positions, "Id", "Position", employees1.Position);
            return View(employees1);

        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fio,Position,Salary,Adress,PhoneNumber")] Employees employees)
        {
            if (id != employees.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlInterpolated($"UpdateEmployee @ID={id},@Fio={employees.Fio}, @Postios={employees.Position},@Salary={employees.Salary}, @Adress={employees.Adress},@PhonNumber={employees.PhoneNumber}");
                /*
                try
                {
                    _context.Update(employees);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesExists(employees.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }*/
                return RedirectToAction(nameof(Index));
            }
            ViewData["Position"] = new SelectList(_context.Positions, "Id", "Position", employees.Position);
            return View(employees);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .Include(e => e.PositionNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);*/
            var employees = _context.Employees.FromSqlInterpolated($"SelectIDEmployee @ID={id}").ToList();
            Employees employees1 = new Employees();
            foreach (var val in employees)
            {
                employees1.Id = val.Id;
                employees1.Fio = val.Fio;
                employees1.Position = val.Position;
                employees1.Salary = val.Salary;
                employees1.Adress = val.Adress;
                employees1.PhoneNumber = val.PhoneNumber;
            }
            return View(employees1);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            callme6lackContext callme6LackContext = new callme6lackContext();
            callme6LackContext.Database.ExecuteSqlInterpolated($"DeleteEmployee @ID={id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
