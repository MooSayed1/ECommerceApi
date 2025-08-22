using Domain.Entities.OrderEntities;

namespace Persistance.Data.Configurations;

public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.Property(o=>o.Price).HasColumnType("decimal(18,3)");
    }
}