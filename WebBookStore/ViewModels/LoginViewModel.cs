using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBookStore.ViewModels
{
    public class LoginViewModel : Controller
    {
        // GET: LoginViewModel
        public ActionResult Index()
        {
            return View();
        }
    }
}