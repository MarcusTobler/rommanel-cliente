namespace DevPack.Core.Extensions;

public static class DateTimeExtensions
{
    public static string ToTimeAgo(this System.DateTime dateTime)
    {
        var result = string.Empty;

        var DefaultTimeZone = "E. South America Standard Time";
        if (Environment.OSVersion.Platform == System.PlatformID.Unix)
        {
            DefaultTimeZone = "America/Sao_Paulo";
        }

        var hora = TimeZoneInfo.ConvertTime(System.DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(DefaultTimeZone));

        var timeSpan = hora.Subtract(dateTime);

        if (timeSpan <= TimeSpan.FromSeconds(60))
        {
            result = string.Format("há {0} segundos", timeSpan.Seconds);
        }
        else if (timeSpan <= TimeSpan.FromMinutes(60))
        {
            result = timeSpan.Minutes > 1 ?
                String.Format("há {0} minutos", timeSpan.Minutes) :
                "há 1 minuto";
        }
        else if (timeSpan <= TimeSpan.FromHours(24))
        {
            result = timeSpan.Hours > 1 ?
                String.Format("há {0} horas", timeSpan.Hours) :
                "há 1 hora";
        }
        else if (timeSpan <= TimeSpan.FromDays(30))
        {
            result = timeSpan.Days > 1 ?
                String.Format("há {0} dias", timeSpan.Days) :
                "ontem";
        }
        else if (timeSpan <= TimeSpan.FromDays(365))
        {
            if (timeSpan.Days > 60)
            {
                result = String.Format("há {0} meses", timeSpan.Days / 30);
            }
            else if (timeSpan.Days >= 30 && timeSpan.Days <= 60)
            {
                result = "há 1 mês";
            }
        }
        else
        {
            if (timeSpan.Days >= 720)
            {
                result = String.Format("há {0} anos", timeSpan.Days / 365);
            }
            else if (timeSpan.Days > 365 && timeSpan.Days < 720)
            {
                result = "há 1 ano";
            }
        }

        return result;
    }
}