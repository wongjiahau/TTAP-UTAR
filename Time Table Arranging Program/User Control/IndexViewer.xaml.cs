using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Time_Table_Arranging_Program.MVVM_Framework.Models;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    ///     Interaction logic for PreviousNextButton.xaml
    /// </summary>
    public partial class IndexViewer : UserControl {
        private CyclicIndex _cyclicIndex;

        public IndexViewer() {
            InitializeComponent();
        }

        public void Initialize(CyclicIndex cyclicIndex) {
            _cyclicIndex = cyclicIndex;
            DataContext = _cyclicIndex;
        }


        private void PreviousButton_OnClick(object sender, RoutedEventArgs e) {
            _cyclicIndex.CurrentValue--;
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e) {
            _cyclicIndex.CurrentValue++;
        }

        private void RandomButton_OnClick(object sender, RoutedEventArgs e) {
            var rnd = new Random();
            int result = rnd.Next(0, _cyclicIndex.MaxValue);
            while (result == _cyclicIndex.CurrentValue) result = rnd.Next(0, _cyclicIndex.MaxValue + 1);
            _cyclicIndex.CurrentValue = result;
            PackIcon nextIcon;
            while (true) {
                var currentIcon = RandomButton.Content as PackIcon;
                nextIcon = GetRandomDiceIcon();
                if (currentIcon.Kind == nextIcon.Kind) continue;
                break;
            }
            RandomButton.Content = nextIcon;
        }

        private PackIcon GetRandomDiceIcon() {
            var rand = new Random();
            switch (rand.Next(1, 7)) {
                case 1:
                    return new PackIcon {Kind = PackIconKind.Dice1};
                case 2:
                    return new PackIcon {Kind = PackIconKind.Dice2};
                case 3:
                    return new PackIcon {Kind = PackIconKind.Dice3};
                case 4:
                    return new PackIcon {Kind = PackIconKind.Dice4};
                case 5:
                    return new PackIcon {Kind = PackIconKind.Dice5};
                case 6:
                    return new PackIcon {Kind = PackIconKind.Dice6};
                default:
                    goto case 5;
            }
        }
    }

    public class IndexViewerVisibilityConverter : System.Windows.Data.IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if ((int) value < 0) {
                return Visibility.Hidden;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}