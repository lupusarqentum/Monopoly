using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyLogic.Parsers
{
    internal sealed class Lexer
    {
        private readonly string input;
        private readonly List<Token> tokens;

        private bool wasTokenized;

        private int pos;

        public Lexer(string input)
        {
            this.input = input;
            tokens = new List<Token>();

            wasTokenized = false;
        }

        public List<Token> GetTokens()
        {
            if (wasTokenized) return tokens;
            wasTokenized = true;

            if (string.IsNullOrEmpty(input)) return tokens;

            var current = Peek();
            do
            {
                if (char.IsDigit(current)) TokenizeNumber();
                else if (char.IsLetter(current)) TokenizeWord();
                else if (current == '\"') TokenizeString();
                else if (Token.OperatorsChars.IndexOf(current) != -1) TokenizeOperator();
                else Next();

                current = Peek();
            } while (current != '\0');

            AddToken(new Token(TokenType.Eof, ""));

            return tokens;
        }

        private void TokenizeNumber()
        {
            var buffer = new StringBuilder();

            var current = Peek();
            while (char.IsDigit(current))
            {
                buffer.Append(current);
                current = Next();
            }

            AddToken(TokenType.Number, buffer.ToString());
        }

        private void TokenizeWord()
        {
            var buffer = new StringBuilder();

            var current = Peek();
            while (char.IsLetterOrDigit(current))
            {
                buffer.Append(current);
                current = Next();
            }

            var result = buffer.ToString();

            if (Array.IndexOf(Token.Keywords, result) != -1)
                AddToken(TokenType.Keyword, result);
            else
                AddToken(TokenType.Word, result);
        }

        private void TokenizeOperator()
        {
            var buffer = new StringBuilder();

            var current = Peek();
            while (IsItOperator(buffer.ToString(), current))
            {
                buffer.Append(current);
                current = Next();
            }

            AddToken(TokenType.Operator, buffer.ToString());
        }

        private static bool IsItOperator(string buffer, char current) 
            => Token.OperatorsChars.IndexOf(current) != -1 && Array.IndexOf(Token.Operators, buffer + current) != -1;

        private void TokenizeString()
        {
            var buffer = new StringBuilder();

            var current = Next();
            while (current != '\"')
            {
                buffer.Append(current);
                current = Next();
            }

            Next();

            AddToken(TokenType.String, buffer.ToString());
        }

        private char Next()
        {
            pos++;
            return Peek();
        }

        private char Peek(int position = 0)
        {
            var relativePosition = pos + position;
            return relativePosition >= input.Length ? '\0' : input[relativePosition];
        }

        private void AddToken(Token token) => tokens.Add(token);
        private void AddToken(TokenType type, string text) => AddToken(new Token(type, text));
    }
}
