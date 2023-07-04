using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class PlaceConfiguration : IEntityTypeConfiguration<Place>
    {
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.Property(place => place.Id)
                   .UseIdentityColumn()
                   .IsRequired();

            builder.Property(place => place.Price)
                   .IsRequired();

            builder.Property(place => place.Number)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(place => place.PlaceType)
                   .IsRequired();

            builder.Property(place => place.IsAvaliable)
                   .IsRequired();
        
            builder.HasOne(place => place.TrainCar)
                   .WithMany(car => car.Places)
                   .HasForeignKey(place => place.TrainCarId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Places_TrainCarId");

            builder.Property(place => place.ActionUser)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(place => place.CreatedAt)
                   .IsRequired();

            builder.Property(place => place.UpdatedAt)
                   .IsRequired();
        }
    }
}
