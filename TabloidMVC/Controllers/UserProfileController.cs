using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public ActionResult Index()
        {
            List<UserProfile> userProfiles = _userProfileRepository.GetAll();

            return View(userProfiles);
        }
        public ActionResult Details(int id)
        {
            var user = _userProfileRepository.GetUserById(id);

            

            if (user.Id == null)
            {
                int userId = user.Id;
                if (userId == null)
                {
                    return NotFound();
                }
            }

            return View(user);
        }
    }
}
