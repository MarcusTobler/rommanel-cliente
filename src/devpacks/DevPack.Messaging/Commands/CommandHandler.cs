using DevPack.Data.Abstractions;
using FluentValidation.Results;

namespace DevPack.Messaging.Commands;

public abstract class CommandHandler
{
    protected readonly ValidationResult ValidationResult = new();

    protected void AddError(string message) =>
        ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
    
    protected void AddError(string propertyName, string message) =>
        ValidationResult.Errors.Add(new ValidationFailure(propertyName, message));

    protected ValidationResult PersistData(IUnitOfWork uow)
    {
        if (!uow.Commit()) AddError("An error occurred while trying to persist data");

        return ValidationResult;
    }
    
    protected ValidationResult PersistData(IUnitOfWork uow, string message)
    {
        if (!uow.Commit()) AddError(message);

        return ValidationResult;
    }
    
    protected async Task<ValidationResult> PersistDataAsync(IUnitOfWork uow)
    {
        if (!await uow.CommitAsync()) AddError("An error occurred while trying to persist data");

        return ValidationResult;
    }
    
    protected async Task<ValidationResult> PersistDataAsync(IUnitOfWork uow, string errorMessage)
    {
        if (!await uow.CommitAsync()) AddError(errorMessage);

        return ValidationResult;
    }

}