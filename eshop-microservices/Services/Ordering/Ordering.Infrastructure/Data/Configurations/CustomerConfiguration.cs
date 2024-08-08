using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;
internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        //Has Conversion : conversion between a property's data type in the application and its representation in the database. 
        builder.Property(c => c.Id).HasConversion(
            customerId => customerId.Value, // Applicationdan db ye nasil tutulacagi
            dbId => CustomerId.Of(dbId)); // DBden cekildiiiginde CustomerId objesine nasil donusecegi

        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(255);

        builder.HasIndex(c => c.Email).IsUnique();

    }
}
