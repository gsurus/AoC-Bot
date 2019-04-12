using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace AoC_Bot.Modules
{
    public class DirectoryModule : ModuleBase<SocketCommandContext>
    {
        [Command("directory"), Alias("dir")]
        public async Task Info()
        {
            var emb = new EmbedBuilder()
            .WithColor(new Color(0x7D0000))
            .WithThumbnailUrl("https://i.imgur.com/pqlWhne.png")
            .WithAuthor(author => 
            {
                author
                .WithName("Directory")
                .WithIconUrl("https://i.imgur.com/8gV0VUf.png");
            })
            .AddField("Marketplace", "Placeholder")
            .AddField("Missions", "Placeholder")
            .AddField("Rankings", "Placeholder");
            var embed = emb.Build();
            //Context.Client.CurrentUser.Username
            await ReplyAsync(embed: embed, message: $"");
        }
    }
}
