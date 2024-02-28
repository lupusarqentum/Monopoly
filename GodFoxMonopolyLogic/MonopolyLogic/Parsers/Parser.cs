using System;
using System.Collections.Generic;
using MonopolyLogic.Parsers.ast;
using MonopolyLogic.Parsers.Values;

namespace MonopolyLogic.Parsers
{
    internal sealed class Parser
    {
        private readonly List<Token> tokens;

        private int pos;

        internal Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            pos = 0;
        }

        internal List<IStatement> Parse()
        {
            var statements = new List<IStatement>();

            while (Peek().type != TokenType.Eof)
            {
                var token = Peek();

                if (token.type == TokenType.Word)
                {
                    var nextToken = Peek(1);

                    if (nextToken.type == TokenType.Operator && nextToken.text.Equals("="))
                        statements.Add(AssignmentStatement());
                    else if (nextToken.type == TokenType.Operator && nextToken.text.Equals("("))
                        statements.Add(FunctionCallingStatement());
                    else
                        throw new Exception("Unknown statement");
                    continue;
                }

                throw new Exception("Unknown statement");
            }

            return statements;
        }

        private IStatement FunctionCallingStatement()
        {
            var functionName = Peek().text;
            Next(2);

            var arguments = new List<IExpression>();

            while (true)
            {
                var exprCtorArgument = GetExpression();

                try
                {
                    Consume(TokenType.Operator, ",");
                }
                catch (Exception)
                {
                    Consume(TokenType.Operator, ")");
                    arguments.Add(exprCtorArgument);
                    return new FunctionCallingStatement(functionName, arguments.ToArray());
                }

                arguments.Add(exprCtorArgument);
            }
        }

        private IStatement AssignmentStatement()
        {
            var name = Peek().text;
            Next(2);
            var expr = GetExpression();
            return new AssignmentStatement(name, expr);
        }

        private IExpression GetExpression() => GetPrimaryExpression();

        private IExpression GetPrimaryExpression()
        {
            var token = Peek();
            Next();

            return token.type switch
            {
                TokenType.Number => new ValueExpression(new NumberValue(Convert.ToInt32(token.text))),
                TokenType.String => new ValueExpression(new StringValue(token.text)),
                TokenType.Operator when token.text.Equals("[") => GetArrayExpression(),
                TokenType.Word when Match(TokenType.Operator, "(") => FunctionCallingExpression(token.text),
                TokenType.Word => new VariableAccessingExpression(token.text),
                TokenType.Keyword when token.text.Equals("new") => GetNewExpression(),
                _ => throw new Exception("Unknown expression"),
            };
        }

        private IExpression FunctionCallingExpression(string functionName)
        {
            var arguments = new List<IExpression>();

            while (true)
            {
                var exprCtorArgument = GetExpression();

                try
                {
                    Consume(TokenType.Operator, ",");
                }
                catch (Exception)
                {
                    Consume(TokenType.Operator, ")");
                    arguments.Add(exprCtorArgument);
                    return new FunctionCallingExpression(functionName, arguments.ToArray());
                }

                arguments.Add(exprCtorArgument);
            }
        }

        private IExpression GetNewExpression()
        {
            string name = Consume(TokenType.Word).text;
            var arguments = new List<IExpression>();

            Consume(TokenType.Operator, "(");

            while (true)
            {
                var exprCtorArgument = GetExpression();

                try
                {
                    Consume(TokenType.Operator, ",");
                }
                catch (Exception)
                {
                    Consume(TokenType.Operator, ")");
                    arguments.Add(exprCtorArgument);
                    return new NewExpression(name, arguments);
                }

                arguments.Add(exprCtorArgument);
            }
        }

        private IExpression GetArrayExpression()
        {
            var arrayElements = new List<IExpression>();

            while (true)
            {
                var exprArrayElement = GetExpression();
                
                try
                {
                    Consume(TokenType.Operator, ",");
                }
                catch (Exception)
                {
                    Consume(TokenType.Operator, "]");
                    arrayElements.Add(exprArrayElement);
                    return new ArrayExpression(arrayElements);
                }

                arrayElements.Add(exprArrayElement);
            }
        }

        private bool Consume(TokenType type, string text)
        {
            var peekToken = Peek();
            if (peekToken.type == type && peekToken.text.Equals(text))
            {
                Next();
                return true;
            }
            throw new Exception($"Syntax Error, was waiting for token {type}:{text}");
        }

        private bool Match(TokenType type, string text)
        {
            var peekToken = Peek();
            if (peekToken.type == type && peekToken.text.Equals(text))
            {
                Next();
                return true;
            }
            return false;
        }

        private Token Consume(TokenType type)
        {
            var peekToken = Peek();
            if (peekToken.type == type)
            {
                Next();
                return peekToken;
            }
            throw new Exception($"Syntax Error, was waiting for token type {type}");
        }

        private Token Next(int step = 1)
        {
            pos += step;
            return Peek();
        }

        private Token Peek(int position = 0)
        {
            var relativePosition = pos + position;
            return relativePosition >= tokens.Count ? new Token(TokenType.Eof, "") : tokens[relativePosition];
        }
    }
}
