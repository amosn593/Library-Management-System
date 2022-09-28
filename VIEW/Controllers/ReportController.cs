using DOMAIN.IConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REPORTS.Models;
using System.Text;

namespace VIEW.Controllers
{
    [Authorize(Policy = "Basic")]
    [Authorize(Policy = "Medium")]
    public class ReportController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        
        public ReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
            
        }

        //Books Report
        public async Task<IActionResult> BookReport()
        {
                        
            try
            {
                //Getting all books
                var report = await _unitOfWork.Book.GetAll();

                //Creating a Instance of books model of type list
                List<BookReport> books = new();

                //Mapping report to books
                foreach (var b in report)
                {
                    books.Add(new()

                    {
                        Title = b.Title,
                        Subject = b.Subject,
                        SerialNumber = b.SerialNumber,
                        Name = b.Form.Name,
                        Source = b.BookSource.Source,
                        RegisterDate = b.RegisterDate,                    
                        RegisteredBy = b.RegisteredBy,
                        Issued = b.Issued

                    }
                    );
                };

                               
                //Getting current datetime to include in the filename

                DateTime time = DateTime.Now;

                string gen_time = time.ToString();

               

                // Creating a CSV file
                var builder = new StringBuilder();
                builder.AppendLine("No., Book_Title, Book_Subject, Book_SerialNO., Book_Class," +
                    "Book_Source, Registered_By, Registered_Date, Issued");
                int counter = 1;
                foreach (var bk in books)
                {
                    builder.AppendLine($"{counter},{bk.Title},{bk.Subject},{bk.SerialNumber}," +
                        $"{bk.Name},{bk.Source}, {bk.RegisteredBy}, {bk.RegisterDate},{bk.Issued}");
                    counter++;
                }

               
                return File(Encoding.UTF8.GetBytes(builder.ToString()),
                    "text/csv", $"Registered_Books_{gen_time}.csv");

            }
            catch (Exception)
            {
                
                return RedirectToAction(nameof(BookController.Index));
            }
        }

        public async Task<IActionResult> StudentReport()
        {

            try
            {
                //Getting all Students
                var report = await _unitOfWork.Student.GetAll();

                //Creating a Instance of books model of type list
                List<StudentReport> students = new();

                //Mapping report to books
                foreach (var b in report)
                {
                    students.Add(new()

                    {
                        Name = b.Name,
                        AdminNumber = b.AdminNumber,
                        Form = b.Form.Name,
                        Active = b.Active
                        

                    }
                    );
                };

               
                //Getting current datetime to include in the filename

                DateTime time = DateTime.Now;

                string gen_time = time.ToString();



                // Creating a CSV file
                var builder = new StringBuilder();
                builder.AppendLine("No., Student_Name, Studen_Admin_NO, Student_Class, Active");
                int counter = 1;

                foreach (var st in students)
                {
                    builder.AppendLine($"{counter},{st.Name},{st.AdminNumber},{st.Form}," +
                        $"{st.Active}");
                    counter++;
                }


                return File(Encoding.UTF8.GetBytes(builder.ToString()),
                    "text/csv", $"Registered_Students_{gen_time}.csv");

            }
            catch (Exception)
            {

                return RedirectToAction(nameof(BookController.Index));
            }
        }

        public async Task<IActionResult> BorrowingReport()
        {

            try
            {
                //Getting all books
                var report = await _unitOfWork.Borrowing.GetAll();

                //Creating a Instance of books model of type list
                List<BorrowingReport> borrowings = new();

                //Mapping report to books
                foreach (var b in report)
                {
                    borrowings.Add(new()

                    {
                        Book_Title = b.CurrentBook.Title,
                        Book_Subject = b.CurrentBook.Subject,
                        Book_SerialNumber = b.CurrentBook.SerialNumber,
                        Book_Class = b.CurrentBook.Form.Name,
                        Book_Source = b.CurrentBook.BookSource.Source,
                        Book_IssueDate = b.RegisterDate,
                        Book_DueDate = b.ReturnDate,
                        Book_IssuedBy = b.IssuedBy,
                        Book_Issued = b.Issued,
                        Student_Name = b.CurrentStudent.Name,
                        Student_AdminNumber = b.CurrentStudent.AdminNumber,
                        Student_Active = b.CurrentStudent.Active

                    }
                    );
                };


                //Getting current datetime to include in the filename

                DateTime time = DateTime.Now;

                string gen_time = time.ToString();



                // Creating a CSV file
                var builder = new StringBuilder();
                builder.AppendLine("No., Book_Title, Book_Subject,  Book_Class," +
                    "Book_Source, Book_IssuedBy, Book_IssueDate, Book_DueDate, Book_Issued ," +
                    " Book_SerialNumber , Student_Name , Student_AdminNo, Student_Active");
                int counter = 1;

                foreach (var bk in borrowings)
                {
                    builder.AppendLine($"{counter},{bk.Book_Title},{bk.Book_Subject},{bk.Book_Class}," +
                        $"{bk.Book_Source}, {bk.Book_IssuedBy}, {bk.Book_IssueDate},{bk.Book_DueDate}," +
                        $"{bk.Book_Issued},{bk.Book_SerialNumber},{bk.Student_Name}," +
                        $"{bk.Student_AdminNumber},{bk.Student_Active},");
                    counter++;
                }


                return File(Encoding.UTF8.GetBytes(builder.ToString()),
                    "text/csv", $"Issued_Books_At_{gen_time}.csv");

            }
            catch (Exception)
            {

                return RedirectToAction(nameof(BookController.Index));
            }
        }

        public async Task<IActionResult> IssuanceHistoryReport()
        {

            try
            {
                //Getting all books
                var report = await _unitOfWork.Borrowing.GetIssuanceHistory();

                //Creating a Instance of books model of type list
                List<IssuanceHistory> history = new();

                //Mapping report to books
                foreach (var b in report)
                {
                    history.Add(new()

                    {
                        Book_Title = b.CurrentBook.Title,
                        Book_Subject = b.CurrentBook.Subject,
                        Book_SerialNumber = b.CurrentBook.SerialNumber,
                        Book_Class = b.CurrentBook.Form.Name,
                        Book_Source = b.CurrentBook.BookSource.Source,
                        Book_IssueDate = b.RegisterDate,
                        Book_DueDate = b.ReturnDate,
                        Book_IssuedBy = b.IssuedBy,
                        Book_ReturnDate = b.ReturnedDate,
                        Book_ReturnedBy = b.ReturnedBy,
                        Book_Issued = b.Issued,
                        Student_Name = b.CurrentStudent.Name,
                        Student_AdminNumber = b.CurrentStudent.AdminNumber,
                        Student_Active = b.CurrentStudent.Active

                    }
                    );
                };


                //Getting current datetime to include in the filename

                DateTime time = DateTime.Now;

                string gen_time = time.ToString();



                // Creating a CSV file
                var builder = new StringBuilder();
                builder.AppendLine("No., Book_Title, Book_Subject,  Book_Class," +
                    "Book_Source, Book_IssuedBy, Book_IssueDate, Book_DueDate, Book_ReturnedDate," +
                    " Book_ReturnedBy ,Book_Issued ," +
                    " Book_SerialNumber , Student_Name , Student_AdminNo, Student_Active");
                int counter = 1;

                foreach (var bk in history)
                {
                    builder.AppendLine($"{counter},{bk.Book_Title},{bk.Book_Subject},{bk.Book_Class}," +
                        $"{bk.Book_Source}, {bk.Book_IssuedBy}, {bk.Book_IssueDate},{bk.Book_DueDate}" +
                        $",{bk.Book_ReturnDate}, {bk.Book_ReturnedBy}," +
                        $"{bk.Book_Issued},{bk.Book_SerialNumber},{bk.Student_Name}," +
                        $"{bk.Student_AdminNumber},{bk.Student_Active},");
                    counter++;
                }


                return File(Encoding.UTF8.GetBytes(builder.ToString()),
                    "text/csv", $"Books_IssuanceHistory_At_{gen_time}.csv");

            }
            catch (Exception)
            {

                return RedirectToAction(nameof(BookController.Index));
            }
        }

        public async Task<IActionResult> OverDueReport()
        {

            try
            {
                //Getting all books
                var report = await _unitOfWork.Borrowing.GetOverDueIssuance();

                //Creating a Instance of books model of type list
                List<OverDueIssuanceReport> overdue = new();

                //Mapping report to books
                foreach (var b in report)
                {
                    overdue.Add(new()

                    {
                        Book_Title = b.CurrentBook.Title,
                        Book_Subject = b.CurrentBook.Subject,
                        Book_SerialNumber = b.CurrentBook.SerialNumber,
                        Book_Class = b.CurrentBook.Form.Name,
                        Book_Source = b.CurrentBook.BookSource.Source,
                        Book_IssueDate = b.RegisterDate,
                        Book_DueDate = b.ReturnDate,
                        Book_IssuedBy = b.IssuedBy,
                        Book_Issued = b.Issued,
                        Student_Name = b.CurrentStudent.Name,
                        Student_AdminNumber = b.CurrentStudent.AdminNumber,
                        Student_Active = b.CurrentStudent.Active

                    }
                    );
                };


                //Getting current datetime to include in the filename

                DateTime time = DateTime.Now;

                string gen_time = time.ToString();



                // Creating a CSV file
                var builder = new StringBuilder();
                builder.AppendLine("No., Book_Title, Book_Subject,  Book_Class," +
                    "Book_Source, Book_IssuedBy, Book_IssueDate, Book_DueDate, " +
                    " Book_Issued ," +
                    " Book_SerialNumber , Student_Name , Student_AdminNo, Student_Active");
                int counter = 1;

                foreach (var bk in overdue)
                {
                    builder.AppendLine($"{counter},{bk.Book_Title},{bk.Book_Subject},{bk.Book_Class}," +
                        $"{bk.Book_Source}, {bk.Book_IssuedBy}, {bk.Book_IssueDate},{bk.Book_DueDate}," +
                        $"{bk.Book_Issued},{bk.Book_SerialNumber},{bk.Student_Name}," +
                        $"{bk.Student_AdminNumber},{bk.Student_Active},");
                    counter++;
                }


                return File(Encoding.UTF8.GetBytes(builder.ToString()),
                    "text/csv", $"Books_OverDueIssuance_At_{gen_time}.csv");

            }
            catch (Exception)
            {

                return RedirectToAction(nameof(BookController.Index));
            }
        }

    }
}
