using System.Diagnostics.CodeAnalysis;

namespace DevPack.Core.Helpers.Filter
{
    public interface IFilterOptions<[DynamicallyAccessedMembers(FilterOptions.DynamicallyAccessedMembers)] out TFilter>
        where TFilter : class
    {
        TFilter Filter { get; }
    }
}