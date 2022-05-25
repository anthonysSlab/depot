namespace Depot.Modules
{
    using Depot.Services;
    using Discord;
    using Discord.Commands;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly UpdateService updateService;

        public AdminModule(UpdateService updateService)
        {
            this.updateService = updateService;
        }

        [Command("any news")]
        public async Task CheckUpdates()
        {
            if (Context.User.Id != 308203742736678914 && Context.User.Id != 627015977233678336) return;

            string latestVersion = await updateService.GetLatestVersion();
            string currentVersion = UpdateService.GetCurrentVersion();
            int versionComparison = await updateService.CheckVersionAsync();

            StringBuilder sb = new();
            sb.AppendLine($"Current version: {currentVersion}");
            sb.AppendLine($"Latest version: {latestVersion}");
            sb.AppendLine(versionComparison < 0 ? "A new version is available" : versionComparison > 0 ? ":O you are using a newer version than my mom published" : "The bot is up to date");
            sb.AppendLine();
            sb.AppendLine("Changelog:");
            sb.AppendLine(await updateService.GetChangelog());

            EmbedBuilder builder = new();
            builder.AddField(new EmbedFieldBuilder()
            {
                Name = "Update check",
                Value = sb.ToString()
            });

            if (versionComparison < 0)
            {
                await ReplyAsync(embed: builder.Build(), components: new ComponentBuilder().WithButton("Update", "updatebutton").Build());
            }
            else
                await ReplyAsync(embed: builder.Build());
        }

        [Command("testupdatescript")]
        public Task TestScript()
        {
            if (Context.User.Id != 308203742736678914 && Context.User.Id != 627015977233678336) return Task.CompletedTask;
            updateService.TestUpdateScript(Context.Channel.Id);
            return Task.CompletedTask;
        }

        [Command("restart")]
        public async Task RestartBot()
        {
            if (Context.User.Id != 308203742736678914 && Context.User.Id != 627015977233678336) return;
            await ReplyAsync("Restarting...");
            updateService.Restart();
        }
    }
}