using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.String;

namespace Mobnet.SharedKernel;

public static class StringExtensions
{
    private static char sensitive = '*';
    private static readonly Regex UrlizeRegex = new Regex(@"[^A-Za-z0-9_~]+", RegexOptions.Multiline | RegexOptions.Compiled);
    private static readonly Regex EmailRegex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Compiled);

    public static bool IsEmail(this string field)
    {
        return field.IsPresent() && EmailRegex.IsMatch(field);
    }

    public static bool IsMissing(this string value)
    {
        return IsNullOrEmpty(value);
    }

    public static bool IsPresent(this string value)
    {
        return !IsNullOrWhiteSpace(value);
    }

    public static string TruncateSensitiveInformation(this string part)
    {
        var truncatedString = new char[part.Length];
        truncatedString[0] = part[0];

        for (var i = 1; i < part.Length - 1; i++)
        {
            truncatedString[i] = sensitive;
        }
        truncatedString[part.Length - 1] = part[part.Length - 1];

        return new string(truncatedString);
    }

    public static string ToSha256(this string value)
    {
        var crypt = SHA256.Create();
        var hash = new StringBuilder();
        byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(value));

        foreach (byte theByte in crypto)
        {
            hash.Append(theByte.ToString("x2"));
        }

        return hash.ToString();
    }

    public static string RemoveDiacritics(this string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var finalText = new char[text.Length];
        var lastIndex = 0;
        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                finalText[lastIndex++] = c;
            }
        }
        Array.Resize(ref finalText, lastIndex);

        return new string(finalText).Normalize(NormalizationForm.FormC);
    }

    public static string Urlize(this string str)
    {
        var title = str.Trim().ToLower().RemoveDiacritics();

        title = UrlizeRegex.Replace(title, "-");

        if (title.StartsWith('-'))
            title = title[1..];

        if (title.EndsWith('-'))
            title = title[..^1];

        return title;
    }

    public static string OnlyNumbers(this string str)
    {
        var onlyNumbers = new char[str.Length];
        var lastIndex = 0;

        foreach (var c in str)
        {
            if (c < '0' || c > '9') continue;

            onlyNumbers[lastIndex++] = c;
        }
        Array.Resize(ref onlyNumbers, lastIndex);
        return new string(onlyNumbers);
    }

    public static string Capitalize(this string value, bool isRestLower)
    {
        var spanChars = value.AsSpan();
        var newSpan = new Span<char>(new char[value.Length]);
        spanChars.CopyTo(newSpan);

        if (isRestLower)
        {
            spanChars.ToLowerInvariant(newSpan);
        }

        newSpan[0] = char.ToUpper(spanChars[0]);

        return newSpan.ToString();
    }
}