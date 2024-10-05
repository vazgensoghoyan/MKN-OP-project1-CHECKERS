using Checkers;

var a = new RussianDraughts();

Console.Write( a );

while (a.IsGameOn())
{
    a.NextMove();
    Console.Write(a);
}