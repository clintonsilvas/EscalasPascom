using Microsoft.AspNetCore.Identity;

namespace EscalasPascom.Models
{
    public class Usuario : IdentityUser
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string TipoUsuario { get; set; } = string.Empty;
        public List<Pastoral> Pastorais { get; set; } = new List<Pastoral>();
        public List<Papel> Papeis { get; set; } = new();
        public List<Indisponibilidade> Indisponibilidades { get; set; } = new();
        public List<MissaPessoa> MissasEscalado { get; set; } = new List<MissaPessoa>();
        public bool IsAtivo { get; set; } = true;
    }
}
