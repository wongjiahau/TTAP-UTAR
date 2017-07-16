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
    public class NotificationBar : Popup {
        private static NotificationBar _singleton;                
        protected NotificationBar() {                                    
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

        protected void Initialize(string message , string actionContent , Action actionHandler , bool closePopupAfterActionButtonClicked = true) {
            Action defaultAction = () => { this.IsOpen = false; };
            Action resultAction;
            if (actionHandler == null) resultAction = defaultAction;
            else {
                if (closePopupAfterActionButtonClicked) {
                    resultAction =
                        (Action)Delegate.Combine(new List<Action>() { defaultAction , actionHandler }.ToArray());
                }
                else {                    
                    resultAction = actionHandler;
                }
            }
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


        private readonly Label _label;
        private readonly Button _button;
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

        public static void Show(string message , string actionContent = "OK" , Action actionHandler = null , bool closePopupAfterActionButtonClicked = true) {
            if (_singleton == null) {
                _singleton = new NotificationBar();
            }
            _singleton.Initialize(message , actionContent , actionHandler , closePopupAfterActionButtonClicked);            
            _singleton.IsOpen = false;
            _singleton.IsOpen = true;
        }

   

    }

    public class AutoCloseNotificationBar : NotificationBar {
        private static AutoCloseNotificationBar _singleton;
        private const int ShowPopupDuration = 3;//seconds
        private readonly DispatcherTimer _timer;
        private AutoCloseNotificationBar() {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(ShowPopupDuration) };
            Opened += Popup_Opened;
            Closed += Popup_Closed;
            AllowsTransparency = true;
        }
               
        public new static void Show(string message , string actionContent = "OK" , Action actionHandler = null , bool closePopupAfterActionButtonClicked = true) {
            if (_singleton == null) {
                _singleton = new AutoCloseNotificationBar();
            }
            _singleton.Initialize(message , actionContent , actionHandler , closePopupAfterActionButtonClicked);
            _singleton._timer.Stop();
            _singleton.IsOpen = false;
            _singleton.IsOpen = true;
        }

        private void Popup_Opened(object sender , EventArgs e) {
            _timer.Start();
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender , EventArgs e) {
            IsOpen = false;
            _timer.Stop();
        }

        private void Popup_Closed(object sender , EventArgs e) {
            _timer.IsEnabled = false;
        }
    }
}