namespace EscalasPascom.Models
{
    public class Escala
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Mes { get; set; } = string.Empty;

        public int Ano { get; set; }

        public List<Missa> Missas { get; set; } = new List<Missa>();
    }
}
