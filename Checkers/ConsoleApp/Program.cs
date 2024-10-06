using Checkers;

var a = new RussianDraughts();

Console.WriteLine( RussianDraughts.GetTheRules() );

Console.Write( a );

while (a.IsGameOn())
{
    a.NextMove();
    Console.Write(a);
}