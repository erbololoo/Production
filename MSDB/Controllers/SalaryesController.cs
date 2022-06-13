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
using MSDB.Models;

namespace MSDB.Controllers
{
    public class SalaryesController : Controller
    {
        float SumSalary = 0;
        private readonly callme6lackContext _context;

        public SalaryesController(callme6lackContext context)
        {
            _context = context;
        }

        // GET: Salaryes
        public async Task<IActionResult> Index(int yearstring, int monthstring, string M, int sum)
        {
            ViewData["Yea"] = new SelectList(_context.Years, "Year_Name", "Year_Name");
            ViewData["Month"] = new SelectList(_context.Months, "Id", "Month_Name");
            callme6lackContext db = new callme6lackContext();
            Employees employees = new Employees();
            var FP = db.Employees.FromSqlInterpolated($"SelectEmployee").ToList(); 
            // Запрос на хранимую процедуру для взятия количества записей
            List<Employees> employees1 = new List<Employees>();

            int i = 0;
            while (FP.Count > i)
            // Цикл от выше взятой записи
            {
                // Запись в модель
                employees1.Add(new Employees()
                {
                    Id = Convert.ToInt32(FP[i].Id),
                    Fio = FP[i].Fio.ToString()
                });
                i++;
            }

            Years years = new Years();
            var YE = db.Years.FromSqlInterpolated($"SelectAllYears").ToList();
            // Запрос на хранимую процедуру для взятия количества записей
            List<Years> years1 = new List<Years>();

            int k = 0;
            while (YE.Count > k)
            // Цикл от выше взятой записи
            {
                // Запись в модель
                years1.Add(new Years()
                {
                    Year_Name=Convert.ToInt32(YE[k].Year_Name)
                });
                k++;
            }

            Months months = new Months();
            var MO = db.Months.FromSqlInterpolated($"SelectAllMonth").ToList();
            // Запрос на хранимую процедуру для взятия количества записей
            List<Months> months1 = new List<Months>();

            int m = 0;
            while (MO.Count > m)
            // Цикл от выше взятой записи
            {
                // Запись в модель
                months1.Add(new Months()
                {
                    Id = Convert.ToInt32(MO[m].Id),
                    Month_Name = MO[m].Month_Name.ToString()
                });
                m++;
            }
            
            var In = db.Salaryes.FromSqlInterpolated($"SelectAllSalaryes").ToList();
            // Запрос на хранимую процедуру для взятия количества записей
            List<Salaryes> salaryes = new List<Salaryes>();
            int f = 0;
            while (In.Count > f)
            // Цикл от выше взятой записи
            {
                foreach (Employees val in employees1)
                // Берет запись из локальной переменной Сотрудник и записывает в новую локальную переменную
                {
                    foreach(Years val2 in years1)
                    // Берет запись из локальной переменной Год и записывает в новую локальную переменную
                    {
                        foreach (Months val3 in months1)
                        // Берет запись из локальной переменной Месяц и записывает в новую локальную переменную
                        {
                            if (val.Id == In[f].Employee && val2.Year_Name == In[f].Year && val3.Id == In[f].Month)
                            // Логическая переменная, которая проверяет на схожесть записей
                            {
                                employees = val;
                                years = val2;
                                months = val3;
                                salaryes.Add(new Salaryes()
                                // Запись в модель
                                {
                                    Id = Convert.ToInt32(In[f].Id),
                                    Year = Convert.ToInt32(In[f].Year),
                                    Month = Convert.ToInt32(In[f].Month),
                                    ParticipationInThePurchase = (short)Convert.ToInt32(In[f].ParticipationInThePurchase),
                                    ParticipationOnSale = (short)Convert.ToInt32(In[f].ParticipationOnSale),
                                    ParticipationOnProduction = (short)Convert.ToInt32(In[f].ParticipationOnProduction),
                                    TotalNumberOfParticipations=Convert.ToInt32(In[f].TotalNumberOfParticipations),
                                    Bonus = Convert.ToInt32(In[f].Bonus),
                                    Salary = Convert.ToInt32(In[f].Salary),
                                    TotalSum = (float)In[f].TotalSum,
                                    Boolean= (bool)In[f].Boolean,
                                    Year_NameNavigation = years,
                                    MonthNavigation = months,
                                    EmployeeNavigation=employees,
                                });
                            }
                        }
                    }
                }
                f++;
            }
            
            if(yearstring != 0 && monthstring!=0)
            // Логический оператор, который проверяет на использование фильтрации
            {
                var sal = _context.Salaryes.FromSqlInterpolated($"SelectYM @y={ yearstring}, @m={monthstring}").ToList();

                List<Salaryes> salaryes1 = new List<Salaryes>();
                ViewBag.Mas = sum;
                if (sal.Count==0)
                // Логический оператор, который проверяет на наличие записи в таблице "Зарплата"
                // Если запись по выбранной дате не имеет записи, то
                {
                    SqlParameter sqlParameterY = new SqlParameter("@Year", yearstring);
                    // Входной параметр год
                    SqlParameter sqlParameterM = new SqlParameter("@Month", monthstring);
                    // Входной параметр месяц
                    db.Database.ExecuteSqlInterpolated($"Salaryies @y={yearstring}, @m={monthstring}").ToString();
                    
                    // Запись в таблицу "Зарплата" по выбранной дате
                    return RedirectToAction("Index", new { yearstring = sqlParameterY, monthstring = sqlParameterM });
                }
                else
                // Иначе
                {
                    double summ = 0;
                    for(int j = 0; j < sal.Count; j++)
                    {
                        summ += sal[j].TotalSum;
                    }
                    ViewBag.Summa = summ;

                    int n = 0;
                    while (sal.Count > n)
                    {
                        foreach (Employees val in employees1)
                        {
                            foreach (Years val2 in years1)
                            {
                                foreach (Months val3 in months1)
                                {
                                    if (val.Id == sal[n].Employee && val2.Year_Name == sal[n].Year && val3.Id == sal[n].Month)
                                    {
                                        employees = val;
                                        years = val2;
                                        months = val3;
                                        salaryes1.Add(new Salaryes()
                                        {
                                            Id = Convert.ToInt32(sal[n].Id),
                                            Year = Convert.ToInt32(sal[n].Year),
                                            Month = Convert.ToInt32(sal[n].Month),
                                            ParticipationInThePurchase = (short)Convert.ToInt32(sal[n].ParticipationInThePurchase),
                                            ParticipationOnSale = (short)Convert.ToInt32(sal[n].ParticipationOnSale),
                                            ParticipationOnProduction = (short)Convert.ToInt32(sal[n].ParticipationOnProduction),
                                            TotalNumberOfParticipations = Convert.ToInt32(sal[n].TotalNumberOfParticipations),
                                            Bonus = Convert.ToInt32(sal[n].Bonus),
                                            Salary = Convert.ToInt32(sal[n].Salary),
                                            TotalSum = (float)sal[n].TotalSum,
                                            Boolean = (bool)sal[n].Boolean,
                                            Year_NameNavigation = years,
                                            MonthNavigation = months,
                                            EmployeeNavigation = employees,
                                        });
                                    }
                                }
                            }
                        }
                        n++;
                    }
                    return View(sal);
                }
            }
            else
            {
                ViewBag.Message = M;
                
                // Сообщение о каких-либо ошибках
                return View(salaryes);
            }
            ViewData["Yea"] = new SelectList(_context.Years, "Year_Name", "Year_Name");
            ViewData["Month"] = new SelectList(_context.Months, "Id", "Month_Name");
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio");
        }

