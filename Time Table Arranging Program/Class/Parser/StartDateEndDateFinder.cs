using System;
using System.Globalization;
using Time_Table_Arranging_Program.Class.Parser;
using Time_Table_Arranging_Program.Class.TokenParser;
using HtmlAgilityPack;

namespace Time_Table_Arranging_Program.Class {
    public sealed class StartDateEndDateFinder {
        private readonly DateTime _endDate;
        private readonly DateTime _startDate;

        public StartDateEndDateFinder(string input) {
            var doc = new HtmlDocument();
            doc.LoadHtml(input);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div");
            HtmlNode table = nodes[1].SelectSingleNode("table");
            HtmlNode tableRow =
                table.SelectSingleNode("tr") ??
                table.SelectNodes("tbody")[0].SelectSingleNode("tr");
            HtmlNodeCollection tableColumn = tableRow.SelectNodes("td");
            string targetColumn = tableColumn[9].InnerText;
            string[] parseTargetColumn = targetColumn.Split(' ');
            _startDate = DateTime.ParseExact(parseTargetColumn[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            _endDate = DateTime.ParseExact(parseTargetColumn[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        private DateTime ParseDate(string input) {
            return DateTime.ParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public DateTime GetStartDate() {
            return _startDate;
        }

        public DateTime GetEndDate() {
            return _endDate;
        }
    }
}