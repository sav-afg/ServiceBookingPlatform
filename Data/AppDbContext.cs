using ServiceBookingPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace ServiceBookingPlatform.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext
    {
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<User> Users => Set<User>();

    }
}
