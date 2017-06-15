using System.Windows.Media;

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
            Colors.LightGoldenrodYellow,
            Colors.LightGreen,
            Colors.LightBlue,
            Colors.MediumPurple,
            Colors.LightGray,
            Colors.Honeydew,
            Colors.LightCoral
        };

        private int pointer;

        public void GoToNextColor() {
            pointer++;
        }

        public Color GetCurrentColor() {
            if (pointer == _chosenColors.Length) return Colors.White;
            return _chosenColors[pointer];
        }

        public Brush GetCurrentBrush() {
            return new SolidColorBrush(GetCurrentColor());
        }
    }
}