namespace Smith.WPF.HtmlEditor
{
    using System.Globalization;
    using System.Text.RegularExpressions;

    internal class ChineseWordCounter : WordCounter
    {
        #region Methods

        public override int Count(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            var sec = Regex.Split(text, @"\s");
            int count = 0;
            foreach (var si in sec)
            {
                int ci = Regex.Matches(si, @"[\u0000-\u00ff]+").Count;
                foreach (var c in si)
                    if ((int)c > 0x00FF) ci++;
                count += ci;
            }
            return count;
        }

        #endregion Methods
    }

    internal class EnglishWordCounter : WordCounter
    {
        #region Fields

        static readonly string pattern = @"[\S]+";

        #endregion Fields

        #region Methods

        public override int Count(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            MatchCollection collection = Regex.Matches(text, pattern);
            return collection.Count;
        }

        #endregion Methods
    }

    internal abstract class WordCounter
    {
        #region Methods

        public static WordCounter Create(CultureInfo culture)
        {
            string tag = culture.IetfLanguageTag.ToLower();

            switch (tag)
            {
                case "zh-cn": return new ChineseWordCounter();
                default: return new EnglishWordCounter();
            }
        }

        public static WordCounter Create()
        {
            return Create(CultureInfo.CurrentCulture);
        }

        public abstract int Count(string text);

        #endregion Methods
    }
}