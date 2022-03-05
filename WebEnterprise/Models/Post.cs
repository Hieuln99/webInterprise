using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OpenDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ClosedDate { get; set; }

        public int CateId { get; set; }

        public Category Category { get; set; }

        public string UserId { get; set; }

        public CustomUser CustomUser { get; set; }
    }
}
