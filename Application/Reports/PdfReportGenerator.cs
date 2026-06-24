using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports;

public class PdfReportGenerator : IReportGenerator
{
    public ReportFormat Format => ReportFormat.Pdf;

    public byte[] Generate(IEnumerable<OrderReportRow> rows, decimal total)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Verdana));

                page.Header().Column(col =>
                {
                    col.Item().Text("Orders Report").FontSize(22).SemiBold().FontColor(Colors.Blue.Medium);
                    col.Item().Text($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm}").FontSize(10).Italic();
                });

                page.Content().PaddingVertical(15).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(4);
                        columns.RelativeColumn(5);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderCellStyle).Text("ID");
                        header.Cell().Element(HeaderCellStyle).Text("Customer");
                        header.Cell().Element(HeaderCellStyle).Text("Instrument");
                        header.Cell().Element(HeaderCellStyle).Text("Services");
                        header.Cell().Element(HeaderCellStyle).AlignRight().Text("Sum");
                    });

                    foreach (var row in rows)
                    {
                        table.Cell().Element(BodyCellStyle).Text(row.Id);
                        table.Cell().Element(BodyCellStyle).Text(row.Customer);
                        table.Cell().Element(BodyCellStyle).Text(row.Instrument);
                        table.Cell().Element(BodyCellStyle).Text(row.Services);
                        table.Cell().Element(BodyCellStyle).AlignRight().Text($"{row.Sum:0.00}");
                    }

                    table.Cell().ColumnSpan(4).Element(TotalCellStyle).AlignRight().Text("Total");
                    table.Cell().Element(TotalCellStyle).AlignRight().Text($"{total:0.00}");
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                });
            });
        }).GeneratePdf();
    }

    private static IContainer HeaderCellStyle(IContainer container) =>
        container.DefaultTextStyle(x => x.SemiBold())
            .PaddingVertical(8)
            .BorderBottom(1.5f)
            .BorderColor(Colors.Black);

    private static IContainer BodyCellStyle(IContainer container) =>
        container.PaddingVertical(6)
            .BorderBottom(0.5f)
            .BorderColor(Colors.Grey.Lighten2);

    private static IContainer TotalCellStyle(IContainer container) =>
        container.DefaultTextStyle(x => x.SemiBold().FontSize(12))
            .PaddingVertical(10)
            .BorderTop(1)
            .BorderColor(Colors.Black);
}
