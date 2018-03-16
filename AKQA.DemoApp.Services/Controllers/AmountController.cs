using AKQA.DemoApp.Domain.Entities;
using AKQA.DemoApp.Domain.Exceptions;
using AKQA.DemoApp.Services.Helpers;
using AKQA.DomoApp.Services.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AKQA.DemoApp.Services.Controllers
{
    [Route("api/[controller]")]    
    public class AmountController : Controller
    {
        /// <summary>
        /// Enpoint to convert amount to words
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConvertToWords/{amount}")]
        [EnableCors("AllowAKQADemoApp")]
        public IActionResult ConvertToWords(string amount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(amount))
                {
                    return NotFound();
                }                

                if (!decimal.TryParse(amount, out decimal input))
                {
                    return BadRequest(new GetAmountTextResponse
                    {
                        IsSuccess = false,
                        Error = new ApiResponseError
                        {
                            Code = AmountToWordsConverterErrorCodes.InvalidAmount.ToString(),
                            Message = EnumHelper.GetDescription(AmountToWordsConverterErrorCodes.InvalidAmount)
                        }
                    });
                }

                return Ok(new GetAmountTextResponse
                {
                    IsSuccess = true,
                    Result = AmountToWordsConverter.Convert(input)
                });
            }
            catch (AmountToWordsConverterException ex)
            {
                return BadRequest(new GetAmountTextResponse
                {
                    IsSuccess = false,
                    Error = new ApiResponseError
                    {
                        Code = ex.ErrorCode.ToString(),
                        Message = EnumHelper.GetDescription(ex.ErrorCode)
                    }
                });
            }
        }
    }
}
