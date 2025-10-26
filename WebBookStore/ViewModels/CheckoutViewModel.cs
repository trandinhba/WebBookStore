using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBookStore.ViewModels
{
    public class CheckoutViewModel : Controller
    {
        // GET: CheckoutViewModel
        public ActionResult Index()
        {
            return View();
        }
    }
}