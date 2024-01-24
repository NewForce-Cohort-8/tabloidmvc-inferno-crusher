using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }
// Adds new action method named "My Posts" which renders an HTML page with only posts authored by the currently logged in user
        public IActionResult MyPosts()
        {
            //get currently logged in user's userProfileId and store it in the userId variable
            int userId = GetCurrentUserProfileId();
            //get all posts that are authored by the currently logged in user, by using the GetCurrentUserPosts method from the postRepository, and passing in the current user's userProfileId as an argument
            var posts = _postRepository.GetCurrentUserPosts(userId);
            //return those posts to the View associated with the MyPosts action method, which is Views>Post>MyPosts.cshtml
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            var comments = _commentRepository.GetAllPostComments(id);

            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }

            var vm = new PostDetailsViewModel();
            vm.Post = post;
            vm.Comments = comments;

            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        // GET: PostController/Delete/3
        public ActionResult Delete(int id)
            //I changed Post post in line 75 to var post because it matched language i saw up above. I'm unsure if this is the right move
        {
            var post = _postRepository.GetPublishedPostById(id);

            return View(post);
        }

        // POST: PostController/Delete/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(post);
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        // GET: Posts/Edit/1
        public ActionResult Edit(int id)
        {
            var vm = new PostEditViewModel();
            vm.Post = _postRepository.GetPublishedPostById(id);
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        // POST: Post/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            post.Id = id;
            var vm = new PostEditViewModel();
            vm.Post = post;
            vm.CategoryOptions = _categoryRepository.GetAll();
            try
            {
                _postRepository.Edit(vm.Post);
                return RedirectToAction("Details", new { id = id });
            }
            catch(Exception ex) 
            {
                return View(vm);
            }
        }
    }
}