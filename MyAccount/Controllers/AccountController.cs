using MyAccount.Models;
using MyAccount.Services;
using MyAccount.Services.IServices;
using MyAccount.Utils.Authorize;
using MyAccount.Utils.Enums;
using PagedList;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace MyAccount.Controllers
{
    [OwnerAuthorize(OwnerLevel.普通主人)]
    public class AccountController : Controller
    {
        private IAccountService accountService = new AccountService();
        private ICustomersService customersService = new CustomersService();

        // GET: Account
        public ActionResult Index(string findBy, string keyword = "", int page = 1)
        {
            int size = 10;
            IPagedList<Customer> list = null;

            if (keyword.Equals(string.Empty))
            {
                list = customersService.GetCustomersPage(page, size);
            }
            else
            {
                list = customersService.GetCustomersPage(findBy, keyword, page, size);
            }

            ViewBag.findByItems = customersService.GetFindByItems();
            ViewBag.findBy = findBy;
            ViewBag.keyword = keyword;

            return View(list);
        }

        // GET: Account/BalanceChange
        public ActionResult BalanceChange(int? id)
        {
            ViewBag.accTypes = accountService.BindAccTypeItems(1);
            ViewBag.customer = customersService.GetCustomer(id);
            return View();
        }

        // POST: Account/BalanceChange
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BalanceChange([Bind(Include = "Id,Limit,Note")] Account_Log account_Log,string cid,string accType)
        {
            accountService.BalanceChange(account_Log,cid,accType);

            return RedirectToAction("Index");
        }

        // GET: Account/AccLogList
        public ActionResult AccLogList(int? id,int page=1)
        {
            if(id!=null)
            { 
                int size = 10;
                ViewBag.id = id;
                return View(accountService.GetAccLogPage(id,page,size));
            }

            return View();
        }

        public ActionResult LogCancel(int id)
        {
            accountService.LogCancel(id);

            return RedirectToAction("AccLogList");
        }

        public FileResult ToExcel(string findBy, string keyword = "")
        {
            byte[] bytes = Encoding.Default.GetBytes(customersService.GetExcelHtml(findBy,keyword));
            return File(bytes, "application/ms-excel", "result.xls");
        }

        #region AccType操作

        // GET: Account_Type
        public ActionResult AccTypeList(int page = 1)
        {
            int size = 10;
            return View(accountService.GetAccTypePage(page,size));
        }

        // GET: Account_Type/Details/5
        public ActionResult AccTypeDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account_Type account_Type = accountService.GetAccount_Type(id);
            if (account_Type == null)
            {
                return HttpNotFound();
            }
            return View(account_Type);
        }

        // GET: Account_Type/Create
        public ActionResult AccTypeCreate()
        {
            return View();
        }

        // POST: Account_Type/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AccTypeCreate([Bind(Include = "Id,Type,Name")] Account_Type account_Type)
        {
            if (ModelState.IsValid)
            {
                accountService.AccTypeCreate(account_Type);
            }

            return RedirectToAction("AccTypeList");
        }

        // GET: Account_Type/Edit/5
        public ActionResult AccTypeEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account_Type account_Type = accountService.GetAccount_Type(id);
            if (account_Type == null)
            {
                return HttpNotFound();
            }
            return View(account_Type);
        }

        // POST: Account_Type/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AccTypeEdit([Bind(Include = "Id,Type,Name")] Account_Type account_Type)
        {
            if (ModelState.IsValid)
            {
                accountService.AccTypeEdit(account_Type);   
            }
            return RedirectToAction("AccTypeList");
        }

        // GET: Account_Type/Delete/5
        public ActionResult AccTypeDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account_Type account_Type = accountService.GetAccount_Type(id);
            if (account_Type == null)
            {
                return HttpNotFound();
            }
            return View(account_Type);
        }

        // POST: Account_Type/Delete/5
        [HttpPost, ActionName("AccTypeDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            accountService.AccTypeDelete(id);
            return RedirectToAction("AccTypeList");
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                accountService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}