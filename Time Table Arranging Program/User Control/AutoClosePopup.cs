using System;
using System.Collections;
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
        private AutoClosePopup() {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            Opened += Popup_Opened;
            Closed += Popup_Closed;
            AllowsTransparency = true;
            PlacementTarget = Global.MainWindow;
            Placement = PlacementMode.Center;
            VerticalOffset = -(Global.MainWindow.ActualHeight / 2 - 90);
            HorizontalAlignment = HorizontalAlignment.Center;
            PopupAnimation = PopupAnimation.Slide;
            this._label = GetLabel();
            this._button = new Button();
            this.Child = GetBorder();
        }

        private void Initialize(string message, string actionContent, Action actionHandler) {
            Action defaultAction = () => { this.IsOpen = false; };
            Action resultAction;
            if (actionHandler == null) resultAction = defaultAction;
            else resultAction = (Action) Delegate.Combine(new List<Action>() {defaultAction, actionHandler}.ToArray());
            _label.Content = message;
            _button.Content = actionContent;
            _button.Command = new RelayCommand(resultAction);
        }

        private Label GetLabel() {
            return new Label {                
                FontSize = 15 ,
                Foreground = Brushes.White ,
                FontWeight = FontWeights.DemiBold
            };
        }


        private Label _label;
        private Button _button;
        private Border GetBorder() {
            return new Border {
                CornerRadius = new CornerRadius(5) ,
                Padding = new Thickness(5) ,
                Margin = new Thickness(5) ,
                Background = Brushes.Black ,
                Child = new StackPanel() {
                    Orientation = Orientation.Horizontal ,
                    Children = {
                        _label,
                        _button
                    }
                }
            };            
        }

        public static void Show(string message , string actionContent = "OK", Action actionHandler = null) {
            if (_singleton == null) {
                _singleton = new AutoClosePopup();
            }            
            _singleton.Initialize(message, actionContent, actionHandler);
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