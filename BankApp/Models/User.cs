using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Surname { get; set; }
        public string AdressOfResidance { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<UserAccount> UserAccounts { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User()
        {
            UserRoles = new List<UserRole>();
        }
    }
}
