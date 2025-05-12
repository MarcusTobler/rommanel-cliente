namespace DevPack.Domain.Abstractions.Messaging;

public interface ICommandResult
{
    public bool IsSuccess { get; }
    public bool HasValidationFailure { get; }
}

public interface ICommandResult<out TValue> : ICommandResult
{
    public TValue Value { get; }
}