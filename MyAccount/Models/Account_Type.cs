using MyAccount.Utils.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyAccount.Models
{
    public class Account_Type
    {
        private int id;
        private AccountType_type type;
        private string name;

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

        [Display(Name = "业务类型")]
        public AccountType_type Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        [Display(Name = "业务名称")]
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
    }
}