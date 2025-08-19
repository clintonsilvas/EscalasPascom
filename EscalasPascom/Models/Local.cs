namespace EscalasPascom.Models
{
    public class Local
    {
        public int Id { get; set; }
        public required string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public List<Missa> Missas { get; set; } = new List<Missa>();
    }
}
