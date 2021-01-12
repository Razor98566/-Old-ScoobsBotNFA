using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;

namespace Scoobs_Bot_NFA.Modules
{
    public class Word : ModuleBase<SocketCommandContext>
    {
        [Command("word")]
        [Summary("Returns a random word with a fitting Wikipedia search")]
        public async Task RandWord()
        {
            var words = File.ReadAllLines(@"External Sources/words.txt");

            var rand = new Random();
            var word = words[rand.Next(1, 370104)];
            var message = "Here's your random word: " + word + "\nhttp://en.wikipedia.org/w/index.php/?search=" + word;
            await ReplyAsync(message);
        }
    }
}