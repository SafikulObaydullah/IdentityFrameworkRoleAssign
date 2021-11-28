using CrudWithImage1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrudWithImage1.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        // GET: Roles
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string rName)
        {
            string msg = "";
         IdentityResult result=   roleManager.Create(new IdentityRole { Name = rName });
            if(result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else if(result.Errors.Count()>0)
            {
                foreach(var er in result.Errors)
                {
                    msg = er;   
                }
                ViewBag.erro = msg;
            }
            return View();

        }
        [HttpGet]
        public ActionResult Edit(string Id)
        {
            return View(db.Roles.Find(Id));
        }
        [HttpPost]
        public ActionResult Edit(IdentityRole identityRole)
        {
            try
            {
                string msg = "";
                IdentityRole updateRole = db.Roles.Find(identityRole.Id);
                updateRole.Name = identityRole.Name;
                IdentityResult result = roleManager.Update(updateRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else if (result.Errors.Count() > 0)
                {
                    foreach (var er in result.Errors)
                    {
                        msg = er;
                    }
                    ViewBag.erro = msg;
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(identityRole);
        }
        public ActionResult Delete(string Id)
        {
            string msg = "";
            var roles = db.Roles.Find(Id);
            IdentityResult result =  roleManager.Delete(roles);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            //else if (result.Errors.Count() > 0)
            //{
            //    foreach (var er in result.Errors)
            //    {
            //        msg = er;
            //    }
            //    ViewBag.erro = msg;
            //}
            return View();
        }

    }
}