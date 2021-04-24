﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Database;

namespace TaskAdmin.Controllers
{


    public class UserController : Controller
    {


        private readonly dbContext db;

        public UserController(dbContext context)
        {
            db = context;
        }


        public IActionResult Index()
        {
            return View();
        }


        public JsonResult Login(string name, string pwd)
        {
            var Data = new { status = true };


            if (name == "admin" && pwd == "123456")
            {
                HttpContext.Session.SetString("userid", "admin");
            }
            else
            {
                Data = new { status = false };
            }


            return Json(Data);
        }

    }
}
