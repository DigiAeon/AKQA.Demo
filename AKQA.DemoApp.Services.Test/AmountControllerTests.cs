using RestSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AKQA.DomoApp.Services.Models;
using AKQA.DemoApp.Domain.Exceptions;
using AKQA.DemoApp.Services.Helpers;

namespace AKQA.DemoApp.Services.Test.Extensions
{
    /// <summary>
    /// Test class for amount controller
    /// </summary>
    [TestClass]
    public class AmountControllerTests : BaseTests
    {
        private const string ControllerName = "Amount";

        /// <summary>
        /// This method calls {ControllerName}/ConvertToWords/{amount} endpoint to get result
        /// </summary>
        /// <param name="amount">Amount to pass in api endpoint</param>
        /// <returns></returns>
        private IRestResponse<GetAmountTextResponse> ConvertToWords(string amount)
        {
            var client = new RestClient(DemoAppServiceBaseUrl);
            var request = new RestRequest($"{ControllerName}/ConvertToWords/{amount}");

            return client.Execute<GetAmountTextResponse>(request);
        }

        /// <summary>
        /// When valid amount is passed then convert to words api should return result with OK status code
        /// </summary>
        /// <param name="amount">Test amount</param>
        /// <param name="expectedResult">Expected result</param>
        [TestMethod]
        [DataRow("0.1", "TEN CENTS")]
        [DataRow("1", "ONE DOLLAR")]
        [DataRow("0.01", "ONE CENT")]
        [DataRow("0.0", "ZERO DOLLAR")]
        [DataRow("0", "ZERO DOLLAR")]
        [DataRow("123.45", "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FOURTY-FIVE CENTS")]
        [DataRow("101.40", "ONE HUNDRED AND ONE DOLLARS AND FOURTY CENTS")]
        [DataRow("100000.99", "ONE HUNDRED THOUSAND DOLLARS AND NINTY-NINE CENTS")]
        [DataRow("100500.99", "ONE HUNDRED THOUSAND FIVE HUNDRED DOLLARS AND NINTY-NINE CENTS")]
        [DataRow("101501.99", "ONE HUNDRED AND ONE THOUSAND FIVE HUNDRED AND ONE DOLLARS AND NINTY-NINE CENTS")]
        [DataRow("10101501.99", "TEN MILLION ONE HUNDRED AND ONE THOUSAND FIVE HUNDRED AND ONE DOLLARS AND NINTY-NINE CENTS")]
        [DataRow("900010101501.99", "NINE HUNDRED BILLION TEN MILLION ONE HUNDRED AND ONE THOUSAND FIVE HUNDRED AND ONE DOLLARS AND NINTY-NINE CENTS")]
        [DataRow("451900010101501.99", "FOUR HUNDRED AND FIFTY-ONE TRILLIAN NINE HUNDRED BILLION TEN MILLION ONE HUNDRED AND ONE THOUSAND FIVE HUNDRED AND ONE DOLLARS AND NINTY-NINE CENTS")]
        [DataRow("999999999999999.99", "NINE HUNDRED AND NINTY-NINE TRILLIAN NINE HUNDRED AND NINTY-NINE BILLION NINE HUNDRED AND NINTY-NINE MILLION NINE HUNDRED AND NINTY-NINE THOUSAND NINE HUNDRED AND NINTY-NINE DOLLARS AND NINTY-NINE CENTS")]
        public void WHEN_Valid_Amount_Is_Passed_THEN_Convert_To_Words_Api_Should_Return_OK(string amount, string expectedResult)
        {
            var response = ConvertToWords(amount);

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.AreEqual(response.Data.IsSuccess, true);
            Assert.AreEqual(response.Data.Error, null);
            Assert.IsTrue(string.Compare(response.Data.Result, expectedResult, true) == 0);
        }

        /// <summary>
        /// When invalid amount is passed then convert to words api should return error with Bad Request code
        /// </summary>
        /// <param name="amount">Test amount</param>
        /// <param name="expectedErrorCode">Expected error code</param>
        [TestMethod]
        [DataRow("12.32s", "InvalidAmount")]        
        [DataRow("-0.01", "NegativeAmountNotSupported")]
        [DataRow("9999999999999991.99", "ExceededMaxSupportedAmount")]
        public void WHEN_Invalid_Amount_Is_Passed_THEN_Should_Return_BadRequest(string amount, string expectedErrorCode)
        {
            var response = ConvertToWords(amount);
            var errorCode = EnumHelper.Parse<AmountToWordsConverterErrorCodes>(expectedErrorCode);

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
            Assert.AreEqual(response.Data.IsSuccess, false);
            Assert.AreEqual(response.Data.Error.Code, errorCode.ToString());
            Assert.AreEqual(response.Data.Error.Message, EnumHelper.GetDescription(errorCode));
            Assert.IsNull(response.Data.Result);
        }

        /// <summary>
        /// When blank amount is passed then convert to words api should return Not Found code
        /// </summary>
        /// <param name="amount">Test amount</param>
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WHEN_Blank_Amount_Is_Passed_THEN_Should_Return_NotFound(string amount)
        {
            var response = ConvertToWords(amount);
            
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.NotFound);
            Assert.IsNull(response.Data);
        }
    }
}
