using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class TrainConfiguration : IEntityTypeConfiguration<Train>
    {
        public void Configure(EntityTypeBuilder<Train> builder)
        {
            builder.Property(train => train.Id)
                   .UseIdentityColumn()
                   .IsRequired();

            builder.Property(train => train.Number)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.HasOne(train => train.Route)
                   .WithMany(route => route.Trains)
                   .HasForeignKey(train => train.RouteId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Trains_RouteId");

            builder.Property(train => train.ActionUser)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(train => train.CreatedAt)
                   .IsRequired();

            builder.Property(train => train.UpdatedAt)
                   .IsRequired();
        }
    }
}
