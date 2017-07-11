using System.Windows.Forms;
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
            Colors.Yellow,
            Colors.LightGreen,
            Colors.LightBlue,                          
            Colors.MediumPurple,
            Colors.GreenYellow,
            Colors.MediumVioletRed,
            Colors.LightGray,
            Colors.LightGoldenrodYellow,
            Colors.DeepSkyBlue,
            Colors.LightCoral,

        };

        private int _pointer;

        public void GoToNextColor() {
            _pointer++;
        }

        public Color GetCurrentColor() {
            if (_pointer == _chosenColors.Length) return Colors.White;
            return _chosenColors[_pointer];
        }

        public Brush GetCurrentBrush() {
            return new SolidColorBrush(GetCurrentColor());
        }
    }
}