using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Scoobs_Bot_NFA
{
    internal class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private string _prefix;
        private static void Main(string[] args)
        {
            new Program().RunBotAsync().GetAwaiter().GetResult();
        }

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            Console.Write("Please enter the bot's token: ");
            var token = Console.ReadLine();
            Environment.SetEnvironmentVariable("token", token);

            Console.Write("Please enter the command prefix: ");
            _prefix = Console.ReadLine();

            _client.Log += _client_Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));

            await _client.StartAsync();

            _client.MessageReceived += MessageReceived;

            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message != null && message.Author.IsBot) return;

            var argPos = 0;
            if (message.HasStringPrefix(_prefix, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }

        private async Task MessageReceived(SocketMessage message)
        {
            var mes = message.ToString().ToLower();
            if (mes.Contains("wedges") || mes.Contains("wedge") || 
                mes.Contains("kartoffelprodukt") || mes.Contains("kartoffelprodukte"))
            {
                if (mes.Contains("'wedge ") || mes.Contains("'wedgeamount")) return;
                await message.Channel.SendMessageAsync("Give Wedge or say goodbye to your kneecaps :angry:");
            }
        }
    }
}