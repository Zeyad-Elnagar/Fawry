using Fawry.CustomerAddresses;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;

namespace Fawry.Configuration
{
    internal class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.ConfigureByConvention();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(50);
            builder.Property(x => x.AddressLine).IsRequired().HasMaxLength(250);
            builder.Property(x => x.UserId).IsRequired();
            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => x.UserId);
        }
    }
}
