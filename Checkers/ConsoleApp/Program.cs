using Checkers;

var a = new RussianDraughts();

a.StartGame(out Color whoWon);

Console.WriteLine( whoWon + " won");