        public async Task<IActionResult> IndexAdd(int yearstring, int monthstring) // Метод для выдачи заработной платы сотрудникам
        {
            ViewData["Yea"] = new SelectList(_context.Years, "Year_Name", "Year_Name");
            ViewData["Month"] = new SelectList(_context.Months, "Id", "Month_Name");
            callme6lackContext db = new callme6lackContext();
            SqlParameter sqlParameterY = new SqlParameter("@y", yearstring);
            // Входной параметр год
            SqlParameter sqlParameterM = new SqlParameter("@m", monthstring);
            // Входной параметр месяц
            var sqlParameterOut = new SqlParameter
            // Выходной параметр
            {
                ParameterName = "@p",
                DbType = DbType.Int32,
                Size = 11   ,
                Direction = ParameterDirection.Output
            };
            _context.Database.ExecuteSqlRaw($"CheckingBudget @y, @m, @p OUT", sqlParameterY, sqlParameterM, sqlParameterOut);
            // Проверка на выдачу заработной платы
            int p = Convert.ToInt32(sqlParameterOut.Value);
            int sum = Convert.ToInt32(sqlParameterM.Value);
            
            if (p==1)
            // Логический оператор, который проверяет допустимость суммы для выдачи заработной платы сотрудникам
            {
                string M= "Не хватает бюджета на выдачу зарплаты!";
                ViewBag.Ma = "Общая сумма зарплаты: " + sum;
                return RedirectToAction("Index", new { yearstring = sqlParameterY, monthstring = sqlParameterM, M,sum });
                // Возвращает значения в метод INDEX
            }
            else if(p==2)
            // Логический оператор, который проверяет о выдаче заработной платы по выбранной дате
            {
                string M = "Зарплата уже выдана!";
                ViewBag.Ma = "Общая сумма зарплаты: " + sum;
                return RedirectToAction("Index", new { yearstring = sqlParameterY, monthstring = sqlParameterM, M, sum });
                // Возвращает значения в метод INDEX
            }
            else
            // Логический оператор, который проверяет если выше перечисленный логические операторы не использовались,
            // то выдает оклад сотрудникам по выбранной дате
            {
                db.Database.ExecuteSqlInterpolated($"BooleanUpdate @y={yearstring}, @m={monthstring}").ToString();
                ViewBag.Ma = "Общая сумма зарплаты: " + sum;
                return RedirectToAction("Index", new { yearstring = sqlParameterY, monthstring = sqlParameterM, sum });
                // Возвращает значения в метод INDEX
            }
            return View();
        }

