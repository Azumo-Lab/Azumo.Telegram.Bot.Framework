namespace Azumo.Utils.Script
{
    public class StringCommand
    {
        public static implicit operator StringCommand(string s) => new();

        public static StringCommand operator |(string a, StringCommand b) => new();

        public static StringCommand operator &(StringCommand a, StringCommand b) => null!;
    }
}
