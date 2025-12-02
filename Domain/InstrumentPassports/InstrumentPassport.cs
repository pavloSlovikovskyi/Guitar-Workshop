using Domain.Instruments;

namespace Domain.InstrumentPassports
{
    public class InstrumentPassport
    {
        public InstrumentPassportId Id { get; }
        public InstrumentId InstrumentId { get; }
        public Instrument Instrument { get; private set; }

        public DateTime IssueDate { get; private set; }
        public string Details { get; private set; }

        private InstrumentPassport(
            InstrumentPassportId id,
            InstrumentId instrumentId,
            DateTime issueDate,
            string details)
        {
            Id = id;
            InstrumentId = instrumentId;
            IssueDate = issueDate;
            Details = details;
        }

        public static InstrumentPassport New(
            InstrumentPassportId id,
            InstrumentId instrumentId,
            DateTime issueDate,
            string details)
            => new(id, instrumentId, issueDate, details);

        public void UpdateDetails(DateTime issueDate, string details)
        {
            IssueDate = issueDate;
            Details = details;
        }
    }
}
