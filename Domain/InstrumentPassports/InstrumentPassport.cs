using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.InstrumentPassports
{
    public class InstrumentPassport
    {
        public Guid InstrumentId { get; }
        public DateTime IssueDate { get; private set; }
        public string Details { get; private set; }

        public InstrumentPassport(Guid instrumentId, DateTime issueDate, string details)
        {
            InstrumentId = instrumentId;
            IssueDate = issueDate;
            Details = details;
        }

        public void UpdateDetails(DateTime issueDate, string details)
        {
            IssueDate = issueDate;
            Details = details;
        }
    }

}
