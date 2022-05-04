namespace Depot
{
    using Depot.Enitities;
    using Depot.Services;
    using Discord;
    using Discord.Commands;
    using Discord.Interactions;
    using Discord.WebSocket;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    public class DePotClient
    {
        private readonly DiscordSocketClient client;
        private readonly InteractionService interactionService;
        private readonly CommandService cmdService;
        private IServiceProvider? services;
        private readonly TextService textService;
        private readonly LogService logService;
        private readonly ReactionService reactionService;
        private readonly ConfigService configService;
        private readonly Config config;

        public DePotClient()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                MessageCacheSize = 50,
                LogLevel = LogSeverity.Debug
            });

            cmdService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Debug,
                SeparatorChar = ' ',
                CaseSensitiveCommands = false
            });

            interactionService = new(client, new InteractionServiceConfig()
            {
                LogLevel = LogSeverity.Debug,
                ThrowOnError = true,
            });

            logService = new();
            configService = new();
            textService = new(client);
            reactionService = new(client);
            config = configService.GetConfig();
        }

        public async Task InitializeAsync()
        {
            services = SetupServices();
            client.Log += LogAsync;
            client.Ready += Ready;

            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task Ready()
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var cmdHandler = new CommandHandler(client, cmdService, services);
#pragma warning restore CS8604 // Possible null reference argument.
            await cmdHandler.InitializeAsync();

            var intHandler = new InteractionHandler(client, interactionService, services);
            await intHandler.InitializeAsync();
            await client.SetGameAsync("uwu");
        }

        private async Task LogAsync(LogMessage logMessage)
        {
            await logService.LogAsync(logMessage);
        }

        private IServiceProvider SetupServices()
            => new ServiceCollection()
                    .AddSingleton(client)
                    .AddSingleton(cmdService)
                    .AddSingleton(interactionService)
                    .AddSingleton(logService)
                    .AddSingleton(textService)
                    .AddSingleton(reactionService)
                    .BuildServiceProvider();
    }
}