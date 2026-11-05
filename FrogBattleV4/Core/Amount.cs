using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace FrogBattleV4.Core;

public readonly struct Amount : INumber<Amount>
{
    private const long Decimals = 1000;
    private readonly long _l;

    private Amount(long value) => _l = value;

    public static int Radix => 10;
    public static Amount One => new(Decimals);
    public static Amount Zero => default;
    public static Amount AdditiveIdentity => Zero;
    public static Amount MultiplicativeIdentity => One;

    int IComparable.CompareTo(object? obj) =>
        obj is not Amount other
            ? throw new ArgumentException($"Object must be of type {nameof(Amount)} to compare.", nameof(obj))
            : CompareTo(other);

    public int CompareTo(Amount other) => _l.CompareTo(other._l);
    public bool Equals(Amount other) => _l.Equals(_l);

    public override bool Equals(object? obj) => obj is Amount other && Equals(other);
    public override int GetHashCode() => _l.GetHashCode();

    // Formatting needs special logic - Amount supposedly has decimals
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        throw new NotImplementedException();
        return _l.ToString(format, formatProvider);
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        throw new NotImplementedException();
        return _l.TryFormat(destination, out charsWritten, format, provider);
    }

    public static Amount Parse(string s, IFormatProvider? provider) => TryParse(s, provider, out var result)
        ? result
        : throw new FormatException($"Cannot parse '{s}' to Amount");

    public static Amount Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => TryParse(s, provider, out var result)
        ? result
        : throw new FormatException($"Cannot parse '{s}' to Amount");

    public static Amount Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => TryParse(s, style, provider, out var result)
        ? result
        : throw new FormatException($"Cannot parse '{s}' to Amount");

    public static Amount Parse(string s, NumberStyles style, IFormatProvider? provider) => TryParse(s, style, provider, out var result)
        ? result
        : throw new FormatException($"Cannot parse '{s}' to Amount");

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Amount result)
    {
        throw new NotImplementedException();
        if (long.TryParse(s, provider, out long value))
        {
            result = new Amount(value * Decimals);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Amount result)
    {
        throw new NotImplementedException();
        if (long.TryParse(s, provider, out long value))
        {
            result = new Amount(value * Decimals);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Amount result)
    {
        throw new NotImplementedException();
        if (long.TryParse(s, style, provider, out long value))
        {
            result = new Amount(value * Decimals);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Amount result)
    {
        throw new NotImplementedException();
        if (long.TryParse(s, style, provider, out long value))
        {
            result = new Amount(value * Decimals);
            return true;
        }

        result = default;
        return false;
    }

    public static bool operator ==(Amount left, Amount right) => left.Equals(right);
    public static bool operator !=(Amount left, Amount right) => !(left == right);
    public static bool operator >(Amount left, Amount right) => left.CompareTo(right) > 0;
    public static bool operator >=(Amount left, Amount right) => left.CompareTo(right) >= 0;
    public static bool operator <(Amount left, Amount right) => left.CompareTo(right) < 0;
    public static bool operator <=(Amount left, Amount right) => left.CompareTo(right) <= 0;
    public static Amount operator +(Amount value) => new(+value._l);
    public static Amount operator -(Amount value) => new(-value._l);
    public static Amount operator +(Amount left, Amount right) => new(left._l + right._l);
    public static Amount operator -(Amount left, Amount right) => new(left._l - right._l);
    public static Amount operator ++(Amount value) => new(value._l + 1);
    public static Amount operator --(Amount value) => new(value._l - 1);
    public static Amount operator *(Amount left, Amount right) => new(left._l * right._l / Decimals);
    public static Amount operator /(Amount left, Amount right) => new(left._l * Decimals / right._l);
    public static Amount operator %(Amount left, Amount right)
    {
        throw new NotSupportedException("Modulo is not supported for rational numbers.");
    }

    public static Amount Abs(Amount value) => new(long.Abs(value._l));

    public static bool IsPositive(Amount value) => long.IsPositive(value._l);
    public static bool IsNegative(Amount value) => long.IsNegative(value._l);
    public static bool IsInteger(Amount value) => value._l % Decimals == 0;
    public static bool IsEvenInteger(Amount value) => IsInteger(value) && long.IsEvenInteger(value._l / Decimals);
    public static bool IsOddInteger(Amount value) => IsInteger(value) && long.IsOddInteger(value._l / Decimals);
    public static bool IsZero(Amount value) => value.Equals(Zero);
    public static Amount MinMagnitude(Amount x, Amount y) => new(long.MinMagnitude(x._l, y._l));
    public static Amount MaxMagnitude(Amount x, Amount y) => new(long.MaxMagnitude(x._l, y._l));

    public static implicit operator Amount(long value) => new(value * Decimals);
    public static explicit operator long(Amount value) => value._l / Decimals;
    public static explicit operator double(Amount value) => value._l;

    static bool INumberBase<Amount>.IsComplexNumber(Amount value) => false;
    static bool INumberBase<Amount>.IsImaginaryNumber(Amount value) => false;
    static bool INumberBase<Amount>.IsCanonical(Amount value) => true;
    static bool INumberBase<Amount>.IsNormal(Amount value) => true;
    static bool INumberBase<Amount>.IsNaN(Amount value) => false;
    static bool INumberBase<Amount>.IsFinite(Amount value) => true;
    static bool INumberBase<Amount>.IsInfinity(Amount value) => false;
    static bool INumberBase<Amount>.IsNegativeInfinity(Amount value) => false;
    static bool INumberBase<Amount>.IsPositiveInfinity(Amount value) => false;
    static bool INumberBase<Amount>.IsRealNumber(Amount value) => true;
    static bool INumberBase<Amount>.IsSubnormal(Amount value) => false;
    static Amount INumberBase<Amount>.MinMagnitudeNumber(Amount x, Amount y) => MinMagnitude(x, y);
    static Amount INumberBase<Amount>.MaxMagnitudeNumber(Amount x, Amount y) => MaxMagnitude(x, y);
    static bool INumberBase<Amount>.TryConvertFromChecked<TOther>(TOther value, out Amount result) => throw new NotImplementedException();
    static bool INumberBase<Amount>.TryConvertFromSaturating<TOther>(TOther value, out Amount result) => throw new NotImplementedException();
    static bool INumberBase<Amount>.TryConvertFromTruncating<TOther>(TOther value, out Amount result) => throw new NotImplementedException();
    static bool INumberBase<Amount>.TryConvertToChecked<TOther>(Amount value, [MaybeNullWhen(false)] out TOther result) => throw new NotImplementedException();
    static bool INumberBase<Amount>.TryConvertToSaturating<TOther>(Amount value, [MaybeNullWhen(false)] out TOther result) => throw new NotImplementedException();
    static bool INumberBase<Amount>.TryConvertToTruncating<TOther>(Amount value, [MaybeNullWhen(false)] out TOther result) => throw new NotImplementedException();
}