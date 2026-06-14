using MyAccount.Utils.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAccount.Models
{
    public class Owner : ICloneable
    {
        private int id;
        private string name;
        private string pwd;
        private string pwd2;
        private OwnerState state = OwnerState.正常;
        private OwnerLevel level;
        private DateTime lastTime;
        private DateTime createTime;

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

        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "{0}长度必须为1-16")]
        //[RegularExpression("[a-zA-Z][a-zA-Z0-9_]",ErrorMessage ="{0}必须以字母开头，只能包括字母、数字、下划线")]
        [Display(Name = "用户名")]
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

        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "{0}长度必须为6-32")]
        //[RegularExpression("[A-Za-z0-9!#$%^&*.~]", ErrorMessage = "{0}中包含非法字符")]
        [Display(Name = "密码")]
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

        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "{0}长度必须为6-32")]
        [Display(Name = "密码确认")]
        [Compare("Pwd",ErrorMessage ="两次输入的密码不一致")]
        [DataType(DataType.Password)]
        public string Pwd2
        {
            get
            {
                return pwd2;
            }

            set
            {
                pwd2 = value;
            }
        }

        [Display(Name = "状态")]
        public OwnerState State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        [Display(Name = "等级")]
        public OwnerLevel Level
        {
            get
            {
                return level;
            }

            set
            {
                level = value;
            }
        }

        [Display(Name = "最后登录时间")]
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

        [Display(Name = "创建时间")]
        public DateTime CreateTime
        {
            get
            {
                return createTime;
            }

            set
            {
                createTime = value;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}