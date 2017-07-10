using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Store.WebUI.Models;

[assembly: OwinStartupAttribute(typeof(Store.WebUI.Startup))]
namespace Store.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }

        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

   
            if (!roleManager.RoleExists("Admin"))
            {

                //create Admin role  
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);


                var user = new ApplicationUser();
                user.UserName = "ivashyn.vadym@gmail.com";
                user.Email = "ivashyn.vadym@gmail.com";

                string userPWD = "Ivashyn_1995";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }

            // Creating User role    
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "user@gmail.com";
                user.Email = "user@gmail.com";

                string userPWD = "User_1995";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "User");

                }
            }
            context.SaveChanges();
        }
    }
}
