namespace Depot
{
    using Discord;
    using Discord.Interactions;
    using Discord.WebSocket;
    using System;
    using System.Reflection;

    public class InteractionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _interaction;
        private readonly IServiceProvider _services;

        // Retrieve client and CommandService instance via ctor
        public InteractionHandler(DiscordSocketClient client, InteractionService interaction, IServiceProvider services)
        {
            _interaction = interaction;
            _client = client;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _interaction.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _interaction.Log += LogAsync;
            _client.ButtonExecuted += ButtonExecuted;
            _client.InteractionCreated += async interaction =>
            {
                var ctx = new SocketInteractionContext(_client, interaction);
                await _interaction.ExecuteCommandAsync(ctx, _services);
            };

            await _interaction.RegisterCommandsGloballyAsync();
        }

        private async Task ButtonExecuted(SocketMessageComponent arg)
        {
            var ctx = new SocketInteractionContext<SocketMessageComponent>(_client, arg);
            await _interaction.ExecuteCommandAsync(ctx, _services);
        }

        private Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }
    }
}