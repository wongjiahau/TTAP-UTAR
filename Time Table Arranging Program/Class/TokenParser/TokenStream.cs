namespace Time_Table_Arranging_Program.Class.TokenParser {
    public interface ITokenStream {
        void GoToNextToken();
        IToken PreviousToken();
        IToken CurrentToken();
        IToken NextToken();
        bool IsAtLastToken();
    }

    public class TokenStream : ITokenStream {
        private readonly IToken[] _tokens;
        private int counter;

        public TokenStream(IToken[] tokens) {
            _tokens = tokens;
        }

        public void GoToNextToken() {
            counter++;
        }

        public IToken PreviousToken() {
            if (counter == 0) return new EmptyToken();
            return _tokens[counter - 1];
        }

        public IToken CurrentToken() {
            return _tokens[counter];
        }

        public IToken NextToken() {
            if (counter == _tokens.Length - 1) return new EmptyToken();
            return _tokens[counter + 1];
        }

        public bool IsAtLastToken() {
            return counter == _tokens.Length - 1;
        }
    }
}