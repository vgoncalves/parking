using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.API.Features;

public class GetParking
{
    public record Request : IRequest<CommandResult<Response>>
    {
        public string? Id { get; set; }
    }

    public record Response
    {
        public string Name { get; set; }
    }

    public class Handler : IRequestHandler<Request, CommandResult<Response>>
    {
        private readonly AppDbContext _appDbContext;

        public Handler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<CommandResult<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            var parking = await _appDbContext.Parkings
                .Where(x => x.Id == request.Id)
                .Select(x => new Response() { Name = x.Name })
                .FirstOrDefaultAsync(cancellationToken);

            return CommandResult<Response>.Success(parking);
        }
    }
}