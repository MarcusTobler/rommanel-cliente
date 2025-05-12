using FluentValidation;
using FluentValidation.Results;

namespace DevPack.Messaging.Domain;

public interface IDomainValidator<in TEntity> : IValidator<TEntity> where TEntity : class
{
    ValidationResult GetValidationResult();
    bool IsValid();
    Task<bool> IsValidAsync(CancellationToken cancellationToken = default);
}