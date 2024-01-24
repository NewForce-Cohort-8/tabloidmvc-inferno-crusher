namespace TabloidMVC.Models.ViewModels
{
    public class CommentCreateViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
        public int UserId { get; set; }
        public Comment Comment { get; set; }
    }
}
