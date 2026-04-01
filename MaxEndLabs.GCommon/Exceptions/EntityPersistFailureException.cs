namespace MaxEndLabs.GCommon.Exceptions
{
	public class EntityPersistFailureException : Exception
	{
		public EntityPersistFailureException()
		{

		}

		public EntityPersistFailureException(string message)
			: base(message)
		{

		}
	}
}
