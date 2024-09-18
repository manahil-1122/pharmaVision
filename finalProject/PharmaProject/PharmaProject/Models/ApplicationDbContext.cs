
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PharmaProject.Models;
public class ApplicationDbContext : DbContext

{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    
    public DbSet<Users> Clients { get; set; }
    public DbSet<Tablets> Medicine { get; set; }
    public DbSet<Capsules> Encap { get; set; }
    public DbSet<Liquid> Syringe { get; set; }
    public DbSet<Feedback> reviews { get; set; }
    public DbSet<Apply> resume { get; set; }

    public DbSet<Order> orders { get; set; }
    public DbSet<OrderStatus> status { get; set; }




}
