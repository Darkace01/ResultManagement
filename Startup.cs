using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using ResultManagement.Models;

[assembly: OwinStartupAttribute(typeof(ResultManagement.Startup))]
namespace ResultManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            //check if admin exists  
            if (!roleManager.RoleExists("Admin"))
            {

                //create admin role
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //create admin             

                var user = new ApplicationUser();
                user.UserName = "admin@techsavvyng.com";
                user.Email = "admin@techsavvyng.com";

                string userPWD = "Admin#1";

                var chkUser = UserManager.Create(user, userPWD);

                //add adminRole to admin
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "admin");

                }
            }


            //check if manager exists  
            if (!roleManager.RoleExists("Manager"))
            {

                //create manager role
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

                //create Result manager             

                var user = new ApplicationUser();
                user.UserName = "manager@techsavvyng.com";
                user.Email = "manager@techsavvyng.com";

                string userPWD = "Manager#1";

                var chkUser = UserManager.Create(user, userPWD);

                //add managerRole to manager  
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "admin");

                }
            }

        }
    }
}
