using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace CrunchyBotNext.Commands
{
    [Summary("Help")]
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        public CommandService Commands { get; set; }

        [Summary("Gives you a list of all commands, or gives you information on a command if you specify the command.")]
        [Command("help")]
        public async Task Help(string? command = null)
        {
            List<ModuleInfo> mods = Commands.Modules.ToList();
            List<CommandInfo> cmds = Commands.Commands.ToList();
            EmbedBuilder embedBuilder = new EmbedBuilder();

            if (command != null)
            {
                CommandInfo? cmd = cmds.Find(c => c.Name == command);
                string pm = "";

                foreach (ParameterInfo pa in cmd?.Parameters) pm += 
                    $"{((pa.IsOptional) ? $"[" : "<")}{pa.Name}{((pa.IsOptional) ? $" = {pa.DefaultValue}" : "")}{((pa.IsOptional) ? $"]" : ">")}";

                string aliases = string.Join(',', cmd.Aliases);

                await ReplyAsync($"```<> = required parameter\n[] = optional parameter\n{cmd?.Name} {pm} - {cmd?.Summary}\n\naliases: {aliases}```");
            } else
            {
                foreach (ModuleInfo mod in mods)
                {
                    string c = "";

                    foreach (CommandInfo cmd in mod.Commands) c += $"`{cmd.Name}` ";

                    embedBuilder.AddField(mod.Summary, c);
                }

                embedBuilder.WithDescription("Run c-help <command> to get info on a command");

                await Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
            }
        }
    }
}
