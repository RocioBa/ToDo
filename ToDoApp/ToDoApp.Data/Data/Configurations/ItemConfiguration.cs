

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using ToDoApp.Data.Data.Models;

namespace ToDoApp.Data.Data.Configurations;
internal class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title).HasColumnType("varchar")
                                      .HasMaxLength(100)
                                      .IsRequired();

        builder.Property(p => p.Description).HasColumnType("varchar")
                                            .HasMaxLength(500)
                                            .IsRequired(false);

        builder.Property(p => p.RegisterDate).HasColumnType("datetime")
                                             .HasDefaultValueSql("GETDATE()")
                                             .IsRequired();

        builder.Property(p => p.Priority).HasConversion<string>()
                                         .HasMaxLength(30)
                                         .IsRequired();
    }
}
