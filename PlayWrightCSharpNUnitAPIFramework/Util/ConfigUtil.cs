using Newtonsoft.Json.Linq;
using System.IO;

namespace PlayWrightCSharpNUnitAPIFramework.Util
{
    public class ConfigUtil
    {
        public static string GetConfig(string parameter)
        {
            string configStream = File.ReadAllText("../../../AppConfig.json");
            var config = JObject.Parse(configStream);
            string configValue = config.GetValue(parameter).ToString();
            return configValue;
        }

        public static string GetBearerToken(string parameter)
        {
            string configStream = File.ReadAllText("../../../bearerToken.json");
            var config = JObject.Parse(configStream);
            string configValue = config.GetValue(parameter).ToString();
            return configValue;
        }

        public static string GetResponse(string parameter)
        {
            string configStream = File.ReadAllText("../../../response.json");
            var config = JObject.Parse(configStream);
            string configValue = config.GetValue(parameter).ToString();
            return configValue;
        }
    }
}
