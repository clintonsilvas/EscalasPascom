namespace EscalasPascom.Models
{
    public class UsuarioPapelPastoral
    {
        public string IdUsuario { get; set; } = null!;

        public Usuario Usuario { get; set; } = null!;
        public int IdPapel { get; set; }    
        public Papel Papel { get; set; } = null!;

        public int IdPastoral { get; set; }
        public Pastoral Pastoral { get; set; } = null!;
    }
}
