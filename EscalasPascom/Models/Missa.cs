namespace EscalasPascom.Models
{
    public class Missa
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }        
        public int LocalId { get; set; }
        public Local Local { get; set; }
        public string? Descricao { get; set; }  
        public List<Escala> Escalas { get; set; } = new List<Escala>();
        public int IdParoquia { get; set; }
        public Paroquia Paroquia { get; set; } = null!;

        public List<MissaPessoa> EscaladosNaMissa { get; set; } = new List<MissaPessoa>();

        public List<PapelMissa> MissaRequisitoPapel { get; set; } = new List<PapelMissa>();

    }
}
