using Microsoft.EntityFrameworkCore;

namespace App.API;

public static partial class Program
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase(nameof(AppDbContext)));
        services.AddMediatR(typeof(CreateParking.Handler));
        services.AddValidatorsFromAssemblyContaining<CreateParking.Validator>();
    }

    public static void AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.CustomSchemaIds(x =>
            {
                var value = x.FullName?
                    .Replace(x.Namespace ?? string.Empty, string.Empty)
                    .Replace("+", ".")
                    .Substring(1);

                return value;
            });
        });
    }
}

