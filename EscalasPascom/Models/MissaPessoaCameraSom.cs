namespace EscalasPascom.Models
{
    public class MissaPessoaCameraSom
    {
        public int MissaId { get; set; }
        public Missa Missa { get; set; }
        public int PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }
        public int PapelId { get; set; }
        public Papel Papel { get; set; }
    }
}
