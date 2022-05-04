namespace Depot.Services
{
    using Depot.Enitities;
    using Newtonsoft.Json;

    public class ConfigService
    {
        public Config GetConfig()
        {
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json")) ?? new();
        }
    }
}