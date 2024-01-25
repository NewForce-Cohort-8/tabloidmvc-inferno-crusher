using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {

        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public CommentController(IPostRepository postRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository, IUserProfileRepository userProfileRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _userProfileRepository = userProfileRepository;
        }
        // GET: CommentController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {
            int userId = GetCurrentUserProfileId();
            var post = _postRepository.GetPublishedPostById(id);
            var comments = _commentRepository.GetAllPostComments(id);

            if (post == null)
            {
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }

            var vm = new PostDetailsViewModel();
            vm.Post = post;
            vm.Comments = comments;
            vm.UserId = userId;

            return View(vm);
        }

        // GET: CommentController/Create
        public ActionResult Create(int id)
        {
            int userId = GetCurrentUserProfileId();
            var post = _postRepository.GetPublishedPostById(id);

            var comment = new Comment();
            var vm = new CommentCreateViewModel();
            vm.Post = post;
            vm.Comment = comment;
            vm.Comment.PostId = vm.Post.Id;
            vm.Comment.UserProfileId = userId;

            return View(vm);
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentCreateViewModel vm)
        {
            try
            {
                vm.Comment.CreateDateTime = DateAndTime.Now;
                vm.Comment.UserProfile = _userProfileRepository.GetUserById(vm.Comment.UserProfileId);
                var comments = _commentRepository.GetAllPostComments(vm.Comment.PostId);
                _commentRepository.Add(vm.Comment);

                return RedirectToAction("Details", new { id = vm.Comment.PostId });
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            var comment = _commentRepository.GetCommentById(id);
            return View(comment);
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comment comment)
        {
            try
            {
                _commentRepository.Edit(comment);
                return RedirectToAction("Details", "Post", new { id = comment.PostId });
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            var comment = _commentRepository.GetCommentById(id);

            return View(comment);
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Comment comment)
        {
            try
            {
                Comment commentToDelete = _commentRepository.GetCommentById(id);
                int postId = commentToDelete.PostId;
                _commentRepository.Delete(id);

                return RedirectToAction("Details", new { id = postId });
            }
            catch
            {
                return View(comment);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
