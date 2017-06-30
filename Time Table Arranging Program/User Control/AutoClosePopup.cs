using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;

namespace Time_Table_Arranging_Program.User_Control {
    public class AutoClosePopup : Popup {
        private static AutoClosePopup _singleton;

        private readonly DispatcherTimer _timer;

        private AutoClosePopup() {
            Opened += Popup_Opened;
            Closed += Popup_Closed;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(2);
            AllowsTransparency = true;            
            Placement = PlacementMode.Bottom;
            HorizontalAlignment = HorizontalAlignment.Center;
            PopupAnimation = PopupAnimation.Slide;
        }

        public static void Show(string message) {
            if (_singleton == null) {
                _singleton = new AutoClosePopup();
            }
            var border = new Border
            {
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(5),
                Margin = new Thickness(5),
                Background = Brushes.Black,
                Child = new Label
                {
                    Content = message,
                    FontSize = 15,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.DemiBold
                }
            };
            _singleton.Child = border;
            _singleton._timer.Stop();
            _singleton.IsOpen = false;
            _singleton.IsOpen = true;
        }

        private void Popup_Opened(object sender, EventArgs e) {
            _timer.Start();
            _timer.Tick += delegate
            {
                IsOpen = false;
                _timer.Stop();
            };
        }

        private void Popup_Closed(object sender, EventArgs e) {
            _timer.IsEnabled = false;
        }
    }
}