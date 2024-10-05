namespace Checkers;

enum Color
{
    None,
    White,
    Black,
}

enum Role
{
    None,
    Man,
    King,
}

readonly struct Figure
{
    public readonly Color Color;
    public readonly Role Role;

    public Figure(Color color, Role role)
    {
        Color = color;
        Role = role;
    }

    public static bool operator ==(Figure a, Figure b) => a.Color == b.Color && a.Role == b.Role;

    public static bool operator !=(Figure a, Figure b) => !(a == b);

    public override string ToString()
    {
        return ((int)Color).ToString() + ((int)Role).ToString();
    }
}