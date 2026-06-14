using MyAccount.Models;
using MyAccount.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyAccount.Utils.Authorize
{
    public class OwnerAuthorize : AuthorizeAttribute
    {
        private OwnerLevel needLevel;
        public OwnerAuthorize(OwnerLevel needLevel)
        {
            this.needLevel = needLevel;
        }

        public OwnerLevel NeedLevel
        {
            get
            {
                return needLevel;
            }

            set
            {
                needLevel = value;
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Owner loginOwner = httpContext.Session["loginOwner"] as Owner;

            if (loginOwner != null)
            {
                if(loginOwner.Level>=needLevel)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            Owner loginOwner = HttpContext.Current.Session["loginOwner"] as Owner;

            if (loginOwner == null)
            {
                filterContext.Result = new RedirectResult("~/Owners/Login");
            }
            else if(loginOwner.Level<needLevel)
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Insufficient permissions to perform this operation.");
            }
        }
    }
}