using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using MyAccount.Models;
using MyAccount.Utils.Authorize;
using MyAccount.Utils.Security;
using MyAccount.Services.IServices;
using MyAccount.Services;
using MyAccount.Utils.Enums;
using MyAccount.Utils;
using System.Linq;
using System;
using PagedList;

namespace MyAccount.Controllers
{
    /// <summary>
    /// 主人 控制器
    /// </summary>
    [OwnerAuthorize(OwnerLevel.系统管理员)]
    public class OwnersController : Controller
    {
        private IOwnersService ownersService = new OwnersService(new MD5Encrypt());

        /// <summary>
        /// 主人信息列表页面
        /// </summary>
        /// <returns></returns>
        // GET: Owners
        public ActionResult Index()
        {
            if (OwnersService.IsLogined)
            {
                ViewBag.userName = "当前登录主人:" + OwnersService.GetLoginOwner().Name;
            }
            else
            {
                ViewBag.userName = "当前未登录任何主人";
            }

            return View();
        }

        public ActionResult List(string findBy, string keyword="", int page=1)
        {
            int size = 10;
            IPagedList list = null;

            if (keyword.Equals(string.Empty))
            {
                list = ownersService.GetOwnersPage(OwnersService.GetLoginOwner().Level, page, size);
            }
            else
            {
                list = ownersService.GetOwnersPage(OwnersService.GetLoginOwner().Level, findBy, keyword, page, size);
            }

            ViewBag.findByItems = ownersService.GetFindByItems();
            ViewBag.findBy = findBy;
            ViewBag.keyword = keyword;

            return View(list);
        }

        /// <summary>
        /// 主人登录页面
        /// </summary>
        /// <returns></returns>
        // Get: Owners/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            //检查是否已经登录过
            if (OwnersService.IsLogined)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }

        /// <summary>
        /// 主人登录页面表单提交
        /// </summary>
        /// <param name="name">主人名字</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        // Post: Owners/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(string name,string pwd)
        {
            //检测返回的登录状态
            switch (ownersService.LoginHash(name, pwd))
            {
                //登陆成功
                case 1:
                    return RedirectToAction("Index","Home");

                //用户名或密码有误
                case 0:
                    ViewBag.stateInfo = "登录失败，用户名或者密码有误";
                    return View();

                //用户被停用
                case -1:
                    ViewBag.stateInfo = "此用户已经被停用";
                    return View();

                //其他
                default:
                    return RedirectToAction("Login");
            }
        }

        /// <summary>
        /// 主人退出登录
        /// </summary>
        /// <returns></returns>
        // Get: Owners/Logout
        [AllowAnonymous]
        public ActionResult Logout()
        {
            //清空已经登录的信息
            ownersService.Logout();
            return RedirectToAction("Login");
        }
        
        /// <summary>
        /// 更改主人状态 0:停用，1:正常
        /// </summary>
        /// <param name="id">主人id</param>
        /// <returns></returns>
        public ActionResult StateChange(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!ownersService.StateChange(id))
            {
                return HttpNotFound();
            }

            return RedirectToAction("List");
        }

        public ActionResult PwdReset(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = ownersService.GetOwner(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PwdReset([Bind(Include = "Id,Pwd,Pwd2")] Owner owner)
        {
            ownersService.PwdReset(owner.Id,owner.Pwd,owner.Pwd2);
            return RedirectToAction("List");
        }

        /// <summary>
        /// 主人信息细节页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Owners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = ownersService.GetOwner(id);
            if (owner == null)
            {
                return HttpNotFound();
            }

            ViewBag.lb_state = owner.State;
            ViewBag.lb_level = owner.Level;

            return View(owner);
        }

        /// <summary>
        /// 创建新主人页面
        /// </summary>
        /// <returns></returns>
        // GET: Owners/Create
        public ActionResult Create()
        {
            ViewBag.levelList = ownersService.GetLevelItems();

            return View();
        }

        /// <summary>
        /// 创建新主人页面 提交
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        // POST: Owners/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Pwd,Pwd2,Level")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                ownersService.CreateOwner(owner);
            }

            return RedirectToAction("List");
        }

        /// <summary>
        /// 主人信息修改页面
        /// </summary>
        /// <param name="id">要修改的主人id</param>
        /// <returns></returns>
        // GET: Owners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = ownersService.GetOwner(id);
            if (owner == null)
            {
                return HttpNotFound();
            }

            ViewBag.levelList = ownersService.GetLevelItems();

            return View(owner);
        }

        /// <summary>
        /// 主人信息修改页面 提交
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        // POST: Owners/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Pwd,Level")] Owner owner)
        {
            ownersService.EditOwner(owner);
            
            return RedirectToAction("List");
        }

        /// <summary>
        /// 主人信息删除页面
        /// </summary>
        /// <param name="id">要删除的主人id</param>
        /// <returns></returns>
        // GET: Owners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = ownersService.GetOwner(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        /// <summary>
        /// 主人信息删除页面 提交
        /// </summary>
        /// <param name="id">要删除的主人id</param>
        /// <returns></returns>
        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ownersService.DeleteOwner(id);
            return RedirectToAction("List");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ownersService.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
