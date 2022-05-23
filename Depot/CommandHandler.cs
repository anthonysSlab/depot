using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace Depot;

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IServiceProvider _services;

    // Retrieve client and CommandService instance via ctor
    public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
    {
        _commands = commands;
        _client = client;
        _services = services;
    }

    public async Task InitializeAsync()
    {
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        _commands.Log += LogAsync;
        _client.MessageReceived += HandleMessageAsync;
    }

    private Task LogAsync(LogMessage logMessage)
    {
        Console.WriteLine(logMessage.Message);
        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(SocketMessage socketMessage)
    {
        var argPos = 0;
        if (socketMessage.Author.IsBot) return;

        if (socketMessage is not SocketUserMessage userMessage)
            return;

        if (!(
            userMessage.HasCharPrefix('!', ref argPos) ||
            userMessage.HasCharPrefix('$', ref argPos) ||
            userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos) ||
            userMessage.Author.IsBot))
            return;

        var context = new SocketCommandContext(_client, userMessage);
        await _commands.ExecuteAsync(context, argPos, _services);
    }
}