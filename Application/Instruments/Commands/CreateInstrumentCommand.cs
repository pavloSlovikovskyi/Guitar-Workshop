using Domain.Enums;
using Domain.Instruments;
using Domain.Customers;
using MediatR;
using Application.Common;

namespace Application.Instruments.Commands
{
    public record CreateInstrumentCommand(
        string Model,
        string SerialNumber,
        DateTime RecieveDate,
        InstrumentStatus Status,
        CustomerId CustomerId 
    ) : IRequest<Result<InstrumentId>>;
}
