using EscalasPascom.Models;
using EscalasPascom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace EscalasPascom.Controllers
{
    public class PessoaController : Controller
    {
        public AppDbContext _context;
        public PessoaController(AppDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var pasconeiros = _context.Pessoas
                .Include(p => p.Papeis)
                .Select(p => new PessoaViewModel
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    PapeisSelecionados = p.Papeis.Select(pa => pa.Id).ToList(),
                    TodosPapeis = _context.Papeis
                        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Descricao })
                        .ToList(),
                    IsAtivo = p.IsAtivo
                }).OrderBy(p => p.Nome)
                .ToList();

            // Garante que haja pelo menos uma linha vazia para nova pessoa
            if (!pasconeiros.Any() || pasconeiros.All(p => p.Id != 0))
            {
                pasconeiros.Add(new PessoaViewModel
                {
                    TodosPapeis = _context.Papeis
                        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Descricao })
                        .ToList()
                });
            }

            return View(pasconeiros);
        }

        [HttpPost]
        public IActionResult SalvarPessoas(List<PessoaViewModel> model)
        {
            foreach (var vm in model)
            {
                if (string.IsNullOrWhiteSpace(vm.Nome))
                    continue; // Ignora linhas vazias

                if (vm.Id == 0)
                {
                    // Nova pessoa
                    var nova = new Pessoa
                    {
                        Nome = vm.Nome,
                        IsAtivo = vm.IsAtivo,
                        Papeis = _context.Papeis
                            .Where(p => vm.PapeisSelecionados.Contains(p.Id))
                            .ToList()
                    };
                    _context.Pessoas.Add(nova);
                }
                else
                {
                    // Edita existente
                    var existente = _context.Pessoas
                        .Include(p => p.Papeis)
                        .FirstOrDefault(p => p.Id == vm.Id);

                    if (existente != null)
                    {
                        existente.Nome = vm.Nome;
                        existente.IsAtivo = vm.IsAtivo;

                        // Atualiza papeis
                        existente.Papeis.Clear();
                        var papeisSelecionados = _context.Papeis
                            .Where(p => vm.PapeisSelecionados.Contains(p.Id))
                            .ToList();
                        foreach (var papel in papeisSelecionados)
                            existente.Papeis.Add(papel);
                    }
                }
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


    }
}
