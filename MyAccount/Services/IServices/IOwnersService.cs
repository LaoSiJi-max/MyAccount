using MyAccount.Models;
using MyAccount.Utils.Enums;
using PagedList;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyAccount.Services.IServices
{
    interface IOwnersService
    {
        Owner GetOwner(int? id);
        List<SelectListItem> GetLevelItems();
        List<SelectListItem> GetFindByItems();
        IPagedList<Owner> GetOwnersPage(OwnerLevel ownerLevel, int page, int size);
        IPagedList<Owner> GetOwnersPage(OwnerLevel ownerLevel,string findBy,string keyword, int page, int size);
        int LoginHash(string name, string pwd);
        void Logout();
        bool StateChange(int? id);
        bool PwdReset(int id,string newPwd, string newPwd2);
        bool CreateOwner(Owner owner);
        bool EditOwner(Owner owner);
        bool DeleteOwner(int id);
        void Dispose();
    }
}
