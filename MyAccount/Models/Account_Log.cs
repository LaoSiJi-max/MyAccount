using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAccount.Models
{
    public class Account_Log
    {
        private int id;
        private Owner owner;
        private Customer customer;
        private Account_Type accountType;
        private double limit;
        private string note;
        private DateTime time;
        private bool isCancel;

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

        public Owner Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
            }
        }

        public Customer Customer
        {
            get
            {
                return customer;
            }

            set
            {
                customer = value;
            }
        }

        public Account_Type AccountType
        {
            get
            {
                return accountType;
            }

            set
            {
                accountType = value;
            }
        }


        [Display(Name = "资金数量")]
        public double Limit
        {
            get
            {
                return limit;
            }

            set
            {
                limit = value;
            }
        }

        [Display(Name = "备注信息")]
        public string Note
        {
            get
            {
                return note;
            }

            set
            {
                note = value;
            }
        }

        [Display(Name = "时间")]
        public DateTime Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        [Display(Name = "是否撤销")]
        public bool IsCancel
        {
            get
            {
                return isCancel;
            }

            set
            {
                isCancel = value;
            }
        }
    }
}