using System.Reflection;
using System.Runtime.CompilerServices;

namespace DevPack.Core.Enum
{
    public abstract record Enumeration<TEnum>(int Value, string Name) : Enumeration<TEnum, int>(Value, Name)
        where TEnum : Enumeration<TEnum, int>;
    
    public abstract record Enumeration<TEnum, TValue> : IComparable<Enumeration<TEnum, TValue>>
        where TEnum : Enumeration<TEnum, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        private static readonly Lazy<Dictionary<string, TEnum?>> _fromName =
            new Lazy<Dictionary<string, TEnum?>>(() => GetAllOptions().ToDictionary(item => item.Name)!);

        private static readonly Lazy<Dictionary<string, TEnum?>> _fromNameIgnoreCase =
            new Lazy<Dictionary<string, TEnum?>>(() => GetAllOptions().ToDictionary(item => item.Name, StringComparer.OrdinalIgnoreCase)!);

        private static readonly Lazy<Dictionary<TValue, TEnum?>> _fromValue =
            new Lazy<Dictionary<TValue, TEnum?>>(() =>
            {
                // multiple enums with same value are allowed but store only one per value
                var dictionary = new Dictionary<TValue, TEnum?>();
                foreach (var item in GetAllOptions())
                {
                    if (item._value != null && !dictionary.ContainsKey(item._value))
                        dictionary.Add(item._value, item);
                }
                return dictionary!;
            });
        
        private static IEnumerable<TEnum> GetAllOptions()
        {
            var baseType = typeof(TEnum);
            var enumTypes = Assembly.GetAssembly(baseType)?.GetTypes().Where(t => baseType.IsAssignableFrom(t));

            var options = new List<TEnum>();
            (enumTypes ?? Type.EmptyTypes).ToList().ForEach(x =>
            {
                var typeEnumOptions = x.GetFieldsOfType<TEnum>();
                options.AddRange(typeEnumOptions);
            });
            return options.OrderBy(t => t.Name).ToList();
        }
        
        /// <summary>
        /// Gets a collection containing all the instances of <see cref="Enumeration{TEnum, TValue}"/>.
        /// </summary>
        /// <value>A <see cref="IReadOnlyCollection{TEnum}"/> containing all the instances of <see cref="Enumeration{TEnum, TValue}"/>.</value>
        /// <remarks>Retrieves all the instances of <see cref="Enumeration{TEnum, TValue}"/> referenced by public static read-only fields in the current class or its bases.</remarks>
        public static IReadOnlyCollection<TEnum?> List =>
            _fromName.Value.Values
                .ToList()
                .AsReadOnly();

        private readonly TValue? _value;
        private readonly string _name;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>A <typeparamref name="TValue"/> that is the value of the <see cref="Enumeration{TEnum, TValue}"/>.</value>
        public TValue? Value => _value;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>A <see cref="String"/> that is the name of the <see cref="Enumeration{TEnum, TValue}"/>.</value>
        public string Name => _name;
        
        protected Enumeration(TValue value, string name)
        {
            if (string.IsNullOrEmpty(name))
                ThrowHelper.ThrowArgumentNullOrEmptyException(nameof(name));
            if (value == null)
                ThrowHelper.ThrowArgumentNullException(nameof(value));

            _name = name;
            _value = value;
        }
        
        /// <summary>
        /// Gets the type of the inner value.
        /// </summary>
        /// <value>A <see name="System.Type"/> that is the type of the value of the <see cref="Enumeration{TEnum, TValue}"/>.</value>
        public Type GetValueType() =>
            typeof(TValue);

        /// <summary>
        /// Gets the item associated with the specified name.
        /// </summary>
        /// <param name="name">The name of the item to get.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case during the comparison; otherwise, <c>false</c>.</param>
        /// <returns>
        /// The item associated with the specified name. 
        /// If the specified name is not found, throws a <see cref="KeyNotFoundException"/>.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is <c>null</c>.</exception> 
        /// <exception cref="EnumerationNotFoundException"><paramref name="name"/> does not exist.</exception> 
        /// <seealso cref="Enumeration{TEnum, TValue}.TryFromName(string, out TEnum)"/>
        /// <seealso cref="Enumeration{TEnum, TValue}.TryFromName(string, bool, out TEnum)"/>
        public static TEnum? FromName(string name, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(name))
                ThrowHelper.ThrowArgumentNullOrEmptyException(nameof(name));

