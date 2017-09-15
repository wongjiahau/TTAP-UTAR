using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using HtmlAgilityPack;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Model;

namespace Time_Table_Arranging_Program {
    public static class ExtensionMethods {
        public static List<Slot> GetSelectedSlots(this List<SubjectModel> subjectModels) {
            var result = new List<Slot>();
            for (var i = 0 ; i < subjectModels.Count ; i++) {
                var model = subjectModels[i];
                result.AddRange(model.GetSelectedSlots());
            }
            return result;
        }

        public static bool ScrambledEquals<T>(this IEnumerable<T> list1 , IEnumerable<T> list2) {
            var cnt = new Dictionary<T , int>();
            foreach (var s in list1) {
                if (cnt.ContainsKey(s)) {
                    cnt[s]++;
                }
                else {
                    cnt.Add(s , 1);
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
                withoutBracket = s.Substring(0 , s.IndexOf("("));
            var tokens = withoutBracket.Split(' ');
            var filtrate = tokens.ToList();
            filtrate.RemoveAll(x => x == "&");
            var result = "";
            foreach (string t in filtrate) {
                if (t == "") continue;
                if (t.All(c => char.ToLower(c) == 'i')) {
                    result += "-" + t;
                    continue;
                }
                result += t.ToUpper()[0];
            }
            return result;
        }

        public static bool IsInteger(this string s) {
            return s.All(Char.IsDigit);
        }

        public static string TruncateRight(this string s, int maxCharLimit) {
            if (s.Length <= maxCharLimit) return s;
            return s.Substring(0, maxCharLimit) + "...";
        }

        public static string Beautify(this string s) {
            var tokens = s.Split(' ');
            for (var i = 0 ; i < tokens.Length ; i++) {
                if (tokens[i].ToLower() == "and") { tokens[i] = "&";
                    continue;
                }
                if (tokens[i].All(t => t == 'I')) continue;
                if (tokens[i] == "") continue;
                tokens[i] = tokens[i].ToLower();
                tokens[i] = Char.ToUpper(tokens[i][0]) + tokens[i].Substring(1 , tokens[i].Length - 1);
            }
            var result = "";
            foreach (var t in tokens) {
                result += t + " ";
            }            
            return result.Substring(0 , result.Length - 1).TrimStart(' ').TrimEnd(' ');
        }

        public static Color Darker(this Color c) {
            return Color.FromArgb(c.A,
                (byte) (c.R*0.8), (byte) (c.G*0.8), (byte) (c.B*0.8));
        }

        public static Color Lighter(this Color c) {
            return Color.FromArgb(c.A ,
                (byte)(c.R * 1.25) , (byte)(c.G * 1.25) , (byte)(c.B * 1.25));
        }

        public static void WaitForSeconds(double seconds) {
            var s = Stopwatch.StartNew();
            while (s.ElapsedMilliseconds / 1000.0 < seconds) ;
        }

        public static string RemoveTags(string html) {                        
                string result = "";
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//text()")) {
                    result += node.InnerText;
                }
                return result;            
        }
    }
}