using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Task1_TMG.Extensions
{
    public static class TextRangeExtension
    {
        /// <summary>
        /// В исходном тексте (TextRange) для каждой входной строки (line) из перечисления строк (lines)
        /// ищем эквивалентную подстроку подстроку (TextRange)
        /// </summary>
        /// <param name="textRange">Исходный TextRange</param>
        /// <param name="lines">Набор строк, которые будем искать в TextRange</param>
        /// <returns>Возвращает перечисление TextRange</returns>
        public static IEnumerable<TextRange> GetTextRanges(this TextRange textRange, IEnumerable<string> lines)
        {
            // Исходные текст
            var text = textRange.Text;
            
            // На момент начала обработки указатель конца доллжен указывать на начало текста
            var end = textRange.Start;

            // Ищем каждую строку в тексте. Строки в перечислении идут по порядку появления в исходной строке!
            foreach (var line in lines)
            {
                // Указатель на начало вхождения подстроки в строку
                var startIndex = text.IndexOf(line, StringComparison.InvariantCultureIgnoreCase);
                // Длина строки
                var length = line.Length;
                
                // Указатель на начало подстроки внутри текста
                var start = end!.GetPositionAtOffset(startIndex, LogicalDirection.Forward);
                // Указатель на конец подстроки внутри текста
                end = start!.GetPositionAtOffset(length, LogicalDirection.Forward);

                // Убираем ненужный текст
                text = text.Substring(startIndex + length);
                
                yield return new TextRange(start, end);
            }
        }
    }
}