using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestingGame
{
    public class Player
    {
        public string Name { get; set; }
        public int Attempts { get; set; }
        public string Password { get; set; }

        public Player(string name, int attempts,string password)
        {
            Name = name;
            Attempts = attempts;
            Password = password;
        }
    }
}
