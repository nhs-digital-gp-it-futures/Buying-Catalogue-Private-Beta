using System;

namespace SharePointPnP.IdentityModel.Extensions.S2S
{
  public sealed class EpochTime
  {
    public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    public long SecondsSinceUnixEpoch { get; }

    public DateTime DateTime
    {
      get
      {
        TimeSpan timeSpan = TimeSpan.FromSeconds(SecondsSinceUnixEpoch);
        return DateTimeUtil.AddNonNegative(UnixEpoch, timeSpan);
      }
    }

    public EpochTime(string secondsSinceUnixEpochString)
    {
      if (!long.TryParse(secondsSinceUnixEpochString, out long secondsSinceUnixEpoch))
      {
        throw new ArgumentException("Invalid date time string format.", "secondsSinceUnixEpochString");
      }

      SecondsSinceUnixEpoch = secondsSinceUnixEpoch;
    }

    public EpochTime(long secondsSinceUnixEpoch)
    {
      if (secondsSinceUnixEpoch < 0L)
      {
        throw new ArgumentException("secondsSinceUnixEpoch must be greater than or equal to zero.", "secondsSinceUnixEpoch");
      }

      SecondsSinceUnixEpoch = secondsSinceUnixEpoch;
    }

    public EpochTime(DateTime dateTime)
    {
      if (dateTime < UnixEpoch)
      {
        string message = string.Format(System.Globalization.CultureInfo.InvariantCulture, "DateTime must be greater than or equal to {0}", new object[] { UnixEpoch.ToString() });
        throw new ArgumentOutOfRangeException("dateTime", message);
      }

      SecondsSinceUnixEpoch = (long)(dateTime - UnixEpoch).TotalSeconds;
    }
  }
}
