using Domain.Instruments;

namespace Domain.Customers
{
    public class Customer
    {
        public CustomerId Id { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; private set; }

        public ICollection<Instrument> Instruments { get; private set; } = new List<Instrument>();

        private Customer(
            CustomerId id,
            string firstName,
            string lastName,
            string phoneNumber,
            string email,
            DateTime createdAt,
            DateTime? updatedAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static Customer New(
            CustomerId id,
            string firstName,
            string lastName,
            string phoneNumber,
            string email)
            => new(id, firstName, lastName, phoneNumber, email, DateTime.UtcNow, null);

        public void UpdateDetails(string firstName, string lastName, string phoneNumber, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
