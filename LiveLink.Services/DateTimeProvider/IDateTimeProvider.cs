using System;

namespace LiveLink.Services.DateTimeProvider
{
	public interface IDateTimeProvider
	{
		DateTime Now();
	}

	public class FakeDateTimeProvider : IDateTimeProvider
	{
		private readonly DateTime _dateTime;

		public FakeDateTimeProvider(DateTime dateTime)
		{
			_dateTime = dateTime;
		}

		public DateTime Now() => _dateTime;
	}

	public class UtcDateTimeProvider : IDateTimeProvider
	{
		public DateTime Now() => DateTime.UtcNow;
	}

	public class LocalDateTimeProvider : IDateTimeProvider
	{
		public DateTime Now() => DateTime.Now;
	}
}
