using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace Scoobs_Bot_NFA.Modules
{
    // Modules must be public and inherit from an IModuleBase
    public class Roll : ModuleBase<SocketCommandContext>
    {
        //Random dice roll, amount of dices and sides must be specified
        [Command("roll")]
        [Summary("Simple dice roll, amount of dices and sides must be specified as parameters!")]
        public async Task DiceRoll(int numOfDice, int numOfSides)
        {
            var random = new Random();
            var message = "";

            for (var i = 0; i < numOfDice; i++)
            {
                var num = random.Next(0, numOfSides);
                message += "Dice: " + i + ", Value: " + num + "\n";
            }

            await ReplyAsync(message);
        }
    }
}