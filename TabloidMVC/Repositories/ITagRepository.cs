using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        void Add(Tag tag);
        List<Tag> GetAllPublishedTags();
        //Post GetPublishedTagById(int id);
        Tag GetUserTagById(int id);
        void DeleteTag(int id);
        //added GetCurrentUserPosts here after adding it to PostRepository.cs
    }
}
