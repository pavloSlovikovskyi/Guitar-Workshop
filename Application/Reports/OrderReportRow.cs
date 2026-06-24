namespace Application.Reports;

public record OrderReportRow(
    string Id,
    string Customer,
    string Instrument,
    string Services,
    decimal Sum
);
