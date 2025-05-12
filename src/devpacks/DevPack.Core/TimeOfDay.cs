using System.Text.RegularExpressions;

namespace DevPack.Core;

public class TimeOfDay
{
    private const int MINUTES_PER_DAY = 60 * 24;
    private const int SECONDS_PER_DAY = SECONDS_PER_HOUR * 24;
    private const int SECONDS_PER_HOUR = 3600;
    private static readonly Regex _TodRegex = new(@"\d?\d:\d\d:\d\d|\d?\d:\d\d");

    public TimeOfDay()
    {
        Init(0, 0, 0);
    }

    public TimeOfDay(int hour, int minute, int second = 0)
    {
        Init(hour, minute, second);
    }

    public TimeOfDay(int hhmmss)
    {
        Init(hhmmss);
    }

    public TimeOfDay(System.DateTime dt)
    {
        Init(dt);
    }

    public TimeOfDay(TimeOfDay td)
    {
        Init(td.Hour, td.Minute, td.Second);
    }

    public TimeOfDay(string time)
    {
        Init(time);
    }

    public int HHMMSS => Hour * 10000 + Minute * 100 + Second;

    public int Hour { get; private set; }
    public int Minute { get; private set; }
    public int Second { get; private set; }

    public double TotalDays => TotalSeconds / (24d * SECONDS_PER_HOUR);

    public double TotalHours => TotalSeconds / (1d * SECONDS_PER_HOUR);

    public double TotalMinutes => TotalSeconds / 60d;

    public int TotalSeconds => Hour * 3600 + Minute * 60 + Second;

    public bool Equals(TimeOfDay? other)
    {
        if (other == null) return false;
        return TotalSeconds == other.TotalSeconds;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        var td = obj as TimeOfDay;
        return td != null && Equals(td);
    }

    public override int GetHashCode()
    {
        return TotalSeconds;
    }

    public System.DateTime ToDateTime(System.DateTime dt)
    {
        return new System.DateTime(dt.Year, dt.Month, dt.Day, Hour, Minute, Second);
    }

    public override string ToString()
    {
        return ToString("HH:mm:ss");
    }

    public string ToString(string format)
    {
        var now = System.DateTime.Now;
        var dt = new System.DateTime(now.Year, now.Month, now.Day, Hour, Minute, Second);
        return dt.ToString(format);
    }

    public TimeSpan ToTimeSpan()
    {
        return new TimeSpan(Hour, Minute, Second);
    }

    public System.DateTime ToToday()
    {
        var now = System.DateTime.Now;
        return new System.DateTime(now.Year, now.Month, now.Day, Hour, Minute, Second);
    }

    private void Init(int hour, int minute, int second)
    {
        if (hour < 0 || hour > 23) throw new ArgumentException("Invalid hour, must be from 0 to 23.");
        if (minute < 0 || minute > 59) throw new ArgumentException("Invalid minute, must be from 0 to 59.");
        if (second < 0 || second > 59) throw new ArgumentException("Invalid second, must be from 0 to 59.");
        Hour = hour;
        Minute = minute;
        Second = second;
    }

    private void Init(int hhmmss)
    {
        var hour = hhmmss / 10000;
        var min = (hhmmss - hour * 10000) / 100;
        var sec = hhmmss - hour * 10000 - min * 100;
        Init(hour, min, sec);
    }

    private void Init(System.DateTime dt)
    {
        Init(dt.Hour, dt.Minute, dt.Second);
    }

    private void Init(string time)
    {
        if (int.TryParse(time.Replace(":", ""), out var hhmmss))
            Init(hhmmss);
        else
            Init(0, 0, 0);
    }

    #region -- Static --

    public static TimeOfDay Midnight => new(0, 0);
    public static TimeOfDay Noon => new(12, 0);

    public static TimeOfDay operator -(TimeOfDay t1, TimeOfDay t2)
    {
        var now = System.DateTime.Now;
        var dt1 = new System.DateTime(now.Year, now.Month, now.Day, t1.Hour, t1.Minute, t1.Second);
        var ts = new TimeSpan(t2.Hour, t2.Minute, t2.Second);
        var dt2 = dt1 - ts;
        return new TimeOfDay(dt2);
    }

    public static bool operator !=(TimeOfDay? t1, TimeOfDay? t2)
    {
        if (ReferenceEquals(t1, t2))
            return true;
        if (ReferenceEquals(t1, null))
            return true;
        return t2 is not null && t1.TotalSeconds != t2.TotalSeconds;
    }

    public static bool operator !=(TimeOfDay t1, System.DateTime dt2)
    {
        if (ReferenceEquals(t1, null)) return false;
        var dt1 = new System.DateTime(dt2.Year, dt2.Month, dt2.Day, t1.Hour, t1.Minute, t1.Second);
        return dt1 != dt2;
    }

    public static bool operator !=(System.DateTime dt1, TimeOfDay t2)
    {
        if (ReferenceEquals(t2, null)) return false;
        var dt2 = new System.DateTime(dt1.Year, dt1.Month, dt1.Day, t2.Hour, t2.Minute, t2.Second);
        return dt1 != dt2;
    }

    public static TimeOfDay operator +(TimeOfDay t1, TimeOfDay t2)
    {
        var now = System.DateTime.Now;
        var dt1 = new System.DateTime(now.Year, now.Month, now.Day, t1.Hour, t1.Minute, t1.Second);
        var ts = new TimeSpan(t2.Hour, t2.Minute, t2.Second);
        var dt2 = dt1 + ts;
        return new TimeOfDay(dt2);
    }

