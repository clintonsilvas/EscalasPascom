using EscalasPascom.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EscalasPascom.Controllers
{
    public class PapelController : Controller
    {
        private readonly AppDbContext _context;

        public PapelController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var papeis = _context.Papeis.OrderBy(p => p.Descricao).ToList();
            return View(papeis);
        }

        [HttpPost]
        public IActionResult SalvarPapeis(List<Papel> papeis)
        {
            foreach (var papel in papeis)
            {
                if (string.IsNullOrWhiteSpace(papel.Descricao))
                    continue; // ignora vazios

                if (papel.Id == 0)
                {
                    // Novo papel
                    _context.Papeis.Add(papel);
                }
                else
                {
                    // Atualizar papel existente
                    var papelDb = _context.Papeis.Find(papel.Id);
                    if (papelDb != null)
                    {
                        papelDb.Descricao = papel.Descricao;
                    }
                }
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Excluir(int id)
        {
            var papel = _context.Papeis.Find(id);
            if (papel != null)
            {
                _context.Papeis.Remove(papel);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
