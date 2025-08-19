using EscalasPascom.Models;
using EscalasPascom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; // .xlsx
using NPOI.HSSF.UserModel; // .xls
using System.Text.RegularExpressions;

namespace EscalasPascom.Controllers
{

    public class MissaController : Controller
    {
        private readonly AppDbContext _context;

        public MissaController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? mes, int? ano)
        {
            var mesAtual = mes ?? DateTime.Now.Month;
            var anoAtual = ano ?? DateTime.Now.Year;

            var missas = _context.Missas
                .Include(m => m.Local)
                .Where(m => m.Data.Month == mesAtual && m.Data.Year == anoAtual)
                .OrderBy(m => m.Data)
                .Select(m => new MissaViewModel
                {
                    Id = m.Id,
                    Ano = m.Data.Year,
                    Mes = m.Data.Month,
                    Dia = m.Data.Day,
                    Hora = m.Data.TimeOfDay,
                    IdLocal = m.LocalId,
                    Local = m.Local,
                    Descricao = m.Descricao,
                    TemTransmissao = m.TemTransmissao,
                    TemMidia = m.TemMidia,
                    QuantidadeCameraSom = m.QuantidadeCameraSom
                })
                .ToList();

            ViewBag.Locais = _context.Locais.Where(l => l.Ativo).OrderBy(l => l.Nome).ToList();
            ViewBag.MesSelecionado = mesAtual;
            ViewBag.AnoSelecionado = anoAtual;

            return View(missas);
        }

        [HttpPost]
        public IActionResult SalvarMissas(List<MissaViewModel> missas, string missasRemovidas)
        {
            foreach (var vm in missas)
            {
                if (vm.Id == 0)
                {
                    var nova = new Missa
                    {
                        Data = new DateTime(vm.Ano, vm.Mes, vm.Dia).Add(vm.Hora),
                        LocalId = vm.IdLocal,
                        Descricao = vm.Descricao,
                        TemTransmissao = vm.TemTransmissao,
                        TemMidia = vm.TemMidia,
                        QuantidadeCameraSom = vm.QuantidadeCameraSom
                    };
                    _context.Missas.Add(nova);
                }
                else
                {
                    var existente = _context.Missas.Find(vm.Id);
                    if (existente != null)
                    {
                        existente.Data = new DateTime(vm.Ano, vm.Mes, vm.Dia).Add(vm.Hora);
                        existente.LocalId = vm.IdLocal;
                        existente.Descricao = vm.Descricao;
                        existente.TemTransmissao = vm.TemTransmissao;
                        existente.TemMidia = vm.TemMidia;
                        existente.QuantidadeCameraSom = vm.QuantidadeCameraSom;
                        _context.Missas.Update(existente);
                    }
                }
            }
            // 4. Lógica para remover as missas
            if (!string.IsNullOrEmpty(missasRemovidas))
            {
                var idsParaRemover = missasRemovidas.Split(',')
                                                   .Select(int.Parse)
                                                   .ToList();

                var missasParaExcluir = _context.Missas
                    .Where(m => idsParaRemover.Contains(m.Id));

                _context.Missas.RemoveRange(missasParaExcluir);
            }
            _context.SaveChanges();
            TempData["Sucesso"] = "Missas salvas com sucesso!";

            return RedirectToAction(nameof(Index), new { mes = missas.FirstOrDefault()?.Mes, ano = missas.FirstOrDefault()?.Ano });
        }

