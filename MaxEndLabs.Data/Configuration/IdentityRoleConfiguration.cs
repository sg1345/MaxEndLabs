using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static MaxEndLabs.Data.Common.IdentityConstrains.IdentityRole;

namespace MaxEndLabs.Data.Configuration
{
	public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
	{
		public void Configure(EntityTypeBuilder<IdentityRole> entity)
		{
			entity
				.HasData(SeedIdentityRole());
		}

		private IdentityRole[] SeedIdentityRole()
		{
			return
			[
				new IdentityRole
				{
					Id = AdminRoleId,
					Name = "Admin",
					NormalizedName = "ADMIN"
				},
				new IdentityRole
				{
					Id = UserRoleId,
					Name = "User",
					NormalizedName = "USER"
				}
			];
		}
	}
}
