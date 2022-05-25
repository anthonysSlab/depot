using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Help()
        {
            if (Context.User.Id == 191521298772525056) { await ReplyAsync("pot works in mysterious ways"); return; }
            StringBuilder sb = new();

            sb.AppendLine("!help commands - list all commands that ya can access\n");
            sb.AppendLine("!help git - get a link to de github repo to contribute, see updates, and read more about de project\n");
            sb.AppendLine("!help donate - for all the wholesome beans that want to support this project :3\n");
            sb.AppendLine("!help wizcraft - link to wizcraft's official curse forge page, with all updates, downloads, and an extensive description (the latter still WIP)");

            await ReplyAsync(sb.ToString());
        }

        [Command("help git")]
        public async Task HelpG()
        {
            await ReplyAsync("herr it is! \n https://github.com/anthonysSlab/depot");
        }

        [Command("help donate")]
        public async Task HelpD()
        {
            await ReplyAsync("much thank :3 \n https://www.patreon.com/anthonyslab?fan_landing=true");
        }

        [Command("help wizcraft")]
        public async Task HelpW()
        {
            await ReplyAsync("herr it is! \n https://www.curseforge.com/minecraft/modpacks/wickedpack");
        }

        [Command("help commands")]
        public async Task HelpC()
        {
            StringBuilder sb = new();

            sb.AppendLine("!roll {int}d[int] rolls dice, the first number is the amount of dice rolled and the second is the number of sides those dice have.");
            sb.AppendLine("Max allowed amount is 15 and the only accepted amounts of sides are 2,4,6,8,10,12,20,100");
            sb.AppendLine("after a successful roll the bot will return all of the rolls and the total\n");

            if (Context.Guild.GetUser(Context.User.Id).GuildPermissions.ManageMessages)
            {
                sb.AppendLine("!uwuify[string]: makes ffe bot uwuify any stwing ffat you input, and dewete youw message\n");
                sb.AppendLine("!cay [channel] [string]: makes the bot say whatever string you input, in whatever channel ye set\n");
                sb.AppendLine("!say [string]: makes the pot say whatever string you input and then delete your message\n");
            }
            else
            {
                sb.AppendLine("!uwuify [string]: makes ffe bot uwuify any stwing ffat you input\n");
            }

            if (Context.Guild.GetUser(Context.User.Id).GuildPermissions.ViewAuditLog)
            {
                sb.AppendLine("!gam [string]: sets the pot's status to \"playing[string]\"\n");
                sb.AppendLine("!warns [memberId]: displays all of the warns that the user has with the exact date of their creation and reason\n");
            }

            if (Context.Guild.GetUser(Context.User.Id).GuildPermissions.KickMembers)
            {
                sb.AppendLine("!warn [memberId] [reason]: warns the user! and automatically executes an action, defferent to how many times the user was warned already.\n");
                sb.AppendLine("!unwarn [memberId]: removes a warn from a specified member. note: it will not remove a timeout or a ban and that has to be done manually.\n");
            }

            if (Context.Guild.GetUser(Context.User.Id).GuildPermissions.ManageWebhooks)
            {
                sb.AppendLine("!sacredscrolls: enables the activity kick feature\n");
                sb.AppendLine("!profanescrolls: disabled the activity kick feature\n");
                sb.AppendLine("!godmode [roleName]: add an immune role which prevents all that have it from beung kicked due to inactivity\n");
                sb.AppendLine("!devilmode [roleName]: removes an immune role\n");
                sb.AppendLine("!sacredinfo: spits out a wall of info containing:");
                sb.AppendLine("- the status of inactivity kicking");
                sb.AppendLine("- warn delay after inactivity");
                sb.AppendLine("- kick delay after inactivity");
                sb.AppendLine("- immune roles to kicks from inactivity");
                sb.AppendLine("- all tracked users, the last time they were active, and if they were warned about inactivity or not\n");
            }

            if (Context.Guild.GetUser(Context.User.Id).GuildPermissions.ManageGuild)
            {
                sb.AppendLine("!markedtime [TimeSpan]: sets the time at which an inacive member will recieve a warning about their inactivity\n");
            }

            await ReplyAsync(sb.ToString());
        }
    }
}