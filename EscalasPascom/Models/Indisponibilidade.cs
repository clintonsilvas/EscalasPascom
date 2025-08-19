namespace EscalasPascom.Models
{
    public class Indisponibilidade
    {
        public int Id { get; set; }
        public int PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan? HoraInicio { get; set; } 
        public TimeSpan? HoraFim { get; set; }

        public string? Observacao { get; set; }
    }
}
