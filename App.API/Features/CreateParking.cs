

namespace App.API.Features
{
    public class CreateParking
    {
        public record Request : IRequest<CommandResult<Response>>
        {
            public Request(string? name, AddressType? address)
            {
                Name=name;
                Address=address;
            }

            public string? Name { get; set; }

            public AddressType? Address { get; set; }

            public record AddressType
            {
                public AddressType(string? postalCode, string? state, string? city, string? street, string? number, string? complement)
                {
                    City=city;
                    Street=street;
                    PostalCode=postalCode;
                    Number=number;
                    State=state;
                    Complement=complement;
                }

                public string? City { get; set; }

                public string? Street { get; set; }

                public string? PostalCode { get; set; }

                public string? Number { get; set; }

                public string? State { get; set; }

                public string? Complement { get; set; }
            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();

                RuleFor(x => x.Address!.Number).NotEmpty();
                RuleFor(x => x.Address!.City).NotEmpty();
                RuleFor(x => x.Address!.PostalCode).NotEmpty();
                RuleFor(x => x.Address!.Complement).NotEmpty();
                RuleFor(x => x.Address!.State).NotEmpty();
                RuleFor(x => x.Address!.Street).NotEmpty();
            }
        }

        public record Response
        {
            public Response(string id)
            {
                Id=id;
            }

            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Request, CommandResult<Response>>
        {
            private readonly IValidator<Request> _validator;
            private readonly AppDbContext _db;

            public Handler(IValidator<Request> validator, AppDbContext db)
            {
                _validator=validator;
                _db=db;
            }

            public async Task<CommandResult<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                    return CommandResult<Response>.Fail(validationResult.GetMessages());

                var newParking = new Parking(request.Name!, new Address(
                    request.Address!.City!,
                    request.Address!.Street!,
                    request.Address!.PostalCode!,
                    request.Address!.Number!,
                    request.Address!.State!,
                    request.Address!.Complement!));

                await _db.Parkings.AddAsync(newParking);
                await _db.SaveChangesAsync();

                return CommandResult<Response>.Success(new Response(newParking.Id));
            }
        }
    }
}
