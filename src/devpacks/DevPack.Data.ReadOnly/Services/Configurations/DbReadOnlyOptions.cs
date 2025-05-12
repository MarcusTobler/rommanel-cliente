// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public class DbReadOnlyOptions
{
    internal IList<IDbReadOnlyOptionsExtensions> Extensions { get; } = new List<IDbReadOnlyOptionsExtensions>();

    public void RegisterExtensions(IDbReadOnlyOptionsExtensions extension)
    {
        ArgumentNullException.ThrowIfNull(extension, nameof(extension));
        
        Extensions.Add(extension);
    }
}