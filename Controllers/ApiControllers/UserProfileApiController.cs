using Sabio.Web.Domain;
using Sabio.Web.Models;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
    //skinny controllers chubby services
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UserProfileApiController : ApiController
    {
        private IMediaService _mediaService { get; set; }
        public UserProfileApiController(IMediaService mediaService)
        {
            this._mediaService = mediaService;
        }
        [Route("current"), HttpGet]
        [Authorize]
        public HttpResponseMessage Current()
        {
            string currentUser = UserService.GetCurrentUserId();

            User Data = UserProfileServices.GetFullUserById(currentUser);            

            Data.IsConnected = false; //    you cannot connect with yourself

            ItemResponse<User> response = new ItemResponse<User>();

            response.Item = Data;

            return Request.CreateResponse(response);
        }


        [Route(), HttpGet]
        public HttpResponseMessage Get(int pageNumber = 1, int pageSize = 5, string queryString = null) 
        {
            List<User> userData = UserProfileServices.Get(pageNumber, pageSize, queryString);

            PagedItemsResponse<User> response = new PagedItemsResponse<User>();

            response.Items = userData;

            response.PageNumber = pageNumber;

            response.PageSize = pageSize;

            response.TotalItems = UserProfileServices.GetPagedUsers(); // change to count all users

            response.QueryString = queryString;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route(), HttpPost]
        public HttpResponseMessage Post(UsersProfilesRequest model)
        {

            if (model == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Fill out all fields");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            int userId = UserProfileServices.Post(model);

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = userId;


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("currentUser"), HttpPut] //Update profile
        public HttpResponseMessage Update(UsersProfilesRequest model)
        {
            if (model == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Request payload was null");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            string id = UserService.GetCurrentUserId();
            UserProfileServices.Put(id, model);
            ItemResponse<bool> response = new ItemResponse<bool>();
            response.Item = true;
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("{userId:Guid}"), HttpGet]
        public HttpResponseMessage Get(string userId)
        {
            User Data = UserProfileServices.GetFullUserById(userId);
            

            string currentUser = UserService.GetCurrentUserId();

            ConnectionsService connectionsService = new ConnectionsService();

            Data.Connection = connectionsService.IsConnected(currentUser, userId);
            
            ItemResponse<User> response = new ItemResponse<User>();

            response.Item = Data;

            return Request.CreateResponse(response);
        }


        // Media Update
        [Route("media"), HttpPut] //Update Media
        public HttpResponseMessage Update( UsersProfilesMediaUpdateRequest model)
        {

            if (model == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to process request");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            model.UserId = UserService.GetCurrentUserId();
            this._mediaService.Put(model);
            ItemResponse<bool> response = new ItemResponse<bool>();
            response.Item = true;
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("theme"), HttpPut] //Update Theme Media
        public HttpResponseMessage ThemeUpdate(UsersProfilesMediaUpdateRequest model)
        {

            if (model == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to process request");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            model.UserId = UserService.GetCurrentUserId();
            this._mediaService.PutTheme(model);
            ItemResponse<bool> response = new ItemResponse<bool>();
            response.Item = true;
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("admin"), HttpGet]
        public HttpResponseMessage GetJoint()
        {
            List<User> userData = UserProfileServices.GetJoint();

            ItemsResponse<User> response = new ItemsResponse<User>();

            response.Items = userData;


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("admin/{userId:Guid}"), HttpPut] //Update profile
        public HttpResponseMessage AdminUpdate(Guid userId, UserAdminUpdateRequest model)
        {
            if (model == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Request payload was null");
            }

            model.UserId = userId;

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            UserProfileServices.PutAdmin(userId, model);
            ItemResponse<bool> response = new ItemResponse<bool>();
            response.Item = true;
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("admin/search/{searchstring}"), HttpGet]
        public HttpResponseMessage SearchUsers(string searchString)
        {
            List<User> searchResult = UserProfileServices.SearchUsers(searchString);

            ItemsResponse<User> response = new ItemsResponse<User>();

            response.Items = searchResult;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("myprofile"), HttpGet]
        public HttpResponseMessage GetById()
        {
            string id = UserService.GetCurrentUserId();

            User userData = UserProfileServices.GetFullUserById(id);

            ItemResponse<User> response = new ItemResponse<User>();

            response.Item = userData;


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("cardtemplate"), HttpPut]
        public HttpResponseMessage UpdateTemplate(User model)
        {
            string id = UserService.GetCurrentUserId();

            model.UserId = id;

            UserProfileServices.UpdateCard(model);

            return Request.CreateResponse(HttpStatusCode.OK, "updated");
        }

    };

}