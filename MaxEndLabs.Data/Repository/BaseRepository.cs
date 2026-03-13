namespace MaxEndLabs.Data.Repository
{
	public class BaseRepository : IDisposable
	{
		private bool isDisposed = false;
		private readonly MaxEndLabsDbContext dbContext;

		protected BaseRepository(MaxEndLabsDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		protected MaxEndLabsDbContext DbContext => dbContext;

		protected async Task<int> SaveChangesAsync()
		{
			return await DbContext.SaveChangesAsync();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (!isDisposed)
			{
				if (disposing)
				{
					dbContext?.Dispose();
				}
			}
			isDisposed = true;
		}
	}
}
