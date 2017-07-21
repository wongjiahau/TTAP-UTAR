using System;
using System.Globalization;
using Time_Table_Arranging_Program.Class.Parser;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace Time_Table_Arranging_Program.Class {
    public sealed class StartDateEndDateFinder : TokenFinder {
        private  DateTime _endDate;
        private  DateTime _startDate;

        public StartDateEndDateFinder(string input) : base(input) { }

        private DateTime ParseDate(string input) {
            return DateTime.ParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public DateTime GetStartDate() {
            return _startDate;
        }

        public DateTime GetEndDate() {
            return _endDate;
        }

        protected override void ExtractToken(ITokenStream tokenStream) {
            tokenStream.GoToNextToken();
            _startDate = ParseDate(tokenStream.CurrentToken().Value());
            tokenStream.GoToNextToken();
            _endDate = ParseDate(tokenStream.NextToken().Value());
        }

        protected override bool HaltingCondition(ITokenStream tokenStream) {
            return tokenStream.CurrentToken().Value().ToLower() == "(weeks)";
        }
    }
}