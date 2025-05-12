using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using DevPack.Domain.Abstractions;
using DevPack.Domain.Core.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Bson;

namespace DevPack.Domain.Core;

public abstract class Entity<TType, TAuditable> : Entity<TType>, IAuditable
    where TAuditable : struct
{
    public DateTime? CreatedAt { get; }
    public string? CreatedBy { get; }
    public DateTime? LastUpdatedAt { get; }
    public string? LastUpdatedBy { get; }
    
    protected Entity(TType id) : base(id) { }
    protected Entity(TType id, string createdBy) : base(id) =>
        (CreatedAt, CreatedBy) = (DateTime.UtcNow, createdBy);
    protected Entity(TType id, DateTime createdAt, string createdBy, DateTime? updatedAt = null, string? updatedBy = null) 
        : base(id) =>
        (CreatedAt, CreatedBy, LastUpdatedAt, LastUpdatedBy) = (createdAt, createdBy, updatedAt, updatedBy);
}

public abstract class Entity<TType> : IEntity
{
    [IgnoreDataMember] 
    public TType Id { get; init; }
    
    [NotMapped]
    [IgnoreDataMember]
    public bool IsValid { get; protected set; }

    [NotMapped]
    [IgnoreDataMember]
    public ValidationResult? ValidationResult { get; protected set; }

    //protected Entity() { }
    protected Entity(TType id)
    {
        if (!IsValidId(id))
            throw new EntityIdIsNotValidDomainException($"{nameof(Id)} is not valid or empty.");

        Id = id;
    }

    private static bool IsValidId(TType id)
    {
        if (id == null) return false;

        var tryParse = Guid.TryParse(id.ToString(), out _);
        if (tryParse) 
            return true;

        var typeCode = Type.GetTypeCode(id.GetType());

        return typeCode switch
        {
            TypeCode.Int16 => short.Parse(id.ToString() ?? string.Empty) >= 0,
            TypeCode.Int32 => int.Parse(id.ToString() ?? string.Empty) >= 0,
            TypeCode.Int64 => long.Parse(id.ToString() ?? string.Empty) >= 0,
            TypeCode.String => !string.IsNullOrEmpty(id.ToString()),
            TypeCode.Object => id.GetType() == ObjectId.Empty.GetType(),
            _ => false
        };
    }
    
    public virtual bool IsValidated(AbstractValidator<IEntity> validator)
        => validator.Validate(this).IsValid;
    
    #region Domain Events
    
    private readonly List<IDomainEvent> _domainEvents = [];
    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;
    
    public void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    #endregion

    #region Base Behaviours

    public override bool Equals(object? obj)
    {
        var compareTo = obj as Entity<TType>;

        if (ReferenceEquals(this, compareTo)) return true;

        return !ReferenceEquals(null, compareTo) && Id!.Equals(compareTo.Id);
    }

    public static bool operator ==(Entity<TType>? a, Entity<TType>? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TType>? a, Entity<TType>? b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().GetHashCode() ^ 93) + Id!.GetHashCode();
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    #endregion
}

public abstract class Entity : Entity<Guid>
{
    protected Entity(Guid id) : base(id) { }
    
}
