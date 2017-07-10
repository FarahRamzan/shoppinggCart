using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopChart.Models;
using System.Data;
using System.Data.Entity;


namespace ShopChart.Views.UserAccount
{
   
        public class AdminController : Controller
        {
            //

            ShopCartDbEntities18 db = new ShopCartDbEntities18();
            public ActionResult Index()
            {
                return View();
            }
           
        }

    }
