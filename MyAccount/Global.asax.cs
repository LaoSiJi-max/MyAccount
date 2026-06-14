using MyAccount.Models;
using MyAccount.Utils.Enums;
using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyAccount
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            bool reset = false;

            if (reset)
            {
                Database.SetInitializer(new DropCreateDatabaseAlways<MyAccountContext>());
                MyAccountContext db = new MyAccountContext();
                Owner boss = new Owner();
                boss.Id = 1;
                boss.Name = "boss";
                boss.Pwd = "e1adc3949ba59abbe56e057f2f883e";
                boss.Pwd2 = "e1adc3949ba59abbe56e057f2f883e";
                boss.State = OwnerState.正常;
                boss.Level = OwnerLevel.超级管理员;
                boss.CreateTime = DateTime.Now;
                boss.LastTime = DateTime.Now;
                db.Owners.Add(boss);
                db.SaveChanges();
            }
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
