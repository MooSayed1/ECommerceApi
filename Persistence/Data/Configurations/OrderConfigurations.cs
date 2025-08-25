using Domain.Entities.OrderEntities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Persistance.Data.Configurations;

public class OrderConfigurations :  IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(o => o.ShippingAddress,p=>p.WithOwner()); // 1 to 1 relation total
        builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade); // to delete order items if the order is deleted
        builder.Property(o => o.PaymentStatus).HasConversion(paymentStatus => paymentStatus.ToString()
            ,s=> Enum.Parse<OrderPaymentStatus>(s));
        builder.HasOne(d=>d.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);
        builder.Property(o=>o.SubTotal).HasColumnType("decimal(18,3)");
    }
}