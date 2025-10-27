using EscalasPascom.Models;
using EscalasPascom.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EscalasPascom.Controllers
{
    public class ServoController : Controller
    {
        private readonly AppDbContext _context;

        public ServoController(AppDbContext context)
        {
            _context = context;
        }

        //public IActionResult Index(int? mes, int? ano)
        //{
        //    var mesAtual = mes ?? DateTime.Now.Month;
        //    var anoAtual = ano ?? DateTime.Now.Year;

        //    var pastorais = _context.
                

        //    ViewBag.Locais = _context.Locais.Where(l => l.Ativo).OrderBy(l => l.Nome).ToList();
        //    ViewBag.MesSelecionado = mesAtual;
        //    ViewBag.AnoSelecionado = anoAtual;

        //    return View(missas);
        public IActionResult Index()
        {

            return View();
        }
    }
}
