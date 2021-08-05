using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Xml.Linq;

/**
 * Adapted by Moon on 1/27/2021
 * https://stackoverflow.com/questions/1145659/ignore-namespaces-in-linq-to-xml
 * Extension method to help with parsing XElements, particularly to aid in ignoring namespaces
 */

namespace KikClient.Utilities
{
    public static class Extensions
    {
        public static XElement ElementAnyNS(this XContainer source, string localName)
        {
            return source.ElementsAnyNS(localName).FirstOrDefault();
        }

        public static IEnumerable<XElement> ElementsAnyNS(this XContainer source, string localName)
        {
            return source.Elements().Where(e => e.Name.LocalName == localName);
        }

        /// <summary>
        /// Converts from a kik-encoded base64 string to one which can be parsed by a normal base64 parser.
        /// Kik uses a custom base64 alphabet for encoding protobuf, as shown below
        /// </summary>
        /// <param name="base64">Input string to be converted to a standard base64 string</param>
        /// <returns>A standard base64 string</returns>
        public static string ConvertKikBase64ToStandardBase64(this string base64)
        {
            while (base64.Length % 4 != 0) base64 += "=";
            return base64.Replace("-", "+").Replace("_", "/");
        }

        /// <summary>
        /// Converts from a standard base64 string to one which can be parsed by kik.
        /// Kik uses a custom base64 alphabet for encoding protobuf, as shown below
        /// </summary>
        /// <param name="base64">Input string to be converted to a kik-encoded base64 string</param>
        /// <returns>A kik-encoded base64 string</returns>
        public static string ConvertStandardBase64ToKikBase64(this string base64)
        {
            return base64.Replace("=", "").Replace("+", "-").Replace("/", "_");
        }

        /// <summary>
        /// Escapes XML code using SecurityElement.Escape
        /// https://stackoverflow.com/questions/2032021/is-there-an-implementation-of-org-apache-commons-lang-stringescapeutils-for-net
        /// </summary>
        /// <param name="unescaped">The unescaped string to escape</param>
        /// <returns>The escaped string</returns>
        public static string EscapeXml(this string unescaped)
        {
            return SecurityElement.Escape(unescaped);
        }

        /// <summary>
        /// Unescapes XML, using string.Replace, since SecurityElement doesn't
        /// provide an Unescape function as well
        /// https://stackoverflow.com/questions/2032021/is-there-an-implementation-of-org-apache-commons-lang-stringescapeutils-for-net
        /// </summary>
        /// <param name="escaped">The escaped string to unescape</param>
        /// <returns>The unescaped string</returns>
        public static string UnescapeXml(this string escaped)
        {
            return escaped.Replace("&lt;", "<")
                          .Replace("&gt;", ">")
                          .Replace("&quot;", "\"")
                          .Replace("&apos;", "'")
                          .Replace("&amp;", "&");
        }

        public static string GetUsernameFromJid(this string jid)
        {
            if (jid == "kikteam@talk.kik.com")
            {
                return "kikteam";
            }
            if (!jid.Contains("@"))
            {
                return jid;
            }
            if (jid.Substring(jid.LastIndexOf("@")).Contains("group"))
            {
                return "GROUP";
            }
            return jid.Substring(0, jid.LastIndexOf("_"));
        }

        public static bool IsAlias(this string jid)
        {
            var usernameLength = GetUsernameFromJid(jid).Length;
            var suffix = jid.Substring(usernameLength, jid.Length - usernameLength);
            var server = suffix.Substring(0, suffix.IndexOf("@"));

            return usernameLength == 52 && server == "_a";
        }
    }
}
