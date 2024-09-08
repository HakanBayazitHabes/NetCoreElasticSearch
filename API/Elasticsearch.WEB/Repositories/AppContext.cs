using Elasticsearch.WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace Elasticsearch.WEB.Repositories
{

    public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
    {

        public DbSet<Drug> DRUG { get; set; }
        public DbSet<DrugBarcode> DRUGBARCODE { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Drug - DrugBarcode ilişkisi (One-to-Many)
            modelBuilder.Entity<Drug>()
                .HasMany(d => d.DrugBarcodes)
                .WithOne(db => db.Drug)
                .HasForeignKey(db => db.DrugId);
        }

    }
}