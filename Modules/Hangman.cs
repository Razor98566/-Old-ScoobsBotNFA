using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Discord.Commands;

namespace Scoobs_Bot_NFA.Modules
{
    public class Hangman : ModuleBase<SocketCommandContext>
    {
        //Globals
        public static string Word = "";
        public static List<string> Placeholder = new List<string>();
        public static List<string> Guesses = new List<string>();
        public static int Tries;
        public static int WrongGuess;

        [Command("hangmanstart")]
        [Summary("Starts a round of hangman. Try to guess the word taken from a huge list of words!")]
        public async Task HangmanGame()
        {
            //Clearing globals
            Word = "";
            Placeholder.Clear();
            Guesses.Clear();
            Tries = 0;
            WrongGuess = 0;
            
            //Initial test messages, read the wordlist, random word assingnment and building the placeholder
            await ReplyAsync(
                "A round of hangman has started!\nMake your guess with the 'hangmanguess <word or letter>' command.\nUse the 'hangmancheck' command to see your progress.\nIf you really can't guess the word just use 'hangmanreveal' command to reveal the word and end the round\nUse the 'hangmanstart' command again to re-roll!\nYou can make 20 wrong guesses, 21 means you've lost. :flushed:");
            var words = File.ReadAllLines(@"External Sources/words.txt");
            var rand = new Random();
            Word = words[rand.Next(1, 370104)];

            var guess = new string[Word.Length];
            
            for (var i = 0; i < Word.Length; i++) Placeholder.Add("-");
            var output = "";
            await ReplyAsync("The word contains " + guess.Length + " letters");
            foreach (var s in Placeholder) output += s;

            await ReplyAsync(output);
        }

        [Command("hangmanguess")]
        [Summary("Part of the hangman game, used to make a guess.")]
        public async Task Guess(string a)
        {
            if (Word == "")
            {
                await ReplyAsync("You can't guess if there's no word to guess! :flushed:");
                return;
            }
            
            //Icrement tries and add current guess to global list, check if the guess is a single letter or word
            Tries += 1;
            Guesses.Add(a + " ");
            if (a.Length == 1)
            {
                //foundIndexes init, checking if a is equal to the Word we're looking for
                var foundIndexes = 0;
                for (var i = 0; i < Word.Length; i++)
                    if (a == Word[i].ToString())
                    {
                        foundIndexes += 1;
                        Placeholder[i] = a;
                    }
                
                if (foundIndexes == 0)
                {
                    WrongGuess += 1;
                    await ReplyAsync("You guessed poorly!");
                    await ChangeHangman();
                }
                else
                {
                    await ReplyAsync("Good guess, '" + a + "' was correct!");
                    var output = "";
                    foreach (var s in Placeholder) output += s;

                    var comp = string.Join(null, Placeholder);
                    if (comp.Equals(Word))
                    {
                        await HangmanCheck();
                    }
                    else
                    {
                        await ReplyAsync("New Placeholder: " + output);
                        await ChangeHangman();
                    }
                }
            }

            if (a.Length > 1)
            {
                if (a == Word)
                {
                    //If the word was guessed correctly the game is being terminated, gloals flushed
                    await ReplyAsync("Word guessed correctly! The word was: " + Word +
                                     "\nGood job! Terminating game session...");
                    Word = "";
                    Placeholder.Clear();
                    Tries = 0;
                    Guesses.Clear();
                }
                else
                {
                    WrongGuess += 1;
                    await ReplyAsync("You guessed poorly!\nBetter luck next time king :wine:");
                }
            }
        }

        [Command("hangmancheck")]
        [Summary("Part of the hangman game. Used to check the current placeholder.")]
        public async Task HangmanCheck()
        {
            if (Word == "")
            {
                await ReplyAsync("You can't check if there's no word to check! :flushed:");
                return;
            }
            
            //If the placeholder is equal to Word the game is being terminated and the winner is being informed
            var comp = string.Join(null, Placeholder);
            if (comp == Word)
            {
                var outputGuesses = "";
                foreach (var s in Guesses) outputGuesses += s;
                await ReplyAsync("The word '" + Word + "' was correct!\nAmount of tries: " + Tries +
                                 "\nAmount of wrong guesses: " + WrongGuess + "\nGuesses: " + outputGuesses +
                                 "\nGood job!\nErasing the word now, making room for a new round");
                await ChangeHangman();
                Word = "";
                Placeholder.Clear();
                Tries = 0;
                WrongGuess = 0;
                Guesses.Clear();
            }
            else
            {
                //If the current placeholder is not equal to Word the game continues
                var outputGuess = "";
                var outputGuesses = "";
                foreach (var s in Placeholder) outputGuess += s;
                foreach (var s in Guesses) outputGuesses += s;
                await ReplyAsync("Current placeholder: " + outputGuess + "\nCurrent guesses: " + outputGuesses +
                                 "\nAmount of tries: " + Tries + "\nAmount of wrong guesses: " + WrongGuess);
                await ChangeHangman();
                await ReplyAsync("\nKeep trying!");
            }
        }

