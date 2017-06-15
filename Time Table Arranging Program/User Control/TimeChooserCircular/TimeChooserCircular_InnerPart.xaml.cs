using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Time_Table_Arranging_Program.User_Control.TimeChooserCircular {
    /// <summary>
    ///     Interaction logic for TimeChooserCircular.xaml
    /// </summary>
    public partial class TimeChooserCircular_InnerPart : UserControl {
        private readonly List<TimeHand> hourHands = new List<TimeHand>();
        private readonly List<TimeHand> minuteHands = new List<TimeHand>();
        private SolidColorBrush _color;

        public TimeChooserCircular_InnerPart() {
            InitializeComponent();
            var hour = 1;
            var minute = 5;
            for (var angle = 0; angle < 360; angle += 30) {
                if (minute == 60) minute = 0;
                var hourHand = new TimeHand
                {
                    RotateAngle = angle,
                    Number = hour,
                    Type = TimeHandType.Hour,
                    HandIsVisible = hour == 1,
                    Color = Color
                };
                hourHand.ButtonClicked += HourHand_ButtonClicked;
                var minuteHand = new TimeHand
                {
                    RotateAngle = angle,
                    Number = minute,
                    Type = TimeHandType.Minute,
                    HandIsVisible = minute == 0,
                    Color = Color
                };
                minuteHand.ButtonClicked += MinuteHand_ButtonClicked;
                Grid.Children.Add(hourHand);
                Grid.Children.Add(minuteHand);

                hourHands.Add(hourHand);
                minuteHands.Add(minuteHand);
                hour++;
                minute += 5;
            }
        }

        public SolidColorBrush Color {
            get { return _color; }
            set
            {
                foreach (var hand in hourHands) {
                    hand.Color = value;
                }
                foreach (var hand in minuteHands) {
                    hand.Color = value;
                }
                _color = value;
            }
        }


        public int ChosenHour {
            get { return hourHands.Find(x => x.HandIsVisible).Number; }
            set
            {
                foreach (var hand in hourHands) {
                    hand.HandIsVisible = hand.Number == value;
                }
            }
        }

        public int ChosenMinute {
            get { return minuteHands.Find(x => x.HandIsVisible).Number; }
            set
            {
                foreach (var hand in minuteHands) {
                    hand.HandIsVisible = hand.Number == value;
                }
            }
        }

        public event EventHandler TimeChanged;

        private void MinuteHand_ButtonClicked(object sender, EventArgs e) {
            foreach (var t in minuteHands) {
                if (t.Number != (sender as TimeHand).Number)
                    t.HandIsVisible = false;
            }
            TimeChanged(this, null);
        }

        private void HourHand_ButtonClicked(object sender, EventArgs e) {
            foreach (var t in hourHands) {
                if (t.Number != (sender as TimeHand).Number)
                    t.HandIsVisible = false;
            }
            TimeChanged(this, null);
        }
    }
}