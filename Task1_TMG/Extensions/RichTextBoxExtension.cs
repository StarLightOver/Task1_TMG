using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Task1_TMG.Extensions
{
    public static class RichTextBoxExtension
    {
        /// <summary>
        /// В элементе (RichTextBox) подсвечиваем подстроки (lines) выбранным цветом (color)
        /// </summary>
        /// <param name="richTextBox">Исходный RichTextBox</param>
        /// <param name="lines">Набор подстрок для выделения</param>
        /// <param name="color">Цвет выделения</param>
        public static void ColorizeSubstrings(this RichTextBox richTextBox, IEnumerable<string> lines, SolidColorBrush color)
        {
            var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);

            if (textRange.IsEmpty)
                return;

            foreach (var textRangeForConcreteLine in textRange.GetTextRanges(lines))
            {
                textRangeForConcreteLine.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            }
        }
    }
}