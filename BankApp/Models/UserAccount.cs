using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AmountOfMoney { get; set; }
    }
}
