using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EscalasPascom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using A = DocumentFormat.OpenXml.Drawing;
using Pic = DocumentFormat.OpenXml.Drawing.Pictures;
using Wp = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using System.IO;
using System.Linq;

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

            // 🔹 Cria uma nova escala
            var escala = new Escala
            {
                Nome = $"Escala {mesSel}/{anoSel}",
                Mes = mesSel.ToString(),
                Ano = anoSel
            };

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
            var missas = _context.Missas.Include(m => m.Local).Include(m => m.Escalados).ThenInclude(e => e.Pessoa).Include(m => m.Escalados).ThenInclude(e => e.Papel).ToList();


            // 2. Crie um documento em memória
            using (MemoryStream memStream = new MemoryStream())
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(memStream, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                    Body body = mainPart.Document.AppendChild(new Body());
                   
                    Table table = new Table();
                    
                    TableProperties tblProps = new TableProperties(
                        new TableBorders(
                            new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }
                        )
                    );
                    table.AppendChild(tblProps);

                                        
                    TableRow headerRow = new TableRow();
                    string[] headers = { "Data", "Dia", "Horario", "Local", "Operador", "Câmera/Som", "Mídia" };
                    foreach (var header in headers)
                    {
                        TableCell cell = new TableCell();
                        cell.Append(new Paragraph(new Run(new Text(header))));
                        headerRow.Append(cell);
                    }
                    table.AppendChild(headerRow);

                    // 5. Preencher a tabela com os dados
                    foreach (var missa in missas)
                    {
                        TableRow dataRow = new TableRow();

                        var operadores = string.Join(", ", missa.Escalados.Where(e => e.Papel?.Descricao == "Operador").Select(e => e.Pessoa.Nome));
                        var cameraSom = string.Join(", ", missa.Escalados.Where(e => e.Papel?.Descricao == "Foto/Som").Select(e => e.Pessoa.Nome));
                        var midia = string.Join(", ", missa.Escalados.Where(e => e.Papel?.Descricao == "Mídia").Select(e => e.Pessoa.Nome));
                        var diaSemana = missa.Data.ToString("dddd", new System.Globalization.CultureInfo("pt-BR")).ToUpper();

                        dataRow.Append(new TableCell(new Paragraph(new Run(new Text(missa.Data.ToString("dd"))))));

                        // Célula 2: Dia e Descrição (ajustado)
                        TableCell diaDescricaoCell = new TableCell();

                        // Parágrafo para o Dia da Semana em negrito
                        Paragraph diaParagraph = new Paragraph(
                            new Run(new RunProperties(new Bold()), new Text(diaSemana))
                        );

                        // Parágrafo para a Descrição com fonte menor
                        Paragraph descricaoParagraph = new Paragraph(
                            new Run(new RunProperties(new FontSize() { Val = "16" }), new Text(missa.Descricao)) // Tamanho da fonte menor (16 em "points" é 8)
                        );

                        diaDescricaoCell.Append(diaParagraph, descricaoParagraph);
                        dataRow.Append(diaDescricaoCell);

                        //dataRow.Append(new TableCell(new Paragraph(new Run(new Text(diaSemana)))));
                        dataRow.Append(new TableCell(new Paragraph(new Run(new Text(missa.Data.ToString("HH:mm"))))));
                        dataRow.Append(new TableCell(new Paragraph(new Run(new Text(missa.Local?.Nome)))));
                        dataRow.Append(new TableCell(new Paragraph(new Run(new Text(operadores)))));
                        dataRow.Append(new TableCell(new Paragraph(new Run(new Text(cameraSom)))));
                        dataRow.Append(new TableCell(new Paragraph(new Run(new Text(midia)))));

                        table.AppendChild(dataRow);
                    }
                    body.AppendChild(table);

                    mainPart.Document.Save();
                }

                // 6. Retorne o arquivo para o navegador
                memStream.Position = 0;
                return File(memStream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Escala.docx");
            }
        }
        public IActionResult ExportarPdf(int mes, int ano)
        {
            var missas = _context.Missas
                .Include(m => m.Local)
                .Include(m => m.Escalados)
                    .ThenInclude(e => e.Pessoa)
                .Include(m => m.Escalados)
                    .ThenInclude(e => e.Papel)
                .Where(m => m.Data.Month == mes && m.Data.Year == ano) // filtra por mês e ano
                .ToList();
            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(25);
                    page.Header().Row(row =>
                    {
                        row.RelativeColumn().Container().Height(60).Image("wwwroot/lib/imagens/logopascom.png");

                        // Coluna para o texto central
                        row.RelativeColumn(2).Column(column =>
                        {
                            column.Item().AlignCenter().Text("PARÓQUIA SÃO JOAQUIM").FontSize(15).Bold();
                            column.Item().AlignCenter().Text("DIOCESE DE GUAXUPÉ").FontSize(12).Bold();
                            column.Item().AlignCenter().Text("ESCALA PASTORAL DA COMUNICAÇÃO - PASCOM").FontSize(12).Bold();                            
                        });

                        row.RelativeColumn().Container().Height(70).Image("wwwroot/lib/imagens/logoparoquia.png");
                    });

                    page.Content().Table(table =>
                    {
                        // Cabeçalho da tabela
                        table.ColumnsDefinition(columns =>
                        {   
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(3);
                        });

                        table.Cell().Row(1).ColumnSpan(7).Container().Border(1).Padding(5).AlignCenter().AlignMiddle()
                            .Text("Celebrações Eucarísticas").Bold();

                        table.Cell().Row(2).Column(1).Container().Border(1).Padding(5).AlignCenter().AlignMiddle().Text("Dia");
                        table.Cell().Row(2).Column(2).Container().Border(1).Padding(5).AlignCenter().AlignMiddle().Text("Descrição");
                        table.Cell().Row(2).Column(3).Container().Border(1).Padding(5).AlignCenter().AlignMiddle().Text("Horário");
                        table.Cell().Row(2).Column(4).Container().Border(1).Padding(5).AlignCenter().AlignMiddle().Text("Local");
                        table.Cell().Row(2).Column(5).Container().Border(1).Padding(5).AlignCenter().AlignMiddle().Text("Operador");
                        table.Cell().Row(2).Column(6).Container().Border(1).Padding(5).AlignCenter().AlignMiddle().Text("Câmera/Som");
                        table.Cell().Row(2).Column(7).Container().Border(1).Padding(5).AlignCenter().AlignMiddle().Text("Mídia");

                        uint rowIndex = 3;
                        foreach (var missa in missas)
                        {
                            table.Cell().Row(rowIndex).Column(1).Container().Border(1).Padding(5).AlignCenter().AlignMiddle()
                                .Text(missa.Data.ToString("dd"));

                            table.Cell().Row(rowIndex).Column(2).Container().Border(1).Padding(5).AlignCenter().AlignMiddle().Column(column =>
                            {
                                column.Item().Text(missa.Data.ToString("dddd", new System.Globalization.CultureInfo("pt-BR")).ToUpper()).Bold();
                                column.Item().Text(missa.Descricao).FontSize(8);
                            });

                            table.Cell().Row(rowIndex).Column(3).Container().Border(1).Padding(5).AlignCenter().AlignMiddle()
                                .Text(missa.Data.ToString("HH:mm") + "h");

                            table.Cell().Row(rowIndex).Column(4).Container().Border(1).Padding(5).AlignCenter().AlignMiddle()
                                .Text(missa.Local?.Nome);

                            table.Cell().Row(rowIndex).Column(5).Container().Border(1).Padding(5).AlignCenter().AlignMiddle()
                                .Text(string.Join(", ", missa.Escalados.Where(e => e.Papel?.Descricao == "Operador").Select(e => e.Pessoa.Nome)));

                            table.Cell().Row(rowIndex).Column(6).Container().Border(1).Padding(5).AlignCenter().AlignMiddle()
                                .Text(string.Join(", ", missa.Escalados.Where(e => e.Papel?.Descricao == "Foto/Som").Select(e => e.Pessoa.Nome)));

                            table.Cell().Row(rowIndex).Column(7).Container().Border(1).Padding(5).AlignCenter().AlignMiddle()
                                .Text(string.Join(", ", missa.Escalados.Where(e => e.Papel?.Descricao == "Mídia").Select(e => e.Pessoa.Nome)));

                            rowIndex++;
                        }

                    });
                });
            });

            byte[] pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", "Escala.pdf");
        }

    }
}
