using AspNetCoreHero.ToastNotification.Abstractions;
using DOMAIN.IConfiguration;
using DOMAIN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using USERVIEW.Areas.Identity.Data;

namespace VIEW.Controllers
{
    [Authorize(Policy ="Basic")]
    public class BorrowingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        private readonly UserManager<USERVIEWUser> _userManager;

        public BorrowingController(IUnitOfWork unitOfWork,
            INotyfService notyf,
            UserManager<USERVIEWUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _notyf = notyf;
            _userManager = userManager;

        }
        // GET All Borrowings: BorrowingController
        public async Task<IActionResult> Index(string? BookSerialNo, int pg=1)
        {
            

            if (String.IsNullOrEmpty(BookSerialNo))
            {
                try
                {
                    var borrowings = await _unitOfWork.Borrowing.GetAll();

                    int pageSize = PagerModel.PageSize;

                    if (pg < 1)
                    {
                        pg = 1;
                    }

                    int resCount = borrowings.Count();

                    var pager = new PaginationModel(resCount, pg, pageSize);

                    int recSkip = (pg - 1) * pageSize;

                    var data = borrowings.Skip(recSkip).Take(pager.PageSize).ToList();

                    ViewBag.Pager = pager;

                    return View(data);
                    //return View(await _unitOfWork.Borrowing.GetAll());
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
                    var borrowings = await _unitOfWork.Borrowing.GetByBookSerialNo(SearchParam);

                    int pageSize = PagerModel.PageSize;

                    if (pg < 1)
                    {
                        pg = 1;
                    }

                    int resCount = borrowings.Count();

                    var pager = new PaginationModel(resCount, pg, pageSize);

                    int recSkip = (pg - 1) * pageSize;

                    var data = borrowings.Skip(recSkip).Take(pager.PageSize).ToList();

                    ViewBag.Pager = pager;

                    return View(data);
                    //return View(await _unitOfWork.Borrowing.GetByBookSerialNo(SearchParam));
                }
                catch (Exception)
                {
                    _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                    return View();
                }

            }
        }

        // GET: BorrowingController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {


