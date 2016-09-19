using Sabio.Web.Domain;
using Sabio.Web.Models;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/userbase")]
    public class UsersApiController : ApiController
    {
        [Route, HttpGet]
        public HttpResponseMessage GetUsers()
        {
            List<BaseUser> MessageData = UserService.GetUsers();

            ItemsResponse<BaseUser> resp = new ItemsResponse<BaseUser>();

            resp.Items = MessageData;

            return Request.CreateResponse(HttpStatusCode.OK, resp);
        }

        [Route("tag/{slug}"), HttpGet]
        public HttpResponseMessage GetUsersByTagSlug(string slug)
        {
            List<User> userData = UserProfileServices.GetByTagSlug(slug);

            ItemsResponse<User> response = new ItemsResponse<User>();

            response.Items = userData;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("discover"), HttpGet]
        public HttpResponseMessage Get([FromUri] UsersGetRequest GetUsers)
        {

            List<User> userData = UserProfileServices.Get(GetUsers);

            ItemsResponse<User> response = new ItemsResponse<User>();

            response.Items = userData;


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }

}
