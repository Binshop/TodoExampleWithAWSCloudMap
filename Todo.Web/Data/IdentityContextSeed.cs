using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Web.Models;

namespace Todo.Web.Data
{
    public static class IdentityContextSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var user in userList)
            {
                await EnsureUser(userManager, user.Key, user.Value);
            }
        }

        private static async Task<string> EnsureUser(UserManager<ApplicationUser> userManager,
                                                     string userName, string userPassword)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(user, userPassword);
            }

            return user.Id;
        }

        private static Dictionary<string, string> userList => new Dictionary<string, string>
        {
            {"demo@local", "Demo!234" }
        };
    }
}
