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

		public DateTime Now()
		{
			return _dateTime;
		}
	}

	public class UtcDateTimeProvider : IDateTimeProvider
	{
		public DateTime Now()
		{
			return DateTime.UtcNow;
		}
	}

	public class LocalDateTimeProvider : IDateTimeProvider
	{
		public DateTime Now()
		{
			return DateTime.Now;
		}
	}
}