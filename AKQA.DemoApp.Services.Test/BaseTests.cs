namespace AKQA.DemoApp.Services.Test
{
    /// <summary>
    /// This class provide common functionalities for tests
    /// </summary>
    public class BaseTests
    {
        /// <summary>
        /// Demo app service base url
        /// </summary>
        public string DemoAppServiceBaseUrl
        {
            get
            {
                return "http://localhost:51986/api/";
            }
        }
    }
}
