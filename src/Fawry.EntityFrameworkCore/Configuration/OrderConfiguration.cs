using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fawry.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;

namespace Fawry.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ConfigureByConvention();
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.UserId).IsRequired();
            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.OrderDate).IsRequired();
            builder.Property(x => x.Status).HasMaxLength(300);
            builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
            builder.HasOne(x => x.PaymentType)
                   .WithMany()
                   .HasForeignKey(x => x.PaymentTypeId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);
            builder.HasOne(x => x.CustomerAddress)
                   .WithMany()
                   .HasForeignKey(x => x.CustomerAddressId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => x.UserId);
        }
    }
}
