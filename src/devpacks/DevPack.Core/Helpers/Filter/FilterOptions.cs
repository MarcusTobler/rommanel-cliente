using System.Diagnostics.CodeAnalysis;

namespace DevPack.Core.Helpers.Filter
{
    public static class FilterOptions
    {
        internal const DynamicallyAccessedMemberTypes DynamicallyAccessedMembers =
            DynamicallyAccessedMemberTypes.PublicParameterlessConstructor;

        public static string DefaultName = string.Empty;

        public static IFilterOptions<TFilter> ToOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMembers)] TFilter>(
            TFilter filter)
            where TFilter : Filter
        {
            DefaultName = nameof(TFilter);
            return FilterWrapper<TFilter>.ToFilter(filter);
        }
    }
}