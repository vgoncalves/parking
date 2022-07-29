using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.API.Data.TypeConfig
{
    public class ParkingConfig : IEntityTypeConfiguration<Parking>
    {
        public void Configure(EntityTypeBuilder<Parking> builder)
        {
            builder.OwnsOne(x => x.Address);
        }
    }
}
