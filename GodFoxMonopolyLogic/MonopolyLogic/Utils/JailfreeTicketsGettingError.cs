using System;
using System.Runtime.Serialization;

namespace MonopolyLogic.Utils
{
    public class JailfreeTicketsGettingError : Exception
    {
        public JailfreeTicketsGettingError() { }

        public JailfreeTicketsGettingError(string message) : base(message) { }

        public JailfreeTicketsGettingError(string message, Exception inner) : base(message, inner) { }

        protected JailfreeTicketsGettingError(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