            if (ignoreCase)
                return FromName(_fromNameIgnoreCase.Value);
            else
                return FromName(_fromName.Value);

            TEnum? FromName(IReadOnlyDictionary<string, TEnum?> dictionary)
            {
                if (!dictionary.TryGetValue(name, out var result))
                {
                    ThrowHelper.ThrowNameNotFoundException<TEnum, TValue>(name);
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the item associated with the specified name.
        /// </summary>
        /// <param name="name">The name of the item to get.</param>
        /// <param name="result">
        /// When this method returns, contains the item associated with the specified name, if the key is found; 
        /// otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="Enumeration{TEnum, TValue}"/> contains an item with the specified name; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is <c>null</c>.</exception> 
        /// <seealso cref="Enumeration{TEnum, TValue}.FromName(string, bool)"/>
        /// <seealso cref="Enumeration{TEnum, TValue}.TryFromName(string, bool, out TEnum)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryFromName(string name, out TEnum? result) =>
            TryFromName(name, false, out result);

        /// <summary>
        /// Gets the item associated with the specified name.
        /// </summary>
        /// <param name="name">The name of the item to get.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case during the comparison; otherwise, <c>false</c>.</param>
        /// <param name="result">
        /// When this method returns, contains the item associated with the specified name, if the name is found; 
        /// otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="Enumeration{TEnum, TValue}"/> contains an item with the specified name; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is <c>null</c>.</exception> 
        /// <seealso cref="Enumeration{TEnum, TValue}.FromName(string, bool)"/>
        /// <seealso cref="Enumeration{TEnum, TValue}.TryFromName(string, out TEnum)"/>
        public static bool TryFromName(string name, bool ignoreCase, out TEnum? result)
        {
            if (!string.IsNullOrEmpty(name))
                return ignoreCase
                    ? _fromNameIgnoreCase.Value.TryGetValue(name, out result)
                    : _fromName.Value.TryGetValue(name, out result);
            
            result = default;
            
            return false;
        }

        /// <summary>
        /// Gets an item associated with the specified value.
        /// </summary>
        /// <param name="value">The value of the item to get.</param>
        /// <returns>
        /// The first item found that is associated with the specified value.
        /// If the specified value is not found, throws a <see cref="KeyNotFoundException"/>.
        /// </returns>
        /// <exception cref="EnumerationNotFoundException"><paramref name="value"/> does not exist.</exception> 
        /// <seealso cref="Enumeration{TEnum, TValue}.FromValue(TValue, TEnum)"/>
        /// <seealso cref="Enumeration{TEnum, TValue}.TryFromValue(TValue, out TEnum)"/>
        public static TEnum FromValue(TValue? value)
        {
            if (value == null)
                ThrowHelper.ThrowArgumentNullException(nameof(value));

            if (!_fromValue.Value.TryGetValue(value!, out var result))
            {
                ThrowHelper.ThrowValueNotFoundException<TEnum, TValue>(value!);
            }
            
            return result!;
        }

        /// <summary>
        /// Gets an item associated with the specified value.
        /// </summary>
        /// <param name="value">The value of the item to get.</param>
        /// <param name="defaultValue">The value to return when item not found.</param>
        /// <returns>
        /// The first item found that is associated with the specified value.
        /// If the specified value is not found, returns <paramref name="defaultValue"/>.
        /// </returns>
        /// <seealso cref="Enumeration{TEnum, TValue}.FromValue(TValue)"/>
        /// <seealso cref="Enumeration{TEnum, TValue}.TryFromValue(TValue, out TEnum)"/>
        public static TEnum FromValue(TValue? value, TEnum? defaultValue)
        {
            if (value == null)
                ThrowHelper.ThrowArgumentNullException(nameof(value));

            return (!_fromValue.Value.TryGetValue(value!, out var result) 
                ? defaultValue 
                : result)!;
        }

        /// <summary>
        /// Gets an item associated with the specified value.
        /// </summary>
        /// <param name="value">The value of the item to get.</param>
        /// <param name="result">
        /// When this method returns, contains the item associated with the specified value, if the value is found; 
        /// otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="Enumeration{TEnum, TValue}"/> contains an item with the specified name; otherwise, <c>false</c>.
        /// </returns>
        /// <seealso cref="Enumeration{TEnum, TValue}.FromValue(TValue)"/>
        /// <seealso cref="Enumeration{TEnum, TValue}.FromValue(TValue, TEnum)"/>
        public static bool TryFromValue(TValue? value, out TEnum? result)
        {
            if (value != null) 
                return _fromValue.Value.TryGetValue(value, out result);
            
            result = default;
            
            return false;
        }

        public override string ToString() =>
            _name;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() =>
            _value!.GetHashCode();

//        public override bool Equals(object? obj) =>
//            (obj is Enumeration<TEnum, TValue> other) && Equals(other);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Enumeration{TEnum, TValue}"/> value.
        /// </summary>
        /// <param name="other">An <see cref="Enumeration{TEnum, TValue}"/> value to compare to this instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> has the same value as this instance; otherwise, <c>false</c>.</returns>
        public virtual bool Equals(Enumeration<TEnum, TValue>? other)
        {
            // check if same instance
            if (object.ReferenceEquals(this, other))
                return true;

            // it's not same instance so 
            // check if it's not null and is same value
            if (other is null)
                return false;

            return _value != null && _value.Equals(other._value);
        }

        /// <summary>
        /// When this instance is one of the specified <see cref="Enumeration{TEnum, TValue}"/> parameters.
        /// Execute the action in the subsequent call to Then().
        /// </summary>
        /// <param name="EnumerationWhen">A collection of <see cref="Enumeration{TEnum, TValue}"/> values to compare to this instance.</param>
        /// <returns>A executor object to execute a supplied action.</returns>
        public EnumerationThen<TEnum, TValue> When(Enumeration<TEnum, TValue> enumerationWhen) =>
            new EnumerationThen<TEnum, TValue>(this.Equals(enumerationWhen), false, this);

        /// <summary>
        /// When this instance is one of the specified <see cref="Enumeration{TEnum, TValue}"/> parameters.
        /// Execute the action in the subsequent call to Then().
        /// </summary>
        /// <param name="smartEnums">A collection of <see cref="Enumeration{TEnum, TValue}"/> values to compare to this instance.</param>
        /// <returns>A executor object to execute a supplied action.</returns>
        public EnumerationThen<TEnum, TValue> When(params Enumeration<TEnum, TValue>[] enumerations) =>
            new EnumerationThen<TEnum, TValue>(enumerations.Contains(this), false, this);

        /// <summary>
        /// When this instance is one of the specified <see cref="Enumeration{TEnum, TValue}"/> parameters.
        /// Execute the action in the subsequent call to Then().
        /// </summary>
        /// <param name="enumerations">A collection of <see cref="Enumeration{TEnum, TValue}"/> values to compare to this instance.</param>
        /// <returns>A executor object to execute a supplied action.</returns>
        public EnumerationThen<TEnum, TValue> When(IEnumerable<Enumeration<TEnum, TValue>> enumerations) =>
            new EnumerationThen<TEnum, TValue>(enumerations.Contains(this), false, this);

//        public static bool operator ==(Enumeration<TEnum, TValue> left, Enumeration<TEnum, TValue> right)
//        {
//            // Handle null on left side
//            if (left is null)
//                return right is null; // null == null = true

//            // Equals handles null on right side
//            return left.Equals(right);
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static bool operator !=(Enumeration<TEnum, TValue> left, Enumeration<TEnum, TValue> right) =>
//            !(left == right);

        /// <summary>
        /// Compares this instance to a specified <see cref="Enumeration{TEnum, TValue}"/> and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">An <see cref="Enumeration{TEnum, TValue}"/> value to compare to this instance.</param>
        /// <returns>A signed number indicating the relative values of this instance and <paramref name="other"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int CompareTo(Enumeration<TEnum, TValue>? other) =>
            _value!.CompareTo(other!._value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Enumeration<TEnum, TValue> left, Enumeration<TEnum, TValue> right) =>
            left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Enumeration<TEnum, TValue> left, Enumeration<TEnum, TValue> right) =>
            left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Enumeration<TEnum, TValue> left, Enumeration<TEnum, TValue> right) =>
            left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Enumeration<TEnum, TValue> left, Enumeration<TEnum, TValue> right) =>
            left.CompareTo(right) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator TValue?(Enumeration<TEnum, TValue> smartEnum) =>
            smartEnum._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Enumeration<TEnum, TValue>?(TValue? value) =>
            FromValue(value);
    }
}