using MyAccount.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAccount.Services.IServices
{
    interface ICustomersService
    {
        List<SelectListItem> GetFindByItems();
        IPagedList<Customer> GetCustomersPage(int page,int size);
        IPagedList<Customer> GetCustomersPage(string findBy,string keyword,int page,int size);
        string GetExcelHtml(string findBy, string keyword);
        Customer GetCustomer(int? id);
        bool CreateCustomer(Customer customer);
        bool EditCustomer(Customer customer);
        bool DeleteCustomer(int id);
        void Dispose();
    }
}
