using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace Time_Table_Arranging_Program.Class.Parser {
    public abstract class TokenFinder {
        protected TokenFinder(string input) {
            ITokenStream tokenStream = new TokenStream(Tokenizer.Tokenize(input));

            while (true) {
                if (HaltingCondition(tokenStream)) {
                    ExtractToken(tokenStream);
                }

                if (tokenStream.IsAtLastToken()) return;
                tokenStream.GoToNextToken();
            }
        }

        protected abstract void ExtractToken(ITokenStream tokenStream);

        protected abstract bool HaltingCondition(ITokenStream tokenStream);
    }
}