using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Scoobs_Bot_NFA.Modules
{
    public class WedgesAndGBP : ModuleBase<SocketCommandContext>
    {
        public static int Wedges;
        public static int Gbp;
        
        [Command("wedge")]
        [Summary("Give Scoobs some wedges, he deserves them!")]
        public async Task GiveWedge(int amount = 0)
        {
            if (amount == 0)
            {
                await ReplyAsync("Thanks for nothing, rude stranger! :angry:");
                return;
            }

            if (amount >= 1)
            {
                Wedges += amount;
                await ReplyAsync("Thanks for the Wedges kind stranger! :hugging:");
                return;
            }
            //It's not always true tho
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (amount < 0)
            {
                Wedges += amount;
                await ReplyAsync("Take away one more Wedge and I'll get Tyler 1 to smash you kneecaps! :angry:");
            }
        }

        [Command("gbp")]
        [Summary("Our good boy deserves some good boy points, he needs some tendies you know?")]
        public async Task GoodBoyPoints(int amount = 0)
        {
            if (amount == 0)
            {
                await ReplyAsync(
                    "Why tease me with Good Boy Points???\n Give me GBP and tendies now you dirty whore REEEEEEEEE!");
                return;
            }

            if (amount >= 1)
            {
                Gbp += amount;
                await ReplyAsync(
                    "I'm very grateful for this generous gift kind stranger, I shall invest in yummy tendies and mountain dew ^-^");
                return;
            }
            //It's not always true tho
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (amount < 0)
                await ReplyAsync(
                    "No one takes await my Good Boy Points unpunished!\n *Starts charging towards you while unzipping pants and activating the shit spinner move*\n PAY FOR YOU CRIMES REEEEEEE\n *Takes back GBP like the good boy he is, no GBP has been decremented*");
        }

        [Command("wedgeamount")]
        [Summary("How many wedges has Scoobs received? Let's find it out!")]
        public async Task WedgeAmount()
        {
            var output = Wedges;
            if (Wedges > 0)
            {
                await ReplyAsync("Scoobs got " + output + " Wedges!\n Good boy!\n *schlop schlop schlop schlop*");
                await ReplyAsync(
                    "https://cdn.discordapp.com/attachments/583693598751719436/718381230688829440/scoobwedge.png");
            }

            if (Wedges == 0) await ReplyAsync("Sadly Scoobs has not aquired any Wedges, he'll starve soon :sob:");

            if (Wedges < 0)
                await ReplyAsync(
                    "Scoobs is owning Wedges to this server, soon he'll seek refuge in Albania. He's not going to pay... :flag_al:");
        }

        [Command("gbpamount")]
        [Summary("How much tendies and Mountain Dew can our good boy buy? Let's find it out!")]
        public async Task GbpAmount()
        {
            var output = Gbp;
            if (Gbp > 0)
            {
                await ReplyAsync("Our good boy has gathered " + output + " GBP, spend it on some tendies!");
                await ReplyAsync(
                    "https://cdn.discordapp.com/attachments/583693598751719436/718382336579993610/tendies.jpg");
            }
            else
            {
                await ReplyAsync("No Good Boy Points found, fuck you chad! :angry:");
            }
        }
        private async Task MessageReceived(SocketMessage message)
        {
            if (message != null && message.Author.IsBot) return;
            
            var mes = message.ToString().ToLower();
            if (mes.Contains("!wedge")) return;
            if (mes.Contains("wedges") || mes.Contains("wedge") || 
                mes.Contains("kartoffelprodukt") || mes.Contains("kartoffelprodukte"))
            {
                if (mes.Contains("'wedge ") || mes.Contains("'wedgeamount")) return;
                await message.Channel.SendMessageAsync("Give Wedge or say goodbye to your kneecaps :angry:");
            }
        }
    }
}