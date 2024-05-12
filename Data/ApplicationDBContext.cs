using Microsoft.EntityFrameworkCore;
using Taller.Models;

namespace Taller.Data;

public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : DbContext(options)
{
    public DbSet<Client> Clients {get;set;}
    public DbSet<Worker> Workers {get;set;}
    public DbSet<Service> Services {get;set;}
    public DbSet<Bill> Bills {get;set;}

}