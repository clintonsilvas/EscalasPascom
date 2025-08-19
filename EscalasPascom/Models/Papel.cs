namespace EscalasPascom.Models
{
    public class Papel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public List<Pessoa> Pessoas { get; set; } = new();
    }
}
