using MaxEndLabs.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaxEndLabs.Data.Configuration
{
	public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
	{
		public void Configure(EntityTypeBuilder<ProductVariant> entity)
		{
			entity
				.HasQueryFilter(pv => pv.IsDeleted == false);
			entity
				.HasData(SeedProductVariant());
            entity
                .Property(pv => pv.Price)
                .HasPrecision(10, 2);
        }

		private ProductVariant[] SeedProductVariant()
		{
			return
				[
					new ProductVariant
					{
						Id = Guid.Parse("75feaf94-8a87-495e-910e-01276c94d0c8"),
						ProductId = Guid.Parse("02c314ef-6c38-41cd-ac00-aa3a986e3f46"),
						VariantName = "Size S / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("fac660c5-b882-426d-bc27-5c20a6634402"),
						ProductId = Guid.Parse("02c314ef-6c38-41cd-ac00-aa3a986e3f46"),
						VariantName = "Size M / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("4c8177a6-5c5d-4ce5-9955-65626c6262a3"),
						ProductId = Guid.Parse("02c314ef-6c38-41cd-ac00-aa3a986e3f46"),
						VariantName = "Size L / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("91367123-74d9-435e-91ce-98debdeaf9ff"),
						ProductId = Guid.Parse("02c314ef-6c38-41cd-ac00-aa3a986e3f46"),
						VariantName = "Size XL / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("430e4616-4746-4fdb-a227-5584361edd68"),
						ProductId = Guid.Parse("1dec9339-ab58-4186-90c3-c6eb7f971893"),
						VariantName = "Size S / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("21f64de2-0991-4363-baff-16aa1b8064f4"),
						ProductId = Guid.Parse("1dec9339-ab58-4186-90c3-c6eb7f971893"),
						VariantName = "Size M / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("0135a402-3bfe-4cb0-9ae6-96a18e8dadef"),
						ProductId = Guid.Parse("1dec9339-ab58-4186-90c3-c6eb7f971893"),
						VariantName = "Size L / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("38657209-0631-4310-8839-7a8badc05367"),
						ProductId = Guid.Parse("1dec9339-ab58-4186-90c3-c6eb7f971893"),
						VariantName = "Size XL / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("4147f8f0-6c0a-4005-a916-20cef7685120"),
						ProductId = Guid.Parse("e28b6ccc-c7d5-49d6-95ae-1c6e665c6ced"),
						VariantName = "Size S / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("d57befc8-55a9-41c1-b5f6-7a47381de37f"),
						ProductId = Guid.Parse("e28b6ccc-c7d5-49d6-95ae-1c6e665c6ced"),
						VariantName = "Size M / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("e2994b78-47b5-4aaa-afec-3d1a093b5a43"),
						ProductId = Guid.Parse("e28b6ccc-c7d5-49d6-95ae-1c6e665c6ced"),
						VariantName = "Size L / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("389068f2-e83d-4d11-8d7c-db2760966585"),
						ProductId = Guid.Parse("e28b6ccc-c7d5-49d6-95ae-1c6e665c6ced"),
						VariantName = "Size XL / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("c3adf1e3-384b-4fbd-9601-4ad589f5cd8d"),
						ProductId = Guid.Parse("96ddcde3-5ff2-4fd4-b685-f216945568a3"),
						VariantName = "Size S / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("6f665e33-c4a9-4548-aa78-accdad51909a"),
						ProductId = Guid.Parse("96ddcde3-5ff2-4fd4-b685-f216945568a3"),
						VariantName = "Size M / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("7f8adfae-2530-4920-b71c-4a58ce6a428d"),
						ProductId = Guid.Parse("96ddcde3-5ff2-4fd4-b685-f216945568a3"),
						VariantName = "Size L / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("51b55327-3e64-406b-b2b6-341bb4562ce1"),
						ProductId = Guid.Parse("96ddcde3-5ff2-4fd4-b685-f216945568a3"),
						VariantName = "Size XL / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("3415ca9a-14ab-44a8-8cfe-cfbaa687fddd"),
						ProductId = Guid.Parse("467ea21c-0fcf-4dde-801a-17e77ee4a5b2"),
						VariantName = "Size S / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("f94fc295-e46a-41f1-b016-0055ebfbde22"),
						ProductId = Guid.Parse("467ea21c-0fcf-4dde-801a-17e77ee4a5b2"),
						VariantName = "Size M / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("3c793526-7537-49f4-8fff-aa25dd25ad48"),
						ProductId = Guid.Parse("467ea21c-0fcf-4dde-801a-17e77ee4a5b2"),
						VariantName = "Size L / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("53c5dc85-a298-4627-b65a-bcae0f94682a"),
						ProductId = Guid.Parse("467ea21c-0fcf-4dde-801a-17e77ee4a5b2"),
						VariantName = "Size XL / Color: Black"
					},
					new ProductVariant
					{
						Id = Guid.Parse("b347cf2f-c0cd-416f-a330-e75614b11c60"),
						ProductId = Guid.Parse("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"),
						VariantName = "Size 38 EU"
					},
					new ProductVariant
					{
						Id = Guid.Parse("2e64045a-0c1b-4122-9100-14df928f025a"),
						ProductId = Guid.Parse("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"),
						VariantName = "Size 39 EU"
					},
					new ProductVariant
					{
						Id = Guid.Parse("077ce4ee-d228-421e-8c67-b6936a511e36"),
						ProductId = Guid.Parse("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"),
						VariantName = "Size 40 EU"
					},
					new ProductVariant
					{
						Id = Guid.Parse("8c3c3fd0-d1cb-49e6-8d2b-e7c04a059917"),
						ProductId = Guid.Parse("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"),
						VariantName = "Size 41 EU"
					},
					new ProductVariant
					{
						Id = Guid.Parse("0bedd38e-210c-4552-9de8-8ba385901981"),
						ProductId = Guid.Parse("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"),
						VariantName = "Size 42 EU"
					},
					new ProductVariant
					{
						Id = Guid.Parse("8a2e6649-bd1a-4147-acf0-5e20405d315d"),
						ProductId = Guid.Parse("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"),
						VariantName = "Size 43 EU"
					},
					new ProductVariant
					{
						Id = Guid.Parse("1cb5d36e-0e0c-401e-a6dc-a1d287e8afb6"),
						ProductId = Guid.Parse("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"),
						VariantName = "Size 44 EU"
					},
					new ProductVariant
					{
						Id = Guid.Parse("04eb7d5e-ad31-417a-8544-470f559583c2"),
						ProductId = Guid.Parse("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"),
						VariantName = "Size 45 EU"
					},
					new ProductVariant
					{
						Id = Guid.Parse("2ceab2fb-6d88-421a-98b4-ba293000fa8e"),
						ProductId = Guid.Parse("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"),
						VariantName = "Size 46 EU"
					},
					new ProductVariant
					{
						Id = Guid.Parse("694e0600-e0ca-456a-9ec9-9dd3b25651d9"),
						ProductId = Guid.Parse("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"),
						VariantName = "Size 47 EU"
					},
					new ProductVariant
					{
						Id = Guid.Parse("7e905a23-8f10-4074-aade-7c29693e6e93"),
						ProductId = Guid.Parse("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"),
						VariantName = "Size 48 EU"
					},
					new ProductVariant
					{
						Id = Guid.Parse("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"),
						ProductId = Guid.Parse("a8bcb2ba-bd47-4370-a443-505776baebb3"),
						VariantName = "Double Rich Chocolate / 2.26KG",
						Price = 99.99m
					}, 
					new ProductVariant
					{
						Id = Guid.Parse("7b0f801b-5af3-4c03-9406-e4262f5e3c16"),
						ProductId = Guid.Parse("c7f9f6ad-8512-49b0-bb48-980c3922fe60"),
						VariantName = "Unflavored / 317g",
						Price = 30m
					},
					new ProductVariant
					{
						Id = Guid.Parse("21b6185b-b955-46c4-a48f-1e8b037fa265"),
						ProductId = Guid.Parse("c4ed28d4-92cb-4532-8949-044be6a66e28"),
						VariantName = "Color: Black and Green"
					},
                    new ProductVariant
                    {
                        Id = Guid.Parse("f472b8d1-9c65-4d1a-a89e-32cf4b918a11"),
                        ProductId = Guid.Parse("3b89e3a0-8d5a-4e2b-9e45-8f6a9c1d3e2a"),
                        VariantName = "Color: Brown"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("8d12e9b4-3a76-4f81-b51c-918d2a6c4f33"),
                        ProductId = Guid.Parse("7f1c2d9e-4b6a-485c-89a1-2d3e4f5a6b7c"),
                        VariantName = "Color: White and Red"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("1b8c3a9d-7e42-4658-8d1f-b3a2c5e9d411"),
                        ProductId = Guid.Parse("9e8d7c6b-5a4f-3e2d-1c9b-8a7b6c5d4e3f"),
                        VariantName = "Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("5c4d9e2a-1b8f-4a36-9d7c-8e1f2a3b4c55"),
                        ProductId = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-e1f2a3b4c5d6"),
                        VariantName = "Color: Black and Green"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("9e1f2a3b-4c5d-6e7f-8a9b-0c1d2e3f4a55"),
                        ProductId = Guid.Parse("d6c5b4a3-2f1e-d0c9-b8a7-f6e5d4c3b2a1"),
                        VariantName = "Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("2a3b4c5d-6e7f-8a9b-0c1d-e1f2a3b4c5d6"),
                        ProductId = Guid.Parse("e5d4c3b2-a1f0-e9d8-c7b6-a5b4c3d2e1f0"),
                        VariantName = "Color: Blue"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("6e7f8a9b-0c1d-2e3f-4a5b-c6d7e8f9a0b1"),
                        ProductId = Guid.Parse("f0e1d2c3-b4a5-6b7c-8d9e-0f1a2b3c4d5e"),
                        VariantName = "Color: Blue"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("0c1d2e3f-4a5b-6c7d-8e9f-a0b1c2d3e4f5"),
                        ProductId = Guid.Parse("5e4d3c2b-1a0f-e9d8-c7b6-a5b4c3d2e1f0"),
                        VariantName = "Color: Black and Gold"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"),
                        ProductId = Guid.Parse("c3d2e1f0-a5b4-c7b6-e9d8-1a0f5e4d3c2b"),
                        VariantName = "Size S / Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e"),
                        ProductId = Guid.Parse("c3d2e1f0-a5b4-c7b6-e9d8-1a0f5e4d3c2b"),
                        VariantName = "Size M / Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f"),
                        ProductId = Guid.Parse("c3d2e1f0-a5b4-c7b6-e9d8-1a0f5e4d3c2b"),
                        VariantName = "Size L / Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8a"),
                        ProductId = Guid.Parse("c3d2e1f0-a5b4-c7b6-e9d8-1a0f5e4d3c2b"),
                        VariantName = "Size XL / Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8a9b"),
                        ProductId = Guid.Parse("b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e"),
                        VariantName = "Size S / Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("f6a7b8c9-d0e1-4f2a-3b4c-5d6e7f8a9b0c"),
                        ProductId = Guid.Parse("b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e"),
                        VariantName = "Size M / Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("a7b8c9d0-e1f2-4a3b-4c5d-6e7f8a9b0c1d"),
                        ProductId = Guid.Parse("b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e"),
                        VariantName = "Size L / Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("b8c9d0e1-f2a3-4b4c-5d6e-7f8a9b0c1d2e"),
                        ProductId = Guid.Parse("b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e"),
                        VariantName = "Size XL / Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("c9d0e1f2-a3b4-4c5d-6e7f-8a9b0c1d2e3f"),
                        ProductId = Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1a2b3c4d5e6f"),
                        VariantName = "Size S / Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("d0e1f2a3-b4c5-4d6e-7f8a-9b0c1d2e3f4a"),
                        ProductId = Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1a2b3c4d5e6f"),
                        VariantName = "Size M / Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("e1f2a3b4-c5d6-4e7f-8a9b-0c1d2e3f4a5b"),
                        ProductId = Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1a2b3c4d5e6f"),
                        VariantName = "Size L / Color: Black"
                    },
                    new ProductVariant
                    {
                        Id = Guid.Parse("f2a3b4c5-d6e7-4f8a-9b0c-1d2e3f4a5b6c"),
                        ProductId = Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1a2b3c4d5e6f"),
                        VariantName = "Size XL / Color: Black"
                    }
                ];
		}
	}
}
