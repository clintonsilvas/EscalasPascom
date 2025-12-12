using Microsoft.EntityFrameworkCore;

namespace EscalasPascom.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Papel> Papeis { get; set; }
        public DbSet<Missa> Missas { get; set; }
        public DbSet<Indisponibilidade> Indisponibilidades { get; set; }
        public DbSet<Local> Locais { get; set; }
        public DbSet<Escala> Escalas { get; set; }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<MissaPessoaCameraSom>()
                .HasKey(mpcs => new { mpcs.MissaId, mpcs.PessoaId, mpcs.PapelId });

            modelBuilder.Entity<MissaPessoaCameraSom>()
                .HasOne(mpcs => mpcs.Missa)
                .WithMany(m => m.Escalados)
                .HasForeignKey(mpcs => mpcs.MissaId);

            modelBuilder.Entity<MissaPessoaCameraSom>()
                .HasOne(mpcs => mpcs.Pessoa)
                .WithMany(p => p.Missas)
                .HasForeignKey(mpcs => mpcs.PessoaId);

            modelBuilder.Entity<MissaPessoaCameraSom>()
                .HasOne(mpcs => mpcs.Papel)
                .WithMany()
                .HasForeignKey(mpcs => mpcs.PapelId);
        }



    }

}
