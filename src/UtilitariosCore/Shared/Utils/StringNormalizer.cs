using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace UtilitariosCore.Shared.Utils;

public static class StringNormalizer
{
    /// <summary>
    /// Quita tildes y diacríticos, elimina todo lo que no sea letra o espacio,
    /// y aplica Title Case: primera letra de cada palabra en mayúscula, resto minúscula.
    /// Ejemplo: "ángelA  márTÍNez" → "Angela Martinez"
    /// </summary>
    public static string ToTitleCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        var withoutAccents = RemoveAccents(input);
        var onlyLetters = Regex.Replace(withoutAccents, @"[^a-zA-Z\s]", string.Empty);
        var trimmed = Regex.Replace(onlyLetters.Trim(), @"\s+", " ");

        if (string.IsNullOrWhiteSpace(trimmed)) return trimmed;

        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(trimmed.ToLower());
    }

    /// <summary>
    /// Quita tildes y diacríticos, elimina todo lo que no sea letra o espacio,
    /// convierte a minúscula. Sin números ni símbolos.
    /// Ejemplo: "Gordita 2024!" → "gordita"
    /// </summary>
    public static string ToNormalizedTag(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        var withoutAccents = RemoveAccents(input);
        var onlyLetters = Regex.Replace(withoutAccents, @"[^a-zA-Z\s]", string.Empty);
        var trimmed = Regex.Replace(onlyLetters.Trim(), @"\s+", " ");

        return trimmed.ToLower();
    }

    /// <summary>
    /// Normaliza una lista de tags: elimina vacíos, normaliza cada uno y quita duplicados.
    /// </summary>
    public static List<string> NormalizeTags(IEnumerable<string> tags)
    {
        return tags
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(ToNormalizedTag)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Distinct()
            .ToList();
    }

    private static string RemoveAccents(string input)
    {
        var normalized = input.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
