using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Instruments
{
    public class Instrument
    {
        public Guid Id { get; }
        public string Model { get; private set; }
        public string SerialNumber { get; private set; }
        public DateTime RecieveDate { get; private set; }
        public InstrumentStatus Status { get; private set; }
        public Guid CustomerId { get; private set; }

        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; private set; }

        private Instrument(Guid id, string model, string serialNumber, DateTime recieveDate, InstrumentStatus status, Guid customerId, DateTime createdAt, DateTime? updatedAt)
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

        public static Instrument New(Guid id, string model, string serialNumber, DateTime recieveDate, InstrumentStatus status, Guid customerId)
        {
            return new Instrument(id, model, serialNumber, recieveDate, status, customerId, DateTime.UtcNow, null);
        }

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
