namespace Depot.Services
{
    using Discord;
    using Discord.WebSocket;
    using Octokit;
    using System;
    using System.Buffers.Binary;
    using System.Diagnostics;
    using System.IO.Compression;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class UpdateService
    {
        private const string Owner = "anthonysSlab";
        private const string Name = "depot";
        private readonly DiscordSocketClient client;

        public UpdateService(DiscordSocketClient client)
        {
            client.Ready += Ready;
            this.client = client;
        }

        private async Task Ready()
        {
            if (!File.Exists("UPDATED"))
            {
                return;
            }

            int result = int.Parse(File.ReadAllText("UPDATED"));
            File.Delete("UPDATED");

            if (!File.Exists("UPDATE"))
            {
                return;
            }

            byte[] buffer = File.ReadAllBytes("UPDATE");
            ulong channelId = BinaryPrimitives.ReadUInt64LittleEndian(buffer);
            File.Delete("UPDATE");

            if (File.Exists("update.log"))
            {
                string log = File.ReadAllText("update.log");
                File.Delete("update.log");

                ITextChannel channel = (ITextChannel)client.GetChannel(channelId);
                EmbedBuilder builder = new();

                if (result == 0)
                {
                    builder.AddField(new EmbedFieldBuilder() { Name = "Updater", Value = "The update was completed successfully" });
                }
                else
                {
                    StringBuilder sb = new();
                    sb.AppendLine("An error ocurred during the upgrade");
                    sb.AppendLine(log);
                    builder.AddField(new EmbedFieldBuilder() { Name = "Updater", Value = sb.ToString() });
                }

                await channel.SendMessageAsync(embed: builder.Build());
            }
            else
            {
                ITextChannel channel = (ITextChannel)client.GetChannel(channelId);
                EmbedBuilder builder = new();

                if (result == 0)
                {
                    builder.AddField(new EmbedFieldBuilder() { Name = "Updater", Value = "The update was completed successfully" });
                }
                else
                {
                    StringBuilder sb = new();
                    sb.AppendLine("An error ocurred during the upgrade");
                    sb.AppendLine("No log has been generated");
                    builder.AddField(new EmbedFieldBuilder() { Name = "Updater", Value = sb.ToString() });
                }

                await channel.SendMessageAsync(embed: builder.Build());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public void Restart()
        {
#nullable disable
            // Starts a new instance of the program itself
            Process.Start(Environment.ProcessPath);
#nullable enable

            // Closes the current process
            Environment.Exit(0);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public async Task<int> CheckVersionAsync()
        {
            GitHubClient client = new(new ProductHeaderValue("GodOfUwULauncher"));

            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll(Owner, Name);

            //Setup the versions
            Version latestGitHubVersion = new(releases[0].TagName);

            Version localVersion = new(GetCurrentVersion());

            int versionComparison = localVersion.CompareTo(latestGitHubVersion);

            return versionComparison;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public async Task<string> GetLatestVersion()
        {
            GitHubClient client = new(new ProductHeaderValue("GodOfUwULauncher"));

            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll(Owner, Name);

            return releases[0].TagName;
        }

        public static string GetCurrentVersion()
        {
            return Assembly.GetExecutingAssembly().GetName()?.Version?.ToString() ?? string.Empty;
        }

        public async Task<string> GetChangelog()
        {
            GitHubClient client = new(new ProductHeaderValue("GodOfUwULauncher"));

            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll(Owner, Name);

            return releases[0].Body;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public async Task Update(ulong channelId)
        {
            GitHubClient client = new(new ProductHeaderValue("GodOfUwULauncher"));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll(Owner, Name);

            Version latestGitHubVersion = new(releases[0].TagName);

            Version localVersion = new(GetCurrentVersion());

            int versionComparison = localVersion.CompareTo(latestGitHubVersion);

            if (versionComparison >= 0)
            {
                return;
            }

            byte[] buffer = new byte[8];
            BinaryPrimitives.WriteUInt64LittleEndian(buffer, channelId);
            File.WriteAllBytes("UPDATE", buffer);

            const string dir = "tmp";

            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);

            string platformString = OperatingSystem.IsLinux() ? "linux" : "win";
            ReleaseAsset asset = releases[0].Assets.First(x => x.Name.Contains(platformString));
            HttpClient clientweb = new();
            Stream stream = clientweb.GetStreamAsync(asset.BrowserDownloadUrl).Result;
            Stream fs = File.Create("tmp.zip");
            stream.CopyTo(fs);
            fs.Flush();
            fs.Position = 0;
            stream.Close();
            ZipArchive archive = new(fs);
            archive.ExtractToDirectory(dir);
            fs.Close();
            File.Delete("tmp.zip");

            if (OperatingSystem.IsWindows())
            {
                Process.Start("cmd.exe", "/c " + @".\tmp\update.bat");
                Environment.Exit(0);
            }
            else if (OperatingSystem.IsLinux())
            {
                Process.Start("bash", "tmp/update.sh");
                Environment.Exit(0);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public void TestUpdateScript(ulong channelId)
        {
            byte[] buffer = new byte[8];
            BinaryPrimitives.WriteUInt64LittleEndian(buffer, channelId);
            File.WriteAllBytes("UPDATE", buffer);

            if (OperatingSystem.IsWindows())
            {
                Process.Start("cmd.exe", "/c " + @".\tmp\update.bat /a update.log");
                Environment.Exit(0);
            }
            else if (OperatingSystem.IsLinux())
            {
                Process.Start("bash", "tmp/update.sh");
                Environment.Exit(0);
            }
        }
    }
}