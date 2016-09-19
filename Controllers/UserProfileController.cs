using Sabio.Web.Models;
using Sabio.Web.Models.ViewModels;
using Sabio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sabio.Web.Controllers
{
    [RoutePrefix("user")]
    public class UserProfileController : BaseController
    {
        [Route("profile/{userId:Guid}")]
        public ActionResult Manage(Guid userId)
        {
            ItemViewModel<Guid> vm = new ItemViewModel<Guid>();
            vm.Item = userId;


            //update UserProfile ViewCount
            UserProfileServices.PutProfileViews(userId.ToString());


            return View("UserProfile", vm );


        }

        //new view confirm user
        [Route("verify/{tokenGuid:guid}")]
        public ActionResult Reset(Guid tokenGuid)           
        {
            ItemViewModel<Guid> vm = new ItemViewModel<Guid>();
            vm.Item = tokenGuid;

            Token verifyUser = TokenService.GetByToken(tokenGuid);

            if (verifyUser == null || verifyUser.IsUsed == true)
            {
                return View("Error", vm);
            }
            else
            {
                UserProfileServices.EmailConfirmed(verifyUser.UserId);

                ApplicationUser newUser = UserService.GetUserById(verifyUser.UserId);

                UserService.ExternalAuthSignIn(newUser);

                TokenService.MarkedIsUsed(verifyUser);

                return RedirectToAction("Index", "MyProfile");
            }
        }
        [Route("discover")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
