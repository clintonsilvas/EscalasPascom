namespace EscalasPascom.Models
{
    public class MissaPessoa
    {
        public int IdMissa { get; set; }
        public Missa Missa { get; set; }
        public string IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public int IdPapel { get; set; }
        public Papel Papel { get; set; }
    }
}
