using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lima.Dictionaries.Api.Utils
{
    public class TextUtils
    {
        private static readonly Dictionary<char, string> ConvertedLetters = new Dictionary<char, string>
    {
        {'а', "a"},
        {'б', "b"},
        {'в', "v"},
        {'г', "g"},
        {'д', "d"},
        {'е', "e"},
        {'ё', "yo"},
        {'ж', "zh"},
        {'з', "z"},
        {'и', "i"},
        {'й', "j"},
        {'к', "k"},
        {'л', "l"},
        {'м', "m"},
        {'н', "n"},
        {'о', "o"},
        {'п', "p"},
        {'р', "r"},
        {'с', "s"},
        {'т', "t"},
        {'у', "u"},
        {'ф', "f"},
        {'х', "h"},
        {'ц', "c"},
        {'ч', "ch"},
        {'ш', "sh"},
        {'щ', "sch"},
        {'ъ', "j"},
        {'ы', "i"},
        {'ь', "j"},
        {'э', "e"},
        {'ю', "yu"},
        {'я', "ya"},
        {'А', "A"},
        {'Б', "B"},
        {'В', "V"},
        {'Г', "G"},
        {'Д', "D"},
        {'Е', "E"},
        {'Ё', "Yo"},
        {'Ж', "Zh"},
        {'З', "Z"},
        {'И', "I"},
        {'Й', "J"},
        {'К', "K"},
        {'Л', "L"},
        {'М', "M"},
        {'Н', "N"},
        {'О', "O"},
        {'П', "P"},
        {'Р', "R"},
        {'С', "S"},
        {'Т', "T"},
        {'У', "U"},
        {'Ф', "F"},
        {'Х', "H"},
        {'Ц', "C"},
        {'Ч', "Ch"},
        {'Ш', "Sh"},
        {'Щ', "Sch"},
        {'Ъ', "J"},
        {'Ы', "I"},
        {'Ь', "J"},
        {'Э', "E"},
        {'Ю', "Yu"},
        {'Я', "Ya"}
    };
        private static readonly Dictionary<char, string> _lattinToCyrillicMap = new Dictionary<char, string>()
        {
            { 'a', "а" },
            { 'b', "б" },
            { 'c', "к" },
            { 'd', "д" },
            { 'e', "е" },
            { 'f', "ф" },
            { 'g', "г" },
            { 'h', "х" },
            { 'i', "и" },
            { 'j', "дж" },
            { 'k', "к" },
            { 'l', "л" },
            { 'm', "м" },
            { 'n', "н" },
            { 'o', "о" },
            { 'p', "п" },
            { 'q', "к" },
            { 'r', "р" },
            { 's', "с" },
            { 't', "т" },
            { 'u', "ю" },
            { 'w', "в" },
            { 'x', "кс" },
            { 'y', "я" },
            { 'z', "з" },
        };

        public static string SmartConvert(string source)
        {
            if (source.All(c => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')))
                return ConvertToCyrillic(source);

            return ConvertToLatin(source);
        }

        public static string ConvertToCyrillic(string source)
        {
            var result = new StringBuilder();
            foreach (var letter in source.ToLower())
            {
                if (_lattinToCyrillicMap.ContainsKey(letter))
                    result.Append(_lattinToCyrillicMap[letter]);
            }
            return result.ToString();
        }

        public static string ConvertToLatin(string source)
        {
            var result = new StringBuilder();
            foreach (var letter in source.ToLower())
            {
                if (ConvertedLetters.ContainsKey(letter))
                    result.Append(ConvertedLetters[letter]);
            }
            return result.ToString();
        }
    }
}
