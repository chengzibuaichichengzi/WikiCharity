using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikiCharity.Models;

namespace WikiCharity.Controllers
{
    public class CompareController : Controller
    {

        private static List<Charity> myList = new List<Charity>();

        // GET: Compare
        public ActionResult FavList()
        {
            myList = Session["MyList"] as List<Charity>;
            return View(myList);
        }


    }
}