    public static bool operator <(TimeOfDay t1, TimeOfDay t2)
    {
        if (ReferenceEquals(t1, t2))
            return true;
        if (ReferenceEquals(t1, null))
            return true;
        return t1.TotalSeconds < t2.TotalSeconds;
    }

    public static bool operator <(TimeOfDay t1, System.DateTime dt2)
    {
        if (ReferenceEquals(t1, null)) return false;
        var dt1 = new System.DateTime(dt2.Year, dt2.Month, dt2.Day, t1.Hour, t1.Minute, t1.Second);
        return dt1 < dt2;
    }

    public static bool operator <(System.DateTime dt1, TimeOfDay t2)
    {
        if (ReferenceEquals(t2, null)) return false;
        var dt2 = new System.DateTime(dt1.Year, dt1.Month, dt1.Day, t2.Hour, t2.Minute, t2.Second);
        return dt1 < dt2;
    }

    public static bool operator <=(TimeOfDay t1, TimeOfDay t2)
    {
        if (ReferenceEquals(t1, t2)) return true;

        if (ReferenceEquals(t1, null))
        {
            return true;
        }

        if (t1 == t2) return true;
        return t1.TotalSeconds <= t2.TotalSeconds;
    }

    public static bool operator <=(TimeOfDay t1, System.DateTime dt2)
    {
        if (ReferenceEquals(t1, null)) return false;
        var dt1 = new System.DateTime(dt2.Year, dt2.Month, dt2.Day, t1.Hour, t1.Minute, t1.Second);
        return dt1 <= dt2;
    }

    public static bool operator <=(System.DateTime dt1, TimeOfDay t2)
    {
        if (ReferenceEquals(t2, null)) return false;
        var dt2 = new System.DateTime(dt1.Year, dt1.Month, dt1.Day, t2.Hour, t2.Minute, t2.Second);
        return dt1 <= dt2;
    }

    public static bool operator ==(TimeOfDay? t1, TimeOfDay? t2)
    {
        if (ReferenceEquals(t1, t2))
            return true;
        if (ReferenceEquals(t1, null))
            return true;
        return t1.Equals(t2);
    }

    public static bool operator ==(TimeOfDay t1, System.DateTime dt2)
    {
        if (ReferenceEquals(t1, null)) return false;
        var dt1 = new System.DateTime(dt2.Year, dt2.Month, dt2.Day, t1.Hour, t1.Minute, t1.Second);
        return dt1 == dt2;
    }

    public static bool operator ==(System.DateTime dt1, TimeOfDay t2)
    {
        if (ReferenceEquals(t2, null)) return false;
        var dt2 = new System.DateTime(dt1.Year, dt1.Month, dt1.Day, t2.Hour, t2.Minute, t2.Second);
        return dt1 == dt2;
    }

    public static bool operator >(TimeOfDay t1, TimeOfDay t2)
    {
        if (ReferenceEquals(t1, t2))
            return true;
        if (ReferenceEquals(t1, null))
            return true;
        return t1.TotalSeconds > t2.TotalSeconds;
    }

    public static bool operator >(TimeOfDay t1, System.DateTime dt2)
    {
        if (ReferenceEquals(t1, null)) return false;
        var dt1 = new System.DateTime(dt2.Year, dt2.Month, dt2.Day, t1.Hour, t1.Minute, t1.Second);
        return dt1 > dt2;
    }

    public static bool operator >(System.DateTime dt1, TimeOfDay t2)
    {
        if (ReferenceEquals(t2, null)) return false;
        var dt2 = new System.DateTime(dt1.Year, dt1.Month, dt1.Day, t2.Hour, t2.Minute, t2.Second);
        return dt1 > dt2;
    }

    public static bool operator >=(TimeOfDay t1, TimeOfDay t2)
    {
        if (ReferenceEquals(t1, t2))
            return true;
        if (ReferenceEquals(t1, null))
            return true;
        return t1.TotalSeconds >= t2.TotalSeconds;
    }

    public static bool operator >=(TimeOfDay t1, System.DateTime dt2)
    {
        if (ReferenceEquals(t1, null)) return false;
        var dt1 = new System.DateTime(dt2.Year, dt2.Month, dt2.Day, t1.Hour, t1.Minute, t1.Second);
        return dt1 >= dt2;
    }

    public static bool operator >=(System.DateTime dt1, TimeOfDay t2)
    {
        if (ReferenceEquals(t2, null)) return false;
        var dt2 = new System.DateTime(dt1.Year, dt1.Month, dt1.Day, t2.Hour, t2.Minute, t2.Second);
        return dt1 >= dt2;
    }

    /// <summary>
    ///     Input examples:
    ///     14:21:17            (2pm 21min 17sec)
    ///     02:15               (2am 15min 0sec)
    ///     2:15                (2am 15min 0sec)
    ///     2/1/2017 14:21      (2pm 21min 0sec)
    ///     TimeOfDay=15:13:12  (3pm 13min 12sec)
    /// </summary>
    public static TimeOfDay Parse(string s)
    {
        // We will parse any section of the text that matches this
        // pattern: dd:dd or dd:dd:dd where the first doublet can
        // be one or two digits for the hour.  But minute and second
        // must be two digits.
        var m = _TodRegex.Match(s);
        var text = m.Value;
        var fields = text.Split(':');
        if (fields.Length < 2) throw new ArgumentException("No valid time of day pattern found in input text");
        var hour = Convert.ToInt32(fields[0] as object);
        var min = Convert.ToInt32(fields[1] as object);
        var sec = fields.Length > 2 ? Convert.ToInt32(fields[2] as object) : 0;

        return new TimeOfDay(hour, min, sec);
    }

    #endregion
}