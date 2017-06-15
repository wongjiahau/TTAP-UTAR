using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.User_Control {
    public partial class TimetableDescriptionViewer : UserControl {
        public TimetableDescriptionViewer() {
            InitializeComponent();
        }

        public void Clear() {
            SubjectDescGrid.Children.Clear();
        }

        public void Update(List<Slot> slots) {
            SubjectDescGrid.Children.Clear();
            var previousSubjectName = "";
            IColorGenerator c = new ColorGenerator();
            var currentTextBlock = new Label();
            var gridRowIndex = 0;
            foreach (var s in slots) {
                if (s.SubjectName != previousSubjectName) {
                    SubjectDescGrid.RowDefinitions.Add(new RowDefinition());

                    var box = GenerateSubjectName(s, c);
                    AddGridChildren(box, 0, gridRowIndex);

                    currentTextBlock = GenerateSubjectCode(c);
                    currentTextBlock.Content = $"{s.Code} \t=> {s.Type}{s.Number}";
                    AddGridChildren(currentTextBlock, 1, gridRowIndex);

                    var copyBtn = GenerateCopyButton(s.Code);
                    AddGridChildren(copyBtn, 2, gridRowIndex++);
                    c.GoToNextColor();
                }
                else {
                    string toBeAppended = $" {s.Type}{s.Number}";
                    if (!((string) currentTextBlock.Content).Contains(toBeAppended))
                        currentTextBlock.Content += toBeAppended;
                }

                previousSubjectName = s.SubjectName;
            }
        }

        private Button GenerateCopyButton(string codeToBeCopied) {
            var button = new Button
            {
                Margin = new Thickness(1),
                Width = 200,
                Content = $"Copy code {codeToBeCopied}",
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            button.Click += (sender, args) =>
            {
                Clipboard.SetDataObject(codeToBeCopied);
                AutoClosePopup.Show($"{codeToBeCopied} is copied to clipboard!");
                button.Background = Brushes.DarkCyan;
            };

            return button;
        }

        private Label GenerateSubjectCode(IColorGenerator c) {
            return new Label
            {
                FontWeight = FontWeights.Bold,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Background = c.GetCurrentBrush()
            };
        }

        private void AddGridChildren(UIElement child, int columnIndex, int rowIndex) {
            SubjectDescGrid.Children.Add(child);
            Grid.SetColumn(child, columnIndex);
            Grid.SetRow(child, rowIndex);
        }

        private Border GenerateSubjectName(Slot s, IColorGenerator c) {
            var textblock = new TextBlock
            {
                Text = s.SubjectName,
                Margin = new Thickness(2),
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.DemiBold,
                VerticalAlignment = VerticalAlignment.Center
            };
            var border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Black,
                Child = textblock,
                Background = c.GetCurrentBrush()
            };
            return border;
        }
    }
}