        [Command("hangmanreveal")]
        [Summary("Part of the hangman game. Used as a final measure, reveals the word and end the game.")]
        public async Task HangmanReveal()
        {
            if (Word == "")
            {
                await ReplyAsync("You can't reveal something that doesn't exist! :flushed:");
                return;
            }
            
            var a = Word;
            await ReplyAsync("So you have given up it seems... HA!\n The word was: " + a +
                             "\nErasing the word now, making room for a new round");
            Word = "";
            Placeholder.Clear();
            Guesses.Clear();
            Tries = 0;
            WrongGuess = 0;
        }

        [Command("hangmantries")]
        [Summary("Part of the hangman game. Returns the amount of guesses made so far.")]
        public async Task HangmanTries()
        {
            if (Word == "")
            {
                await ReplyAsync("I can't show you your tries if you haven't even tried yet! :flushed:");
                return;
            }

            await ChangeHangman();
            var output = "";
            foreach (var s in Guesses) output += s;
            await ReplyAsync("Amount of tries so far: " + output + "\nGuesses so far: " + Tries +
                             "\nAmount of wrong guesses so far:" + WrongGuess);
        }

        public async Task ChangeHangman()
        {
            //Changes the hangman output according to the tries so dont so far, ends the game at the last possible try
            string message;
            switch (WrongGuess)
            {
                case 1:
                    message = "=====================\n#/\n=====================\n";
                    await ReplyAsync(message);
                    break;
                case 2:
                    message = "=====================\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 3:
                    message = "=====================\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 4:
                    message = "=====================\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 5:
                    message = "=====================\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 6:
                    message = "=====================\n#  |\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 7:
                    message = "=====================\n#  |\n#  |\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 8:
                    message = "=====================\n#  |\n#  |\n#  |\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 9:
                    message =
                        "=====================\n#  |\n#  |\n#  |\n#  |\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 10:
                    message =
                        "=====================\n#  |-\n#  |\n#  |\n#  |\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 11:
                    message =
                        "=====================\n#  |--\n#  |\n#  |\n#  |\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 12:
                    message =
                        "=====================\n#  |---\n#  |\n#  |\n#  |\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 13:
                    message =
                        "=====================\n#  |----\n#  |\n#  |\n#  |\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 14:
                    message =
                        "=====================\n#  |-----\n#  |\n#  |\n#  |\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 15:
                    message =
                        "=====================\n#  |-----\\\n#  |\n#  |\n#  |\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 16:
                    message =
                        "=====================\n#  |-----\\\n#  |           |\n#  |\n#  |\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 17:
                    message =
                        "=====================\n#  |-----\\\n#  |           |\n#  |         :grinning:\n#  |\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 18:
                    message =
                        "=====================\n#  |-----\\\n#  |           |\n#  |         :grin:\n#  |         :martial_arts_uniform:\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 19:
                    message =
                        "=====================\n#  |-----\\\n#  |           |\n#  |         :sweat_smile:\n#  |         :martial_arts_uniform:\n#  |\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 20:
                    message =
                        "=====================\n#  |-----\\\n#  |           |\n#  |         :face_with_raised_eyebrow:\n#  |         :martial_arts_uniform:\n#  |      :leg:\n#  |\n#  |\n#/\\\n=====================";
                    await ReplyAsync(message);
                    break;
                case 21:
                    message =
                        "=====================\n#  |-----\\\n#  |           |\n#  |         :flushed:\n#  |         :martial_arts_uniform:\n#  |      :leg::leg:\n#  |\n#  |\n#/\\\n=====================\nYep, looks like you lost!\n This was the word I was looking for: " +
                        Word + "\n Erasing the word now, making room for a new round";
                    await ReplyAsync(message);
                    Word = "";
                    Placeholder.Clear();
                    Tries = 0;
                    Guesses.Clear();
                    break;
            }
        }
    }
}