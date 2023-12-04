namespace Azumo.ShellGenerate.Tokens
{
    public class Echo : TokenBase
    {
        private TokenBase? PrintText;

        public override string Generate()
        {
            return $"echo {PrintText?.Generate() ?? string.Empty}";
        }

        public override TokenBase Param(TokenBase token)
        {
            PrintText = token;
            return this;
        }
    }
}
