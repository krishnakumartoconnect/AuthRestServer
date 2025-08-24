
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA1050 // Declare types in namespaces
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
#pragma warning restore CA1050 // Declare types in namespaces
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
}
