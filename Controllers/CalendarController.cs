using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sabio.Web.Controllers
{
    public class CalendarController : BaseController
    {
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }
    }
}