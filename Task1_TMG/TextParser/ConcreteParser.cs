using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace Task1_TMG
{
    public class ConcreteParser : IParser
    {
        private static readonly char[] Separators = {',', ';'};

        /// <summary>
        /// Метод находит Id в строке. Id - это число от 1 до 20.
        /// </summary>
        /// <param name="parsingLine">Исходная строка</param>
        /// <returns>Возвращает перечисление уникальных Id и перечисление строк, не являющиеся Id в порядке их следования в исходной строке</returns>
        public (IEnumerable<int>, IEnumerable<string>) ParseIdsFromText(string parsingLine)
        {
            // Массив для Id
            var ids = new List<int>();
            // Массив для любых строк, не являющиеся Id
            var badLines = new List<string>();
            
            var splitParsingLine = parsingLine.Split(Separators);
            
            foreach (var line in splitParsingLine)
            {
                // Убираем незначашие нули и пробелы
                var processedLine = line.Trim(' ').TrimStart('0');

                if (int.TryParse(processedLine, out var id))
                    ids.Add(id);
                else
                {
                    badLines.Add(line);
                }
            }
            
            // Возвращаем множество уникальных Id и массив строк, не являющиеся Id
            return (new HashSet<int>(ids), badLines);
        }
    }
}