using DevPack.Core.Extensions;
using FluentValidation;

namespace DevPack.Domain.Messaging.Commands;

public abstract partial class CommandValidator<TCommand> : AbstractValidator<TCommand>, 
    ICommandValidator<TCommand> where TCommand : Command
{

    public abstract void Validate();
    
    protected bool BeValidUri(string link) => 
        string.IsNullOrEmpty(link) || Uri.TryCreate(link, UriKind.Absolute, out _);

    protected bool BeValidCpf(string cpf) => 
        cpf.IsValidCpf();
    
    protected bool BeValidCnpj(string cnpj) => 
        cnpj.IsValidCnpj();
    
    protected bool Be18YearsOrOlder(DateTime date) => 
        DateTime.Today.AddYears(-18) >= date;
 
    protected bool BeValidBirthDate(DateTime date) =>
        date != default && date <= DateTime.Today;
    
    protected bool BeValidPhoneNumber(string phoneNumber) =>
        PhoneNumberRegex().IsMatch(phoneNumber);
    
    // Expressão regular para validar:
    // - Código do país opcional: (+55)
    // - Código da região opcional: (11) ou 11
    // - Número do telefone: 9999-9999 ou 99999-9999 (com ou sem hífen)
    [System.Text.RegularExpressions.GeneratedRegex(@"^(\+\d{2}\s?)?(\(\d{2}\)|\d{2})?\s?\d{4,5}-?\d{4}$")]
    private static partial System.Text.RegularExpressions.Regex PhoneNumberRegex();

}