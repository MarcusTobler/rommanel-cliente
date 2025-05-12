using System.Buffers;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DevPack.Core.Extensions;

public static partial class StringExtensions
{
    private static char _sensitive = '*';
    private static char _at = '@';
    private static readonly Regex UrlizeRegex = new Regex("[^A-Za-z0-9_~]+", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex EmailRegex = new Regex("^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", RegexOptions.Compiled);

    public static string UrlEncode(this string url) => Uri.EscapeDataString(url);

    public static bool NotEqual(this string original, string compareTo) => !original.Equals(compareTo);

    public static bool IsEmail(this string field) => field.IsPresent() && StringExtensions.EmailRegex.IsMatch(field);

    public static bool IsMissing(this string value) => string.IsNullOrEmpty(value);

    public static bool IsPresent(this string? value) => !string.IsNullOrWhiteSpace(value);
    
    public static bool ContainsAny(this string theString, params string[] tokens)
    {
        return tokens.Any(theString.Contains);
    }

    public static bool EqualsAny(this string theString, params string[] tokens)
    {
        return tokens.Any(token => theString.Equals(token, StringComparison.InvariantCultureIgnoreCase));
    }

    public static Uri ToUri(this string url, UriKind kind = UriKind.RelativeOrAbsolute)
    {
        return new Uri(url, kind);
    }

    public static DateTime? ToDate(this string theString, DateTime? defaultDate = null)
    {
        DateTime date;
        var success = DateTime.TryParse(theString, out date);
        return success ? date : defaultDate;
    }

    public static bool ToBool(this string theString)
    {
        bool value;
        var success = bool.TryParse(theString, out value);
        return success && value;
    }

    public static int ToInt(this string theString, int defaultValue = 0)
    {
        int number;
        var success = int.TryParse(theString, out number);
        return success ? number : defaultValue;
    }

    public static bool IsNullOrWhitespace(this string theString)
    {
        return string.IsNullOrWhiteSpace(theString);
    }

    public static bool IsAlphaNumeric(this string theString)
    {
        return string.IsNullOrWhiteSpace(theString);
    }

    public static bool IsValidUrl(this string url)
    {
        return !string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    public static bool IsValidCpf(this string cpf)
    {
        cpf = cpf.Replace(".", "").Replace("-", "").Trim();
        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            return false;

        // Verifica CPFs inválidos conhecidos
        if (cpf.All(c => c == cpf[0]))
            return false;

        // Calcula primeiro dígito verificador
        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
    
        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        if (int.Parse(cpf[9].ToString()) != digit1)
            return false;

        // Calcula segundo dígito verificador
        sum = 0;
        for (var i = 0; i < 10; i++)
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
    
        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return int.Parse(cpf[10].ToString()) == digit2;
    }

    public static bool IsValidCnpj(this string cnpj)
    {
        cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
        if (cnpj.Length != 14 || !cnpj.All(char.IsDigit))
            return false;

        // Verifica CNPJs inválidos conhecidos
        if (cnpj.All(c => c == cnpj[0]))
            return false;

        // Calcula primeiro dígito verificador
        int[] weights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var sum = 0;
        for (var i = 0; i < 12; i++)
            sum += int.Parse(cnpj[i].ToString()) * weights1[i];
    
        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        if (int.Parse(cnpj[12].ToString()) != digit1)
            return false;

        // Calcula segundo dígito verificador
        int[] weights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        sum = 0;
        for (var i = 0; i < 13; i++)
            sum += int.Parse(cnpj[i].ToString()) * weights2[i];
    
        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return int.Parse(cnpj[13].ToString()) == digit2;
    }
    
    public static string ToSlug(this string str)
    {
        var acentos = new[]
        {
            "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È",
            "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â",
            "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û"
        };
        var semAcento = new[]
        {
            "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E",
            "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a",
            "e", "i", "o", "u", "A", "E", "I", "O", "U"
        };

        for (var i = 0; i < acentos.Length; i++) str = str.Replace(acentos[i], semAcento[i]);

        var caracteresEspeciais = new[]
        {
            "¹", "²", "³", "£", "¢", "¬", "º", "¨", "\"", "'", ".", ",", "-", ":", "(", ")", "ª", "|", "\\\\", "°",
            "_", "@", "#", "!", "$", "%", "&", "*", ";", "/", "<", ">", "?", "[", "]", "{", "}", "=", "+", "§", "´",
            "`", "^", "~"
        };

        for (var i = 0; i < caracteresEspeciais.Length; i++) str = str.Replace(caracteresEspeciais[i], "");

        return str.Trim().ToLower().Replace("  ", " ").Replace(" ", "-");
    }
    
    public static string ReplaceSpecialCharacters(this string str)
    {
        var acentos = new[]
        {
            "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È",
            "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â",
            "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û"
        };
        var semAcento = new[]
        {
            "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E",
            "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a",
            "e", "i", "o", "u", "A", "E", "I", "O", "U"
        };

        for (var i = 0; i < acentos.Length; i++) str = str.Replace(acentos[i], semAcento[i]);

        var caracteresEspeciais = new[]
        {
            "¹", "²", "³", "£", "¢", "¬", "º", "¨", "\"", "'", ".", ",", "-", ":", "(", ")", "ª", "|", "\\\\", "°",
            "_", "@", "#", "!", "$", "%", "&", "*", ";", "/", "<", ">", "?", "[", "]", "{", "}", "=", "+", "§", "´",
            "`", "^", "~"
        };

        for (var i = 0; i < caracteresEspeciais.Length; i++) str = str.Replace(caracteresEspeciais[i], "");

        return str.Trim().Replace("  ", " ");
    }

    public static bool EmailIsValid(this string email)
    {
        return Regex.Match(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$").Success;
    }

    public static string ToNormalize(this string text)
    {
        var acentos = new[]
        {
            "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È",
            "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â",
            "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û"
        };
        var semAcento = new[]
        {
            "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E",
            "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a",
            "e", "i", "o", "u", "A", "E", "I", "O", "U"
        };

        for (var i = 0; i < acentos.Length; i++) text = text.Replace(acentos[i], semAcento[i]);

        var caracteresEspeciais = new[]
        {
            "¹", "²", "³", "£", "¢", "¬", "º", "¨", "\"", "'", ",", "(", ")", "ª", "|", "\\\\", "°",
            "_", "@", "#", "!", "$", "%", "&", "*", ";", "/", "<", ">", "?", "[", "]", "{", "}", "=", "+", "§", "´",
            "`", "^", "~"
        };

        for (var i = 0; i < caracteresEspeciais.Length; i++) text = text.Replace(caracteresEspeciais[i], "");

        return text.Trim().Replace("  ", " ").ToUpper();
    }

    public static string RemoveDiacritics(this string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
    
    public static string RemoveSpecialCharacters(this string text)
    {
        var stringBuilder = new StringBuilder();

        foreach (var c in from c in text let unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c) where unicodeCategory != UnicodeCategory.NonSpacingMark select c)
        {
            stringBuilder.Append(c);
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
    
    public static string RemoveAccents(this string text)
    {
        var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(text);
        return Encoding.ASCII.GetString(bytes);
    }
    
    public static string RemoveDiacriticsAndSpecialCharacters(this string text)
    {
        return text.RemoveDiacritics().RemoveSpecialCharacters();
    }
    
    public static string RemoveDiacriticsAndAccents(this string text)
    {
        return text.RemoveDiacritics().RemoveAccents();
    }
    
    public static string RemoveAllCharacters(this string text)
    {
        return text.RemoveDiacritics().RemoveAccents().RemoveSpecialCharacters();
    }
    
    public static string RemoveSpecialCharacters(this string text, bool removeAccented)
    {
        var regex = string.Empty;

        if (removeAccented)
        {
            regex = "[^0-9a-zA-Z]+";
        }
        else
        {
            regex = "[!#$%&\\(\\)\\*\\+,-.\\/:;?@\\[\\]\\\\_\\{|\\}´=\"\\^<>\\~¨°ºª§¬¢£³²¹]";
        }

        return Regex.Replace(text, regex, string.Empty);
    }
    
    public static string PrepareForSearch(this string text)
    {
        text = Regex.Replace(text, "\\((.|\n)*?\\)", "");//remove informacoes entre parenteses
        text = Regex.Replace(text, "([a-zA-Z0-9])-([a-zA-Z0-9])", "$1WXY1YXW$2");//para manter o -
        text = Regex.Replace(text, @"([a-zA-Z0-9])\.([a-zA-Z0-9])", "$1WXY2YXW$2");//para manter o .
        text = text.RemoveSpecialCharacters(false);//remove pontuacao
        text = text.ReplaceSpecialCharacters().Replace("-", " ").Replace("wxy1yxw", "-").Replace("wxy2yxw", ".");// substitui caracteres especiais
        while (Regex.IsMatch(text, @"\s[a-zA-Z0-9]{1}\s"))
        {
            text = Regex.Replace(text, @"\s[a-zA-Z0-9]{1}\s", " ");//remove 1 char
        }
        text = Regex.Replace(text, @"\s[a-zA-Z0-9]{1}$", " ");//remove 1 char, no final
        text = Regex.Replace(text, @"^[a-zA-Z0-9]{1}\s", " ");//remove 1 char, no inicio

        if (text.Length > 150)
        {
            text = text.Substring(0, 150);
        }
        
        var value = RemoveStopWords(text);
        text = value.Contains(',') ? "NEAR((" + value + "), MAX, TRUE)" : text;

        return text;

    }
    
    public static string RemoveHtmlTags(this string text)
    {
        return Regex.Replace(text, "<.*?>", string.Empty);
    }
    
    public static string ToStringFullText(this string value)
    {
        value = Regex.Replace(value, "\\((.|\n)*?\\)", "");//remove informacoes entre parenteses
        value = Regex.Replace(value, "([a-zA-Z0-9])-([a-zA-Z0-9])", "$1WXY1YXW$2");//para manter o -
        value = Regex.Replace(value, @"([a-zA-Z0-9])\.([a-zA-Z0-9])", "$1WXY2YXW$2");//para manter o .
        value = value.RemoveSpecialCharacters(false);//remove pontuacao
        value = value.ReplaceSpecialCharacters().Replace("-", " ").Replace("wxy1yxw", "-").Replace("wxy2yxw", ".");// substitui caracteres especiais
        while (Regex.IsMatch(value, @"\s[a-zA-Z0-9]{1}\s"))
        {
            value = Regex.Replace(value, @"\s[a-zA-Z0-9]{1}\s", " ");//remove 1 char
        }
        value = Regex.Replace(value, @"\s[a-zA-Z0-9]{1}$", " ");//remove 1 char, no final
        value = Regex.Replace(value, @"^[a-zA-Z0-9]{1}\s", " ");//remove 1 char, no inicio

        if (value.Length > 150)
        {
            value = value.Substring(0, 150);
        }

        var text = RemoveStopWords(value);

        if (text.Contains(','))
        {
            value = "NEAR((" + text + "), MAX, TRUE)";//insere clausula para fulltext
        }
        else
        {
            value = text;
        }

        return value;
    }
    
    public static double FindSimilarity(this string source, string target)
    {
        if (source == null || target == null) {
            throw new ArgumentException("FindSimilarity: Strings must not be null");
        }
        
        if (string.IsNullOrEmpty(source))
        {
            return string.IsNullOrEmpty(target) ? 1 : // 100% match
                0; // 0% match
        }

        if (string.IsNullOrEmpty(target))
        {
            return 0; // 0% match
        }

        if (source.Equals(target, StringComparison.OrdinalIgnoreCase))
        {
            return 1; // 100% match
        }

        double maxLength = Math.Max(source.Length, target.Length);
        if (maxLength > 0) {
            return (maxLength - ComputeLevenshteinDistance(source, target)) / maxLength;
        }

        return 1.0;
        
        //int stepsToSame = ComputeLevenshteinDistance(source, target);
        //return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
    }
    
    public static string UrlPathCombine(this string path1, params string[] path2)
    {
        path1 = path1.TrimEnd('/') + "/";
        return path2.Aggregate(path1, (current, path2_1) => UrlCombine(current, path2_1)
                .Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
    }

    public static string AddSpacesToSentence(this string state)
    {
        var charArray = state.ToCharArray();
        var array = new char[charArray.Length + StringExtensions.HowManyCapitalizedChars(charArray) - 1];
        array[0] = charArray[0];
        var num1 = 1;
        for (var index1 = 1; index1 < charArray.Length; ++index1)
        {
            if (char.IsUpper(charArray[index1]) && (charArray[index1 - 1] != ' ' && !char.IsUpper(charArray[index1 - 1]) || char.IsUpper(charArray[index1 - 1]) && index1 < charArray.Length - 1 && !char.IsUpper(charArray[index1 + 1])))
            {
                var chArray1 = array;
                var index2 = num1;
                var num2 = index2 + 1;
                chArray1[index2] = ' ';
                var chArray2 = array;
                var index3 = num2;
                num1 = index3 + 1;
                var num3 = (int) charArray[index1];
                chArray2[index3] = (char) num3;
            }
            else
                array[num1++] = charArray[index1];
        }
        return new string(array.AsSpan());
    }
    
    public static bool HasTrailingSlash(this string? url) => url != null && url.EndsWith("/");

    public static string TruncateSensitiveInformation(this string part) => part.AsSpan().TruncateSensitiveInformation();

    public static string TruncateSensitiveInformation(this ReadOnlySpan<char> part)
    {
      char[] chArray = new char[2];
      Span<char> span1 = new Span<char>(chArray);
      if (!(part != (ReadOnlySpan<char>) string.Empty))
        return string.Empty;
      ReadOnlySpan<char> readOnlySpan1 = part;
      ReadOnlySpan<char> readOnlySpan2 = readOnlySpan1.Slice(0, 1);
      ref ReadOnlySpan<char> local1 = ref readOnlySpan2;
      Span<char> span2 = span1;
      Span<char> destination1 = span2.Slice(0, 1);
      local1.CopyTo(destination1);
      readOnlySpan1 = part;
      int length1 = readOnlySpan1.Length;
      int start1 = length1 - 1;
      ReadOnlySpan<char> readOnlySpan3 = readOnlySpan1.Slice(start1, length1 - start1);
      ref ReadOnlySpan<char> local2 = ref readOnlySpan3;
      span2 = span1;
      Span<char> destination2 = span2.Slice(1, span2.Length - 1);
      local2.CopyTo(destination2);
      return string.Create<char[]>(part.Length, chArray, (SpanAction<char, char[]>) ((span, s) =>
      {
        s.AsSpan<char>(0, 1).CopyTo(span);
        for (int index = 1; index < span.Length - 1; ++index)
          span[index] = _sensitive;
        Span<char> span3 = s.AsSpan<char>(s.Length - 1);
        ref Span<char> local3 = ref span3;
        Span<char> span4 = span;
        int length2 = span4.Length;
        int start2 = length2 - 1;
        Span<char> destination3 = span4.Slice(start2, length2 - start2);
        local3.CopyTo(destination3);
      }));
    }

    public static string TruncateEmail(this string email)
    {
      var readOnlySpan1 = email.AsSpan(0, email.IndexOf(_at)).TruncateSensitiveInformation().AsSpan();
      var readOnlySpan2 = email.AsSpan(email.IndexOf(_at) + 1).TruncateSensitiveInformation().AsSpan();
      var destination = new Span<char>(new char[email.Length]);
      readOnlySpan1.CopyTo(destination);
      destination[readOnlySpan1.Length] = _at;
      readOnlySpan2.CopyTo(destination[(readOnlySpan1.Length + 1)..]);
      return destination.ToString();
    }

    public static string ToSha256(this string value)
    {
        var stringBuilder = new StringBuilder();
        var bytes = Encoding.ASCII.GetBytes(value);
        foreach (var num in SHA256.HashData(bytes))
            stringBuilder.Append(num.ToString("x2"));
        return stringBuilder.ToString();
    }

    public static string ToSha512(this string value)
    {
        var stringBuilder = new StringBuilder();
        var bytes = Encoding.ASCII.GetBytes(value);
        foreach (var num in SHA512.HashData(bytes))
            stringBuilder.Append(num.ToString("x2"));
        return stringBuilder.ToString();
    }

    public static string ToBase64(this string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
    
    public static string ToBase64(this string str, Encoding? enc) => (enc ?? Encoding.Default).GetBytes(str).ToBase64();

    public static string ToBase64(this byte[] data, Encoding? enc = null) => Convert.ToBase64String(data);
    
    public static string FromBase64ToString(this string str, Encoding? enc = null) => (enc ?? Encoding.Default).GetString(str.FromBase64());

    public static byte[] FromPlainHexDumpStyleToByteArray(this string hex)
    {
        var byteArray = hex.Length % 2 != 1 ? new byte[hex.Length >> 1] : throw new Exception("The binary key cannot have an odd number of digits");
        for (var index = 0; index < hex.Length >> 1; ++index)
            byteArray[index] = (byte) ((StringExtensions.GetHexVal(hex[index << 1]) << 4) + StringExtensions.GetHexVal(hex[(index << 1) + 1]));
        return byteArray;
    }

    public static string ToPlainHexDumpStyle(this byte[] data) => BitConverter.ToString(data).Replace("-", "").ToLower();

    public static string Capitalize(this string value, bool isRestLower)
    {
        var source = value.AsSpan();
        var destination = new Span<char>(new char[value.Length]);
        source.CopyTo(destination);
        if (isRestLower)
            source.ToLowerInvariant(destination);
        destination[0] = char.ToUpper(source[0]);
        return destination.ToString();
    }
    
    public static byte[] FromBase64(this string str) => Convert.FromBase64String(str);
    
    public static string Urlize(this string str)
    {
        var input = str.Trim().ToLower().RemoveDiacritics();
        var str1 = UrlizeRegex.Replace(input, "-");
        if (str1.StartsWith('-'))
        {
            var str2 = str1;
            str1 = str2.Substring(1, str2.Length - 1);
        }

        if (!str1.EndsWith('-')) return str1;
        
        var str3 = str1;
        str1 = str3[..^1];
        return str1;
    }
    
    public static string OnlyNumbers(this string str)
    {
        var array = new char[str.Length];
        var newSize = 0;
        foreach (var ch in str)
        {
            switch (ch)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    array[newSize++] = ch;
                    break;
            }
        }
        Array.Resize(ref array, newSize);
        return new string(array);
    }
    
    
    
    private static int ComputeLevenshteinDistance(string source, string target)
    {
        var m = source.Length;
        var n = target.Length;
 
        var T = new int[m + 1][];
        for (var i = 0; i < m + 1; ++i) {
            T[i] = new int[n + 1];
        }
 
        for (var i = 1; i <= m; i++) {
            T[i][0] = i;
        }
        for (var j = 1; j <= n; j++) {
            T[0][j] = j;
        }
 
        int cost;
        for (var i = 1; i <= m; i++) {
            for (var j = 1; j <= n; j++) {
                cost = source[i - 1] == target[j - 1] ? 0: 1;
                T[i][j] = Math.Min(Math.Min(T[i - 1][j] + 1, T[i][j - 1] + 1),
                    T[i - 1][j - 1] + cost);
            }
        }
 
        return T[m][n];
    }
    
    private static string RemoveStopWords(string text)
    {
        if (string.IsNullOrEmpty(text))
                return string.Empty;

        text = text.Trim();
        text = Regex.Replace(text, @"\s+", " ");

        string[] words = text.Split(' ');
        var query = string.Empty;

        var listStopWords = new string[] {
            "a","about","above","after","again","against","ain","all","am","an","and","any","are","aren","aren't","as","at","be","because","been","before","being","below","between","both","but","by","can","couldn","couldn't","d","did","didn","didn't","do","does","doesn","doesn't","doing","don","don't","down","during","each","few","for","from","further","had","hadn","hadn't","has","hasn","hasn't","have","haven","haven't","having","he","her","here","hers","herself","him","himself","his","how","i","if","in","into","is","isn","isn't","it","it's","its","itself","just","ll","m","ma","me","mightn","mightn't","more","most","mustn","mustn't","my","myself","needn","needn't","no","nor","not","now","o","of","off","on","once","only","or","other","our","ours","ourselves","out","over","own","re","s","same","shan","shan't","she","she's","should","should've","shouldn","shouldn't","so","some","such","t","than","that","that'll","the","their","theirs","them","themselves","then","there","these","they","this","those","through","to","too","under","until","up","ve","very","was","wasn","wasn't","we","were","weren","weren't","what","when","where","which","while","who","whom","why","will","with","won","won't","wouldn","wouldn't","y","you","you'd","you'll","you're","you've","your","yours","yourself","yourselves","could","he'd","he'll","he's","here's","how's","i'd","i'll","i'm","i've","let's","ought","she'd","she'll","that's","there's","they'd","they'll","they're","they've","we'd","we'll","we're","we've","what's","when's","where's","who's","why's","would","able","abst","accordance","according","accordingly","across","act","actually","added","adj","affected","affecting","affects","afterwards","ah","almost","alone","along","already","also","although","always","among","amongst","announce","another","anybody","anyhow","anymore","anyone","anything","anyway","anyways","anywhere","apparently","approximately","arent","arise","around","aside","ask","asking","auth","available","away","awfully","b","back","became","become","becomes","becoming","beforehand","begin","beginning","beginnings","begins","behind","believe","beside","besides","beyond","biol","brief","briefly","c","ca","came","cannot","can't","cause","causes","certain","certainly","co","com","come","comes","contain","containing","contains","couldnt","date","different","done","downwards","due","e","ed","edu","effect","eg","eight","eighty","either","else","elsewhere","end","ending","enough","especially","et","etc","even","ever","every","everybody","everyone","everything","everywhere","ex","except","f","far","ff","fifth","first","five","fix","followed","following","follows","former","formerly","forth","found","four","furthermore","g","gave","get","gets","getting","give","given","gives","giving","go","goes","gone","got","gotten","h","happens","hardly","hed","hence","hereafter","hereby","herein","heres","hereupon","hes","hi","hid","hither","home","howbeit","however","hundred","id","ie","im","immediate","immediately","importance","important","inc","indeed","index","information","instead","invention","inward","itd","it'll","j","k","keep","keeps","kept","kg","km","know","known","knows","l","largely","last","lately","later","latter","latterly","least","less","lest","let","lets","like","liked","likely","line","little","'ll","look","looking","looks","ltd","made","mainly","make","makes","many","may","maybe","mean","means","meantime","meanwhile","merely","mg","might","million","miss","ml","moreover","mostly","mr","mrs","much","mug","must","n","na","name","namely","nay","nd","near","nearly","necessarily","necessary","need","needs","neither","never","nevertheless","new","next","nine","ninety","nobody","non","none","nonetheless","noone","normally","nos","noted","nothing","nowhere","obtain","obtained","obviously","often","oh","ok","okay","old","omitted","one","ones","onto","ord","others","otherwise","outside","overall","owing","p","page","pages","part","particular","particularly","past","per","perhaps","placed","please","plus","poorly","possible","possibly","potentially","pp","predominantly","present","previously","primarily","probably","promptly","proud","provides","put","q","que","quickly","quite","qv","r","ran","rather","rd","readily","really","recent","recently","ref","refs","regarding","regardless","regards","related","relatively","research","respectively","resulted","resulting","results","right","run","said","saw","say","saying","says","sec","section","see","seeing","seem","seemed","seeming","seems","seen","self","selves","sent","seven","several","shall","shed","shes","show","showed","shown","showns","shows","significant","significantly","similar","similarly","since","six","slightly","somebody","somehow","someone","somethan","something","sometime","sometimes","somewhat","somewhere","soon","sorry","specifically","specified","specify","specifying","still","stop","strongly","sub","substantially","successfully","sufficiently","suggest","sup","sure","take","taken","taking","tell","tends","th","thank","thanks","thanx","thats","that've","thence","thereafter","thereby","thered","therefore","therein","there'll","thereof","therere","theres","thereto","thereupon","there've","theyd","theyre","think","thou","though","thoughh","thousand","throug","throughout","thru","thus","til","tip","together","took","toward","towards","tried","tries","truly","try","trying","ts","twice","two","u","un","unfortunately","unless","unlike","unlikely","unto","upon","ups","us","use","used","useful","usefully","usefulness","uses","using","usually","v","value","various","'ve","via","viz","vol","vols","vs","w","want","wants","wasnt","way","wed","welcome","went","werent","whatever","what'll","whats","whence","whenever","whereafter","whereas","whereby","wherein","wheres","whereupon","wherever","whether","whim","whither","whod","whoever","whole","who'll","whomever","whos","whose","widely","willing","wish","within","without","wont","words","world","wouldnt","www","x","yes","yet","youd","youre","z","zero","a's","ain't","allow","allows","apart","appear","appreciate","appropriate","associated","best","better","c'mon","c's","cant","changes","clearly","concerning","consequently","consider","considering","corresponding","course","currently","definitely","described","despite","entirely","exactly","example","going","greetings","hello","help","hopefully","ignored","inasmuch","indicate","indicated","indicates","inner","insofar","it'd","keep","keeps","novel","presumably","reasonably","second","secondly","sensible","serious","seriously","sure","t's","third","thorough","thoroughly","three","well","wonder",
            "de","a","o","que","e","do","da","em","um","para","é","com","não","uma","os","no","se","na","por","mais","as","dos","como","mas","foi","ao","ele","das","tem","à","seu","sua","ou","ser","quando","muito","há","nos","já","está","eu","também","só","pelo","pela","até","isso","ela","entre","era","depois","sem","mesmo","aos","ter","seus","quem","nas","me","esse","eles","estão","você","tinha","foram","essa","num","nem","suas","meu","às","minha","têm","numa","pelos","elas","havia","seja","qual","será","nós","tenho","lhe","deles","essas","esses","pelas","este","fosse","dele","tu","te","vocês","vos","lhes","meus","minhas","teu","tua","teus","tuas","nosso","nossa","nossos","nossas","dela","delas","esta","estes","estas","aquele","aquela","aqueles","aquelas","isto","aquilo","estou","está","estamos","estão","estive","esteve","estivemos","estiveram","estava","estávamos","estavam","estivera","estivéramos","esteja","estejamos","estejam","esivesse","estivéssemos","estivessem","estiver","estivermos","estiverem","hei","há","havemos","hão","houve","houvemos","houveram","houvera","houvéramos","haja","hajamos","hajam","houvesse","houvéssemos","houvessem","houver","houvermos","houverem","houverei","houverá","houveremos","houverão","houveria","houveríamos","houveriam","sou","somos","são","era","éramos","eram","fui","foi","fomos","foram","fora","fôramos","seja","sejamos","sejam","fosse","fôssemos","fossem","for","formos","forem","serei","será","seremos","serão","seria","seríamos","seriam","tenho","tem","temos","tém","tinha","tínhamos","tinham","tive","teve","tivemos","tiveram","tivera","tivéramos","tenha","tenhamos","tenham","tivesse","tivéssemos","tivessem","tiver","tivermos","tiverem","terei","terá","teremos","terão","teria","teríamos","teriam"
        };
        var q = string.Empty;
        query = words.Where(word => !listStopWords.Any(a => a.ToLower() == word.ToLower()))
            .Aggregate(query, (current, word) => current + word.Trim() + ",");

        return string.IsNullOrEmpty(query) ? string.Empty : query.Trim(',');
    }

    private static string UrlCombine(string path1, string path2)
    {
        path1 = path1.TrimEnd('/') + "/";
        path2 = path2.TrimStart('/');
        return Path.Combine(path1, path2).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }
    
    private static int HowManyCapitalizedChars(char[] state)
    {
        var num = 0;
        for (var index = 0; index < state.Length; ++index)
        {
            if (char.IsUpper(state[index]))
                ++num;
        }
        return num;
    }
    
    private static int GetHexVal(char hex)
    {
        var num = (int) hex;
        return num - (num < 58 ? 48 : (num < 97 ? 55 : 87));
    }
}