using MaxEndLabs.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static MaxEndLabs.Data.Common.IdentityConstrains.ApplicationUser;

namespace MaxEndLabs.Data.Configuration
{
	public class IdentityUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> entity)
		{
			entity
				.HasData(SeedIdentityUser());
		}

		private ApplicationUser[] SeedIdentityUser()
		{
			var adminUser = new ApplicationUser
			{
				Id = AdminUserId,
				FullName = "Dimitar Ivanov Berbatov",
				UserName = "admin@labs.com",
				NormalizedUserName = "ADMIN@LABS.COM",
				Email = "admin@labs.com",
				NormalizedEmail = "ADMIN@LABS.COM",
				EmailConfirmed = true,
				ConcurrencyStamp = "4bfdb153-a446-4968-a40f-34adc37a6f28",
				SecurityStamp = "c0142471-6ffd-44b4-b430-8b3c7acf8fbf",
				PasswordHash = "AQAAAAIAAYagAAAAEP9JzX0YwKm6N+luPkuHrobdVlv+8FQxWWME12rmICorZgnbmrHziPC+5WxRG5/6aw=="
			};

			return [adminUser];
		}
	}
}
