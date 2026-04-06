using MaxEndLabs.Data.Common;
using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            entity
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            entity
                .HasData(SeedOrders());
        }

        private IEnumerable<Order> SeedOrders()
        {
            var orders = new List<Order>();

            // We use a base Guid and just change the last two digits so EF Core always 
            // sees the exact same Guids every time, preventing migration conflicts!
            var baseGuidString = "11111111-2222-3333-4444-0000000000";
            var testUserId = IdentityConstrains.ApplicationUser.TestUserId;

            for (int i = 1; i <= 30; i++)
            {
                // Pad the number (e.g., "01", "02" ... "30")
                string idSuffix = i.ToString("D2");

                orders.Add(new Order
                {
                    Id = Guid.Parse(baseGuidString + idSuffix),
                    UserId = testUserId,
                    OrderNumber = $"ORD-2026040{i}-{10 + i}BC", // E.g., ORD-20260401-11BC
                    StreetAddress = $"1 Demo User Avenue",
                    City = "Sofia",
                    Postcode = "1000",
                    Status = i % 4 == 0 ? OrderStatus.Completed : OrderStatus.Paid, // Mix of Paid & Completed
                    TotalAmount = i % 2 == 0 ? 66.00m : 99.99m, // Matches the OrderItems calculation below
                    CreatedAt = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc).AddDays(i),
                    UpdatedAt = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc).AddDays(i)
                });
            }

            return orders;
        }
    }
}
