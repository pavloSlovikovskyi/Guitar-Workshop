using ClosedXML.Excel;

namespace Application.Reports;

public class ExcelReportGenerator : IReportGenerator
{
    public ReportFormat Format => ReportFormat.Excel;

    public byte[] Generate(IEnumerable<OrderReportRow> rows, decimal total)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Orders Report");

        sheet.Cell(1, 1).Value = "ID";
        sheet.Cell(1, 2).Value = "Customer";
        sheet.Cell(1, 3).Value = "Instrument";
        sheet.Cell(1, 4).Value = "Services";
        sheet.Cell(1, 5).Value = "Sum";

        var rowIndex = 2;
        foreach (var row in rows)
        {
            sheet.Cell(rowIndex, 1).Value = row.Id;
            sheet.Cell(rowIndex, 2).Value = row.Customer;
            sheet.Cell(rowIndex, 3).Value = row.Instrument;
            sheet.Cell(rowIndex, 4).Value = row.Services;
            sheet.Cell(rowIndex, 5).Value = row.Sum;
            rowIndex++;
        }

        sheet.Cell(rowIndex, 1).Value = "Total";
        sheet.Range(rowIndex, 1, rowIndex, 4).Merge();
        sheet.Cell(rowIndex, 5).Value = total;

        sheet.Row(1).Style.Font.Bold = true;
        sheet.Row(rowIndex).Style.Font.Bold = true;
        sheet.Column(5).Style.NumberFormat.Format = "0.00";
        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
