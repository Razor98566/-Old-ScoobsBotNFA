using System.Threading.Tasks;
using BooruSharp.Booru;
using Discord.Commands;

namespace Scoobs_Bot_NFA.Modules
{
    public class Weebs : ModuleBase<SocketCommandContext>
    {
        public static int Naughty;
        [RequireNsfw]
        [Command("weeb")]
        [Summary("Returns a random anime picture")]
        public async Task Weeb(string args = "")
        {
            Naughty += 1;
            var booru = new Yandere();
            BooruSharp.Search.Post.SearchResult result;
            if (args == "")
            {
                result = await booru.GetRandomImageAsync(args);
            }
            else
            {
                result = await booru.GetRandomImageAsync();
            }
                
            await ReplyAsync("Image: " + result.fileUrl.AbsoluteUri);
        }
        [RequireNsfw]
        [Command("weebspam")]
        [Summary("Returns a given amount of anime pictures, if desired a tag can be given as an optional argument.")]
        public async Task WeebSpam(int amount, string args = "")
        {
            for (int i = 0; i < amount; i++)
            {
                await Weeb(args);
            }
        }
        
        [Command("weebamount")]
        [Summary("Shows the amount of anime pics posted by the bot.")]
        public async Task WeebAmount()
        {
            var a = Naughty;
            await ReplyAsync("Scoobs has posted " + a + " weeb pics! :flushed:");
        }
    }
}