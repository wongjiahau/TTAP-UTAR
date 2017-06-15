using System;
using System.Globalization;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace Time_Table_Arranging_Program.Class {
    public class StartDateEndDateParser {
        private readonly DateTime _endDate;
        private readonly DateTime _startDate;

        public StartDateEndDateParser(string input) {
            ITokenStream tokenStream = new TokenStream(Tokenizer.Tokenize(input));

            while (true) {
                if (tokenStream.CurrentToken().Value().ToLower() == "(weeks)") {
                    tokenStream.GoToNextToken();
                    _startDate = ParseDate(tokenStream.CurrentToken().Value());
                    tokenStream.GoToNextToken();
                    _endDate = ParseDate(tokenStream.NextToken().Value());
                    return;
                }

                if (tokenStream.IsAtLastToken()) return;
                tokenStream.GoToNextToken();
            }
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