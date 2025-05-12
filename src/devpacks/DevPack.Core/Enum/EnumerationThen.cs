namespace DevPack.Core.Enum
{
    public readonly struct EnumerationThen<TEnum, TValue>
        where TEnum : Enumeration<TEnum, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        private readonly bool isMatch;
        private readonly Enumeration<TEnum, TValue> enumeration;
        private readonly bool stopEvaluating;

        internal EnumerationThen(bool isMatch, bool stopEvaluating, Enumeration<TEnum, TValue> enumeration)
        {
            this.isMatch = isMatch;
            this.enumeration = enumeration;
            this.stopEvaluating = stopEvaluating;
        }

        /// <summary>
        /// Calls <paramref name="doThis"/> Action when the preceding When call matches.
        /// </summary>
        /// <param name="doThis">Action method to call.</param>
        /// <returns>A chainable instance of CaseWhen for more when calls.</returns>
        public EnumerationWhen<TEnum, TValue> Then(Action doThis)
        {
            if (!stopEvaluating && isMatch)
                doThis();

            return new EnumerationWhen<TEnum, TValue>(stopEvaluating || isMatch, enumeration);
        }
    }
}