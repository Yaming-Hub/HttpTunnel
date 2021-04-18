using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using HttpTunnel.Configurations;

namespace HttpTunnel.Implementations
{
    public static class UriExtensions
    {
        public static Uri Replace(this Uri uri, IEnumerable<UrlReplaceRule> rules)
        {
            if (rules != null)
            {
                var absoluteUri = uri.AbsoluteUri;

                foreach (var rule in rules)
                {
                    absoluteUri = Replace(absoluteUri, rule);
                }

                return new Uri(absoluteUri);
            }
            else
            {
                return uri;
            }
        }

        public static string Replace(string uri, UrlReplaceRule rule)
        {
            if (rule != null && rule.Pattern != null && rule.Replacement != null)
            {
                Regex regex = null;

                try
                {
                    // TODO: Cache regex in concurrent dictionary.
                    regex = new Regex(rule.Pattern, RegexOptions.IgnoreCase);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine($"Invalid rule pattern: {rule.Pattern}");
                }

                if (regex != null && regex.IsMatch(uri))
                {
                    uri = regex.Replace(uri, rule.Replacement);
                }
            }

            return uri;
        }
    }
}
