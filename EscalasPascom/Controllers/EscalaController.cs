using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EscalasPascom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.XWPF.UserModel;
using System.Globalization;


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

            // Busca escala já criada no banco
            var escala = _context.Escalas
                .Include(e => e.Missas)
                    .ThenInclude(m => m.Local)
                .Include(e => e.Missas)
                    .ThenInclude(m => m.Escalados)
                        .ThenInclude(e => e.Pessoa)
                .Include(e => e.Missas)
                    .ThenInclude(m => m.Escalados)
                        .ThenInclude(e => e.Papel)
                .FirstOrDefault(e => e.Mes == mesSel.ToString() && e.Ano == anoSel);

            bool existeEscala = escala != null;

            List<Missa> missas;
            if (existeEscala)
            {
                missas = escala.Missas
                    .OrderBy(m => m.Data)
                    .ToList();
            }
            else
            {
                // Carrega só as missas sem escala
                missas = _context.Missas
                    .Include(m => m.Local)
                    .Include(m => m.Escalados)
                        .ThenInclude(e => e.Pessoa)
                    .Include(m => m.Escalados)
                        .ThenInclude(e => e.Papel)
                    .Where(m => m.Data.Month == mesSel && m.Data.Year == anoSel)
                    .OrderBy(m => m.Data)
                    .ToList();
            }

            ViewBag.MesSelecionado = mesSel;
            ViewBag.AnoSelecionado = anoSel;
            ViewBag.Sorteado = existeEscala; 

            return View(missas);
        }



        [HttpPost]
        public IActionResult Sortear(int? mes, int? ano)
        {
            int mesSel = mes ?? DateTime.Now.Month;
            int anoSel = ano ?? DateTime.Now.Year;
            
            var escala = new Escala
            {
                Nome = $"Escala {mesSel}/{anoSel}",
                Mes = mesSel.ToString(),
                Ano = anoSel
            };
            
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
                missa.Escala = escala; // 🔹 vincula missa à escala
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

        
        public IActionResult ExportarEscala(int mes, int ano)
        {
            var escala = _context.Escalas
                .Include(e => e.Missas)
                    .ThenInclude(m => m.Local)
                .Include(e => e.Missas)
                    .ThenInclude(m => m.Escalados)
                        .ThenInclude(e => e.Pessoa)
                .Include(e => e.Missas)
                    .ThenInclude(m => m.Escalados)
                        .ThenInclude(e => e.Papel)
                .FirstOrDefault(e => e.Mes == mes.ToString() && e.Ano == ano);

            if (escala == null)
            {
                return NotFound("Escala não encontrada.");
            }
            return View(escala);

        }

        public IActionResult ExportarWord(int mes, int ano)
        {
            
            var missas = _context.Missas
                .Include(m => m.Local)
                .Include(m => m.Escalados)
                    .ThenInclude(e => e.Pessoa)
                .Include(m => m.Escalados)
                    .ThenInclude(e => e.Papel)
                .Where(m => m.Data.Month == mes && m.Data.Year == ano)
                .ToList();

            
            using (MemoryStream memStream = new MemoryStream())
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(memStream, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    // 3. Definição de Estilos
                    RunProperties defaultRunProps = new RunProperties(
                        new RunFonts() { Ascii = "Consolas", HighAnsi = "Consolas" },
                        new FontSize() { Val = "22" } // 11pt
                    );

                    // 4. Cabeçalho do Documento
                    Paragraph header = new Paragraph(
                        new ParagraphProperties(
                            new Justification() { Val = JustificationValues.Center }
                        ),
                        // CORREÇÃO: Usar 'new DocumentFormat.OpenXml.Wordprocessing.Text()'
                        new Run(new RunProperties(
                            new RunFonts() { Ascii = "Consolas", HighAnsi = "Consolas" },
                            new FontSize() { Val = "26" }, // 13pt
                            new Bold()
                        ), new DocumentFormat.OpenXml.Wordprocessing.Text($"ESCALA {mes}/{ano}"))
                    );
                    body.AppendChild(header);
                    body.AppendChild(new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text("")))); // espaço

                    // 5. Estrutura da Tabela
                    Table table = new Table();

                    // AJUSTE: TableWidth para "10000" (100% da largura)
                    TableProperties tblProps = new TableProperties(
                        new TableWidth() { Type = TableWidthUnitValues.Pct, Width = "10000" },
                        new TableBorders(
                            new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }
                        )
                    );
                    table.AppendChild(tblProps);

                    // 6. Cabeçalho da Tabela
                    TableRow headerRow = new TableRow();
                    string[] headers = { "Dia/Descrição", "Horário", "Local", "Operador", "Câmera/Som", "Mídia" };
                    foreach (var h in headers)
                    {
                        TableCell cell = new TableCell(
                            new TableCellProperties(
                                // Centralização Vertical
                                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
                            ),
                            new Paragraph(
                                new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), // Centralização Horizontal
                                new Run(new RunProperties(
                                    new RunFonts() { Ascii = "Consolas", HighAnsi = "Consolas" },
                                    new FontSize() { Val = "26" },
                                    new Bold()
                                ), new DocumentFormat.OpenXml.Wordprocessing.Text(h))
                            )
                        );
                        headerRow.Append(cell);
                    }
                    table.AppendChild(headerRow);

                    // 7. Função de Ajuda para Células de Dados (Centralização e Preenchimento)
                    TableCell CreateCenteredCell(string text)
                    {
                        // Se o texto for vazio, use "======"
                        string content = string.IsNullOrEmpty(text) ? "======" : text;

                        return new TableCell(
                            new TableCellProperties(
                                // Centralização Vertical
                                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
                            ),
                            new Paragraph(
                                // Centralização Horizontal
                                new ParagraphProperties(new Justification() { Val = JustificationValues.Center }),
                                new Run(defaultRunProps.CloneNode(true) as RunProperties, new DocumentFormat.OpenXml.Wordprocessing.Text(content))
                            )
                        );
                    }

                    // 8. Dados da Tabela
                    foreach (var missa in missas)
                    {
                        TableRow dataRow = new TableRow();

                        var operadores = string.Join(", ", missa.Escalados.Where(e => e.Papel?.Descricao == "Operador").Select(e => e.Pessoa.Nome));
                        var cameraSom = string.Join(", ", missa.Escalados.Where(e => e.Papel?.Descricao == "Foto/Som").Select(e => e.Pessoa.Nome));
                        var midia = string.Join(", ", missa.Escalados.Where(e => e.Papel?.Descricao == "Mídia").Select(e => e.Pessoa.Nome));
                        var diaSemana = missa.Data.ToString("dddd", new CultureInfo("pt-BR")).ToUpper();

                        // Célula Dia/Descrição (Ligeiramente diferente devido ao Break)
                        TableCell diaDescricaoCell = new TableCell(
                            new TableCellProperties(
                                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center } // Centralização Vertical
                            ),
                            new Paragraph(
                                new ParagraphProperties(
                                    new Justification() { Val = JustificationValues.Center }, // Centralização Horizontal
                                    new SpacingBetweenLines() { Line = "200" }
                                ),
                                new Run(defaultRunProps.CloneNode(true) as RunProperties, new DocumentFormat.OpenXml.Wordprocessing.Text(diaSemana)),
                                new Break(),
                                new Run(defaultRunProps.CloneNode(true) as RunProperties, new DocumentFormat.OpenXml.Wordprocessing.Text(missa.Descricao))
                            )
                        );
                        dataRow.Append(diaDescricaoCell);

                        // Demais Células usando a função de ajuda
                        dataRow.Append(CreateCenteredCell(missa.Data.ToString("HH:mm")));
                        dataRow.Append(CreateCenteredCell(missa.Local?.Nome));
                        dataRow.Append(CreateCenteredCell(operadores));
                        dataRow.Append(CreateCenteredCell(cameraSom));
                        dataRow.Append(CreateCenteredCell(midia));

                        table.AppendChild(dataRow);
                    }

                    body.AppendChild(table);
                    mainPart.Document.Save();
                }

                // 9. Retorno do Arquivo
                memStream.Position = 0;
                return File(memStream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"Escala_{mes}_{ano}.docx");
            }
        }



        public async Task<IActionResult> ExportarHtml(int mes, int ano)
        {
            var escala = _context.Escalas
                .Include(e => e.Missas)
                    .ThenInclude(m => m.Local)
                .Include(e => e.Missas)
                    .ThenInclude(m => m.Escalados)
                        .ThenInclude(e => e.Pessoa)
                .Include(e => e.Missas)
                    .ThenInclude(m => m.Escalados)
                        .ThenInclude(e => e.Papel)
                .FirstOrDefault(e => e.Mes == mes.ToString() && e.Ano == ano);

            if (escala == null)
                return NotFound("Escala não encontrada.");

            Response.Headers.Add("Content-Disposition", "inline; filename=Escala.html");
            Response.ContentType = "text/html";
            return View("ExportarEscalaPdf", escala);
        }


    }
}
