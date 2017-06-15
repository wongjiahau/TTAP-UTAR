using System.Collections.Generic;
using System.Linq;

namespace Time_Table_Arranging_Program {
    public static class ExtensionMethods {
        public static bool ScrambledEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2) {
            var cnt = new Dictionary<T, int>();
            foreach (var s in list1) {
                if (cnt.ContainsKey(s)) {
                    cnt[s]++;
                }
                else {
                    cnt.Add(s, 1);
                }
            }
            foreach (var s in list2) {
                if (cnt.ContainsKey(s)) {
                    cnt[s]--;
                }
                else {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }

        public static string GetInitial(this string s) {
            var withoutBracket = s;
            if (s.Contains("("))
                withoutBracket = s.Substring(0, s.IndexOf("("));
            var tokens = withoutBracket.Split(' ');
            var filtrate = tokens.ToList();
            filtrate.RemoveAll(x => x.ToLower() == "and");
            var result = "";
            foreach (var t in filtrate) {
                if (t != "")
                    result += t.ToUpper()[0];
            }
            return result;
        }

        public static bool IsInteger(this string s) {
            return s.All(char.IsDigit);
        }
    }
}