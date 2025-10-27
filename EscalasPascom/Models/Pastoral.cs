namespace EscalasPascom.Models
{
    public class Pastoral
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        
        public int IdParoquia { get; set; }
        public Paroquia Paroquia { get; set; } = null!;

        public List<Escala> Escalas { get; set; } = new List<Escala>();
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public List<Papel> Papeis { get; set; } = new List<Papel>();
    }
}
