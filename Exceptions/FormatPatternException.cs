using System;
using System.Collections.Generic;
using System.Text;

namespace TradeReportsConverter.Exceptions
{
    [Serializable]
    class FormatPatternException : Exception
    {
        public FormatPatternException()
        {
        }

        public FormatPatternException(string message) : base(message)
        {
        }

        public FormatPatternException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
