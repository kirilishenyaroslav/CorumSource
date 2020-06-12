// Decompiled with JetBrains decompiler
// Type: GridMvc.Sanitizer
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System.Text.RegularExpressions;

namespace GridMvc
{
  public class Sanitizer : ISanitizer
  {
    private static readonly Regex Tags = new Regex("<[^>]*(>|$)", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex Whitelist = new Regex("\r\n                ^</?(b(lockquote)?|code|d(d|t|l|el)|em|h(1|2|3)|i|kbd|li|ol|p(re)?|s(ub|up|trong|trike)?|ul)>$|\r\n                ^<(b|h)r\\s?/?>$", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
    private static readonly Regex WhitelistA = new Regex("\r\n                ^<a\\s\r\n                href=\"(\\#\\d+|(https?|ftp)://[-a-z0-9+&@#/%?=~_|!:,.;\\(\\)]+)\"\r\n                (\\stitle=\"[^\"<>]+\")?\\s?>$|\r\n                ^</a>$", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
    private static readonly Regex WhitelistImg = new Regex("\r\n                ^<img\\s\r\n                src=\"https?://[-a-z0-9+&@#/%?=~_|!:,.;\\(\\)]+\"\r\n                (\\swidth=\"\\d{1,3}\")?\r\n                (\\sheight=\"\\d{1,3}\")?\r\n                (\\salt=\"[^\"<>]*\")?\r\n                (\\stitle=\"[^\"<>]*\")?\r\n                \\s?/?>$", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

    public string Sanitize(string html)
    {
      if (string.IsNullOrEmpty(html))
        return html;
      MatchCollection matchCollection = Sanitizer.Tags.Matches(html);
      for (int index = matchCollection.Count - 1; index > -1; --index)
      {
        Match match = matchCollection[index];
        string input = match.Value.ToLowerInvariant();
        if (!Sanitizer.Whitelist.IsMatch(input) && !Sanitizer.WhitelistA.IsMatch(input) && !Sanitizer.WhitelistImg.IsMatch(input))
          html = html.Remove(match.Index, match.Length);
      }
      return html;
    }
  }
}
