namespace Checkers;

public class IncorrectDataException : Exception
{
    public IncorrectDataException(string message = "Неправильный формат вводимых данных!") : base(message)
    { }
}
