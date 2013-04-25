using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiLayerSignalRSample.Controllers {

    [Authorize]
    public partial class HomeController : Controller
    {

        public virtual ViewResult Index()
        {

            return View();
        }
    }
}