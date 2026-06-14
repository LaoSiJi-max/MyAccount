using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyAccount.Models
{
    public class MyAccountContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public MyAccountContext() : base("name=MyAccountContext")
        {
        }

        public System.Data.Entity.DbSet<MyAccount.Models.Owner> Owners { get; set; }
        public System.Data.Entity.DbSet<MyAccount.Models.Customer> Customers { get; set; }
        public System.Data.Entity.DbSet<MyAccount.Models.Account_Log> Account_Log { get; set; }
        public System.Data.Entity.DbSet<MyAccount.Models.Account_Type> Account_Type { get; set; }
    }
}
