using MyAccount.Models;
using PagedList;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyAccount.Services.IServices
{
    interface IAccountService
    {
        List<SelectListItem> BindAccTypeItems(int type);
        bool BalanceChange(Account_Log account_Log, string cid, string accType);
        IPagedList<Account_Log> GetAccLogPage(int? id, int page, int size);
        IPagedList<Account_Type> GetAccTypePage(int page, int size);
        Account_Type GetAccount_Type(int? id);
        bool AccTypeCreate(Account_Type account_Type);
        bool AccTypeEdit(Account_Type account_Type);
        bool AccTypeDelete(int id);
        bool LogCancel(int id);
        void Dispose();
    }
}
