using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.Models.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OpenDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ClosedDate { get; set; }

        public int CatId { get; set; }

        public string CatName { get; set; }

        public string UserId { get; set; }

        public string AuthorName { get; set; }
    }
}
