using System;
using System.Drawing;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace Time_Table_Arranging_Program {
    public interface IColorGenerator {
        void GoToNextColor();
        Color GetCurrentColor();
        Brush GetCurrentBrush();
    }

    public class ColorGenerator : IColorGenerator {
        private readonly Color[] _chosenColors =
        {
            Colors.LightPink,
            Colors.Orange,
            Colors.Yellow,
            Colors.LightGreen,
            Colors.LightBlue,
            Colors.MediumPurple,
            Colors.LightGray,
            Colors.LightGoldenrodYellow,
            Colors.MediumPurple,
            Colors.MediumVioletRed,
            Colors.DeepSkyBlue,
            Colors.LightCoral
        };

        private int _pointer;

        public void GoToNextColor() {
            _pointer++;
        }

        public Color GetCurrentColor() {
            if (_pointer >= _chosenColors.Length) return RandomColor();
            return _chosenColors[_pointer];
        }

        private Color RandomColor() {
            var randomGen = new Random();
            var names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            var color = System.Drawing.Color.FromKnownColor(randomColorName);
            return ConvertToSystemWindowMediaColor(color);
        }

        private Color ConvertToSystemWindowMediaColor(System.Drawing.Color color) {
            return System.Windows.Media.Color.FromArgb(color.A , color.R , color.G , color.B);
        }

        public Brush GetCurrentBrush() {
            return new SolidColorBrush(GetCurrentColor());
        }
    }
}