using EscalasPascom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EscalasPascom.Controllers
{
    public class EscalaController : Controller
    {
        private readonly AppDbContext _context;

        public EscalaController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? mes, int? ano)
        {
            int mesSel = mes ?? DateTime.Now.Month;
            int anoSel = ano ?? DateTime.Now.Year;

            var missas = _context.Missas
                .Include(m => m.Local)
                .Include(m => m.Escalados)
                .Where(m => m.Data.Month == mesSel && m.Data.Year == anoSel)
                .OrderBy(m => m.Data)
                .ToList();


            var pessoas = _context.Pessoas.Include(p => p.Indisponibilidades).Include(p => p.Papeis).ToList();

            var operadores = pessoas.Where(p => p.Papeis.Any(r => r.Descricao == "Operador")).ToList();
            var fotografos = pessoas.Where(p => p.Papeis.Any(r => r.Descricao == "Foto/Som")).ToList();
            var midias = pessoas.Where(p => p.Papeis.Any(r => r.Descricao == "Mídia")).ToList();

            ViewBag.Operadores = operadores;
            ViewBag.Fotografos = fotografos;
            ViewBag.Midias = midias;

            ViewBag.MesSelecionado = mesSel;
            ViewBag.AnoSelecionado = anoSel;

            ViewBag.Sorteado = false;

            return View(missas);
        }


        [HttpPost]
        public IActionResult Sortear(int? mes, int? ano)
        {
            int mesSel = mes ?? DateTime.Now.Month;
            int anoSel = ano ?? DateTime.Now.Year;

            // Busca tudo de uma vez
            var missas = _context.Missas
                .Include(m => m.Local)
                .Include(m => m.Escalados)
                .Where(m => m.Data.Month == mesSel && m.Data.Year == anoSel)
                .OrderBy(m => m.Data)
                .ToList();

            var pessoas = _context.Pessoas
                .Include(p => p.Indisponibilidades)
                .Include(p => p.Papeis)
                .ToList();

            var operadores = pessoas.Where(p => p.Papeis.Any(r => r.Descricao == "Operador")).ToList();
            var fotografos = pessoas.Where(p => p.Papeis.Any(r => r.Descricao == "Foto/Som")).ToList();
            var midias = pessoas.Where(p => p.Papeis.Any(r => r.Descricao == "Mídia")).ToList();


            var contadorEscalas = new Dictionary<int, int>();

            Pessoa SortearBalanceado(List<Pessoa> lista, DateTime data)
            {
                var disponiveis = lista
                    .Where(p => !p.Indisponibilidades.Any(i => i.Data.Date == data.Date))
                    .OrderBy(p => contadorEscalas.ContainsKey(p.Id) ? contadorEscalas[p.Id] : 0)
                    .ThenBy(_ => Guid.NewGuid())
                    .ToList();

                var escolhido = disponiveis.FirstOrDefault();
                if (escolhido != null)
                {
                    if (!contadorEscalas.ContainsKey(escolhido.Id))
                        contadorEscalas[escolhido.Id] = 0;

                    contadorEscalas[escolhido.Id]++;
                }

                return escolhido;
            }

            foreach (var missa in missas)
            {                
                if (missa.TemTransmissao)
                {
                    var operador = SortearBalanceado(operadores, missa.Data);
                    if (operador != null)
                        missa.Escalados.Add(new MissaPessoaCameraSom
                        {
                            MissaId = missa.Id,
                            PessoaId = operador.Id,
                            PapelId = operador.Papeis.FirstOrDefault(r => r.Descricao == "Operador").Id
                        });
                }

                if (missa.TemMidia)
                {
                    var midia = SortearBalanceado(midias, missa.Data);
                    if (midia != null)
                        missa.Escalados.Add(new MissaPessoaCameraSom
                        {
                            MissaId = missa.Id,
                            PessoaId = midia.Id,
                            PapelId = midia.Papeis.FirstOrDefault(r => r.Descricao == "Mídia").Id
                        }); 
                }

                for (var i = 0; i < missa.QuantidadeCameraSom; i++)
                {
                    var fotografo = SortearBalanceado(fotografos, missa.Data);
                    if (fotografo != null)
                        missa.Escalados.Add(new MissaPessoaCameraSom
                        {
                            MissaId = missa.Id,
                            PessoaId = fotografo.Id,
                            PapelId = fotografo.Papeis.FirstOrDefault(r => r.Descricao == "Foto/Som").Id
                        });
                }
            }

            _context.SaveChanges();

            ViewBag.Operadores = operadores;
            ViewBag.Fotografos = fotografos;
            ViewBag.Midias = midias;
            ViewBag.MesSelecionado = mesSel;
            ViewBag.AnoSelecionado = anoSel;

            ViewBag.Sorteado = true;

            return View("Index", missas);
        }


        //[HttpPost]
        //public IActionResult Salvar(List<Missa> missas)
        //{
        //    foreach (var vm in missas)
        //    {
        //        var existente = _context.Missas.Find(vm.Id);
        //        if (existente != null)
        //        {
        //            existente.OperadorId = vm.OperadorId;
        //            existente.MidiaId = vm.MidiaId;
        //            existente.CamerasSom = vm.CamerasSom;
        //        }
        //    }
        //    _context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}
    }

}
