

namespace REPORTS.Models
{
    public class IssuanceHistory
    {
        public string Book_Title { get; set; }

        public string Book_Subject { get; set; }


        public string Book_Class { get; set; }

        public string Book_Source { get; set; }

        public string Book_SerialNumber { get; set; }

        public string Book_Issued { get; set; }

        public string Book_IssuedBy { get; set; }

        public string Book_ReturnedBy { get; set; }


        public DateTime Book_IssueDate { get; set; }

        public DateTime Book_DueDate { get; set; }

        public DateTime? Book_ReturnDate { get; set; }


        public string Student_Name { get; set; }

        public string Student_AdminNumber { get; set; }


        public string Student_Active { get; set; }
    }
}
