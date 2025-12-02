using System;

namespace API.Dtos
{
    public record CreateInstrumentPassportRequest(
        Guid InstrumentId,
        DateTime IssueDate,
        string Details
    );

    public record UpdateInstrumentPassportRequest(
        DateTime IssueDate,
        string Details
    );

    public record InstrumentPassportResponse(
        Guid Id,
        Guid InstrumentId,
        DateTime IssueDate,
        string Details
    )
    {
        public static InstrumentPassportResponse FromDomainModel(Domain.InstrumentPassports.InstrumentPassport model)
            => new(
                model.Id.Value,
                model.InstrumentId.Value,
                model.IssueDate,
                model.Details
            );
    }
}
