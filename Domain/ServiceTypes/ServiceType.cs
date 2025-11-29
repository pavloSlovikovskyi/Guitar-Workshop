using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ServiceTypes
{
    public class ServiceType
    {
        public Guid Id { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }


        private ServiceType(Guid id, string title, string description, decimal price)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
        }

        public static ServiceType New(Guid id, string title, string description, decimal price)
        {
            return new ServiceType(id, title, description, price);
        }

        public void UpdateDetails(string title, string description, decimal price)
        {
            Title = title;
            Description = description;
            Price = price;
        }
    }

}
