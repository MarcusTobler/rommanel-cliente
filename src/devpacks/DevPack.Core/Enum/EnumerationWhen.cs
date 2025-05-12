namespace DevPack.Core.Enum
{
    public readonly struct EnumerationWhen<TEnum, TValue>
        where TEnum : Enumeration<TEnum, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        private readonly Enumeration<TEnum, TValue> enumeration;
        private readonly bool stopEvaluating;

        internal EnumerationWhen(bool stopEvaluating, Enumeration<TEnum, TValue> enumeration)
        {
            this.stopEvaluating = stopEvaluating;
            this.enumeration = enumeration;
        }

        /// <summary>
        /// Execute this action if no other calls to When have matched.
        /// </summary>
        /// <param name="action">The Action to call.</param>
        public void Default(Action action)
        {
            if (!stopEvaluating)
            {
                action();
            }
        }

        /// <summary>
        /// When this instance is one of the specified <see cref="SmartEnum{TEnum, TValue}"/> parameters.
        /// Execute the action in the subsequent call to Then().
        /// </summary>
        /// <param name="smartEnumWhen">A collection of <see cref="SmartEnum{TEnum, TValue}"/> values to compare to this instance.</param>
        /// <returns>A executor object to execute a supplied action.</returns>
        public EnumerationThen<TEnum, TValue> When(Enumeration<TEnum, TValue> enumerationWhen) =>
            new EnumerationThen<TEnum, TValue>(isMatch: enumeration.Equals(enumerationWhen), stopEvaluating: stopEvaluating, enumeration: enumeration);

        /// <summary>
        /// When this instance is one of the specified <see cref="SmartEnum{TEnum, TValue}"/> parameters.
        /// Execute the action in the subsequent call to Then().
        /// </summary>
        /// <param name="smartEnums">A collection of <see cref="SmartEnum{TEnum, TValue}"/> values to compare to this instance.</param>
        /// <returns>A executor object to execute a supplied action.</returns>
        public EnumerationThen<TEnum, TValue> When(params Enumeration<TEnum, TValue>[] enumerations) =>
            new EnumerationThen<TEnum, TValue>(isMatch: enumerations.Contains(enumeration), stopEvaluating: stopEvaluating, enumeration: enumeration);

        /// <summary>
        /// When this instance is one of the specified <see cref="SmartEnum{TEnum, TValue}"/> parameters.
        /// Execute the action in the subsequent call to Then().
        /// </summary>
        /// <param name="smartEnums">A collection of <see cref="SmartEnum{TEnum, TValue}"/> values to compare to this instance.</param>
        /// <returns>A executor object to execute a supplied action.</returns>
        public EnumerationThen<TEnum, TValue> When(IEnumerable<Enumeration<TEnum, TValue>> enumerations) =>
            new EnumerationThen<TEnum, TValue>(isMatch: enumerations.Contains(enumeration), stopEvaluating: stopEvaluating, enumeration: enumeration);
    }
}