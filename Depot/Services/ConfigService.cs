namespace Depot.Services
{
    using Depot.Enitities;
    using Newtonsoft.Json;

    public class ConfigService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public Config GetConfig()
        {
            if (File.Exists("config.json"))
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json")) ?? new();
            return new();
        }
    }
}