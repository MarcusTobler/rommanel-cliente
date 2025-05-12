namespace DevPack.Domain.Core.Custom;

public record Money(decimal Amount, Currency Currency)
{
    public static Money Zero() => new(0, Currency.None);
    public static Money Zero(Currency currency) => new(0, currency);
    
    public bool IsZero() => this == Zero(Currency);

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Currencies have to be equal");
        
        return left with { Amount = left.Amount + right.Amount };
    }
}