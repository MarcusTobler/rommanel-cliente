using System.Linq.Expressions;
using FluentValidation;

namespace DevPack.Domain.Messaging.Commands;

public class CommandValidatorBuilder<TCommand> where TCommand : Command
{
    private readonly AbstractValidator<TCommand> _validator;
    
    public CommandValidatorBuilder(AbstractValidator<TCommand> validator)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }
    
    public IRuleBuilderInitial<TCommand, TProperty> RuleFor<TProperty>(
        Expression<Func<TCommand, TProperty>> expression)
    {
        return _validator.RuleFor(expression);
    }
}