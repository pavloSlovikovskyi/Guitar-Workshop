using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ServiceTypes
{
    public record ServiceTypeId(Guid Value)
    {
        public static ServiceTypeId Empty() => new(Guid.Empty);
        public static ServiceTypeId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
        public static implicit operator Guid(ServiceTypeId id) => id.Value;
        public static explicit operator ServiceTypeId(Guid value) => new ServiceTypeId(value);
    }
}
