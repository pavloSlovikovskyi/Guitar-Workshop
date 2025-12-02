using Domain.RepairOrdersServiceTypes;

namespace Domain.ServiceTypes
{
    public class ServiceType
    {
        public ServiceTypeId Id { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }

        public ICollection<RepairOrderServiceType> RepairOrderLinks { get; private set; } =
            new List<RepairOrderServiceType>();

        private ServiceType(ServiceTypeId id, string title, string description, decimal price)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
        }

        public static ServiceType New(ServiceTypeId id, string title, string description, decimal price)
            => new(id, title, description, price);

        public void UpdateDetails(string title, string description, decimal price)
        {
            Title = title;
            Description = description;
            Price = price;
        }
    }
}
