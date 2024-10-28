using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingGame
{
    public class Game
    {
        public int AllGames { get;set; } = 0;
        public int AllTries { get;set; } = 0;
        public static List<Player> players = new List<Player>();
        public void AddPlayer(string playerName, int attempts,string password)
        {
            players.Add(new Player(playerName, attempts,password));
            AllGames++;
            AllTries += attempts;
            WriteFile();
        }

        public void ShowReport(string playerName, int attempts)
        {
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var attemptsGr = players.GroupBy(p => p.Attempts).ToDictionary(g => g.Key, g => g.Count());

            int betterPlayers = attemptsGr.Where(entry => entry.Key < attempts).Sum(entry => entry.Value);

            int position = betterPlayers + 1;


            Console.WriteLine($"Игрок {playerName} угадал число за {attempts} попыток.");
            Console.WriteLine($"Игрок {playerName} занимает {position} место из {players.Count} игроков.");

            using (StreamWriter streamWriter = new StreamWriter("gameRounds.txt", true))
            {
                streamWriter.WriteLine($"[{currentTime}] Игрок {playerName} угадал число за {attempts} попыток.");
                streamWriter.WriteLine($"Игрок {playerName} занимает {position} место из {players.Count} игроков.");
                streamWriter.WriteLine(); 
            }
        }

        public void Load()
        {
            if (File.Exists("game.txt"))
            {
                players = new List<Player>();
                var lines = File.ReadAllLines("game.txt");
                AllGames = int.Parse(lines[0].Split(": ")[1]);
                AllTries = int.Parse(lines[1].Split(": ")[1]);
                players = new List<Player>();
                foreach (var line in lines.Skip(2))
                {
                    var parts = line.Split('|');
                    string playerName = parts[0].Trim();
                    int attempts = int.Parse(parts[1].Trim());
                    string playerPas = parts[2].Trim();
                    players.Add(new Player(playerName, attempts, playerPas));
                }
            }
        }
        
        private void WriteFile()
        {
            using (StreamWriter writer = new StreamWriter("game.txt"))
            {
                writer.WriteLine($"Всего игр: {AllGames}");
                writer.WriteLine($"Всего попыток: {AllTries}");
                foreach (var player in players)
                {
                    writer.WriteLine($"{player.Name} | {player.Attempts} | {player.Password}");
                }
            }
        }
    }
}
