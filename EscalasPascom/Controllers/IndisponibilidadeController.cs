using EscalasPascom.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EscalasPascom.Controllers
{
    public class IndisponibilidadeController : Controller
    {
        private readonly AppDbContext _context;

        public IndisponibilidadeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? mes, int? ano)
        {
            mes ??= DateTime.Now.Month;
            ano ??= DateTime.Now.Year;

            var pessoas = _context.Pessoas
                .Include(p => p.Indisponibilidades
                    .Where(i => i.Data.Month == mes && i.Data.Year == ano))
                .OrderBy(p => p.Nome)
                .ToList();

            ViewBag.MesSelecionado = mes;
            ViewBag.AnoSelecionado = ano;

            return View(pessoas);
        }
        [HttpPost]
        public IActionResult SalvarIndisponibilidades(string mes,string ano, Dictionary<int, List<int>> indisponibilidadesPorPessoa)
        {
            // Valida e converte mês e ano
            if (!int.TryParse(mes, out int mesInt) || mesInt < 1 || mesInt > 12 ||
                !int.TryParse(ano, out int anoInt) || anoInt < 1)
            {
                TempData["Erro"] = "Mês ou ano inválido.";
                return RedirectToAction("Index");
            }

            // Validação: se não houver nenhuma indisponibilidade marcada
            bool temMarcacao = indisponibilidadesPorPessoa != null &&
                               indisponibilidadesPorPessoa.Any(p => p.Value != null && p.Value.Any());

            if (!temMarcacao)
            {
                TempData["Erro"] = "Nenhuma indisponibilidade foi marcada.";
                return RedirectToAction("Index", new { mes = mesInt, ano = anoInt });
            }

            try
            {
                foreach (var pessoa in _context.Pessoas.Include(p => p.Indisponibilidades))
                {
                    // Remove indisponibilidades do mês/ano selecionado
                    var existentes = pessoa.Indisponibilidades
                        .Where(i => i.Data.Year == anoInt && i.Data.Month == mesInt)
                        .ToList();
                    _context.Indisponibilidades.RemoveRange(existentes);

                    // Adiciona as novas
                    if (indisponibilidadesPorPessoa.TryGetValue(pessoa.Id, out var dias))
                    {
                        foreach (var dia in dias)
                        {
                            if (dia >= 1 && dia <= DateTime.DaysInMonth(anoInt, mesInt))
                            {
                                _context.Indisponibilidades.Add(new Indisponibilidade
                                {
                                    PessoaId = pessoa.Id,
                                    Data = new DateTime(anoInt, mesInt, dia)
                                });
                            }
                        }
                    }
                }

                _context.SaveChanges();
                TempData["Sucesso"] = "Indisponibilidades salvas com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Ocorreu um erro ao salvar as indisponibilidades: " + ex.Message;
            }

            return RedirectToAction("Index", new { mes = mesInt, ano = anoInt });
        }



    }
}
