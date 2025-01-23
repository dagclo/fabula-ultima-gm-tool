using System;
/// <summary>
/// https://stackoverflow.com/questions/4135317/make-first-letter-of-a-string-upper-case-with-maximum-performance
/// </summary>
public static class StringExtensions
{
    public static string ToCamelCase(this string input)
    {
        var value = input?.ToLowerInvariant();
        switch (value)
        {
            case null: throw new ArgumentNullException(nameof(input));
            case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            case "willpower": return "WillPower";
            default: return value[0].ToString().ToUpper() + value.Substring(1);
        }
    }
}
