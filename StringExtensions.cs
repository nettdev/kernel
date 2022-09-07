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

    public static string UrlEncode(this string url)
    {
        return Uri.EscapeDataString(url);
    }

    public static bool NotEqual(this string original, string compareTo)
    {
        return !original.Equals(compareTo);
    }

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

    private static string UrlCombine(string path1, string path2)
    {
        path1 = path1.TrimEnd('/') + "/";
        path2 = path2.TrimStart('/');

        return Path.Combine(path1, path2)
            .Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }

    public static string UrlPathCombine(this string path1, params string[] path2)
    {
        path1 = path1.TrimEnd('/') + "/";
        foreach (var s in path2)
        {
            path1 = UrlCombine(path1, s).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        return path1;
    }

    public static string TruncateSensitiveInformation(this string part)
    {
        if (!part.IsPresent())
            return string.Empty;

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

    public static string FromBase64ToString(this string str)
    {
        return (Encoding.UTF8).GetString(FromBase64(str));
    }

    public static byte[] FromBase64(this string str)
    {
        return Convert.FromBase64String(str);
    }
    public static string ToBase64(this string str)
    {
        return ToBase64((Encoding.UTF8).GetBytes(str));
    }

    public static string ToBase64(this byte[] data)
    {
        return Convert.ToBase64String(data);
    }
    public static byte[] FromPlainHexDumpStyleToByteArray(this string hex)
    {
        if (hex.Length % 2 == 1)
            throw new Exception("The binary key cannot have an odd number of digits");

        byte[] arr = new byte[hex.Length >> 1];

        for (int i = 0; i < hex.Length >> 1; ++i)
        {
            arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
        }

        return arr;
    }

    private static int GetHexVal(char hex)
    {
        int val = (int)hex;
        return val - (val < 58 ? 48 : 55);
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