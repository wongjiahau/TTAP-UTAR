using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Time_Table_Arranging_Program.User_Control.TimeChooserCircular {
    /// <summary>
    ///     Interaction logic for TimeHand.xaml
    /// </summary>
    public enum TimeHandType {
        Hour,
        Minute
    }

    public interface ITimeHand {
        TimeHandType Type { set; }
        int Number { get; set; }
        bool HandIsVisible { get; set; }
        int RotateAngle { set; }
        SolidColorBrush Color { set; }
        event EventHandler ButtonClicked;
    }

    public partial class TimeHand : UserControl, ITimeHand {
        private const int LengthOfHourHand = 100;

        private SolidColorBrush _color = Brushes.Black;

        private TimeHandType _type;

        public TimeHand() {
            InitializeComponent();
        }

        public TimeHandType Type {
            get { return _type; }
            set {
                _type = value;
                if (_type == TimeHandType.Hour) {
                    Rectangle.Width = LengthOfHourHand;
                    Button.FontSize = Button.FontSize + 5;
                    Rectangle.Height = Rectangle.Height + 5;
                }
                else if (_type == TimeHandType.Minute) Rectangle.Width = LengthOfHourHand + 40;
            }
        }

        public event EventHandler ButtonClicked;

        public int Number {
            get { return int.Parse(Button.Content.ToString()); }
            set { Button.Content = value; }
        }

        public bool HandIsVisible {
            get { return Rectangle.Visibility == Visibility.Visible; }
            set {
                if (value) {
                    Rectangle.Visibility = Visibility.Visible;
                    FillCircle(true);
                }
                else {
                    Rectangle.Visibility = Visibility.Hidden;
                    FillCircle(false);
                }
            }
        }

        public int RotateAngle {
            set {
                var centreOfRotation = Button.Width / 2;
                RenderTransform = new RotateTransform(value - 60, centreOfRotation, centreOfRotation);
                Button.RenderTransform = new RotateTransform(-(value - 60), centreOfRotation, centreOfRotation);
            }
        }

        public SolidColorBrush Color {
            get { return _color; }
            set {
                _color = value;
                Rectangle.Fill = value;
            }
        }

        private void Button_OnClick(object sender, RoutedEventArgs e) {
            FillCircle(true);
            if (HandIsVisible == false)
                HandIsVisible = true;
            ButtonClicked?.Invoke(this, null);
        }

        private void FillCircle(bool Fill) {
            if (Fill) {
                Button.Background = Color ?? TimeChooser.PrimaryColor;
                Button.Foreground = TimeChooser.SecondaryColor;
            }
            else {
                Button.Background = TimeChooser.ClockBackgroundColor;
                Button.Foreground = Brushes.Black;
            }
        }
    }
}