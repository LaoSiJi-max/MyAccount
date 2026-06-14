using System;
using System.Collections.Generic;
using System.Linq;
using MyAccount.Models;
using System.Web.Mvc;
using System.Data.Entity;
using MyAccount.Services.IServices;
using PagedList;

namespace MyAccount.Services
{
    public class AccountService : IAccountService
    {
        private MyAccountContext db = new MyAccountContext();
        public bool AccTypeCreate(Account_Type account_Type)
        {
            try
            {
                db.Account_Type.Add(account_Type);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AccTypeDelete(int id)
        {
            try
            {
                Account_Type account_Type = db.Account_Type.Find(id);
                db.Account_Type.Remove(account_Type);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AccTypeEdit(Account_Type account_Type)
        {
            try
            {
                db.Entry(account_Type).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool BalanceChange(Account_Log account_Log, string cid, string accType)
        {
            try
            {
                account_Log.Owner = db.Owners.Find(OwnersService.GetLoginOwner().Id);
                account_Log.AccountType = db.Account_Type.Find(Convert.ToInt32(accType));
                account_Log.Time = DateTime.Now;
                account_Log.IsCancel = false;

                account_Log.Customer = db.Customers.Find(Convert.ToInt32(cid));
                account_Log.Customer.LastTime = DateTime.Now;
                account_Log.Customer.Balance += account_Log.Limit * Convert.ToInt32(account_Log.AccountType.Type);

                db.Entry(account_Log).State = EntityState.Modified;
                db.Account_Log.Add(account_Log);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IPagedList<Account_Log> GetAccLogPage(int? id,int page,int size)
        {
            return db.Account_Log.Where(al => al.Customer.Id == id).OrderBy(al=>al.Time).ToPagedList(page,size);
        }

        public Account_Type GetAccount_Type(int? id)
        {
            return db.Account_Type.Find(id); 
        }

        public List<SelectListItem> BindAccTypeItems(int type)
        {
            List<Account_Type> accTypes = db.Account_Type.ToList();
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (Account_Type t in accTypes)
            {
                list.Add(new SelectListItem() { Text = t.Name, Value = t.Id.ToString() });
            }

            return list;
        }

        public IPagedList<Account_Type> GetAccTypePage(int page, int size)
        {
            return db.Account_Type.OrderBy(at => at.Id).ToPagedList(page, size);
        }

        public bool LogCancel(int id)
        {
            Account_Log alog = db.Account_Log.Where(al => al.Id == id).Include("Customer").Include("AccountType").ToList()[0];

            if (alog.IsCancel)
            {
                alog.Customer.Balance = alog.Customer.Balance + alog.Limit * Convert.ToInt32(alog.AccountType.Type);
                alog.IsCancel = false;

                db.Entry(alog).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                alog.Customer.Balance = alog.Customer.Balance - alog.Limit * Convert.ToInt32(alog.AccountType.Type);
                alog.IsCancel = true;

                db.Entry(alog).State = EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }
    }
}