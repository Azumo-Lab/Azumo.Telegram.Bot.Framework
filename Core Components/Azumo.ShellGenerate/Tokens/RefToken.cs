namespace Azumo.ShellGenerate.Tokens
{
    internal class RefToken : TokenBase
    {
        private readonly string Name;
        private readonly TokenBase Value;

        private bool Definition;

        private TokenBase? ParamToken;

        public override string Generate()
        {
            if (!Definition)
            {
                Definition = true;
                return $"${Name}=${{{Value.Generate()}}}";
            }
            else
                return $"${Name} {ParamToken?.Generate() ?? string.Empty}";
        }

        public override TokenBase Param(TokenBase token)
        {
            ParamToken = token;
            return this;
        }

        public RefToken(TokenBase refToken, string name)
        {
            Name = name;
            Value = refToken;
        }
    }
}
