namespace EscalasPascom.Models
{
    public class Missa
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }        
        public int LocalId { get; set; }
        public Local Local { get; set; }
        public string? Descricao { get; set; }
        public bool TemTransmissao { get; set; }
        public bool TemMidia { get; set; }
        public int QuantidadeCameraSom { get; set; }
        public List<MissaPessoaCameraSom> Escalados { get; set; } = new List<MissaPessoaCameraSom>();
        public int? EscalaId { get; set; }
        public Escala? Escala { get; set; }

    }
}
