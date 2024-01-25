using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        //void Add(Tag tag);
        List<Tag> GetAllPublishedTags();
        //Post GetPublishedTagById(int id);
        //Post GetUserPostById(int id, int userProfileId);
        //void DeletePost(int id);
        //added GetCurrentUserPosts here after adding it to PostRepository.cs
    }
}
