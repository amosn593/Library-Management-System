

namespace REPORTS.Models
{
    public class BookReport
    {
        public string Title { get; set; }
        public string Subject { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public DateTime RegisterDate { get; set; }
        public string RegisteredBy { get; set; }

        public string Issued { get; set; }
    }
}
