using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PlayWrightCSharpNUnitAPIFramework.Util
{
    [Binding]
    public class UserCreationStep
    {
        private IAPIResponse _apiResponse;
        private IPlaywright playwright;
        private IAPIRequestContext _requestContext;
        public readonly APIUtil _apiUtil;
        private readonly string _baseUrl;

        public UserCreationStep()
        {
            _apiUtil = new APIUtil();
            _baseUrl = ConfigUtil.GetConfig("BookStoreBaseUrl");
            InitializeRequestContext().Wait();
        }

        private async Task InitializeRequestContext()
        {
            await _apiUtil.InitializeRequestContextAsync(_baseUrl);
        }

        // For GET Call

        [When(@"I Send '([^']*)' Request to '([^']*)' Endpoint of '([^']*)'")]
        public async Task WhenISendRequestToEndpointOf(string method, string endPoint, string baseUrl)
        {
            _apiResponse = await _apiUtil.ExecuteGet(endPoint);
        }

        [When(@"I will verify the Status Code is (.*)")]
        public async Task WhenIWillVerifyTheStatusCodeIs(int statusCode)
        {
            Assert.AreEqual(_apiResponse.Status, statusCode);
        }

        [Then(@"I will verify Response is Contains Following details")]
        public async Task ThenIWillVerifyResponseIsContainsFollowingDetails(Table table)
        {
            var responseBody = await _apiResponse.TextAsync();
            Assert.IsTrue(responseBody.Contains(table.Rows[0][0]));
            Assert.IsTrue(responseBody.Contains(table.Rows[0][1]));
            Assert.IsTrue(responseBody.Contains(table.Rows[0][2]));
        }

        // For Post Call

        [When(@"I Send '([^']*)' Request to '([^']*)' Endpoint with '([^']*)'")]
        public async Task WhenISendRequestToEndpointWith(string method, string endPoint, string baseUrl, Table table)
        {
            var requestBody = new
            {
                UserName = table.Rows[0][0],
                Password = table.Rows[0][1]
            };
            _apiResponse = await _apiUtil.ExecutePost(endPoint, requestBody);
        }

        [Then(@"I will verify response info contains following details")]
        public async Task ThenIWillVerifyResponseInfoContainsFollowingDetails(Table table)
        {
            var responseBody = await _apiResponse.TextAsync();
            Assert.IsTrue(responseBody.Contains(table.Rows[0][0]));
        }
    }
}
