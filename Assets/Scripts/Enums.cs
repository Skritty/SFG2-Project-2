[System.Flags]
public enum ColorType
{
    White = (1 << 0),
    Red = (1 << 1),
    Green = (1 << 2),
    Blue = (1 << 3),
    Yellow = (1 << 4),
    Magenta = (1 << 5),
    Cyan = (1 << 6)
}
public enum DeckPosition { Top, Middle, Bottom}
[System.Flags]
public enum SelectUIElements
{
    Connect = (1<<0),
    Rotate = (1 << 1),
    Toggle = (1 << 2),
    Move = (1 << 3),
    Target = (1 << 4),
    Activate = (1 << 5),
    TargetActivate = (1 << 6)
}