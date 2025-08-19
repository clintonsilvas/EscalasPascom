using EscalasPascom.Models;

namespace EscalasPascom.ViewModels
{
    public class MissaViewModel
    {
        public int Id { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }
        public TimeSpan Hora { get; set; }
        public int IdLocal { get; set; }
        public Local Local { get; set; }
        public string Descricao { get; set; }
        public bool TemTransmissao { get; set; }      
        public bool TemMidia { get; set; }       
        public int QuantidadeCameraSom { get; set; }
    }
}