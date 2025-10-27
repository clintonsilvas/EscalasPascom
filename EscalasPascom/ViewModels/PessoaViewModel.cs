using Microsoft.AspNetCore.Mvc.Rendering;

namespace EscalasPascom.ViewModels
{
    public class PessoaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<int> PapeisSelecionados { get; set; } = new();
        public List<SelectListItem> TodosPapeis { get; set; } = new();
        public bool IsAtivo { get; set; } = true;

    }
}
