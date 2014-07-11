using MgJava2014.Models.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MgJava2014.Controllers
{
    public class HomeController : Controller
    {
        private MainDBEntities _dbConnection;
        private MainDBEntities dbConnection { get { return _dbConnection ?? (_dbConnection = new MainDBEntities()); } }
        
        //
        // GET: /Home/

        public ActionResult Index()
        {
            IEnumerable<User> users = dbConnection.Users.ToList();
            return View(users);
        }

    }
}
