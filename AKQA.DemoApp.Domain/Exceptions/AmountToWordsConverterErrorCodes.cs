using System.ComponentModel;

namespace AKQA.DemoApp.Domain.Exceptions
{
    public enum AmountToWordsConverterErrorCodes
    {
        [Description("Invalid amount")]
        InvalidAmount,

        [Description("Negative amount is not supported")]
        NegativeAmountNotSupported,

        [Description("Exceeded maximum supported amount (999999999999999.99)")]
        ExceededMaxSupportedAmount
    }
}
