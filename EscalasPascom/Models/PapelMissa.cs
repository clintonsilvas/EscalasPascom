namespace EscalasPascom.Models
{
    public class PapelMissa
    {        
        public int IdPapel { get; set; }
        public Papel Papel { get; set; } = null!;
        public int IdMissa { get; set; }
        public Missa Missa { get; set; } = null!;
        public int Quantidade { get; set; }
    }
}
