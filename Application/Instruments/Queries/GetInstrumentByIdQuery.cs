using Application.Common;
using Domain.Instruments;
using MediatR;
using System;

namespace Application.Instruments.Queries;

public record GetInstrumentByIdQuery(Guid Id) : IRequest<Result<Instrument>>;
