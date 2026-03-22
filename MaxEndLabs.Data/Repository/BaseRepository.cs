namespace MaxEndLabs.Data.Repository
{
	public class BaseRepository : IDisposable
	{
		private bool isDisposed = false;
		private readonly MaxEndLabsDbContext _dbContext;

		protected BaseRepository(MaxEndLabsDbContext dbContext)
		{
			this._dbContext = dbContext;
		}

		protected MaxEndLabsDbContext DbContext => _dbContext;

		public async Task<int> SaveChangesAsync()
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
					_dbContext?.Dispose();
				}
			}
			isDisposed = true;
		}
	}
}
