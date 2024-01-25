using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabloidMVC.Models;
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
        public ActionResult View(int id)
        {
            
           var userProfile = _userProfileRepository.GetUserById(id);
            return View(userProfile);
        }
    }
}
