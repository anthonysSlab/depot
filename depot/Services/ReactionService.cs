namespace Depot.Services
{
    using Discord;
    using Discord.WebSocket;
    using Newtonsoft.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class ReactionService
    {
        private const string Path = "reactions.json";
        private readonly DiscordSocketClient _client;
        private readonly Random random = new();
        private ReactionFile file;

        public ReactionService(DiscordSocketClient client)
        {
            _client = client;
            _client.MessageReceived += MessageReceived;
            file = ReactionFile.Load(Path);
        }

        public void ReloadReactions()
        {
            file = ReactionFile.Load(Path);
        }

        private async Task MessageReceived(SocketMessage arg)
        {
            if (arg.Source.HasFlag(MessageSource.Bot))
                return;

            Reaction? reaction = file.Reactions.FirstOrDefault(x => x.Regex.IsMatch(arg.CleanContent));

            if (reaction != null)
            {
                await arg.Channel.SendMessageAsync(reaction.GetRandomReaction(random));
            }
        }
    }

    public class ReactionFile
    {
        public List<Reaction> Reactions { get; set; } = new();

        public static ReactionFile Load(string path)
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<ReactionFile>(File.ReadAllText(path)) ?? new();
            }
            else
            {
                ReactionFile file = new();
                file.Save(path);
                return file;
            }
        }

        public void Save(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }

    public class Reaction
    {
        private Regex? regex;

        public string Target { get; set; } = string.Empty;

        public List<string> Replies { get; set; } = new();

        [JsonIgnore]
        public Regex Regex
        {
            get
            {
                if (regex == null)
                {
                    regex = new Regex(Target, RegexOptions.Compiled);
                }
                return regex;
            }
        }

        public string GetRandomReaction(Random random)
        {
            return Replies[random.Next(0, Replies.Count - 1)];
        }
    }
}