using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcUI.Models
{
  // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  public class ApplicationUser : IdentityUser, System.Security.Principal.IIdentity
  {
    private System.Security.Principal.IIdentity _identity;

    public ApplicationUser()
    { }

    public ApplicationUser(ProjectTracker.Library.Security.PTIdentity identity)
    {
      _identity = identity;
      this.UserName = Name;
      this.Id = Name;
      foreach (var item in identity.Roles)
      {
        this.Roles.Add(new IdentityUserRole { UserId = this.Id, RoleId = item });
      }
    }

    public string AuthenticationType
    {
      get { return _identity.AuthenticationType; }
    }

    public bool IsAuthenticated
    {
      get { return _identity.IsAuthenticated; }
    }

    public string Name
    {
      get { return _identity.Name; }
    }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
      // Add custom user claims here
      return userIdentity;
    }
  }

  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationDbContext()
      : base("DefaultConnection")
    {
    }

    static ApplicationDbContext()
    {
      // Set the database intializer which is run once during application start
      // This seeds the database with admin user credentials and admin role
      Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
    }

    public static ApplicationDbContext Create()
    {
      return new ApplicationDbContext();
    }
  }
}