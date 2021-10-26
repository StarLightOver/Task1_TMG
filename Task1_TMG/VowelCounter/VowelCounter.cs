using System.Globalization;
using System.Linq;
using System.Text;

namespace Task1_TMG
{
    public class VowelCounter
    {
        // Лигатуры вынесены отдельно, а не как комбинация, например, 'æ' = "ae", по причине того, что
        // string.Normalize(...) на .Net 5.0 под Windows работает не корректно.
        // Глубоко не копал поэтому лигатуры не определил как считать лигатуры по типу "fi",
        // как согласная + гласная, либо как согласная. Принял, что их считаю, как согласные буквы.
        private static readonly char[] EngVowel = {'a', 'e', 'i', 'o', 'u', 'y'};
        private static readonly char[] RusVowel = {'а', 'о', 'у', 'ы', 'э', 'я', 'е', 'ё', 'ю', 'и'};
        private static readonly char[] Ligatures = {'æ'};
        
        /// <summary>
        /// Расчитыва количество гласный букв и количество слов в строке.
        /// </summary>
        /// <param name="inputText">Исходная строка</param>
        /// <returns>Возвращает число слов в строке и число гласных букв/returns>
        public (int CountWords, int CountVowelLetters) CalculateCount(string inputText)
        {
            var countWords = 0;
            var countVowelLetters = 0;
            
            // Получаем массив допустимых гласных букв
            var allVowel = EngVowel.Concat(RusVowel).Concat(Ligatures).ToArray();
            
            // Нормализуем все слова в FormD. Отдельно смотреть NormalizationForm.
            var normalizeText = inputText
                .Normalize(NormalizationForm.FormD)
                .ToCharArray()
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();
            
            // Указатель на то, принадлежит ли текущая буквыа слову или нет
            var pointerWord = false;
            
            foreach (var letter in normalizeText)
            {
                // Мы стоим на букве и слово не началось - устанавливаем флаг, что сейчас мы находимся на слове
                // и увеличиваем счетчик слов
                if (char.IsLetter(letter) && !pointerWord)
                {
                    pointerWord = true;
                    countWords++;
                }

                // Мы не на букве и слово не закончилось - снимаем флаг, что сейчас мы находимся на слове
                if (IsEndWord(letter) && pointerWord)
                {
                    pointerWord = false;
                }
                
                // Если гласная буква, то увеличиваем счетчик гл. букв
                if (allVowel.Contains(char.ToLowerInvariant(letter)))
                {
                    countVowelLetters++;
                }
            }

            return (countVowelLetters, countWords);
        }

        private static bool IsEndWord(char value)
        {
            // Не учитываем '-' и '\`', т.к. они могут быть частью слова
            switch (value)
            {
                case '!':
                case '"':
                case '#':
                case '%':
                case '&':
                case '(':
                case ')':
                case '*':
                case ',':
                case ' ':
                case '.':
                case '/':
                case ':':
                case ';':
                case '?':
                case '@':
                case '[':
                case '\\':
                case ']':
                case '{':
                case '}':
                    return true;
                default:
                    return false;
            }
        }
    }
}