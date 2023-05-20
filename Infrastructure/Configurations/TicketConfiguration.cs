using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.Property(ticket => ticket.Id)
                   .UseIdentityColumn()
                   .IsRequired();

            builder.Property(ticket => ticket.BookDate)
                   .IsRequired();

            builder.HasOne(ticket => ticket.Place)
                   .WithMany(place => place.Tickets)
                   .HasForeignKey(ticket => ticket.PlaceId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Tickets_PlaceId");

            builder.Property(ticket => ticket.ActionUser)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(ticket => ticket.CreatedAt)
                   .IsRequired();

            builder.Property(ticket => ticket.UpdatedAt)
                   .IsRequired();
        }
    }
}
