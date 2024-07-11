using Microsoft.Playwright;

namespace PlayWrightCSharpNUnitAPIFramework.Util
{
    internal class APIRequestOptions : APIRequestContextOptions
    {
        public object Data { get; set; }
    }
}