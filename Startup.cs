using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebApplication1.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUsers();
        }

        public void CreateDefaultRolesAndUsers()
        {
            var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();
            if (!roleManger.RoleExists("Administrator"))
            {
                role.Name = "Administrator";
                roleManger.Create(role);

                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "ybahnasy123@gmail.com";
                var check = userManager.Create(user,"Yasser123");
                if (check.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Administrator");
                }
            }

        }
    }
}
