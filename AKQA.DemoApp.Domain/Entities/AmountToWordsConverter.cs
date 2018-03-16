using AKQA.DemoApp.Domain.Exceptions;

namespace AKQA.DemoApp.Domain.Entities
{
    public static class AmountToWordsConverter
    {
        /// <summary>
        /// Convert to words.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string Convert(decimal dollarAmount)
        {
            if (decimal.Round(dollarAmount, 2) != dollarAmount)
            {
                throw new AmountToWordsConverterException(AmountToWordsConverterErrorCodes.InvalidAmount);
            }
            else if (dollarAmount < 0)
            {
                throw new AmountToWordsConverterException(AmountToWordsConverterErrorCodes.NegativeAmountNotSupported);
            }
            else if (dollarAmount > 999999999999999.99m)
            {
                throw new AmountToWordsConverterException(AmountToWordsConverterErrorCodes.ExceededMaxSupportedAmount);
            }

            if (dollarAmount == 0)
            {
                return "ZERO DOLLAR";
            }

            var dollarParts = dollarAmount.ToString("F").Split('.');

            var dollars = dollarParts.Length > 0 ? System.Convert.ToInt64(dollarParts[0]) : 0;
            var cents = dollarParts.Length > 1 ? System.Convert.ToInt64(dollarParts[1]) : 0;

            string result = string.Empty;

            if (dollars > 0)
            {
                result = $"{ConvertToWords(dollars)} {(dollars == 1 ? "DOLLAR" : "DOLLARS" )}";
            }

            if (dollars > 0 && cents > 0)
            {
                result = $"{result} AND";
            }

            if (cents > 0)
            {
                result = $"{result} {ConvertToWords(cents)} {(cents == 1 ? "CENT" : "CENTS")}";
            }

            return result.Replace("  ", " ").Trim();
        }

        /// <summary>
        /// This method converts the 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private static string ConvertToWords(long number)
        {
            var amount = number.ToString();

            // Postfix after last 3 digits
            var last3digitPostfix = new[] { "", " THOUSAND ", " MILLION ", " BILLION ", " TRILLIAN " };
            
            var i = 0;
            var result = string.Empty;

            while (amount.Length > 0)
            {
                // Get last 3 digits from dollar
                string last3digits = amount.Length < 3
                                        ? amount
                                        : amount.Substring(amount.Length - 3);

                // Remove the 3 last digits from dollar
                amount = amount.Length < 3
                            ? ""
                            : amount.Remove(amount.Length - 3);
                
                last3digits = ConvertThreeDigitNumberToWords(int.Parse(last3digits));
                
                result = last3digits + last3digitPostfix[i] + result.Trim();

                i++;
            }

            return result;
        }

        /// <summary>
        /// This method will convert number not more than 999 to words.
        /// </summary>
        /// <param name="number">Number (between 0 to 999)</param>        
        /// <returns></returns>
        private static string ConvertThreeDigitNumberToWords(int number)
        {
            var Ones = new[]
            {
                "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN",
                "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINTEEN"
            };

            var Tens = new[]
            {
                "TEN", "TWENTY", "THIRTY", "FOURTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINTY"
            };

            var word = string.Empty;

            var isEligibleToUseAnd = number > 100 && number % 100 > 0;

            if (number > 99 && number < 1000)
            {
                var i = number / 100;
                word = word + Ones[i - 1] + " HUNDRED ";
                number = number % 100;
            }

            if (isEligibleToUseAnd)
            {
                word = word + "AND ";
            }

            var hasSecondLastDigit = false;

            if (number > 19 && number < 100)
            {
                hasSecondLastDigit = true;

                var i = number / 10;
                word = word + Tens[i - 1];
                number = number % 10;
            }

            if (number > 0 && number < 20)
            {
                if (hasSecondLastDigit)
                {
                    word = word + "-";
                }

                word = word + Ones[number - 1];
            }            

            return word;
        }
    }
}
