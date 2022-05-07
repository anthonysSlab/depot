namespace Depot.Services;

using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

public class TextService
{
    private readonly Dictionary<string, string> _text = new();
    private readonly DiscordSocketClient _client;

    public TextService(DiscordSocketClient client)
    {
        _client = client;
        _client.MessageReceived += MessageReceived;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        _text = JsonConvert.DeserializeObject<UwuText>(File.ReadAllText("replies.json")).Content;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    private async Task MessageReceived(SocketMessage arg)
    {
        static string Ragge(string arg)
        {
            if (arg == null) return "no";
            arg = arg.Replace("'", "");
            Regex regex = new(@"^Im (.*)$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled); //additional words return no FIIIIIIX
            Match match = regex.Match(arg);

            if (match.Success)
            {
                string name = match.Groups[1].Value;
                string output = $"hello {name}, I'm pot";
                return output;
            }
            return "no";
        }

        if (arg.Author.Id == 159985870458322944) { await arg.Channel.SendMessageAsync("FUCK YE BOT!"); return; };
        if (arg.Author.Id == 962301614172041237) return;
        if (Ragge(arg.CleanContent) != "no")
        {
            await arg.Channel.SendMessageAsync(Ragge(arg.CleanContent));
            return;
        }
        if (arg.Author.IsBot) return;
        if (arg.Author is not IGuildUser user) return;
        //if (user.GuildId != 817259966485889044) return;
        if (arg.Content.StartsWith('!')) return;
        var answer = _text.FirstOrDefault(x => arg.Content.Contains(x.Key, StringComparison.CurrentCultureIgnoreCase));
        if (answer.Value != null)
        {
            await arg.Channel.SendMessageAsync(answer.Value);
        }
    }
}

public class UwuText
{
    public Dictionary<string, string> Content { get; set; } = new();
}