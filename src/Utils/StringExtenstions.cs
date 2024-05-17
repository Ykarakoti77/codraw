namespace Codraw.Utils;

public static class StringExtensions
{
    public static List<string> StripWhitespace(this string commaSeparatedString,string delimitter)
    {
        var ids = commaSeparatedString.Split(delimitter);

        var strippedIds = ids.Select(id => id.Trim());

        return strippedIds.ToList();
    }
}