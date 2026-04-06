using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static MaxEndLabs.Data.Common.IdentityConstrains.IdentityRole;

namespace MaxEndLabs.Data.Configuration
{
	public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
	{
		public void Configure(EntityTypeBuilder<IdentityRole<Guid>> entity)
		{
			entity
				.HasData(SeedIdentityRole());
		}

		private IdentityRole<Guid>[] SeedIdentityRole()
		{
			return
			[
				new IdentityRole<Guid>
				{
					Id = AdminRoleId,
					Name = "Admin",
					NormalizedName = "ADMIN"
				},
				new IdentityRole<Guid>
				{
					Id = UserRoleId,
					Name = "User",
					NormalizedName = "USER"
				}
			];
		}
	}
}
