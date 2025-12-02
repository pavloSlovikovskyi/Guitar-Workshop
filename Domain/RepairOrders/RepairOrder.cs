using Domain.Instruments;
using Domain.Enums;

namespace Domain.RepairOrders
{
    public class RepairOrder
    {
        public RepairOrderId Id { get; }
        public InstrumentId InstrumentId { get; private set; }
        public Instrument Instrument { get; private set; }   // навігація

        public DateTime OrderDate { get; private set; }
        public RepairOrderStatus Status { get; private set; }
        public string Notes { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; private set; }

        private RepairOrder(
            RepairOrderId id,
            InstrumentId instrumentId,
            DateTime orderDate,
            RepairOrderStatus status,
            string notes,
            DateTime createdAt,
            DateTime? updatedAt)
        {
            Id = id;
            InstrumentId = instrumentId;
            OrderDate = orderDate;
            Status = status;
            Notes = notes;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static RepairOrder New(
            RepairOrderId id,
            InstrumentId instrumentId,
            DateTime orderDate,
            RepairOrderStatus status,
            string notes)
            => new(id, instrumentId, orderDate, status, notes, DateTime.UtcNow, null);

        public void UpdateDetails(DateTime orderDate, RepairOrderStatus status, string notes)
        {
            OrderDate = orderDate;
            Status = status;
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete(string notes)
        {
            Status = RepairOrderStatus.Completed;
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            Status = RepairOrderStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(RepairOrderStatus newStatus)
        {
            Status = newStatus;
        }
    }
}
