namespace EscalasPascom.Models
{
    public class Paroquia
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Diocese { get; set; } = string.Empty;
        public List<Pastoral> Pastorais { get; set; } = new List<Pastoral>();
    }
}
