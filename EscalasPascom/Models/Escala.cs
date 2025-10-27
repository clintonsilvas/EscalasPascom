namespace EscalasPascom.Models
{
    public class Escala
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime Data { get; set; } 
        public int IdPastoral { get; set; }
        public Pastoral Pastoral { get; set; } = null!;

        public List<Missa> Missas { get; set; } = new List<Missa>();        
        
    }
}
