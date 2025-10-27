namespace EscalasPascom.Models
{
    public class Papel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public int IdPastoral { get; set; } 
        public Pastoral Pastoral { get; set; }


    }
}
