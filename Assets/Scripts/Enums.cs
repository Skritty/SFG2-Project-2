public enum ColorType { White, Red, Green, Blue, Yellow, Magenta, Cyan }
public enum DeckPosition { Top, Middle, Bottom}
[System.Flags]
public enum SelectUIElements
{
    Connect = (1<<0),
    Rotate = (1 << 1),
    Toggle = (1 << 2),
    Move = (1 << 3)
}