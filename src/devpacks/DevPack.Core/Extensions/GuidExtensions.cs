namespace DevPack.Core.Extensions;

public class GuidExtensions
{
    public static Guid NewSequentialGuid()
    {
        var tempGuid = Guid.NewGuid();
        var bytes = tempGuid.ToByteArray();
        var time = System.DateTime.Now;
        bytes[3] = (byte) time.Year;
        bytes[2] = (byte) time.Month;
        bytes[1] = (byte) time.Day;
        bytes[0] = (byte) time.Hour;
        bytes[5] = (byte) time.Minute;
        bytes[4] = (byte) time.Second;

        return new Guid(bytes);
    }
}