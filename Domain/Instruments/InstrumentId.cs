using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Instruments
{
    public record InstrumentId(Guid Value)
    {
        public static InstrumentId Empty() => new(Guid.Empty);
        public static InstrumentId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();

        public static implicit operator Guid(InstrumentId id) => id.Value;

        public static explicit operator InstrumentId(Guid value) => new(value);
    }
}
