using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MassTransitDemo.SagaDemo.Saga
{
    public class ShoppingCartMap : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.CurrentState).IsRequired().HasMaxLength(50);
        }
    }
}