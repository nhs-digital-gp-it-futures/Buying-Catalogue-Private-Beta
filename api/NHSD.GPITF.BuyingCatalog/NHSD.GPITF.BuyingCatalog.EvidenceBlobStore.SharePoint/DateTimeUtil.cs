using System;

namespace SharePointPnP.IdentityModel.Extensions.S2S
{
  [System.Diagnostics.DebuggerNonUserCode]
  public static class DateTimeUtil
  {
    public static DateTime Add(DateTime time, TimeSpan timespan)
    {
      if (timespan == TimeSpan.Zero)
      {
        return time;
      }

      if (timespan > TimeSpan.Zero && DateTime.MaxValue - time <= timespan)
      {
        return GetMaxValue(time.Kind);
      }

      if (timespan < TimeSpan.Zero && DateTime.MinValue - time >= timespan)
      {
        return GetMinValue(time.Kind);
      }

      return time + timespan;
    }

    public static DateTime AddNonNegative(DateTime time, TimeSpan timeSpan)
    {
      if (timeSpan < TimeSpan.Zero)
      {
        throw new ArgumentException("TimeSpan must be greater than or equal to TimeSpan.Zero.", "timeSpan");
      }

      return Add(time, timeSpan);
    }

    public static DateTime GetMaxValue(DateTimeKind kind) => new DateTime(DateTime.MaxValue.Ticks, kind);

    public static DateTime GetMinValue(DateTimeKind kind) => new DateTime(DateTime.MinValue.Ticks, kind);

    public static DateTime? ToUniversalTime(DateTime? value)
    {
      if (!value.HasValue || value.Value.Kind == DateTimeKind.Utc)
      {
        return value;
      }

      return new DateTime?(ToUniversalTime(value.Value));
    }

    public static DateTime ToUniversalTime(DateTime value)
    {
      if (value.Kind == DateTimeKind.Utc)
      {
        return value;
      }

      return value.ToUniversalTime();
    }
  }
}
