namespace DevPack.Domain.Core;

public abstract class ValueObject : IComparable, IComparable<ValueObject>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();
    
    public abstract override string? ToString();
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType()) return false;

        var other = (ValueObject) obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return current * 23 + (obj?.GetHashCode() ?? 0);
                }
            });
    }

    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            return true;

        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }

    public ValueObject? GetCopy()
    {
        return MemberwiseClone() as ValueObject;
    }
    
    public int CompareTo(object? obj)
    {
        if (obj == null)
            return 1;

        var thisType = GetUnproxiedType(this);
        var otherType = GetUnproxiedType(obj);

        if (thisType != otherType)
            return string.Compare(thisType.ToString(), otherType.ToString(), StringComparison.Ordinal);

        var other = (ValueObject)obj;

        var components = GetEqualityComponents().ToArray();
        var otherComponents = other.GetEqualityComponents().ToArray();

        for (var i = 0; i < components.Length; i++)
        {
            var comparison = CompareComponents(components[i], otherComponents[i]);
            if (comparison != 0)
                return comparison;
        }

        return 0;
    }
    
    public int CompareTo(ValueObject? other)
    {
        return CompareTo(other as object);
    }
    
    private static int CompareComponents(object? object1, object? object2)
    {
        if (object1 is null && object2 is null)
            return 0;

        if (object1 is null)
            return -1;

        if (object2 is null)
            return 1;

        if (object1 is IComparable comparable1 && object2 is IComparable comparable2)
            return comparable1.CompareTo(comparable2);

        return object1.Equals(object2) ? 0 : -1;
    }

    internal static Type GetUnproxiedType(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        const string EFCoreProxyPrefix = "Castle.Proxies.";
        const string NHibernateProxyPostfix = "Proxy";

        var type = obj.GetType();
        var typeString = type.ToString();

        if (typeString.Contains(EFCoreProxyPrefix) || typeString.EndsWith(NHibernateProxyPostfix))
            return type.BaseType!;

        return type;
    }
    
}