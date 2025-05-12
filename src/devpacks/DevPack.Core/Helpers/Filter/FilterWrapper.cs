using System.Diagnostics.CodeAnalysis;

namespace DevPack.Core.Helpers.Filter
{
    public class FilterWrapper<[DynamicallyAccessedMembers(FilterOptions.DynamicallyAccessedMembers)] TFilter> :
        IFilterOptions<TFilter> where TFilter : class
    {
        public TFilter Filter { get; protected set; }

        protected FilterWrapper(TFilter options) => (Filter) = (options);

        public static FilterWrapper<TFilter> ToFilter([DynamicallyAccessedMembers(FilterOptions.DynamicallyAccessedMembers)] TFilter filter)
        {
            return new FilterWrapper<TFilter>(filter);
        }
    }
}