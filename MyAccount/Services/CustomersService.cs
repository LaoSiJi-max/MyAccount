using System;
using System.Collections.Generic;
using System.Linq;
using MyAccount.Models;
using System.Data.Entity;
using MyAccount.Services.IServices;
using System.Web.Mvc;
using PagedList;
using System.Text;
using System.IO;

namespace MyAccount.Services
{
    public class CustomersService : ICustomersService
    {
        private MyAccountContext db = new MyAccountContext();

        public bool CreateCustomer(Customer customer)
        {
            try
            {
                customer.Balance = 0;
                customer.LastTime = DateTime.Now;

                db.Customers.Add(customer);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteCustomer(int id)
        {
            try
            {
                Customer customer = db.Customers.Find(id);
                db.Customers.Remove(customer);
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

        public bool EditCustomer(Customer customer)
        {
            try
            {
                Customer oldCustomer = db.Customers.Find(customer.Id);
                oldCustomer.Name = customer.Name;
                oldCustomer.Pwd = customer.Pwd;
                oldCustomer.Num = customer.Num;
                oldCustomer.Balance = customer.Balance;

                db.Entry(oldCustomer).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Customer GetCustomer(int? id)
        {
            return db.Customers.Find(id);
        }

        public IPagedList<Customer> GetCustomersPage(int page, int size)
        {
            return db.Customers.OrderBy(c=>c.Id).ToPagedList(page,size);
        }

        public IPagedList<Customer> GetCustomersPage(string findBy, string keyword, int page, int size)
        {
            switch (findBy)
            {
                case "id":
                    int id = Convert.ToInt32(keyword);
                    return db.Customers.Where(c => c.Id==id).OrderBy(c => c.Id).ToPagedList(page,size);

                case "num":
                    return db.Customers.Where(c => c.Num==keyword).OrderBy(c => c.Id).ToPagedList(page, size);

                case "name":
                    return db.Customers.Where(c => c.Name.IndexOf(keyword) != -1).OrderBy(c => c.Id).ToPagedList(page, size);

                default:
                    return null;

            }
        }

        public List<SelectListItem> GetFindByItems()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            string[] names = new string[] { "ID", "编号","名字" };
            string[] values = new string[] { "id", "num","name" };

            for (int i = 0; i < names.Length; i++)
            {
                list.Add(new SelectListItem { Text = names[i], Value = values[i] });
            }

            return list;
        }

        public string GetExcelHtml(string findBy, string keyword)
        {
            List<Customer> list = null;

            if (keyword.Equals(string.Empty))
            {
                list = db.Customers.OrderBy(c => c.Id).ToList();
            }
            else
            {
                switch (findBy)
                {
                    case "id":
                        int id = Convert.ToInt32(keyword);
                        list = db.Customers.Where(c => c.Id == id).OrderBy(c => c.Id).ToList();
                        break;

                    case "num":
                        list = db.Customers.Where(c => c.Num == keyword).OrderBy(c => c.Id).ToList();
                        break;

                    case "name":
                        list = db.Customers.Where(c => c.Name.IndexOf(keyword) != -1).OrderBy(c => c.Id).ToList();
                        break;

                    default:
                        break;

                }
            }

            var html = new StringBuilder();
            html.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            html.Append("<tr>");
            var title = new List<string> { "ID", "编号", "姓名", "余额", "最后操作时间" };
            foreach (var v in title)
            {
                html.AppendFormat("<td text-align:center; font-weight:bold;'>{0}</td>", v);
            }
            html.Append("</tr>");
            foreach (Customer c in list)
            {
                html.Append("<tr>");
                html.AppendFormat("<td>{0}</td>", c.Id);
                html.AppendFormat("<td>{0}</td>", c.Num);
                html.AppendFormat("<td>{0}</td>", c.Name);
                html.AppendFormat("<td>{0}</td>", c.Balance);
                html.AppendFormat("<td>{0}</td>", c.LastTime);
                html.Append("</tr>");
            }
            html.Append("</table>");

            return html.ToString();
        }
    }
}