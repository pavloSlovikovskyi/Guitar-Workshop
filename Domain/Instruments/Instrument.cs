using Domain.Customers;
using Domain.Enums;
using Domain.InstrumentPassports;

namespace Domain.Instruments
{
    public class Instrument
    {
        public InstrumentId Id { get; }
        public string Model { get; private set; }
        public string SerialNumber { get; private set; }
        public DateTime RecieveDate { get; private set; }
        public InstrumentStatus Status { get; private set; }

        public CustomerId CustomerId { get; private set; }
        public Customer Customer { get; private set; }

        public InstrumentPassport InstrumentPassport { get; private set; }

        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; private set; }

        private Instrument(
            InstrumentId id,
            string model,
            string serialNumber,
            DateTime recieveDate,
            InstrumentStatus status,
            CustomerId customerId,
            DateTime createdAt,
            DateTime? updatedAt)
        {
            Id = id;
            Model = model;
            SerialNumber = serialNumber;
            RecieveDate = recieveDate;
            Status = status;
            CustomerId = customerId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static Instrument New(
            InstrumentId id,
            string model,
            string serialNumber,
            DateTime recieveDate,
            InstrumentStatus status,
            CustomerId customerId)
            => new(id, model, serialNumber, recieveDate, status, customerId, DateTime.UtcNow, null);

        public void UpdateDetails(string model, string serialNumber, DateTime recieveDate)
        {
            Model = model;
            SerialNumber = serialNumber;
            RecieveDate = recieveDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(InstrumentStatus status)
        {
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
