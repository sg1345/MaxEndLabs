using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static MaxEndLabs.Data.Common.IdentityConstrains.IdentityUser;
using static MaxEndLabs.Data.Common.IdentityConstrains.IdentityRole;

namespace MaxEndLabs.Data.Configuration
{
	public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserRole<string>> entity)
		{
			entity.HasData(SeedUserRole());
		}

		private IdentityUserRole<string>[] SeedUserRole()
		{
			return 
			[
				new IdentityUserRole<string>
				{
					UserId = AdminUserId,
					RoleId = AdminRoleId
				}
			];
		}
	}
}
