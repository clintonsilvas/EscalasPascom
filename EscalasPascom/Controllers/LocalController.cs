using EscalasPascom.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EscalasPascom.Controllers
{
    public class LocalController : Controller
    {

        private readonly AppDbContext _context;

        public LocalController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var locais = _context.Locais.OrderBy(l => l.Nome).ToList();
            return View(locais);
        }

        [HttpPost]
        public IActionResult Salvar(List<Local> locais)
        {
            if (locais == null)
            {
                TempData["Erro"] = "Nenhum local foi enviado.";
                return RedirectToAction("Index");
            }

            try
            {
                foreach (var local in locais)
                {
                    if (local.Id == 0)
                    {
                        // Novo local
                        if (!string.IsNullOrWhiteSpace(local.Nome))
                            _context.Locais.Add(local);
                    }
                    else
                    {
                        // Atualização
                        var existente = _context.Locais.Find(local.Id);
                        if (existente != null)
                        {
                            existente.Nome = local.Nome ?? string.Empty;
                            existente.Ativo = local.Ativo;
                        }
                    }
                }

                _context.SaveChanges();
                TempData["Sucesso"] = "Locais salvos com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao salvar locais: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
