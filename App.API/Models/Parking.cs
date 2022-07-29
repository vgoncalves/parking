#nullable disable

namespace App.API.Models
{
    public record Parking
    {
        public Parking(string name, Address address)
        {
            Name=name;
            Address=address;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public Address Address { get; set; }
    }
}
