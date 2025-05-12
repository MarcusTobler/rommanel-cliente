using DevPack.Core.Enum;

namespace DevPack.Domain.Core.Custom;

public sealed record Currency(int Value, string Name, string? Symbol) : Enumeration<Currency>(Value, Name)
{
    internal static readonly Currency None = new Currency(0, "", "");

    public static readonly Currency USD = new Currency(1, "USD", "US$");
    public static readonly Currency EUR = new Currency(2, "EUR", "\u20ac");
    public static readonly Currency BRL = new Currency(3, "BRL", "R$");
    public static readonly Currency GBP = new Currency(4, "GBP", "\u00a3");
    public static readonly Currency AUD = new Currency(5, "AUD", "AU$");
}