                return View(await _unitOfWork.Borrowing.GetById((int)id));
            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));

            }
        }

        // GET: BorrowingController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BorrowingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {
                String BookSerialNo = collection["BookSerialNo"];
                BookSerialNo = BookSerialNo.Trim();
                String StudentAdminNo = collection["StudentAdminNo"];
                StudentAdminNo = StudentAdminNo.Trim();
                DateTime ReturnDate =  DateTime.Parse(collection["ReturnDate"]);

                
                //checking if due date greater than today
                if(DateTime.Now >= ReturnDate)
                {
                    _notyf.Error("Due Date Must Be Greater Than or Equals To Todays Date.");
                    return View();

                }

                //Check if book is available
                var book = await _unitOfWork.Book.CheckIfIssued(BookSerialNo);
                if (book == null)
                {
                    _notyf.Error("Book is not available for Issuance, Kindly check Serial No. and Try Again!!!");
                    return View();

                }

                //Check if Students is Active
                var student = await _unitOfWork.Student.CheckIfActive(StudentAdminNo);
                if (student == null)
                {
                    _notyf.Error("Student Admin No. not found, Kindly confirm and try again!!!");
                    return View();
                }

                //Create new instance of Borrowing model and add to the database
                Borrowing borrowing = new();

                DateTime RegisterDate = DateTime.Now;

                var user = await _userManager.GetUserAsync(User);

                borrowing.RegisterDate = RegisterDate;
                borrowing.ReturnDate = ReturnDate;
                borrowing.IssuedBy = user.UserName;
                borrowing.Issued = "YES";
                borrowing.CurrentBook = book;
                borrowing.CurrentStudent = student;

                _unitOfWork.Borrowing.Add(borrowing);
                

                //Update the book model to Issued to Yes
                book.Issued = "YES";
                _unitOfWork.Book.Update(book);


                //Save changes to database
                await _unitOfWork.Complete();

                _notyf.Success("Book Issued Successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return View();
            }
        }
                
             
                
        // POST: BorrowingController/Delete/5
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
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
            }
        }


        public async Task<IActionResult> IssuanceHistory(string? BookSerialNo, int pg=1)
        {
            
            if (String.IsNullOrEmpty(BookSerialNo))
            {
                try
                {
                    var borrowings = await _unitOfWork.Borrowing
                        .GetIssuanceHistory();
                    
                    
                    int pageSize = PagerModel.PageSize;

                    if(pg < 1)
                    {
                        pg = 1;
                    }

                    int resCount = borrowings.Count();

                    var pager = new PaginationModel(resCount, pg, pageSize);

                    int recSkip = (pg - 1) * pageSize;

                    var data = borrowings.Skip(recSkip).Take(pager.PageSize).ToList();

                    ViewBag.Pager = pager;

                    return View(data);

                    //return View(borrowings);
                }
                catch (Exception)
                {
                    _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                    return View();
                }
            }

            else
            {
                string searchparam = BookSerialNo.Trim();
                               

                try
                {
                    var borrowings = await _unitOfWork.Borrowing
                        .GetIssuanceHistoryByBookSerialNo(searchparam);

                    int pageSize = PagerModel.PageSize;

                    if (pg < 1)
                    {
                        pg = 1;
                    }

                    int resCount = borrowings.Count();

                    var pager = new PaginationModel(resCount, pg, pageSize);

                    int recSkip = (pg - 1) * pageSize;

                    var data = borrowings.Skip(recSkip).Take(pager.PageSize).ToList();

                    ViewBag.Pager = pager;

                    

                    return View(data);

                   // return View(borrowings);
                }
                catch (Exception)
                {
                    _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                    return View();
                }

            }
        }

        public async Task<IActionResult> OverDue(string? BookSerialNo, int pg=1)
        {
            if (String.IsNullOrEmpty(BookSerialNo))
            {
                try
                {
                    var borrowings = await _unitOfWork.Borrowing
                        .GetOverDueIssuance();

                    int pageSize = PagerModel.PageSize;

                    if (pg < 1)
                    {
                        pg = 1;
                    }

                    int resCount = borrowings.Count();

                    var pager = new PaginationModel(resCount, pg, pageSize);

                    int recSkip = (pg - 1) * pageSize;

                    var data = borrowings.Skip(recSkip).Take(pager.PageSize).ToList();

                    ViewBag.Pager = pager;

                    return View(data);

                   // return View(borrowings);
                }
                catch (Exception)
                {
                    _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                    return View();
                }

            }
            else
            {
                string searchparam = BookSerialNo.Trim();


                try
                {
                    var borrowings = await _unitOfWork.Borrowing
                        .GetOverDueIssuanceByBookSerialNo(searchparam);

                    int pageSize = PagerModel.PageSize;

                    if (pg < 1)
                    {
                        pg = 1;
                    }

                    int resCount = borrowings.Count();

                    var pager = new PaginationModel(resCount, pg, pageSize);

                    int recSkip = (pg - 1) * pageSize;

                    var data = borrowings.Skip(recSkip).Take(pager.PageSize).ToList();

                    ViewBag.Pager = pager;

                    return View(data);

                    // return View(borrowings);
                }
                catch (Exception)
                {
                    _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                    return View();
                }

            }

        }

        public async Task<IActionResult> OverDueDetails(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            try
            {
                var borrowing = await _unitOfWork.Borrowing
                    .GetOverDueIssuanceById((int)id);

                return View(borrowing);

            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));

            }



        }

        public async Task<IActionResult> IssuanceHistoryDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var borrowing = await _unitOfWork.Borrowing
                    .GetIssuanceHistoryById((int)id);

                return View(borrowing);

            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));

            }



        }
    }
}
