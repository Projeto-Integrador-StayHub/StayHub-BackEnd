using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AdminModel> Admins { get; set; }
        public DbSet<DonoHotelModel> DonosHoteis { get; set; }
        public DbSet<HospedeModel> Hospedes { get; set; }
        public DbSet<QuartoModel> Quartos { get; set; }
        public DbSet<ReservaModel> Reservas { get; set; }
}
}