        [HttpPost]
        public IActionResult Importar(IFormFile arquivo, int mes, int ano)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                TempData["Erro"] = "Selecione um arquivo Excel (.xls ou .xlsx).";
                return RedirectToAction(nameof(Index), new { mes, ano });
            }

            var ext = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
            if (ext != ".xlsx" && ext != ".xls")
            {
                TempData["Erro"] = "Formato inválido. Use .xls ou .xlsx.";
                return RedirectToAction(nameof(Index), new { mes, ano });
            }
                       
            var locais = _context.Locais.Where(l => l.Ativo).OrderBy(l => l.Nome).ToList();

            // Carrega o que já existe no mês/ano para deduplicar
            var existentes = _context.Missas
                .Include(m => m.Local)
                .Where(m => m.Data.Month == mes && m.Data.Year == ano)
                .ToList();

            var importados = new List<MissaViewModel>();
            var avisos = new List<string>();

            using (var stream = arquivo.OpenReadStream())
            {
                IWorkbook wb = (ext == ".xlsx") ? new XSSFWorkbook(stream) : new HSSFWorkbook(stream);
                var sheet = wb.NumberOfSheets > 0 ? wb.GetSheetAt(0) : null;
                if (sheet == null)
                {
                    TempData["Erro"] = "Não encontrei planilhas no arquivo.";
                    return RedirectToAction(nameof(Index), new { mes, ano });
                }

                // Tenta detectar header: se a primeira linha tiver "Dia" em vez de número, começa na próxima
                var firstRowNum = sheet.FirstRowNum;
                var startRow = firstRowNum;
                var firstRow = sheet.GetRow(firstRowNum);
                if (firstRow != null)
                {
                    var cell0 = firstRow.GetCell(0);
                    if (!TryGetInt(cell0, out _))
                        startRow = firstRowNum + 1; // pular cabeçalho
                }

                for (int r = startRow; r <= sheet.LastRowNum; r++)
                {
                    var row = sheet.GetRow(r);
                    if (row == null) continue;

                    // Colunas: 0=Dia, 1=Hora, 2=Local, 3=Descrição, [4]=Transmissão, [5]=Mídia, [6]=Qtd Câmera/Som
                    if (!TryGetInt(row.GetCell(0), out int dia)) continue; // linha inválida

                    if (!TryGetHora(row.GetCell(1), out var hora))
                    {
                        // se hora vier vazia, ignora linha
                        continue;
                    }

                    var localNome = (row.GetCell(2)?.ToString() ?? "").Trim();
                    var descricao = (row.GetCell(3)?.ToString() ?? "").Trim();

                    bool temTrans = TryGetBool(row.GetCell(4), defaultValue: false);
                    bool temMidia = TryGetBool(row.GetCell(5), defaultValue: false);
                    int qtdCamSom = TryGetInt(row.GetCell(6), defaultValue: 1);
                    if (qtdCamSom < 0) qtdCamSom = 0; // sanity

                    // mapear local pelo nome, buscando também por parte do nome
                    var localMatch = locais.FirstOrDefault(l =>
                        l.Nome != null && l.Nome.Trim().Contains(localNome, StringComparison.OrdinalIgnoreCase));

                    int idLocal;
                    if (localMatch == null)
                    {
                        idLocal = locais.First().Id;
                        if (!string.IsNullOrWhiteSpace(localNome))
                            avisos.Add($"Linha {r + 1}: Local \"{localNome}\" não encontrado. Usado \"{locais.First().Nome}\".");
                    }
                    else
                    {
                        idLocal = localMatch.Id;
                    }

                    // deduplicar contra existentes
                    var exists = existentes.Any(m =>
                        m.Data.Year == ano &&
                        m.Data.Month == mes &&
                        m.Data.Day == dia &&
                        m.Data.TimeOfDay == hora &&
                        m.LocalId == idLocal &&
                        string.Equals(m.Descricao ?? "", descricao ?? "", StringComparison.OrdinalIgnoreCase));

                    if (exists) continue;

                    importados.Add(new MissaViewModel
                    {
                        Id = 0,
                        Ano = ano,
                        Mes = mes,
                        Dia = dia,
                        Hora = hora,
                        IdLocal = idLocal,
                        Descricao = descricao,
                        TemTransmissao = temTrans,
                        TemMidia = temMidia,
                        QuantidadeCameraSom = qtdCamSom
                    });
                }
            }

            // Recarrega a mesma tela com os existentes + importados para revisão
            var jaCadastradas = _context.Missas
                .Include(m => m.Local)
                .Where(m => m.Data.Month == mes && m.Data.Year == ano)
                .OrderBy(m => m.Data)
                .Select(m => new MissaViewModel
                {
                    Id = m.Id,
                    Ano = m.Data.Year,
                    Mes = m.Data.Month,
                    Dia = m.Data.Day,
                    Hora = m.Data.TimeOfDay,
                    IdLocal = m.LocalId,
                    Local = m.Local,
                    Descricao = m.Descricao,
                    TemTransmissao = m.TemTransmissao,
                    TemMidia = m.TemMidia,
                    QuantidadeCameraSom = m.QuantidadeCameraSom
                })
                .ToList();

            // Junta
            var modelo = jaCadastradas.Concat(importados)
                                      .OrderBy(m => m.Dia)
                                      .ThenBy(m => m.Hora)
                                      .ToList();

            ViewBag.Locais = locais;
            ViewBag.MesSelecionado = mes;
            ViewBag.AnoSelecionado = ano;

            if (importados.Count > 0)
                TempData["Sucesso"] = $"Importei {importados.Count} registro(s).";
            if (avisos.Any())
                TempData["Erro"] = string.Join("<br/>", avisos);

            return View(nameof(Index), modelo);
        }

        // ===== Helpers NPOI =====

        private static bool TryGetInt(ICell cell, out int value)
        {
            value = 0;
            if (cell == null) return false;

            try
            {
                if (cell.CellType == CellType.Numeric || cell.CellType == CellType.Formula)
                {
                    value = (int)cell.NumericCellValue;
                    return true;
                }
            }
            catch { /* fallback */ }

            var s = cell.ToString()?.Trim();
            return int.TryParse(s, out value);
        }

        private static int TryGetInt(ICell cell, int defaultValue)
            => TryGetInt(cell, out var v) ? v : defaultValue;

        private static bool TryGetBool(ICell cell, bool defaultValue)
        {
            if (cell == null) return defaultValue;

            if (cell.CellType == CellType.Boolean) return cell.BooleanCellValue;

            var s = cell.ToString()?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(s)) return defaultValue;

            if (s is "sim" or "s" or "yes" or "y" or "true" or "1" or "x") return true;
            if (s is "não" or "nao" or "n" or "no" or "false" or "0") return false;

            return defaultValue;
        }

        private static bool TryGetHora(ICell cell, out TimeSpan hora)
        {
            hora = default;
            if (cell == null) return false;

            // Se vier como data/hora numérica do Excel
            if ((cell.CellType == CellType.Numeric || cell.CellType == CellType.Formula) &&
                NPOI.SS.UserModel.DateUtil.IsCellDateFormatted(cell))
            {
                var dt = cell.DateCellValue;
                if (dt.HasValue) // Check if the nullable DateTime has a value
                {
                    hora = dt.Value.TimeOfDay; // Access the value and then the TimeOfDay property
                    return true;
                }
            }

            var raw = cell.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(raw)) return false;

            // Normaliza formatos: "18h30", "18 h 30", "18:30", "1830", "19h"
            raw = raw.Replace("H", "h").Replace(" ", "");
            raw = raw.Replace(".", ":").Replace("h", ":");

            // Se vier "1830" => "18:30"; "19" => "19:00"
            var digits = Regex.Replace(raw, "[^0-9]", "");
            if (digits.Length == 4 && !digits.Contains(":"))
                raw = digits.Insert(2, ":");
            else if (digits.Length == 2 && !digits.Contains(":"))
                raw = digits + ":00";
            else if (digits.Length == 1 && !digits.Contains(":"))
                raw = "0" + digits + ":00";

            // Tenta parsear
            if (TimeSpan.TryParse(raw, out hora)) return true;

            
            string[] formats = { "h\\:m", "hh\\:m", "h\\:mm", "hh\\:mm" };
            foreach (var f in formats)
            {
                if (TimeSpan.TryParseExact(raw, f, null, out hora))
                    return true;
            }

            return false;
        }


    }

}
