using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Application.Reports;

public class WordReportGenerator : IReportGenerator
{
    public ReportFormat Format => ReportFormat.Word;

    public byte[] Generate(IEnumerable<OrderReportRow> rows, decimal total)
    {
        using var stream = new MemoryStream();
        using var document = DocX.Create(stream);

        document.InsertParagraph("Orders Report")
            .FontSize(18)
            .Bold();

        document.InsertParagraph($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm}")
            .FontSize(10)
            .Italic();

        var rowList = rows.ToList();
        var table = document.AddTable(rowList.Count + 2, 5);
        table.Design = TableDesign.TableGrid;

        table.Rows[0].Cells[0].Paragraphs[0].Append("ID").Bold();
        table.Rows[0].Cells[1].Paragraphs[0].Append("Customer").Bold();
        table.Rows[0].Cells[2].Paragraphs[0].Append("Instrument").Bold();
        table.Rows[0].Cells[3].Paragraphs[0].Append("Services").Bold();
        table.Rows[0].Cells[4].Paragraphs[0].Append("Sum").Bold();

        for (var i = 0; i < rowList.Count; i++)
        {
            var rowIndex = i + 1;
            var row = rowList[i];
            table.Rows[rowIndex].Cells[0].Paragraphs[0].Append(row.Id);
            table.Rows[rowIndex].Cells[1].Paragraphs[0].Append(row.Customer);
            table.Rows[rowIndex].Cells[2].Paragraphs[0].Append(row.Instrument);
            table.Rows[rowIndex].Cells[3].Paragraphs[0].Append(row.Services);
            table.Rows[rowIndex].Cells[4].Paragraphs[0].Append($"{row.Sum:0.00}");
        }

        var totalRowIndex = rowList.Count + 1;
        table.Rows[totalRowIndex].Cells[0].Paragraphs[0].Append("Total").Bold();
        table.Rows[totalRowIndex].MergeCells(0, 3);
        table.Rows[totalRowIndex].Cells[4].Paragraphs[0].Append($"{total:0.00}").Bold();

        document.InsertTable(table);

        document.Save();
        stream.Position = 0;
        return stream.ToArray();
    }
}
