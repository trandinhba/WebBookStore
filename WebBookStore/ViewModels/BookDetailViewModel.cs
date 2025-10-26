using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBookStore.ViewModels
{
    public class BookDetailViewModel : Controller
    {
        // GET: BookDetailViewModel
        public ActionResult Index()
        {
            return View();
        }
    }
}