using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EscalasPascom.Models
{
    public class AppDbContext : IdentityDbContext<Usuario>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Entidades Principais
        public DbSet<Paroquia> Paroquias { get; set; }
        public DbSet<Pastoral> Pastorais { get; set; }
        public DbSet<Papel> Papeis { get; set; }
        public DbSet<Missa> Missas { get; set; }
        public DbSet<Indisponibilidade> Indisponibilidades { get; set; }
        public DbSet<Local> Locais { get; set; }
        public DbSet<Escala> Escalas { get; set; }

        // Entidades de Ligação (Embora navegadas, é bom ter o DbSet para clareza e consulta)
        public DbSet<MissaPessoa> UsuarioMissas { get; set; }
        public DbSet<MissaEscala> MissaEscalas { get; set; }
        public DbSet<PapelMissa> MissaPapeis { get; set; }
        public DbSet<UsuarioPapelPastoral> UsuarioPastorais { get; set; }
        public DbSet<UsuarioPapelPastoral> UsuarioPapelPastorais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 2. CHAMA A BASE do IdentityDbContext (ESSENCIAL!)
            base.OnModelCreating(modelBuilder);

            // === Configuração das Chaves Compostas ===

            // 1. UsuarioPastoral (N:N entre Usuario e Pastoral)
            modelBuilder.Entity<UsuarioPapelPastoral>()
                .HasKey(up => new { up.IdUsuario, up.IdPastoral });
            // Configura o tipo de ID do Identity (string)
            modelBuilder.Entity<UsuarioPapelPastoral>()
                .Property(up => up.IdUsuario).HasMaxLength(450);


            // 2. UsuarioPapelPastoral (N:N:N - Habilidade do Usuário)
            modelBuilder.Entity<UsuarioPapelPastoral>()
                .HasKey(upp => new { upp.IdUsuario, upp.IdPapel, upp.IdPastoral });
            modelBuilder.Entity<UsuarioPapelPastoral>()
                .Property(upp => upp.IdUsuario).HasMaxLength(450);


            // 3. UsuarioMissa (N:N:N - Quem está escalado, onde e com qual papel)
            modelBuilder.Entity<MissaPessoa>()
                .HasKey(um => new { um.IdUsuario, um.IdMissa, um.IdPapel });
            modelBuilder.Entity<MissaPessoa>()
                .Property(um => um.IdUsuario).HasMaxLength(450);
            // Relacionamento com Usuario: Garante que a chave PessoaId é a chave do Identity
            modelBuilder.Entity<MissaPessoa>()
                .HasOne(um => um.Usuario) // "Pessoa" é o Usuario na MissaPessoa
                .WithMany(u => u.MissasEscalado)
                .HasForeignKey(um => um.IdUsuario);


            // 4. MissaEscala (N:N entre Missa e Escala)
            modelBuilder.Entity<MissaEscala>()
                .HasKey(me => new { me.IdMissa, me.IdEscala });


            // 5. MissaPapel (N:N - Requisito de Quantidade)
            modelBuilder.Entity<PapelMissa>()
                .HasKey(mp => new { mp.IdMissa, mp.IdPapel });

            // === Outras Configurações ===

            // Configuração de Chave Estrangeira Única (Se necessário):
            // Se o Papel deve ser único dentro da Pastoral, você pode adicionar um índice.
            modelBuilder.Entity<Papel>()
                .HasIndex(p => new { p.Descricao, p.IdPastoral })
                .IsUnique();

            // Filtro Global de Dados (EXEMPLO)
            // Para garantir o isolamento, considere o filtro global que discutimos.
            // Exemplo para Missa:
            /*
            modelBuilder.Entity<Missa>().HasQueryFilter(
                m => m.IdParoquia == EF.Property<int>(m, "ParoquiaIdFiltro")
            );
            */

        }
    }

}
