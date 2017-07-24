namespace Time_Table_Arranging_Program.Class.TokenParser {
    public interface ITokenStream {
        void GoToNextToken();
        IToken PreviousToken();
        IToken CurrentToken();
        IToken NextToken();
        bool IsAtLastToken();
        int CurrentIndex { get; }        
    }

    public class TokenStream : ITokenStream {
        private readonly IToken[] _tokens;
        private int _currentIndex;

        public TokenStream(IToken[] tokens) {
            _tokens = tokens;
        }

        public void GoToNextToken() {
            _currentIndex++;
        }

        public IToken PreviousToken() {
            if (_currentIndex == 0) return new EmptyToken();
            return _tokens[_currentIndex - 1];
        }

        public IToken CurrentToken() {
            return _tokens[_currentIndex];
        }

        public IToken NextToken() {
            if (_currentIndex == _tokens.Length - 1) return new EmptyToken();
            return _tokens[_currentIndex + 1];
        }

        public bool IsAtLastToken() {
            return _currentIndex == _tokens.Length - 1;
        }

        public int CurrentIndex => _currentIndex;
    }
}