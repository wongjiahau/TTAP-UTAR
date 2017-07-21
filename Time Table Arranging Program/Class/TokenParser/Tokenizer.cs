using System.Collections.Generic;
using System.Linq;

namespace Time_Table_Arranging_Program.Class.TokenParser {
    public static class Tokenizer {
        public static IToken[] Tokenize(string input) {
            input = FilterOutNonPrintingChar(input);


            var tokens = input.Split(' ', '\t', '\n', '\r');
            var result = new List<IToken>();
            foreach (var t in tokens) {
                result.Add(new Token(t));
            }
            result.RemoveAll(s => s.Value() == string.Empty);
            return result.ToArray();
        }

        public static string FilterOutNonPrintingChar(string input) {
            string result = " ";
            foreach (char c in input) {
                if ((c >= 0 && c <= 31) || c == 127) {
                    if (result.Last() != ' ')
                        result += " ";
                }
                else {
                    result += c.ToString();
                }
            }
            return result;
        }

        public static IToken[] Tokenize_Obsolete(string input) {
            var tokens = input.Split(' ', '\t', '\n', '\r');
            var result = new List<IToken>();
            foreach (var t in tokens) {
                result.Add(new Token(t));
            }
            return result.ToArray();
        }
    }
}