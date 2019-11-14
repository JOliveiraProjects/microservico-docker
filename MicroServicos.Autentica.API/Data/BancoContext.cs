using MicroServicos.Autentica.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServicos.Autentica.API.Data
{
    public class BancoContext : DbContext
    {
        public DbSet<AutenticaModel> Autentica { get; set; }

        public BancoContext(DbContextOptions<BancoContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
