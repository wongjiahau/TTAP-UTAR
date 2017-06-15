using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Class {
    public static class ExtensionClass {
        public static string Beautify(this string s) {
            var tokens = s.Split(' ');
            for (var i = 0; i < tokens.Length; i++) {
                if (tokens[i].All(t => t == 'I')) continue;
                if (tokens[i] == "") continue;
                tokens[i] = tokens[i].ToLower();
                tokens[i] = char.ToUpper(tokens[i][0]) + tokens[i].Substring(1, tokens[i].Length - 1);
            }
            var result = "";
            foreach (var t in tokens) {
                result += t + " ";
            }
            return result.Substring(0, result.Length - 1);
        }
    }
}