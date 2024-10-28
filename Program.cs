using System;
namespace TestingGame
{
    public class Program
    {
        static Game game = new Game();
        static void Main(string[] args)
        {
            game.Load();
            while (true)
            {

                string numberOne = RndGenerate();
                Console.WriteLine(numberOne);
                string playerName;
                string playerPas;
                while (true)
                {
                    Console.WriteLine("Введите имя игрока: ");
                    playerName = Console.ReadLine();
                    var player = Game.players.Find(p => p.Name == playerName);
                    if (player != null)
                    {

                        Console.WriteLine("Введите пароль игрока: ");
                        playerPas = Console.ReadLine();
                        if (playerPas == player.Password)
                        {
                            Console.WriteLine("Вход выполнен");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неверный пароль.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Новый игрок.");
                        Console.WriteLine("Введите пароль игрока: ");
                        playerPas = Console.ReadLine();
                        break;
                    }
                    
                }
                int attempts = 0;

                while (true)
                {
                    attempts++;
                    string st2;
                    do
                    {
                        Console.WriteLine("Введите четырехзначное число, используя числа от 1 до 9, не повторяясь:");
                        st2 = Console.ReadLine();
                    } while (!Unique(st2));

                    char[] charsTaken = st2.ToCharArray();
                    char[] charsHad = numberOne.ToCharArray();
                    int equal = 0;
                    int coincidence = 0;

                    for (int i = 0; i < charsTaken.Length; i++)
                    {
                        if (charsTaken[i] == charsHad[i])
                        {
                            equal++;
                        }
                        if (numberOne.Contains(charsTaken[i]))
                        {
                            coincidence++;
                        }
                    }

                    Console.WriteLine("Равно = " + equal + "; Совпало = " + coincidence);

                    if (equal == 4)
                    {
                        game.AddPlayer(playerName, attempts,playerPas);
                        game.ShowReport(playerName, attempts);
                        break;
                    }
                }


                Console.WriteLine("Хотите сыграть еще раз? N - для выхода.");
                if (Console.ReadLine().ToLower() == "n")
                {
                    break;
                }
            }
        }
        static string RndGenerate()
        {
            Random random = new Random();
            List<int> list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            string st = "";
            for (int i = 0; i < 4; i++)
            {
                int n = list[random.Next(1, list.Count)];
                st = st + n;
                list.Remove(n);
            }
            return st;
        }

        static bool Unique(string st)
        {
            if ( (st.Length == 4) && int.TryParse(st,out int r) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}