        // GET: Salaryes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryes = await _context.Salaryes
                .Include(s => s.EmployeeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salaryes == null)
            {
                return NotFound();
            }

            return View(salaryes);
        }

        // GET: Salaryes/Create
        public IActionResult Create()
        {
            ViewData["Yea"] = new SelectList(_context.Years, "Year_Name", "Year_Name");
            ViewData["Month"] = new SelectList(_context.Months, "Id", "Month_Name");
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio");
            return View();
        }

        // POST: Salaryes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Year,Month,Employee,ParticipationInThePurchase,ParticipationOnSale,ParticipationOnProduction,TotalNumberOfParticipations,Bonus,Salary,TotalSum,Boolean")] Salaryes salaryes)
        {
            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlInterpolated($"Salaryies @y={salaryes.Year}, @m={salaryes.Month}");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Yea"] = new SelectList(_context.Years, "Year_Name", "Year_Name", salaryes.Year);
            ViewData["Month"] = new SelectList(_context.Months, "Id", "Month_Name", salaryes.Month);
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio", salaryes.Employee);
            return View(salaryes);
        }

        // GET: Salaryes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryes = await _context.Salaryes.FindAsync(id);
            if (salaryes == null)
            {
                return NotFound();
            }
            ViewData["Yea"] = new SelectList(_context.Years, "Year_Name", "Year_Name", salaryes.Year);
            ViewData["Month"] = new SelectList(_context.Months, "Id", "Month_Name", salaryes.Month);
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio", salaryes.Employee);
            return View(salaryes);
        }

        // POST: Salaryes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Year,Month,Employee,ParticipationInThePurchase,ParticipationOnSale,ParticipationOnProduction,TotalNumberOfParticipations,Bonus,Salary,TotalSum,Boolean")] Salaryes salaryes)
        {
            if (id != salaryes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salaryes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryesExists(salaryes.Id))
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
            ViewData["Yea"] = new SelectList(_context.Years, "Year_Name", "Year_Name", salaryes.Year);
            ViewData["Month"] = new SelectList(_context.Months, "Id", "Month_Name", salaryes.Month);
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Fio", salaryes.Employee);
            return View(salaryes);
        }

        // GET: Salaryes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryes = await _context.Salaryes
                .Include(s => s.EmployeeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salaryes == null)
            {
                return NotFound();
            }

            return View(salaryes);
        }

        // POST: Salaryes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salaryes = await _context.Salaryes.FindAsync(id);
            _context.Salaryes.Remove(salaryes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryesExists(int id)
        {
            return _context.Salaryes.Any(e => e.Id == id);
        }
    }
}
