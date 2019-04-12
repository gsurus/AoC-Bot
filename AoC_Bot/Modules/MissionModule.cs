using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace AoC_Bot.Modules
{
    public class MissionModule : ModuleBase<SocketCommandContext>
    {
        public static Color green = new Color(0x77b255);
        public static Color red = new Color(0xdd2e44);

        [Command("missions")]
        [Alias("jobs", "miss")]
        public async Task Missions()
        {
            for (int i = 0; i < File.ReadLines(@"data\missions.txt").Count(); i++)
            {
                string line = File.ReadLines(@"data\missions.txt").Skip(i).Take(1).First();
                string[] values = line.Split(' ');
                var emb = new EmbedBuilder();
                if (values[7] == "false")
                {
                    emb.WithColor(green);
                    emb.WithAuthor(author => {
                        author
                            .WithName("Open")
                            .WithIconUrl("https://i.imgur.com/j9bWncQ.png");
                    });
                }
                else
                {
                    emb.WithColor(red);
                    emb.WithAuthor(author => {
                        author
                            .WithName("Accepted")
                            .WithIconUrl("https://i.imgur.com/m9OP79S.png");

                    });
                }
                emb.WithDescription($"**Title** {values[0]} \n **Party:** {values[4]} \n **Objective:** {values[1]} \n **Reward:** {values[2]} \n **Ranking Points**: {values[3]}");
                emb.WithFooter(footer => {
                    footer
                        .WithText($"issued by {values[5]} id {values[6]}");
                });
                var embed = emb.Build();
                await ReplyAsync("", embed: embed);
            }
        }

        [Command("missions")]
        [Alias("jobs", "miss")]
        public async Task Missions(params string[] args)
        {
            int x = 0;
            for (int i = 0; i < File.ReadLines(@"data\missions.txt").Count(); i++)
            {
                string line = File.ReadLines(@"data\missions.txt").Skip(i).Take(1).First();
                string[] values = line.Split(' ');
                var emb = new EmbedBuilder();

                if (args[0] == "open" && values[7] == "false")
                {
                    x = 1;
                    emb.WithColor(green);
                    emb.WithAuthor(author => {
                        author
                            .WithName("Open")
                            .WithIconUrl("https://i.imgur.com/j9bWncQ.png");
                    });
                    emb.WithDescription($"**Title** {values[0]} \n **Party:** {values[4]} \n **Objective:** {values[1]} \n **Reward:** {values[2]} \n **Ranking Points**: {values[3]}");
                    emb.WithFooter(footer => {
                        footer
                            .WithText($"issued by {values[5]} id {values[6]}");
                    });
                    var embed = emb.Build();
                    await ReplyAsync("", embed: embed);
                    
                }
                if (args[0] == "accepted" && values[7] == "true")
                {
                    x = 1;
                    emb.WithColor(red);
                    emb.WithAuthor(author => {
                        author
                            .WithName("Accepted")
                            .WithIconUrl("https://i.imgur.com/m9OP79S.png");

                    });
                    emb.WithDescription($"**Title** {values[0]} \n **Party:** {values[4]} \n **Objective:** {values[1]} \n **Reward:** {values[2]} \n **Ranking Points**: {values[3]}");
                    emb.WithFooter(footer => {
                        footer
                            .WithText($"issued by {values[5]} id {values[6]}");
                    });
                    var embed = emb.Build();
                    await ReplyAsync("", embed: embed);
                }

                Console.WriteLine(x);
            }
            if (x == 0)
            {
                Console.WriteLine("SanityCheck");
                EmbedBuilder _emb = new EmbedBuilder();
                _emb.WithDescription(":x: No Missions Found");
                _emb.WithColor(red);
                Embed _embed = _emb.Build();
                await ReplyAsync(embed: _embed);
            }
        }

        [Command("mission.accept"), Alias("macc"), Summary("Accept a mission either by name or ID")]
        public async Task AcceptMission(int args)
        {
            var linesRead = File.ReadLines(@"data\missions.txt");
            string line = null;
            List<string> lines = new List<string>();
            foreach (var lineRead in linesRead)
            {
                lines.Add(lineRead);
            }
            int x = 0;
            for (int i = 0; i < lines.Count(); i++)
            {
                line = lines[i];
                if (Int32.Parse(line.Split(' ').ElementAt(6)) == args && line.Split(' ').Last() == "true")
                {
                    Console.WriteLine(Int32.Parse(line.Split(' ').ElementAt(6)));
                    Console.WriteLine(line.Split(' ').Last());
                    EmbedBuilder _emb = new EmbedBuilder();
                    _emb.WithDescription(":x: Mission Already Accepted");
                    _emb.WithColor(red);
                    Embed _embed = _emb.Build();
                    await ReplyAsync(embed: _embed);
                    return;
                }
                if (Int32.Parse(line.Split(' ').ElementAt(6)) == args && line.Split(' ').Last() == "false")
                {
                    
                    x = i;
                    line = line.Replace("false", "true");
                    LineChanger(line, @"data\missions.txt", i + 1);
                    EmbedBuilder _emb = new EmbedBuilder();
                    _emb.WithDescription(":white_check_mark: Mission Accepted");
                    _emb.WithColor(green);
                    Embed _embed = _emb.Build();
                    await ReplyAsync(embed: _embed);
                    return;
                }
            }
            EmbedBuilder emb = new EmbedBuilder();
            emb.WithDescription(":x: Mission not Found");
            emb.WithColor(red);
            Embed embed = emb.Build();
            await ReplyAsync(embed: embed);
            return;
        }

        [Command("mission.accept"), Alias("macc"), Summary("Accept a mission either by title or ID")]
        public async Task AcceptMission(string args)
        {
            var linesRead = File.ReadLines(@"data\missions.txt");
            string line = null;
            List<string> lines = new List<string>();
            foreach (var lineRead in linesRead)
            {
                lines.Add(lineRead);
            }
            int x = 0;
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Split(' ').FirstOrDefault() == args)
                {
                    line = lines[i];
                    x = i;
                    if(lines[i].Split(' ').Last() == "true")
                    {
                        EmbedBuilder _emb = new EmbedBuilder();
                        _emb.WithDescription(":x: Mission Already Accepted");
                        _emb.WithColor(red);
                        Embed _embed = _emb.Build();
                        await ReplyAsync(embed: _embed);
                        return;
                    }
                }
            }
            if (line == null)
            {
                EmbedBuilder _emb = new EmbedBuilder();
                _emb.WithDescription(":x: Mission not Found");
                _emb.WithColor(red);
                Embed _embed = _emb.Build();
                await ReplyAsync(embed: _embed);
                return;
            }
            else
            {
                line = line.Replace("false", "true");
                LineChanger(line, @"data\missions.txt", x + 1);
                EmbedBuilder emb = new EmbedBuilder();
                emb.WithDescription(":white_check_mark: Mission Accepted");
                emb.WithColor(green);
                Embed embed = emb.Build();
                await ReplyAsync(embed: embed);
            }
            //line = File.ReadLines(@"data\missions.txt").Skip(args).Take(1).First();
        }

        static void LineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
    }

    [Permissions.RequireRole("admin" /*"Inner Circle"*/)]
    public class MissionAdministration : ModuleBase<SocketCommandContext>
    {
        int missionNumber = File.ReadLines(@"data\missions.txt").Count();
        [Command("missions.add")]
        [Alias("madd"), Summary("Command to add new missions. Inputs: Mission name, Mission Requirements, currency_value, rankpoint_reward, is_solo_quest")]
        public async Task AddMission([Remainder] string str)
        {
            string[] values = str.Split(' ');
            if (values.Length < 5)
            {
                await ReplyAsync("Error: Please input correct number of variables");
                return;
            }
            if (MissionValidation(@"data\missions.txt", values[0]) == false)
            {
                EmbedBuilder _emb = new EmbedBuilder();
                _emb.WithDescription(":x: Mission Name Taken");
                _emb.WithColor(MissionModule.red);
                Embed _embed = _emb.Build();
                await ReplyAsync(embed: _embed);
                return;
            }
            using (StreamWriter writer = new StreamWriter(@"data\missions.txt", true))
            {
                writer.WriteLine($"{values[0]} {values[1]} {values[2]} {values[3]} {values[4]} {Context.User.Username} {missionNumber} false");
                writer.Dispose();
                writer.Close();
            }

            EmbedBuilder emb = new EmbedBuilder();
            emb.WithDescription(":white_check_mark: Mission Added");
            emb.WithColor(MissionModule.green);
            Embed embed = emb.Build();
            await ReplyAsync(embed: embed);
        }

        [Command("missions.remove")]
        [Alias("mrmv"), Summary("Remove a mission by Title or ID")]
        public async Task RemoveMission(string args)
        {

            var linesRead = File.ReadLines(@"data\missions.txt");
            string line = null;
            List<string> lines = new List<string>();
            foreach (var lineRead in linesRead)
            {
                lines.Add(lineRead);
            }
            int x = 0;
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Split(' ').FirstOrDefault() == args)
                {
                    line = lines[i];
                    x = i;
                }
            }
            if (line == null)
            {
                EmbedBuilder _emb = new EmbedBuilder();
                _emb.WithDescription(":x: Mission not Found");
                _emb.WithColor(MissionModule.red);
                Embed _embed = _emb.Build();
                await ReplyAsync(embed: _embed);
                return;
            }
            else
            {
                EmbedBuilder emb = new EmbedBuilder();
                emb.WithDescription(":white_check_mark: Mission Removed");
                emb.WithColor(MissionModule.green);
                Embed embed = emb.Build();
                await ReplyAsync(embed: embed);
                LineChanger(@"data\missions.txt", x);
            }
            
        }

        [Command("missions.remove")]
        [Alias("mrmv"), Summary("Remove a mission by Title or ID")]
        public async Task RemoveMission(int args)
        {
            Console.WriteLine("int");
            var linesRead = File.ReadLines(@"data\missions.txt");
            string line = null;
            List<string> lines = new List<string>();
            foreach (var lineRead in linesRead)
            {
                lines.Add(lineRead);
            }
            for (int i = 0; i < lines.Count(); i++)
            {
                line = lines[i];
                int x = Int32.Parse(line.Split(' ').ElementAt(6));
                if (x == args)
                {
                    LineChanger(@"data\missions.txt", i);
                    EmbedBuilder _emb = new EmbedBuilder();
                    _emb.WithDescription(":white_check_mark: Mission Removed");
                    _emb.WithColor(MissionModule.green);
                    Embed _embed = _emb.Build();
                    await ReplyAsync(embed: _embed);
                    return;
                }
            }
            EmbedBuilder emb = new EmbedBuilder();
            emb.WithDescription(":x: Mission not Found");
            emb.WithColor(MissionModule.red);
            Embed embed = emb.Build();
            await ReplyAsync(embed: embed);
            return;

            /*
            if (File.ReadLines(@"data\missions.txt").Skip(args).Take(1).First() == null)
            {
                EmbedBuilder _emb = new EmbedBuilder();
                _emb.WithDescription(":x: Mission not Found");
                _emb.WithColor(MissionModule.red);
                Embed _embed = _emb.Build();
                await ReplyAsync(embed: _embed);
                return;
            }
            LineChanger(@"data\missions.txt", args);

            EmbedBuilder emb = new EmbedBuilder();
            emb.WithDescription(":white_check_mark: Mission Removed");
            emb.WithColor(MissionModule.green);
            Embed embed = emb.Build();
            await ReplyAsync(embed: embed);
            */
        }

        [Command("missions.complete")]
        [Alias("mcmpl")]
        public async Task CompleteMission(int args)
        {
            await ReplyAsync("In development");
        }

        [Command("missions.complete")]
        [Alias("mcmpl")]
        public async Task CompleteMission(string args)
        {
            await ReplyAsync("In development");
        }

        static void LineChanger(string fileName, int line_to_edit)
        {
            List<string> linesList = File.ReadAllLines(fileName).ToList();
            linesList.RemoveAt(line_to_edit);
            File.WriteAllLines(fileName, linesList.ToArray());
        }

        public static bool MissionValidation(string file, string args)
        {
            var linesRead = File.ReadLines(file);
            string line = null;
            int x = 0;

            List<string> lines = new List<string>();
            foreach (var lineRead in linesRead)
            {
                lines.Add(lineRead);
            }
            
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Split(' ').FirstOrDefault() == args)
                {
                    return false;
                }
            }
            return true;
        }
    }
}



//emb.AddField($"Mission Name", values[0], true);
//emb.AddField($"Requirements", values[1], true);
//emb.AddField($"Reward", values[2], true);
//emb.AddField($"Ranking Points", values[3], true);
//emb.AddField($"Party Requirement", values[4], true);