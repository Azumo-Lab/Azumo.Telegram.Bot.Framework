namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AuthenticateAttribute : Attribute
    {
        public HashSet<string> RoleName { get; }
        public AuthenticateAttribute(params string[] role)
        {
            RoleName = new HashSet<string>(role);
        }

        public AuthenticateAttribute()
        {

        }
    }
}
