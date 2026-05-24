using Application.Common;
using MediatR;

namespace Application.Reports;

public record GetOrdersReportQuery(ReportFormat Format) : IRequest<Result<byte[]>>;
