using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace CinemaScraper
{
    public static class HtmlNodeExtensions
    {
        public static string GetCleanedText(this HtmlNode obj)
        {
            var text = obj.InnerText;

            var cleaned = Regex.Replace(text, @"\t|\n|\r", "").TrimStart().TrimEnd();
            var shortened = Regex.Replace(cleaned, @"\s+", " ");

            return shortened;
        }
    }
}
