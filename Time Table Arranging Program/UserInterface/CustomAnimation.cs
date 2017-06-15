using System;
using System.Windows.Media.Animation;

namespace Time_Table_Arranging_Program.UserInterface {
    public class CustomAnimation {
        public const double DecelerationConstant = 0.7;

        /// <summary>
        ///     These constants are based on Google's Material Design guidelines
        ///     https://material.io/guidelines/motion/duration-easing.html#duration-easing-common-durations
        /// </summary>
        public static readonly TimeSpan FullScreenAnimationDuration = TimeSpan.FromSeconds(0.375);

        public static readonly TimeSpan EnteringScreenAnimationDuration = TimeSpan.FromSeconds(0.225);

        public static readonly TimeSpan LeavingScreenAnimationDuration = TimeSpan.FromSeconds(0.195);

        private static DoubleAnimation GetDoubleAnimation(double from, double to, bool setFillBehaviorStop = true) {
            var da = new DoubleAnimation {From = from, To = to, DecelerationRatio = DecelerationConstant};
            if (setFillBehaviorStop)
                da.FillBehavior = FillBehavior.Stop;
            return da;
        }

        public static DoubleAnimation GetFullScreenAnimation(double from, double to, bool setFillBehaviorStop = true) {
            var result = GetDoubleAnimation(from, to, setFillBehaviorStop);
            result.Duration = FullScreenAnimationDuration;
            return result;
        }

        public static DoubleAnimation GetEnteringScreenAnimation(double from, double to, bool setFillBehaviorStop = true) {
            var result = GetDoubleAnimation(from, to, setFillBehaviorStop);
            result.Duration = EnteringScreenAnimationDuration;
            return result;
        }

        public static DoubleAnimation GetLeavingScreenAnimation(double from, double to, bool setFillBehaviorStop = true) {
            var result = GetDoubleAnimation(from, to, setFillBehaviorStop);
            result.Duration = LeavingScreenAnimationDuration;
            return result;
        }
    }
}