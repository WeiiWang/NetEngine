﻿using Cms.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cms.Controllers
{
    [IsLogin]
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


        [IsLogin(IsSkip = true)]
        public IActionResult Login()
        {
            return View();
        }


        [IsLogin(IsSkip = true)]
        public JsonResult Login_Run(string name, string pwd)
        {
            var Data = new { status = true };



            var userlist = db.TUser.ToList();
            var user = db.TUser.Where(t => t.Name == name & t.PassWord == pwd & t.IsDelete == false).FirstOrDefault();

            if (user != null)
            {
                HttpContext.Session.SetString("userid", user.Id.ToString());
                HttpContext.Session.SetString("nickname", user.NickName);
            }
            else
            {
                Data = new { status = false };
            }

            return Json(Data);
        }


        //退出系统
        public void Login_Exit()
        {

            HttpContext.Session.SetString("userid", "");
            HttpContext.Session.SetString("nickname", "");

            Response.Redirect("/User/Login/");
        }



        public IActionResult UserIndex()
        {
            return View();
        }



        [HttpGet]
        public JsonResult GetUserList()
        {

            IList<TUser> list = db.TUser.Where(t => t.IsDelete == false).ToList();

            return Json(new { data = list });
        }



        public IActionResult UserEdit(Guid id)
        {

            if (id == default)
            {
                return View(new TUser());
            }
            else
            {

                var UserSys = db.TUser.Where(t => t.Id == id).FirstOrDefault();
                return View(UserSys);
            }

        }


        public bool UserSave(TUser user)
        {


            if (user.Id == default)
            {
                //执行添加
                user.Id = Guid.NewGuid();
                user.IsDelete = false;
                user.CreateTime = DateTime.Now;

                user.RoleId = Guid.Parse("136A214E-7523-42F2-9760-8EED42B31BA1");

                db.TUser.Add(user);
            }
            else
            {
                //执行修改
                var dbUserSys = db.TUser.Where(t => t.Id == user.Id).FirstOrDefault();

                dbUserSys.Name = user.Name;
                dbUserSys.NickName = user.NickName;
                dbUserSys.Phone = user.Phone;
                dbUserSys.Email = user.Email;
                dbUserSys.PassWord = user.PassWord;
            }

            db.SaveChanges();


            return true;
        }


        public JsonResult UserDelete(Guid id)
        {

            var user = db.TUser.Where(t => t.Id == id).FirstOrDefault();

            user.IsDelete = true;
            user.DeleteTime = DateTime.Now;

            db.SaveChanges();

            var data = new { status = true, msg = "删除成功！" };
            return Json(data);

        }

    }

}