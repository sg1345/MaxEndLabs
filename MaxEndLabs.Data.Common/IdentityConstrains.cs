namespace MaxEndLabs.Data.Common
{
	public class IdentityConstrains 
	{
		public static class ApplicationUser
		{
            public static readonly Guid AdminUserId = Guid.Parse("06313180-fa45-42b5-ac33-1333f673455d");
            public static readonly Guid TestUserId = Guid.Parse("9d2b0228-4d0d-4c23-8b49-01a698857709");
        }

		public static class IdentityRole
		{
			public static readonly Guid AdminRoleId = Guid.Parse("d333796b-4dd0-410c-92fd-481a6dc3fd0d");
			public static readonly Guid UserRoleId = Guid.Parse("e444796b-4dd0-410c-92fd-481a6dc3fd0d");
		}
	}
}
