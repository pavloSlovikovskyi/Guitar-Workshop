using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.InstrumentPassports
{
    public record InstrumentPassportId(Guid Value)
    {
        public static InstrumentPassportId Empty() => new(Guid.Empty);
        public static InstrumentPassportId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
