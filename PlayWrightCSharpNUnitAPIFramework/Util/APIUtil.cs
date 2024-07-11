using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using Microsoft.Playwright;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;

namespace PlayWrightCSharpNUnitAPIFramework.Util
{
    [Binding]
    public class APIUtil
    {
        public APIUtil(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
        }

        public APIUtil()
        {

        }

        private IAPIRequestContext _requestContext;
        private static ExtentReports? _extentReports;
        private readonly FeatureContext _featureContext;
        private readonly ScenarioContext _scenarioContext;
        private ExtentTest _scenario;
        private static ExtentTest _feature;
        private static readonly Dictionary<string, ExtentTest> _featureTests = new();

        public async Task InitializeRequestContextAsync(string baseUrl)
        {
            var playwright = Playwright.CreateAsync().Result;
            _requestContext = playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions()
            {
                BaseURL = baseUrl,
                IgnoreHTTPSErrors = true,
            }).Result;
        }

        [BeforeTestRun]
        public static void InitializeExtentReports()
        {
            _extentReports = new ExtentReports();
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../APITestResults/APITestReports.html");
            var spark = new ExtentSparkReporter(reportPath);
            spark.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Dark;
            _extentReports.AttachReporter(spark);
        }

        [BeforeScenario("@API")]
        public async Task BeforeScenario()
        {
            var featureTitle = _featureContext.FeatureInfo.Title;
            if (!_featureTests.ContainsKey(featureTitle))
            {
                _feature = _extentReports.CreateTest<Feature>(featureTitle);
                _featureTests.Add(featureTitle, _feature);
            }
            else
            {
                _feature = _featureTests[featureTitle];
            }

            _scenario = _feature.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
        }

        [AfterStep("@API")]
        public async Task AfterStep()
        {
            var fileName = $"{_featureContext.FeatureInfo.Title.Trim()}_{Regex.Replace(_scenarioContext.ScenarioInfo.Title, @"\s", string.Empty)}";

            if (_scenarioContext.TestError == null)
            {
                switch (_scenarioContext.StepContext.StepInfo.StepDefinitionType)
                {
                    case StepDefinitionType.Given:
                        _scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                    case StepDefinitionType.When:
                        _scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                    case StepDefinitionType.Then:
                        _scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                var errorMessage = _scenarioContext.TestError.Message;
                switch (_scenarioContext.StepContext.StepInfo.StepDefinitionType)
                {
                    case StepDefinitionType.Given:
                        _scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(errorMessage);
                        break;
                    case StepDefinitionType.When:
                        _scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(errorMessage);
                        break;
                    case StepDefinitionType.Then:
                        _scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(errorMessage);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            _extentReports?.Flush();
        } 

        public async Task<IAPIResponse> ExecuteGet(string endPoint)
        {
            var response = await _requestContext.GetAsync(endPoint);
            return response;
        }

        public async Task<IAPIResponse> ExecuteGet(string endPoint, Dictionary<string, string> headers)
        {
            var response = await _requestContext.GetAsync(endPoint, new APIRequestContextOptions
            {
                Headers = headers
            });

            return response;
        }

        public async Task<IAPIResponse> ExecutePost(string endPoint, object requestBody)
        {
            var response = await _requestContext.PostAsync(endPoint, new APIRequestContextOptions
            {
                DataObject = requestBody
            });

            return response;
        }


        public async Task<IAPIResponse> ExecutePost(string endPoint, object requestBody, Dictionary<string, string> headers)
        {
            var response = await _requestContext.PostAsync(endPoint, new APIRequestContextOptions
            {
                DataObject = requestBody,
                Headers = headers
            });

            return response;
        }
    }
}

