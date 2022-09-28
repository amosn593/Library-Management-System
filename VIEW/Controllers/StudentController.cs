using AspNetCoreHero.ToastNotification.Abstractions;
using DAL.Data;
using DOMAIN.IConfiguration;
using DOMAIN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using USERVIEW.Areas.Identity.Data;

namespace VIEW.Controllers
{
    [Authorize(Policy = "Basic")]
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LibraryDbContext _context;
        private readonly INotyfService _notyf;
        private readonly UserManager<USERVIEWUser> _userManager;

        public StudentController(IUnitOfWork unitOfWork,
            INotyfService notyf,
            LibraryDbContext context,
            UserManager<USERVIEWUser> userManager
            )
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _notyf = notyf;
            _userManager = userManager;

        }
        // GET All Students: StudentController
        public async Task<IActionResult> Index(string? AdminNo, int pg = 1)
        {
            if (String.IsNullOrEmpty(AdminNo))
            {
                try
                {
                    var students = await _unitOfWork.Student.GetAll();

                    int pageSize = PagerModel.PageSize;

                    if (pg < 1)
                    {
                        pg = 1;
                    }

                    int resCount = students.Count();

                    var pager = new PaginationModel(resCount, pg, pageSize);

                    int recSkip = (pg - 1) * pageSize;

                    var data = students.Skip(recSkip).Take(pager.PageSize).ToList();

                    ViewBag.Pager = pager;

                    return View(data);
                   // return View(await _unitOfWork.Student.GetAll());
                }
                catch (Exception)
                {
                    _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                    return View();
                }

            }
            else
            {
                //Trim bookserial number of whitespces
                String SearchParam = AdminNo.Trim();

                try
                {
                    var students = await _unitOfWork.Student.GetByAdminNo(SearchParam);

                    int pageSize = PagerModel.PageSize;

                    if (pg < 1)
                    {
                        pg = 1;
                    }

                    int resCount = students.Count();

                    var pager = new PaginationModel(resCount, pg, pageSize);

                    int recSkip = (pg - 1) * pageSize;

                    var data = students.Skip(recSkip).Take(pager.PageSize).ToList();

                    ViewBag.Pager = pager;

                    return View(data);
                    //return View(await _unitOfWork.Student.GetByAdminNo(SearchParam));
                }
                catch (Exception)
                {
                    _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                    return View();
                }

            }
            
        }

        
        // GET: StudentController/Create
        public  IActionResult Create()
        {
            try
            {

                ViewData["FormID"] = new SelectList(_context.Form, "Id", "Name");
                return View();

            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));

            }
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AdminNumber,FormID")] Student student)
        {
            if (ModelState.IsValid)
            {
              
                try
                {
                    //Check if student admin number is already registered
                    var student_check = await _unitOfWork.Student
                        .GetByAdminNo(student.AdminNumber.Trim());
                   
                    if (!student_check.Any())
                    {
                        var user = await _userManager.GetUserAsync(User);
                        student.RegisteredBy = user.UserName;

                        _unitOfWork.Student.Add(student);
                        await _unitOfWork.Complete();
                        _notyf.Success("Student Registered Successfully.");
                        return RedirectToAction(nameof(Index));

                    }
                    else
                    {
                        ViewData["FormID"] = new SelectList(_context.Form, "Id", "Name", student.FormID);
                        _notyf.Error("This Student Admission Number is Already Registered.");
                        return View(student);

                    }
                }
                catch (Exception)
                {
                    _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                    return RedirectToAction(nameof(Index));

                }
            }
            else
            {
                try
                {
                    ViewData["FormID"] = new SelectList(_context.Form, "Id", "Name", student.FormID);
                    _notyf.Error("Correctlt fill the form, and Try Again!!!");
                    return View(student);

                }
                catch (Exception)
                {
                    _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                    return RedirectToAction(nameof(Index));

                }
            }

        }

        // GET: StudentController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var student = await _unitOfWork.Student.GetById(id);
                ViewData["FormID"] = new SelectList(_context.Form, "Id", "Name", student.FormID);
                return View( student);

            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
            }
            
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AdminNumber,FormID")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    //Check if AdminNumber Already registered
                    var entity = await _context.Student
                        .Where(i => i.Id != student.Id)
                        .Where(s => s.AdminNumber.Contains(student.AdminNumber))
                        .FirstOrDefaultAsync();

                    if (entity != null)
                    {
                        ViewData["FormID"] = new SelectList(_context.Form, "Id", "Name", student.FormID);
                        _notyf.Error("This Student Admission Number is Already Registered.");
                        return View(student);

                    }
                    _unitOfWork.Student.Update(student);
                    await _unitOfWork.Complete();
                    _notyf.Success("Student Updated Successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ViewData["FormID"] = new SelectList(_context.Form, "Id", "Name", student.FormID);
                    _notyf.Error("Something went wrong, kindly try again!!!.");
                    return View(student);
                }

            }
            else
            {
                ViewData["FormID"] = new SelectList(_context.Form, "Id", "Name", student.FormID);
                _notyf.Error("Something went wrong, kindly try again!!!.");
                return View(student);

            }
                
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var model = new StudentBorrowings();

                model.Student = await _unitOfWork.Student.GetById((int)id);
                    

                model.Borrowings = await _context.Borrowing
                    .Where(d => d.StudentID == id)
                    .Where(c => c.Issued == "Yes")
                    .OrderByDescending(r => r.RegisterDate)
                    .Include(b => b.CurrentBook)
                    .Include(b => b.CurrentStudent)
                    .ToListAsync();

                if (model.Student == null)
                {
                    return NotFound();
                }

                return View(model);

            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));

            }


        }


        // POST: StudentController/Delete/5
        [Authorize(Policy = "Premium")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                //Check if student is currently Issued a Book
                bool Issued_book = await _unitOfWork.Student.CheckIfIssuedBook((int)id);

                if (Issued_book)
                {
                    _notyf.Error("This student is currently Issued a Book!!!");
                    return RedirectToAction(nameof(Index));

                }

                await _unitOfWork.Student.Delete((int)id);
                await _unitOfWork.Complete();
                _notyf.Success("Student Deleted Successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                // Get borrowing record
                var entity = await _unitOfWork.Borrowing.GetById((int)id);

                //Get the issued book
                var book = await _unitOfWork.Book.GetById(entity.BookID);

                //Set Issued column = "NO"
                entity.Issued = "NO";
                book.Issued = "NO";

                //Update Returned By and Returned date
                var user = await _userManager.GetUserAsync(User);
                

                entity.ReturnedBy = user.UserName;
                entity.ReturnedDate = DateTime.Now;

                //Update Database
                _unitOfWork.Borrowing.Update(entity);
                _unitOfWork.Book.Update(book);

                //Save database changes
                await _unitOfWork.Complete();
                _notyf.Success("Book Returned Successfully.");
                return RedirectToAction(nameof(StudentController.Details), new { id = entity.StudentID});
            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
