namespace EscalasPascom.Models
{
    public class MissaEscala
    {
        public int IdMissa { get; set; }
        public Missa Missa { get; set; } = null!;
        public int IdEscala { get; set; }
        public Escala Escala { get; set; } = null!;
    }
}
