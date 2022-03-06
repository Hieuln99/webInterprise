namespace WebEnterprise.Models
{
    public class UserLikePost
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public int PostId { get; set; }

        public bool Status { get; set; }
    }
}
