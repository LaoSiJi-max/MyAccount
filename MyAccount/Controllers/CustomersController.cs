using System.Net;
using System.Web.Mvc;
using MyAccount.Models;
using MyAccount.Utils.Authorize;
using MyAccount.Services.IServices;
using MyAccount.Services;
using MyAccount.Utils.Enums;
using System.Collections.Generic;
using MyAccount.Utils;
using System.Linq;
using PagedList;

namespace MyAccount.Controllers
{
    [OwnerAuthorize(OwnerLevel.普通主人)]
    public class CustomersController : Controller
    {
        private ICustomersService customersService = new CustomersService();

        // GET: Customers
        public ActionResult Index(string findBy,string keyword="",int page=1)
        {
            int size = 10;
            IPagedList<Customer> list = null;

            if (keyword.Equals(string.Empty))
            {
                list = customersService.GetCustomersPage(page, size);
            }
            else
            {
                list = customersService.GetCustomersPage(findBy,keyword,page,size);
            }

            ViewBag.findByItems = customersService.GetFindByItems();
            ViewBag.findBy = findBy;
            ViewBag.keyword = keyword;

            return View(list);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = customersService.GetCustomer(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Num,Pwd,Name")] Customer customer)
        {
            customersService.CreateCustomer(customer);

            return RedirectToAction("Index");
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = customersService.GetCustomer(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Num,Pwd,Name,Balance")] Customer customer)
        {
            customersService.EditCustomer(customer);

            return RedirectToAction("Index");
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = customersService.GetCustomer(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            customersService.DeleteCustomer(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                customersService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
