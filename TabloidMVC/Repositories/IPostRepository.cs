using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        List<Post> GetAllPublishedPosts();
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        void DeletePost(int id);
        //added GetCurrentUserPosts here after adding it to PostRepository.cs
        List<Post> GetCurrentUserPosts(int currentUserId);
        void Edit(Post post);
    }
}