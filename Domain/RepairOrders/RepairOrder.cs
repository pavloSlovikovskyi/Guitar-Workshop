using Domain.Enums;
using System;

namespace Domain.RepairOrders
{
    public class RepairOrder
    {
        public Guid Id { get; }
        public Guid InstrumentId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public RepairOrderStatus Status { get; set; }
        public string Notes { get; private set; }

        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; private set; }

        private RepairOrder(Guid id, Guid instrumentId, DateTime orderDate, RepairOrderStatus status, string notes, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            InstrumentId = instrumentId;
            OrderDate = orderDate;
            Status = status;
            Notes = notes;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static RepairOrder New(Guid id, Guid instrumentId, DateTime orderDate, RepairOrderStatus status, string notes)
        {
            return new RepairOrder(id, instrumentId, orderDate, status, notes, DateTime.UtcNow, null);
        }

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
    }
}
