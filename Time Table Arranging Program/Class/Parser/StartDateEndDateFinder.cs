using System;
using System.Globalization;
using Time_Table_Arranging_Program.Class.Parser;
using Time_Table_Arranging_Program.Class.TokenParser;
using HtmlAgilityPack;

namespace Time_Table_Arranging_Program.Class {
    public sealed class StartDateEndDateFinder{
        private  DateTime _endDate;
        private  DateTime _startDate;

        public StartDateEndDateFinder(string input)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(input);

            //get timetable start date and end date
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div");

            HtmlNodeCollection tableColumn = nodes[1].SelectNodes("td");

            string targetColumn = tableColumn[9].InnerText;


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