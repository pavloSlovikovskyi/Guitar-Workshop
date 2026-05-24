namespace Application.Reports;

public interface IReportGenerator
{
    ReportFormat Format { get; }
    byte[] Generate(IEnumerable<OrderReportRow> rows, decimal total);
}
