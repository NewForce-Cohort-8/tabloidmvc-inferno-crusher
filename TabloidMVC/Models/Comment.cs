using System.ComponentModel.DataAnnotations;

namespace TabloidMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public int UserProfileId { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreateDateTime { get; set; }
        public UserProfile UserProfile { get; set; }

    }
}
