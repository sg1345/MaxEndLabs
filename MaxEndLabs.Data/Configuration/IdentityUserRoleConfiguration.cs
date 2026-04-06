using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static MaxEndLabs.Data.Common.IdentityConstrains.ApplicationUser;
using static MaxEndLabs.Data.Common.IdentityConstrains.IdentityRole;

namespace MaxEndLabs.Data.Configuration
{
	public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> entity)
		{
			entity.HasData(SeedUserRole());
		}

		private IdentityUserRole<Guid>[] SeedUserRole()
		{
			return 
			[
				new IdentityUserRole<Guid>
				{
					UserId = AdminUserId,
					RoleId = AdminRoleId
				}
			];
		}
	}
}
