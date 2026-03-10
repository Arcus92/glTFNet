using System.Text;

namespace glTFNet.Generator.Utils;

public static class StringHelper
{
    private static readonly char[] Separators = [' ', '.', '_', '-', '/'];
    
    /// <summary>
    /// Converts the given text into PascalCase.
    /// </summary>
    /// <param name="text">The text in camelCase.</param>
    /// <returns>returns the text in PascalCase.</returns>
    public static string ToPascalCase(this string text)
    {
        var builder = new StringBuilder(text);
        builder[0] = char.ToUpperInvariant(builder[0]);

        for (var i = 0; i < builder.Length; i++)
        {
            var c = builder[i];
            if (Separators.Contains(c))
            {
                builder.Remove(i, 1);
                if (i < builder.Length)
                {
                    builder[i] = char.ToUpperInvariant(builder[i]);
                }
            }
        }
        
        return builder.ToString();
    }
}