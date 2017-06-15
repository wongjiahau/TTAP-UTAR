using System.Linq;
using Time_Table_Arranging_Program.Class.Converter;

namespace Time_Table_Arranging_Program.Class.TokenParser {
    public interface IToken {
        bool IsPositiveInteger();
        bool IsPositiveNumberThatContainDecimalPoint();
        bool IsTime();
        bool IsSlotType();
        bool IsDay();
        bool IsPossiblySubjectCode();
        bool IsPossiblyVenuValue();
        string Value();
    }

    public class Token : IToken {
        private readonly string _value;

        public Token(string value) {
            _value = value;
        }

        public bool IsPositiveInteger() {
            return _value.All(char.IsDigit);
        }

        public bool IsPositiveNumberThatContainDecimalPoint() {
            var decimalCounter = 0;
            foreach (var x in _value) {
                if (x == '.') {
                    decimalCounter++;
                    continue;
                }
                if (!char.IsDigit(x)) return false;
            }
            return decimalCounter == 1;
        }

        public bool IsTime() {
            if (!_value.Contains(":")) return false;
            if (_value[0] == ':' || _value[_value.Length - 1] == ':') return false;
            var tokens = _value.Split(':');
            if (new Token(tokens[0]).IsPositiveInteger() && new Token(tokens[1]).IsPositiveInteger())
                return true;
            return false;
        }

        public bool IsSlotType() {
            string[] slotType = {"L", "T", "P"};
            return slotType.Contains(_value);
        }

        public bool IsDay() {
            return Day.IsDay(_value);
        }

        public bool IsPossiblySubjectCode() {
            if (_value == "") return false;
            return char.IsLetter(_value.First()) && char.IsDigit(_value.Last());
        }

        public bool IsPossiblyVenuValue() {
            if (_value.Length == 0) return false;
            if (char.ToLower(_value[0]) == 'k' && char.IsLetterOrDigit(_value.Last())) return true;
            return false;
        }

        public string Value() {
            return _value;
        }
    }

    public class EmptyToken : Token {
        public EmptyToken() : base("") {}
    }
}