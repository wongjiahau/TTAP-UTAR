using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using Time_Table_Arranging_Program.MVVM_Framework;

namespace Time_Table_Arranging_Program.User_Control {
    public class AutoClosePopup : Popup {
        private static AutoClosePopup _singleton;

        private readonly DispatcherTimer _timer;
        private Label _label;

        public string Message {
            get { return (string)_label.Content; }
            set { _label.Content = value; }
        }
        private AutoClosePopup() {
            Opened += Popup_Opened;
            Closed += Popup_Closed;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5);
            AllowsTransparency = true;
            PlacementTarget = Global.MainWindow;
            Placement = PlacementMode.Center;
            VerticalOffset = - (Global.MainWindow.ActualHeight / 2 - 90);
            HorizontalAlignment = HorizontalAlignment.Center;
            PopupAnimation = PopupAnimation.Slide;
            InitializeLabel();
            InitializeChild();

        }

        private void InitializeLabel() {
            _label = new Label {
                Content = "" ,
                FontSize = 15 ,
                Foreground = Brushes.White ,
                FontWeight = FontWeights.DemiBold
            };
        }

        private void InitializeChild() {
            var border = new Border {
                CornerRadius = new CornerRadius(5) ,
                Padding = new Thickness(5) ,
                Margin = new Thickness(5) ,
                Background = Brushes.Black ,
                Child = new StackPanel() {
                    Orientation = Orientation.Horizontal ,
                    Children = {
                        _label,
                        new Button
                        {
                            Content = "OK",
                            Command = new RelayCommand(()=> { _singleton.IsOpen = false; })
                        }
                    }
                }

            };
            this.Child = border;
        }

        public static void Show(string message) {
            if (_singleton == null) {
                _singleton = new AutoClosePopup();
            }
            _singleton.Message = message;
            _singleton._timer.Stop();
            _singleton.IsOpen = false;
            _singleton.IsOpen = true;
        }

        private void Popup_Opened(object sender , EventArgs e) {
            _timer.Start();
            _timer.Tick += delegate {
                IsOpen = false;
                _timer.Stop();
            };
        }

        private void Popup_Closed(object sender , EventArgs e) {
            _timer.IsEnabled = false;
        }
    }
}