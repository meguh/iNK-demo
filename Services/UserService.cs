using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Sabio.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Sabio.Web.Exceptions;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Sabio.Web.Domain;
using System.Data;
using Sabio.Data;
using System.Data.SqlClient;
using Sabio.Web.Models.Requests;

namespace Sabio.Web.Services
{
    public class UserService : BaseService
    {

        private static ApplicationUserManager GetUserManager()
        {
            return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        public static IdentityUser CreateUser(string email, string password)
        {
            ApplicationUserManager userManager = GetUserManager();

            ApplicationUser newUser = new ApplicationUser { UserName = email, Email = email, LockoutEnabled = false };
            IdentityResult result = null;
            try
            {
                result = userManager.Create(newUser, password);

            }
            catch
            {
                throw;
            }

            if (result.Succeeded)
            {
                return newUser;
            }
            else
            {
                throw new IdentityResultException(result);
            }
        }


        public static bool Signin(string emailaddress, string password)
        {
            bool result = false;

            ApplicationUserManager userManager = GetUserManager();
            IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            ApplicationUser user = userManager.Find(emailaddress, password);
            if (user != null)
            {
                ClaimsIdentity signin = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, signin);
                result = true;

            }
            return result;
        }

        public static bool IsUser(string emailaddress)
        {
            bool result = false;

            ApplicationUserManager userManager = GetUserManager();
            IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            ApplicationUser user = userManager.FindByEmail(emailaddress);


            if (user != null)
            {

                result = true;

            }

            return result;
        }

        public static ApplicationUser GetUser(string emailaddress)
        {


            ApplicationUserManager userManager = GetUserManager();
            IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            ApplicationUser user = userManager.FindByEmail(emailaddress);

            return user;
        }


        public static ApplicationUser GetUserById(string userId)
        {

            ApplicationUserManager userManager = GetUserManager();
            IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            ApplicationUser user = userManager.FindById(userId);

            return user;
        }

        public static bool ChangePassWord(string userId, string newPassword)
        {
            bool result = false;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newPassword))
            {
                throw new Exception("You must provide a userId and a password");
            }

            ApplicationUser user = GetUserById(userId);

            if (user != null)
            {

                ApplicationUserManager userManager = GetUserManager();

                userManager.RemovePassword(userId);
                IdentityResult res = userManager.AddPassword(userId, newPassword);

                result = res.Succeeded;

            }

            return result;
        }

        public static bool AdminChangePassWord(string UserId, string NewPassword)
        {
            bool result = false;

            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(NewPassword))
            {
                throw new Exception("You must provide a userId and a password");
            }

            ApplicationUser user = GetUserById(UserId);

            if (user != null)
            {

                ApplicationUserManager userManager = GetUserManager();

                userManager.RemovePassword(UserId);
                IdentityResult res = userManager.AddPassword(UserId, NewPassword);

                result = res.Succeeded;

            }

            return result;
        }


        public static bool Logout()
        {
            bool result = false;

            IdentityUser user = GetCurrentUser();

            if (user != null)
            {
                IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                result = true;
            }

            return result;
        }


        public static IdentityUser GetCurrentUser()
        {
            if (!IsLoggedIn())
                return null;
            ApplicationUserManager userManager = GetUserManager();

            IdentityUser currentUserId = userManager.FindById(GetCurrentUserId());
            return currentUserId;
        }

        public static string GetCurrentUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }


        public static bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(GetCurrentUserId());

        }

        public static List<BaseUser> GetUsers()
        {
            List<BaseUser> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.AspNetUsers_Select"
               , inputParamMapper: null//executes before stored proc
               , map: delegate (IDataReader reader, short set) //executes after stored proc
               {
                   BaseUser p = new BaseUser();
                   int startingIndex = 0; //startingOrdinal

                   /*
                       [Id] - nvarchar
                      ,[Email]-nvarchar
                      ,[EmailConfirmed] -bit
                      ,[PasswordHash] -nvarchar
                      ,[SecurityStamp] -nvarchar
                      ,[PhoneNumber] -nvarchar
                      ,[PhoneNumberConfirmed] -bit
                      ,[TwoFactorEnabled] -bit
                      ,[LockoutEndDateUtc] -datetime
                      ,[LockoutEnabled] - bit
                      ,[AccessFailedCount] -int 
                      ,[UserName]-nvarchar
                   */
                   p.Id = reader.GetSafeString(startingIndex++);
                   p.Email = reader.GetSafeString(startingIndex++);
                   p.EmailConfirmed = reader.GetSafeBool(startingIndex++);
                   p.PasswordHash = reader.GetSafeString(startingIndex++);
                   p.SecurityStamp = reader.GetSafeString(startingIndex++);
                   p.PhoneNumber = reader.GetSafeString(startingIndex++);
                   p.PhoneNumberConfirmed = reader.GetSafeBool(startingIndex++);
                   p.TwoFactorEnabled = reader.GetSafeBool(startingIndex++);
                   p.LockoutEndDateUtc = reader.GetSafeDateTime(startingIndex++);
                   p.LockoutEnabled = reader.GetSafeBool(startingIndex++);
                   p.AccessFailedCount = reader.GetSafeInt32(startingIndex++);
                   p.UserName = reader.GetSafeString(startingIndex++);

                   if (list == null)
                   {
                       list = new List<BaseUser>();
                   }

                   list.Add(p);
               }
               );

            return list;
        }

        public static bool ExternalAuthSignIn(ApplicationUser user)
        {
            bool result = false;

            ApplicationUserManager userManager = GetUserManager();
            IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            if (user != null)
            {
                ClaimsIdentity signin = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, signin);
                result = true;

            }
            return result;
        }

       


    }
}