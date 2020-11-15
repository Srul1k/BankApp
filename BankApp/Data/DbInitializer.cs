using BankApp.Models;
using System.Linq;

namespace BankApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User{ Age = 22, Name = "Дмитрий", Surname = "Чмель", AdressOfResidance = "ул. Пушкинская 100", PhoneNumber = "+375256238790",
                Email = "chmel@mail.ru", Password = "1337"},
                new User{ Age = 24, Name = "Валерия", Surname = "Гарапучик", AdressOfResidance = "ул. Крыничная 17", PhoneNumber = "+375337658923",
                Email = "garik@mail.ru", Password = "0666"},
                new User{ Age = 19, Name = "Александр", Surname = "Новик", AdressOfResidance = "ул. Мира 3", PhoneNumber = "+375333222028",
                Email = "novik@mail.ru", Password = "1488"},
            };

            foreach (var u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();


            var roles = new Role[]
            {
                new Role { Name = "User" },
                new Role { Name = "Admin" }
            };

            foreach (var r in roles)
            {
                context.Roles.Add(r);
            }
            context.SaveChanges();

            var userAccounts = new UserAccount[]
            {
                new UserAccount{ UserId = 1, AmountOfMoney = 1500 },
                new UserAccount{ UserId = 1, AmountOfMoney = 829 },
                new UserAccount{ UserId = 2, AmountOfMoney = 619 },
            };

            foreach (var ua in userAccounts)
            {
                context.UserAccounts.Add(ua);
            }
            context.SaveChanges();

            foreach (var u in users)
            {
                u.UserRoles.Add(new UserRole { UserId = u.Id, RoleId = roles[0].Id });
            }

            users[0].UserRoles.Add(new UserRole { UserId = users[0].Id, RoleId = roles[1].Id });

            context.SaveChanges();
        }
    }
}
