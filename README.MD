# Описание игры

Игра представляет собой консольное приложение на языке C#, в котором пользователю предлагается угадать случайное 4-значное число, сгенерированное программой. Игроку необходимо ввести четырехзначное число, содержащее цифры от 1 до 9. После каждой попытки программа сообщает, сколько цифр совпали по значению и позиции (равно) и сколько цифр совпали, но находятся на другой позиции (совпало). Также в игре предусмотрена система авторизации игроков и ведется учет всех попыток.

## Вход или регистрация игрока

В цикле программа запрашивает у пользователя имя. Если пользователь уже зарегистрирован, ему предлагается ввести пароль, который проверяется. В случае нового игрока предлагается ввести пароль для регистрации.

```csharp
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
            break; // завершение проверки после успешного входа
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
        break; // регистрация нового игрока
    }
}
```

## Цикл игры

Попытки: Счетчик attempts увеличивается с каждой попыткой игрока угадать число.

```csharp
int attempts = 0;
```
## Проверка ввода: Ввод игрока проверяется с помощью метода Unique, чтобы убедиться, что это четырехзначное число без повторяющихся цифр от 1 до 9.

```csharp
string st2;
do
{
    Console.WriteLine("Введите четырехзначное число, используя числа от 1 до 9, не повторяясь:");
    st2 = Console.ReadLine();
} while (!Unique(st2)); // проверка на уникальность и корректность ввода
```

## Проверка введенного числа
Метод Unique проверяет, что введенное число состоит из 4 цифр.

```csharp
static bool Unique(string st)
{
    if ((st.Length == 4) && int.TryParse(st, out int r))
    {
        return true;
    }
    else
    {
        return false;
    }
}
```

## Сравнение чисел
Проверяются полные совпадения (значение и позиция) и частичные совпадения (цифра на другой позиции).

```csharp
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
```

## Условие победы
Когда количество полных совпадений equal достигает 4, игрок выигрывает. Статистика сохраняется с помощью методов AddPlayer и ShowReport.

```csharp
    if (equal == 4)
    {
        game.AddPlayer(playerName, attempts, playerPas);
        game.ShowReport(playerName, attempts);
        break;
    }
```

## Добавление игрока

Метод AddPlayer сохраняет данные игрока (имя, количество попыток и пароль) для отслеживания его успехов.

```csharp
public void AddPlayer(string playerName, int attempts, string password)
{
    players.Add(new Player(playerName, attempts, password));
    AllGames++;
    AllTries += attempts;
    WriteFile(); // сохранение данных игрока в файл
}
```
## Повтор игры

Метод ShowReport выводит отчет по игре и сохраняет его в файл для последующего просмотра.

```csharp
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
```

g