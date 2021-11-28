using CrudWithImage1.Models;
using CrudWithImage1.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrudWithImage1.Controllers
{

    public class UserProfileController : Controller
    {
       static ApplicationDbContext db = new ApplicationDbContext();
        UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));


        // GET: UserProfile
        [Authorize]
        public ActionResult Index()
        {
            string email = User.Identity.Name;
            //var usersRoleID = db.Users.Where(u => u.Email == email)
            //                       .SelectMany(s => s.Roles)
            //                       .Select(p => p.RoleId).ToList();
            List<USersRoleVM> uSersRoleVMs = new List<USersRoleVM>();
            uSersRoleVMs = (from user in db.Users
                            select new
                            {
                                UserId = user.Id,
                                Username = user.UserName ,
                                Email = user.Email,
                                FullName = user.FullName,
                                PicturePath = user.PicturePath,
                                RolesNames = (from userRole in user.Roles
                                              join role in db.Roles on userRole.RoleId
                                              equals role.Id
                                              select role.Name).ToList()
                            }).ToList().Select(p => new USersRoleVM()
                            {
                                UserId = p.UserId,
                                Username = p.Username,
                                Email = p.Email,
                                Role = string.Join(",", p.RolesNames), 
                                FullName = p.FullName,
                                PicturePath = p.PicturePath,
                            }).OrderBy(r => r.Username).ToList();



            var usersRoleID = db.Users
                                   .SelectMany(s => s.Roles)
                                   .Select(p => p.RoleId).ToList();

            var rName = db.Roles.Where(r => usersRoleID.Contains(r.Id)).Select(s => s.Name).ToList();
            ViewBag.rName = string.Join(",", rName);
            return View(uSersRoleVMs);
        }
        
        

        [HttpGet]
       // [Authorize(Roles ="Super Admin")]
        public ActionResult Edit(string uid)
        {
            //string uid = User.Identity.GetUserId();

            var userProfile = new UserProfile();
            var user = db.Users.Find(uid);
            if (user != null)
            {
                userProfile.FullName = user.FullName;
                userProfile.DOB = user.DOB;
                userProfile.PicturePath = user.PicturePath; 
            }
            return View("Create", userProfile);
        }
        [HttpPost]
       // [Authorize(Roles = "Super Admin")]
        public ActionResult Edit(UserProfile userProfile, HttpPostedFileBase Pic)
        {
            string uid = User.Identity.GetUserId();

            var user = db.Users.Find(uid);
            if (user != null)
            {
                user.FullName = userProfile.FullName;
                user.DOB = userProfile.DOB;
                if (Pic != null)
                {
                    string fPath = Server.MapPath("~/") + "UserImages";
                    string fext = Path.GetExtension(Pic.FileName);
                    string fName = Path.GetFileNameWithoutExtension(Pic.FileName) + "_" + user.FullName + fext;
                    string fileSave = Path.Combine(fPath, fName);
                    if (!Directory.Exists(fPath))
                    {
                        Directory.CreateDirectory(fPath);
                    }
                    Pic.SaveAs(fileSave);
                    user.PicturePath = "~/UserImages/" + fName;
                }
                else
                {
                    user.PicturePath = userProfile.PicturePath;
                }
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "No user found");
                return View("Create", user);
            }
        }
        [HttpGet]
        public ActionResult AssignRole(string uId)
        {
            var usersRoleID = db.Users.Where(u => u.Id == uId)
                                   .SelectMany(s => s.Roles)
                                   .Select(p => p.RoleId).ToList();

            var rName = db.Roles.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Name,
                Selected = usersRoleID.Contains(s.Id)
            });
            ViewBag.rName = rName;
            ViewBag.Uid = uId;
            return View();

        }
        
        [HttpPost]
        public ActionResult AssignRole(string uId, string[] selectedRole)
        {
            var roleList = userManager.GetRoles(uId).ToArray();
            var res = userManager.RemoveFromRoles(uId,roleList);
            IdentityResult result = userManager.AddToRoles(uId, selectedRole);
            string msg = "";
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            if (result.Errors.Count() > 0)
            {
                foreach (var err in result.Errors)
                {
                    msg = err;
                }
            }
            return View();
        }

    }
}