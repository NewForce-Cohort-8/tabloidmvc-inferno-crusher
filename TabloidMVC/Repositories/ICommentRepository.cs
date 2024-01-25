using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        void Add(Comment comment);
        void Edit(Comment comment);
        void Delete(int id);
        List<Comment> GetAllPostComments(int id);
        Comment GetCommentById(int commentId);

    }
}
