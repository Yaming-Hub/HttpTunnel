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
                // TODO: Cache regex in concurrent dictionary.
                var regex = new Regex(rule.Pattern, RegexOptions.IgnoreCase);
                
                if (regex != null && regex.IsMatch(uri))
                {
                    uri = regex.Replace(uri, rule.Replacement);
                }
            }

            return uri;
        }
    }
}
