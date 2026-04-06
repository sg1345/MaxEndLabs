using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxEndLabs.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaxEndLabs.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            entity
                .HasKey(oi => new { oi.OrderId, oi.ProductId, oi.ProductVariantId });

            entity
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(oi => oi.ProductVariant)
                .WithMany(pv => pv.OrderItems)
                .HasForeignKey(oi => oi.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .Property(oi => oi.UnitPrice)
                .HasPrecision(10, 2);

            entity
                .Property(oi=>oi.LineTotal)
                .HasPrecision(10, 2);

            entity
                .HasData(SeedOrderItems());
        }

        private IEnumerable<OrderItem> SeedOrderItems()
        {
            var orderItems = new List<OrderItem>();
            var baseOrderGuidString = "11111111-2222-3333-4444-0000000000";

            // Product 1: T-Shirt
            var prod1Id = Guid.Parse("02c314ef-6c38-41cd-ac00-aa3a986e3f46");
            var var1Id = Guid.Parse("75feaf94-8a87-495e-910e-01276c94d0c8"); // Size S, Black
            var price1 = 40.00m;

            // Product 2: Nasar House Straps
            var prod2Id = Guid.Parse("c4ed28d4-92cb-4532-8949-044be6a66e28");
            var var2Id = Guid.Parse("21b6185b-b955-46c4-a48f-1e8b037fa265"); // Black and Green
            var price2 = 26.00m;

            // Product 3: Gold Standard Whey
            var prod3Id = Guid.Parse("a8bcb2ba-bd47-4370-a443-505776baebb3");
            var var3Id = Guid.Parse("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"); // Double Rich Chocolate
            var price3 = 99.99m;

            for (int i = 1; i <= 30; i++)
            {
                string orderIdSuffix = i.ToString("D2");
                var orderId = Guid.Parse(baseOrderGuidString + orderIdSuffix);

                if (i % 2 == 0) // Even numbered orders get 2 items
                {
                    orderItems.Add(new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = prod1Id,
                        ProductVariantId = var1Id,
                        Quantity = 1,
                        UnitPrice = price1,
                        LineTotal = price1
                    });

                    orderItems.Add(new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = prod2Id,
                        ProductVariantId = var2Id,
                        Quantity = 1,
                        UnitPrice = price2,
                        LineTotal = price2
                    });
                }
                else // Odd numbered orders get 1 item
                {
                    orderItems.Add(new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = prod3Id,
                        ProductVariantId = var3Id,
                        Quantity = 1,
                        UnitPrice = price3,
                        LineTotal = price3
                    });
                }
            }

            return orderItems;
        }
    }
}
