using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace Time_Table_Arranging_Program.User_Control {
    public class MaybeUnoccupiedToggleButton : ToggleButton {
        public static SolidColorBrush BackgroundColor = Brushes.LightGreen;

        public MaybeUnoccupiedToggleButton() {
            Checked += OnChecked;
            Unchecked += OnUnchecked;
            Background = BackgroundColor;
            Cursor = Cursors.Hand;
        }

        private void OnUnchecked(object sender, RoutedEventArgs e) {
            Content = new Border {Background = BackgroundColor};
        }

        private void OnChecked(object sender, RoutedEventArgs routedEventArgs) {
            Content = new PackIcon {Kind = PackIconKind.Close, Width = 30, Height = 30};
        }
    }
}