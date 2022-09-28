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
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LibraryDbContext _context;
        private readonly INotyfService _notyf;
        private readonly UserManager<USERVIEWUser> _userManager;

        public BookController(IUnitOfWork unitOfWork,
            INotyfService notyf,
            LibraryDbContext context,
            UserManager<USERVIEWUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _notyf = notyf;
            _userManager = userManager;
        }
        // GET: BookController
        [HttpGet]
        public async Task<IActionResult> Index(string? BookSerialNo, int pg = 1)
        {
            

            if (String.IsNullOrEmpty(BookSerialNo))
            {
                try
                {
                    var books = await _unitOfWork.Book.GetAll();

                    int pageSize = PagerModel.PageSize;

                    if (pg < 1)
                    {
                        pg = 1;
                    }

                    int resCount = books.Count();

                    var pager = new PaginationModel(resCount, pg, pageSize);

                    int recSkip = (pg - 1) * pageSize;

                    var data = books.Skip(recSkip).Take(pager.PageSize).ToList();

                    ViewBag.Pager = pager;

                    return View(data);
                    //return View(await _unitOfWork.Book.GetAll());
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
                String SearchParam = BookSerialNo.Trim();

                try
                {
                    var books = await _unitOfWork.Book.GetBySerialNo(SearchParam);

                    int pageSize = PagerModel.PageSize;

                    if (pg < 1)
                    {
                        pg = 1;
                    }

                    int resCount = books.Count();

                    var pager = new PaginationModel(resCount, pg, pageSize);

                    int recSkip = (pg - 1) * pageSize;

                    var data = books.Skip(recSkip).Take(pager.PageSize).ToList();

                    ViewBag.Pager = pager;

                    return View(data);
                    // return View(await _unitOfWork.Book.GetBySerialNo(SearchParam));
                }
                catch (Exception)
                {
                    _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                    return View();
                }

            }
        }

        // GET: BookController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {

                
                return View(await _unitOfWork.Book.GetById((int)id));
            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));

            }
            
        }

        // GET: BookController/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["FormID"] = new SelectList(_context.Form, "Id", "Name");
                ViewData["BookSourceID"] = new SelectList(_context.BookSource, "Id", "Source");
                return View();
            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
            }
            
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,RegisterDate,Subject,SerialNumber,FormID," +
            " BookSourceID")] Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Checking if book already registered
                    var bk = await _unitOfWork.Book.GetSingleBookBySerialNo(book.SerialNumber.Trim());

                    if (bk != null)
                    {
                        ViewData["FormID"] = new SelectList(_context.Form, "Id", "Name", book.FormID);
                        ViewData["BookSourceID"] = new SelectList(_context.BookSource, "Id", "Source");
                        _notyf.Error("This Book Serial Number is Already Registered.");
                        return View(book);

                    }
                    var user = await _userManager.GetUserAsync(User);
                    book.RegisteredBy = user.UserName;
                    book.RegisterDate = DateTime.Now;
                    _unitOfWork.Book.Add(book);
                    await _unitOfWork.Complete();
                    _notyf.Success("Book Registered Successfully.");
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception)
                {
                    _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                    return RedirectToAction(nameof(Index));
                }

            }
            else
            {
                _notyf.Error("Correct below error and Try Again!!!");
                return View(book);


            }
            
        }

        // GET: BookController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var book = await _unitOfWork.Book.GetById((int)id);
                if (book == null)
                {
                    return NotFound();
                }
                ViewData["FormID"] = new SelectList(_context.Form, "Id", "Name", book.FormID);
                ViewData["BookSourceID"] = new SelectList(_context.BookSource, "Id", "Source");
                return View(book);

            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));

            }
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,RegisterDate,Subject," +
            "SerialNumber,FormID, BookSourceID")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Check if AdminNumber Already registered
                    var entity = await _context.Book
                        .Where(i => i.Id != book.Id)
                        .Where(s => s.SerialNumber.Contains(book.SerialNumber))
                        .FirstOrDefaultAsync();

                    if (entity != null)
                    {
                        ViewData["FormID"] = new SelectList(_context.Form, "Id", "Name", book.FormID);
                        ViewData["BookSourceID"] = new SelectList(_context.BookSource, "Id", "Source");
                        _notyf.Error("This Book Serial Number is Already Registered.");
                        return View(book);

                    }

                    _unitOfWork.Book.Update(book);
                    await _unitOfWork.Complete();
                    _notyf.Success("Book Updated Successfully.");
                    return RedirectToAction(nameof(Index));
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
                    ViewData["FormID"] = new SelectList(_context.Form, "Id", "Name", book.FormID);
                    ViewData["BookSourceID"] = new SelectList(_context.BookSource, "Id", "Source");
                    _notyf.Error("Something went wrong, kindly try again!!!.");
                    return View(book);

                }
                catch (Exception)
                {
                    throw new Exception("Something went wrong, try again later!!!");
                }
            }
        }



        // POST: BookController/Delete/5
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
                //Check if the book is currently issued
                var book = await _unitOfWork.Book.GetById((int)id);
                if(book.Issued == "YES")
                {
                    _notyf.Error("This Book is currently Issued to a student!!!");
                    return RedirectToAction(nameof(Index));

                }
                await _unitOfWork.Book.Delete((int)id);
                await _unitOfWork.Complete();
                _notyf.Success("Book Deleted Successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
