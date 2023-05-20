using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class TrainCarConfiguration : IEntityTypeConfiguration<TrainCar>
    {
        public void Configure(EntityTypeBuilder<TrainCar> builder)
        {
            builder.Property(car => car.Id)
                   .UseIdentityColumn()
                   .IsRequired();

            builder.Property(car => car.Number)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.HasOne(car => car.Train)
                   .WithMany(train => train.TrainCars)
                   .HasForeignKey(car => car.TrainId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_TrainCars_TrainId");

            builder.Property(car => car.ActionUser)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(car => car.CreatedAt)
                   .IsRequired();

            builder.Property(car => car.UpdatedAt)
                   .IsRequired();
        }
    }
}
