namespace App.API.Features
{
    public class CreateParking
    {
        public record Request : IRequest<CommandResult<Response>>
        {
            public string? Name { get; init; }

            public AddressType Address { get; init; } = new();

            public record AddressType
            {
                public string? City { get; init; }

                public string? Street { get; init; }

                public string? PostalCode { get; init; }

                public string? Number { get; init; }

                public string? State { get; init; }

                public string? Complement { get; init; }
            }
        }

        public record Response
        {
            public string Id { get; init; } = string.Empty;
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

        public class Handler : IRequestHandler<Request, CommandResult<Response>>
        {
            private readonly IValidator<Request> _validator;
            private readonly AppDbContext _db;

            public Handler(IValidator<Request> validator, AppDbContext db)
            {
                _validator = validator;
                _db = db;
            }

            public async Task<CommandResult<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return CommandResult<Response>.InvalidRequest(validationResult.GetMessages());

                var parking = new Parking(request.Name, new Address(
                    request.Address!.City!,
                    request.Address!.Street!,
                    request.Address!.PostalCode!,
                    request.Address!.Number!,
                    request.Address!.State!,
                    request.Address!.Complement!));

                await _db.AddAsync(parking);
                await _db.SaveChangesAsync(cancellationToken);

                return CommandResult<Response>.Success(new Response() { Id = parking.Id });
            }
        }
    }
}