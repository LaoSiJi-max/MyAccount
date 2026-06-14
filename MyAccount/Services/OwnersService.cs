using MyAccount.Models;
using MyAccount.Services.IServices;
using MyAccount.Utils.Enums;
using MyAccount.Utils.Ifs.ISecurity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MyAccount.Services
{
    public class OwnersService : IOwnersService
    {
        private static MyAccountContext db = null;
        private IEncrypt encrypt = null;

        private static bool isLogined = false;

        public static bool IsLogined
        {
            get
            {
                return isLogined;
            }

            set
            {
                isLogined = value;
            }
        }

        /// <summary>
        /// 获取登录的主人信息
        /// </summary>
        /// <returns></returns>
        public static Owner GetLoginOwner()
        {
            Owner loginOwner = (Owner)HttpContext.Current.Session["loginOwner"];

            return loginOwner;
        }

        public OwnersService(IEncrypt iEncrypt)
        {
            db = new MyAccountContext();
            encrypt = iEncrypt;
        }
        public IPagedList<Owner> GetOwnersPage(OwnerLevel ownerLevel,int page,int size)

        {
            int count = db.Owners.Where(o => o.Level != OwnerLevel.超级管理员).Where(o => o.Level < ownerLevel).Count();
            //避开超级管理员(3级)和小于参数等级的主人
            return db.Owners.Where(o => o.Level != OwnerLevel.超级管理员).Where(o => o.Level < ownerLevel).OrderBy(o => o.Id).ToPagedList(page,size);
        }

        public IPagedList<Owner> GetOwnersPage(OwnerLevel ownerLevel, string findBy, string keyword, int page, int size)
        {
            switch (findBy)
            {
                case "id":
                    int id = Convert.ToInt32(keyword);
                    return db.Owners.Where(o => o.Id == id).Where(o => o.Level != OwnerLevel.超级管理员).Where(o => o.Level < ownerLevel).OrderBy(o => o.Id).ToPagedList(page,size);

                case "name":
                    return db.Owners.Where(o=>o.Name.IndexOf(keyword)!=-1).Where(o => o.Level != OwnerLevel.超级管理员).Where(o => o.Level < ownerLevel).OrderBy(o => o.Id).ToPagedList(page, size);

                default:
                    return null;
            }
            
        }

        public Owner GetOwner(int? id)
        {
            return db.Owners.Find(id);
        }

        /// <summary>
        /// 主人登录验证
        /// </summary>
        /// <param name="name">主人名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public int LoginHash(string name,string pwd)
        {
            string cipherpwd = encrypt.DoEncrypt(pwd);
            //按用户输入的名字、密码查询
            List<Owner> list = db.Owners.Where(o => o.Name==name).Where(o => o.Pwd== cipherpwd).ToList();

            //检测是否有相符的记录
            if (list.Count > 0)
            {
                Owner loginOwner = list[0];

                //检测用户状态
                switch (loginOwner.State)
                {
                    case OwnerState.正常:
                        OwnersService.IsLogined = true;

                        //更新最后登录时间
                        loginOwner.LastTime = DateTime.Now;
                        db.Entry(loginOwner).State = EntityState.Modified;
                        db.SaveChanges();

                        //克隆一个新的登录用户
                        Owner loginOwner2 = (Owner)loginOwner.Clone();
                        //清空密码字段
                        loginOwner2.Pwd = null;
                        loginOwner2.Pwd2 = null;
                        //把登录用户的信息存入session
                        HttpContext.Current.Session["loginOwner"] = loginOwner2;

                        return 1;

                    case OwnerState.停用:
                        return -1;

                    default:
                        return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 主人登出
        /// </summary>
        public void Logout()
        {
            OwnersService.IsLogined = false;
            HttpContext.Current.Session["loginOwner"] = null;
        }

        public bool StateChange(int? id)
        {
            Owner owner = db.Owners.Find(id);

            if (owner != null)
            {
                if (owner.State == OwnerState.正常)
                {
                    owner.State = OwnerState.停用;
                }
                else if (owner.State == OwnerState.停用)
                {
                    owner.State = OwnerState.正常;
                }

                db.Entry(owner).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool PwdReset(int id, string newPwd, string newPwd2)
        {
            try
            {
                Owner owner = db.Owners.Find(id);
                owner.Pwd = encrypt.DoEncrypt(newPwd);
                owner.Pwd2 = encrypt.DoEncrypt(newPwd2);

                db.Entry(owner).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取权限等级下拉列表内容
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetLevelItems()
        {
            List<SelectListItem> levelList = new List<SelectListItem>();
            string[] levelName = new string[] { "普通主人", "系统管理员", "超级管理员" };

            for (int i = 0; i < Convert.ToInt32(OwnersService.GetLoginOwner().Level); i++)
            {
                levelList.Add(new SelectListItem { Text = levelName[i], Value = i.ToString() });
            }

            return levelList;
        }

        public bool CreateOwner(Owner owner)
        {
            try
            {
                owner.Pwd = encrypt.DoEncrypt(owner.Pwd);
                owner.Pwd2 = encrypt.DoEncrypt(owner.Pwd2);
                owner.LastTime = DateTime.Now;
                owner.CreateTime = DateTime.Now;

                db.Owners.Add(owner);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EditOwner(Owner owner)
        {
            try
            {
                Owner newOwner = db.Owners.Find(owner.Id);
                newOwner.Name = owner.Name;
                newOwner.Level = owner.Level;

                db.Entry(newOwner).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteOwner(int id)
        {
            try
            {
                Owner owner = db.Owners.Find(id);
                db.Owners.Remove(owner);
                db.SaveChanges();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public List<SelectListItem> GetFindByItems()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            string[] names = new string[] { "ID", "名字"};
            string[] values = new string[] { "id", "name"};

            for (int i = 0; i < names.Length; i++)
            {
                list.Add(new SelectListItem { Text = names[i], Value = values[i] });
            }

            return list;
        }
    }
}