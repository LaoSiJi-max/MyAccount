using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAccount.Models
{
    public class Customer
    {
        private int id;
        private string num;
        private string pwd;
        private string name;
        private double balance;
        private DateTime lastTime;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        [Display(Name="编号")]
        [StringLength(32, ErrorMessage = "{0}长度必须为1-32")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string Num
        {
            get
            {
                return num;
            }

            set
            {
                num = value;
            }
        }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "{0}长度必须为6-32")]
        [DataType(DataType.Password)]
        public string Pwd
        {
            get
            {
                return pwd;
            }

            set
            {
                pwd = value;
            }
        }

        [Display(Name = "客户名")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "{0}长度必须为1-16")]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        [Display(Name = "余额")]
        [Required(ErrorMessage = "{0}不能为空")]
        public double Balance
        {
            get
            {
                return balance;
            }

            set
            {
                balance = value;
            }
        }

        [Display(Name = "最后操作时间")]
        public DateTime LastTime
        {
            get
            {
                return lastTime;
            }

            set
            {
                lastTime = value;
            }
        }
    }
}