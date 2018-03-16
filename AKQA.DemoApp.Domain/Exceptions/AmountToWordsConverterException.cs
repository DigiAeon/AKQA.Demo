using System;

namespace AKQA.DemoApp.Domain.Exceptions
{
    public class AmountToWordsConverterException : Exception
    {
        public AmountToWordsConverterErrorCodes ErrorCode { get; private set; }

        internal AmountToWordsConverterException(AmountToWordsConverterErrorCodes code)
        {
            ErrorCode = code;
        }
    }
}
