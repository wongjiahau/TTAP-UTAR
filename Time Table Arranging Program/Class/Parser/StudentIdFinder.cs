using Time_Table_Arranging_Program.Class.TokenParser;

namespace Time_Table_Arranging_Program.Class.Parser {
    public sealed class StudentIdFinder : TokenFinder {
        private string _studentId;
        public StudentIdFinder(string input) : base(input) { }

        public string GetStudentId() {
            return _studentId;
        }

        protected override void ExtractToken(ITokenStream ts) {
            _studentId = ts.PreviousToken().Value().Trim('(', ')');
        }

        protected override bool HaltingCondition(ITokenStream ts) {
            return
                ts.CurrentToken().Value().ToLower() == "user" &&
                ts.NextToken().Value().ToLower() == "guide";
        }
    }
}