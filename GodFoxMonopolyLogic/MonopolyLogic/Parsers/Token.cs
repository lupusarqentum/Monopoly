namespace MonopolyLogic.Parsers
{
    internal enum TokenType
    {
        Keyword,
        Operator,
        
        Word,

        Number,
        String,

        Eof,
    }

    internal sealed class Token
    {
        internal readonly TokenType type;
        internal readonly string text;

        internal static string OperatorsChars = "()[],=";
        internal static string[] Keywords = { "new", };
        internal static string[] Operators = { "(", ")", "[", "]", ",", "=", };

        public Token(TokenType type, string text)
        {
            this.type = type;
            this.text = text;
        }

        public override string ToString() 
            => $"Token {type}: {text}";
    }
}
