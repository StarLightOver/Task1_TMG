using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Task1_TMG.Extensions;

namespace Task1_TMG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const int CountColumn = 3;

        private readonly IIntegrationService _service;
        private readonly IParser _parser;

        private readonly VowelCounter _counter = new();

        public MainWindow(IIntegrationService service, IParser parser)
        {
            _service = service;
            _parser = parser;

            InitializeComponent();

            InitializeEvent();

            InputText.Document.Blocks.Clear();
        }

        /// <summary>
        /// Инициализируем различные события для компонентов
        /// </summary>
        private void InitializeEvent()
        {
            CalculateButton.Click += CalculateButton_Click;
            InputText.GotFocus += InputText_GotFocus;
        }

        /// <summary>
        /// Обработчик получения фокус для поля ввода InputText
        /// </summary>
        private void InputText_GotFocus(object sender, RoutedEventArgs e)
        {
            var textRange = new TextRange(InputText.Document.ContentStart, InputText.Document.ContentEnd);
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
        }

        /// <summary>
        /// Обработчик нажатия на кнопку CalculateButton
        /// </summary>
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            TableRow_Clear(DataRowGroup);

            var textFromTextBox = new TextRange(InputText.Document.ContentStart, InputText.Document.ContentEnd).Text;

            var (recognizedIds, noRecognizedLines) = _parser.ParseIdsFromText(textFromTextBox);

            foreach (var dataModel in recognizedIds.Select(id => _service.GetStringById(id)))
            {
                if (dataModel.ErrorMassages == null)
                {
                    var (countVowelLetters, countWords) = _counter.CalculateCount(dataModel.Data);

                    TableRow_PrintData(
                        DataRowGroup,
                        new MainViewModel()
                        {
                            Text = dataModel.Data,
                            CountWords = countWords,
                            CountVowelLetters = countVowelLetters,
                        });
                }
                else
                {
                    TableRow_PrintError(
                        DataRowGroup,
                        new MainViewModel()
                        {
                            Text = dataModel.ErrorMassages
                                .Aggregate("", (acc, errorMassage) => acc + errorMassage + "\n"),
                            CountWords = 0,
                            CountVowelLetters = 0,
                        });
                }
            }

            InputText.ColorizeSubstrings(noRecognizedLines, Brushes.Red);
        }

        /// <summary>
        /// Добавить строку с данными в группу строк, принадлежащей таблице
        /// </summary>
        private static void TableRow_PrintData(TableRowGroup dataRowGroup, MainViewModel viewModel)
        {
            var newTableRow = new TableRow();
            
            // Добавить ячейку с текстом
            var newTableCell = new TableCell(new Paragraph(new Run(viewModel.Text))
            {
                Padding = new Thickness(0, 10, 0, 10),
            });
            newTableRow.Cells.Add(newTableCell);

            // Добавить ячейку с количеством слом в тексте
            newTableCell = new TableCell(new Paragraph(new Run(viewModel.CountWords.ToString()))
            {
                Padding = new Thickness(0, 10, 0, 10),
            });
            newTableCell.TextAlignment = TextAlignment.Center;
            newTableRow.Cells.Add(newTableCell);

            // Добавить ячейку с количеством галсных букв в тексте
            newTableCell = new TableCell(new Paragraph(new Run(viewModel.CountVowelLetters.ToString()))
            {
                Padding = new Thickness(0, 10, 0, 10),
            });
            newTableCell.TextAlignment = TextAlignment.Center;
            newTableRow.Cells.Add(newTableCell);

            dataRowGroup.Rows.Add(newTableRow);
        }

        /// <summary>
        /// Добавить строку с ошибкой в группу строк, принадлежащей таблице
        /// </summary>
        private static void TableRow_PrintError(TableRowGroup dataRowGroup, MainViewModel viewModel)
        {
            var newTableRow = new TableRow();

            var runItem = new Run(viewModel.Text)
            {
                Foreground = Brushes.Red
            };

            var newTableCell = new TableCell(new Paragraph(runItem)
            {
                Padding = new Thickness(0, 10, 0, 10),
            });
            
            newTableCell.ColumnSpan = CountColumn;
            newTableRow.Cells.Add(newTableCell);

            dataRowGroup.Rows.Add(newTableRow);
        }

        /// <summary>
        /// Очистить группу строк, принадлежащей таблице
        /// </summary>
        private static void TableRow_Clear(TableRowGroup dataRowGroup)
        {
            dataRowGroup.Rows.Clear();
        }
    }
}