namespace Depot.Modules
{
    using Depot.Services;
    using Discord;
    using Discord.Interactions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class InteractionModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly UpdateService updateService;

        public InteractionModule(UpdateService updateService)
        {
            this.updateService = updateService;
        }

        [ComponentInteraction("aboutbutton")]
        public async Task About()
        {
            EmbedBuilder builder = new();

            StringBuilder sb = new();
            sb.AppendLine($"currently running version {Assembly.GetExecutingAssembly().GetName().Version}");
            sb.AppendLine("created by Juna");
            builder.AddField(new EmbedFieldBuilder()
            {
                Name = "About",
                Value = sb.ToString(),
            });
            await Context.Interaction.RespondAsync(embed: builder.Build());
        }

        [ComponentInteraction("updatebutton")]
        public async Task Update()
        {
            if (Context.User.Id != 308203742736678914 && Context.User.Id != 627015977233678336) return;
            EmbedBuilder builder = new();
            builder.AddField("Updater", "Update in progress this could take a minute or two...");
            await Context.Interaction.RespondAsync(embed: builder.Build());
            await updateService.Update(Context.Channel.Id);
        }
    }
}