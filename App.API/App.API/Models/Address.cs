namespace App.API.Models
{
    public record class Address
    {
        public Address(string city, string street, string postalCode, string number, string state, string complement)
        {
            City=city;
            Street=street;
            PostalCode=postalCode;
            Number=number;
            State=state;
            Complement=complement;
        }

        public string City { get; set; }

        public string Street { get; set; }

        public string PostalCode { get; set; }

        public string Number { get; set; }

        public string State { get; set; }

        public string Complement { get; set; }
    }
}
