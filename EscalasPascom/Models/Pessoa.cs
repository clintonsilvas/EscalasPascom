namespace EscalasPascom.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Papel> Papeis { get; set; } = new();
        public List<Indisponibilidade> Indisponibilidades { get; set; } = new();
        public List<MissaPessoaCameraSom> Missas { get; set; } = new List<MissaPessoaCameraSom>();
        public bool IsAtivo { get; set; } = true;
    }
}
