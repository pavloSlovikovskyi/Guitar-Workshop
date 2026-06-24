using Application.Common;
using Application.Common.Interfaces.Queries;
using MediatR;

namespace Application.Reports;

public class GetOrdersReportHandler : IRequestHandler<GetOrdersReportQuery, Result<byte[]>>
{
    private readonly IRepairOrderQueries _queries;
    private readonly IEnumerable<IReportGenerator> _generators;

    public GetOrdersReportHandler(
        IRepairOrderQueries queries,
        IEnumerable<IReportGenerator> generators)
    {
        _queries = queries;
        _generators = generators;
    }

    public async Task<Result<byte[]>> Handle(
        GetOrdersReportQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _queries.GetAllWithIncludesAsync(cancellationToken);

        var rows = orders.Select(o => new OrderReportRow(
            Id: o.Id.Value.ToString(),
            Customer: o.Instrument?.Customer is null
                ? "Unknown"
                : $"{o.Instrument.Customer.FirstName} {o.Instrument.Customer.LastName}",
            Instrument: o.Instrument is null
                ? "Unknown"
                : $"{o.Instrument.Model} ({o.Instrument.SerialNumber})",
            Services: string.Join(", ", o.RepairOrderServiceTypes.Select(x => x.ServiceType.Title)),
            Sum: o.RepairOrderServiceTypes.Sum(x => x.ServiceType.Price)
        )).ToList();

        var total = rows.Sum(r => r.Sum);
        var generator = _generators.FirstOrDefault(g => g.Format == request.Format);
        if (generator is null)
            return Result<byte[]>.Failure($"Report generator for format '{request.Format}' is not registered.");

        var content = generator.Generate(rows, total);
        return Result<byte[]>.Success(content);
    }
}
