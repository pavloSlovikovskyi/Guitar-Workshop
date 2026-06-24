using Application.Common;
using Application.Reports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = AppRoles.Master)]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrdersReport(
        [FromQuery] ReportFormat format,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrdersReportQuery(format), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        var (mime, extension) = GetFormatMeta(format);
        var fileName = $"orders-report-{DateTime.UtcNow:yyyyMMddHHmmss}.{extension}";
        return File(result.Value!, mime, fileName);
    }

    private static (string Mime, string Extension) GetFormatMeta(ReportFormat format)
    {
        return format switch
        {
            ReportFormat.Excel => 
                ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xlsx"),
            
            ReportFormat.Word => 
                ("application/pdf", "pdf"), 
                
            _ => ("application/pdf", "pdf")
        };
    }
}
