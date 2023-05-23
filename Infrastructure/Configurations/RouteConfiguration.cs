using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.Property(route => route.Id)
                   .UseIdentityColumn()   
                   .IsRequired();

            builder.Property(route => route.DepartureTime)
                   .IsRequired();

            builder.Property(route => route.ArrivalTime)
                   .IsRequired();

            builder.Property(route => route.FromCity)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(route => route.ToCity)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(route => route.ActionUser)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(route => route.CreatedAt)
                   .IsRequired();

            builder.Property(route => route.UpdatedAt)
                   .IsRequired();
        }
    }
}
