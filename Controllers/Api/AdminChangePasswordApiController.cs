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
    [RoutePrefix("api/password")]
    public class AdminChangePasswordApiController : ApiController
    {
        [Route("admin"), HttpPut]
        public HttpResponseMessage Put(AdminPasswordChangeRequest model)
        {
            if (model == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Error is null");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool result = UserService.AdminChangePassWord(model.UserId, model.NewPassword);

            if (result == true)
            {
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Password is not strong enough.");
            }

        }
    }
}
