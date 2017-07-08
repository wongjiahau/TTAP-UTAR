using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            IColorGenerator c = new ColorGenerator();
            var gridRowIndex = 0;
            var subjects = Subject.GroupIntoSubjects(slots);
            foreach (Subject s in subjects) {
                SubjectDescGrid.RowDefinitions.Add(new RowDefinition());
                AddGridChildren(GetSubjectNameLabel(s.Name , c) , 0 , gridRowIndex);
                AddGridChildren(GetLabel(c , s.Lecture) , 1 , gridRowIndex);
                AddGridChildren(GetLabel(c , s.Tutorial) , 2 , gridRowIndex);
                AddGridChildren(GetLabel(c , s.Practical) , 3 , gridRowIndex);
                AddGridChildren(GetCopyButton(s.Code) , 4 , gridRowIndex);
                gridRowIndex++;
                c.GoToNextColor();
            }
        }
        public void GenerateAsImage(List<Slot> slots) {
            SubjectDescGrid.Children.Clear();
            IColorGenerator c = new ColorGenerator();
            var gridRowIndex = 0;
            var subjects = Subject.GroupIntoSubjects(slots);
            foreach (Subject s in subjects) {
                SubjectDescGrid.RowDefinitions.Add(new RowDefinition());
                AddGridChildren(GetLabel(c , s.Code) , 0 , gridRowIndex);
                AddGridChildren(GetSubjectNameLabel(s.Name , c, false) , 1 , gridRowIndex);
                AddGridChildren(GetLabel(c , s.Lecture) , 2 , gridRowIndex);
                AddGridChildren(GetLabel(c , s.Tutorial) , 3 , gridRowIndex);
                AddGridChildren(GetLabel(c , s.Practical) , 4 , gridRowIndex);                
                gridRowIndex++;
                c.GoToNextColor();
            }
        }

        private Button GetCopyButton(string codeToBeCopied) {
            var button = new Button {
                Margin = new Thickness(1) ,
                Width = 180 ,
                Content = $"Copy code {codeToBeCopied}" ,
                FontSize = 12 ,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            button.Click += (sender , args) => {
                Clipboard.SetDataObject(codeToBeCopied);
                AutoClosePopup.Show($"{codeToBeCopied} is copied to clipboard!");
                button.Background = Brushes.DarkCyan;
            };

            return button;
        }

        private Label GetLabel(IColorGenerator c , string content = "") {
            return new Label {
                Content = content ,
                FontWeight = FontWeights.Bold ,
                VerticalContentAlignment = VerticalAlignment.Center ,
                HorizontalContentAlignment = HorizontalAlignment.Center ,
                FontFamily = new FontFamily("Consolas") ,
                FontSize = 13.5 ,
                BorderBrush = Brushes.Black ,
                BorderThickness = new Thickness(1) ,
                Background = c.GetCurrentBrush()
            };
        }

        private void AddGridChildren(UIElement child , int columnIndex , int rowIndex) {
            SubjectDescGrid.Children.Add(child);
            Grid.SetColumn(child , columnIndex);
            Grid.SetRow(child , rowIndex);
        }

        private Border GetSubjectNameLabel(string subjectName , IColorGenerator c, bool truncate = true) {
            var textblock = new TextBlock {
                Text = subjectName,
                Margin = new Thickness(2) ,
                TextAlignment = TextAlignment.Center ,
                FontWeight = FontWeights.DemiBold ,
                VerticalAlignment = VerticalAlignment.Center
            };
            if (truncate) textblock.Text = subjectName.TruncateRight(30);
            var border = new Border {
                BorderThickness = new Thickness(1) ,
                BorderBrush = Brushes.Black ,
                Child = textblock ,
                Background = c.GetCurrentBrush() ,
                ToolTip = subjectName
            };
            var originalColor = c.GetCurrentColor();
            var originalBrush = c.GetCurrentBrush();
            border.MouseEnter += (sender , args) => {
                border.Background = new SolidColorBrush(originalColor.Darker());
            };
            border.MouseLeave += (sender , args) => {
                border.Background = originalBrush;
            };

            return border;
        }
    }

    public class Subject {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Lecture { get; set; }
        public string Tutorial { get; set; }
        public string Practical { get; set; }
        public Subject(List<Slot> slots) {            
            Name = slots[0].SubjectName;
            Code = slots[0].Code;
            Lecture = "-";
            Tutorial = "-";
            Practical = "-";
            foreach (var s in slots) {
                switch (s.Type) {
                    case "L": Lecture = $"{s.Type}-{s.Number}"; break;
                    case "T": Tutorial = $"{s.Type}-{s.Number}"; break;
                    case "P": Practical = $"{s.Type}-{s.Number}"; break;
                }
            }
        }
        public static List<Subject> GroupIntoSubjects(List<Slot> slots) {
            var result = new List<Subject>();
            var dic = new Dictionary<string , List<Slot>>();
            foreach (Slot s in slots) {
                if (!dic.ContainsKey(s.Code)) {
                    dic.Add(s.Code , new List<Slot>());
                }
                dic[s.Code].Add(s);
            }
            foreach (KeyValuePair<string , List<Slot>> entry in dic) {
                result.Add(new Subject(entry.Value));
            }
            return result;

        }
    }
}